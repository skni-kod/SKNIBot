using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SKNIBot.Core.Database.Logger
{
    public class DbLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger();
        }

        public void AddProvider(ILoggerProvider provider)
        {

        }
    }
}
