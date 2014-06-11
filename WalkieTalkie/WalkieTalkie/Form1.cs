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


namespace WalkieTalkie
{
    public partial class Form1 : Form
    {
        private bool connected = false;
        private Socket s;
        private Thread t;
        private FifoStream m_Fifo = new FifoStream();
        private WaveOutPlayer m_Player;
        private WaveInRecorder m_Recorder;

        private byte[] m_PlayBuffer;
        private byte[] m_RecBuffer;
        public Form1()
        {
            InitializeComponent();
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            t = new Thread(new ThreadStart(Voice_In));
        }

        private void Voice_In()
        {
            byte[] b;          //한번에 소켓 통신에서 읽어올 데이터의 크기
            s.Bind(new IPEndPoint(IPAddress.Any, int.Parse(this.textBox2.Text)));
            //thread로 계속 통신을 위해
            while(true)
            {
                b = new byte[16384];
                s.Receive(b);
                textBox4.Text += Encoding.ASCII.GetString(b, 0, b.Length) + Environment.NewLine;

                //m_Fifo.Write(b, 0, b.Length);
            }
        }

        //관리되지 않는 데이터(마이크로 들어온 데이터)를 버퍼에 저장
        private void Voice_Out(IntPtr data, int size)
        {
            //버퍼가 size보다 작거나 버퍼가 초기화되지 않았을때 버퍼 생성
            if (m_RecBuffer == null || m_RecBuffer.Length < size)
                m_RecBuffer = new byte[size + 4];
            System.Runtime.InteropServices.Marshal.Copy(data, m_RecBuffer, 0, size);
            int b_Size = m_RecBuffer.Length;
            for (int i = b_Size - 4; i < b_Size; i++)
            {
                m_RecBuffer[i] = 97;
            }
            s.SendTo(m_RecBuffer, new IPEndPoint(IPAddress.Parse(this.textBox1.Text), int.Parse(this.textBox3.Text)));
        }

        //connect 버튼 클릭시 이벤트
        private void btn_cnt_Click(object sender, EventArgs e)
        {
            if (connected == false)
            {
                t.Start();
                connected = true;
            }
            Start();
        }

        private void btn_dcnt_Click(object sender, EventArgs e)
        {
            Stop();
            s.Shutdown(SocketShutdown.Both);        //작업을 마무리하고 보내기 받기 사용 불가
            s.Disconnect(true);                     //소켓 연결을 닫고 다시 사용할 수 있도록(true)인 경우
        }
        private void Start()
        {
            Stop();
            try
            {
                WaveFormat fmt = new WaveFormat(44100, 16, 2);
                m_Player = new WaveOutPlayer(-1, fmt, 16384, 3, new BufferFillEventHandler(Filler));
                m_Recorder = new WaveInRecorder(-1, fmt, 16384, 3, new BufferDoneEventHandler(Voice_Out));
            }
            catch
            {
                Stop();
                throw;
            }
        }

        private void Stop()
        {
            if (m_Player != null)
                try
                {
                    m_Player.Dispose();
                }
                finally
                {
                    m_Player = null;
                }
            if (m_Recorder != null)
                try
                {
                    m_Recorder.Dispose();
                }
                finally
                {
                    m_Recorder = null;
                }
            m_Fifo.Flush();
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
        private void Form1_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t.Abort();
            s.Close();
            Stop();
        }
    }
    public class FifoStream : Stream
    {
        private const int BlockSize = 65536;
        //private const int MaxBlocksInCache = (3 * 1024 * 1024) / BlockSize;

        private int m_Size;                                         //stream에 쓰여진 총 메모리 사이즈 byte 단위
        private int m_RPos;                                         //단위 block 내에서 앞오로 읽시 시작할 byte의 인덱스
        private int m_WPos;                                         //단위 block 내에서 앞으로 쓰기 시작할 byte의 인덱스
        //private Stack m_UsedBlocks = new Stack();                   
        private ArrayList m_Blocks = new ArrayList();               //stream에 쓰여진 data를 BlockSize단위로 나누어 저장한 ArrayList

