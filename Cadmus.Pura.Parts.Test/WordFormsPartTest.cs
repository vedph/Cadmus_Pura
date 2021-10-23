using Cadmus.Core;
using Cadmus.Seed.Pura.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cadmus.Pura.Parts.Test
{
    public sealed class WordFormsPartTest
    {
        private static WordFormsPart GetPart()
        {
            WordFormsPartSeeder seeder = new WordFormsPartSeeder();
            IItem item = new Item
            {
                FacetId = "default",
                CreatorId = "zeus",
                UserId = "zeus",
                Description = "Test item",
                Title = "Test Item",
                SortKey = ""
            };
            return (WordFormsPart)seeder.GetPart(item, null, null);
        }

        private static WordFormsPart GetEmptyPart()
        {
            return new WordFormsPart
            {
                ItemId = Guid.NewGuid().ToString(),
                RoleId = "some-role",
                CreatorId = "zeus",
                UserId = "another",
            };
        }

        [Fact]
        public void Part_Is_Serializable()
        {
            WordFormsPart part = GetPart();

            string json = TestHelper.SerializePart(part);
            WordFormsPart part2 =
                TestHelper.DeserializePart<WordFormsPart>(json);

            Assert.Equal(part.Id, part2.Id);
            Assert.Equal(part.TypeId, part2.TypeId);
            Assert.Equal(part.ItemId, part2.ItemId);
            Assert.Equal(part.RoleId, part2.RoleId);
            Assert.Equal(part.CreatorId, part2.CreatorId);
            Assert.Equal(part.UserId, part2.UserId);

            Assert.Equal(part.Forms.Count, part2.Forms.Count);
        }

        [Fact]
        public void GetDataPins_NoEntries_Ok()
        {
            WordFormsPart part = GetPart();
            part.Forms.Clear();

            List<DataPin> pins = part.GetDataPins(null).ToList();

            Assert.Single(pins);
            DataPin pin = pins[0];
            Assert.Equal("tot-count", pin.Name);
            TestHelper.AssertPinIds(part, pin);
            Assert.Equal("0", pin.Value);
        }

        [Fact]
        public void GetDataPins_Entries_Ok()
        {
            WordFormsPart part = GetEmptyPart();

            for (int n = 1; n <= 3; n++)
            {
                // La Lb Lc
                var form = new WordForm
                {
                    Lemma = "L" + new string((char)('a' + n - 1), 1),
                    Pos = n % 2 == 0? "even" : "odd"
                };
                // LA LB LC
                form.Lid = form.Lemma.ToUpperInvariant();
                if (n == 2)
                {
                    form.Variants.Add(new VariantForm
                    {
                        Value = "Váriant"
                    });
                }

                part.Forms.Add(form);
            }

            List<DataPin> pins = part.GetDataPins(null).ToList();

            Assert.Equal(21, pins.Count);

            // tot-count
            DataPin pin = pins.Find(p => p.Name == "tot-count");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);
            Assert.Equal("3", pin.Value);

            // pos
            pin = pins.Find(p => p.Name == "pos" && p.Value == "odd");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);
            pin = pins.Find(p => p.Name == "pos" && p.Value == "even");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);

            // form
            pin = pins.Find(p => p.Name == "form" && p.Value == "la");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);
            pin = pins.Find(p => p.Name == "form" && p.Value == "lb");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);
            pin = pins.Find(p => p.Name == "form" && p.Value == "lc");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);
            pin = pins.Find(p => p.Name == "form" && p.Value == "variant");
            Assert.NotNull(pin);
            TestHelper.AssertPinIds(part, pin);

            // La Lb Lc
            int nr = 0;
            for (char c = 'a'; c <= 'c'; c++)
            {
                nr++;
                char uc = char.ToUpper(c);

                // eid
                string eid = $"L{uc}";
                pin = pins.Find(p => p.Name == "eid" && p.Value == eid);
                Assert.NotNull(pin);
                TestHelper.AssertPinIds(part, pin);

                // lemma@EID
                pin = pins.Find(p => p.Name == "lemma@" + eid && p.Value == $"l{c}");
                Assert.NotNull(pin);
                TestHelper.AssertPinIds(part, pin);

                // u-lemma@EID
                pin = pins.Find(p => p.Name == "u-lemma@" + eid && p.Value == $"L{c}");
                Assert.NotNull(pin);
                TestHelper.AssertPinIds(part, pin);

                // pos@EID
                string pos = nr % 2 == 0 ? "even" : "odd";
                pin = pins.Find(p => p.Name == "pos@" + eid && p.Value == pos);
                Assert.NotNull(pin);
                TestHelper.AssertPinIds(part, pin);

                if (nr == 2)
                {
                    // variant@EID
                    pin = pins.Find(p => p.Name == "variant@" + eid
                        && p.Value == "variant");
                    Assert.NotNull(pin);
                    TestHelper.AssertPinIds(part, pin);

                    // u-variant@EID
                    pin = pins.Find(p => p.Name == "u-variant@" + eid
                        && p.Value == "Váriant");
                    Assert.NotNull(pin);
                    TestHelper.AssertPinIds(part, pin);
                }
            }
        }
    }
}
