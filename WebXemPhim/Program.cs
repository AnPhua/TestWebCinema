using WebXemPhim.Services.Implements;
using WebXemPhim.Services.Interfaces;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;// JwtBearerDefaults,SymmetricSecurityKey
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text; // Encoding
using WebXemPhim.Handle.Global;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.Converters;
using MovieManagement.Services.Implements;
using MovieManagement.Services.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;

public class Program
{
    
   
    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<IAuthServices, AuthServices>();
        builder.Services.AddScoped<ICinemaServices, CinemaServices>();
        builder.Services.AddScoped<ISeatServices, SeatServices>();
        builder.Services.AddScoped<ITicketServices, TicketServices>();
        builder.Services.AddScoped<IScheduleServices, ScheduleServices>();
        builder.Services.AddScoped<IBannerServices, BannerServices>();
        builder.Services.AddScoped<IRoomServices, RoomServices>();
        builder.Services.AddScoped<IFoodServices, FoodServices>();
        builder.Services.AddScoped<IPromotionServices, PromotionServices>();
        builder.Services.AddScoped<IRankCustomerServices, RankCustomerServices>();
        builder.Services.AddScoped<IMovieServices, MovieServices>();
        builder.Services.AddScoped<IVNPayServices, VNPayServices>();
        builder.Services.AddScoped<IBillServices, BillServices>();

        builder.Services.AddSingleton<ResponseObject<DataResponsesUser>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesToken>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesMovieType>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesCinema>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesMovie>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesRoom>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesFood>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBanner>>();
        builder.Services.AddSingleton<ResponseObject<DataRepsonsesPromotion>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesRankCustomer>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesMovie>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesCinema>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesSeat>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesTicket>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesSchedule>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBill>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBillFood>>();
        builder.Services.AddSingleton<ResponseObject<DataResponsesBillTicket>>();

        builder.Services.AddScoped<AuthServices>();


        builder.Services.AddSingleton<UserConverter>();
        builder.Services.AddSingleton<TicketConverter>();
        builder.Services.AddSingleton<SeatConverter>();
        builder.Services.AddSingleton<SchedulesConverter>();
        builder.Services.AddSingleton<RoomConverter>();
        builder.Services.AddSingleton<RankCustomerConverter>();
        builder.Services.AddSingleton<BannerConverter>();
        builder.Services.AddSingleton<PromotionConverter>();
        builder.Services.AddSingleton<MovieTypeConverter>();
        builder.Services.AddSingleton<MovieConverter>();
        builder.Services.AddSingleton<CinemaConverter>();
        builder.Services.AddSingleton<FoodConverter>();
        builder.Services.AddSingleton<BillConverter>();
        builder.Services.AddSingleton<BillTicketConverter>();
        builder.Services.AddSingleton<BillFoodConverter>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
       // builder.Services.AddControllers()
//.AddJsonOptions(options =>
//{
//options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
         //       options.JsonSerializerOptions.MaxDepth = 32;
//});
        //builder.Services.AddRazorPages();
        //JsonSerializerOptions options = new()
        //{
        //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
        //    WriteIndented = true
        //};
        //builder.Services.AddControllers().AddJsonOptions(x =>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
        builder.Services.AddScoped<AuthServices>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "Example:{Token} ",
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
            });
            x.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x => {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:SecretKey").Value!))
            };
        });
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        Global.DomainName = builder.Configuration["DomainName"];

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapRazorPages();
        //    endpoints.MapControllers();
        //});
        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}