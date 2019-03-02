namespace KD.Serialization.Java
{
    /// <summary>
    /// Constants specific for Java language.
    /// Name are the same for easier understanding.
    /// 
    /// @see java.io.ObjectStreamConstants
    /// </summary>
    internal static class JavaConstants
    {
        /// <summary>
        /// Maximum data block length.
        /// </summary>
        internal const int MAX_BLOCK_SIZE = 1024;
        /// <summary>
        /// Maximum data block header length.
        /// </summary>
        internal const int MAX_HEADER_SIZE = 5;
        /// <summary>
        /// Length of char buffer. User for writing Java strings.
        /// </summary>
        internal const int CHAR_BUF_SIZE = 256;
        /// <summary>
        /// </summary>
        internal const int FLOAT_EXP_BIT_MASK = 2139095040;
        /// <summary>
        /// </summary>
        internal const int FLOAT_SIGNIF_BIT_MASK = 8388607;
        /// <summary>
        /// </summary>
        internal const long DOUBLE_EXP_BIT_MASK = 9218868437227405312L;
        /// <summary>
        /// </summary>
        internal const long DOUBLE_SIGNIF_BIT_MASK = 4503599627370495L;
        /// <summary>
        /// Magic number that is written to stream header.
        /// </summary>
        internal const short STREAM_MAGIC = unchecked((short)0xACED);
        /// <summary>
        /// Version number that is written to the stream.
        /// </summary>
        internal const short STREAM_VERSION = (short)5;
        /// <summary>
        /// First tag value.
        /// </summary>
        internal const byte TC_BASE = 0x70;
        /// <summary>
        /// Null object reference.
        /// </summary>
        internal const byte TC_NULL = (byte)0x70;
        /// <summary>
        /// Reference to an object already written into the stream.
        /// </summary>
        internal const byte TC_REFERENCE = (byte)0x71;
        /// <summary>
        /// New class descriptor.
        /// </summary>
        internal const byte TC_CLASSDESC = (byte)0x72;
        /// <summary>
        /// New object.
        /// </summary>
        internal const byte TC_OBJECT = (byte)0x73;
        /// <summary>
        /// New string.
        /// </summary>
        internal const byte TC_STRING = (byte)0x74;
        /// <summary>
        /// New array.
        /// </summary>
        internal const byte TC_ARRAY = (byte)0x75;
        /// <summary>
        /// Reference to class.
        /// </summary>
        internal const byte TC_CLASS = (byte)0x76;
        /// <summary>
        /// Block of optional data.
        /// Byte following tag indicates number of bytes in this block data.
        /// </summary>
        internal const byte TC_BLOCKDATA = (byte)0x77;
        /// <summary>
        /// End of optional block data blocks for an object.
        /// </summary>
        internal const byte TC_ENDBLOCKDATA = (byte)0x78;
        /// <summary>
        /// Reset stream context. 
        /// All handles written into stream are reset.
        /// </summary>
        internal const byte TC_RESET = (byte)0x79;
        /// <summary>
        /// Long block data.
        /// The long following the tag indicates the number of bytes in this block data.
        /// </summary>
        internal const byte TC_BLOCKDATALONG = (byte)0x7A;
        /// <summary>
        /// Exception during write.
        /// </summary>
        internal const byte TC_EXCEPTION = (byte)0x7B;
        /// <summary>
        /// Long string.
        /// </summary>
        internal const byte TC_LONGSTRING = (byte)0x7C;
        /// <summary>
        /// New proxy class descriptor.
        /// </summary>
        internal const byte TC_PROXYCLASSDESC = (byte)0x7D;
        /// <summary>
        /// New Java enum constant.
        /// </summary>
        internal const byte TC_ENUM = (byte)0x7E;
        /// <summary>
        /// Last tag value.
        /// </summary>
        internal const byte TC_MAX = (byte)0x7E;
        /// <summary>
        /// First wire handle to be assigned.
        /// </summary>
        internal const int BASE_WIRE_HANDLE = 0x7e0000;
        /// <summary>
        /// Bit mask which indicates that class implements ISerializable.
        /// </summary>
        internal const byte SC_WRITE_METHOD = 0x01;
        /// <summary>
        /// Bit mask which indicates that class has Serializable attribute.
        /// </summary>
        internal const byte SC_SERIALIZABLE = 0x02;
        /// <summary>
        /// Bit mask which indicates that class is an enum.
        /// </summary>
        internal const byte SC_ENUM = 0x10;
    }
}
