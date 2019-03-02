using System;
using static KD.Serialization.Java.JavaConstants;

namespace KD.Serialization.Java
{
    /// <summary>
    /// Used for low level operations.
    /// 
    /// @see java.io.Bits
    /// </summary>
    internal static class JavaBits
    {
        internal static void PutBoolean(byte[] buffer, int position, bool value)
        {
            buffer[position] = (byte)(value ? 1 : 0);
        }

        internal static void PutChar(byte[] buffer, int position, char value)
        {
            buffer[position + 1] = (byte)(value);
            buffer[position] = (byte)(value >> 8);
        }

        internal static void PutDouble(byte[] buffer, int position, double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            long converted = BitConverter.ToInt64(bytes, 0);

            // Java magic trick - see Double.java - Double.doubleToLongBits(...)
            if (((converted & DOUBLE_EXP_BIT_MASK) == DOUBLE_EXP_BIT_MASK) && (converted & DOUBLE_SIGNIF_BIT_MASK) != 0L)
            {
                converted = 0x7ff8000000000000L;
            }

            PutLong(buffer, position, converted);
        }

        internal static void PutInt(byte[] buffer, int position, int value)
        {
            buffer[position + 3] = (byte)(value);
            buffer[position + 2] = (byte)(value >> 8);
            buffer[position + 1] = (byte)(value >> 16);
            buffer[position] = (byte)(value >> 24);
        }

        internal static void PutLong(byte[] buffer, int position, long value)
        {
            buffer[position + 7] = (byte)(value);
            buffer[position + 6] = (byte)(value >> 8);
            buffer[position + 5] = (byte)(value >> 16);
            buffer[position + 4] = (byte)(value >> 24);
            buffer[position + 3] = (byte)(value >> 32);
            buffer[position + 2] = (byte)(value >> 40);
            buffer[position + 1] = (byte)(value >> 48);
            buffer[position] = (byte)(value >> 56);
        }

        internal static void PutFloat(byte[] buffer, int position, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            int converted = BitConverter.ToInt32(bytes, 0);

            // Java magic trick - see Float.java - Float.floatToIntBits(...)
            if (((converted & FLOAT_EXP_BIT_MASK) == FLOAT_EXP_BIT_MASK) &&
                (converted & FLOAT_SIGNIF_BIT_MASK) != 0)
            {
                converted = 0x7fc00000;
            }

            PutInt(buffer, position, converted);
        }

        internal static void PutShort(byte[] buffer, int position, short value)
        {
            buffer[position + 1] = (byte)(value);
            buffer[position] = (byte)(value >> 8);
        }
    }
}
