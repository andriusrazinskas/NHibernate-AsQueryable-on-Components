using FluentNHibernate.Mapping;
using NHibernate.AsQueryableOnComponent.Entities;

namespace NHibernate.AsQueryableOnComponent.Mappings;

public class ProductMap : ClassMap<Product>
{
    public ProductMap()
    {
        Id(x => x.Id);
        Map(x => x.Name);
        Map(x => x.Price);
        References(x => x.Store);
    }
}
