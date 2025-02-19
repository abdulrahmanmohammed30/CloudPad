using NoteTakingApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.RepositoryContracts
{
    public interface IResourceRepository
    {
        Task<Resource> CreateAsync(Guid noteId, Resource resource);
        Task<List<Resource>> GetAllAsync(Guid noteId);
        Task<bool> DeleteAsync(Guid resourceId);
        Task<Resource?> GetByIdAsync(Guid resourceId);
        public Task UpdateAsync(Resource resource);
        Task<bool> ExistsAsync(Guid resourceId);
    }
}
