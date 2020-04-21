﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.DTO;
using PersonalWebsite.IService;
using PersonalWebsite.AdminWeb.Models;

namespace PersonalWebsite.AdminWeb.Controllers
{
    public class AdminUserController : Controller
    {
        protected IAdminUserService AdminUserService { get; set; }
        protected IRoleService RoleService { get; set; }

        public AdminUserController(IAdminUserService AdminUserService, IRoleService RoleService)
        {
            this.AdminUserService = AdminUserService;
            this.RoleService = RoleService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
        [HttpPost]
        public IActionResult List(string str)
        {
            var adminUsers = AdminUserService.GetAll();
            Result result = new Result();
            result.Code = 0;
            result.Data = adminUsers;
            result.Count = adminUsers.Length;
            result.Msg = "";
            return Json(result);
        }
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Add(AdminUserAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new Result { Code = 1, Msg = "数据验证未通过" });
            }
            //服务器端的校验必不可少
            bool exists = AdminUserService.GetByPhoneNum(model.PhoneNum) != null;
            if (exists)
            {
                return Json(new Result { Code = 1, Msg = "手机号已经存在" });
            }

            long userId = AdminUserService.AddAdminUser(model.Name, model.PhoneNum, model.Password, model.Email, null);
            RoleService.AddRoleIds(userId, model.RoleIds);
            return Json(new Result { Code = 0, Msg = "保存成功" });
        }


    }
}