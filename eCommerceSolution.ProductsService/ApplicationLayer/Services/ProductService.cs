using ApplicationLayer.DTOs.Products;
using ApplicationLayer.ServiceContracts;
using AutoMapper;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;

namespace ApplicationLayer.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
            throw new KeyNotFoundException("Category not found.");

        var product = _mapper.Map<Product>(request);

        await _productRepository.AddAsync(product);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> UpdateProductAsync(UpdateProductRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
            throw new KeyNotFoundException("Category not found.");

        _mapper.Map(request, product);

        _productRepository.Update(product);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new KeyNotFoundException("Product not found.");

        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto?> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            return null;

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(Guid categoryId)
    {
        var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetFeaturedProductsAsync()
    {
        var products = await _productRepository.GetFeaturedProductsAsync();

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string keyword)
    {
        var products = await _productRepository.SearchProductsAsync(keyword);

        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }
}