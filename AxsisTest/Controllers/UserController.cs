using AxsisTest.Database;
using AxsisTest.Enums;
using AxsisTest.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AxsisTest.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private AxsisTestEntities db;
        public UserController()
        {
            db = new AxsisTestEntities();
        }

        public ActionResult Index()
        {
            ViewBag.Users = db.AspNetUsers;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            AspNetUser user = db.AspNetUsers.FirstOrDefault(q => q.Id == id);
            EditViewModel model = new EditViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Gender = (GenderEnum)user.Gender
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool alreadyEmail = db.AspNetUsers.Any(q => q.Id != model.UserId && q.Email == model.Email);
                if (alreadyEmail) ModelState.AddModelError("", "Email en uso");

                bool alreadyUsername = db.AspNetUsers.Any(q => q.Id != model.UserId && q.UserName == model.UserName);
                if (alreadyUsername) ModelState.AddModelError("", "Usuario en uso");

                if (alreadyEmail || alreadyUsername) return View(model);

                AspNetUser user = db.AspNetUsers.FirstOrDefault(q => q.Id == model.UserId);
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Gender = (int)model.Gender;

                if (model.Password != null)
                {
                    ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    string token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                    await userManager.ResetPasswordAsync(user.Id, token, model.Password);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult DeleteUser(string userId)
        {
            bool success = false;
            string message;
            try
            {
                AspNetUser deletedUser = db.AspNetUsers.FirstOrDefault(q => q.Id == userId);
                deletedUser.Active = false;
                db.SaveChanges();
                success = true;
                message = "Success";
                return Json(new { success, message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message;
                return Json(new { success, message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}