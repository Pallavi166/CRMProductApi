using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        //public ProductsController(IProductService productService)
        //{
        //    _productService = productService;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var products = await _productService.GetAllAsync();
        //    return Ok(products);
        //}


        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The requested product.</returns>
        /// <response code="200">Returns the requested product.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Product not found.</response>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {


            _logger.LogInformation("Fetching product with Id {Id}", id);
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }



        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="dto">The product information.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="201">Product created successfully.</response>
        /// <response code="400">Invalid product data.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Only administrators can create products.</response>

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {

            _logger.LogInformation("Creating a new product.");
            var product = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }




        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <param name="dto">The updated product information.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">Product updated successfully.</response>
        /// <response code="400">Invalid request or ID mismatch.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Only administrators can update products.</response>
        /// <response code="404">Product not found.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {

            _logger.LogInformation("Updating product with Id {Id}", id);
            if (id != dto.Id)
                return BadRequest("Id mismatch.");

            var updated = await _productService.UpdateAsync(dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">Product deleted successfully.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Only administrators can delete products.</response>
        /// <response code="404">Product not found.</response>

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            _logger.LogInformation("Deleting product with Id {Id}", id);
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }



        /// <summary>
        /// Retrieves a paginated list of all products.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of products.</returns>
        /// <response code="200">Returns the list of products.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {

            _logger.LogInformation("Fetching all products.");
            var products = await _productService.GetAllAsync(paginationParams);

            return Ok(products);
        }
    }
}