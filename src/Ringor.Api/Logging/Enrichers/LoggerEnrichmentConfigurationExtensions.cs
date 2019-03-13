using Serilog;
using Serilog.Configuration;

namespace Dalion.Ringor.Api.Logging.Enrichers {
    public static class LoggerEnrichmentConfigurationExtensions {
        public static LoggerConfiguration WithDemystifiedStackTraces(this LoggerEnrichmentConfiguration enrichmentConfiguration) {
            return enrichmentConfiguration.With(new DemystifiedStackTraceEnricher());
        }
    }
}