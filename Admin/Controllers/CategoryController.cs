﻿using Domain.DTO.AdminDtos;
using Domain.Entities;
using Domain.Helpers;
using Domain.Services.AdminServices.CategoryService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _service;

        public CategoryController(UserManager<User> userManager, ICategoryService service)
        {
            _userManager = userManager;
            _service = service;
        }
        public IActionResult Index(SearchCriteria model)
        {
            var data = _service.GetList(model);
            var res = new TableDto<CategoryDto>();
            res.DataList = data.Items.ToList();
            res.TotalCount = (int)Math.Ceiling(Convert.ToDouble(data.TotalCount) / model.PageSize);
            res.PageNumber = model.PageNumber;
            res.Search = model.Search;
            return View(res);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto data)
        {
            var user = await _userManager.GetUserAsync(User);
            var res = await _service.Create(data, user.Id);
            if (res)
                return Json(new { status = true });
            return Json(new { status = false, message = "حدث خطأ ما" });
        }
        public IActionResult Edit(int Id)
        {
            var res = _service.GetById(Id);
            return View(res);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(CategoryDto data)
        {
            var user = await _userManager.GetUserAsync(User);
            var res = await _service.Edit(data, user.Id);
            if (res)
                return Json(new { status = true });
            return Json(new { status = false, message = "حدث خطأ ما" });
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
        public JsonResult GetDDl()
        {
            return Json(_service.GetDDL());
        }
    }
}
