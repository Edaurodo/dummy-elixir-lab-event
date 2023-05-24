namespace dummy_elixir_lab_event.utils.attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumPropertyNameAttribute : Attribute
    {
        public string String { get; }
        public EnumPropertyNameAttribute(string attributename)
        {
            String = attributename;
        }
    }
}
