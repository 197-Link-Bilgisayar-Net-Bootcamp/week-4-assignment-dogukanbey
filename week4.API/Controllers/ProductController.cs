using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using week4.Data.Models;
using week4.Service.Dtos;
using week4.Service.Services;

namespace week4.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IMapper _mapper;

        private readonly IProductService _service;

        public ProductController(IMapper mapper, IProductService productService)
        {

            _mapper = mapper;
            _service = productService;
        }


        /// GET api/products
        [HttpGet]
        public async Task<IActionResult> All()
        {

            return Ok(await _service.GetAllAsync());
        }




        // GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var product = await _service.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ProductDto productDto)
        {



            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);


            return Ok();

        }



        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {

            await _service.UpdateAsync(_mapper.Map<Product>(productDto));


            return Ok();
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);

            await _service.RemoveAsync(product);


            return Ok();
        }






    }
}
