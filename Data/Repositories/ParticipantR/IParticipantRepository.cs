using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ParticipantR
{
    public interface IParticipantRepository
    {
        // General 
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<Participant[]> GetAllSystemUsersAsync();
        Task<Participant> GetSystemUserAsync(string email);
        Task<Participant[]> GetSystemUsersAsyncByName(string name);
    }
}
