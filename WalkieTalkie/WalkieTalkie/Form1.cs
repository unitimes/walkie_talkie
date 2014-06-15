using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;
using Voice;
using System.Net.NetworkInformation;


namespace WalkieTalkie
{
    public partial class Form1 : Form
    {
        private UdpClient udpVoice;                 //음성을 주고 받기 위한
        private UdpClient udpData;                  //데이터를 주고 받기 위한
        private FifoStream m_Fifo;                  
        private WaveOutPlayer wavePlayer;
        private WaveInRecorder waveRecorder;
        private bool voiceFlag = false;
        private IPEndPoint tempIP;
        private IPEndPoint targetDataIP;
        private IPEndPoint targetVoiceIP;

        private byte[] m_PlayBuffer;
        private byte[] m_RecBuffer;

        //스레드에서 gui 통제
        delegate void SetTextCallback(string text);
        delegate void SetBoolCallback(int val);

        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            SetBtn(0);
            m_Fifo = new FifoStream();
            //데이터 통신을 위한 udpclient 생성
            udpData = new UdpClient(3000);
            //데이터 비동기 수신 대기
            OnReceiveData();
        }
        private void InitUdpVoice()
        {
            //음성 통신을 위한 udpclient 생성
            udpVoice = new UdpClient(2000);
        }
        private void InitCall()
        {
            ShowStatus("연결 완료");
            SetBtn(1);
            //원격 연결점 생성
            targetDataIP = tempIP;
            targetVoiceIP = new IPEndPoint(tempIP.Address, 2000);

            WaveFormat fmt = new WaveFormat(44100, 16, 2);
            wavePlayer = new WaveOutPlayer(-1, fmt, 16384, 3, new BufferFillEventHandler(Filler));
            waveRecorder = new WaveInRecorder(-1, fmt, 16384, 3, new BufferDoneEventHandler(Voice_Out));
            //음성 비동기 수신 대기
            udpVoice.BeginReceive(new AsyncCallback(ReceiveVoiceCallback), udpVoice);
        }
        //가용 버튼 등 설정을 위한 메서드
        private void SetBtn(int val)
        {
            if (this.btn_cnt.InvokeRequired)
            {
                SetBoolCallback d = new SetBoolCallback(SetBtn);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                switch (val)
                {
                    case 0:
                        {
                            checkBox1.Enabled = false;
                            btn_dcnt.Enabled = false;
                            btn_cnt.Enabled = true;
                            checkBox1.Checked = false;
                            voiceFlag = false;
                            break;
                        }
                    case 1:
                        {
                            btn_cnt.Enabled = false;
                            btn_dcnt.Enabled = true;
                            checkBox1.Enabled = true;
                            break;
                        }
                    case 2:
                        {
                            checkBox1.Enabled = false;
                            voiceFlag = true;
                            break;
                        }
                    case 3:
                        {
                            checkBox1.Enabled = true;
                            voiceFlag = false;
                            break;
                        }
                }
            }
        }

