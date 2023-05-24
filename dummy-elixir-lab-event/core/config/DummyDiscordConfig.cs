using Newtonsoft.Json;

namespace dummy_elixir_lab_event.core.config
{
    public sealed class DummyDiscordConfig
    {
        [JsonProperty("token")]
        public string Token { get; }

        [JsonProperty("message_cache_size")]
        public int MessageCacheSize { get; }

        public DummyDiscordConfig(string? token, int? messagecachesize)
        {
            Token = string.IsNullOrWhiteSpace(token) ? "insert your bot token here" : token;
            MessageCacheSize = messagecachesize ?? 512;
        }
    }
}
