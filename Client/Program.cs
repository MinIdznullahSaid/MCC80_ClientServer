using Client.Contracts;
using Client.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped(typeof(IGeneralRepository<,>), typeof(GeneralRepository<,>));
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Jwt Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false;
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWTConfig:SecretKey"])),
               ValidateIssuer = false,
               //Usually, this is your application base URL
               ValidIssuer = builder.Configuration["JWTConfig:Issuer"],
               ValidateAudience = false,
               //If the JWT is created using a web service, then this would be the consumer URL.
               ValidAudience = builder.Configuration["JWTConfig:Audience"],
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
           };
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Custome Error page
app.UseStatusCodePages(async context => {
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode.Equals((int)HttpStatusCode.Unauthorized))
    {
        response.Redirect("/unauthorized");
    }
    else if (response.StatusCode.Equals((int)HttpStatusCode.NotFound))
    {
        response.Redirect("/not-found");
    }
    else if (response.StatusCode.Equals((int)HttpStatusCode.Forbidden))
    {
        response.Redirect("/forbidden");
    }
});

app.UseSession();

//Add JWToken to all incoming HTTP Request Header
app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
