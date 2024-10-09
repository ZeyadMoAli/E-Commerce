using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Core.Entities;

namespace Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams)
        :base(x=> 
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.Contains(productSpecParams.Search))&&
            (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId)&&
            (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId)
        )
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);
        if (!string.IsNullOrEmpty(productSpecParams.Sort))
        {
            switch (productSpecParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }
        else
        {
            AddOrderBy(x => x.Name);

        }

    }


    public ProductsWithTypesAndBrandsSpecification(int id):base(product => product.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}