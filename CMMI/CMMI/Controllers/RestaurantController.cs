using CMMI.Models;
using Autofac;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using CMMI.Interfaces.Facade;
using CMMI.Models.DTO;

namespace CMMI.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly ILog _logger;
        private readonly IRestaurantFacade _facade;

        public RestaurantController(IRestaurantFacade facade)
        {
            _logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
            _facade = facade;
        }

        /// <summary>
        /// Add a restaurant to the database.  
        /// </summary>
        /// <remarks>If the restaurant is added to the database http status code 200 is returned. 
        /// If the restaurant already exists http status code 409 (conflict) is returned.</remarks>
        /// <param name="restaurant">Constructed Restaurant.</param>
        /// <returns>HttpStatusCode.Ok(200) if the restaurant is successfully created.</returns>
        /// <response code="200">The restaurant has been succesfully created. </response>
        /// <response code="409">The restaurant record already exists. </response>
        [HttpPost]
        public IHttpActionResult Add([FromBody]Restaurant restaurant)
        {
            try
            {
                _logger.Info($"Add method called on the Restaurant controller.");
                var existingRestaurant = _facade.GetExistingRestaurant(restaurant);
                if (existingRestaurant != null)
                {
                    _logger.Info("No restaurant was found matching the request. ");
                    return Conflict();
                }
                _facade.AddRestaurant(restaurant);
                return Ok("Restaurant created successfully in the database. ");
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Returns restaurants within a certain city or zip code. 
        /// </summary>
        /// <remarks>Accepts a restaurant request (comprised of RestaurantId, ZipCode and / or City) through query string parameters. </remarks>
        /// <param name="request"></param>
        /// <returns>List of restaurants within a certain area.</returns>
        /// <response code="200">The request was successful. </response>
        /// <response code="404">No restaurants match the search criteria supplied. </response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Restaurant>))]
        public IHttpActionResult Get([FromUri]RestaurantRequest request)
        {
            try
            {
                _logger.Info("Request received for restaurants by address properties. ");
                var restaurants = _facade.GetAllRestaurantsByAddress(request).ToList();
                if (restaurants.Count == 0)
                {
                    _logger.Info("No Restaurants found matching the request. ");
                    return NotFound();
                }
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return InternalServerError();
            }
        }
    }
}