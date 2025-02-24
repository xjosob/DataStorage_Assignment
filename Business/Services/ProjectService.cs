using Business.Interfaces;
using Data.Repositories;

namespace Business.Services
{
    public class ProjectService(ProjectRepository projectRepository) : IProjectService
    {
        private readonly ProjectRepository projectRepository = projectRepository;

        // Create
        public async Task<ProjectEntity?> CreateProjectAsync(ProjectEntity projectEntity)
        {
            return await projectRepository.CreateProjectAsync(projectEntity);
        }

        // Read
        public async Task<IEnumerable<ProjectEntity>> GetProjectsAsync()
        {
            return await projectRepository.GetProjectsAsync();
        }

        public async Task<ProjectEntity?> GetProjectByIdAsync(string id)
        {
            return await projectRepository.GetProjectByIdAsync(id);
        }

        // Update
        public async Task<ProjectEntity?> UpdateProjectAsync(ProjectEntity projectEntity)
        {
            return await projectRepository.UpdateProjectAsync(projectEntity);
        }

        // Delete
        public async Task<ProjectEntity?> DeleteProjectAsync(string id)
        {
            return await projectRepository.DeleteProjectAsync(id);
        }
    }
}
