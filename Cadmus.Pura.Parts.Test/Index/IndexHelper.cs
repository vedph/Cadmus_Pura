using Cadmus.Core.Config;
using Cadmus.Index.Graph;
using System.IO;

namespace Cadmus.Pura.Parts.Test.Index
{
    // TODO: remap x: to true ontologies

    static internal class IndexHelper
    {
        static private Stream GetResourceStream(string name) =>
            typeof(IndexHelper).Assembly.GetManifestResourceStream(
                "Cadmus.Pura.Parts.Test.Assets." + name);

        static private int AddNodes(IGraphRepository repository)
        {
            IGraphPresetReader reader = new JsonGraphPresetReader();

            using var stream = GetResourceStream("PresetNodes.json");
            int count = 0;
            foreach (UriNode node in reader.ReadNodes(stream))
            {
                node.Id = repository.AddUri(node.Uri);
                repository.AddNode(node, true);
                count++;
            }

            return count;
        }

        static private void AddMappings(IGraphRepository repository)
        {
            IGraphPresetReader reader = new JsonGraphPresetReader();

            using var stream = GetResourceStream("PresetMappings.json");
            foreach (NodeMapping mapping in reader.ReadMappings(stream))
            {
                repository.AddMapping(mapping);
            }
        }

        static private void AddThesauri(IGraphRepository repository)
        {
            IGraphPresetReader reader = new JsonGraphPresetReader();

            using var stream = GetResourceStream("PresetThesauri.json");
            foreach (Thesaurus thesaurus in reader.ReadThesauri(stream))
            {
                repository.AddThesaurus(thesaurus, false, "x:classes/");
            }
        }

        static public int AddPresets(IGraphRepository repository, bool thesauri)
        {
            int nodeCount = AddNodes(repository);
            AddMappings(repository);
            if (thesauri) AddThesauri(repository);

            return nodeCount;
        }
    }
}
