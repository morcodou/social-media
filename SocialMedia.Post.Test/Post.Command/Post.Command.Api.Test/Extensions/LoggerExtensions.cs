namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void VerifyLoggerInformation<T>(this ILogger<T> logger, string message) =>
            logger.VerifyLogger(LogLevel.Information, message);

        public static void VerifyLoggerError<T>(this ILogger<T> logger, string message) =>
            logger.VerifyLogger(LogLevel.Error, message);

        public static void VerifyLoggerWarning<T>(this ILogger<T> logger, string message) =>
            logger.VerifyLogger(LogLevel.Warning, message);

        private static void VerifyLogger<T>(this ILogger<T> logger, LogLevel logLevel, string message)
        {
            Mock.Get(logger)
                .Verify(x => x.Log(logLevel,
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((m, _) => $"{m}".Contains(message)),
                                It.IsAny<Exception>(),
                                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.AtLeastOnce());
        }
    }
}