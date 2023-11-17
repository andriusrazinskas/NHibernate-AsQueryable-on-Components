namespace NHibernate.AsQueryableOnComponent.Entities;

public class Product
{
    public virtual int Id { get; protected set; }
    public virtual string Name { get; set; }
    public virtual double Price { get; set; }
    public virtual Store Store { get; set; }
}