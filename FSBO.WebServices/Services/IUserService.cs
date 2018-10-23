using System.Threading.Tasks;
using System.Collections.Generic;

using FSBO.DAL;
using Dto = FSBO.WebServices.Models.Dto;

namespace FSBO.WebServices.Services
{
    public interface IUserService
    {
        // Subscriber methods
        //IEnumerable<Subscriber> GetSubscribers(bool onlyActive = true);
        
        
        User Authenticate(string username, string password);

        IEnumerable<User> GetAll();

        User GetById(int id);

        Task<User> Create(Dto.RegistrationData registrationData);

        void Update(User userParam, string password = null);

        void Delete(int id);
        
    }
}