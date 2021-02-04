using Cadmus.Core;
using Fusi.Tools.Config;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Cadmus.Seed.Pura.Parts;

namespace Cadmus.Pura.Parts.Test
{
    public sealed class LemmaTagLayerFragmentTest
    {
        private static LemmaTagLayerFragment GetFragment()
        {
            var seeder = new LemmaTagLayerFragmentSeeder();
            return (LemmaTagLayerFragment)
                seeder.GetFragment(null, "1.2", "exemplum fictum");
        }

        private static LemmaTagLayerFragment GetEmptyFragment()
        {
            return new LemmaTagLayerFragment
            {
                Location = "1.23",
            };
        }

        [Fact]
        public void Fragment_Has_Tag()
        {
            TagAttribute attr = typeof(LemmaTagLayerFragment).GetTypeInfo()
                .GetCustomAttribute<TagAttribute>();
            string typeId = attr != null ? attr.Tag : GetType().FullName;
            Assert.NotNull(typeId);
            Assert.StartsWith(PartBase.FR_PREFIX, typeId);
        }

        [Fact]
        public void Fragment_Is_Serializable()
        {
            LemmaTagLayerFragment fragment = GetFragment();

            string json = TestHelper.SerializeFragment(fragment);
            LemmaTagLayerFragment fragment2 =
                TestHelper.DeserializeFragment<LemmaTagLayerFragment>(json);

            Assert.Equal(fragment.Location, fragment2.Location);
            Assert.Equal(fragment.Tag, fragment.Tag);
            Assert.Equal(fragment.Value, fragment.Value);
            Assert.Equal(fragment.NormValue, fragment.NormValue);
        }

        [Fact]
        public void GetDataPins_Ok()
        {
            LemmaTagLayerFragment fragment = GetEmptyFragment();
            fragment.Tag = "tag";
            fragment.Value = "Héllo";
            fragment.NormValue = "HELLO";

            List<DataPin> pins = fragment.GetDataPins(null).ToList();

            Assert.Equal(3, pins.Count);

            DataPin pin = pins.Find(p => p.Name == PartBase.FR_PREFIX + "value"
               && p.Value == "hello");
            Assert.NotNull(pin);

            pin = pins.Find(p => p.Name == PartBase.FR_PREFIX + "n-value"
               && p.Value == "HELLO");
            Assert.NotNull(pin);

            pin = pins.Find(p => p.Name == PartBase.FR_PREFIX + "u-value"
               && p.Value == "Héllo");
            Assert.NotNull(pin);
        }
    }
}
