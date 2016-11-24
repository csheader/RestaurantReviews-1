using System;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using CMMI.Interfaces.Facade;
using CMMI.Models;
using log4net;

namespace CMMI.Controllers
{
    public class UserController : ApiController
    {
        private readonly ILog _logger;
        private readonly IUserFacade _facade;

        public UserController(IUserFacade facade)
        {
            _logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
            _facade = facade;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <remarks>This method adds a user to the database. This was created for testing purposes. No constraints on the table. </remarks>
        /// <param name="user"></param>
        /// <returns>HttpStatusCode 200 when the user is created. </returns>
        /// <response code="200">The user has been successfully created.</response>
        /// <response code="400">Unable to insert the new user into the database.</response>
        /// <response code="409">The user name already exists within the database and could not be inserted. </response>
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        public IHttpActionResult AddNewUser([FromBody]User user)
        {
            if (user.UserName == null || user.UserId != 0)
                return BadRequest("A userName must be supplied and the supplied UserId must be empty or 0. ");
            _logger.Info($"User Controller recieved request to create a new user: {user.UserName}");
            try
            {
                var existingUser = _facade.GetUser(user);
                if (existingUser != null)
                {
                    return Conflict();
                }
                _facade.AddUser(user);
                return Ok("User successfully created. ");
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex);
                return BadRequest("Invalid Request. ");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return InternalServerError();
            }
        }
    }
}