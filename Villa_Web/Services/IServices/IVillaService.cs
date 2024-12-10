﻿using Villa_Web.Models.DTO;

namespace Villa_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VillaCreateDto entity, string token);
        Task<T> UpdateAsync<T>(VillaUpdateDto entity, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
