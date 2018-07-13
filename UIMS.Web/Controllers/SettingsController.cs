using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.DTO;
using UIMS.Web.Models;
using UIMS.Web.Services;

namespace UIMS.Web.Controllers
{
    [Produces("application/json")]
    public class SettingsController:ApiController
    {
        private readonly SettingsService _settingsService;
        private readonly IMapper _mapper;

        public SettingsController(SettingsService settingsService, IMapper mapper)
        {
            _mapper = mapper;
            _settingsService = settingsService;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SettingsInsertViewModel settingsInsertVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _settingsService.IsExistsAsync(x=>x.AccessName == settingsInsertVM.AccessName && x.Name == settingsInsertVM.Name))
            {
                ModelState.AddModelError("Errors", "این تنظیمات قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            await _settingsService.AddAsync(settingsInsertVM);
            await _settingsService.SaveChangesAsync();

            return Ok();

        }

        [SwaggerResponse(200, typeof(SettingsViewModel))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var settings = await _settingsService.GetAsync(id);
            if (settings == null)
                return NotFound();

            return Ok(settings);

        }

        [SwaggerResponse(200, typeof(SettingsViewModel))]
        [HttpGet("{accessName}")]
        public async Task<IActionResult> GetValue(string accessName)
        {
            var settings = await _settingsService.GetAsync(x=>x.AccessName == accessName);
            if (settings == null)
                return NotFound();

            return Ok(settings.Value);

        }


        [SwaggerResponse(200, typeof(PaginationViewModel<SettingsViewModel>))]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] FilterInputViewModel filterInputVM)
        {
            return Ok(await _settingsService.GetAllAsync(filterInputVM.Filters, filterInputVM.Page, filterInputVM.PageSize));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var setting = await _settingsService.GetAsync(x => x.Id == id);
            if (setting == null)
                return NotFound();

            _settingsService.Remove(setting);
            await _settingsService.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SettingsUpdateViewModel settingsUpdateVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var settings = await _settingsService.GetAsync(x => x.Id == settingsUpdateVM.Id);
            if (settings == null)
                return NotFound();
            if (await _settingsService.IsExistsAsync(x => x.AccessName == settingsUpdateVM.AccessName && x.Name == settingsUpdateVM.Name && x.Id != settingsUpdateVM.Id))
            {
                ModelState.AddModelError("Errors", "این تنظیمات قبلا در سیستم ثبت شده است");
                return BadRequest(ModelState);
            }

            settings = _mapper.Map(settingsUpdateVM, settings);
            _settingsService.Update(settings);
            await _settingsService.SaveChangesAsync();

            return Ok();
        }
    }
}
