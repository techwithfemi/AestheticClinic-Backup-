// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models.Account;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Account;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/account")]
    [Authorize]
    public class UserAccountController : BaseApiController
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IAuthorizationService _authorizationService;

        public UserAccountController(ILogger<UserAccountController> logger, IMapper mapper,
            IUserAccountService userAccountService, IAuthorizationService authorizationService) : base(logger, mapper)
        {
            _userAccountService = userAccountService;
            _authorizationService = authorizationService;
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userAccountService.GetUserByUserNameAsync(model.UserNameOrEmail)
                ?? await _userAccountService.GetUserByEmailAsync(model.UserNameOrEmail);

            if (user == null || !user.IsEnabled || string.IsNullOrWhiteSpace(user.Email))
                return NoContent();

            var resetUrl = string.IsNullOrWhiteSpace(model.ResetUrl)
                ? $"{Request.Scheme}://{Request.Host}/login?reset=true&userNameOrEmail={{userNameOrEmail}}&token={{token}}"
                : model.ResetUrl;

            var result = await _userAccountService.SendPasswordResetEmailAsync(user, resetUrl);

            if (!result.Succeeded)
            {
                AddModelError(result.Errors);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userAccountService.GetUserByUserNameAsync(model.UserNameOrEmail)
                ?? await _userAccountService.GetUserByEmailAsync(model.UserNameOrEmail);

            if (user == null || !user.IsEnabled)
            {
                AddModelError("Invalid password reset request.");
                return BadRequest(ModelState);
            }

            var token = Uri.UnescapeDataString(model.Token);
            var result = await _userAccountService.ResetPasswordWithTokenAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
            {
                AddModelError(result.Errors);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPost("change-password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.Equals(model.NewPassword, model.ConfirmPassword, StringComparison.Ordinal))
            {
                AddModelError("New password and confirmation password do not match.", nameof(model.ConfirmPassword));
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var user = await _userAccountService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound(userId);

            var result = await _userAccountService.UpdatePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Contains("Incorrect password", StringComparison.OrdinalIgnoreCase)))
                    AddModelError("Current password is incorrect.");
                else
                    AddModelError(result.Errors);

                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpGet("users/me")]
        [ProducesResponseType(200, Type = typeof(UserVM))]
        public async Task<IActionResult> GetCurrentUser()
        {
            return await GetUserById(GetCurrentUserId());
        }

        [HttpGet("users/{id}", Name = nameof(GetUserById))]
        [ProducesResponseType(200, Type = typeof(UserVM))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id,
                UserAccountManagementOperations.ReadOperationRequirement)).Succeeded)
                return new ChallengeResult();

            var userVM = await GetUserViewModelHelper(id);

            if (userVM != null)
                return Ok(userVM);

            return NotFound(id);
        }

        [HttpGet("users/username/{userName}")]
        [ProducesResponseType(200, Type = typeof(UserVM))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var appUser = await _userAccountService.GetUserByUserNameAsync(userName);

            if (!(await _authorizationService.AuthorizeAsync(User, appUser?.Id ?? string.Empty,
                UserAccountManagementOperations.ReadOperationRequirement)).Succeeded)
                return new ChallengeResult();

            var userVM = appUser != null ? await GetUserViewModelHelper(appUser.Id) : null;

            if (userVM != null)
                return Ok(userVM);

            return NotFound(userName);
        }

        [HttpGet("users")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserVM>))]
        public async Task<IActionResult> GetUsers()
        {
            return await GetUsers(-1, -1);
        }

        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [Authorize(AuthPolicies.ViewAllUsersPolicy)]
        [ProducesResponseType(200, Type = typeof(List<UserVM>))]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _userAccountService.GetUsersAndRolesAsync(pageNumber, pageSize);

            var usersVM = new List<UserVM>();

            foreach (var item in usersAndRoles)
            {
                var userVM = _mapper.Map<UserVM>(item.User);
                userVM.Roles = item.Roles;

                usersVM.Add(userVM);
            }

            return Ok(usersVM);
        }

        [HttpPut("users/me")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UserEditVM user)
        {
            var userId = GetCurrentUserId($"Error retrieving the userId for user \"{user.UserName}\".");
            return await UpdateUser(userId, user);
        }

        [HttpPut("users/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserEditVM user)
        {
            var appUser = await _userAccountService.GetUserByIdAsync(id);
            var currentRoles = appUser != null
                ? (await _userAccountService.GetUserRolesAsync(appUser)).ToArray() : null;

            var manageUsersPolicy = _authorizationService.AuthorizeAsync(User, id,
                UserAccountManagementOperations.UpdateOperationRequirement);
            var assignRolePolicy = _authorizationService.AuthorizeAsync(User, (user.Roles, currentRoles),
                AuthPolicies.AssignAllowedRolesPolicy);

            if ((await Task.WhenAll(manageUsersPolicy, assignRolePolicy)).Any(r => !r.Succeeded))
                return new ChallengeResult();

            if (appUser == null)
                return NotFound(id);

            if (!string.IsNullOrWhiteSpace(user.Id) && id != user.Id)
                AddModelError("Conflicting user id in parameter and model data.", nameof(id));

            var isNewPassword = !string.IsNullOrWhiteSpace(user.NewPassword);
            var isNewUserName = !appUser.UserName!.Equals(user.UserName, StringComparison.OrdinalIgnoreCase);

            if (GetCurrentUserId() == id)
            {
                if (string.IsNullOrWhiteSpace(user.CurrentPassword))
                {
                    if (isNewPassword)
                        AddModelError("Current password is required when changing your own password.", "Password");

                    if (isNewUserName)
                        AddModelError("Current password is required when changing your own username.", "Username");
                }
                else if (isNewPassword || isNewUserName)
                {
                    if (!await _userAccountService.CheckPasswordAsync(appUser, user.CurrentPassword))
                        AddModelError("The username/password couple is invalid.");
                }
            }

            if (ModelState.IsValid)
            {
                _mapper.Map(user, appUser);

                var result = await _userAccountService.UpdateUserAsync(appUser, user.Roles);

                if (result.Succeeded)
                {
                    if (isNewPassword)
                    {
                        if (!string.IsNullOrWhiteSpace(user.CurrentPassword))
                            result = await _userAccountService.UpdatePasswordAsync(appUser, user.CurrentPassword,
                                user.NewPassword!);
                        else
                            result = await _userAccountService.ResetPasswordAsync(appUser, user.NewPassword!);
                    }

                    if (result.Succeeded)
                        return NoContent();
                }

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPatch("users/me")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] JsonPatchDocument<UserPatchVM> patch)
        {
            return await UpdateUser(GetCurrentUserId(), patch);
        }

        [HttpPatch("users/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] JsonPatchDocument<UserPatchVM> patch)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id,
                UserAccountManagementOperations.UpdateOperationRequirement)).Succeeded)
                return new ChallengeResult();

            var appUser = await _userAccountService.GetUserByIdAsync(id);
            if (appUser == null)
                return NotFound(id);

            var userPVM = _mapper.Map<UserPatchVM>(appUser);
            patch.ApplyTo(userPVM, e => AddModelError(e.ErrorMessage));

            if (ModelState.IsValid)
            {
                _mapper.Map(userPVM, appUser);

                var result = await _userAccountService.UpdateUserAsync(appUser);

                if (result.Succeeded)
                    return NoContent();

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("users")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(201, Type = typeof(UserVM))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> Register([FromBody] UserEditVM user)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, (user.Roles, Array.Empty<string>()),
                AuthPolicies.AssignAllowedRolesPolicy)).Succeeded)
                return new ChallengeResult();

            if (string.IsNullOrWhiteSpace(user.NewPassword))
                AddModelError($"{nameof(user.NewPassword)} is required when registering a new user.",
                    nameof(user.NewPassword));

            if (user.Roles == null)
                AddModelError($"{nameof(user.Roles)} is required when registering a new user.", nameof(user.Roles));

            if (ModelState.IsValid)
            {
                var appUser = _mapper.Map<ApplicationUser>(user);
                var result = await _userAccountService.CreateUserAsync(appUser, user.Roles!, user.NewPassword!);

                if (result.Succeeded)
                {
                    var userVM = await GetUserViewModelHelper(appUser.Id);
                    return CreatedAtAction(nameof(GetUserById), new { id = userVM?.Id }, userVM);
                }

                AddModelError(result.Errors);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(200, Type = typeof(UserVM))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!(await _authorizationService.AuthorizeAsync(User, id,
                UserAccountManagementOperations.DeleteOperationRequirement)).Succeeded)
                return new ChallengeResult();

            var appUser = await _userAccountService.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            var canDelete = await _userAccountService.TestCanDeleteUserAsync(id);
            if (!canDelete.Success)
            {
                AddModelError($"User \"{appUser.UserName}\" cannot be deleted at this time. " +
                    "Delete the associated records and try again.");
                AddModelError(canDelete.Errors, "Records");
            }

            if (ModelState.IsValid)
            {
                var userVM = await GetUserViewModelHelper(appUser.Id);
                var result = await _userAccountService.DeleteUserAsync(appUser);

                if (!result.Succeeded)
                {
                    throw new UserAccountException($"The following errors occurred whilst deleting user \"{id}\": " +
                        $"{string.Join(", ", result.Errors)}");
                }

                return Ok(userVM);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("users/unblock/{id}")]
        [Authorize(AuthPolicies.ManageAllUsersPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var appUser = await _userAccountService.GetUserByIdAsync(id);

            if (appUser == null)
                return NotFound(id);

            appUser.LockoutEnd = null;
            var result = await _userAccountService.UpdateUserAsync(appUser);

            if (!result.Succeeded)
            {
                throw new UserAccountException($"The following errors occurred whilst unblocking user: " +
                    $"{string.Join(", ", result.Errors)}");
            }

            return NoContent();
        }

        [HttpGet("users/me/preferences")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> UserPreferences()
        {
            var userId = GetCurrentUserId();

            var appUser = await _userAccountService.GetUserByIdAsync(userId);
            if (appUser != null)
                return Ok(appUser.Configuration);

            return NotFound(userId);
        }

        [HttpPut("users/me/preferences")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UserPreferences([FromBody] string? data)
        {
            var userId = GetCurrentUserId();
            var appUser = await _userAccountService.GetUserByIdAsync(userId);

            if (appUser != null)
            {
                appUser.Configuration = data;
                var result = await _userAccountService.UpdateUserAsync(appUser);

                if (!result.Succeeded)
                {
                    throw new UserAccountException(
                        $"The following errors occurred whilst updating User Configurations: " +
                        $"{string.Join(", ", result.Errors)}");
                }

                return NoContent();
            }

            return NotFound(userId);
        }

        private async Task<UserVM?> GetUserViewModelHelper(string userId)
        {
            var userAndRoles = await _userAccountService.GetUserAndRolesAsync(userId);
            if (userAndRoles == null)
                return null;

            var userVM = _mapper.Map<UserVM>(userAndRoles.Value.User);
            userVM.Roles = userAndRoles.Value.Roles;

            return userVM;
        }

        [HttpPost("test-email")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> TestEmailSending([FromQuery] string recipientEmail)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                AddModelError("recipientEmail query parameter is required");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Testing email sending to {RecipientEmail}", recipientEmail);

            var testBody = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
</head>
<body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Email Test</h2>
        <p>This is a test email from VCP Aesthetic Clinic.</p>
        <p>If you receive this, your email configuration is working correctly!</p>
        <p>Sent at: " + DateTime.Now + @"</p>
    </div>
</body>
</html>";

            var result = await _userAccountService.SendTestEmailAsync(recipientEmail, testBody);

            if (!result.Succeeded)
            {
                AddModelError(string.Join(", ", result.Errors));
                return BadRequest(ModelState);
            }

            return Ok(new { message = "Test email sent successfully", recipient = recipientEmail });
        }
    }
}
