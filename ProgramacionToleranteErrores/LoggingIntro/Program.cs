using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder =>
{
    //// You can change the minimum log level to control which log messages are displayed. For example, you can set it to LogLevel.Information to only show information, warning, error, and critical messages, while ignoring trace and debug messages.
    //builder.AddConsole().SetMinimumLevel(LogLevel.Trace);
    //// You can also set the minimum log level to LogLevel.None to disable all logging, or to LogLevel.Critical to only show critical messages.
    //.SetMinimumLevel(LogLevel.None);

    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true; // This option enables the inclusion of scopes in the log output, which can provide additional context for log messages.
        options.SingleLine = true; // This option formats log messages to be displayed on a single line, which can make logs easier to read in certain scenarios.
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss "; // This option specifies the format of the timestamp included in log messages, allowing you to customize how timestamps are displayed.
    }).SetMinimumLevel(LogLevel.Trace);
});

var logger = loggerFactory.CreateLogger<Program>();


logger.LogTrace("This is a trace log message."); // This log level is typically used for detailed debugging information and is usually disabled in production environments.
logger.LogDebug("This is a debug log message."); // This log level is typically used for detailed debugging information and is usually disabled in production environments.

int res = 5 + 10;

logger.LogInformation("Result: {0}", res); // This log level is typically used for general application events and is usually enabled in production environments.
logger.LogWarning("This is a warning log message."); // This log level is typically used for warning messages and is usually enabled in production environments.
logger.LogError("This is an error log message."); // This log level is typically used for error messages and is usually enabled in production environments.
logger.LogCritical("This is a critical log message."); // This log level is typically used for critical messages and is usually enabled in production environments.

loggerFactory.Dispose();
