using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using UIMS.Web.Services;
using AutoMapper;
using Swashbuckle.AspNetCore.SwaggerGen;
using UIMS.Web.Extentions;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Semester")]
    public class SemesterController : ApiController
    {

        private readonly SemesterService _semesterService;
        private readonly IMapper _mapper;

        public SemesterController(SemesterService semesterService, IMapper mapper)
        {
            _semesterService = semesterService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(200,typeof(PaginationViewModel<SemesterViewModel>))]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            return Ok(await _semesterService.GetAll(page, pageSize));
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, typeof(SemesterViewModel))]
        public async Task<IActionResult> Get(int id)
        {
            var semester = await _semesterService.GetAsync(id);
            if (semester == null)
                return NotFound();

            return Ok(semester);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SemesterInsertViewModel semesterInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!semesterInsertVM.Name.IsSemester())
            {
                ModelState.AddModelError("Semester", "نیمسال تحصیلی وارد شده صحیح نیست.");
                return BadRequest(ModelState);
            }

            if (await _semesterService.IsExistsAsync(x=>x.Name == semesterInsertVM.Name))
            {
                ModelState.AddModelError("Semester", "نیمسال تحصیلی قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            var result  = await _semesterService.AddAsync(semesterInsertVM);
            result.Enable = false;
            await _semesterService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetCurrent(int id)
        {
            var semester = await _semesterService.GetAsync(x=>x.Id == id);
            if (semester == null)
                return NotFound();

            await _semesterService.SetCurrentAsycn(semester);
            await _semesterService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SemesterUpdateViewModel semesterUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var semester = await _semesterService.GetAsync(x => x.Id == semesterUpdateVM.Id);

            if (semester == null)
                return NotFound();

            var isSemesterExists = await _semesterService.IsExistsAsync(x => x.Name == semesterUpdateVM.Name && x.Id != semesterUpdateVM.Id);
            if(isSemesterExists)
            {
                ModelState.AddModelError("Semester", "این نیمسال قبلا در سیستم ثبت شده است.");
                return BadRequest(ModelState);
            }

            semester = _mapper.Map(semesterUpdateVM, semester);
            _semesterService.Update(semester);

            await _semesterService.SaveChangesAsync();
            return Ok();

        }
    }
}