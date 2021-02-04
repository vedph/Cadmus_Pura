using Cadmus.Core;
using Cadmus.Core.Layers;
using Cadmus.Pura.Parts;
using Fusi.Tools.Config;
using System;
using System.Reflection;
using Xunit;

namespace Cadmus.Seed.Pura.Parts.Test
{
    public sealed class LemmaTagLayerFragmentSeederTest
    {
        private static readonly PartSeederFactory _factory;
        private static readonly SeedOptions _seedOptions;
        private static readonly IItem _item;

        static LemmaTagLayerFragmentSeederTest()
        {
            _factory = TestHelper.GetFactory();
            _seedOptions = _factory.GetSeedOptions();
            _item = _factory.GetItemSeeder().GetItem(1, "facet");
        }

        [Fact]
        public void TypeHasTagAttribute()
        {
            Type t = typeof(LemmaTagLayerFragmentSeeder);
            TagAttribute attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
            Assert.NotNull(attr);
            Assert.Equal("seed.fr.it.vedph.pura.lemma-tag", attr.Tag);
        }

        [Fact]
        public void GetFragmentType_Ok()
        {
            LemmaTagLayerFragmentSeeder seeder = new LemmaTagLayerFragmentSeeder();
            Assert.Equal(typeof(LemmaTagLayerFragment), seeder.GetFragmentType());
        }

        [Fact]
        public void Seed_WithOptions_Ok()
        {
            LemmaTagLayerFragmentSeeder seeder = new LemmaTagLayerFragmentSeeder();
            seeder.SetSeedOptions(_seedOptions);
            ITextLayerFragment fragment = seeder.GetFragment(_item, "1.1", "alpha");

            Assert.NotNull(fragment);

            LemmaTagLayerFragment fr = fragment as LemmaTagLayerFragment;
            Assert.NotNull(fr);

            Assert.Equal("1.1", fr.Location);
            Assert.NotEmpty(fr.Value);
            Assert.NotEmpty(fr.NormValue);
        }
    }
}
