using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase _mockDatabase;

        public UserRepository(MockDatabase mockDatabase)
        {
            _mockDatabase = mockDatabase;
        }

        public void Create(User obj)
        {
            if (!_mockDatabase.Users.TryAdd(obj.UserId, obj))
            {
                throw new Exception("User is not created");
            }
        }

        public User Read(Guid key)
        {
            if (_mockDatabase.Users.TryGetValue(key, out var user))
            {
                return user;
            }

            throw new Exception("User missing");
        }

        public void Remove(User obj)
        {
            if (!_mockDatabase.Users.Remove(obj.UserId))
            {
                throw new Exception("User missing");
            }
        }

        public void Update(User obj)
        {
            if (_mockDatabase.Users.TryGetValue(obj.UserId, out var _))
            {
                _mockDatabase.Users[obj.UserId] = obj;
                return;
            }

            throw new Exception("User missing");
        }
    }
}
