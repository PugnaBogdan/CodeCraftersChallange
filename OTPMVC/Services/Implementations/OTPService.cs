using OTPMVC.Models;
using OTPMVC.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;

namespace OTPMVC.Services.Implementations
{
    public class OTPService : IOTPService
    {
        private const int PasswordValidityInSeconds = 30;
        private static OTP generatedOTP = new OTP() { Password = string.Empty, CreatedAt = new DateTime() };
        public OTPService()
        {

        }


        public OTP GetOTP()
        {
            return generatedOTP;
        }

        public string GenerateOTP(string? inputString, DateTime dateTime)
        {
            string input = inputString + dateTime.ToString("yyyyMMddHHmmss");
            byte[] hashBytes = CalculateHash(input);
            int otpValue = ExtractOTPValue(hashBytes);
            string otp = otpValue.ToString("D6");

            generatedOTP = new OTP()
            {
                Password = otp,
                CreatedAt = DateTime.Now
            };

            return otp;
        }

        private byte[] CalculateHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return sha256.ComputeHash(inputBytes);
            }
        }

        private int ExtractOTPValue(byte[] hashBytes)
        {
            int offset = hashBytes[hashBytes.Length - 1] & 0xf;
            int binaryCode = (hashBytes[offset] & 0x7f) << 24 |
                             (hashBytes[offset + 1] & 0xff) << 16 |
                             (hashBytes[offset + 2] & 0xff) << 8 |
                             hashBytes[offset + 3] & 0xff;

            return binaryCode % 1000000; 
        }

        public bool VerifyOTP(OTP generatedOTP)
        {
            var generatedPasswordTime = generatedOTP.CreatedAt.HasValue ? generatedOTP.CreatedAt.Value.AddSeconds(PasswordValidityInSeconds) : new DateTime();

            return generatedPasswordTime > DateTime.Now; 
        }
    }
}
