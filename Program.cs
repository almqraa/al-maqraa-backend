using Al_Maqraa.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
=======
>>>>>>> b5cee9870e47090a4a6a1272854165066e074528

builder.Services.AddScoped<AlMaqraaDB>();
builder.Services.AddScoped<SurahService>();
builder.Services.AddScoped<AyahService>();
builder.Services.AddScoped<SheikhService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<DayService>();
builder.Services.AddScoped<SpeechToTextService>();


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
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<AudioHub>("/recitations");
});

app.MapControllers();


app.Run();
