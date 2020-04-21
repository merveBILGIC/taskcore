using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using taskcore.Dao;
using taskcore.Manager;
using taskcore.Models;

namespace taskcore.Controllers
{
    public class AccountController : Controller
    {

        private readonly DatabaseContext _context;
        private UserDao userDao = null;
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Project/Index");
            }
            return View();
        }
        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Project/Index");
            }
            return View();
        }

        public IActionResult Profile()
        {
            if (!HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("Index");
            }
            return View();
        }

        public IActionResult ForgotPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("Index");
            }
            return View();
        }

         public IActionResult ResetPassword()
        {

            return View();
        }

        public IActionResult None()
        {
            return View();
        }

        public async Task<IActionResult> Statistics()
        {
            int id = (int)HttpContext.Session.GetInt32("id");
            List<ProjectsProgress> res = await ProjectDao.getInstance().ProgressList(id);
            return View(res);
        }

        public IActionResult Settings()
        {
            if (!HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("NotFound");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            User user = await GetUserDao().Find(model);
            if (user != null)
            {
                HttpContext.Session.SetInt32("id", user.Id);
                HttpContext.Session.SetString("name", user.Name);
                HttpContext.Session.SetString("surname", user.Surname);
                HttpContext.Session.SetString("username", user.Username);
                UserManager.SetCurrentUser(user);
                return Redirect("/Project/Index");
            }

            ViewBag.LoginError = true;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("Index");
        }
        public async Task<IActionResult> Register(User user)
        {
                await GetUserDao().Create(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdatePassword(string Code,string Password)
        {
            if(MailManager.getCode() == Code){
                User tmp = UserManager.GetCurrentUser();
                tmp.Password = Password;
                _context.Update(tmp);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> SendACode(string email){
            User tmp = await _context.User.FirstOrDefaultAsync(w => w.Email == email);

            if(tmp != null){
                UserManager.SetCurrentUser(tmp);
                await MailManager.ResetPasswordCode();
            }else{
                return Json(false);
            }
            return Redirect("ResetPassword");
        }

        public UserDao GetUserDao(){
            return userDao == null ? userDao = UserDao.getInstance() : userDao;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
