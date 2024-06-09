
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using URM.Core.Models;
using URM.Core.Services;
using URM.Repository;
using URM.Service.DependencyResolvers;
using URM.Service.Mapping;
using URM.Service.Services;
using Microsoft.AspNetCore.Authorization;

namespace URM.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Title = "User Role Management API", Version = "v1" });
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
			});



			builder.Services.AddIdentity<AppUser, AppRole>(opt =>
			{
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireUppercase = true;
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequiredLength = 6;
			}).AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();


			builder.Services.AddAuthentication(opt =>
			{

				opt.DefaultAuthenticateScheme =
				opt.DefaultChallengeScheme =
				opt.DefaultForbidScheme =
				opt.DefaultScheme =
				opt.DefaultSignInScheme =
				opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
						)
				};
			});

			builder.Services.AddDbContext<AppDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
			});



			builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

			// Autofac için yapýlandýrma
			builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
			{
				// Autofac modülünü yükleyin
				containerBuilder.RegisterModule(new AutoFacBusinessModule());
			});


			builder.Services.AddAutoMapper(typeof(MapProfile));



			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin",
					builder => builder.WithOrigins("http://localhost:5173") // React uygulamanýzýn URL'si
					.AllowAnyHeader()
					.AllowAnyMethod());
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors("AllowSpecificOrigin");

			app.UseHttpsRedirection();

			app.UseAuthentication();


			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
