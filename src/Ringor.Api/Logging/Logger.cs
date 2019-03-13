using System;
using Dalion.Ringor.Utils.Logging;

namespace Dalion.Ringor.Api.Logging {
    public class Logger<TContext> : ILogger<TContext>, IDisposable {
        private readonly Serilog.ILogger _logger;

        public Logger(Serilog.ILogger logger) {
            _logger = logger?.ForContext<TContext>() ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Dispose() {
            (_logger as IDisposable)?.Dispose();
        }

        public void Verbose(string messageTemplate) {
            _logger.Verbose(messageTemplate);
        }

        public void Verbose<T1>(string messageTemplate, T1 propertyValue) {
            _logger.Verbose(messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Verbose(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues) {
            _logger.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate) {
            _logger.Verbose(exception, messageTemplate);
        }

        public void Verbose<T1>(Exception exception, string messageTemplate, T1 propertyValue) {
            _logger.Verbose(exception, messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues) {
            _logger.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate) {
            _logger.Debug(messageTemplate);
        }

        public void Debug<T1>(string messageTemplate, T1 propertyValue) {
            _logger.Debug(messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug(string messageTemplate, params object[] propertyValues) {
            _logger.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate) {
            _logger.Debug(exception, messageTemplate);
        }

        public void Debug<T1>(Exception exception, string messageTemplate, T1 propertyValue) {
            _logger.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues) {
            _logger.Debug(exception, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate) {
            _logger.Information(messageTemplate);
        }

        public void Information<T1>(string messageTemplate, T1 propertyValue) {
            _logger.Information(messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(string messageTemplate, params object[] propertyValues) {
            _logger.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate) {
            _logger.Information(exception, messageTemplate);
        }

        public void Information<T1>(Exception exception, string messageTemplate, T1 propertyValue) {
            _logger.Information(exception, messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues) {
            _logger.Information(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate) {
            _logger.Warning(messageTemplate);
        }

        public void Warning<T1>(string messageTemplate, T1 propertyValue) {
            _logger.Warning(messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Warning(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning(string messageTemplate, params object[] propertyValues) {
            _logger.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate) {
            _logger.Warning(exception, messageTemplate);
        }

        public void Warning<T1>(Exception exception, string messageTemplate, T1 propertyValue) {
            _logger.Warning(exception, messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues) {
            _logger.Warning(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate) {
            _logger.Error(messageTemplate);
        }

        public void Error<T1>(string messageTemplate, T1 propertyValue) {
            _logger.Error(messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(string messageTemplate, params object[] propertyValues) {
            _logger.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate) {
            _logger.Error(exception, messageTemplate);
        }

        public void Error<T1>(Exception exception, string messageTemplate, T1 propertyValue) {
            _logger.Error(exception, messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues) {
            _logger.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate) {
            _logger.Fatal(messageTemplate);
        }

        public void Fatal<T1>(string messageTemplate, T1 propertyValue) {
            _logger.Fatal(messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues) {
            _logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate) {
            _logger.Fatal(exception, messageTemplate);
        }

        public void Fatal<T1>(Exception exception, string messageTemplate, T1 propertyValue) {
            _logger.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) {
            _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) {
            _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues) {
            _logger.Fatal(exception, messageTemplate, propertyValues);
        }
    }
}