using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using dummy_elixir_lab_event.utils;
using System.Collections.Concurrent;

namespace dummy_elixir_lab_event.modules.elixir_event.command.slash.add.servuces
{
    public sealed class AddService
    {
        private DiscordClient _client;
        private ConcurrentDictionary<ulong, EmojiData> _data;
        public AddService(DiscordClient client)
        {
            _client = client;
            _data = new ConcurrentDictionary<ulong, EmojiData>();
            _client.ComponentInteractionCreated += Add_Component_Interaction_Created;
            _client.ModalSubmitted += Add_Modal_Submutted;
        }

        public DiscordMessageBuilder EmojiBuilderMessage(DiscordUser user, string name, Uri url, DiscordEmoji? emoji = null)
        {
            UpdateOrCreateData(user, name, url);
            if (emoji is null)
            {
                return new DiscordMessageBuilder().AddEmbed(
                    DummyUtils.ToDiscordEmbed(new DummyEmbed(
                        color: "#E3B56E",
                        title: new("Criando um novo emoji para você!"),
                        description: "‎",
                        thumbnail: url.OriginalString,
                        fields: new List<DummyEmbedField>(){
                            new("<:emoji:1108777590979829792> Nome", $"```:{name}:```", true),
                            new("<:id:1108426946536280095> Identidade", $"```1234567890123456789```", true),
                            new("<:mention:1108684750337609799> Menção", $"```<:{name}:123456789012345678>```", true)
                        }
                        ))).AddComponents(new List<DiscordActionRowComponent>()
                        {
                            new(new[]{
                                new DiscordButtonComponent(ButtonStyle.Secondary, $"add_name_emoji", "Editar Nome", false, new DiscordComponentEmoji(DiscordEmoji.FromGuildEmote(_client, 1110978177062408282))),
                                new DiscordButtonComponent(ButtonStyle.Success, $"add_new_emoji", "Enviar", false)
                            })
                        });
            }
            _data.TryRemove(user.Id, out _);
            return new DiscordMessageBuilder().AddEmbed(
                DummyUtils.ToDiscordEmbed(new DummyEmbed(
                    color: "#E3B56E",
                    title: new("Emoji criado!"),
                    description: "‎",
                    thumbnail: emoji.Url,
                    fields: new List<DummyEmbedField>(){
                        new("<:emoji:1108777590979829792> Nome", $"```{emoji.Name}```", true),
                        new("<:id:1108426946536280095> Identidade", $"```{emoji.Id}```", true),
                        new("<:mention:1108684750337609799> Menção", $"```{emoji}```", true)
                    })));
        }
        private Task Add_Component_Interaction_Created(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            if (args.Id.StartsWith("add_"))
            {
                _ = Task.Run(async () =>
                {
                    switch (args.Id)
                    {
                        case "add_name_emoji":
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, new DiscordInteractionResponseBuilder()
                            .WithCustomId($"add_mod_edit")
                            .WithTitle("Adicione uma nome para seu emoji")
                            .AddComponents(new[] { new TextInputComponent("Digite um nome para se emoji", "add_emoji_name", "Não pode conter espaços em branco", _data[args.User.Id].Name, true, TextInputStyle.Short, 2, 32) }));
                            break;
                        case "add_new_emoji":
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(await AddNewEmoji()));
                            break;
                    }
                    async Task<DiscordMessageBuilder> AddNewEmoji()
                    {
                        DiscordEmoji? emoji = null;
                        try
                        {
                            string path = string.Empty;
                            using (HttpClient httpClient = new())
                            {
                                string extension = Path.GetExtension(_data[args.User.Id].Url.GetLeftPart(UriPartial.Path));
                                path = Path.Combine(new[] { DummyUtils.DirectoryTemp, $"{_data[args.User.Id].Name}{extension}" });
                                var imageBytes = await httpClient.GetByteArrayAsync(_data[args.User.Id].Url);
                                await File.WriteAllBytesAsync(path, imageBytes);

                                using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
                                {
                                    emoji = await args.Guild.CreateEmojiAsync(_data[args.User.Id].Name, fs);
                                    fs.Close();
                                    fs.Dispose();
                                }
                                httpClient.CancelPendingRequests();
                                httpClient.Dispose();
                            }
                            File.Delete(path);
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
                        if (emoji is null)
                        {
                            return new DiscordMessageBuilder().AddEmbed(
                                DummyUtils.ToDiscordEmbed(new DummyEmbed(
                                    color: "#FF0000",
                                    description: "> **Ocorreu um erro ao tentar criar um novo emoji**")));
                        }
                        return EmojiBuilderMessage(args.User, _data[args.User.Id].Name, _data[args.User.Id].Url, emoji);
                    }
                });
            }
            return Task.CompletedTask;
        }
        private Task Add_Modal_Submutted(DiscordClient client, ModalSubmitEventArgs args)
        {
            if (args.Interaction.Data.CustomId == "add_mod_edit")
            {
                _ = Task.Run(async () =>
                {
                    string name = args.Values["add_emoji_name"];
                    if (name.Contains(' ')) { name = name.Replace(' ', '_'); }
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(EmojiBuilderMessage(args.Interaction.User, name, _data[args.Interaction.User.Id].Url)));
                });
            }
            return Task.CompletedTask;
        }
        private void UpdateOrCreateData(DiscordUser user, string name, Uri url)
        {
            if (_data.ContainsKey(user.Id)) { _data[user.Id].UpdateName(name); }
            else { _data.TryAdd(user.Id, new(name, url)); }
        }
    }
    internal sealed class EmojiData
    {
        public string Name { get; private set; }
        public Uri Url { get; }
        public EmojiData(string name, Uri url)
        {
            Name = name;
            Url = url;
        }
        public void UpdateName(string name) => Name = name;
    }
}
