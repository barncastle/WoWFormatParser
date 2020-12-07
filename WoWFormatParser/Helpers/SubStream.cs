using System;
using System.IO;
using System.Text;

namespace WoWFormatParser.Helpers
{
    public class SubStream : Stream
    {
        public override bool CanRead => BaseStream.CanRead;
        public override bool CanSeek => BaseStream.CanSeek;
        public override bool CanWrite => false;
        public override long Length => End - Start;
        public override long Position
        {
            get => BaseStream.Position - Start;
            set
            {
                var offset = BaseStream.Position + value;
                if (offset < Start || offset > End)
                    throw new ArgumentOutOfRangeException(nameof(value));

                BaseStream.Position = offset;
            }
        }

        private readonly Stream BaseStream;
        private readonly long Start;
        private readonly long End;

        public SubStream(Stream stream, long length)
        {
            BaseStream = stream;
            Start = stream.Position;
            End = Math.Min(stream.Length, Start + length);
        }

        public BinaryReader GetBinaryReader()
        {
            return new BinaryReader(this, Encoding.UTF8, true);
        }

        public override void Flush() => BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return origin switch
            {
                SeekOrigin.Begin => Position = offset,
                SeekOrigin.Current => Position += offset,
                SeekOrigin.End => Position = Length - offset,
                _ => Position,
            };
        }


        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            /* prevented */
        }

        public override void Close()
        {
            /* prevented */
        }
    }
}
