using System.Security.Cryptography;
using System.Text;

namespace SIMAPI.Business.Helper
{
    public static class TokenHelpers
    {
        public static string GenerateRandomToken(int size = 32)
        {
            var bytes = new byte[size];
            RandomNumberGenerator.Fill(bytes);
            // Use Base64Url style if you prefer:
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public static string ComputeSha256Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash); // store as base64 string
        }
    }
}
