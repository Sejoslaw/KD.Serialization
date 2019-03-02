using System;

namespace KD.Serialization
{
    /// @author Krzysztof Dobrzyński - k.dobrzynski94@gmail.com - https://github.com/Sejoslaw
    /// <summary>
    /// Core interface for all deserializers.
    /// </summary>
    public interface IDeserializer : IDisposable
    {
        /// <summary>
        /// Reads in a boolean.
        /// </summary>
        /// <returns></returns>
        bool ReadBoolean();

        /// <summary>
        /// Reads an 8 bit byte.
        /// </summary>
        /// <returns></returns>
        byte ReadByte();

        /// <summary>
        /// Reads a 16 bit char.
        /// </summary>
        /// <returns></returns>
        char ReadChar();

        /// <summary>
        /// Reads a 64 bit double.
        /// </summary>
        /// <returns></returns>
        double ReadDouble();

        /// <summary>
        /// Reads a 32 bit float.
        /// </summary>
        /// <returns></returns>
        float ReadFloat();

        /// <summary>
        /// Reads a 32 bit int.
        /// </summary>
        /// <returns></returns>
        int ReadInt();
        
        /// <summary>
        /// Reads a 64 bit long.
        /// </summary>
        /// <returns></returns>
        long ReadLong();

        /// <summary>
        /// Reads an object.
        /// </summary>
        /// <returns></returns>
        object ReadObject();

        /// <summary>
        /// Reads a 16 bit short.
        /// </summary>
        /// <returns></returns>
        short ReadShort();

        /// <summary>
        /// Reads an unsigned 8 bit byte.
        /// </summary>
        /// <returns></returns>
        int ReadUnsignedByte();

        /// <summary>
        /// Reads an unsigned 16 bit short.
        /// </summary>
        /// <returns></returns>
        int ReadUnsignedShort();

        /// <summary>
        /// Reads a String.
        /// </summary>
        /// <returns></returns>
        string ReadString();

        /// <summary>
        /// Skips bytes.
        /// </summary>
        /// <param name="numberOfBytes">Number of bites to skip.</param>
        void SkipBytes(int numberOfBytes);
    }
}
