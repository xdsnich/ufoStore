using Microsoft.AspNetCore.Mvc;
using ufoShopBack.CRUDoperations;
using ufoShopBack.Data.Entities;
using ufoShopBack.Services;
namespace ufoShopBack.Controllers.ItemControllers
{
    [ApiController]
    [Route("api/[controller]")]
    
        public class ProductController : Controller
        {
            private readonly ProductCRUD _productCRUD;
            public ProductController(ProductCRUD productCRUD)
            {
                _productCRUD = productCRUD;
            }
            [HttpGet]
            public async Task<IActionResult> GetProduct()
            {
                var products = await _productCRUD.GetAsync();
                return Ok(products);
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetProductById(int id)
            {
                var product = await _productCRUD.GetAsync(id);
                return product != null ? Ok(product) : NotFound();
            }
            [HttpPost]
            public async Task<IActionResult> CreateUser([FromBody] Product product)
            {
                if (product == null) {
                    return BadRequest("all data should be entered");
                }
                await _productCRUD.CreateAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateUser(int id, [FromBody] Product product)
            {
                try
                {
                    var productFromDb = await _productCRUD.GetAsync(id);
                    if (productFromDb == null)
                    {
                        return NotFound();
                    }

                    await _productCRUD.UpdateAsync(product, id);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating product: {ex.Message}");
                    return StatusCode(500, "An error occurred while updating the product.");
                }

            }
            [HttpDelete]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var productFromDb = await _productCRUD.GetAsync(id);
                if (productFromDb == null)
                {
                    return NotFound();
                }
                await _productCRUD.DeleteAsync(id);
                return NoContent();
            }
        }
    }


