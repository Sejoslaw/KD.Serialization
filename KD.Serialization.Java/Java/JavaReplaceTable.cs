using System.Collections.Generic;

namespace KD.Serialization.Java
{
    /// <summary>
    /// Table which is used to map object to replacement object.
    /// 
    /// @see java.io.ObjectOutputStream.ReplaceTable
    /// </summary>
    internal class JavaReplaceTable
    {
        /// <summary>
        /// Maps: Object -> Index
        /// </summary>
        private JavaHandleTable handleTable;
        /// <summary>
        /// Maps: Index -> Replacement Object
        /// </summary>
        private List<object> replacements = new List<object>();

        internal JavaReplaceTable(int initialCapacity)
        {
            this.handleTable = new JavaHandleTable(initialCapacity);

            for (int i = 0; i < initialCapacity; ++i)
            {
                this.replacements.Add(null);
            }
        }

        /// <summary>
        /// Maps object to replacement object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="replacement"></param>
        internal void Assign(object obj, object replacement)
        {
            int index = this.handleTable.Assign(obj);
            while (index >= this.replacements.Count)
            {
                this.replacements.Add(null);
            }
            this.replacements[index] = replacement;
        }

        /// <summary>
        /// Returns replacement for specified object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal object Lookup(object obj)
        {
            int index = this.handleTable.Lookup(obj);
            return index >= 0 ? this.replacements[index] : obj;
        }
    }
}