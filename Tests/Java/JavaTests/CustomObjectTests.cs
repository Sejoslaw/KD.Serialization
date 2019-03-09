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
                variable = 19,
                flag = true
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
                variable = 19,
                flag = true
            };

            this.Write("SerializableObject.bin", serializer =>
            {
                serializer.WriteObject(sample);
                serializer.Flush();
            });
        }
    }
}
