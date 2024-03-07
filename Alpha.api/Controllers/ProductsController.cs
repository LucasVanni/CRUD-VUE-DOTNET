using Alpha.api.Data;
using Alpha.api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Alpha.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;
        private static HttpClient _client = new HttpClient();
        private IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>        
        public ProductsController(
            ProductsContext context, 
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Consulta de Produtos
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize">Quantidade de produtos por página. O padrão é 10.</param>
        /// <param name="sortBy">Campo pelo qual os produtos serão ordenados. Deixe vazio para ordenação padrão.</param>
        /// <returns>Uma lista paginada de produtos.</returns>
        /// <remarks>
        /// Exemplo de uso:
        /// <code>
        /// GET /products?page=2&amp;pageSize=20&amp;sortBy=nome
        /// </code>
        /// </remarks>
        /// <response code="200">Uma listagem com os produtos</response>
        [HttpGet]
        [Route("/products")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10, string sortBy = "")
        {

            await sincronizar();

            IQueryable<Product> query = _context.Products;

            query = query.OrderBy(p => p.Id);

            if (sortBy.ToLower() == "price")
                query = query.OrderBy(p => p.Price);

            else if (sortBy.ToLower() == "price_desc")
                query = query.OrderByDescending(p => p.Price);

            var pagedProducts = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var product in pagedProducts)
            {
                product.ImageBase64 = "data:image/jpeg;base64," + product.ImageBase64;
            }

            int totalProducts = await _context.Products.CountAsync();

            return Ok(new { pagedProducts, totalProducts });
        }



        /// <summary>
        /// Salvar um produto
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /products
        ///     {
        ///         name: 'Logo',
        ///         price: 0,
        ///         barCode: '',
        ///         imageBase64: ''
        ///     }
        /// </remarks>
        [HttpPost]
        [Route("/products")]
        public async Task<ActionResult> CreateProduct(PostProductVM request)
        {
            var product = _mapper.Map<Product>(request);

            var lastProduct = await _context.Products.OrderByDescending(p => p.Id).FirstOrDefaultAsync();

            product.Id = (lastProduct != null) ? lastProduct.Id + 1 : 1;

            product.ImageBase64 = product.ImageBase64.Contains(",") ? product.ImageBase64.Split(',').ToList<string>()[1] : product.ImageBase64;

            await _context.Products.AddAsync(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Não foi possível inserir o produto: " + ex.Message);
            }

            var productToFakeStore = new
            {
                title = product.Name,
                price = product.Price.ToString(),
                description = "",
                image = product.Image,
                category = "api"
            };

            var ret = await _client.PostAsJsonAsync("https://fakestoreapi.com/products", productToFakeStore);

            return Ok(product);
        }

        /// <summary>
        /// Editar um produto
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     PUT /products
        ///     {
        ///         'id': '1'
        ///         name: '',
        ///         price: 0,
        ///         barCode: '',
        ///         imageBase64: ''
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [Route("/products")]
        public async Task<ActionResult> UpdateProduct(PutProductVM request)
        {
            var product = _mapper.Map<Product>(request);

            var dbProduct = await _context.Products.FindAsync(product.Id);

            if (dbProduct == null)
            {
                return NotFound("Produto não encontrado");
            }

            var imageBase64 = product.ImageBase64.Contains(",") ? product.ImageBase64.Split(',').ToList<string>()[1] : product.ImageBase64;

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ImageBase64 = imageBase64;
            dbProduct.BarCode = product.BarCode;

            try
            {
                _context.Update(dbProduct);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }
            

            return Ok(dbProduct);
        }


        /// <summary>
        /// Deletar um produto
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <remarks>
        /// Exemplo de uso:
        /// <code>
        /// DELETE /products?id=3
        /// </code>
        /// </remarks>
        /// <response code="200" />
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

        private async Task sincronizar()
        {
            var response = await _client.GetAsync("https://fakestoreapi.com/products");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(result))
                return;

            var products = JsonConvert.DeserializeObject<List<Product>>(result);

            var productsDbGeral = await _context.Products.ToListAsync();
            var listUpdate = productsDbGeral
                .Where(
                x => products.Any(
                    y => y.Id == x.Id)
            );
            if (listUpdate != null && listUpdate.Any())
                _context.Products.UpdateRange(listUpdate);

            var listCreate = products
                .Where(x =>
                    !productsDbGeral.Any(y =>
                        y.Id == x.Id
                ));

            if (listCreate != null && listCreate.Any())
            {
                await _context.Products.AddRangeAsync(listCreate);

                foreach (var product in listCreate)
                {
                    product.BarCode = "";
                    using var httClient = new HttpClient();
                    var imageBytes = await httClient.GetByteArrayAsync(product.Image);
                    product.ImageBase64 = Convert.ToBase64String(imageBytes);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
