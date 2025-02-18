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
            try
            {
                if (string.IsNullOrEmpty(projectEntity.Id))
                {
                    throw new InvalidOperationException("Project ID cannot be null or empty.");
                }
                await _context.Projects.AddAsync(projectEntity);
                await _context.SaveChangesAsync();
                return projectEntity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a project: {ex.Message}");
            }
            return null!;
        }

        // Read
        public async Task<IEnumerable<ProjectEntity>> GetProjectsAsync()
        {
            try
            {
                return await _context.Projects.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting projects: {ex.Message}");
                return [];
            }
        }

        public async Task<ProjectEntity> GetProjectByIdAsync(string id)
        {
            try
            {
                var projectEntity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
                if (projectEntity == null)
                {
                    throw new InvalidOperationException("Project not found.");
                }
                return projectEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting a project by ID: {ex.Message}");
                return null!;
            }
        }

        // Update
        public async Task<ProjectEntity> UpdateProjectAsync(ProjectEntity projectEntity)
        {
            try
            {
                _context.Projects.Update(projectEntity);
                await _context.SaveChangesAsync();
                return projectEntity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a project: {ex.Message}");
            }
            return null!;
        }

        // Delete
        public async Task<ProjectEntity> DeleteProjectAsync(string id)
        {
            try
            {
                var projectEntity =
                    await _context.Projects.FirstOrDefaultAsync(x => x.Id == id)
                    ?? throw new InvalidOperationException("Project not found.");
                _context.Projects.Remove(projectEntity);
                await _context.SaveChangesAsync();
                return projectEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a project: {ex.Message}");
                return null!;
            }
        }
    }
}
