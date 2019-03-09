using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace KD.Serialization.CSharp
{
    /// <summary>
    /// Serializer which use standard .NET serialization mechanics.
    /// </summary>
    public class CSharpSerializer : AbstractSerializer
    {
        protected BinaryWriter Writer { get; }
        protected IFormatter Formatter { get; }

        public CSharpSerializer(Stream inputStream) : base(inputStream)
        {
            this.Writer = new BinaryWriter(this.Stream);
            this.Formatter = new BinaryFormatter();
        }

        public override void WriteBoolean(bool value)
        {
            this.Writer.Write(value);
        }

        public override void WriteByte(byte value)
        {
            this.Writer.Write(value);
        }

        public override void WriteChar(char value)
        {
            this.Writer.Write(value);
        }

        public override void WriteDouble(double value)
        {
            this.Writer.Write(value);
        }

        public override void WriteFloat(float value)
        {
            this.Writer.Write(value);
        }

        public override void WriteInt(int value)
        {
            this.Writer.Write(value);
        }

        public override void WriteLong(long value)
        {
            this.Writer.Write(value);
        }

        public override void WriteObject(object value)
        {
            this.Formatter.Serialize(this.Stream, value);
        }

        public override void WriteShort(short value)
        {
            this.Writer.Write(value);
        }

        public override void WriteString(string value)
        {
            this.Writer.Write(value);
        }

        public override void WriteUnsignedByte(int value)
        {
            this.Writer.Write((byte)value);
        }

        public override void WriteUnsignedShort(int value)
        {
            this.Writer.Write((ushort)value);
        }
    }
}
