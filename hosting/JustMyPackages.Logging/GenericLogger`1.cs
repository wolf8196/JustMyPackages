using Serilog.Events;

namespace Serilog
{
    internal sealed class GenericLogger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public GenericLogger(ILogger logger)
        {
            _logger = logger.ForContext<T>();
        }

        public ILogger<TClass> WithContext<TClass>()
        {
            return new GenericLogger<TClass>(_logger);
        }

        public void Write(LogEvent logEvent) => _logger.Write(logEvent);
    }
}