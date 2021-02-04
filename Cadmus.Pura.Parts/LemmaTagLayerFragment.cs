using System.Collections.Generic;
using System.Text;
using Fusi.Tools.Config;
using Cadmus.Core.Layers;
using Cadmus.Core;

namespace Cadmus.Pura.Parts
{
    /// <summary>
    /// Lemma tags layer fragment.
    /// Tag: <c>fr.it.vedph.pura.lemma-tag</c>.
    /// </summary>
    /// <seealso cref="ITextLayerFragment" />
    [Tag("fr.it.vedph.pura.lemma-tag")]
    public sealed class LemmaTagLayerFragment : ITextLayerFragment
    {
        /// <summary>
        /// Gets or sets the location of this fragment.
        /// </summary>
        /// <remarks>
        /// The location can be expressed in different ways according to the
        /// text coordinates system being adopted. For instance, it might be a
        /// simple token-based coordinates system (e.g. 1.2=second token of
        /// first block), or a more complex system like an XPath expression.
        /// </remarks>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the normalized value.
        /// </summary>
        public string NormValue { get; set; }

        /// <summary>
        /// Get all the key=value pairs exposed by the implementor.
        /// </summary>
        /// <param name="item">The optional item. The item with its parts
        /// can optionally be passed to this method for those parts requiring
        /// to access further data.</param>
        /// <returns>The pins: <c>fr.tag</c>=tag if any.</returns>
        public IEnumerable<DataPin> GetDataPins(IItem item = null)
        {
            DataPinBuilder builder = new DataPinBuilder(
                DataPinHelper.DefaultFilter);

            builder.AddValue(PartBase.FR_PREFIX + "value",
                Value, filter: true, filterOptions: true);
            builder.AddValue(PartBase.FR_PREFIX + "u-value", Value);
            builder.AddValue(PartBase.FR_PREFIX + "n-value", NormValue);

            return builder.Build(null);
        }

        /// <summary>
        /// Gets the definitions of data pins used by the implementor.
        /// </summary>
        /// <returns>Data pins definitions.</returns>
        public IList<DataPinDefinition> GetDataPinDefinitions()
        {
            return new List<DataPinDefinition>(new[]
            {
                new DataPinDefinition(DataPinValueType.String,
                    PartBase.FR_PREFIX + "value",
                    "The value.",
                    "f"),
                new DataPinDefinition(DataPinValueType.String,
                    PartBase.FR_PREFIX + "u-value",
                    "The unfiltered value."),
                new DataPinDefinition(DataPinValueType.String,
                    PartBase.FR_PREFIX + "n-value",
                    "The normalized value.")
            });
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[LemmaTags]");

            if (!string.IsNullOrEmpty(Value))
                sb.Append(Value);

            if (!string.IsNullOrEmpty(NormValue))
                sb.Append(" (").Append(NormValue).Append(')');

            return sb.ToString();
        }
    }
}
