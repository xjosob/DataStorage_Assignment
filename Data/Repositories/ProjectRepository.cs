using Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProjectRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<ProjectEntity?> CreateProjectAsync(ProjectEntity projectEntity)
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
            return null;
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

        public async Task<ProjectEntity?> GetProjectByIdAsync(string id)
        {
            try
            {
                return await _context
                    .Projects.Include(p => p.Status)
                    .Include(p => p.Customer)
                    .Include(p => p.User)
                    .Include(p => p.Product)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while getting project with ID {id}: {ex.Message}"
                );
                return null;
            }
        }

        // Update
        public async Task<ProjectEntity?> UpdateProjectAsync(ProjectEntity projectEntity)
        {
            try
            {
                var entity = await _context.Projects.FirstOrDefaultAsync(x =>
                    x.Id == projectEntity.Id
                );
                if (entity != null)
                {
                    _context.Entry(entity).CurrentValues.SetValues(projectEntity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine($"Project with ID {projectEntity.Id} not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while updating project with ID {projectEntity.Id}: {ex.Message}"
                );
                return null;
            }
        }

        // Delete
        public async Task<ProjectEntity> DeleteProjectAsync(string id)
        {
            try
            {
                var entity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
                if (entity != null)
                {
                    _context.Projects.Remove(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine($"Project with ID {id} not found.");
                return null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while deleting project with ID {id}: {ex.Message}"
                );
                return null!;
            }
        }
    }
}
