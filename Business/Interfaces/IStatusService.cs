using Data.Entities;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusTypes>> GetStatusTypesAsync();
    }
}
