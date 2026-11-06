using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using WebGym.Models.Entities;

namespace WebGym.Helpers
{
    /// <summary>
    /// Băm và kiểm tra mật khẩu bằng PasswordHasher của ASP.NET Core
    /// (PBKDF2-HMAC-SHA512, salt ngẫu nhiên cho từng mật khẩu, 100.000 vòng lặp).
    ///
    /// Vẫn chấp nhận hash cũ dạng SHA-256 không salt (dữ liệu seed và tài khoản tạo trước đây):
    /// khi kiểm tra đúng sẽ trả về SuccessRehashNeeded để nơi gọi lưu lại hash mới.
    /// </summary>
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<User> Hasher = new();

        // Hash giả dùng khi không tìm thấy tài khoản, để thời gian phản hồi giống như khi có tài khoản
        private static readonly string DummyHash = Hasher.HashPassword(null!, "dummy-password-for-timing");

        public static string Hash(string password)
        {
            return Hasher.HashPassword(null!, password);
        }

        public static PasswordVerificationResult Verify(string? storedHash, string password)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(password))
            {
                return PasswordVerificationResult.Failed;
            }

            if (IsLegacySha256(storedHash, out var legacyBytes))
            {
                var input = SHA256.HashData(Encoding.UTF8.GetBytes(password));
                return CryptographicOperations.FixedTimeEquals(input, legacyBytes)
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : PasswordVerificationResult.Failed;
            }

            try
            {
                return Hasher.VerifyHashedPassword(null!, storedHash, password);
            }
            catch (FormatException)
            {
                return PasswordVerificationResult.Failed;
            }
        }

        /// <summary>Chạy một lần kiểm tra giả để không lộ việc email có tồn tại hay không qua thời gian phản hồi.</summary>
        public static void VerifyDummy(string password)
        {
            Hasher.VerifyHashedPassword(null!, DummyHash, password ?? string.Empty);
        }

        // Hash cũ: Base64 của đúng 32 byte SHA-256 (44 ký tự). Hash mới của PasswordHasher dài hơn nhiều.
        private static bool IsLegacySha256(string hash, out byte[] bytes)
        {
            bytes = Array.Empty<byte>();
            if (hash.Length != 44) return false;

            var buffer = new byte[33];
            if (!Convert.TryFromBase64String(hash, buffer, out var written) || written != 32) return false;

            bytes = buffer[..32];
            return true;
        }
    }
}
