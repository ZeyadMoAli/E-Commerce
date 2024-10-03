using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification()
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }

    public ProductsWithTypesAndBrandsSpecification(int id):base(product => product.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}