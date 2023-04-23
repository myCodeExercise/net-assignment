using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User, Guid> _userRepository;
        private const string UserNotExistMessage = "User does not exist";

        public UserController(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Get(Guid id)
        {
            try
            {
                var user = _userRepository.Read(id);
                return Ok(user);
            }
            catch
            {
                return NotFound();
            }
        }

        public IActionResult Post(UserModelDto user)
        {
            try
            {
                _userRepository.Update(new User { UserId = user.Id, UserName = user.Name, Birthday = user.Birthdate });
                return Accepted();
            }
            catch
            {
                return BadRequest(UserNotExistMessage);
            }
        }

        public IActionResult Put(UserModelDto user)
        {
            try
            {
                _userRepository.Create(new User { UserId = user.Id, UserName = user.Name, Birthday = user.Birthdate });
                return Accepted();
            }
            catch
            {
                return BadRequest(UserNotExistMessage);
            }
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
                _userRepository.Remove(new User { UserId = id });
                return Ok();
            }
            catch
            {
                return BadRequest(UserNotExistMessage);
            }
        }
    }
}