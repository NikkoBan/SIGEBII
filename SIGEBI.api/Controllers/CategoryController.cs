//using Microsoft.AspNetCore.Mvc;
//using SIGEBI.Application.Contracts.Service;
//using SIGEBI.Application.Dtos.CategoryDto;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace SIGEBI.api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CategoryController : ControllerBase
//    {
//        private readonly ICategoryService _categoryService;

//        public CategoryController(ICategoryService categoryService)
//        {
//            _categoryService = categoryService;
//        }
//        // GET: api/<CategoryController>
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await _categoryService.GetAllAsync();
//            return result.Success ? Ok(result.Data) : StatusCode(500, result.Message);
//        }


//        // GET api/<CategoryController>/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _categoryService.GetByIdAsync(id);
//            return result.Success ? Ok(result.Data) : NotFound(result.Message);
//        }


//        // POST api/<CategoryController>
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
//        {
//            var result = await _categoryService.CreateAsync(dto);
//            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
//        }


//        // PUT api/<CategoryController>/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO dto)
//        {
//            var result = await _categoryService.UpdateAsync(id, dto);
//            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
//        }

//        // DELETE api/<CategoryController>/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _categoryService.DeleteAsync(id);
//            return result.Success ? NoContent() : NotFound(result.Message);
//        }
//    }
//}
