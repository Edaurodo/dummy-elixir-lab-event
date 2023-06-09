﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.info.services;

namespace dummy_elixir_lab_event.modules.elixir_event.command.slash.info
{
    [SlashModuleLifespan(SlashModuleLifespan.Transient)]
    [SlashCommandGroup("Info", "Verifique as informações sobre alguma item especifico")]
    public sealed class CommandInfo : ApplicationCommandModule
    {
        private InfoService _infoService;

        public CommandInfo(InfoService service) => _infoService = service;

        [SlashCommand("Server", "Verifique algumas informações sobre o servidor")]
        public async Task GuildInfo(InteractionContext ctx)
        {
            await ctx.DeferAsync(true);
            await ctx.EditResponseAsync(new(_infoService.GetGuildDisplayInfo(ctx.Guild)));
        }

        [SlashCommand("User", "Obetenha inforamações de um úsuario")]
        public async Task GetUserInfo(InteractionContext ctx, [Option("Usuario", "escolha uma usuário")] DiscordUser? user = null)
        {
            await ctx.DeferAsync(true);
            if (user is null)
            {
                await Handler(ctx.User);
                return;
            }
            await Handler(user);
            return;

            async Task Handler(DiscordUser? item)
            {
                if (ctx.Guild.Members.TryGetValue(item.Id, out var member))
                {
                    await ctx.EditResponseAsync(new(_infoService.GetUserDisplayInfo(member, item)));
                }
                else { await ctx.EditResponseAsync(new(_infoService.GetUserDisplayInfo(null, item))); }
            }
        }
    }
}