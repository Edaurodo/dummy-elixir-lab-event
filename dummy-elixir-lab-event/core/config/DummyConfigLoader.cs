using dummy_elixir_lab_event.utils;
using Newtonsoft.Json;

namespace dummy_elixir_lab_event.core.config
{
    public sealed class DummyConfigLoader
    {
        private string _configFile;
        private DummyConfigLoader() => _configFile = Path.Combine(new[] { DummyUtils.DirectoryConfig, "dummy.config.json" });

        public static async Task<DummyConfig> LoadConfigAsync()
        {
            var loader = new DummyConfigLoader();

            _ = Directory.Exists(DummyUtils.DirectoryConfig) ? null : Directory.CreateDirectory(DummyUtils.DirectoryConfig);
            _ = Directory.Exists(DummyUtils.DirectoryTemp) ? null : Directory.CreateDirectory(DummyUtils.DirectoryTemp);

            FileInfo configFile = new FileInfo(loader._configFile);

            if (!configFile.Exists) { await loader.SerializeNewConfigFile(configFile); }
            return loader.DeserializeConfig(configFile).GetAwaiter().GetResult();
        }
        private async Task SerializeNewConfigFile(FileInfo file)
        {
            string json = JsonConvert.SerializeObject(new DummyConfig(), Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(file.Create(), DummyUtils.UTF8))
            {
                await sw.WriteLineAsync(json);
                await sw.FlushAsync();
                sw.Close();
            }
        }
        private async Task<DummyConfig> DeserializeConfig(FileInfo file)
        {
            string json = "{}";
            using (StreamReader sr = new StreamReader(file.OpenRead(), DummyUtils.UTF8))
            {
                json = await sr.ReadToEndAsync();
                sr.Close();
            }
            DummyConfig config = JsonConvert.DeserializeObject<DummyConfig>(json) ?? throw new Exception("Ocorreu um erro na deserialização da configuração, verifique os arquivos da aplicação!");
            ValidateConfig(config);
            return config;
        }
        private void ValidateConfig(DummyConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.Discord.Token) || config.Discord.Token.Contains(' '))
            { throw new Exception("É necessário especificar um token válido para sua aplicação, você deve criar uma aplicação em: 'https://discord.com/developers/applications' para obter um token!"); }
        }
    }
}