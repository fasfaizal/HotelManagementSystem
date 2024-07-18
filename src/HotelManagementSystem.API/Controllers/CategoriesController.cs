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
        [HttpPost]
        public async Task<IActionResult> Post(CategoryRequest categoryRequest)
        {
            var category = await _categoriesService.CreateAsync(categoryRequest);
            return StatusCode(StatusCodes.Status201Created, category);
        }


        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>
        /// A 200 OK response with the category object matching the provided ID,
        /// or a 404 Not Found response if no category is found.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var category = await _categoriesService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
    }
}
