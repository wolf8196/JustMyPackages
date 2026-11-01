using Serilog.Events;

namespace Serilog
{
    internal sealed class GenericLogger<T> : ILogger<T>
    {
        private readonly ILogger logger;

        public GenericLogger(ILogger logger)
        {
            this.logger = logger.ForContext<T>();
        }

        public ILogger<TClass> WithContext<TClass>()
        {
            return new GenericLogger<TClass>(logger);
        }

        public void Write(LogEvent logEvent) => logger.Write(logEvent);
    }
}