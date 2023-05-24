using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.info.enums;
using dummy_elixir_lab_event.utils;

namespace dummy_elixir_lab_event.modules.elixir_event.command.slash.info.services
{
    public sealed class InfoService
    {
        private DiscordClient _client;

        public InfoService(DiscordClient client)
        {
            _client = client;
            _client.ComponentInteractionCreated += Info_Component_Interaction_Created;
            _client.ModalSubmitted += Info_Modal_Submit;
        }
        public DiscordMessageBuilder GetUserDisplayInfo(DiscordMember? member, DiscordUser user)
        {
            List<DiscordEmbed> embeds = new() {
                GetUserEmbedInfo()
            };
            if (member is not null)
            {
                embeds.Add(GetMemberEmbedInfo());
                return new DiscordMessageBuilder().AddEmbeds(embeds).AddComponents(
                    new List<DiscordActionRowComponent>()
                    {
                        new(new[] { new DiscordUserSelectComponent("info_sel_member", "Selecione um membro")}),
                        new(new[] { new DiscordButtonComponent(ButtonStyle.Success, $"info_btnUserPerm_{member.Id}", "Permissões", false, new(DiscordEmoji.FromGuildEmote(_client, 1108710536042000414)))})
                    });
            }
            return new DiscordMessageBuilder()
                .AddEmbeds(embeds);

            DiscordEmbed GetUserEmbedInfo()
            {
                return DummyUtils.ToDiscordEmbed(new(
                    color: "#E3B56E",
                    author: new("Informações sobre o usuário"),
                    title: new($"{user.Username}", $"https://discord.com/users/{user.Id}"),
                    thumbnail: user.AvatarUrl,
                    fields: new List<DummyEmbedField>()
                    {
                        GetUserBadgeField(),
                        new("<:human:1108742021675503698> Nome",$"```{user.Username}#{user.Discriminator}```", true),
                        GetUserCreationTimeField(),
                    },
                    footer: new($"{user.Id}", "https://i.imgur.com/DXsu2kf.png")));
                DummyEmbedField GetUserBadgeField()
                {
                    return new("<:discord:1110326015328260277> Insignias", GetUserBadges(user.Flags), true);

                    string GetUserBadges(UserFlags? flags)
                    {
                        int.TryParse(flags.ToString(), out int cast);
                        Enum userflags = (UserBadges)cast;
                        string value = string.Empty;
                        foreach (Enum flag in Enum.GetValues(userflags.GetType()))
                        {
                            if (userflags.HasFlag(flag))
                            {
                                if (string.IsNullOrEmpty(flag.ToString())) { return "Nenhuma Insignia"; }
                                else { value += BadgeParse(flag.ToString()); }
                            }
                        }
                        if (string.IsNullOrWhiteSpace(value) || flags is null) { return "```Sem Badges```"; }
                        else
                        {
                            var values = value.Split(' ');
                            value = string.Empty;
                            int i = 0;
                            values.ToList().ForEach(x =>
                            {
                                if (i != 3)
                                {
                                    value += $"{x} ";
                                    i++;
                                }
                                else
                                {
                                    value += $"\n{x} ";
                                    i = 1;
                                }
                            });
                        }
                        return value;

                        string BadgeParse(string item)
                        {
                            switch (item)
                            {
                                case "STAFF":
                                    return "<:discord_staff:1110675619966095600> ";
                                case "PARTNE":
                                    return "<:discord_partner:1110675618418401321> ";
                                case "HYPESQUAD":
                                    return "<:hype_squad_events:1110675631802429440> ";
                                case "BUG_HUNTER_LEVEL_1":
                                    return "<:bug_hunter:1110675612584120480> ";
                                case "HYPESQUAD_ONLINE_HOUSE_1":
                                    return "<:hype_squad_bravery:1110675627448750140> ";
                                case "HYPESQUAD_ONLINE_HOUSE_2":
                                    return "<:hype_squad_brilliance:1110675628887396443> ";
                                case "HYPESQUAD_ONLINE_HOUSE_3":
                                    return "<:hype_squad_balance:1110675624881831966> ";
                                case "PREMIUM_EARLY_SUPPORTER":
                                    return "<:early_supporter:1110675623254425621> ";
                                case "TEAM_PSEUDO_USER":
                                    return "";
                                case "BUG_HUNTER_LEVEL_2":
                                    return "<:bug_hunter_level2:1110675614177960056> ";
                                case "VERIFIED_BOT":
                                    return "";
                                case "VERIFIED_DEVELOPER":
                                    return "<:verified_developer:1110675739243712563> ";
                                case "CERTIFIED_MODERATOR":
                                    return "<:discord_moderator:1110675615272673392> ";
                                case "BOT_HTTP_INTERACTIONS":
                                    return "";
                                case "ACTIVE_DEVELOPER":
                                    return "<:active_developer:1110675610189168782> ";
                                default:
                                    return "";
                            }

                        }
                    }
                }
                DummyEmbedField GetUserCreationTimeField()
                {
                    var time = user.CreationTimestamp;
                    string dataString = $"```{time:dd} de {time:MMMM} de {time:yyyy}\nàs {time:HH:mm:ss}```";
                    return new("<:calendar:1108749572341235872> Criou conta em", dataString, true);
                }
            }
            DiscordEmbed GetMemberEmbedInfo()
            {
                return DummyUtils.ToDiscordEmbed(new(
                    color: "#E3B56E",
                    author: new("Informações sobre o membro"),
                    title: new(),
                    fields: new List<DummyEmbedField>()
                    {
                        new ("<:mention:1108684750337609799> Menção", $"<@{member.Id}>", true),
                        new("<:human:1108742021675503698> NickName",$"```{member.DisplayName}```", true),
                        GetMemberEntryTimeField(),
                        GetMemberRolesField(),
                        DummyEmbedField.Empty(false),
                        GetMemberCuriositiesField()
                    }));
                DummyEmbedField GetMemberEntryTimeField()
                {
                    var time = member.JoinedAt;
                    string dataString = $"```{time:dd} de {time:MMMM} de {time:yyyy}\nàs {time:HH:mm:ss}```";
                    return new("<:calendar:1108749572341235872> Membro desde", dataString, true);
                }
                DummyEmbedField GetMemberRolesField()
                {
                    string roles = "```O membro não tem nenhum cargo```";
                    var temp = member.Roles.ToList();
                    if (temp.Count > 0)
                    {
                        int i = 0;
                        roles = string.Empty;
                        temp.Sort((DiscordRole x, DiscordRole y) => { return x.Position < y.Position ? 1 : x.Position > y.Position ? -1 : 0; });
                        temp.ForEach(role =>
                        {
                            if (i != 3)
                            {
                                roles += $"<@&{role.Id}>, ";
                                i++;
                            }
                            else
                            {
                                roles += $"\n<@&{role.Id}>, ";
                                i = 1;
                            }
                        });
                    }
                    return new("<:role:1108699479370104842> Cargos", roles, false);
                }
                DummyEmbedField GetMemberCuriositiesField()
                {
                    string curiosities = $"> **{member.DisplayName}** criou sua conta à:\n";
                    curiosities += GetElapsedTime(DateTimeOffset.Now.Subtract(member.CreationTimestamp));
                    curiosities += $"\n> **{member.DisplayName}** é membro do servidor à:\n";
                    curiosities += GetElapsedTime(DateTimeOffset.Now.Subtract(member.JoinedAt));

                    return new("<:moderation:1108710533080825936> Curiosodades", curiosities, false);

                    string GetElapsedTime(TimeSpan time)
                    {
                        TimeSpan OneYear = new TimeSpan(365, 0, 0, 0, 0);
                        TimeSpan OneMonth = new TimeSpan(30, 0, 0, 0, 0);
                        TimeSpan OneDay = new TimeSpan(1, 0, 0, 0, 0);
                        TimeSpan OneHour = new TimeSpan(0, 1, 0, 0, 0);
                        TimeSpan OneMinute = new TimeSpan(0, 0, 1, 0, 0);
                        TimeSpan OneSecond = new TimeSpan(0, 0, 0, 1, 0);

                        double years = Math.Floor(time / OneYear);
                        double months = Math.Floor((time - (years * OneYear)) / OneMonth);
                        double days = Math.Floor((time - ((years * OneYear) + (months * OneMonth))) / OneDay);
                        double hours = Math.Floor((time - ((years * OneYear) + (months * OneMonth) + (days * OneDay))) / OneHour);
                        double minutes = Math.Floor((time - ((years * OneYear) + (months * OneMonth) + (days * OneDay) + (hours * OneHour))) / OneMinute);
                        double seconds = Math.Floor((time - ((years * OneYear) + (months * OneMonth) + (days * OneDay) + (hours * OneHour) + (minutes * OneMinute))) / OneSecond);

                        string value = "```\n";
                        if (years > 0) { value += $"{years} anos, "; }
                        if (months > 0) { value += $"{months} mês, "; }
                        if (days > 0) { value += $"{days} dias, "; }
                        if (hours > 0) { value += $"{hours} horas, "; }
                        if (minutes > 0) { value += $"{minutes} minutos, "; }
                        value += $"e {seconds} segundos.\n```";
                        return value;
                    }
                }
            }
        }
        public DiscordMessageBuilder GetGuildDisplayInfo(DiscordGuild guild)
        {
            return new DiscordMessageBuilder().AddEmbed(DummyUtils.ToDiscordEmbed(
                new(
                    color: "#E3B56E",
                    author: new(image: guild.IconUrl),
                    thumbnail: guild.IconUrl,
                    title: new(guild.Name, guild.IconUrl),
                    fields: new List<DummyEmbedField>() {
                        GetInfoServer(), GetInfoCreationTime(), GetInfoOwner(),
                        DummyEmbedField.Empty(false),
                        GetInfoMembers(), GetInfoChannels(), GetInfoRoles(),
                    },
                    footer: new(value: $"‎{guild.Id}", "https://i.imgur.com/DXsu2kf.png"))))
                .AddComponents(GetComponentsDefault(InfoType.Guild));

            DummyEmbedField GetInfoServer()
            {
                int premiumCount = guild.PremiumSubscriptionCount ?? 0;
                string isVerified = "Não";
                string isParterned = "Não";

                string guildInfo = string.Empty;

                if (premiumCount > 0) { guildInfo += $"<:boosting:1108814124269453373> Impulsos `{premiumCount}`\n"; }
                if (guild.Features.Count() > 0)
                {
                    if (guild.Features.Contains("VERIFIED")) { isVerified = "Sim"; }
                    if (guild.Features.Contains("PARTNERED")) { isParterned = "Sim"; }
                }
                guildInfo += $"<:verified:1109168737774481478> Vericado: **{isVerified}**\n";
                guildInfo += $"<:partner:1109163572845621359> Parceiro: **{isParterned}**";

                return new($"<:guild:1108798748924063814> Guilda", guildInfo, true);
            }
            DummyEmbedField GetInfoCreationTime()
            {
                var time = guild.CreationTimestamp;
                string dataString = $"`{time:dd} de {time:MMMM} de {time:yyyy}\nàs {time:HH:mm:ss}`";
                return new("<:calendar:1108749572341235872> Criação", dataString, true);
            }
            DummyEmbedField GetInfoOwner()
            {
                return new("<:guild_owner:1108418460213579908> Dono(a)", $"<:mention:1108684750337609799> <@{guild.OwnerId}>\n<:id:1108426946536280095>`{guild.OwnerId}`", true);
            }
            DummyEmbedField GetInfoMembers()
            {
                int botCount = 0;
                int humancount = 0;
                int memberCount = 0;
                List<DiscordMember> members = guild.GetAllMembersAsync().GetAwaiter().GetResult().ToList();

                members.ForEach(member =>
                {
                    if (member.IsBot) { botCount++; }
                    else { humancount++; }
                    memberCount++;
                });
                string totalMembersString = $"<:human:1108742021675503698> Humanos: `{humancount:#.###.###.###}`\n";
                if (botCount > 0) { totalMembersString += $"<:bot:1108713517395214377> Bot's: `{botCount:#.###.###.###}`"; }

                return new($"<:members:1108699528816754730> Membros `{memberCount:#.###.###.###}`", totalMembersString, true);
            }
            DummyEmbedField GetInfoChannels()
            {
                int voiceChannel = 0;
                int voiceChannelLock = 0;
                int textChannel = 0;
                int textChannelLock = 0;
                List<DiscordChannel> channels = guild.Channels.Values.ToList();

                channels.ForEach(channel =>
                {
                    switch (channel.Type)
                    {
                        case ChannelType.Text:
                            textChannel++;
                            if (isPrivate(channel)) { textChannelLock++; }
                            break;
                        case ChannelType.Voice:
                            voiceChannel++;
                            if (isPrivate(channel)) { voiceChannelLock++; }
                            break;
                    }
                });

                string totalChannelsString =
                    $"<:text_channel:1108423965770260540> Texto: `{textChannel}` <:lock:1108688806019862528> `({textChannelLock})`\n" +
                    $"<:voice_channel:1108424038642090006> Voz `{voiceChannel}` <:lock:1108688806019862528> `({voiceChannelLock})`";

                return new($"<:channels_search:1108687105493844018> Canais `{textChannel + voiceChannel}`", totalChannelsString, true);
                bool isPrivate(DiscordChannel item)
                {
                    var permission = item.PermissionOverwrites.ToList();
                    if (permission.Exists(_ => _.Type == OverwriteType.Role && guild.Roles.TryGetValue(_.Id, out var role) && role == guild.EveryoneRole && _.CheckPermission(Permissions.AccessChannels) == PermissionLevel.Denied)) { return true; }
                    return false;
                }
            }
            DummyEmbedField GetInfoRoles()
            {
                int totalRoleCount = 0;
                int commonRoleCount = 0;
                int moderationRoleCount = 0;
                List<DiscordRole> roles = guild.Roles.Values.ToList();

                roles.ForEach(role =>
                {
                    if (!role.IsManaged)
                    {
                        if (HasModerationPermissions(role.Permissions)) { moderationRoleCount++; }
                        else { commonRoleCount++; }
                        totalRoleCount++;
                    }
                });

                string totalRolesString =
                    $"<:social:1108710536042000414> Comun: `{commonRoleCount:#.###.###.###}`\n" +
                    $"<:moderation:1108710533080825936> Moderação: `{moderationRoleCount:#.###.###.###}`";

                return new($"<:role:1108699479370104842> Cargos `{totalRoleCount:#.###.###.###}`", totalRolesString, true);
            }
        }
        public DiscordMessageBuilder GetRoleDisplayInfo(DiscordGuild guild, int page = 1)
        {
            List<DiscordRole> roles = guild.Roles.Values.ToList();
            roles.Sort((DiscordRole x, DiscordRole y) => { return x.Position < y.Position ? 1 : x.Position > y.Position ? -1 : 0; });
            int roleCount = roles.Count;
            int itemPerPage = 15;
            int pageCount = (roleCount / itemPerPage) == 0 ? 1 : (roleCount / itemPerPage) * itemPerPage == roleCount ? (roleCount / itemPerPage) : (roleCount / itemPerPage) + 1;

            List<DiscordRole> currentItems = GetCurrentPageItems();
            string stringItems = string.Empty;
            int count = (page - 1) * itemPerPage == 0 ? 1 : (page - 1) * itemPerPage + 1;

            currentItems.ForEach(_ =>
            {
                stringItems += $"`{count:000}` - <@&{_.Id}>\n";
                count++;
            });

            var components = new List<DiscordActionRowComponent>() {
                new (new[]{GetComponentsDefault(InfoType.Role).ElementAt(0) }),
                new (new[]{new DiscordRoleSelectComponent("info_sel_inforole", "Selecione um cargo")}),
                new(new[]{
                    new DiscordButtonComponent(ButtonStyle.Primary, "info_btn_prevPage", null, page == 1, new(DiscordEmoji.FromGuildEmote(_client, 1110237367459139646))),
                    new DiscordButtonComponent(ButtonStyle.Primary, "info_btn_firstorlast", null, pageCount == 1, new(DiscordEmoji.FromGuildEmote(_client, 1110247539782996251))),
                    new DiscordButtonComponent(ButtonStyle.Danger, "info_btn_close", null, true, new(DiscordEmoji.FromGuildEmote(_client, 1110249878694674562))),
                    new DiscordButtonComponent(ButtonStyle.Primary, "info_btn_toPage", null, pageCount == 1, new(DiscordEmoji.FromGuildEmote(_client, 1110571775223402596))),
                    new DiscordButtonComponent(ButtonStyle.Primary, "info_btn_nextPage", null, page == pageCount, new(DiscordEmoji.FromGuildEmote(_client, 1110237369254293534))),
                })};

            return new DiscordMessageBuilder().AddEmbed(DummyUtils.ToDiscordEmbed(
                new(
                    color: "#E3B56E",
                    author: new(name: $"Página {page:00} de {pageCount:00}"),
                    title: new($"Lista de cargos de {guild.Name}[{roleCount}]"),
                    description: stringItems)))
                .AddComponents(components);

            List<DiscordRole> GetCurrentPageItems()
            {
                int startSearch = page == 1 ? 0 : (page - 1) * itemPerPage;
                int breakSearch = page != pageCount ? itemPerPage : roleCount % itemPerPage == 0 ? itemPerPage : roleCount % itemPerPage;
                return roles.GetRange(startSearch, breakSearch);
            }
        }
        public DiscordMessageBuilder GetRoleInfo(DiscordRole role)
        {
            return new DiscordMessageBuilder().AddEmbed(DummyUtils.ToDiscordEmbed(
                new(color: "#E3B56E", fields: new List<DummyEmbedField>()
                {
                    GetRoleName(), GetRoleTimestamp(), GetRoleColor(),
                    GetRoleIsMentionable(), GetRoleIsModeration(), GetRoleIsManaged()
                },
                footer: new(value: $"‎{role.Id}", "https://i.imgur.com/DXsu2kf.png"))))
                .AddComponents(new List<DiscordActionRowComponent>() {
                    new(new[]{new DiscordRoleSelectComponent("info_sel_role", "Selecione um cargo") }),
                    new(new[]{new DiscordButtonComponent(ButtonStyle.Success, $"info_btnRolePerm_{role.Id}", "Permissões", false, new(DiscordEmoji.FromGuildEmote(_client, 1108710536042000414)))})
                });

            DummyEmbedField GetRoleName()
            {
                return new("<:universal_icon:1108683892669562961> Nome", $" `{role.Name}`", true);
            }
            DummyEmbedField GetRoleTimestamp()
            {
                var time = role.CreationTimestamp;
                string dataString = $"`{time:dd} de {time:MMMM} de {time:yyyy}\nàs {time:HH:mm:ss}`";
                return new("<:calendar:1108749572341235872> Criação", dataString, true);
            }
            DummyEmbedField GetRoleColor()
            {
                string colorString = role.Color.Value == 0 ? "`padrão`" : $"`#{Convert.ToHexString(new[] { role.Color.R, role.Color.G, role.Color.B })}`";
                return new("<:sign:1110328508888129546> Cor", $"<:text_channel:1108423965770260540> {colorString}", true);
            }
            DummyEmbedField GetRoleIsMentionable()
            {
                string isMentionable = role.IsMentionable ? "`Sim`" : "`Não`";

                return new("<:announcement:1110310836657995887> Mencionavél", $"<:mention:1108684750337609799> {isMentionable}", true);
            }
            DummyEmbedField GetRolePosition()
            {
                return new("<:universal_icon:1108683892669562961> Posição", $"<:static:1108784308199620629> `{role.Position}`", true);
            }
            DummyEmbedField GetRoleIsModeration()
            {
                string isModeration = HasModerationPermissions(role.Permissions) ? "Sim" : "Não";
                return new("<:moderation_shield:1110321527754334259> Moderação", $"<:moderation:1108710533080825936> `{isModeration}`", true);
            }
            DummyEmbedField GetRoleIsManaged()
            {
                string isManaged = role.IsManaged ? "Sim" : "Não";
                return new("<:discord:1110326015328260277> Cargo de Bot", $"<:bot:1108713517395214377> `{isManaged}` ", true);
            }
        }
        private IEnumerable<DiscordComponent> GetComponentsDefault(InfoType type)
        {
            List<DiscordSelectComponentOption> infoOptions = new(){
                new("Servidor", "info_opt_infoGuild", null, type == InfoType.Guild, new(DiscordEmoji.FromGuildEmote(_client, 1109919538033741985))),
                new("Cargos", "info_opt_infoRole", null, type == InfoType.Role, new(DiscordEmoji.FromGuildEmote(_client, 1108699479370104842)))
            };

            return new List<DiscordComponent>() {
            new DiscordSelectComponent("info_sel_default", null, infoOptions)};
        }
        private bool HasModerationPermissions(Permissions permissions)
        {
            if (permissions.HasPermission(Permissions.All | Permissions.Administrator | Permissions.BanMembers | Permissions.DeafenMembers | Permissions.KickMembers | Permissions.ManageChannels | Permissions.ManageEmojis | Permissions.ManageEvents | Permissions.ManageGuild | Permissions.ManageMessages | Permissions.ManageNicknames | Permissions.ManageRoles | Permissions.ManageThreads | Permissions.ManageWebhooks | Permissions.ModerateMembers | Permissions.MoveMembers | Permissions.MuteMembers | Permissions.ViewAuditLog)) { return true; }
            return false;
        }
        private Task Info_Component_Interaction_Created(DiscordClient client, ComponentInteractionCreateEventArgs args)
        {
            if (args.Id.StartsWith("info_"))
            {
                _ = Task.Run(async () =>
                {
                    string aux = args.Message.Embeds[0].Author?.Name.ToLower();
                    int.TryParse(aux?.Substring(aux.IndexOf(' ') + 1, 2), out int currentPage);
                    int.TryParse(aux?.Substring(aux.LastIndexOf(' ') + 1, 2), out int pageCount);

                    switch (args.Id)
                    {
                        case "info_sel_default":
                            switch (args.Values[0])
                            {
                                case "info_opt_infoGuild":
                                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetGuildDisplayInfo(args.Guild)));
                                    break;
                                case "info_opt_infoRole":
                                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleDisplayInfo(args.Guild, 1)));
                                    break;
                            }
                            break;
                        case "info_sel_inforole":
                            if (ulong.TryParse(args.Values[0], out ulong id))
                            {
                                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder(GetRoleInfo(args.Guild.GetRole(id))).AsEphemeral(true));
                            }
                            break;
                        case "info_sel_role":
                            if (ulong.TryParse(args.Values[0], out id))
                            {
                                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleInfo(args.Guild.GetRole(id))));
                            }
                            break;
                        case "info_sel_member":
                            if (ulong.TryParse(args.Values[0], out id))
                            {
                                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetUserDisplayInfo(await args.Guild.GetMemberAsync(id), await client.GetUserAsync(id))));
                            }
                            break;
                        case "info_btn_prevPage":
                            currentPage--;
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleDisplayInfo(args.Guild, currentPage)));
                            break;
                        case "info_btn_firstorlast":
                            if (currentPage == pageCount) { await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleDisplayInfo(args.Guild, 1))); }
                            else { await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleDisplayInfo(args.Guild, pageCount))); }
                            break;
                        case "info_btn_toPage":
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, new DiscordInteractionResponseBuilder()
                                .WithCustomId($"info_mod_toPage_{pageCount}")
                                .WithTitle("Digite o número da página")
                                .AddComponents(new[] { new TextInputComponent("Número da página", $"info_mod_page", "apenas números", $"{currentPage}", true, TextInputStyle.Short, 1, 3) }));
                            break;
                        case "info_btn_nextPage":
                            currentPage++;
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleDisplayInfo(args.Guild, currentPage)));
                            break;
                    }
                    if (args.Id.StartsWith("info_btnRolePerm_"))
                    {
                        if (ulong.TryParse(args.Id.Substring(args.Id.LastIndexOf('_') + 1), out ulong id))
                        {
                            DiscordRole role = args.Guild.GetRole(id);
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                .AddEmbed(DummyUtils.ToDiscordEmbed(new(
                                    title: new($"Permissões de `{role.Name}`"),
                                    description: GetPermissions(role.Permissions)
                                    ))).AsEphemeral(true));
                        }
                    }
                    if (args.Id.StartsWith("info_btnUserPerm_"))
                    {
                        if (ulong.TryParse(args.Id.Substring(args.Id.LastIndexOf('_') + 1), out ulong id))
                        {
                            DiscordMember member = await args.Guild.GetMemberAsync(id);
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                .AddEmbed(DummyUtils.ToDiscordEmbed(new(
                                    title: new($"Permissões de `{member.DisplayName}`"),
                                    description: GetPermissions(member.Permissions)
                                    ))).AsEphemeral(true));
                        }
                    }
                });
            }
            return Task.CompletedTask;
        }
        private Task Info_Modal_Submit(DiscordClient client, ModalSubmitEventArgs args)
        {
            _ = Task.Run(async () =>
            {

                if (args.Interaction.Data.CustomId.StartsWith("info_mod_toPage"))
                {
                    string aux = args.Interaction.Data.CustomId;
                    int.TryParse(aux?.Substring(aux.LastIndexOf('_') + 1), out int pageCount);

                    if (args.Values.TryGetValue("info_mod_page", out string value))
                    {
                        if (int.TryParse(value, out int page))
                        {
                            if (page > pageCount || page < 1) { await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("> **Não exista tal página!**").AsEphemeral(true)); }
                            else { await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new(GetRoleDisplayInfo(args.Interaction.Guild, page))); }
                        }
                        else
                        {
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("> **Numero inválido!**").AsEphemeral(true));
                        }
                    }
                }
            });
            return Task.CompletedTask;
        }
        private string GetPermissions(Permissions perms)
        {
            string value = string.Empty;
            var permissions = (DiscordPermissions)perms;
            permissions.ToPermissionString().Split(',').ToList().ForEach(_ =>
            {
                value += $"✔ {_}\n";
            });

            if (string.IsNullOrWhiteSpace(value) || value.Contains("✔ None"))
            {
                value = "✘ Nenhuma permissão expecificada!";
            }
            return $"```yaml\n{value}```";
        }
    }
}

/*
0x0000000000000002
0x0000000000000004
0x0000000000000008
0x0000000000000010
0x0000000000000020
0x0000000000000040
0x0000000000000080
0x0000000000000100
0x0000000000000200
0x0000000000000400
0x0000000000000800
0x0000000000001000
0x0000000000002000
0x0000000000004000
0x0000000000008000
0x0000000000010000
0x0000000000020000
0x0000000000040000
0x0000000000080000
0x0000000000100000
0x0000000000200000
0x0000000000400000
0x0000000000800000
0x0000000001000000
0x0000000002000000
0x0000000004000000
0x0000000008000000
0x0000000010000000
0x0000000020000000
0x0000000040000000
0x0000000080000000
0x0000000100000000
0x0000000200000000
0x0000000400000000
0x0000000800000000
0x0000001000000000
0x0000002000000000
0x0000004000000000
0x0000008000000000
0x0000010000000000
0x0000020000000000
0x0000040000000000
0x0000400000000000
 */