using Business.Interfaces;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ProjectService(DataContext context) : IProjectService
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<ProjectEntity> CreateProjectAsync(ProjectEntity projectEntity)
        {
            if (string.IsNullOrEmpty(projectEntity.Id))
            {
                throw new InvalidOperationException("Project ID cannot be null or empty.");
            }
            await _context.Projects.AddAsync(projectEntity);
            await _context.SaveChangesAsync();
            return projectEntity;
        }

        // Read
        public async Task<IEnumerable<ProjectEntity>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(string id)
        {
            var projectEntity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            return projectEntity ?? null!;
        }

        // Update
        public async Task<ProjectEntity> UpdateProjectAsync(ProjectEntity projectEntity)
        {
            _context.Projects.Update(projectEntity);
            await _context.SaveChangesAsync();
            return projectEntity;
        }

        // Delete
        public async Task<bool> DeleteProjectAsync(string id)
        {
            var projectEntity = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (projectEntity == null)
            {
                return false;
            }
            _context.Projects.Remove(projectEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
