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

        [Fact]
        public void MapLemmaItem_Ok()
        {
            Reset();
            IGraphRepository repository = GetRepository();
            IndexHelper.AddRules(repository);
            NodeMapper mapper = new NodeMapper(repository);

            // item
            IItem item = new Item
            {
                Title = "abaco",
                Description = "An ancient calculating tool.",
                FacetId = "lemma",
                GroupId = "ABACUS",
                SortKey = "abaco",
                CreatorId = "creator",
                UserId = "user"
            };
            GraphSet set = mapper.MapItem(item);

            // TODO

            // part
            WordFormsPart part = new WordFormsPart
            {
                ItemId = item.Id,
                CreatorId = "zeus",
                UserId = "zeus",
            };
            part.Forms.Add(new WordForm
            {
                Lid = "ABACO",
                Lemma = "abaco",
                Pos = "sm",
                Variants = new List<VariantForm>(new VariantForm[]
                {
                    new VariantForm
                    {
                        Pos = "sm",
                        Value = "abbaco"
                    }
                })
            });
            IList<DataPin> pins = part.GetDataPins(item).ToList();
            set = mapper.MapPins(item, part,
                pins.Select(p => Tuple.Create(p.Name, p.Value)).ToList());

            // TODO
        }
    }
}
