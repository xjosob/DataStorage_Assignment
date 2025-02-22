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
                return await _context
                    .Projects.Include(p => p.Status)
                    .Include(p => p.Customer)
                    .Include(p => p.User)
                    .Include(p => p.Product)
                    .ToListAsync();
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
                var existingProject = await _context.Projects.FirstOrDefaultAsync(x =>
                    x.Id == projectEntity.Id
                );

                if (existingProject != null)
                {
                    _context.Entry(existingProject).CurrentValues.SetValues(projectEntity);

                    await _context.Entry(existingProject).Reference(p => p.Status).LoadAsync();
                    await _context.Entry(existingProject).Reference(p => p.Customer).LoadAsync();
                    await _context.Entry(existingProject).Reference(p => p.User).LoadAsync();

                    await _context.SaveChangesAsync();

                    return existingProject;
                }
                else
                {
                    Console.WriteLine("Project not found.");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the project: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
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
                _context.ChangeTracker.Clear();

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
