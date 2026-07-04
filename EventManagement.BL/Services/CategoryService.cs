
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;

namespace EventManagement.BL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> CreateAsync(string name)
        {
            var category = new Category { Name = name };
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<Category?> UpdateAsync(int id, string name)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            category.Name = name;
            await _categoryRepository.UpdateAsync(category);
            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException($"Category with id {id} not found");

            await _categoryRepository.DeleteAsync(id);
        }
    }
}