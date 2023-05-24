using DSharpPlus.Interactivity;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using dummy_elixir_lab_event.core.config;
using DSharpPlus.Interactivity.Extensions;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.info;
using Microsoft.Extensions.DependencyInjection;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.info.services;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.add;

namespace dummy_elixir_lab_event.core
{
    public sealed class DummyBot
    {
        public DummyConfig Config { get; }
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; }
        public SlashCommandsExtension SlashCommands { get; }
        public IServiceProvider Services { get; }

        public DummyBot(DummyConfig config)
        {
            Config = config;

            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Config.Discord.Token,
                MessageCacheSize = Config.Discord.MessageCacheSize,
                LogTimestampFormat = "dd-MM-yyyy | HH:mm:ss",
                MinimumLogLevel = Config.MinimumLogLevel,
                Intents = DiscordIntents.AllUnprivileged,
            });

            Services = new ServiceCollection()
                .AddSingleton(new InfoService(Client))
                .AddSingleton(new AddService(Client))
                .BuildServiceProvider();

            SlashCommands = Client.UseSlashCommands(new() { Services = this.Services });
            Interactivity = Client.UseInteractivity();

            SlashCommands.RegisterCommands<CommandInfo>();
            SlashCommands.RegisterCommands<CommandAdd>();
        }
        public async Task<Task> StartAsync()
        {
            await Client.ConnectAsync();
            return Task.CompletedTask;
        }
    }
}
