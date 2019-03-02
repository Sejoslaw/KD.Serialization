using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using static KD.Serialization.Java.JavaConstants;

namespace KD.Serialization.Java
{
    /// <summary>
    /// Serializer dedicated to serialize objects and primitives the same way as Java do it object serialization (ObjectOutputStream).
    /// The serializer will do all the magic the "ObjectOutputStream" does when serializing an object, with all the overhead.
    /// 
    /// @see java.io.ObjectOutputStream
    /// </summary>
    public class JavaSerializer : ISerializer
    {
        /// <summary>
        /// Current offset into buffer.
        /// </summary>
        private int position = 0;
        /// <summary>
        /// Buffer for writing general / block data.
        /// </summary>
        private byte[] buffer = new byte[MAX_BLOCK_SIZE];
        /// <summary>
        /// Buffer for writing block data headers.
        /// </summary>
        private byte[] hBuffer = new byte[MAX_BLOCK_SIZE];
        /// <summary>
        /// Block data mode.
        /// By default it must be false to prevent from overriding stream header.
        /// </summary>
        private bool blockMode = false;
        /// <summary>
        /// Used when serializing an object.
        /// </summary>
        private int recursionDepth = 0;
        /// <summary>
        /// Maps: Object -> Wire Handle
        /// </summary>
        private JavaHandleTable handleTable;
        /// <summary>
        /// Maps: Object -> Replacement Object
        /// </summary>
        private JavaReplaceTable replaceTable;
        /// <summary>
        /// Stream into which all the serialized data will be written.
        /// </summary>
        private Stream stream;

        public JavaSerializer(Stream stream)
        {
            this.stream = stream;

            this.handleTable = new JavaHandleTable(10);
            this.replaceTable = new JavaReplaceTable(10);

            this.WriteJavaStreamHeader();
            this.SetBlockMode(true);
        }

        public void Dispose()
        {
            this.position = 0;
            this.buffer = null;
            this.hBuffer = null;
        }

        public void Flush()
        {
            this.Drain();
            this.stream.Flush();
        }

        public void WriteBoolean(bool value)
        {
            if (this.position >= MAX_BLOCK_SIZE)
            {
                this.Drain();
            }
            JavaBits.PutBoolean(this.buffer, this.position++, value);
        }

        public void WriteByte(byte value)
        {
            if (this.position >= MAX_BLOCK_SIZE)
            {
                this.Drain();
            }
            this.buffer[this.position++] = value;
        }

        public void WriteChar(char value)
        {
            if (this.position + 2 <= MAX_BLOCK_SIZE)
            {
                JavaBits.PutChar(this.buffer, this.position, value);
                this.position += 2;
            }
        }

        public void WriteDouble(double value)
        {
            if (this.position + 8 <= MAX_BLOCK_SIZE)
            {
                JavaBits.PutDouble(this.buffer, this.position, value);
                this.position += 8;
            }
        }

        public void WriteFloat(float value)
        {
            if (this.position + 4 <= MAX_BLOCK_SIZE)
            {
                JavaBits.PutFloat(this.buffer, this.position, value);
                this.position += 4;
            }
        }

        public void WriteInt(int value)
        {
            if (this.position + 4 <= MAX_BLOCK_SIZE)
            {
                JavaBits.PutInt(this.buffer, this.position, value);
                this.position += 4;
            }
        }

        public void WriteLong(long value)
        {
            if (this.position + 8 <= MAX_BLOCK_SIZE)
            {
                JavaBits.PutLong(this.buffer, this.position, value);
                this.position += 8;
            }
        }

        /// <summary>
        /// Serialize C# object to Java standards.
        /// 
        /// Keep in mind:
        /// C# namespace == Java package
        /// C# usings == Java imports
        /// </summary>
        /// <param name="value"></param>
        public void WriteObject(object value)
        {
            this.WriteObject(value, false);
        }

        public void WriteShort(short value)
        {
            if (this.position + 2 <= MAX_BLOCK_SIZE)
            {
                JavaBits.PutShort(this.buffer, this.position, value);
                this.position += 2;
            }
        }

        public void WriteString(string value)
        {
            int endOffset = value.Length;
            int charPosition = 0;
            int charSize = 0;

            for (int offset = 0; offset < endOffset; ++offset)
            {
                if (charPosition >= charSize)
                {
                    charPosition = 0;
                    charSize = Math.Min(endOffset - offset, CHAR_BUF_SIZE);
                    string subString = value.Substring(offset, offset + charSize);
                    this.WriteStringToBuffer(this.buffer, subString);
                }
            }
        }

