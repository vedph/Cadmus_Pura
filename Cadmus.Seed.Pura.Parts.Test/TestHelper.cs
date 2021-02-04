using Cadmus.Core;
using Cadmus.Core.Config;
using Cadmus.Pura.Parts;
using Fusi.Microsoft.Extensions.Configuration.InMemoryJson;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Cadmus.Seed.Pura.Parts.Test
{
    static internal class TestHelper
    {
        static public Stream GetResourceStream(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    $"Cadmus.Seed.Pura.Parts.Test.Assets.{name}");
        }

        static public string LoadResourceText(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            using (StreamReader reader = new StreamReader(
                GetResourceStream(name),
                Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        static public PartSeederFactory GetFactory()
        {
            // map
            TagAttributeToTypeMap map = new TagAttributeToTypeMap();
            map.Add(new[]
            {
                // Cadmus.Core
                typeof(StandardItemSortKeyBuilder).Assembly,
                // Cadmus.Pura.Parts
                typeof(WordFormsPart).Assembly
            });

            // container
            Container container = new Container();
            PartSeederFactory.ConfigureServices(
                container,
                new StandardPartTypeProvider(map),
                new[]
                {
                    // Cadmus.Seed.Pura.Parts
                    typeof(WordFormsPartSeeder).Assembly
                });

            // config
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddInMemoryJson(LoadResourceText("SeedConfig.json"));
            var configuration = builder.Build();

            return new PartSeederFactory(container, configuration);
        }

        static public void AssertPartMetadata(IPart part)
        {
            Assert.NotNull(part.Id);
            Assert.NotNull(part.ItemId);
            Assert.NotNull(part.UserId);
            Assert.NotNull(part.CreatorId);
        }
    }
}
