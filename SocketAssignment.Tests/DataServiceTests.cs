using ServerApp.Services;

namespace SocketAssignment.Tests
{
    public class DataServiceTests
    {
        [Fact]
        public void GetValue_Valid_ReturnsCorrect()
        {
            var service = new DataService();

            var result = service.GetValue("SetA", "One");

            Assert.Equal(1, result);
        }
    }
}