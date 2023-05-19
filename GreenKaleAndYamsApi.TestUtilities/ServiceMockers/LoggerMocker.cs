using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace GreenKaleAndYamsApi.TestUtilities.ServiceMockers {
	public class LoggerMocker<T> : IDisposable {
		public readonly Mock<ILogger<T>> Mock = new Mock<ILogger<T>>();


		public void Dispose() {
			Mock.VerifyAll();
		}


		public void Setup_Log(LogLevel logLevel = LogLevel.Information, string messageContains = null) {
			if (messageContains == null) {
				Mock.Setup(
					x => x.Log(
						It.Is<LogLevel>(l => l == logLevel),
						It.IsAny<EventId>(),
						It.IsAny<It.IsAnyType>(),
						It.IsAny<Exception>(),
						It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
					)
				);
			} else {
				Func<object, Type, bool> state = (v, t) => v.ToString().Contains(messageContains);
				Mock.Setup(
					x => x.Log(
						It.Is<LogLevel>(l => l == logLevel),
						It.IsAny<EventId>(),
						It.Is<It.IsAnyType>((v, t) => state(v, t)),
						It.IsAny<Exception>(),
						It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
					)
				);
			}
		}

		public void VerifyLogging(LogLevel logLevel = LogLevel.Information, string messageContains = null, Times? times = null) {
			times ??= Times.Once();
			Func<object, Type, bool> state = (v, t) => v.ToString().Contains(messageContains);

			Mock.Verify(
				x => x.Log(
					It.Is<LogLevel>(l => l == logLevel),
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => state(v, t)),
					It.IsAny<Exception>(),
					It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
				(Times)times
			);
		}
	}
}
