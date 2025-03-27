﻿using Domain.DTO.AdminDtos;
using Domain.Entities;
using Domain.Helpers;
using Domain.Services.AdminServices.ItemService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class TripsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITripsService _service;

        public TripsController(UserManager<User> userManager, ITripsService service)
        {
            _userManager = userManager;
            _service = service;
        }
        public IActionResult Index(SearchCriteria model)
        {
            var data = _service.GetList(model);
            var res = new TableDto<TripsDto>();
            res.DataList = data.Items.ToList();
            res.TotalCount = (int)Math.Ceiling(Convert.ToDouble(data.TotalCount) / model.PageSize);
            res.PageNumber = model.PageNumber;
            res.Search = model.Search;
            return View(res);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var res = _service.Delete(id, user.Id);
            if (res)
                return Json(new { status = true });
            return Json(new { status = false, message = "حدث خطأ ما" });
        }
    }
}
