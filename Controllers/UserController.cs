using Al_Maqraa.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data.Entity;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace Al_Maqraa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public UserController(SignInManager<User> signInManager, UserManager<User> userManager,UserService service)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _service = service;
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userModel = new User
                {
                    UserName = model.Name + Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Password = model.Password
                };
                if (await _userManager.FindByEmailAsync(model.Email) == null)
                {
                    userModel.EmailConfirmed = true;
                    var result = await _userManager.CreateAsync(userModel, model.Password);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(userModel, isPersistent: true);
                        var token = await _userManager.GetAuthenticationTokenAsync(userModel, "JwtBearer", "access_token");
                        return Ok(new { Token = token }); 
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
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    var token = await _userManager.GetAuthenticationTokenAsync(user, "JwtBearer", "access_token");
                    return Ok(new { Token = token });
                }
               
                else
                {
                    ModelState.AddModelError(string.Empty, "Wrong Email or Password .");
                }
            }

            return BadRequest(model);
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
        public async Task<IActionResult> PutUser(string id, User User)
        {
            if (id != User.Id)
            {
                return BadRequest();
            }


            try
            {
                await _service.UpdateAsync(User);
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
    }
}
