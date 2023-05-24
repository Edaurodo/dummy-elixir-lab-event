using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dummy_elixir_lab_event.core.config
{
    public sealed class DummyConfig
    {
        // 0 - Trace | 1 - Debug | 2 - Information | 3 - Warning | 4 - Error | 5 - Critical | 6 - None
        [JsonProperty("loglevel")]
        public LogLevel MinimumLogLevel { get; }
        [JsonProperty("discord")]
        public DummyDiscordConfig Discord { get; }

        public DummyConfig(LogLevel? logLevel = null, DummyDiscordConfig? discord = null)
        {
            MinimumLogLevel = logLevel ?? LogLevel.Information;
            Discord = discord ?? new DummyDiscordConfig(null, null);
        }
    }
}
