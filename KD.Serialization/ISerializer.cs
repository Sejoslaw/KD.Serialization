using System;

namespace KD.Serialization
{
    /// @author Krzysztof Dobrzyński - k.dobrzynski94@gmail.com - https://github.com/Sejoslaw
    /// <summary>
    /// Core interface for all serializers.
    /// </summary>
    public interface ISerializer : IDisposable
    {
        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteBoolean(bool value);

        /// <summary>
        /// Writes a 8 bit byte.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteByte(byte value);

        /// <summary>
        /// Writes a 16 bit char.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteChar(char value);

        /// <summary>
        /// Writes a 64 bit double.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteDouble(double value);

        /// <summary>
        /// Writes a 32 bit float.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteFloat(float value);

        /// <summary>
        /// Writes a 32 bit int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteInt(int value);

        /// <summary>
        /// Writes a 64 bit long.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteLong(long value);

        /// <summary>
        /// Writes an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteObject(object value);

        /// <summary>
        /// Writes a 16 bit short.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteShort(short value);

        /// <summary>
        /// Writes an unsigned 8 bit byte.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteUnsignedByte(int value);

        /// <summary>
        /// Writes an unsigned 16 bit short.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteUnsignedShort(int value);

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> Returns true if value was written corrently; otherwise false. </returns>
        void WriteString(string value);

        /// <summary>
        /// Flushes the internal <see cref="System.IO.Stream"/>. 
        /// </summary>
        void Flush();
    }
}
