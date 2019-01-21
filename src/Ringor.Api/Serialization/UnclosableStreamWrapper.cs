using System;
using System.IO;

namespace Dalion.Ringor.Api.Serialization {
    internal class UnclosableStreamWrapper : Stream, IDisposable {
        private readonly Stream _baseStream;

        public UnclosableStreamWrapper(Stream baseStream) {
            _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
        }

        public override bool CanRead => _baseStream.CanRead;
        public override bool CanSeek => _baseStream.CanSeek;
        public override bool CanWrite => _baseStream.CanWrite;
        public override long Length => _baseStream.Length;

        public override long Position {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        public override bool CanTimeout => _baseStream.CanTimeout;

        public override int ReadTimeout {
            get => _baseStream.ReadTimeout;
            set => _baseStream.ReadTimeout = value;
        }

        public override int WriteTimeout {
            get => _baseStream.WriteTimeout;
            set => _baseStream.WriteTimeout = value;
        }

        public new void Dispose() { }

        public override void Flush() {
            _baseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin) {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value) {
            _baseStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count) {
            return _baseStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            _baseStream.Write(buffer, offset, count);
        }

        public override int ReadByte() {
            return _baseStream.ReadByte();
        }

        public override void WriteByte(byte value) {
            _baseStream.WriteByte(value);
        }

        protected override void Dispose(bool disposing) { }
    }
}
