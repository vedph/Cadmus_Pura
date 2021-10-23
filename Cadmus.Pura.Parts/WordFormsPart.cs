using System.Collections.Generic;
using System.Text;
using Cadmus.Core;
using Fusi.Tools.Config;

namespace Cadmus.Pura.Parts
{
    /// <summary>
    /// Word forms part.
    /// <para>Tag: <c>it.vedph.pura.word-forms</c>.</para>
    /// </summary>
    [Tag("it.vedph.pura.word-forms")]
    public sealed class WordFormsPart : PartBase
    {
        /// <summary>
        /// Gets or sets the forms.
        /// </summary>
        public List<WordForm> Forms { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordFormsPart"/> class.
        /// </summary>
        public WordFormsPart()
        {
            Forms = new List<WordForm>();
        }

        /// <summary>
        /// Get all the key=value pairs (pins) exposed by the implementor.
        /// </summary>
        /// <param name="item">The optional item. The item with its parts
        /// can optionally be passed to this method for those parts requiring
        /// to access further data.</param>
        /// <returns>The pins: <c>tot-count</c> and a collection of pins with
        /// these keys: <c>lid</c>, <c>lemma</c>, <c>u-lemma</c>, <c>pos</c>,
        /// <c>variant</c>, <c>u-variant</c>.</returns>
        public override IEnumerable<DataPin> GetDataPins(IItem item)
        {
            DataPinBuilder builder = new DataPinBuilder(
                DataPinHelper.DefaultFilter);

            builder.Set("tot", Forms?.Count ?? 0, false);

            HashSet<string> forms = new HashSet<string>();
            HashSet<string> poss = new HashSet<string>();

            if (Forms?.Count > 0)
            {
                foreach (WordForm form in Forms)
                {
                    string lemma = DataPinHelper.DefaultFilter.Apply(
                        form.Lemma, true);
                    forms.Add(lemma);
                    poss.Add(form.Pos);

                    builder.AddValue("eid", form.Lid);
                    builder.AddValue("lemma@" + form.Lid, lemma);
                    builder.AddValue("u-lemma@" + form.Lid, form.Lemma);
                    builder.AddValue("pos@" + form.Lid, form.Pos);

                    if (form.Variants?.Count > 0)
                    {
                        foreach (var v in form.Variants)
                        {
                            string fv = DataPinHelper.DefaultFilter.Apply(
                                v.Value, true);
                            forms.Add(fv);
                            builder.AddValue("variant@" + form.Lid, fv);
                            builder.AddValue("u-variant@" + form.Lid, v.Value);
                        }
                    }
                }
                builder.AddValues("form", forms);
                builder.AddValues("pos", poss);
            }

            return builder.Build(this);
        }

        /// <summary>
        /// Gets the definitions of data pins used by the implementor.
        /// </summary>
        /// <returns>Data pins definitions.</returns>
        public override IList<DataPinDefinition> GetDataPinDefinitions()
        {
            return new List<DataPinDefinition>(new[]
            {
                new DataPinDefinition(DataPinValueType.Integer,
                   "tot-count",
                   "The total count of forms."),
                new DataPinDefinition(DataPinValueType.String,
                   "eid",
                   "The list of forms lexicographic IDs.",
                   "M"),
                new DataPinDefinition(DataPinValueType.String,
                   "lemma@EID",
                   "The list of each form's lemma.",
                   "Mf"),
                new DataPinDefinition(DataPinValueType.String,
                   "u-lemma@EID",
                   "The list of each form's lemmata, unfiltered.",
                   "M"),
                new DataPinDefinition(DataPinValueType.String,
                   "pos@EID",
                   "The list of each form's POS.",
                   "M"),
                new DataPinDefinition(DataPinValueType.String,
                   "variant@EID",
                   "The list of each form's variants.",
                   "Mf"),
                new DataPinDefinition(DataPinValueType.String,
                   "u-variant@EID",
                   "The list of each form's variants, unfiltered.",
                   "M"),
                new DataPinDefinition(DataPinValueType.String,
                   "form",
                   "The list of each unique lemma's form or variant.",
                   "Mf"),
                new DataPinDefinition(DataPinValueType.String,
                   "pos",
                   "The list of each unique form's POS.",
                   "M")
            });
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[WordForms]");

            if (Forms?.Count > 0)
            {
                sb.Append(' ');
                int n = 0;
                foreach (var entry in Forms)
                {
                    if (++n > 3) break;
                    if (n > 1) sb.Append("; ");
                    sb.Append(entry);
                }
                if (Forms.Count > 3)
                    sb.Append("...(").Append(Forms.Count).Append(')');
            }

            return sb.ToString();
        }
    }
}
