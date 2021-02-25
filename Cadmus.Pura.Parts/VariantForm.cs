namespace Cadmus.Pura.Parts
{
    /// <summary>
    /// A variant form with its optional tag.
    /// </summary>
    public class VariantForm
    {
        /// <summary>
        /// The form's value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the form's POS.
        /// </summary>
        public string Pos { get; set; }

        /// <summary>
        /// The form's optional tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Tag)? Value : $"[{Tag}] ({Pos}) {Value}";
        }
    }
}
