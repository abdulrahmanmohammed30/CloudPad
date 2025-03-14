﻿using CloudPad.Core.Entities;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CloudPad.Infrastructure.Repositories;

public class CountryRepository(AppDbContext context):ICountryRepository
{
    private readonly AppDbContext _context = context;
        
    public async Task<List<Country>> GetAllCountriesAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryByIdAsync(short id)
    {
        return await _context.Countries.FindAsync(id);
    }

    public async Task<Country?> GetCountryByNameAsync(string name)
    {
       return await _context.Countries.FirstOrDefaultAsync(c=>c!.EnglishName.ToLower().Equals(name.ToLower()));
    }

    public async Task<bool> ExistsAsync(short id)
    {
        return await _context.Countries.AnyAsync(c=>c!.CountryId == id);
    }
}