        //라벨에 통신 상태를 표시
        private void ShowStatus(string msg)
        {
            if (this.lbl_status.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ShowStatus);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.lbl_status.Text = msg;
            }
        }

        //데이터 비동기 수신 대기
        private void OnReceiveData()
        {
            udpData.BeginReceive(new AsyncCallback(ReceiveDataCallback), udpData);
        }

        private void ReceiveDataCallback(IAsyncResult ar)
        {
            tempIP = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                Data receiveData = new Data(udpData.EndReceive(ar, ref tempIP));
                //데이터 cmd에 따른 분기
                switch (receiveData.Cmd)
                {
                    case Command.Invite:
                        {
                            string temp = lbl_status.Text;
                            ShowStatus("연결 요청 수신");
                            if (MessageBox.Show("통신 요청을 수락하시겠습니까?",
                                "통신 요청 알림",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.None) == DialogResult.Yes)
                            {
                                SendData(Command.OK, tempIP);
                                InitUdpVoice();
                                InitCall();
                            }
                            else
                            {
                                ShowStatus(temp);
                                SendData(Command.NO, tempIP);
                            }
                            break;
                        }
                    case Command.OK:
                        {
                            MessageBox.Show("통신 요청이 수락되었습니다.", "통신 연결 알림", MessageBoxButtons.OK);
                            InitCall();
                            break;
                        }
                    case Command.NO:
                        {
                            udpVoice.Close();
                            break;
                        }
                    case Command.SpeakingOn:
                        {
                            SetBtn(2);
                            break;
                        }
                    case Command.SpeakingOff:
                        {
                            SetBtn(3);
                            break;
                        }
                    case Command.Close:
                        {
                            Stop();
                            MessageBox.Show("상대방이 연결을 차단하였습니다.", "통신 차단 알림", MessageBoxButtons.OK);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                OnReceiveData();
            }
            catch (Exception e) { }
        }

        private void ReceiveVoiceCallback(IAsyncResult ar)
        {
            IPEndPoint tempIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] buf = new byte[16384];
            try
            {
                buf = udpVoice.EndReceive(ar, ref tempIP);
                if (!checkBox1.Checked && voiceFlag)
                    m_Fifo.Write(buf, 0, buf.Length);
                //음성 통신을 위해 지속적으로 대기
                udpVoice.BeginReceive(new AsyncCallback(ReceiveVoiceCallback), udpVoice);
            }
            catch (Exception e) { }
        }
        private void Filler(IntPtr data, int size)
        {
            if (m_PlayBuffer == null || m_PlayBuffer.Length < size)
                m_PlayBuffer = new byte[size];
            if (m_Fifo.Length >= size)
                m_Fifo.Read(m_PlayBuffer, 0, size);
            else
                for (int i = 0; i < m_PlayBuffer.Length; i++)
                    m_PlayBuffer[i] = 0;
            System.Runtime.InteropServices.Marshal.Copy(m_PlayBuffer, 0, data, size);
        }
        private void Voice_Out(IntPtr data, int size)
        {
            //버퍼가 size보다 작거나 버퍼가 초기화되지 않았을때 버퍼 생성
            if (m_RecBuffer == null || m_RecBuffer.Length < size)
                m_RecBuffer = new byte[size];

            if (checkBox1.Checked)
            {
                //관리되지 않는 데이터(마이크로 들어온 데이터)를 버퍼에 저장
                System.Runtime.InteropServices.Marshal.Copy(data, m_RecBuffer, 0, size);
            }
            int b_Size = m_RecBuffer.Length;
            try
            {
                udpVoice.BeginSend(m_RecBuffer, m_RecBuffer.Length, targetVoiceIP, new AsyncCallback(SendVoiceCallback), udpVoice);
            }
            catch (Exception e) { }
        }
        private void SendVoiceCallback(IAsyncResult ar)
        {
            udpVoice.EndSend(ar);
        }
        //스피킹 버튼 이벤트 처리
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Command cmd;
            if (checkBox1.Checked)
            {
                cmd = Command.SpeakingOn;
            }
            else
            {
                cmd = Command.SpeakingOff;
            }
            SendData(cmd, targetDataIP);
        }
        //connect 버튼 클릭시 이벤트
        private void btn_cnt_Click(object sender, EventArgs e)
        {
            ShowStatus("연결중");
            SendData(Command.Invite, new IPEndPoint(IPAddress.Parse(tBoxIP.Text), 3000));
            InitUdpVoice();
            btn_cnt.Enabled = false;
            btn_dcnt.Enabled = true;
        }
        //disconnect 버튼 클릭시 이벤트
        private void btn_dcnt_Click(object sender, EventArgs e)
        {
            if (targetDataIP != null)
            {
                SendData(Command.Close, targetDataIP);
            }
            else
            {
                Stop();
            }
        }
        //창을 닫을 때의 이벤트
        private void Form1_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SendData(Command.Close, targetDataIP);
        }
        //데이터 송신
        private void SendData(Command cmd, IPEndPoint receiverIp)
        {
            byte[] byteBuf;
            Data sendData;
            Command returnCmd = cmd;

            sendData = new Data();
            sendData.Cmd = cmd;
            byteBuf = new byte[4];
            byteBuf = sendData.ToByte();
            try
            {
                udpData.BeginSend(byteBuf, byteBuf.Length, receiverIp, new AsyncCallback(SendDataCallback), returnCmd);
            }
            catch (Exception e) { }
        }
        private void SendDataCallback(IAsyncResult ar)
        {
            Command temp = (Command)ar.AsyncState;
            udpData.EndSend(ar);
            if((Command)ar.AsyncState == Command.Close)
            {
                Stop();
            }
        }

        private void Stop()
        {
            udpData.Close();
            udpVoice.Close();
            BufferClear();
            tempIP = null;
            targetDataIP = null;
            targetVoiceIP = null;
            IPGlobalProperties prop = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ips = prop.GetActiveUdpListeners();
            bool flag = true;
            // 포트 반환을 기다림
            while (flag)
            {
                int check = 0;
                foreach (IPEndPoint ip in ips)
                {
                    if (ip.Port == 3000 || ip.Port == 2000)
                        check++;
                }
                if (check == 0)
                    flag = false;
            }
            Init();
            SetBtn(0);
            ShowStatus("Connection Status");
        }

        private void BufferClear()
        {
            if (wavePlayer != null)
                try
                {
                    wavePlayer.Dispose();
                }
                finally
                {
                    wavePlayer = null;
                }
            if (waveRecorder != null)
                try
                {
                    waveRecorder.Dispose();
                }
                finally
                {
                    waveRecorder = null;
                }
            m_Fifo.Flush();
        }
    }
}
