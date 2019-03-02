using KD.Serialization.Java;
using System;
using System.IO;

namespace JavaTests
{
    public class BaseTest
    {
        protected void Write(string filePath, Action<JavaSerializer> action)
        {
            var fi = new FileInfo(filePath);
            using (FileStream stream = fi.OpenWrite())
            {
                using (var serializer = new JavaSerializer(stream))
                {
                    action(serializer);
                }
            }
        }
    }
}
