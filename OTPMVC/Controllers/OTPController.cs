using Microsoft.AspNetCore.Mvc;
using OTPMVC.Models;
using OTPMVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace OTPMVC.Controllers
{
    public class OTPController : Controller
    {
        private static UserInput userInputLocal = new UserInput() { UserId = string.Empty, DateTimeInput = new DateTime()};
        
        private readonly IOTPService _oTPService;

        public OTPController(IOTPService oTPService)
        {
            this._oTPService = oTPService;
        }

        public IActionResult Index()
        { 
            return View();
        }



        public IActionResult ValidateOTP()
        {
            var otp = _oTPService.GetOTP();
            return View(otp);
        }

        public IActionResult GenerateOTP(UserInput request)
        {
            if (ModelState.IsValid)
            {
                var password = this._oTPService.GenerateOTP(request.UserId, request.DateTimeInput);
                return RedirectToAction("ValidateOTP");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateUserInput(UserInput userInput)
        {
            if (ModelState.IsValid)
            {
                userInputLocal.UserId = userInput.UserId;
                userInputLocal.DateTimeInput = userInput.DateTimeInput;
            }

            return RedirectToAction("Index");
        }


        public IActionResult CheckPassword(string request)
        {
            var otp = _oTPService.GetOTP();
            if (request == otp.Password)
            {
                if (this._oTPService.VerifyOTP(otp))
                {
                    TempData["Success"] = "1";
                    return RedirectToAction("Success", "Home");
                }
                else
                {
                    TempData["Message"] = "OTP expired, generate a new password";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "Incorrect password. Please try again.";
            }
            
            return RedirectToAction("ValidateOTP");
        }
    }
}
