﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMS.Web.DTO;
using AutoMapper;
using UIMS.Web.Services;
using UIMS.Web.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Presentation")]
    public class PresentationController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly PresentationService _presentationService;
        private readonly SemesterService _semesterService;
        private readonly CourseFieldService _courseFieldService;
        private readonly ProfessorService _professorService;
        private readonly BuildingClassService _buildingClassService;

        public PresentationController(IMapper mapper, PresentationService presentationService, SemesterService semesterService, CourseFieldService courseFieldService, ProfessorService professorService, BuildingClassService buildingClassService)
        {
            _mapper = mapper;
            _presentationService = presentationService;
            _semesterService = semesterService;
            _courseFieldService = courseFieldService;
            _professorService = professorService;
            _buildingClassService = buildingClassService;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PresentationInsertViewModel presentationInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _presentationService.IsExistsAsync(x=>x.Code == presentationInsertVM.Code))
            {
                ModelState.AddModelError("Presentation", "این کلاس طبق کد وارد شده قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            var professor = await _professorService.GetAsync(x => x.Id == presentationInsertVM.ProfessorId.Value);
            if (professor== null)
            {
                ModelState.AddModelError("Professor", "استاد مورد نظر یافت نشد");
                return BadRequest(ModelState);
            }
            if (!professor.User.Enable)
            {
                ModelState.AddModelError("Professor", "امکان اختصاص کلاس به استاد مورد نظر وجود ندارد");
                return BadRequest(ModelState);
            }

            if (!await _courseFieldService.IsExistsAsync(x=>x.Id == presentationInsertVM.CourseFieldId.Value))
            {
                ModelState.AddModelError("CourseField", "درس مورد نظر یافت نشد");
                return BadRequest(ModelState);
            }
            
            if (!await _buildingClassService.IsExistsAsync(x => x.Id == presentationInsertVM.BuildingClassId.Value))
            {
                ModelState.AddModelError("BuildingClass", "کلاس مورد نظر در سیستم یافت نشد");
                return BadRequest(ModelState);
            }

            var currentSemester = await _semesterService.GetCurrentAsycn();
            var presentation = _mapper.Map<Presentation>(presentationInsertVM);
            presentation.SemesterId = currentSemester.Id;
            await _presentationService.AddAsync(presentation);
            await _presentationService.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("{id}")]
        [SwaggerResponse(200,typeof(PresentationViewModel))]
        public async Task<IActionResult> Get(int id)
        {
            var present = await _presentationService.GetAsync(id);

            if (present == null)
                return NotFound();
            return Ok(present);

        }

        [HttpGet]
        [SwaggerResponse(200, typeof(PaginationViewModel<PresentationViewModel>))]
        public async Task<IActionResult> GetAll(int pageSize = 5, int page = 1)
        {
            return Ok(await _presentationService.GetAll(page, pageSize));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var present = await _presentationService.GetAsync(x => x.Id == id);

            if (present == null)
                return NotFound();

            _presentationService.Remove(present);

            await _presentationService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] PresentationUpdateViewModel presentationUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            if (await _presentationService.IsExistsAsync(x => x.Code == presentationUpdateVM.Code && x.Id != presentationUpdateVM.Id))
            {
                ModelState.AddModelError("Presentation", "این کلاس طبق کد وارد شده قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            if (!await _buildingClassService.IsExistsAsync(x => x.Id == presentationUpdateVM.BuildingClassId.Value))
            {
                ModelState.AddModelError("BuildingClass", "کلاس مورد نظر در سیستم یافت نشد");
                return BadRequest(ModelState);
            }

            var present = await _presentationService.GetAsync(x => x.Id == presentationUpdateVM.Id);
            if (present == null)
                return NotFound();

            present = _mapper.Map(presentationUpdateVM, present);

            _presentationService.Update(present);
            await _presentationService.SaveChangesAsync();

            return Ok();
        }
    }
}