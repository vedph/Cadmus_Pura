using Cadmus.Index.Graph;

namespace Cadmus.Pura.Parts.Test.Index
{
    // TODO: remap x: to true ontologies

    static internal class IndexHelper
    {
        static private int AddProperties(IGraphRepository repository)
        {
            // a (rdfs:type)
            repository.AddNode(new Node
            {
                Id = repository.AddUri("a"),
                Tag = Node.TAG_PROPERTY,
                Label = "is-a"
            });
            // rdfs:comment
            repository.AddNode(new Node
            {
                Id = repository.AddUri("rdfs:comment"),
                Tag = Node.TAG_PROPERTY,
                Label = "comment"
            });

            // kad:hasFacet
            repository.AddNode(new Node
            {
                Id = repository.AddUri("kad:hasFacet"),
                Tag = Node.TAG_PROPERTY,
                Label = "has Cadmus facet"
            });
            // kad:isInGroup
            repository.AddNode(new Node
            {
                Id = repository.AddUri("kad:isInGroup"),
                Tag = Node.TAG_PROPERTY,
                Label = "is in Cadmus group"
            });

            // x:hasForm
            repository.AddNode(new Node
            {
                Id = repository.AddUri("x:hasForm"),
                Tag = Node.TAG_PROPERTY,
                Label = "has linguistic form"
            });
            // x:hasIxForm
            repository.AddNode(new Node
            {
                Id = repository.AddUri("x:hasIxForm"),
                Tag = Node.TAG_PROPERTY,
                Label = "has linguistic form (filtered)"
            });
            // x:hasVariantForm
            repository.AddNode(new Node
            {
                Id = repository.AddUri("x:hasVariantForm"),
                Tag = Node.TAG_PROPERTY,
                Label = "has linguistic variant form"
            });
            // x:hasIxVariant
            repository.AddNode(new Node
            {
                Id = repository.AddUri("x:hasIxVariant"),
                Tag = Node.TAG_PROPERTY,
                Label = "has linguistic variant form (filtered)"
            });
            // x:hasPOS
            repository.AddNode(new Node
            {
                Id = repository.AddUri("x:hasPOS"),
                Tag = Node.TAG_PROPERTY,
                Label = "has part of speech"
            });

            return 9;
        }

        static private int AddClasses(IGraphRepository repository)
        {
            repository.AddNode(new Node
            {
                Id = repository.AddUri("x:lemma"),
                IsClass = true,
                Label = "lemma"
            });

            return 1;
        }

        static private void AddItemRules(IGraphRepository repository)
        {
            // lemma item -> x:lemmata/X
            NodeMapping itemMapping = new NodeMapping
            {
                SourceType = NodeSourceType.Item,
                Name = "Lemma item",
                FacetFilter = "lemma",
                Prefix = "x:lemmata/",
                LabelTemplate = "{title}",
                Description = "Lemma item -> node"
            };
            repository.AddMapping(itemMapping);

            // CHILD item dsc -> ITEM rdfs:comment ...
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Item,
                Name = "Item description",
                ParentId = itemMapping.Id,
                TripleP = "rdfs:comment",
                TripleO = "$dsc",
                Description = "Item's description -> rdfs:comment",
            });

            // CHILD item a x:lemma
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Item,
                Name = "Item a lemma",
                ParentId = itemMapping.Id,
                TripleP = "a",
                TripleO = "x:lemma",
                Description = "item a x:lemma"
            });
        }

        static private void AddPinRules(IGraphRepository repository)
        {
            // WordFormsPart
            // eid -> x:forms/EID
            NodeMapping eidMapping = new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                Name = "Pin eid",
                FacetFilter = "lemma",
                PartType = "it.vedph.pura.word-forms",
                PinName = "eid",
                Prefix = "x:forms/",
                LabelTemplate = "{pin-value}",
                // slot key will be the EID itself; its content
                // will be the UID of the node output by this mapping
                Slot = "{pin-value}"
            };
            repository.AddMapping(eidMapping);

            // CHILD eid: x:forms/EID kad:isInGroup item
            NodeMapping eidGroupMapping = new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                ParentId = eidMapping.Id,
                Name = "Pin eid kad:isInGroup ITEM",
                PartType = "it.vedph.pura.word-forms",
                PinName = "eid",
                TripleP = "kad:isInGroup",
                TripleO = "$item"
            };
            repository.AddMapping(eidGroupMapping);

            // lemma@*: x:forms/EID x:hasForm ...
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                Name = "Pin lemma@* x:hasForm ...",
                PartType = "it.vedph.pura.word-forms",
                PinName = "u-lemma@*",
                // the subject UID is got from a slot whose key is equal
                // to the pin's EID suffix (e.g. abacus from lemma@abacus)
                TripleS = "$slot:{pin-eid}",
                TripleP = "x:hasForm",
                TripleO = "$pin-value"
            });

            // u-lemma@*: x:forms/EID x:hasIxForm ...
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                Name = "Pin lemma@* x:hasIxForm ...",
                PartType = "it.vedph.pura.word-forms",
                PinName = "lemma@*",
                TripleS = "$slot:{pin-eid}",
                TripleP = "x:hasIxForm",
                TripleO = "$pin-value"
            });

            // pos@*: x:forms/EID x:hasPOS ...
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                Name = "Pin pos@* x:hasPOS ...",
                PartType = "it.vedph.pura.word-forms",
                PinName = "pos@*",
                TripleS = "$slot:{pin-eid}",
                TripleP = "x:hasPOS",
                TripleO = "$pin-value"
            });

            // u-variant@* x:hasVariantForm ...
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                Name = "Pin u-variant@* x:hasVariantForm ...",
                PartType = "it.vedph.pura.word-forms",
                PinName = "u-variant@*",
                TripleS = "$slot:{pin-eid}",
                TripleP = "x:hasVariantForm",
                TripleO = "$pin-value"
            });

            // variant@* x:hasIxVariantForm ...
            repository.AddMapping(new NodeMapping
            {
                SourceType = NodeSourceType.Pin,
                Name = "Pin variant@* x:hasIxVariantForm ...",
                PartType = "it.vedph.pura.word-forms",
                PinName = "variant@*",
                TripleS = "$slot:{pin-eid}",
                TripleP = "x:hasIxVariantForm",
                TripleO = "$pin-value"
            });
        }

        static public int AddRules(IGraphRepository repository)
        {
            int nodeCount = AddProperties(repository);
            nodeCount += AddClasses(repository);

            AddItemRules(repository);
            AddProperties(repository);
            AddPinRules(repository);

            return nodeCount;
        }
    }
}
