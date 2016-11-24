using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using CMMI.Interfaces.Facade;
using CMMI.Models;
using CMMI.Models.DTO;
using log4net;
using Newtonsoft.Json;

namespace CMMI.Controllers
{
    public class ReviewController : ApiController
    {
        private readonly ILog _logger;
        private readonly IReviewFacade _facade;

        public ReviewController(IReviewFacade facade)
        {
            _logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
            _facade = facade;
        }

        /// <summary>
        /// Gets a list of restaurant reviews that have been submitted by a user.
        /// </summary>
        /// <remarks>This method returns all reviews submitted by a user based on the user's username, email or user Id. </remarks>
        /// <param name="user"></param>
        /// <response code="200">The request was successful. </response>
        /// <response code="404">No valid reviews were found based upon the search criteria supplied.</response>
        /// <returns>List or reviews. </returns>
        [HttpGet]
        public IHttpActionResult GetByUser([FromUri]UserRequest user)
        {
            _logger.Info($"Review Controller received request for reviews by user information.");
            try
            {
                var reviews = _facade.GetByUser(user).ToList();
                if (reviews.Count == 0)
                {
                    return NotFound();
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get reviews per restaurant. 
        /// </summary>
        /// <remarks>Returns a list of reviews for the restaurantId requested. If reviews exist the response will have HttpStatusCode 200. If 
        /// there are no valid reviews then the response will have HttpStatusCode 404. </remarks>
        /// <param name="restaurant"></param>
        /// <response code="200"></response>
        /// <response code="404">No valid reviews were found based upon the search criteria supplied.</response>
        /// <returns>List or reviews. </returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Review>))]
        public IHttpActionResult GetByRestaurant([FromUri]long restaurantId)
        {
            _logger.Info($"Review Controller received request for reviews by Restaurant.");
            try
            {
                var reviews = _facade.GetByRestaurantId(restaurantId).ToList();
                if (reviews.Count == 0)
                {
                    return NotFound();
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return InternalServerError();
            }
        }


        /// <summary>
        /// Adds a review for a restaurant. 
        /// </summary>
        /// <remarks>If the review is added successfully to the database a 200 reponse code is returned.</remarks>
        /// <param name="review"></param>
        /// <returns>HttpStatusCode.Ok(200) the review was successfully created.</returns>
        /// <response code="200">The review was successfully created.</response>
        /// <response code="400">The request is invalid. This is typically caused by a review being posted 
        /// for an invalid restaurant or user within the database. </response>
        [HttpPost]
        [ResponseType(typeof(Review))]
        public IHttpActionResult Add([FromBody]ReviewRequest review)
        {
            _logger.Info($"Review Controller received a request to post a review.");
            try
            {
                _facade.AddReviewForRestaurant(review);
                return Ok("The review has been added for the restaurant. ");
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


        /// <summary>
        /// Removes a review based upon its Id. 
        /// </summary>
        /// <remarks></remarks>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        /// <response code="200">The review has been deleted. </response>
        /// <repsonse code="400">The review was unable to be deleted. This is typically caused by another entity 
        /// already deleting the review or the review does not exist within the target database. </repsonse>
        [HttpPost]
        public IHttpActionResult Remove([FromBody]long reviewId)
        {
            _logger.Info($"Review controller received a request to Remove a record {reviewId}");
            if (reviewId <= 0) return BadRequest("Invalid Request, reviewId must be a non-negative integer. ");
            try
            {
                _facade.RemoveRestaurantReview(reviewId);
                return Ok("Review successfully deleted.");
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex);
                return BadRequest("Invalid Request. ");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}