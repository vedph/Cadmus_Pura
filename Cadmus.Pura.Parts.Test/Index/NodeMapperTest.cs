using Cadmus.Core;
using Cadmus.Index.Graph;
using Cadmus.Index.MySql;
using Cadmus.Index.Sql;
using Fusi.DbManager;
using Fusi.DbManager.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Cadmus.Pura.Parts.Test.Index
{
    // https://github.com/xunit/xunit/issues/1999
    [CollectionDefinition(nameof(NonParallelResourceCollection),
        DisableParallelization = true)]
    public class NonParallelResourceCollection { }
    [Collection(nameof(NonParallelResourceCollection))]
    public sealed class NodeMapperTest
    {
        private const string CST = "Server=localhost;Database={0};Uid=root;Pwd=mysql;";
        private const string DB_NAME = "cadmus-index-test";
        static private readonly string CS = string.Format(CST, DB_NAME);

        private static void Reset()
        {
            IDbManager manager = new MySqlDbManager(CST);
            if (manager.Exists(DB_NAME))
            {
                manager.ClearDatabase(DB_NAME);
            }
            else
            {
                manager.CreateDatabase(DB_NAME,
                    MySqlItemIndexWriter.GetMySqlSchema(), null);
            }
        }

        private static IGraphRepository GetRepository()
        {
            MySqlGraphRepository repository = new MySqlGraphRepository();
            repository.Configure(new SqlOptions
            {
                ConnectionString = CS
            });
            return repository;
        }

        private static IItem GetLemmaItem()
        {
            return new Item
            {
                Title = "abaco",
                Description = "An ancient calculating tool.",
                FacetId = "lemma",
                GroupId = "abaco",
                SortKey = "abaco",
                CreatorId = "creator",
                UserId = "user"
            };
        }

        [Fact]
        public void MapLemmaItem_Ok()
        {
            Reset();
            IGraphRepository repository = GetRepository();
            int nodeCount = IndexHelper.AddRules(repository);
            NodeMapper mapper = new NodeMapper(repository);

            // item
            IItem item = GetLemmaItem();
            GraphSet set = mapper.MapItem(item);

            Assert.Equal(2, set.Nodes.Count);
            Assert.Equal(2, set.Triples.Count);

            // x:lemmata/abaco
            NodeResult node = set.Nodes.FirstOrDefault(
                n => n.Uri == "x:lemmata/abaco");
            Assert.NotNull(node);
            Assert.Equal(NodeSourceType.Item, node.SourceType);
            Assert.Equal(item.Id, node.Sid);
            Assert.Equal("abaco", node.Label);
            Assert.False(node.IsClass);
            Assert.Null(node.Tag);

            // x:lemma is the target of a triple, so it's found in the set,
            // even though it will already be present in the store
            node = set.Nodes.FirstOrDefault(n => n.Uri == "x:lemma");
            Assert.NotNull(node);
            Assert.Equal(NodeSourceType.User, node.SourceType);
            Assert.Null(node.Sid);
            Assert.True(node.IsClass);

            // x:lemmata/abaco rdfs:comment ...
            TripleResult triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "rdfs:comment");
            Assert.NotNull(triple);
            Assert.Equal("x:lemmata/abaco", triple.SubjectUri);
            Assert.Equal(item.Description, triple.ObjectLiteral);
            Assert.Null(triple.ObjectUri);
            Assert.Equal(item.Id, triple.Sid);
            Assert.Null(triple.Tag);

            // x:lemmata/abaco a x:lemma
            triple = set.Triples.FirstOrDefault(t => t.PredicateUri == "a");
            Assert.NotNull(triple);
            Assert.Equal("x:lemmata/abaco", triple.SubjectUri);
            Assert.Equal("x:lemma", triple.ObjectUri);
            Assert.Null(triple.ObjectLiteral);
            Assert.Equal(item.Id, triple.Sid);
            Assert.Null(triple.Tag);
        }

        [Fact]
        public void MapLemmaPins_Ok()
        {
            Reset();
            IGraphRepository repository = GetRepository();
            int nodeCount = IndexHelper.AddRules(repository);
            NodeMapper mapper = new NodeMapper(repository);

            // item
            IItem item = GetLemmaItem();

            // part
            WordFormsPart part = new WordFormsPart
            {
                ItemId = item.Id,
                CreatorId = "zeus",
                UserId = "zeus",
            };
            part.Forms.Add(new WordForm
            {
                Lid = "abaco",
                Lemma = "ábaco",
                Pos = "sm",
                Variants = new List<VariantForm>(new VariantForm[]
                {
                    new VariantForm
                    {
                        Pos = "sm",
                        Value = "ábbaco"
                    }
                })
            });
            IList<DataPin> pins = part.GetDataPins(item).ToList();
            // non-mapped pins: tot-count, form x2, pos.
            // mapped pins: eid=abaco, lemma@abaco=abaco, u-lemma@abaco=ábaco,
            // pos@abaco=sm, variant@abaco=abbaco, u-variant@ABBACO=ábbaco.
            Assert.Equal(10, pins.Count);

            GraphSet set = mapper.MapPins(item, part,
                pins.Select(p => Tuple.Create(p.Name, p.Value)).ToList());

            Assert.Equal(2, set.Nodes.Count);
            Assert.Equal(6, set.Triples.Count);

            // x:lemmata/abaco is the container item, so it's found in this set
            NodeResult node = set.Nodes.FirstOrDefault(
                n => n.Uri == "x:lemmata/abaco");
            Assert.NotNull(node);
            Assert.Equal(NodeSourceType.User, node.SourceType);
            Assert.Null(node.Sid);
            // as this was not mapped from the item, it hasn't its label
            Assert.Equal("x:lemmata/abaco", node.Label);
            Assert.False(node.IsClass);
            Assert.Null(node.Tag);

            // x:forms/abaco
            string sid = part.Id + "|eid|abaco";
            node = set.Nodes.FirstOrDefault(n => n.Uri == "x:forms/abaco");
            Assert.NotNull(node);
            Assert.Equal(NodeSourceType.Pin, node.SourceType);
            Assert.Equal(sid, node.Sid);
            Assert.Equal("abaco", node.Label);
            Assert.False(node.IsClass);
            Assert.Null(node.Tag);

            // x:forms/abaco kad:isInGroup x:lemmata/abaco
            TripleResult triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "kad:isInGroup");
            Assert.NotNull(triple);
            Assert.Equal("x:forms/abaco", triple.SubjectUri);
            Assert.Null(triple.ObjectLiteral);
            Assert.Equal("x:lemmata/abaco", triple.ObjectUri);
            Assert.Equal(sid, triple.Sid);
            Assert.Null(triple.Tag);

            // x:forms/abaco x:hasForm "ábaco"
            triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "x:hasForm");
            Assert.NotNull(triple);
            Assert.Equal("x:forms/abaco", triple.SubjectUri);
            Assert.Equal("ábaco", triple.ObjectLiteral);
            Assert.Null(triple.ObjectUri);
            Assert.Equal(part.Id + "|u-lemma@abaco", triple.Sid);
            Assert.Null(triple.Tag);

            // x:forms/abaco x:hasIxForm "abaco"
            triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "x:hasIxForm");
            Assert.NotNull(triple);
            Assert.Equal("x:forms/abaco", triple.SubjectUri);
            Assert.Equal("abaco", triple.ObjectLiteral);
            Assert.Null(triple.ObjectUri);
            Assert.Equal(part.Id + "|lemma@abaco", triple.Sid);
            Assert.Null(triple.Tag);

            // x:forms/abaco x:hasPOS "sm"
            triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "x:hasPOS");
            Assert.NotNull(triple);
            Assert.Equal("x:forms/abaco", triple.SubjectUri);
            Assert.Equal("sm", triple.ObjectLiteral);
            Assert.Null(triple.ObjectUri);
            Assert.Equal(part.Id + "|pos@abaco", triple.Sid);
            Assert.Null(triple.Tag);

            // x:forms/abaco x:hasVariantForm "ábbaco"
            triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "x:hasVariantForm");
            Assert.NotNull(triple);
            Assert.Equal("x:forms/abaco", triple.SubjectUri);
            Assert.Equal("ábbaco", triple.ObjectLiteral);
            Assert.Null(triple.ObjectUri);
            Assert.Equal(part.Id + "|u-variant@abaco", triple.Sid);
            Assert.Null(triple.Tag);

            // x:forms/abaco x:hasIxVariantForm "abbaco"
            triple = set.Triples.FirstOrDefault(
                t => t.PredicateUri == "x:hasIxVariantForm");
            Assert.NotNull(triple);
            Assert.Equal("x:forms/abaco", triple.SubjectUri);
            Assert.Equal("abbaco", triple.ObjectLiteral);
            Assert.Null(triple.ObjectUri);
            Assert.Equal(part.Id + "|variant@abaco", triple.Sid);
            Assert.Null(triple.Tag);
        }
    }
}
