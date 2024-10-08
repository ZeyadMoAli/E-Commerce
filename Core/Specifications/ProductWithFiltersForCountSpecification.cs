﻿using Core.Entities;

namespace Core.Specifications;

public class ProductWithFiltersForCountSpecification: BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductSpecParams productSpecParams) :base(x=> 
        (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.Contains(productSpecParams.Search))&&
        (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId)&&
        (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId)
    )
    { }
}