using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryRequest">The request object containing the category details.</param>
        /// <returns>
        /// A 201 Created response with the created category object.
        /// </returns>
        /// <response code="201">Returns the created category object.</response>
        /// <response code="400">If the categoryRequest is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> Post(CategoryRequest categoryRequest)
        {
            var category = await _categoriesService.CreateAsync(categoryRequest);
            return StatusCode(StatusCodes.Status201Created, category);
        }
    }
}
