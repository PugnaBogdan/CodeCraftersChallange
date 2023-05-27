using OTPMVC.Models;
using OTPMVC.Services.Implementations;
using OTPMVC.Services.Interfaces;

namespace OTPMVC.Tests
{
    public class Tests
    {
        private OTPService oTPService { get; set; }

        [SetUp]
        public void Setup()
        {
            oTPService = new OTPService();

        }

        [TestCase("Jan 1, 2009")]
        [TestCase("7/13/2021 11:00:00 AM ")]
        [TestCase("07/13/2001 11:00:00-07:00 ")]
        public void VerifyOTP_FalseTest(string dateTime)
        {
            var otp = new OTP() { Password = "password" , CreatedAt = DateTime.Parse(dateTime) };

            var result = oTPService.VerifyOTP(otp);
            Assert.False(result);
        }
    }
}