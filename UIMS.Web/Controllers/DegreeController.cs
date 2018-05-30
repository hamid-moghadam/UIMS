using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using AutoMapper;
using UIMS.Web.Models;
using UIMS.Web.Services;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Degree")]
    public class DegreeController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly DegreeService _degreeService;

        public DegreeController(IMapper mapper, DegreeService degreeService)
        {
            _mapper = mapper;
            _degreeService = degreeService;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DegreeInsertViewModel degreeInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var degree = _mapper.Map<Degree>(degreeInsertVM);

            if (await _degreeService.IsExistsAsync(x=>x.Name == degreeInsertVM.Name))
            {
                ModelState.AddModelError("Degree Exists", "این مقطع قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            await _degreeService.AddAsync(degree);
            await _degreeService.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DegreeViewModel),200)]
        public async Task<IActionResult> Get(int id)
        {
            var degree = await _degreeService.GetAsync(id);

            if (degree == null)
                return NotFound();

            return Ok(degree);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationViewModel<DegreeViewModel>), 200)]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            return Ok(await _degreeService.GetAll(page, pageSize));
        }

        //public async Task<IActionResult> Remove(int id)
        //{
        //    var degree = await _degreeService.GetAsync(x => x.Id == id);

        //    if (degree == null)
        //        return NotFound();

        //    _degreeService.Remove(degree);
        //    await _degreeService.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] DegreeUpdateViewModel degreeUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var degree = await _degreeService.GetAsync(x => x.Id == degreeUpdateVM.Id);
            if (await _degreeService.IsExistsAsync(x => x.Name == degreeUpdateVM.Name && x.Id != degreeUpdateVM.Id))
            {
                ModelState.AddModelError("Degree Exists", "این مقطع قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            degree = _mapper.Map(degreeUpdateVM, degree);
            _degreeService.Update(degree);
            await _degreeService.SaveChangesAsync();

            return Ok();
        }
    }
}