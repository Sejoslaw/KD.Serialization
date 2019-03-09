using System.IO;

namespace KD.Serialization
{
    /// <summary>
    /// Defines an abstract layer for all of the serializers.
    /// </summary>
    public abstract class AbstractSerializer : ISerializer
    {
        /// <summary>
        /// Stream into all the data will be serialized.
        /// </summary>
        protected Stream Stream { get; private set; }

        public AbstractSerializer(Stream inputStream)
        {
            this.Stream = inputStream;
        }

        public virtual void Dispose()
        {
            this.Stream.Close();
            this.Stream = null;
        }

        public virtual void Flush()
        {
            this.Stream.Flush();
        }

        public abstract void WriteBoolean(bool value);
        public abstract void WriteByte(byte value);
        public abstract void WriteChar(char value);
        public abstract void WriteDouble(double value);
        public abstract void WriteFloat(float value);
        public abstract void WriteInt(int value);
        public abstract void WriteLong(long value);
        public abstract void WriteObject(object value);
        public abstract void WriteShort(short value);
        public abstract void WriteString(string value);
        public abstract void WriteUnsignedByte(int value);
        public abstract void WriteUnsignedShort(int value);
    }
}
