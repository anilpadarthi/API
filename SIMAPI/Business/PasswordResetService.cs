using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Helper;
using SIMAPI.Data;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.ResetPassword;
using System.Security.Cryptography;
using System.Text;

namespace SIMAPI.Business
{
    public class PasswordResetService
    {
        private readonly SIMDBContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<PasswordResetService> _logger;

        private const int TokenExpiryMinutes = 30;

        private const string ResetUrl =
            "https://app.leap-tel.com/passwordrest/verify";

        public PasswordResetService(
            SIMDBContext context,
            IPasswordHasher<User> passwordHasher,
            ILogger<PasswordResetService> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        // Generate cryptographically secure token
        private static string GenerateResetToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        // Store only SHA256 token hash in database
        private static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();

            var hash = sha256.ComputeHash(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(hash);
        }

        // Request forgot password
        public async Task<string> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            // Adapt EmailId to your actual email column
            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == normalizedEmail && x.IsSystemAccess == true && x.Status == 1 );

            // Don't reveal whether the email exists
            if (user == null)
                return "Invalid email or email address not found.";

            var token = GenerateResetToken();

            var tokenHash = HashToken(token);

            var now = DateTime.UtcNow;

            // Invalidate previous unused reset tokens
            var previousTokens = await _context.Set<PasswordResetToken>()
                .Where(x => x.UserId == user.UserId
                         && !x.IsUsed)
                .ToListAsync();

            foreach (var oldToken in previousTokens)
            {
                oldToken.IsUsed = true;
                oldToken.UsedDate = now;
            }

            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                TokenHash = tokenHash,
                CreatedDate = now,
                ExpiryDate = now.AddMinutes(TokenExpiryMinutes),
                IsUsed = false
            };

            _context.Add(resetToken);

            await _context.SaveChangesAsync();

            // Build secure reset URL
            var resetLink = ResetUrl +
                "?token=" + Uri.EscapeDataString(token);

            try
            {
                CommunicationHelper.ForgotPasswordEmail(
                    user.Email,
                    resetLink,
                    TokenExpiryMinutes);
                return "Password reset email has been sent.";
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex,
                    "Failed to send password reset email.");
              
                return "Failed to send password reset email.";
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string token)
        {
            try
            {
                // Step 1: Validate input
                if (string.IsNullOrWhiteSpace(token))
                {
                    return false;
                }

                // Step 2: Generate SHA256 hash of received token
                var tokenHash = HashToken(token);

                // Step 3: Get current UTC date
                var currentDate = DateTime.UtcNow;

                // Step 4: Check token in database
                var resetToken = await _context.Set<PasswordResetToken>()
                    .AsNoTracking()
                    .Where(x =>
                        x.TokenHash == tokenHash &&
                        x.IsUsed == false &&
                        x.ExpiryDate > currentDate)
                    .Select(x => new
                    {
                        x.PasswordResetTokenId,
                        x.UserId
                    })
                    .FirstOrDefaultAsync();

                // Step 5: Token not found / expired / already used
                if (resetToken == null)
                {
                    return false;
                }

                // Step 6: Verify associated user exists
                var userExists = await _context.Set<User>()
                    .AsNoTracking()
                    .AnyAsync(x => x.UserId == resetToken.UserId);

                if (!userExists)
                {
                    return false;
                }

                // Step 7: Token is valid
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while verifying password reset token.");

                throw;
            }
        }

        // Reset password
        public async Task<bool> ResetPasswordAsync(
     ResetPasswordRequestDto request)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var tokenHash = HashToken(request.Token);

                var now = DateTime.UtcNow;

                // Lock token row until transaction completes
                var resetToken = await _context.Set<PasswordResetToken>()
                    .FromSqlInterpolated($@"
                SELECT *
                FROM dbo.PasswordResetToken
                    WITH (UPDLOCK, ROWLOCK)
                WHERE TokenHash = {tokenHash}
                  AND IsUsed = 0
                  AND ExpiryDate > {now}")
                    .SingleOrDefaultAsync();

                if (resetToken == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(x =>
                        x.UserId == resetToken.UserId);

                if (user == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Update password
                //user.Password = _passwordHasher.HashPassword(
                //    user,
                //    request.NewPassword);
                user.Password = request.NewPassword;

                // Consume reset token
                resetToken.IsUsed = true;
                resetToken.UsedDate = now;

                // Invalidate other outstanding tokens
                var otherTokens = await _context.Set<PasswordResetToken>()
                    .Where(x =>
                        x.UserId == user.UserId &&
                        x.PasswordResetTokenId !=
                            resetToken.PasswordResetTokenId &&
                        !x.IsUsed)
                    .ToListAsync();

                foreach (var item in otherTokens)
                {
                    item.IsUsed = true;
                    item.UsedDate = now;
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
