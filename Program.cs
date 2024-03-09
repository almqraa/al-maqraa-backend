using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AlMaqraaDB>();
builder.Services.AddScoped<SurahService>();
builder.Services.AddScoped<AyahService>();
builder.Services.AddScoped<SheikhService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StatisticsService>();


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AlMaqraaDB>()
    .AddApiEndpoints()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>().AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseStaticFiles();
app.MapIdentityApi<User>();
app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}").RequireAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
