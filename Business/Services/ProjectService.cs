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
            await _context.Projects.AddAsync(projectEntity);
            await _context.SaveChangesAsync();
            return projectEntity;
        }

        // Read
        public async Task<IEnumerable<ProjectEntity>> GetProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(int id)
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
        public async Task<bool> DeleteProjectAsync(int id)
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
