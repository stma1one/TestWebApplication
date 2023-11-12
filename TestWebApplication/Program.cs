

using System.Text.Json;
using System.Text.Json.Serialization;
using ProjectBL.Models;
using Microsoft.EntityFrameworkCore;

namespace TestWebApplication
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
       
    
            #region DBCONTEXT
            //load connection string
            string connection = builder.Configuration.GetConnectionString("WallaDB");
            //Add DBContext
            builder.Services.AddDbContext<WallaDbContext>(options=>options.UseSqlServer(connection));
            #endregion
            //add controller
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region 1- Json handling
            //json handling
            builder.Services.AddControllers().
                AddJsonOptions(o=>o.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.Preserve);
          
            #endregion

            #region 2- Session Support
          
            //Add Session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(180);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            #region Use FILES And Session
            //use files 
            app.UseStaticFiles();
            app.UseRouting();
            #region Use Session
            app.UseSession();
            #endregion
            #endregion

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}