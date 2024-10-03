using API.Dtos;
using AutoMapper;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[ApiController, Route("api/[controller]")]
public class ProductController: ControllerBase
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductBrand> _productBrandRepository;
    private readonly IGenericRepository<ProductType> _productTypeRepository;
    private readonly IMapper _mapper;
    public ProductController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _productBrandRepository = productBrandRepository;
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
    {
        var spec = new ProductsWithTypesAndBrandsSpecification();
        var products =await _productRepository.ListAsync(spec);
        return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);

        var product = await _productRepository.GetEntityWithSpec(spec);
        return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        
        var brands = await _productBrandRepository.ListAllAsync();
        return Ok(brands);
    }
    
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        var types = await _productTypeRepository.ListAllAsync();
        return Ok(types);
    }
    
    
    
}