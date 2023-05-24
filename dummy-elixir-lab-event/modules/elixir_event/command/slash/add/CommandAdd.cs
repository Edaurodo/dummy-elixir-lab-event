using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.info.services;
using dummy_elixir_lab_event.utils;
using System.Text.RegularExpressions;

namespace dummy_elixir_lab_event.modules.elixir_event.command.slash.add
{
    [SlashModuleLifespan(SlashModuleLifespan.Transient)]
    [SlashCommandGroup("Adicionar", "Comando para adicionar coisas ao servidor")]
    public sealed class CommandAdd : ApplicationCommandModule
    {
        private AddService _add;
        public CommandAdd(AddService service) => _add = service;
        public override async Task<bool> BeforeSlashExecutionAsync(InteractionContext ctx)
        {
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageEmojis))
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(DummyUtils.
                    ToDiscordEmbed(new DummyEmbed(color: "#FF0000", description: "> **É necessário que você tenha a permissão `Gerenciar Emojis e Stricker` para utilizar este comando!**"))).AsEphemeral(true));
                return false;
            }
            if (!ctx.Guild.CurrentMember.Permissions.HasPermission(Permissions.ManageEmojis))
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(DummyUtils.
                    ToDiscordEmbed(new DummyEmbed(color: "#FF0000", description: "> **Eu preciso da permissão `Gerenciar Emojis e Stricker` para adicionar um emoji para você!**"))).AsEphemeral(true));
                return false;
            }
            return true;
        }
        [SlashCommand("Emoji", "Um novo emoji ao servidor")]
        public async Task AddEmoji(InteractionContext ctx, [Option("emoji", "Copie um emoji de outro servidor")] string? emojiArg = null, [Option("Imagem", "envie a imagem do emoji tamanho recomendado (128px X 128px)")] DiscordAttachment? file = null)
        {
            try
            {
                await ctx.DeferAsync(true);
                if (!string.IsNullOrWhiteSpace(emojiArg))
                {
                    string emoji = emojiArg.Substring(0, emojiArg.IndexOf('>') + 1);
                    if (!string.IsNullOrWhiteSpace(emoji) && Regex.IsMatch(emoji, "(<(a?):(\\w\\S+):(\\d{1,32})>)"))
                    {
                        string extension = emoji.Substring(1, 1).ToLower() == "a" ? ".gif" : ".png";
                        string name = emoji.Substring(emoji.IndexOf(':') + 1, emoji.LastIndexOf(':') - emoji.IndexOf(':') - 1);
                        string urlString = $"https://cdn.discordapp.com/emojis/{emoji.Substring(emoji.LastIndexOf(':') + 1, emoji.IndexOf('>') - emoji.LastIndexOf(':') - 1)}{extension}";

                        if (Uri.TryCreate(urlString, UriKind.Absolute, out Uri url))
                        {
                            await ctx.EditResponseAsync(new DiscordWebhookBuilder(_add.EmojiBuilderMessage(ctx.User, name, url)));
                            return;
                        }
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(DummyUtils.ToDiscordEmbed(new DummyEmbed(color: "#FF0000", description: "> **Algo de errado aconteceu, tente novamente!**"))));
                        return;
                    }
                }
                if (file != null)
                {
                    if (file.MediaType.StartsWith("image/"))
                    {
                        string name = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder(_add.EmojiBuilderMessage(ctx.User, name, new Uri(file.Url))));
                        return;
                    }
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(DummyUtils.ToDiscordEmbed(new DummyEmbed(color: "#FF0000", description: "> **O Arquivo deve ser do tipo imagem!**"))));
                    return;
                }
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(DummyUtils.ToDiscordEmbed(new DummyEmbed(color: "#FF0000", description: "> **Você tem que passar alguma argumento para este comando**"))));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Mesage: ");
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(ex.Message);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Source: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Source);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Target Site: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.TargetSite);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Stack Trace: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.StackTrace + "\n");
                Console.ResetColor();
            }
        }
    }
}
