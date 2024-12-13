using System.Reflection.Metadata.Ecma335;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commads.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Application.Restaurants.Commads.DeleteRestaurant;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Restaurants.Application.Restaurants.Commands.EditRestaurant;
using Microsoft.AspNetCore.Authorization;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets all restaurants available.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = PolicyNames.AtLeast2)]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
            return Ok(restaurants);
        }

        /// <summary>
        /// Returns a restaurant by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.HasNationality)]
        public async Task<ActionResult<RestaurantDto?>> GetById([FromRoute] int id)
        {
            var restaurants = await mediator.Send(new GetRestaurantByIdQuery(id));
            return Ok(restaurants);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
        {
            User.IsInRole("");
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResturant([FromRoute] int id)
        {
            await mediator.Send(new DeleteRestaurantCommand(id));
            return NoContent();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditRestaurant(EditRestaurantCommand command, [FromRoute] int id)
        {
            command.Id = id;
            await mediator.Send(command);
            return NoContent();


        }
    }

}
