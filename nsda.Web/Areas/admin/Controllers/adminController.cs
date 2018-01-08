using nsda.Model.dto.response;
using nsda.Services.admin;
using nsda.Utilities;
using nsda.Web.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsda.Web.Areas.admin.Controllers
{
    public class adminController : baseController
    {
        ISysUserService _sysUserService;
        public adminController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        public ActionResult index()
        {
            ViewBag.UserName = UserContext.SysUserContext.Name;
            return View();
        }

        public ActionResult home()
        {
            return View();
        }

        [HttpGet]
        public ContentResult authdata()
        {
            List<MenuData> menudata = new List<MenuData> {
                 new MenuData {MenuCode="1",MenuName="基础数据",MenuId=1,ParentMenuId=0, MenuIcon="fa fa-cog",MenuIndex=1, MenuUrl=""},
                 new MenuData {MenuCode="2",MenuName="地区设置",MenuId=2,ParentMenuId=1, MenuIcon="fa fa-cog",MenuIndex=2, MenuUrl="/sysmanage/company/index"},
                 new MenuData {MenuCode="3",MenuName="城市设置",MenuId=3,ParentMenuId=1, MenuIcon="fa fa-cog",MenuIndex=3, MenuUrl="/sysmanage/dept/index"},
                 new MenuData {MenuCode="4",MenuName="学校设置",MenuId=4,ParentMenuId=1, MenuIcon="fa fa-cog",MenuIndex=4, MenuUrl="/sysmanage/dept/index"},
                 new MenuData {MenuCode="5",MenuName="系统设置",MenuId=5,ParentMenuId=0, MenuIcon="fa fa-cog",MenuIndex=2, MenuUrl=""},
                 new MenuData {MenuCode="6",MenuName="用户管理",MenuId=6,ParentMenuId=5, MenuIcon="fa fa-cog",MenuIndex=1, MenuUrl="/sysmanage/company/index"},
                 new MenuData {MenuCode="7",MenuName="邮件记录",MenuId=7,ParentMenuId=5, MenuIcon="fa fa-cog",MenuIndex=2, MenuUrl="/sysmanage/dept/index"},
                 new MenuData {MenuCode="8",MenuName="日志管理",MenuId=8,ParentMenuId=0, MenuIcon="fa fa-cog",MenuIndex=3, MenuUrl=""},
                 new MenuData {MenuCode="9",MenuName="系统操作日志",MenuId=6,ParentMenuId=5, MenuIcon="fa fa-cog",MenuIndex=1, MenuUrl="/sysmanage/company/index"},
                 new MenuData {MenuCode="10",MenuName="会员操作日志",MenuId=7,ParentMenuId=5, MenuIcon="fa fa-cog",MenuIndex=2, MenuUrl="/sysmanage/dept/index"}
            };
            List<ButtonData> buttondata = new List<ButtonData>();
            var jsonData = new
            {
                buttondata = buttondata,   //功能按钮
                menudata = menudata,     //页面
            };
            return Result(true, "", jsonData);
        }


        public ActionResult info()
        {
            var user = _sysUserService.Detail(UserContext.SysUserContext.Id);
            if (user == null)
                Response.Redirect("/error/error", true);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ContentResult editpwd(string oldPwd,string newPwd)
        {
            string msg = string.Empty;
            var flag = _sysUserService.UpdatePwd(UserContext.SysUserContext.Id,oldPwd,newPwd,out msg);
            return Result<string>(flag, msg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ContentResult edit(string name, string mobile)
        {
            int id = UserContext.SysUserContext.Id;
            string msg = string.Empty;
            var flag = _sysUserService.Edit(new Model.dto.request.SysUserRequest {
                 Id= id,
                 Mobile=mobile,
                 Name=name
            },id, out msg);
            return Result<string>(flag, msg);
        }
    }
}