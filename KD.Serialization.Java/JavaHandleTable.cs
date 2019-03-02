using System.Collections.Generic;

namespace KD.Serialization.Java
{
    /// <summary>
    /// Table which maps objects to integer handles, assigned in ascending order.
    /// 
    /// @see java.io.ObjectOutputStream.HandleTable
    /// </summary>
    internal class JavaHandleTable
    {
        /// <summary>
        /// Maps: Hash Value -> Candidate Handle Value
        /// </summary>
        private List<int> spine = new List<int>();
        /// <summary>
        /// Maps: Handle Value -> Next Candidate Handle Value 
        /// </summary>
        private List<int> next = new List<int>();
        /// <summary>
        /// Maps: Handle Value -> Associated Object
        /// </summary>
        private List<object> objects = new List<object>();
        /// <summary>
        /// Returns the number of mappings in table.
        /// </summary>
        private int Size
        {
            get
            {
                return this.next.Count;
            }
        }

        internal JavaHandleTable(int initialCount)
        {
            // Set start values
            for (int i = 0; i < initialCount; ++i)
            {
                this.spine.Add(-1);
                this.next.Add(0); // default(int)
                this.objects.Add(null);
            }
        }

        /// <summary>
        /// Assigns next available handle to specified object.
        /// Returns handle value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int Assign(object obj)
        {
            int handle = this.Size;
            int index = this.Hash(obj) % this.spine.Count;

            this.objects.Add(obj);
            this.next.Add(this.spine[index]);

            if (index >= this.spine.Count)
            {
                this.spine.Add(handle);
            }
            else
            {
                this.spine[index] = handle;
            }

            return handle++;
        }

        /// <summary>
        /// Returns handle connected with given object; -1 if there is no mapping for specifieid object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int Lookup(object obj)
        {
            if (this.Size == 0)
            {
                return -1;
            }

            int index = this.Hash(obj) % this.spine.Count;
            for (int i = this.spine[index]; i >= 0; i = next[i])
            {
                if (this.objects[i] == obj)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns hash value from given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int Hash(object obj)
        {
            // Java magic bit operation
            return obj.GetHashCode() & 0x7FFFFFFF;
        }
    }
}