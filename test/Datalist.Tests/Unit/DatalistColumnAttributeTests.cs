using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnAttributeTests
    {
        #region DatalistColumnAttribute(Int32 position)

        [Fact]
        public void DatalistColumnAttribute_Position()
        {
            Assert.Equal(-5, new DatalistColumnAttribute(-5).Position);
        }

        #endregion
    }
}
