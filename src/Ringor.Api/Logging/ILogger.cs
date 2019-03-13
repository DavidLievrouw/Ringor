using System;

namespace Dalion.Ringor.Api.Logging {
    /// <summary>
    ///     The methods on <see cref="ILogger{TContext}" /> are guaranteed never to throw exceptions.
    /// </summary>
    /// <typeparam name="TContext">
    ///     The type of the context that should be used in the logging context.
    ///     This is usually the same as the class in which ILogger is used.
    ///     <remarks>
    ///         The multiple overloads when only 1, 2 or 3 property values are provided is for performance reasons.
    ///         See https://github.com/serilog/serilog/issues/479 for more info
    ///     </remarks>
    /// </typeparam>
    public interface ILogger<TContext> {
        /// <summary>
        ///     Write a log event with the verbose level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Verbose("Staring into space, wondering if we're alone.");
        /// </example>
        void Verbose(string messageTemplate);

        /// <summary>
        ///     Write a log event with the verbose level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose("Staring into space, wondering if we're alone.");
        /// </example>
        void Verbose<T>(string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the verbose level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose("Staring into space, wondering if we're alone.");
        /// </example>
        void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the verbose level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose("Staring into space, wondering if we're alone.");
        /// </example>
        void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the verbose level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose("Staring into space, wondering if we're alone.");
        /// </example>
        void Verbose(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the verbose level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </example>
        void Verbose(Exception exception, string messageTemplate);

        /// <summary>
        ///     Write a log event with the verbose level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </example>
        void Verbose<T>(Exception exception, string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the verbose level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </example>
        void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the verbose level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </example>
        void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the verbose level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </example>
        void Verbose(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the debug level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug(string messageTemplate);

        /// <summary>
        ///     Write a log event with the debug level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug<T>(string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the debug level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the debug level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the debug level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the debug level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>logger.Debug(ex, "Swallowing a mundane exception.");</example>
        void Debug(Exception exception, string messageTemplate);

        /// <summary>
        ///     Write a log event with the debug level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>logger.Debug(ex, "Swallowing a mundane exception.");</example>
        void Debug<T>(Exception exception, string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the debug level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>logger.Debug(ex, "Swallowing a mundane exception.");</example>
        void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the debug level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>logger.Debug(ex, "Swallowing a mundane exception.");</example>
        void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the debug level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>logger.Debug(ex, "Swallowing a mundane exception.");</example>
        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the information level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information(string messageTemplate);

        /// <summary>
        ///     Write a log event with the information level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information<T>(string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the information level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the information level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the information level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the information level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information(Exception exception, string messageTemplate);

        /// <summary>
        ///     Write a log event with the information level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information<T>(Exception exception, string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the information level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the information level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the information level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the warning level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning(string messageTemplate);

        /// <summary>
        ///     Write a log event with the warning level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning<T>(string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the warning level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the warning level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the warning level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the warning level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning(Exception exception, string messageTemplate);

        /// <summary>
        ///     Write a log event with the warning level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning<T>(Exception exception, string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the warning level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the warning level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the warning level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the error level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error(string messageTemplate);

        /// <summary>
        ///     Write a log event with the error level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error<T>(string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the error level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the error level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the error level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the error level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>
        ///     logger.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error(Exception exception, string messageTemplate);

        /// <summary>
        ///     Write a log event with the error level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error<T>(Exception exception, string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the error level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the error level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the error level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     logger.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the fatal level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>logger.Fatal("Process terminating.");</example>
        void Fatal(string messageTemplate);

        /// <summary>
        ///     Write a log event with the fatal level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>logger.Fatal("Process terminating.");</example>
        void Fatal<T>(string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the fatal level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>logger.Fatal("Process terminating.");</example>
        void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the fatal level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>logger.Fatal("Process terminating.");</example>
        void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the fatal level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>logger.Fatal("Process terminating.");</example>
        void Fatal(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the fatal level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example>logger.Fatal(ex, "Process terminating.");</example>
        void Fatal(Exception exception, string messageTemplate);

        /// <summary>
        ///     Write a log event with the fatal level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example>logger.Fatal(ex, "Process terminating.");</example>
        void Fatal<T>(Exception exception, string messageTemplate, T propertyValue);

        /// <summary>
        ///     Write a log event with the fatal level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example>logger.Fatal(ex, "Process terminating.");</example>
        void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        /// <summary>
        ///     Write a log event with the fatal level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example>logger.Fatal(ex, "Process terminating.");</example>
        void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        /// <summary>
        ///     Write a log event with the fatal level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>logger.Fatal(ex, "Process terminating.");</example>
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);
    }
}