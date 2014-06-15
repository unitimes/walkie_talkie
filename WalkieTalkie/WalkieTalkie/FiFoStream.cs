using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace WalkieTalkie
{
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
            lock (this)
            {
                int left = count;           //write할 data의 크기, byte 단위
                while (left > 0)             //write할 buffter가 남아있으면
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
            lock (this)
            {
                int result = Peek(buffer, offset, count);
                Advance(result);
                return result;
            }
        }
        public int Peek(byte[] buffer, int offset, int count)
        {
            lock (this)
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
            lock (this)
            {
                int sizeLeft = count;
                while (sizeLeft > 0 && m_Size > 0)
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
            lock (this)
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
