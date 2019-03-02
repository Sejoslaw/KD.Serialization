using System;
using System.Reflection;
using System.Text;

namespace KD.Serialization.Java
{
    /// <summary>
    /// Contains various methods connected with Java primitives.
    /// </summary>
    internal static class JavaPrimitives
    {
        /// <summary>
        /// Returns Java primitive char:
        /// 
        /// B            byte
        /// C            char
        /// D            double
        /// F            float
        /// I            int
        /// J            long
        /// L            class or interface
        /// S            short
        /// Z            boolean
        /// [            array
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static char GetJavaPrimitiveChar(this FieldInfo field)
        {
            return GetJavaClassSignature(field)[0];
        }

        internal static string GetJavaClassSignature(this FieldInfo field)
        {
            StringBuilder sb = new StringBuilder();
            Type type = field.FieldType;

            while (type.IsArray)
            {
                sb.Append('[');
                type = type.GetElementType();
            }

            if (type.IsPrimitive)
            {
                if (type == typeof(int)) sb.Append('I');
                else if (type == typeof(byte)) sb.Append('B');
                else if (type == typeof(long)) sb.Append('J');
                else if (type == typeof(float)) sb.Append('F');
                else if (type == typeof(double)) sb.Append('D');
                else if (type == typeof(short)) sb.Append('S');
                else if (type == typeof(char)) sb.Append('C');
                else if (type == typeof(bool)) sb.Append('Z');
                else if (type == typeof(void)) sb.Append('V'); // TODO: ??? Not sure.
                else
                {
                    // TODO: Maybe better or more accurate exception ??? (try to avoid thinking if to create new exception class - try to use already existing exception)
                    throw new Exception($"Unknown primitive type: { type }");
                }
            }
            else
            {
                sb.Append($"L{ type.FullName.Replace('.', '/') };");
            }

            return sb.ToString();
        }
    }
}