

using Microsoft.IdentityModel.Tokens;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Service;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ParentBookingAPI.Service.UserService;
using ParentBookingAPI.Repository.Interfaces;
using ParentBookingAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITourBookingRespository, TourBookingRepository>();
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<DatabaseHelper>((provider) =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new DatabaseHelper(connectionString);
});


builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme (\"bearer  {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey

        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


// Configure the app
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.AddControllers();


// This code for Multiple Token - Custom

builder.Services.AddAuthentication("Token1Scheme")
    .AddJwtBearer("Token1Scheme", options =>
{

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,

        //match the time with method time
        ClockSkew = TimeSpan.Zero
    };
});

// This code for Single Token - Default

//builder.Services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    //options.RequireHttpsMetadata = false;
//    //options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
//        ValidateIssuer = false,
//        ValidateAudience = false,

//        //ValidateIssuer = true,
//        //ValidateAudience = true,
//        //ValidAudience = builder.Configuration["Jwt:Audience"],
//        //ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalAngularAppPolicy",
        builder =>
        {
            builder.WithOrigins("https://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
             .AllowCredentials(); // Allow credentials (cookies, etc.)
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS
//app.UseCors("FrontEnd");

app.UseCors("LocalAngularAppPolicy");

app.UseHttpsRedirection();

// Use authentication and routing
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
