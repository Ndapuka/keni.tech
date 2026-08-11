using ApplicationLayer.DTOs.Categories;
using ApplicationLayer.ServiceContracts;
using AutoMapper;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.RepositoryContracts;


namespace ApplicationLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);

            if (existingCategory is not null)
                throw new InvalidOperationException("A category with the same name already exists.");

            var category = _mapper.Map<Category>(request);

            await _categoryRepository.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(UpdateCategoryRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category is null)
                throw new KeyNotFoundException("Category not found.");

            _mapper.Map(request, category);

            _categoryRepository.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category is null)
                throw new KeyNotFoundException("Category not found.");

            _categoryRepository.Delete(category);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category is null)
                return null;

            return _mapper.Map<CategoryResponseDto>(category);
        }
    }
}