        public override void Write(byte[] buffer, int offset, int count)            //(쓸 데이터가 들어있는 버퍼, 버퍼내의 쓸 데이터의 첫번째 인덱스, 총 쓸 데이터)
        {
            lock(this)
            {
                int left = count;           //write할 data의 크기, byte 단위
                while(left > 0)             //write할 buffter가 남아있으면
                {
                    int toWrite = Math.Min(BlockSize - m_WPos, left);                           //한 번에 쓸 데이터의 크기 byte 단위
                    Array.Copy(buffer, offset + count - left, GetWBlock(), m_WPos, toWrite);    //(쓸 데이터가 들어있는 버퍼, 버퍼내의 쓸 데이터의 첫번째 인덱스,
                                                                                                // 데이터를 쓸 블럭, 블럭내 첫번째 쓸 인덱스, 한번에 쓸 크기)
                    m_WPos += toWrite;
                    left -= toWrite;
                }
                m_Size += count;
            }
            //throw new NotImplementedException();
        }
        //제일 마지막 블럭을 불러오거나 쓰기할 공간이 없을때 새로운 블록을 추가하여 리턴
        private byte[] GetWBlock()
        {
            byte[] result = null;
            if (m_WPos < BlockSize && m_Blocks.Count > 0)
                result = (byte[])m_Blocks[m_Blocks.Count - 1];
            //m_Blocks에 엘리먼트가 하나도 없을때 혹은 m_WPos가 BlockSize보다 클 때
            else
            {
                result = AllocBlock();
                m_Blocks.Add(result);
                m_WPos = 0;
            }
            return result;
        }
        //쓰기에 사용할 블록을 할당
        private byte[] AllocBlock()
        {
            byte[] result = null;
            //result = m_UsedBlocks.Count > 0 ? (byte[])m_UsedBlocks.Pop() : new byte[BlockSize];
            result = new byte[BlockSize];
            return result;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock(this)
            {
                int result = Peek(buffer, offset, count);
                Advance(result);
                return result;
            }
        }
        public int Peek(byte[] buffer, int offset, int count)
        {
            lock(this)
            {
                int sizeLeft = count;
                int tempBlockPos = m_RPos;
                int tempSize = m_Size;

                int CurrentBlock = 0;
                while (sizeLeft > 0 && tempSize > 0)
                {
                    if (tempBlockPos == BlockSize)
                    //if (tempBlockPos >= BlockSize)
                    {
                        tempBlockPos = 0;
                        CurrentBlock++;
                    }
                    int upper = CurrentBlock < m_Blocks.Count - 1 ? BlockSize : m_WPos;
                    int toFeed = Math.Min(upper - tempBlockPos, sizeLeft);
                    Array.Copy((byte[])m_Blocks[CurrentBlock], tempBlockPos, buffer, offset + count - sizeLeft, toFeed);
                    sizeLeft -= toFeed;
                    tempBlockPos += toFeed;
                    tempSize -= toFeed;
                }
                return count - sizeLeft;
            }
        }
        public int Advance(int count)
        {
            lock(this)
            {
                int sizeLeft = count;
                while(sizeLeft > 0 && m_Size > 0)
                {
                    if (m_RPos == BlockSize)
                    {
                        m_RPos = 0;
                        m_Blocks.RemoveAt(0);
                    }
                    int toFeed = m_Blocks.Count == 1 ? Math.Min(m_WPos - m_RPos, sizeLeft) : Math.Min(BlockSize - m_RPos, sizeLeft);
                    m_RPos += toFeed;
                    sizeLeft -= toFeed;
                    m_Size -= toFeed;
                }
                return count - sizeLeft;
            }
        }
        public override void Flush()
        {
            lock(this)
            {
                m_Blocks.Clear();
                m_RPos = 0;
            }
        }
        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return false; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override long Length
        {
            get
            {
                lock (this)
                    return m_Size;
            }
        }
        public override long Position
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }
        public override void Close()
        {
            Flush();
        }
        public override void SetLength(long len)
        {
            throw new InvalidOperationException();
        }
        public override long Seek(long pos, SeekOrigin o)
        {
            throw new InvalidOperationException();
        }
    }
}
