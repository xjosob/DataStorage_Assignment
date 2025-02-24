using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class StatusService(StatusTypeRepository statusTypeRepository) : IStatusService
    {
        private readonly StatusTypeRepository statusTypeRepository = statusTypeRepository;

        //Create
        public async Task<StatusTypes?> CreateStatusTypeAsync(StatusTypes statusType)
        {
            return await statusTypeRepository.AddStatusAsync(statusType);
        }

        //Read

        public async Task<IEnumerable<StatusTypes>> GetStatusTypesAsync()
        {
            return await statusTypeRepository.GetStatusAsync();
        }

        public async Task<StatusTypes?> GetStatusTypeByIdAsync(int id)
        {
            return await statusTypeRepository.GetStatusByIdAsync(id);
        }

        //Update
        public async Task<StatusTypes?> UpdateStatusTypeAsync(StatusTypes statusType)
        {
            return await statusTypeRepository.UpdateAsync(statusType);
        }

        //Delete
        public async Task<StatusTypes?> DeleteStatusTypeAsync(int id)
        {
            return await statusTypeRepository.DeleteAsync(id);
        }
    }
}
