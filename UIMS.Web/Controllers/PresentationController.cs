using System;
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
using Microsoft.AspNetCore.Authorization;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    //[Route("api/Presentation")]
    public class PresentationController : ApiController
    {
        private readonly IMapper _mapper;

        private readonly PresentationService _presentationService;
        private readonly NotificationService _notificationService;
        private readonly SemesterService _semesterService;
        private readonly CourseFieldService _courseFieldService;
        private readonly ProfessorService _professorService;
        private readonly BuildingClassService _buildingClassService;

        public PresentationController(IMapper mapper, PresentationService presentationService, SemesterService semesterService, CourseFieldService courseFieldService, ProfessorService professorService, BuildingClassService buildingClassService, NotificationService notificationService)
        {
            _mapper = mapper;
            _presentationService = presentationService;
            _semesterService = semesterService;
            _notificationService = notificationService;
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
                ModelState.AddModelError("Errors", "این کلاس طبق کد وارد شده قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            var professor = await _professorService.GetAsync(x => x.Id == presentationInsertVM.ProfessorId.Value);
            if (professor== null)
            {
                ModelState.AddModelError("Errors", "استاد مورد نظر یافت نشد");
                return BadRequest(ModelState);
            }
            if (!professor.User.Enable)
            {
                ModelState.AddModelError("Errors", "امکان اختصاص کلاس به استاد مورد نظر وجود ندارد");
                return BadRequest(ModelState);
            }

            if (!await _courseFieldService.IsExistsAsync(x=>x.Id == presentationInsertVM.CourseFieldId.Value))
            {
                ModelState.AddModelError("Errors", "درس مورد نظر یافت نشد");
                return BadRequest(ModelState);
            }
            
            if (!await _buildingClassService.IsExistsAsync(x => x.Id == presentationInsertVM.BuildingClassId.Value))
            {
                ModelState.AddModelError("Errors", "کلاس مورد نظر در سیستم یافت نشد");
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

        [HttpGet("{semester}")]
        [SwaggerResponse(200, typeof(PresentationDashboardDataViewModel))]
        public async Task<IActionResult> DashboardInfo(string semester)
        {
            return Ok(await _presentationService.GetDashboardInfo(semester));
        }

        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<PresentationViewModel>))]
        public async Task<IActionResult> GetAll([FromBody] FilterInputViewModel filterInputVM)
        {
            return Ok(await _presentationService.GetAllAsync(filterInputVM));
        }

        [Authorize]
        [HttpGet]
        [SwaggerResponse(200, typeof(PaginationViewModel<PresentationViewModel>))]
        public async Task<IActionResult> GetAllByRole(int pageSize = 5, int page = 1)
        {
            var professor = await _professorService.GetAsync(x => x.UserId == UserId);

            if (professor == null)
                return NotFound();

            return Ok(await _presentationService.GetAllByRoleAsync(page,pageSize,professor.Id));
        }



        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<StudentViewModel>))]
        public async Task<IActionResult> GetStudents([FromBody] PresentationGetStudentsViewModel presentationGetStudentsVM)
        {
            return Ok(await _presentationService.GetStudents(presentationGetStudentsVM.Id, presentationGetStudentsVM.Page, presentationGetStudentsVM.PageSize));
        }


        [HttpPost]
        [SwaggerResponse(200, typeof(PaginationViewModel<PresentationViewModel>))]
        public async Task<IActionResult> Search([FromBody]SearchViewModel searchVM)
        {
            var results = await _presentationService.SearchAsync(searchVM.Text, searchVM.Page, searchVM.PageSize);
            return Ok(results);
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

            
            if (presentationUpdateVM.Code != null && await _presentationService.IsExistsAsync(x => x.Code == presentationUpdateVM.Code && x.Id != presentationUpdateVM.Id))
            {
                ModelState.AddModelError("Errors", "این کلاس طبق کد وارد شده قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            if (presentationUpdateVM.BuildingClassId.HasValue && !await _buildingClassService.IsExistsAsync(x => x.Id == presentationUpdateVM.BuildingClassId.Value))
            {
                ModelState.AddModelError("Errors", "کلاس مورد نظر در سیستم یافت نشد");
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