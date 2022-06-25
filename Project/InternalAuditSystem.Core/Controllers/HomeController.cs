using InternalAuditSystem.Repository.ApplicationClasses.User;
using InternalAuditSystem.Repository.Repository.UserRepository;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace InternalAuditSystem.Core.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IHttpContextAccessor _httpContextAccessor;
        public IUserRepository _userRepository;
        public HomeController(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Add new user on login , set user details and return main view
        /// </summary>
        /// <returns>Main page view</returns>
        public ViewResult Index()       
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            if (currentUser.Identity.IsAuthenticated)
            {
                var userDetails = new UserAC()
                {
                    EmailId = currentUser.Identity.Name,
                };
                // get or add/update as a user if user not in sytem
                try
                {
                    var getUser = _userRepository.GetCurrentUserDetailsOnLoginAsync(userDetails);
                    getUser.Wait();
                    if (getUser.Result != null)
                    {
                        // store user id in cookie to get access of user 
                        HttpContext.Response.Cookies.Append(StringConstant.CurrentUserIdKey, getUser.Result.Id.ToString());
                    }
                }
                catch(Exception)
                {
                    throw;
                }
                
            }
            return View();
        }
    }
}