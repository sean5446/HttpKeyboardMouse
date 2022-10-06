
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using System.Diagnostics;

namespace WinFormsApp1
{
    internal class ConfigReader
    {

        public static Config LoadFile(string filepath)
        {
            try
            {
                string ymlContents = new StreamReader(filepath).ReadToEnd();
                var deserializer = new DeserializerBuilder()
                 .WithNamingConvention(CamelCaseNamingConvention.Instance)
                 .Build();
                return deserializer.Deserialize<Config>(ymlContents);
            }
            catch (YamlException ex)
            {
                Trace.WriteLine(ex);
                return new Config();
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine($"Config file {filepath} not found. Loading defaults.");
                return new Config();
            }
        }

        public class Config
        {
            public Server Server { get; set; }
            public Config()
            {
                Server = new();
            }
        }

        public class Server
        {
            public int Port { get; set; } = 8080;
        }

    }
}
