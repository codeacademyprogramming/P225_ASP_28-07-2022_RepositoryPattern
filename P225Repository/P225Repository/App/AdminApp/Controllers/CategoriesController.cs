using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P225Repository.Data.Entities;
using P225Repository.Data.Repositories;
using P225Repository.DTOs;
using P225Repository.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225Repository.App.AdminApp.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CategoryPostDto categoryPostDto)
        {
            if (await _categoryRepository.IsExistAsync(c => !c.IsDeleted && c.Name == categoryPostDto.Name))
            {
                return BadRequest();
            }

            Category category = _mapper.Map<Category>(categoryPostDto);
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("getall/{pageIndex}")]
        public async Task<IActionResult> GetAll(int pageIndex = 1)
        {
            List<CategoryListDto> categoryListDtos = _mapper.Map<List<CategoryListDto>>(await _categoryRepository.GetAllAsync(c => !c.IsDeleted, "Parent", "Children"));

            PagenetedListDto<CategoryListDto> pagenetedListDto = new PagenetedListDto<CategoryListDto>(categoryListDtos, pageIndex, 4);

            return Ok(pagenetedListDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!await _categoryRepository.IsExistAsync(c=>!c.IsDeleted && c.Id == id))
            {
                return NotFound();
            }

            CategoryGetDto categoryGetDto = _mapper.Map<CategoryGetDto>(await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id,"Children"));

            return Ok(categoryGetDto);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] CategoryPutDto categoryPutDto)
        {
            if (id != categoryPutDto.Id) return BadRequest();

            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (category == null) return NotFound();

            category.Name = categoryPutDto.Name;
            category.IsMain = categoryPutDto.IsMain;
            category.ParentId = categoryPutDto.ParentId;
            category.Name = categoryPutDto.Name;
            category.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _categoryRepository.CommitAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (category == null) return NotFound();

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _categoryRepository.CommitAsync();

            return NoContent();
        }
    }
}
