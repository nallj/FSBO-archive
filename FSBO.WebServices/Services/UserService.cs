using System;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper.QueryableExtensions;

using FSBO.DAL;
using Dto = FSBO.WebServices.Models.Dto;

namespace FSBO.WebServices.Services
{
    public class UserService : IUserService
    {
        private IFsboContext DbContext;

        public UserService(IFsboContext dbContext)
        {
            DbContext = dbContext;
        }


        //public IEnumerable<Subscriber> GetSubscribers(bool onlyActive = true)
        //{
        //    var subscribers = DbContext.Subscribers
        //        .Where(s => s.IsActive == onlyActive)
        //        .ProjectTo<Subscriber>();

        //    return subscribers;
        //}

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = DbContext.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.Hash, user.Salt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return DbContext.Users;
        }

        public User GetById(int id)
        {
            return DbContext.Users.Find(id);
        }

        public async Task<User> Create(Dto.RegistrationData registrationData)
        {
            // Create user entity from registration data.
            var dbUser = Mapper.Map<User>(registrationData);

            // Validation.
            if (string.IsNullOrWhiteSpace(registrationData.Password))
                throw new Models.AppException("Password is required");

            if (DbContext.Users.Any(x => x.Username == registrationData.Username))
                throw new Models.AppException("Username " + dbUser.Username + " is already taken");

            // If the area requested by the customer is in the database then replace the suggestion with a reference to that area.
            var matchingDbArea = DbContext.Areas
                .FirstOrDefault(a => a.Value
                    .Contains(registrationData.FirstZip));
            if (matchingDbArea != null)
            {
                dbUser.UserSuggestions = null;
                dbUser.UserAreas = new List<UserArea>()
                {
                    new UserArea()
                    {
                        AreaId = matchingDbArea.AreaId
                    }
                };
            }

            // Generate hash and salt from provided password.
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(registrationData.Password, out passwordHash, out passwordSalt);

            dbUser.Hash = passwordHash;
            dbUser.Salt = passwordSalt;

            // Add user entity and save.
            DbContext.Users.Add(dbUser);
            await DbContext.SaveChangesAsync();

            // Create user payment method entity and associate it to the freshly created user (and visa versa).
            var dbUserPaymentMethod = Mapper.Map<UserPaymentMethod>(registrationData);
            dbUserPaymentMethod.UserId = dbUser.UserId;
            dbUser.UserPaymentMethod = dbUserPaymentMethod;

            // Add user payment method entity and save.
            DbContext.UserPaymentMethods.Add(dbUserPaymentMethod);
            await DbContext.SaveChangesAsync();

            return dbUser;
        }

        public void Update(User userParam, string password = null)
        {
            var user = DbContext.Users.Find(userParam.UserId);

            if (user == null)
                throw new Models.AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (DbContext.Users.Any(x => x.Username == userParam.Username))
                    throw new Models.AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.Username = userParam.Username;
            user.Email = userParam.Email;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.Hash = passwordHash;
                user.Salt = passwordSalt;
            }

            //DbContext.Users.Update(user);
            DbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = DbContext.Users.Find(id);
            if (user != null)
            {
                DbContext.Users.Remove(user);
                DbContext.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    }
}
