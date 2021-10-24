using System.IO;
using System.Text;

namespace Cadmus.Pura.Parts
{
    /// <summary>
    /// PURA graph helper.
    /// </summary>
    static public class PuraGraphHelper
    {
        /// <summary>
        /// Gets the SQL code for seeding the PURA graph.
        /// </summary>
        /// <returns>SQL code.</returns>
        static public string GetGraphSql()
        {
            return new StreamReader(
                typeof(PuraGraphHelper).Assembly.GetManifestResourceStream(
                    "Cadmus.Pura.Parts.Assets.Graph.mysql"), Encoding.UTF8)
                .ReadToEnd();
        }
    }
}
