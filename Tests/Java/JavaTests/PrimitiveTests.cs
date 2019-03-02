using Xunit;

namespace JavaTests
{
    public class PrimitiveTests : BaseTest
    {
        [Fact]
        public void Write_Int()
        {
            this.Write("SerializedDotNetInt.bin", serializer =>
            {
                serializer.WriteInt(9876);
                serializer.Flush();
            });
        }

        [Fact]
        public void Write_Float()
        {
            this.Write("SerializedDotNetFloat.bin", serializer =>
            {
                serializer.WriteFloat(9876);
                serializer.Flush();
            });
        }

        [Fact]
        public void Write_Double()
        {
            this.Write("SerializedDotNetDouble.bin", serializer =>
            {
                serializer.WriteDouble(9876);
                serializer.Flush();
            });
        }

        [Fact]
        public void Write_Short()
        {
            this.Write("SerializedDotNetShort.bin", serializer =>
            {
                serializer.WriteShort(9876);
                serializer.Flush();
            });
        }

        [Fact]
        public void Write_Long()
        {
            this.Write("SerializedDotNetLong.bin", serializer =>
            {
                serializer.WriteLong(9876);
                serializer.Flush();
            });
        }
    }
}
