using System.Collections.Generic;
using System.Text;

namespace Cadmus.Pura.Parts
{
    /// <summary>
    /// A word form in the <see cref="WordFormsPart"/>.
    /// </summary>
    public class WordForm
    {
        /// <summary>
        /// Gets or sets the lexicographic ID for this form.
        /// The lexicographic ID can be defined in different ways according
        /// to the project. In the case of PURA it's just the word form with only
        /// uppercase letters and no diacritics, followed by the homograph
        /// number if greater than 0.
        /// </summary>
        public string Lid { get; set; }

        /// <summary>
        /// Gets or sets the prelemma. This can be any text prefixed to the
        /// lemma (e.g. <c>to</c> before an English verb).
        /// </summary>
        public string Prelemma { get; set; }

        /// <summary>
        /// Gets or sets the lemma.
        /// </summary>
        public string Lemma { get; set; }

        /// <summary>
        /// Gets or sets the postlemma. This can be any text suffixed to the
        /// lemma (e.g. a particle after a phrasal verb, like <c>off</c>
        /// in <c>log off</c>; or the article representing the genre in a Greek
        /// word).
        /// </summary>
        public string Postlemma { get; set; }

        /// <summary>
        /// Gets or sets the homograph number. This is usually 0. Set to 1 or
        /// more to define the order of homograph forms.
        /// </summary>
        public short Homograph { get; set; }

        /// <summary>
        /// Gets or sets the part of speech (POS) for this word.
        /// </summary>
        public string Pos { get; set; }

        /// <summary>
        /// Gets or sets an optional note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets an optional list of variant forms (e.g. alternative
        /// forms, derived forms, etc. -- their type can be specified by
        /// their <see cref="VariantForm.Tag"/>).
        /// </summary>
        public List<VariantForm> Variants { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordForm"/> class.
        /// </summary>
        public WordForm()
        {
            Variants = new List<VariantForm>();
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

            if (!string.IsNullOrEmpty(Prelemma))
                sb.Append(Prelemma).Append(' ');

            sb.Append(Lemma);

            if (!string.IsNullOrEmpty(Postlemma))
                sb.Append(' ').Append(Postlemma);

            if (Homograph > 0) sb.Append(" (").Append(Homograph).Append(')');

            if (!string.IsNullOrEmpty(Pos))
                sb.Append(" [").Append(Pos).Append(']');

            return sb.ToString();
        }
    }
}
