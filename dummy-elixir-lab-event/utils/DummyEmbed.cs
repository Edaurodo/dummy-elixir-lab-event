using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace dummy_elixir_lab_event.utils
{
    public sealed class DummyEmbed
    {
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        private string _color;

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        private string? _description;

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        private string? _thumbnail;

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        private string? _image;

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        private DummyEmbedAuthor? _author;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        private DummyEmbedTitle? _title;

        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        private DummyEmbedFooter? _footer;

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        private IEnumerable<DummyEmbedField>? _fields;

        [JsonIgnore]
        public string? Color
        {
            get => _color;
            set => _color = Regex.IsMatch(value ?? "#2B2D31", "[#][a-fA-F0-9]{6}|[a-fA-F0-9]{6}") ? value ?? "#2B2D31" : "#2B2D31";
        }
        [JsonIgnore]
        public string? Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) || value.Length > 4096 ? null : value;
        }
        [JsonIgnore]
        public string? Thumbnail
        {
            get => _thumbnail;
            set => _thumbnail = Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        [JsonIgnore]
        public string? Image
        {
            get => _image;
            set => _image = Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        [JsonIgnore]
        public DummyEmbedAuthor? Author
        {
            get => _author;
            set => _author = value is null || (value.Name is null && value.Image is null) ? new DummyEmbedAuthor(null, null, null) : value;
        }
        [JsonIgnore]
        public DummyEmbedTitle? Title
        {
            get => _title;
            set => _title = value is null || value.Value is null ? new DummyEmbedTitle(null, null) : value;
        }
        [JsonIgnore]
        public DummyEmbedFooter? Footer
        {
            get => _footer;
            set => _footer = value is null || (value.Value is null && value.Image is null && (value.Timestamp is null || value.Timestamp is false)) ? new DummyEmbedFooter(null, null, false) : value;
        }
        [JsonIgnore]
        public IEnumerable<DummyEmbedField>? Fields
        {
            get => _fields;
            set => _fields = value is null || value.Count() > 25 ? new List<DummyEmbedField>() : value.ToList().FindAll(_ => !(_ is null || _.Title is null || _.Value is null || _.Inline is null));
        }
        [JsonIgnore]
        public static ImmutableDictionary<string, string> Colors { get; } = new Dictionary<string, string>() {
            {"#2B2D31", "Neutro" },
            {"#F0FFFF", "Azure"},
            {"#6495ED", "CornflowerBlue"},
            {"#FF7F50", "Coral"},
            {"#E9967A", "DarkSalmon"},
            {"#FF1493", "DeepPink"},
            {"#00BFFF", "DeepSkyBlue"},
            {"#B22222", "FireBrick"},
            {"#FF69B4", "HotPink"},
            {"#CD5C5C", "IndianRed"},
            {"#F0E68C", "Khaki"},
            {"#20B2AA", "LightSeaGreen"},
            {"#B0C4DE", "LightSteelBlue"},
            {"#FFE4E1", "MistyRose"},
            {"#FFE4B5", "Moccasin"},
            {"#98FB98", "PaleGreen"},
            {"#AFEEEE", "PaleTurquoise"},
            {"#DB7093", "PaleVioletRed"},
            {"#FFDAB9", "PeachPuff"},
            {"#B0E0E6", "PowderBlue"},
            {"#FA8072", "Salmon"},
            {"#008080", "Teal"},
            {"#FF6347", "Tomato"},
            {"#FFFF00", "Yellow"},
        }.ToImmutableDictionary();
        public DummyEmbed(string? color = null, string? description = null, string? thumbnail = null, string? image = null, DummyEmbedAuthor? author = null, DummyEmbedTitle? title = null, DummyEmbedFooter? footer = null, IEnumerable<DummyEmbedField>? fields = null)
        {
            Color = color;
            Description = description;
            Thumbnail = thumbnail;
            Image = image;
            Author = author;
            Title = title;
            Footer = footer;
            Fields = fields;
        }
    }
    public sealed class DummyEmbedAuthor
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        private string? _name;
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        private string? _image;
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        private string? _url;
        [JsonIgnore]
        public string? Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) || value.Length > 256 ? null : value;
        }
        [JsonIgnore]
        public string? Image
        {
            get => _image;
            set => _image = Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        [JsonIgnore]
        public string? Url
        {
            get => _url;
            set => _url = !(this._name is null) && Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        public DummyEmbedAuthor(string? name = null, string? image = null, string? url = null)
        {
            Name = name;
            Image = image;
            Url = url;
        }
    }
    public sealed class DummyEmbedTitle
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        private string? _value;
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        private string? _url;
        [JsonIgnore]
        public string? Value
        {
            get => _value;
            set => _value = string.IsNullOrWhiteSpace(value) || value.Length > 256 ? null : value;
        }
        [JsonIgnore]
        public string? Url
        {
            get => _url;
            set => _url = !(this._value is null) && Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        public DummyEmbedTitle(string? value = null, string? url = null)
        {
            Value = value;
            Url = url;
        }
    }
    public sealed class DummyEmbedField
    {
        [JsonProperty("title")]
        private string? _title;
        [JsonProperty("value")]
        private string? _value;
        [JsonProperty("inline")]
        private bool? _inline;
        [JsonIgnore]
        public string? Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) || value.Length > 256 ? null : value;
        }
        [JsonIgnore]
        public string? Value
        {
            get => _value;
            set => _value = string.IsNullOrWhiteSpace(value) || this._title is null || value.Length > 1024 ? null : value;
        }
        [JsonIgnore]
        public bool? Inline
        {
            get => _inline;
            set => _inline = value is null || this._title is null || this._value is null ? null : value;
        }
        public DummyEmbedField(string title, string value, bool inline)
        {
            Title = title;
            Value = value;
            Inline = inline;
        }
        public static DummyEmbedField Empty(bool inline) => new("‎", "‎", inline);
    }
    public sealed class DummyEmbedFooter
    {
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        private string? _value;
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        private string? _image;
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public bool? _timestamp;
        [JsonIgnore]
        public string? Value
        {
            get => _value;
            set => _value = string.IsNullOrEmpty(value) || value.Length > 2048 ? null : value;
        }
        [JsonIgnore]
        public string? Image
        {
            get => _image;
            set => _image = Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        [JsonIgnore]
        public bool? Timestamp
        {
            get => _timestamp;
            set => _timestamp = value is null || value is false ? null : value;
        }
        public DummyEmbedFooter(string? value = null, string? image = null, bool? timestamp = null)
        {
            Value = value;
            Image = image;
            Timestamp = timestamp;
        }
    }
    public sealed class DummyEmbedSerializable
    {
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        private string _color;

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        private string? _description;

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        private string? _thumbnail;

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        private string? _image;

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        private DummyEmbedAuthor? _author;

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        private DummyEmbedTitle? _title;

        [JsonProperty("footer", NullValueHandling = NullValueHandling.Ignore)]
        private DummyEmbedFooter? _footer;

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        private IEnumerable<DummyEmbedField>? _fields;
        [JsonIgnore]
        public string? Color
        {
            get => _color;
            private set => _color = Regex.IsMatch(value ?? "#2B2D31", "[#][a-fA-F0-9]{6}|[a-fA-F0-9]{6}") ? value ?? "#2B2D31" : "#2B2D31";
        }
        [JsonIgnore]
        public string? Description
        {
            get => _description;
            private set => _description = string.IsNullOrWhiteSpace(value) || value.Length > 4096 ? null : value;
        }
        [JsonIgnore]
        public string? Thumbnail
        {
            get => _thumbnail;
            private set => _thumbnail = Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        [JsonIgnore]
        public string? Image
        {
            get => _image;
            private set => _image = Uri.TryCreate(value, UriKind.Absolute, out _) ? value : null;
        }
        [JsonIgnore]
        public DummyEmbedAuthor? Author
        {
            get => _author;
            private set => _author = value is null || (value.Name is null && value.Image is null) ? null : value;
        }
        [JsonIgnore]
        public DummyEmbedTitle? Title
        {
            get => _title;
            private set => _title = value is null || value.Value is null ? null : value;
        }
        [JsonIgnore]
        public DummyEmbedFooter? Footer
        {
            get => _footer;
            private set => _footer = value is null || (value.Value is null && value.Image is null && value.Timestamp is false) ? null : value;
        }

        [JsonIgnore]
        public IEnumerable<DummyEmbedField>? Fields
        {
            get => _fields;
            private set => _fields = value is null || value.Count() > 25 ? null :
                value.ToList().FindAll(_ => !(_ is null || _.Title is null || _.Value is null || _.Inline is null)).Count > 0 ?
                value.ToList().FindAll(_ => !(_ is null || _.Title is null || _.Value is null || _.Inline is null)) : null;
        }
        public DummyEmbedSerializable(DummyEmbed embed)
        {
            Color = embed.Color;
            Description = embed.Description;
            Thumbnail = embed.Thumbnail;
            Image = embed.Image;
            Author = embed.Author;
            Title = embed.Title;
            Footer = embed.Footer;
            Fields = embed.Fields;
        }
    }
}
