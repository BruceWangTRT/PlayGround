using ClassLibraryA.Model;
using FluentNHibernate.Mapping;

namespace ClassLibraryA.Mapping
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Table("Product");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Category);
            Map(x => x.Discontinued);
        }
    }
}
