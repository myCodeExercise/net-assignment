using RandomTestValues;
using Work.Database;
using Work.Implementation;

namespace Work.Tests
{
    public class UserRepositoryTests
    {
        private readonly MockDatabase _mockDatabase;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _mockDatabase = new MockDatabase(1);
            _userRepository = new UserRepository(_mockDatabase);
        }

        [Fact]
        public void Create_WhenNewUser_CreatesUser()
        {
            //Arrange
            var user = RandomValue.Object<User>();

            //Act
            _userRepository.Create(user);

            //Assert
            _mockDatabase.Users.TryGetValue(user.UserId, out var userFromDb);
            Assert.Equal(userFromDb, user);
        }

        [Fact]
        public void Create_WhenUserExists_ThrowsException()
        {
            //Arrange
            var user = RandomValue.Object<User>();
            _userRepository.Create(user);

            //Act
            var ex = Record.Exception(() => _userRepository.Create(user));

            //Assert
            Assert.NotNull(ex);
        }

        [Fact]
        public void Read_WhenUserExists_ReturnsUser()
        {
            //Arrange            
            var userToRead = _mockDatabase.Users.First();

            //Act
            var user = _userRepository.Read(userToRead.Key);

            //Assert
            Assert.Equal(userToRead.Value, user);
        }

        [Fact]
        public void Read_WhenUserNotExists_ThrowsException()
        {
            //Arrange

            //Act
            var ex = Record.Exception(() => _userRepository.Read(RandomValue.Guid()));

            //Assert
            Assert.NotNull(ex);
        }

        [Fact]
        public void Remove_WhenUserExists_RemovesUser()
        {
            //Arrange            
            var userToRemove = RandomValue.Object<User>();
            _mockDatabase.Users.Add(userToRemove.UserId, userToRemove);

            //Act
            var ex = Record.Exception(() => _userRepository.Remove(userToRemove));

            //Assert
            Assert.Null(ex);
            Assert.False(_mockDatabase.Users.TryGetValue(userToRemove.UserId, out _));
        }

        [Fact]
        public void Remove_WhenUserNotExists_ThrowsException()
        {
            //Arrange            

            //Act
            var ex = Record.Exception(() => _userRepository.Remove(RandomValue.Object<User>()));

            //Assert
            Assert.NotNull(ex);
        }

        [Fact]
        public void Update_WhenUserExists_UpdatesUser()
        {
            //Arrange            
            var userToUpdate = RandomValue.Object<User>();
            _mockDatabase.Users.Add(userToUpdate.UserId, userToUpdate);

            var userExpected = new User { UserId = userToUpdate.UserId, UserName = RandomValue.String(), Birthday = RandomValue.DateTime() };
            //Act
            var ex = Record.Exception(() => _userRepository.Update(userExpected));

            //Assert
            Assert.Null(ex);

            _mockDatabase.Users.TryGetValue(userToUpdate.UserId, out var user);
            Assert.Equal(userExpected, user);
        }

        [Fact]
        public void Update_WhenUserNotExists_ThrowsException()
        {
            //Arrange            

            //Act
            var ex = Record.Exception(() => _userRepository.Update(RandomValue.Object<User>()));

            //Assert
            Assert.NotNull(ex);
        }
    }
}

