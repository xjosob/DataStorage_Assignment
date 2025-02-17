namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectEntity> CreateProjectAsync(ProjectEntity projectEntity);
        Task<IEnumerable<ProjectEntity>> GetProjectsAsync();
        Task<ProjectEntity> GetProjectByIdAsync(string id);
        Task<ProjectEntity> UpdateProjectAsync(ProjectEntity projectEntity);
        Task<ProjectEntity> DeleteProjectAsync(string id);
    }
}