        public void WriteUnsignedByte(int value)
        {
            throw new System.NotImplementedException();
        }

        public void WriteUnsignedShort(int value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Different Java OutputStream classes may include different headers.
        /// This is prepared for "ObjectOutputStream".
        /// </summary>
        private void WriteJavaStreamHeader()
        {
            this.WriteShort(STREAM_MAGIC);
            this.WriteShort(STREAM_VERSION);
        }

        /// <summary>
        /// Sets block data mode to the given mode (true == on, false == off) and returns the previous mode value.
        /// If the new mode is the same as the old mode, no action is taken.
        /// If the new mode differs from the old mode, any buffered data is flushed before switching to the new mode.
        /// </summary>
        /// <param name="mode"></param>
        private bool SetBlockMode(bool mode)
        {
            if (this.blockMode == mode)
            {
                return this.blockMode;
            }
            this.Drain();
            this.blockMode = mode;
            return !this.blockMode;
        }

        /// <summary>
        /// Writes all buffered data from current stream to the underlying stream, but does not flush underlying stream.
        /// </summary>
        private void Drain()
        {
            if (this.position == 0)
            {
                return;
            }

            if (this.blockMode)
            {
                this.WriteBlockHeader(this.position);
            }

            this.stream.Write(this.buffer, 0, this.position);
            this.position = 0;
        }

        /// <summary>
        /// Writes block data header.
        /// Data block shorter than 256 bytes are prefixed with a 2-byte header; all others start with a 5-byte header.
        /// </summary>
        /// <param name="position"></param>
        private void WriteBlockHeader(int position)
        {
            if (position <= 0xFF)
            {
                this.hBuffer[0] = TC_BLOCKDATA;
                this.hBuffer[1] = (byte)position;
                this.stream.Write(this.hBuffer, 0, 2);
            }
            else
            {
                this.hBuffer[0] = TC_BLOCKDATALONG;
                JavaBits.PutInt(this.hBuffer, 1, position);
                this.stream.Write(this.hBuffer, 0, 5);
            }
        }

        private void WriteStringToBuffer(byte[] buffer, string subString)
        {
            for (int i = 0; i < subString.Length; ++i)
            {
                buffer[i] = (byte)subString[i];
            }
        }

        /// <summary>
        /// Main method for writing an object to stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unshared"></param>
        private void WriteObject(object obj, bool unshared)
        {
            bool oldMode = this.SetBlockMode(false);
            this.recursionDepth++;
            try
            {
                int handler;
                if ((obj = this.replaceTable.Lookup(obj)) == null)
                {
                    this.WriteNull();
                    return;
                }
                else if (!unshared && (handler = this.handleTable.Lookup(obj)) != -1)
                {
                    this.WriteHandle(handler);
                    return;
                }
                else if (obj is Type) // Replacement for Java version -> (obj instanceof Class)
                {
                    this.WriteType(obj as Type, unshared); // Equivalent of Java -> WriteClass method
                    return;
                }
                // TODO: Another "else if" - equaivalent of "(obj instanceof ObjectStreamClass)"

                // TODO: Java contains mechanics for the same check but with replaced object.

                // Remaining cases
                Type objType = obj.GetType();
                if (obj is string)
                {
                    this.WriteString(obj as string);
                }
                else if (objType.IsArray)
                {
                    this.WriteArray(obj, unshared);
                }
                else if (objType.IsEnum)
                {
                    this.WriteEnum(obj, unshared);
                }
                else if (objType.IsSerializable)
                {
                    this.WriteOrdinaryObject(obj, unshared);
                }
            }
            finally
            {
                this.recursionDepth--;
                this.SetBlockMode(oldMode);
            }
        }

        /// <summary>
        /// Writes serializable object to Stream. 
        /// (NOT: String, Enum, Array, Type)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="unshared"></param>
        private void WriteOrdinaryObject(object obj, bool unshared)
        {
            Type objType = obj.GetType();

            if (!objType.IsSerializable)
            {
                throw new SerializationException($"Object is not serializable: { obj }");
            }

            this.WriteByte(TC_OBJECT);
            this.WriteTypeDescription(objType, false);
            this.handleTable.Assign(unshared ? null : obj);
            
            if (this.getSerializableFields(objType).Count > 0)
            {
                this.WriteExternalData(obj as ISerializable);
            }
            else
            {
                this.WriteSerialData(obj);
            }
        }

        /// <summary>
        /// Writes instance data for each serializable class of given object, from superclass to subclass.
        /// </summary>
        /// <param name="obj"></param>
        private void WriteSerialData(object obj)
        {
            // TODO: Create an collection with types. 
            // The collection should be ordered by inheritance with those containing "higher" supertypes appearing first. 
            // The last one should be == obj.GetType().
        }

        /// <summary>
        /// Write external data from ISerializable.
        /// </summary>
        /// <param name="serializable"></param>
        private void WriteExternalData(ISerializable serializable)
        {
            // TODO: Add support for different Java protocols.

            this.SetBlockMode(true);
            this.HandleSerializable(serializable);
            this.SetBlockMode(false);
            this.WriteByte(TC_ENDBLOCKDATA);
        }

        /// <summary>
        /// Handled serializable (run ISerializable method with actual parameters).
        /// </summary>
        /// <param name="serializable"></param>
        private void HandleSerializable(ISerializable serializable)
        {
            FormatterConverter formatterConverter = new FormatterConverter();
            SerializationInfo serializationInfo = new SerializationInfo(serializable.GetType(), formatterConverter);
            StreamingContext streamingContext = new StreamingContext(StreamingContextStates.All);

            serializable.GetObjectData(serializationInfo, streamingContext);

            // TODO: Check and test ISerializable support.
            // Serialize additional data
            if (serializationInfo.MemberCount > 0)
            {
                foreach (SerializationEntry entry in serializationInfo)
                {
                    this.WriteString(entry.Name);
                    this.WriteType(entry.ObjectType, false);

                    if (entry.ObjectType.IsPrimitive)
                    {
                        this.TryWriteTypedPrimitive<int>(entry.Value, entry.ObjectType, this.WriteInt);
                        this.TryWriteTypedPrimitive<byte>(entry.Value, entry.ObjectType, this.WriteByte);
                        this.TryWriteTypedPrimitive<long>(entry.Value, entry.ObjectType, this.WriteLong);
                        this.TryWriteTypedPrimitive<float>(entry.Value, entry.ObjectType, this.WriteFloat);
                        this.TryWriteTypedPrimitive<double>(entry.Value, entry.ObjectType, this.WriteDouble);
                        this.TryWriteTypedPrimitive<short>(entry.Value, entry.ObjectType, this.WriteShort);
                        this.TryWriteTypedPrimitive<char>(entry.Value, entry.ObjectType, this.WriteChar);
                        this.TryWriteTypedPrimitive<bool>(entry.Value, entry.ObjectType, this.WriteBoolean);
                    }
                    else
                    {
                        this.WriteObject(entry.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to write primitive value type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <param name="writeInt"></param>
        private void TryWriteTypedPrimitive<T>(object value, Type valueType, Action<T> action)
        {
            if (!valueType.IsPrimitive)
            {
                throw new SerializationException($"Specified value is not a primitive value: { valueType.FullName }");
            }

            if (value is T)
            {
                action((T)value);
            }
        }

        /// <summary>
        /// Writes enum to Stream.
        /// </summary>
        /// <param name="objEnum"></param>
        /// <param name="unshared"></param>
        private void WriteEnum(object objEnum, bool unshared)
        {
            Type obEnumType = objEnum.GetType();

            this.WriteByte(TC_ENUM);
            this.WriteTypeDescription(obEnumType, false);
            this.handleTable.Assign(unshared ? null : objEnum);
            this.WriteString(Enum.GetName(obEnumType, objEnum));
        }

        /// <summary>
        /// Writes given array to Stream.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="unshared"></param>
        private void WriteArray(object array, bool unshared)
        {
            Type arrayType = array.GetType();

            this.WriteByte(TC_ARRAY);
            this.WriteTypeDescription(arrayType, false);
            this.handleTable.Assign(unshared ? null : array);

            Type arrayElementType = arrayType.GetElementType();
            if (arrayElementType.IsPrimitive)
            {
                this.TryWriteTypedArray<int>(array, this.WriteInt);
                this.TryWriteTypedArray<byte>(array, this.WriteByte);
                this.TryWriteTypedArray<long>(array, this.WriteLong);
                this.TryWriteTypedArray<float>(array, this.WriteFloat);
                this.TryWriteTypedArray<double>(array, this.WriteDouble);
                this.TryWriteTypedArray<short>(array, this.WriteShort);
                this.TryWriteTypedArray<char>(array, this.WriteChar);
                this.TryWriteTypedArray<bool>(array, this.WriteBoolean);
            }
            else
            {
                object[] actualArray = array as object[];
                this.WriteInt(actualArray.Length);

                foreach (object arrayElement in actualArray)
                {
                    this.WriteObject(arrayElement);
                }
            }
        }

        /// <summary>
        /// Tries to write typed array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        private void TryWriteTypedArray<T>(object array, Action<T> action)
        {
            if (array is T[])
            {
                T[] typedArray = array as T[];
                foreach (T element in typedArray)
                {
                    action(element);
                }
            }
        }

        /// <summary>
        /// Writes null to stream.
        /// </summary>
        private void WriteNull()
        {
            this.WriteByte(TC_NULL);
        }

        /// <summary>
        /// Writes specified object handler to stream.
        /// </summary>
        /// <param name="handler"></param>
        private void WriteHandle(int handler)
        {
            this.WriteByte(TC_REFERENCE);
            this.WriteInt(BASE_WIRE_HANDLE + handler);
        }

        /// <summary>
        /// Replacement for Java version -> (obj instanceof Class).
        /// Equivalent of Java -> WriteClass method. 
        /// 
        /// Writes representation of given Type to Stream.
        /// </summary>
        /// <param name="type"></param>
        private void WriteType(Type type, bool unshared)
        {
            this.WriteByte(TC_CLASS);
            this.WriteTypeDescription(type, unshared);
            this.handleTable.Assign(unshared ? null : type);
        }

        /// <summary>
        /// Writes description of the specified type to Stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unshared"></param>
        private void WriteTypeDescription(Type type, bool unshared)
        {
            int handle;
            if (type == null)
            {
                this.WriteNull();
            }
            else if (!unshared && (handle = this.handleTable.Lookup(type)) != -1)
            {
                this.WriteHandle(handle);
            }

            // TODO: Replace current method call to check if Type is proxy or not -> replace with if-else to check if current Type is proxy or not
            this.WriteNonProxyDescription(type, unshared);
        }

        /// <summary>
        /// Writes Type description representing a standard Type to Stream.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="unshared"></param>
        private void WriteNonProxyDescription(Type type, bool unshared)
        {
            this.WriteByte(TC_CLASSDESC);
            this.handleTable.Assign(unshared ? null : type);

            this.WriteNonProxyDescription(type);

            this.SetBlockMode(true);
            // Java adds here a methods to check annotations of class.
            // TODO: Maybe add somethind ???
            // This will be equal with removing "sealed" from JavaSerializer class.
            this.SetBlockMode(false);
            this.WriteByte(TC_ENDBLOCKDATA);

            this.WriteTypeDescription(type.BaseType, false);
        }

        /// <summary>
        /// Writes non-proxy Type description to Stream.
        /// </summary>
        /// <param name="type"></param>
        private void WriteNonProxyDescription(Type type)
        {
            this.WriteString(type.FullName);
            // TODO: Write here a SerialVersionUID as this.WriteLong(...)

            byte flags = 0;
            // TODO: Add pre-if check to check if Type can be understand as Java "Externalizable"
            if (type.IsSerializable)
            {
                flags |= SC_SERIALIZABLE;
            }
            if (type.GetInterfaces().Contains(typeof(ISerializable)))
            {
                flags |= SC_WRITE_METHOD;
            }
            if (type.IsEnum)
            {
                flags |= SC_ENUM;
            }
            this.WriteByte(flags);

            this.WriteFields(type);
        }

        /// <summary>
        /// Writes fields from specified type.
        /// </summary>
        /// <param name="type"></param>
        private void WriteFields(Type type)
        {
            List<FieldInfo> serializableFields = type.GetFields().Where(fi => !fi.IsNotSerialized).ToList();

            foreach (FieldInfo field in serializableFields)
            {
                string javaTypeSignature = field.GetJavaClassSignature();

                this.WriteByte((byte)javaTypeSignature[0]);
                this.WriteString(field.Name);

                if (!field.FieldType.IsPrimitive)
                {
                    this.WriteFieldTypeString(javaTypeSignature);
                }
            }
        }

        /// <summary>
        /// Writes string without allowing it to be replaced in stream.
        /// </summary>
        /// <param name="field"></param>
        private void WriteFieldTypeString(string javaTypeSignature)
        {
            int handler;
            if (javaTypeSignature == null)
            {
                this.WriteNull();
            }
            else if ((handler = this.handleTable.Lookup(javaTypeSignature)) != -1)
            {
                this.WriteHandle(handler);
            }
            else
            {
                this.WriteString(javaTypeSignature);
            }
        }

        private List<FieldInfo> getSerializableFields(Type type) 
        {
            return type.GetFields().Where(fi => !fi.IsNotSerialized).ToList();
        }
    }
}