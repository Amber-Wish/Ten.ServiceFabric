using System;
using Microsoft.Extensions.Logging;

namespace Ten.DDD.EFCore
{
    public class EfLogger: ILogger
    {
        private readonly string _categoryName;

        public EfLogger(string categoryName) => this._categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if (_categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
                && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);
                Console.WriteLine("<------------------ sql start ------------------>");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("sql: ");
                Console.ResetColor();
                Console.Write(logContent);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("<------------------ sql end ------------------>");
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}