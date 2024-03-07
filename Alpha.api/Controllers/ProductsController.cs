using Alpha.api.Data;
using Alpha.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;

namespace Alpha.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;
        private static HttpClient _client = new HttpClient();
        private static HttpClient? _lastUsedClient = null;
        private static bool _isDataLoaded = false;

        public ProductsController(ProductsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/products")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, string sortBy = "")
        {
            var response = await _client.GetAsync("https://fakestoreapi.com/products");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();


            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var products = JsonConvert.DeserializeObject<List<Product>>(result);

            var productsDb = await _context.Products.ToListAsync();


            if (!_isDataLoaded || _client != _lastUsedClient)
            {
                if (products != null && products.Any())
                {
                    foreach (var product in products)
                    {
                        if (string.IsNullOrEmpty(product.BarCode))
                        {

                            product.BarCode = "ValorPadrão";
                        }
                    }

                    _context.Products.RemoveRange(productsDb);

                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();


                    _isDataLoaded = true;
                    _lastUsedClient = _client;
                }
            }

            IQueryable<Product> query = _context.Products;

            query = query.OrderBy(p => p.Id);

            if (sortBy.ToLower() == "price")
            {
                query = query.OrderBy(p => p.Price);
            } else if(sortBy.ToLower() == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }

            var pagedProducts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalProducts = await _context.Products.CountAsync();

            return Ok(new { pagedProducts, totalProducts });
        }


        [HttpPost]
        [Route("/products")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            var lastProduct = await _context.Products.OrderByDescending(p => p.Id).FirstOrDefaultAsync();

            product.Id = (lastProduct != null) ? lastProduct.Id + 1 : 1;

            await _context.Products.AddAsync(product);

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Não foi possível inserir o produto: " + ex.Message);
            }

                // TODO - Verificar o porque está dando 404
                //  var productToFakeStore = new
                //   {
                //     price = product.Price.ToString(),
                //     title = product.Name,
                //     description = "",
                //     image = product.Image,
                //     category = "api"
                //    };

                // await _client.PostAsJsonAsync("https://fakestoreapi.com/products", productToFakeStore);

            return Ok(product);
        }

        [HttpPut]
        [Route("/products")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);

            if (dbProduct == null)
            {
                return NotFound("Produto não encontrado");
            }

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.Image = product.Image;
            dbProduct.BarCode = product.BarCode;

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete]
        [Route("/products")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null)
            {
                return NotFound("Produto não encontrado");
            }

            _context.Products.Remove(dbProduct);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }



}
