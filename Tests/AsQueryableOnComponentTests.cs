using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.AsQueryableOnComponent.Entities;
using NHibernate.AsQueryableOnComponent.Mappings;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Tests
{
    public class AsQueryableComponentTests
    {
        private ISessionFactory sessionFactory;

        [SetUp]
        public void Setup()
        {
            sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    var superMart = new Store { Name = "SuperMart" };

                    var potatoes = new Product { Name = "Potatoes", Price = 3.60 };
                    var fish = new Product { Name = "Fish", Price = 4.49 };


                    superMart.AddProduct(potatoes);
                    superMart.AddProduct(fish);

                    session.SaveOrUpdate(superMart);

                    transaction.Commit();
                }
            }
        }

        [Test]
        public void ShouldReturnEntitiesNames_WithAsQueryable()
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    var store = session.Query<Store>().First();

                    // Products are mapped as collection of components - throws QueryException
                    var productNames = store.Products.AsQueryable().Select(p => p.Name).ToArray();

                    Assert.That(productNames, Is.EquivalentTo(new[] { "Potatoes", "Fish" }));
                }
            }
        }

        [Test]
        public void ShouldReturnEntitiesNames_WithoutAsQueryable()
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    var store = session.Query<Store>().First();

                    var productNames = store.Products.Select(p => p.Name).ToArray();

                    Assert.That(productNames, Is.EquivalentTo(new[] { "Potatoes", "Fish" }));
                }
            }
        }

        private const string DbFile = "firstProgram.db";

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(DbFile))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<StoreMap>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists(DbFile))
                File.Delete(DbFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .Create(false, true);
        }
    }
}