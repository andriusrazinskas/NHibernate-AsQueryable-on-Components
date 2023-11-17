namespace NHibernate.AsQueryableOnComponent.Entities;

public class Store
{
    public virtual int Id { get; protected set; }
    public virtual string Name { get; set; }
    public virtual IList<Product> Products { get; set; }

    public Store()
    {
        Products = new List<Product>();
    }

    public virtual void AddProduct(Product product)
    {
        product.Store = this;
        Products.Add(product);
    }
}
