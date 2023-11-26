using Microsoft.EntityFrameworkCore;
using ToDoListApp.Server.DbContext;
using ToDoListApp.Server.Repositories.Implementation;
using ToDoListApp.Server.Repositories.Interface;

namespace ToDoListApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            // Inject database context, as defined in DbContext folder
            // Connects to the database using the connection string defined in appsettings.json
            builder.Services.AddDbContext<ToDoListAppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("TodoListConnectionString"));
            });

            builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            // CORS options, to allow the backend to be reachable from the frontend (here I make it public, for simplicity)
            app.UseCors(options => 
            {
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
            });

            app.UseAuthorization();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
