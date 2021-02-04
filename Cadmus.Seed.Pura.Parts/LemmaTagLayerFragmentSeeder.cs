using Bogus;
using Cadmus.Core;
using Cadmus.Core.Layers;
using Cadmus.Pura.Parts;
using Fusi.Tools.Config;
using System;

namespace Cadmus.Seed.Pura.Parts
{
    /// <summary>
    /// Seeder for <see cref="LemmaTagLayerFragment"/>'s.
    /// Tag: <c>seed.fr.it.vedph.pura.lemma-tags</c>.
    /// </summary>
    /// <seealso cref="FragmentSeederBase" />
    /// <seealso cref="IConfigurable{LemmaTagsLayerFragmentSeederOptions}" />
    [Tag("seed.fr.it.vedph.pura.lemma-tag")]
    public sealed class LemmaTagLayerFragmentSeeder : FragmentSeederBase
    {
        /// <summary>
        /// Gets the type of the fragment.
        /// </summary>
        /// <returns>Type.</returns>
        public override Type GetFragmentType() => typeof(LemmaTagLayerFragment);

        /// <summary>
        /// Creates and seeds a new part.
        /// </summary>
        /// <param name="item">The item this part should belong to.</param>
        /// <param name="location">The location.</param>
        /// <param name="baseText">The base text.</param>
        /// <returns>A new fragment.</returns>
        /// <exception cref="ArgumentNullException">location or baseText
        /// </exception>
        public override ITextLayerFragment GetFragment(
            IItem item, string location, string baseText)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            if (baseText == null)
                throw new ArgumentNullException(nameof(baseText));

            LemmaTagLayerFragment fragment = new Faker<LemmaTagLayerFragment>()
                .RuleFor(fr => fr.Location, location)
                .RuleFor(fr => fr.Tag, f => f.PickRandom(null, "t1", "t2"))
                .RuleFor(fr => fr.Value, f => f.Lorem.Word())
                .Generate();
            fragment.NormValue = fragment.Value.ToUpperInvariant();
            return fragment;
        }
    }
}
