using Al_Maqraa.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
namespace Al_Maqraa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _service;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager,UserService service, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _service = service;
            _emailSender = emailSender;
        }

        [HttpGet("send")]
        public async Task<IActionResult> send(string email)
        {
            await _emailSender.SendEmailAsync(email, "Confirm your email", $" $\"Please confirm your account by clicking this <a href='{{callbackUrl}}'>link</a>.\"");
            return Ok("ok");
        }
  
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (token == null || email == null)
                return BadRequest("Invalid email confirmation request");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid email confirmation request");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Redirect("/EmailConfirm.html");
                // return Ok("Email confirmed successfully!");
            }

            return BadRequest("Email confirmation failed");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var userModel = new User
                {
                    Name = model.Name,
                    UserName = model.Name.Replace(" ", "") + Guid.NewGuid().ToString(),
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                };
                if (await _userManager.FindByEmailAsync(model.Email) == null)
                {
                   // userModel.EmailConfirmed = true;
                    var result = await _userManager.CreateAsync(userModel, model.Password);//sign up
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(userModel);
                        var confirmationLink = Url.Action("ConfirmEmail", "User", new { token, email = userModel.Email }, Request.Scheme);
                        await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                            $"Welcome to AL-Maqraa\nYou're about to use the website just click on the link to confirm your email!" +
                            $"\nPlease confirm your account by clicking this <a href='{confirmationLink}'>link</a>.");
                        await _signInManager.PasswordSignInAsync(userModel, model.Password, isPersistent: true, lockoutOnFailure: false);
                        //login
                        //  await _signInManager.SignInAsync(userModel, isPersistent: true);
                        return Ok("Registration successful. Please check your email to confirm your account.");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Is Already Exist!");

                }
            }
            return BadRequest(ModelState);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
//ppppp
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password) && user.EmailConfirmed)
                {
                    await _signInManager.PasswordSignInAsync(user, model.Password,isPersistent:true,lockoutOnFailure:false);
                    return Ok();
                }
                else if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Wrong Email .");
                }
                else if(!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Wrong Password .");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please Confirm Your Email! we send another confirmation message.");
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "User", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                        $"Welcome to AL-Maqraa\nYou're about to use the website just click on the link to confirm your email!" +
                        $"\nPlease confirm your account by clicking this <a href='{confirmationLink}'>link</a>.");
                }
            }

            return BadRequest(ModelState);
        }
        //------------------------forgot password
        [HttpPost("Forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{Request.Scheme}://{Request.Host}/ResetPassword.html?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password using this link: <a href='{resetLink}'>link</a>");

            return Ok("Reset password link has been sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid input");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
                return BadRequest("Error resetting password");

            return Ok("Password has been reset successfully.");
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var user = await _service.GetAllAsync();
            if (user == null)
            {
                return NotFound();
            }
            return user.ToList();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }
            var User = await _service.GetByIdAsync(id);
            if (User == null)
            {
                return NotFound();
            }

            return User;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserDTO userDTO)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user==null)
            {
                return BadRequest();
            }


            try
            {
                user.Name = userDTO.Name ?? user.Name;
                user.PhoneNumber = userDTO.PhoneNumber ?? user.PhoneNumber;
                user.Gender = userDTO.Gender ?? user.Gender;
                await _service.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User User)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return Problem("Entity set 'User'  is null.");
            }
            await _service.AddAsync(User);
            return CreatedAtAction("GetUser", new { id = User.Id }, User);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }

            var User = await _service.GetByIdAsync(id);
            if (User == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(User.Id);

            return NoContent();
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            var users =  await _service.GetAllAsync();

            return (users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet("statistics/{userId}")]
        public async Task<IActionResult> GetStatisticByUserId(string userId)
        {

            // Retrieve the user associated with the statistic
            var satistic = await _service.GetStatisticByUserId(userId);
            //Statistics statistic = await _context.Statistics.Include(s =>s.User).FirstOrDefaultAsync(ss =>ss.Id==statisticId);
            //var user = statistic.User;
            if (satistic == null)
            {
                return NotFound("User not found");
            }

            return Ok(satistic);
        }
        [HttpGet("Days/{userId}")]
        public async Task<IActionResult> GetDaysByUserId(string userId)
        {

            //Retrieve the user associated with the statistic
             var Days = await _service.GetDaysByUserId(userId);
            //Statistics statistic = await _context.Statistics.Include(s =>s.User).FirstOrDefaultAsync(ss =>ss.Id==statisticId);
            //var user = statistic.User;
            if (Days == null)
            {
                return NotFound("User not found");
            }

            return Ok(Days);
        }
    }
}
