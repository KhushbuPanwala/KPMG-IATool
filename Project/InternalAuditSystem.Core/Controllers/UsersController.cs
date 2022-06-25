using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Method for getting all users
        /// </summary>
        /// <param name="entityId">Id of auditableEntity</param>
        /// <returns>List of Users</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<UserAC>>> GetAllUsersListAsync(Guid entityId)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var userList = await _userRepository.GetAllUsersOfEntityAsync(entityId);
            return Ok(userList);
        }

        /// <summary>
        /// Update selected entity id for the current user 
        /// </summary>
        /// <param name="userDetails">User Details</param>
        /// <returns>Task</returns>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(204)]
        public async Task<ActionResult> UpdateSelectedEntityForCurrentUser([FromBody] UserAC userDetails)
        {
            //if model state is invalid 
            if (!ModelState.IsValid)
                return BadRequest();

            // set current logged in user id 
            userDetails.Id = Guid.Parse(HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);

            //if model state is valid update data
            await _userRepository.UpdateSelectedEntityForCurrentUserAsync(userDetails);
            return NoContent();

        }
    }
}
