
using HQSoft_EX01.Configurations;
using HQSoft_EX01.Data;
using HQSoft_EX01.Exceptions;
using HQSoft_EX01.Services.AuthorService;
using HQSoft_EX01.Services.BookService;
using HQSoft_EX01.Services.ReportService;
using Microsoft.EntityFrameworkCore;

namespace HQSoft_EX01
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateWebApplicationBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            await ConfigureApplicationAsync(app);
        }

        private static WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var port = Environment.GetEnvironmentVariable("PORT");
            if (port != null)
            {
                builder.WebHost.UseUrls($"http://*:{port}");
            }

            return builder;
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            var databaseConnectionString = ConnectionHelper.GetDatabaseConnectionString(builder.Configuration);
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(databaseConnectionString));
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

            builder.Services.AddScoped<IAuthorService, AuthorServiceImp>();
            builder.Services.AddScoped<IBookService, BookServiceImp>();
            builder.Services.AddScoped<IReportService, ReportServiceImp>();

            // Add services to the container.
            builder.Services.AddSwaggerGen();
        }

        private static async Task ConfigureApplicationAsync(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Seed database or perform other startup tasks
            using (var scope = app.Services.CreateScope())
            {
                await DataHelper.ManageDataAsync(scope.ServiceProvider);
            }

            // Configure middleware pipeline
            // app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthorization();
            app.UseExceptionHandler(opt => { });

            app.MapControllers();

            app.Run();
        }
    }
}
