using Microsoft.Extensions.Logging;

namespace Ten.DDD.EFCore
{
    public class EfLoggerProvider: ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EfLogger(categoryName);
        public void Dispose() { }
    }
}