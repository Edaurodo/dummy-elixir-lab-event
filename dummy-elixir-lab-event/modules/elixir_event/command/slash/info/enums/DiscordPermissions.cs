using dummy_elixir_lab_event.utils.attributes;

namespace dummy_elixir_lab_event.modules.elixir_event.command.slash.info.enums
{
    [Flags]
    public enum DiscordPermissions : long
    {
        [EnumPropertyName("None")]
        None = 0x0L,

        [EnumPropertyName("Todas permissoões")]
        All = 0x47FFFFFFFFFEL,

        [EnumPropertyName("Criar convites")]
        CreateInstantInvite = 0x1L,

        [EnumPropertyName("Kickar membros")]
        KickMembers = 0x2L,

        [EnumPropertyName("Banir membros")]
        BanMembers = 0x4L,

        [EnumPropertyName("Administrador")]
        Administrator = 0x8L,

        [EnumPropertyName("Gerenciar canais")]
        ManageChannels = 0x10L,

        [EnumPropertyName("Gerenciar servidor")]
        ManageGuild = 0x20L,

        [EnumPropertyName("Adicionar reações")]
        AddReactions = 0x40L,

        [EnumPropertyName("Ver auditoria")]
        ViewAuditLog = 0x80L,

        [EnumPropertyName("Voz prioritária")]
        PrioritySpeaker = 0x100L,

        [EnumPropertyName("Fazer live")]
        Stream = 0x200L,

        [EnumPropertyName("Ver canais")]
        AccessChannels = 0x400L,

        [EnumPropertyName("Enviar menssagem")]
        SendMessages = 0x800L,

        [EnumPropertyName("Enviar mensagem TTS")]
        SendTtsMessages = 0x1000L,

        [EnumPropertyName("Gerenciar mensagens")]
        ManageMessages = 0x2000L,

        [EnumPropertyName("Inserir links")]
        EmbedLinks = 0x4000L,

        [EnumPropertyName("Anexar arquivo")]
        AttachFiles = 0x8000L,

        [EnumPropertyName("Ler histórico de mensagens")]
        ReadMessageHistory = 0x10000L,

        [EnumPropertyName("Mencionar everyone")]
        MentionEveryone = 0x20000L,

        [EnumPropertyName("Usar emoji externos")]
        UseExternalEmojis = 0x40000L,

        [EnumPropertyName("Ver análise do Servidor")]
        ViewGuildInsights = 0x80000L,

        [EnumPropertyName("Conectar")]
        UseVoice = 0x100000L,

        [EnumPropertyName("Falar")]
        Speak = 0x200000L,

        [EnumPropertyName("Mutar membros")]
        MuteMembers = 0x400000L,

        [EnumPropertyName("Ensurdecer membros")]
        DeafenMembers = 0x800000L,

        [EnumPropertyName("Mover membros")]
        MoveMembers = 0x1000000L,

        [EnumPropertyName("Usar detecção de voz")]
        UseVoiceDetection = 0x2000000L,

        [EnumPropertyName("Mudar nickname")]
        ChangeNickname = 0x4000000L,

        [EnumPropertyName("gerenciar nicknames")]
        ManageNicknames = 0x8000000L,

        [EnumPropertyName("gerenciar cargos")]
        ManageRoles = 0x10000000L,

        [EnumPropertyName("Gerenciar webhooks")]
        ManageWebhooks = 0x20000000L,

        [EnumPropertyName("gerenciar emojis")]
        ManageEmojis = 0x40000000L,

        [EnumPropertyName("Usar comandos")]
        UseApplicationCommands = 0x80000000L,

        [EnumPropertyName("Pedir para falar")]
        RequestToSpeak = 0x100000000L,

        [EnumPropertyName("Gerenciar eventos")]
        ManageEvents = 0x200000000L,

        [EnumPropertyName("Gerenciar threads")]
        ManageThreads = 0x400000000L,

        [EnumPropertyName("Criar threads publica")]
        CreatePublicThreads = 0x800000000L,

        [EnumPropertyName("Criar threads privada")]
        CreatePrivateThreads = 0x1000000000L,

        [EnumPropertyName("Usar stickers externos")]
        UseExternalStickers = 0x2000000000L,

        [EnumPropertyName("Enviar mensagens em threads")]
        SendMessagesInThreads = 0x4000000000L,

        [EnumPropertyName("Usar atividades")]
        StartEmbeddedActivities = 0x8000000000L,

        [EnumPropertyName("Castigar membros")]
        ModerateMembers = 0x10000000000L,

        [EnumPropertyName("Ver analize de monetização")]
        ViewCreatorMonetizationAnalytics = 0x20000000000L,

        [EnumPropertyName("Usar soundpad")]
        UseSoundBoard = 0x40000000000L,

        [EnumPropertyName("Enviar mensagem de voz")]
        SendVoiceMessages = 0x400000000000L
    }
}
