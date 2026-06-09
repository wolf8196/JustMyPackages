namespace Serilog
{
    /// <summary>
    /// Wrapper for Serilog ILogger that allows to attach class name.
    /// </summary>
    /// <typeparam name="T">Class that uses the logger.</typeparam>
    public interface ILogger<out T> : ILogger
    {
        ILogger<TClass> WithContext<TClass>();
    }
}
