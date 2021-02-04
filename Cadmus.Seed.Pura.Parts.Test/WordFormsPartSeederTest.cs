using Cadmus.Core;
using Cadmus.Pura.Parts;
using Fusi.Tools.Config;
using System;
using System.Reflection;
using Xunit;

namespace Cadmus.Seed.Pura.Parts.Test
{
    public sealed class WordFormsPartSeederTest
    {
        private static readonly PartSeederFactory _factory;
        private static readonly SeedOptions _seedOptions;
        private static readonly IItem _item;

        static WordFormsPartSeederTest()
        {
            _factory = TestHelper.GetFactory();
            _seedOptions = _factory.GetSeedOptions();
            _item = _factory.GetItemSeeder().GetItem(1, "facet");
        }

        [Fact]
        public void TypeHasTagAttribute()
        {
            Type t = typeof(WordFormsPartSeeder);
            TagAttribute attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
            Assert.NotNull(attr);
            Assert.Equal("seed.it.vedph.pura.word-forms", attr.Tag);
        }

        [Fact]
        public void Seed_Ok()
        {
            WordFormsPartSeeder seeder = new WordFormsPartSeeder();
            seeder.SetSeedOptions(_seedOptions);

            IPart part = seeder.GetPart(_item, null, _factory);

            Assert.NotNull(part);

            WordFormsPart p = part as WordFormsPart;
            Assert.NotNull(p);

            TestHelper.AssertPartMetadata(p);

            Assert.NotEmpty(p.Forms);
        }
    }
}
