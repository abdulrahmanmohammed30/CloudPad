using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPad.Core.Entities;

namespace CloudPad.Core.RepositoryContracts
{
    public interface IResourceRepository
    {
        Task<Resource> CreateAsync(Resource resource);
        Task<List<Resource>> GetAllAsync(int userId, Guid noteId);
        Task DeleteAsync(int userId, Guid noteId, Guid resourceId);
        Task<Resource?> GetByIdAsync(int userId, Guid noteId, Guid resourceId);
        public Task<Resource> UpdateAsync(Resource resource);
        Task<bool> ExistsAsync(int userId, Guid noteId, Guid resourceId);
    }
}
