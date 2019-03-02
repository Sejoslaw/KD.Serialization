using Xunit;

namespace JavaTests
{
    public class CustomObjectTests : BaseTest
    {
        [Fact]
        public void Write_SimpleObject()
        {
            var sample = new ExampleClass
            {
                someByte = 19
            };

            this.Write("SimpleObject.bin", serializer =>
            {
                serializer.WriteObject(sample);
                serializer.Flush();
            });
        }

        [Fact]
        public void Write_SerializableObject()
        {
            var sample = new ExampleSerializableClass
            {
                someByte = 26
            };

            this.Write("SerializableObject.bin", serializer =>
            {
                serializer.WriteObject(sample);
                serializer.Flush();
            });
        }
    }
}
