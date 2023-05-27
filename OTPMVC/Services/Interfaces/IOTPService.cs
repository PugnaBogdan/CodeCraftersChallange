using OTPMVC.Models;

namespace OTPMVC.Services.Interfaces
{
    public interface IOTPService
    {
        OTP GetOTP();

        string GenerateOTP(string? inputString, DateTime dateTime);

        bool VerifyOTP(OTP generatedOTP);
    }
}
