// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AestheticEMR.Core.Services;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Account
{
    public class UserAccountService : IUserAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserAccountService> _logger;

        public UserAccountService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
            IEmailSender emailSender, ILogger<UserAccountService> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<(ApplicationUser User, string[] Roles)?> GetUserAndRolesAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name!)
                .ToArrayAsync();

            return (user, roles);
        }

        public async Task<List<(ApplicationUser User, string[] Roles)>> GetUsersAndRolesAsync(int page, int pageSize)
        {
            IQueryable<ApplicationUser> usersQuery = _context.Users
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName);

            if (page != -1)
                usersQuery = usersQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                usersQuery = usersQuery.Take(pageSize);

            var users = await usersQuery.ToListAsync();

            var userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToArrayAsync();

            return users
                .Select(u => (u, roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name!)
                    .ToArray()))
                .ToList();
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateUserAsync(ApplicationUser user,
            IEnumerable<string> roles, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            user = (await _userManager.FindByNameAsync(user.UserName!))!;

            try
            {
                result = await _userManager.AddToRolesAsync(user, roles.Distinct());
            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, []);
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(ApplicationUser user)
        {
            return await UpdateUserAsync(user, null);
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(ApplicationUser user,
            IEnumerable<string>? roles)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            if (roles != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = userRoles.Except(roles).ToArray();
                var rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Length != 0)
                {
                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!result.Succeeded)
                        return (false, result.Errors.Select(e => e.Description).ToArray());
                }

                if (rolesToAdd.Length != 0)
                {
                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!result.Succeeded)
                        return (false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return (true, []);
        }

        public async Task<(bool Succeeded, string[] Errors)> ResetPasswordAsync(ApplicationUser user,
            string newPassword)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdatePasswordAsync(ApplicationUser user,
            string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            return (true, []);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (!_userManager.SupportsUserLockout)
                    await _userManager.AccessFailedAsync(user);

                return false;
            }

            return true;
        }

        public async Task<(bool Success, string[] Errors)> TestCanDeleteUserAsync(string userId)
        {
            var errors = new List<string>();

            if (await _context.Orders.Where(o => o.CashierId == userId).AnyAsync())
                errors.Add("User has associated orders");

            //canDelete = !await ; //Do other tests...

            return (errors.Count == 0, errors.ToArray());
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return (true, []);
        }

        public async Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<(bool Succeeded, string[] Errors)> ResetPasswordWithTokenAsync(ApplicationUser user,
            string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<(bool Succeeded, string[] Errors)> SendPasswordResetEmailAsync(ApplicationUser user, string resetUrlTemplate)
        {
            try
            {
                _logger.LogInformation("Starting password reset email process for user: {UserName}", user.UserName);

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(resetToken);
                var encodedUserName = Uri.EscapeDataString(user.UserName ?? user.Email ?? string.Empty);

                var resetUrl = resetUrlTemplate
                    .Replace("{token}", encodedToken, StringComparison.OrdinalIgnoreCase)
                    .Replace("{username}", encodedUserName, StringComparison.OrdinalIgnoreCase)
                    .Replace("{userNameOrEmail}", encodedUserName, StringComparison.OrdinalIgnoreCase);

                if (!resetUrl.Contains("{token}", StringComparison.OrdinalIgnoreCase) &&
                    !resetUrl.Contains("token=", StringComparison.OrdinalIgnoreCase))
                {
                    resetUrl += resetUrl.Contains('?') ? "&" : "?";
                    resetUrl += $"token={encodedToken}";
                }

                if (!resetUrl.Contains("{username}", StringComparison.OrdinalIgnoreCase) &&
                    !resetUrl.Contains("{userNameOrEmail}", StringComparison.OrdinalIgnoreCase) &&
                    !resetUrl.Contains("userNameOrEmail=", StringComparison.OrdinalIgnoreCase))
                {
                    resetUrl += resetUrl.Contains('?') ? "&" : "?";
                    resetUrl += $"userNameOrEmail={encodedUserName}";
                }

                var recipientName = string.IsNullOrWhiteSpace(user.FullName) ? user.UserName ?? "User" : user.FullName;
                var body = BuildPasswordResetEmailBody(recipientName, resetUrl);

                _logger.LogInformation("Sending password reset email to: {Email}", user.Email);
                var result = await _emailSender.SendEmailAsync(recipientName, user.Email!, "Password Reset Request", body, true);

                if (!result.success)
                {
                    _logger.LogError("Failed to send password reset email to {Email}: {ErrorMsg}", user.Email, result.errorMsg);
                    return (false, [result.errorMsg ?? "Unable to send password reset email"]);
                }

                _logger.LogInformation("Successfully sent password reset email to: {Email}", user.Email);
                return (true, []);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending password reset email for user: {UserName}", user.UserName);
                return (false, [$"An error occurred while sending password reset email: {ex.Message}"]);
            }
        }

        private string BuildPasswordResetEmailBody(string recipientName, string resetUrl)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Password Reset Request</h2>
        
        <p>Hello {recipientName},</p>
        
        <p>We received a request to reset your password for your account. If you made this request, please click the button below to proceed:</p>
        
        <div style='text-align: center; margin: 30px 0;'>
            <a href='{resetUrl}' style='background-color: #3498db; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold;'>Reset Your Password</a>
        </div>
        
        <p>Or copy and paste this link in your browser:</p>
        <p style='word-break: break-all; background-color: #f5f5f5; padding: 10px; border-radius: 3px;'>{resetUrl}</p>
        
        <p style='color: #e74c3c;'><strong>Important Security Note:</strong></p>
        <ul>
            <li>This link will expire in 24 hours</li>
            <li>If you did not request this password reset, please ignore this email and your password will remain unchanged</li>
            <li>Never share your password reset link with anyone</li>
        </ul>
        
        <p>Best regards,<br/>
        <strong>VCP Aesthetic Clinic Team</strong></p>
        
        <hr style='border: none; border-top: 1px solid #ddd; margin: 30px 0;'>
        
        <p style='font-size: 12px; color: #888;'>
            This is an automated email. Please do not reply directly to this message.<br/>
            If you have questions, please contact our support team.
        </p>
    </div>
</body>
</html>";
        }

        public async Task<(bool Succeeded, string[] Errors)> SendTestEmailAsync(string recipientEmail, string htmlBody)
        {
            var result = await _emailSender.SendEmailAsync("Test User", recipientEmail, "Test Email", htmlBody, true);

            if (!result.success)
                return (false, [result.errorMsg ?? "Unable to send test email"]);

            return (true, []);
        }
    }
}
