using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product_Management.Data;

namespace Product_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //......This register the Register the MySQL EF Core provider.....//
            var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connStr);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connStr, serverVersion)
            );

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles(); //----This is for image store---//
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

      
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
