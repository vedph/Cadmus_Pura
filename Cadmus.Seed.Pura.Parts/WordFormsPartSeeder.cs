using Bogus;
using Cadmus.Core;
using Cadmus.Pura.Parts;
using Fusi.Tools.Config;
using System;
using System.Collections.Generic;

namespace Cadmus.Seed.Pura.Parts
{
    /// <summary>
    /// Part seeder for <see cref="WordFormsPart"/>.
    /// Tag: <c>seed.it.vedph.pura.word-forms</c>.
    /// </summary>
    /// <seealso cref="PartSeederBase" />
    [Tag("seed.it.vedph.pura.word-forms")]
    public sealed class WordFormsPartSeeder : PartSeederBase
    {
        private static List<VariantForm> GetVariantForms(int min, int max)
        {
            List<VariantForm> forms = new List<VariantForm>();
            int count = Randomizer.Seed.Next(min, max + 1);

            for (int n = 1; n <= count; n++)
            {
                forms.Add(new Faker<VariantForm>()
                    .RuleFor(v => v.Tag, f => f.PickRandom(null, "orth", "dial"))
                    .RuleFor(v => v.Pos, f => f.PickRandom("noun", "adj", "verb"))
                    .RuleFor(v => v.Value, f => f.Lorem.Word())
                    .Generate());
            }

            return forms;
        }

        /// <summary>
        /// Creates and seeds a new part.
        /// </summary>
        /// <param name="item">The item this part should belong to.</param>
        /// <param name="roleId">The optional part role ID.</param>
        /// <param name="factory">The part seeder factory. This is used
        /// for layer parts, which need to seed a set of fragments.</param>
        /// <returns>A new part.</returns>
        /// <exception cref="ArgumentNullException">item or factory</exception>
        public override IPart GetPart(IItem item, string roleId,
            PartSeederFactory factory)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            WordFormsPart part = new WordFormsPart();
            SetPartMetadata(part, roleId, item);

            for (int n = 1; n <= Randomizer.Seed.Next(1, 3 + 1); n++)
            {
                WordForm form = new Faker<WordForm>()
                    .RuleFor(w => w.Lemma, f => f.Lorem.Word())
                    .RuleFor(w => w.Homograph, f => (short)
                        (f.Random.Bool(0.1f) ? 1 : 0))
                    .RuleFor(v => v.Pos, f => f.PickRandom("noun", "adj", "verb"))
                    .RuleFor(w => w.Note, f => f.Random.Bool(0.25f)?
                        f.Lorem.Sentence() : null)
                    .RuleFor(w => w.Variants, GetVariantForms(1, 3))
                    .Generate();

                form.Lid = form.Lemma.ToUpperInvariant();

                part.Forms.Add(form);
            }

            return part;
        }
    }
}
