using System.IO;

namespace KD.Serialization
{
    /// <summary>
    /// Defines an abstract layer for all of the deserializers.
    /// </summary>
    public abstract class AbstractDeserializer : IDeserializer
    {
        /// <summary>
        /// Stream from which all the data will be deserialized.
        /// </summary>
        protected Stream Stream { get; private set; }

        public AbstractDeserializer(Stream inputStream)
        {
            this.Stream = inputStream;
        }

        public virtual void Dispose()
        {
            this.Stream.Close();
            this.Stream = null;
        }

        public abstract bool ReadBoolean();
        public abstract byte ReadByte();
        public abstract char ReadChar();
        public abstract double ReadDouble();
        public abstract float ReadFloat();
        public abstract int ReadInt();
        public abstract long ReadLong();
        public abstract object ReadObject();
        public abstract short ReadShort();
        public abstract string ReadString();
        public abstract int ReadUnsignedByte();
        public abstract int ReadUnsignedShort();
        public abstract void SkipBytes(int numberOfBytes);
    }
}
