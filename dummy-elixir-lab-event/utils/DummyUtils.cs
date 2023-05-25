using DSharpPlus.Entities;
using dummy_elixir_lab_event.modules.elixir_event.command.slash.info.enums;
using dummy_elixir_lab_event.utils.attributes;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace dummy_elixir_lab_event.utils
{
    public static class DummyUtils
    {
        public static Encoding UTF8 = Encoding.UTF8;
        public static string DirectoryConfig = Path.Combine(new[] { Directory.GetCurrentDirectory(), "configuration" });
        public static string DirectoryTemp = Path.Combine(new[] { Directory.GetCurrentDirectory(), "temp" });
        private static Dictionary<DiscordPermissions, string> PermissionStrings { get; set; }

        static DummyUtils()
        {
            PermissionStrings = new Dictionary<DiscordPermissions, string>();
            Type typeFromHandle = typeof(DiscordPermissions);
            TypeInfo typeInfo = typeFromHandle.GetTypeInfo();
            foreach (DiscordPermissions item in Enum.GetValues(typeFromHandle).Cast<DiscordPermissions>())
            {
                string xsv = item.ToString();
                EnumPropertyNameAttribute customAttribute = typeInfo.DeclaredMembers.FirstOrDefault((MemberInfo xm) => xm.Name == xsv).GetCustomAttribute<EnumPropertyNameAttribute>();
                PermissionStrings[item] = customAttribute.String;
            }
        }
        public static string ToPermissionString(this DiscordPermissions perm)
        {
            if (perm == DiscordPermissions.None)
            {
                return PermissionStrings[perm];
            }
            perm &= DiscordPermissions.All;
            IEnumerable<string> source = from xkvp in PermissionStrings
                                         where xkvp.Key != DiscordPermissions.None && (perm & xkvp.Key) == xkvp.Key
                                         select xkvp.Value;
            return string.Join(", ", source.OrderBy((string xs) => xs));
        }
        public static DiscordEmbed ToDiscordEmbed(DummyEmbed embed)
        {
            DiscordEmbedBuilder eb = new DiscordEmbedBuilder();
            eb.WithColor(new DiscordColor(embed.Color));
            _ = embed.Author.Name is null && embed.Author.Image is null ? null : eb.WithAuthor(embed.Author.Name, embed.Author.Url, embed.Author.Image);
            _ = embed.Title.Value is null ? null : eb.WithTitle(embed.Title.Value);
            _ = embed.Title.Url is null ? null : eb.WithUrl(embed.Title.Url);
            _ = embed.Description is null ? null : eb.WithDescription(embed.Description);
            _ = embed.Thumbnail is null ? null : eb.WithThumbnail(embed.Thumbnail);
            _ = embed.Image is null ? null : eb.WithImageUrl(embed.Image);
            _ = embed.Footer.Value is null && embed.Footer.Image is null ? null : eb.WithFooter(embed.Footer.Value, embed.Footer.Image);
            _ = embed.Footer.Timestamp is null || embed.Footer.Timestamp is false ? null : eb.WithTimestamp(DateTime.Now.ToLocalTime());
            if (embed.Fields.Count() > 0)
            {
                embed.Fields.ToList().ForEach(field => { eb.AddField(field.Title, field.Value, field.Inline ?? false); });
            }
            return eb.Build();
        }
        public static bool ValidateColorHex(string hex)
        {
            if (hex.Length > 5 && hex.Length < 8)
            {
                if (Regex.IsMatch(hex, "[#][a-fA-F0-9]{6}|[a-fA-F0-9]{6}")) { return true; }
                else { return false; }
            }
            else { return false; }
        }
    }
}