using Data.Entities;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<StatusTypes?> CreateStatusTypeAsync(StatusTypes statusType);
        Task<IEnumerable<StatusTypes>> GetStatusTypesAsync();
        Task<StatusTypes?> GetStatusTypeByIdAsync(int id);
        Task<StatusTypes?> UpdateStatusTypeAsync(StatusTypes statusType);
        Task<StatusTypes?> DeleteStatusTypeAsync(int id);
    }
}
