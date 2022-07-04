using Microsoft.AspNetCore.Mvc;
using prjAjax.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace prjAjax.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        public ApiController(DemoContext conetxt)
        {
            _context = conetxt;
        }
        public IActionResult Hw2()
        {
            return View();
        }
        public IActionResult Check(string name)
        {
            Debug.WriteLine("asd");
            if (string.IsNullOrEmpty(name))
            {
                return Content("請輸入名字");
            }
            else
            {
                IEnumerable<Member> data = null;
                data = from a in _context.Members
                       where name == a.Name
                       select a;

                if (data.Count() > 0)
                {
                    return Content("名字已有人取用","text/plain",System.Text.Encoding.UTF8);
                }
                else 
                {
                    return Content("此為可用名字", "text/plain", System.Text.Encoding.UTF8);
                }
            }
        }

        //public IActionResult Index(string name,int age=18)
        public IActionResult Index(User user)
        {
            //System.Threading.Thread.Sleep(2000); //停止5秒鐘
            //return Content("<h1>Ajax你真的太無情了</h1>", "text/html", System.Text.Encoding.UTF8);
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "WhyAjaxInPrjTime";
            }
            return Content($"{user.name}你好，我今年{user.age}歲", "text/html", System.Text.Encoding.UTF8);
        }
    }
}
