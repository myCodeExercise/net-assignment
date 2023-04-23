using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRepository<User, Guid> _userRepository;
        private const string UserNotExistMessage = "User does not exist";

        public UserController(ILogger<UserController> logger, IRepository<User, Guid> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserModelDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult Get(Guid id)
        {
            _logger.LogInformation("Received get request for {id}", id);

            try
            {
                var user = _userRepository.Read(id);

                _logger.LogDebug("Returning user {@user}", user);
                return Ok(user);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Accepted)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
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

        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.Accepted)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public IActionResult Put(UserModelDto user)
        {
            try
            {
                _userRepository.Create(new User { UserId = user.Id, UserName = user.Name, Birthday = user.Birthdate });
                return Accepted();
            }
            catch
            {
                return BadRequest("User already exists");
            }
        }

        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
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