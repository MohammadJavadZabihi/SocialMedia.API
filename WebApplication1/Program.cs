using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Servies;
using SocialMedia.Core.Servies.Interface;
using SocialMedia.DataLayer.Context;
using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using SocialMedia.Core.Convertor;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Senders;


var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://localhost:44370", "https://localhost:8080");

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson();

#region Authorization

var key = Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"]
    };
});



#endregion

#region DataContext

builder.Services.AddDbContext<SocialMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalSqlContext")));

#endregion

#region IOC

builder.Services.AddScoped<IUserServies, UserServies>();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddScoped<IUserServies, UserServies>();
builder.Services.AddScoped<IAuthentication, AuthenticationServies>();
builder.Services.AddScoped<SendEmailServies>();

#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
