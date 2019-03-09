using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace KD.Serialization.CSharp
{
    /// <summary>
    /// Deserializer which use standard .NET deserialization mechanics.
    /// </summary>
    public class CSharpDeserializer : AbstractDeserializer
    {
        protected BinaryReader Reader { get; }
        protected IFormatter Formatter { get; }

        public CSharpDeserializer(Stream inputStream) : base(inputStream)
        {
            this.Reader = new BinaryReader(this.Stream);
            this.Formatter = new BinaryFormatter();
        }

        public override bool ReadBoolean()
        {
            return this.Reader.ReadBoolean();
        }

        public override byte ReadByte()
        {
            return this.Reader.ReadByte();
        }

        public override char ReadChar()
        {
            return this.Reader.ReadChar();
        }

        public override double ReadDouble()
        {
            return this.Reader.ReadDouble();
        }

        public override float ReadFloat()
        {
            return this.Reader.ReadSingle();
        }

        public override int ReadInt()
        {
            return this.Reader.ReadInt32();
        }

        public override long ReadLong()
        {
            return this.Reader.ReadInt64();
        }

        public override object ReadObject()
        {
            return this.Formatter.Deserialize(this.Stream);
        }

        public override short ReadShort()
        {
            return this.Reader.ReadInt16();
        }

        public override string ReadString()
        {
            return this.Reader.ReadString();
        }

        public override int ReadUnsignedByte()
        {
            return this.Reader.ReadInt32();
        }

        public override int ReadUnsignedShort()
        {
            return this.Reader.ReadInt32();
        }

        public override void SkipBytes(int numberOfBytes)
        {
            this.Reader.BaseStream.Seek(numberOfBytes, SeekOrigin.Current);
        }
    }
}
