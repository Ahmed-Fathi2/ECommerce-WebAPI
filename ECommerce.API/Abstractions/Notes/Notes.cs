using Mapster;
using Microsoft.AspNetCore.Identity;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Abstractions.Auth;
using ECommerce.BLL.Abstractions.Constants;
using ECommerce.BLL.Abstractions.Errors.User;
using ECommerce.BLL.Dtos.Auth;
using ECommerce.DAL.Data.Entities;


namespace ECommerce.BLL.Managers.Auth
{
    public class Notes 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtProvider jwtProvider;

        public Notes(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            this.jwtProvider = jwtProvider;
        }

     

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            // ==========================================================================================================================
            //1- check Email Exist
            //2- check Email Confirmation
            //3- check User Lockout
            //4- Check Password

            // 2 & 3 & 4  --->> Catched Auto When Using  -->>>  _signInManager.CheckPasswordSignInAsync
            // 2 & 3 & 4  --->> Not Catched whne Usinf   --->>  _userManager.CheckPasswordAsync

            // ==========================================================================================================================

            // 1-  Validate Email
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
                return Result.Failure<LoginResponse>(UserError.InvalidEmailOrPassword);

            // ==========================================================================================================================

            //Validate Confirm Email [Manually] --> Used When :
            /* 
            1-  Used IF You Dont Enable Email Confirmation In Identity Configurations
              options.SignIn.RequireConfirmedEmail = false;

            2- Use _userManager.CheckPasswordAsync
            */

            //if (!user.EmailConfirmed)
            //    return Result.Failure<LoginResponse>(UserError.EmailNotConfirmed);

            // ==========================================================================================================================

            // Check
            //1- If Email Confirmantion Or Not
            //2- If User Lockout Or Not
            //3- If Password Correct Or Not

            //Note::: You Must Enable Idenetity config To Use built_in Email Confirmation And Lockout 


            //var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            // Not Catch If User Lockout Or Not
            // Not Catch If Email Confirmed Or Not
            // It Retutn Bool  so you Must Check Manually Each Error To Know What Is The Exact Error
            // Check Manually ->>
            //if (!user.EmailConfirmed)
            //    return Result.Failure<LoginResponse>(UserError.EmailNotConfirmed);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                var error = result.IsNotAllowed   //    options.SignIn.RequireConfirmedEmail = true;
                    ? UserError.EmailNotConfirmed

                    : result.IsLockedOut  // _signInManager.CheckPasswordSignInAsync(_, _, lockoutOnFailure: true);
                                          //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                                          //options.Lockout.MaxFailedAccessAttempts = 5;
                                          //options.Lockout.AllowedForNewUsers = true;
                                          //await _userManager.SetLockoutEndDateAsync(user, null); if you want to unlock user manually [Note]
                    ? UserError.UserLockedOut

                    : UserError.InvalidEmailOrPassword;

                return Result.Failure<LoginResponse>(error);
            }

            var roles = await _userManager.GetRolesAsync(user);


            // Generate JWT Token
            var (token, expireIn) = jwtProvider.GenerateToken(user, roles);
            var response = new LoginResponse(
                                Id: user.Id, FirstName: user.FirstName,
                                LastName: user.LastName, Email: user.Email!,
                                Token: token, ExpiresIn: expireIn);

            return Result.Success(response);


        }

        public async Task<Result> RegisterAsync(RegisterRequest registerRequest)
        {
            var userIsExist = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (userIsExist is not null)
                return Result.Failure(UserError.EmailAlreadyExists);

            var user = registerRequest.Adapt<ApplicationUser>();


            var userResult = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!userResult.Succeeded)
            {
                var error = userResult.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, ErrorStatusCodes.BadRequest));

            }


            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRole.User);

            if (!roleResult.Succeeded)
            {
                var error = roleResult.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, ErrorStatusCodes.BadRequest));
            }


            //ToDo
            // Send Confirmation Email

            return Result.Success();


        }
    }
}








