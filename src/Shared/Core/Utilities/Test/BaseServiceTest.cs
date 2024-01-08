using Core.CrossCuttingConcerns.Logging;
using Moq;

namespace Core.Utilities.Test
{
    public class BaseServiceTest<T> where T : class
    {
        public readonly Mock<T> serviceMock;
        public readonly Mock<ILogger> loggerMock;

        public BaseServiceTest()
        {
            serviceMock = new Mock<T>();
            loggerMock = new Mock<ILogger>();
        }
    }
}
