using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjAjax.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prjAjax.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;
        private readonly IWebHostEnvironment _host;
        public ApiController(DemoContext conetxt, IWebHostEnvironment hostEnvironment)
        {
            _context = conetxt;
            _host = hostEnvironment;
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
                    return Content("名字已有人取用", "text/plain", System.Text.Encoding.UTF8);
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

        public IActionResult Register(Member member, IFormFile file)
        {   //查看檔案資料
            //string info = $"{file.FileName} - {file.ContentType} - {file.Length}";
            //string path = _host.ContentRootPath; //會取得專案資料夾的實際路徑
            string path = Path.Combine(_host.WebRootPath, "uploads", file.FileName);

            //會取得專案資料夾下wwwroot的實際路徑
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            //寫進資料庫
            byte[] imgByte = null;
            using (var mememoryStream = new MemoryStream())
            {
                file.CopyTo(mememoryStream);
                imgByte = mememoryStream.ToArray();
            }

            member.FileName = file.FileName;
            member.FileData = imgByte;

            _context.Members.Add(member);
            _context.SaveChanges();

            string info = $"{file.FileName} - {file.ContentType} - {file.Length}";
            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult City()
        {
            var cities = (from p in _context.Addresses
                            select p.City).Distinct();

            return Json(cities);
        }

        public IActionResult Districts(string city)
        {
            var districts = (from p in _context.Addresses
                             where p.City == city
                             select p.SiteId).Distinct();

            return Json(districts);
        }

        public IActionResult Roads(string districts)
        {
            var roads = from p in _context.Addresses
                        where p.SiteId == districts
                        select p.Road;

            return Json(roads);
        }

        public IActionResult GetImageBytes(int id = 1)
        {
            Member member = _context.Members.Find(id);
            byte[] img = member.FileData;
            return File(img, "image/jpeg");
        }
    }
}
