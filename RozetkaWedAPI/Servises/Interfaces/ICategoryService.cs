﻿using RozetkaWedAPI.Models;

namespace RozetkaWedAPI.Servises.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task<Category?> UpdateAsync(int id, Category category);
        Task<bool> DeleteAsync(int id);
        Task<Category> CreateAsync(Category category);
    }
}