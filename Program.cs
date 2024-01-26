//using AsistenciasApi.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpContextAccessor();

/*builder.Services.AddDbContext<AsistenciasDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")
    ?? throw new NotImplementedException()));
*/
string key = "fdgeresdksdslssdfasffeDFQERQ=121";

builder.Services.AddAuthentication("Bearer").AddJwtBearer(
    option =>
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

        //option.RequireHttpsMetadata = false;

        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false, // No valides el emisor
            ValidateAudience = false, // No valides la audiencia
            ValidateLifetime = true,
            //ClockSkew = TimeSpan.Zero,
            //RoleClaimType = ClaimTypes.Role,
            //RoleClaimType = "roles",
            ValidAlgorithms = new[] { "HS256" },
            IssuerSigningKey = signingKey
        };
        
    });
/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminRole", policy => policy.RequireClaim("roles", "admin"));
    //options.AddPolicy("AdminRole", policy => policy.RequireRole("admin"));
});*/

// 5. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
