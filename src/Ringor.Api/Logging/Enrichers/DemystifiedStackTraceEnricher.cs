using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Dalion.Ringor.Api.Logging.Enrichers {
    public class DemystifiedStackTraceEnricher : ILogEventEnricher {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) {
            logEvent.Exception?.Demystify();
        }
    }
}