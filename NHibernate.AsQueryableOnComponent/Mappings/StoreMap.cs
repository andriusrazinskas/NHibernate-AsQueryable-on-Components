using FluentNHibernate.Mapping;
using NHibernate.AsQueryableOnComponent.Entities;

namespace NHibernate.AsQueryableOnComponent.Mappings;

public class StoreMap : ClassMap<Store>
{
    public StoreMap()
    {
        Id(x => x.Id);
        Map(x => x.Name);

        HasMany(x => x.Products)
            .Cascade.All()
            .LazyLoad()
            .Component(x =>
            {
                x.Map(y => y.Name);
                x.Map(y => y.Price);
                x.ParentReference(y => y.Store);
            });
    }
}
