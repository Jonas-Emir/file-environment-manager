using FileEnvironmentManager.Application.Menu;
using FileEnvironmentManager.Domain.Interfaces;
using FileEnvironmentManager.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileEnvironmentManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<MenuService>();

            var serviceProvider = services.BuildServiceProvider();
            var menuService = serviceProvider.GetRequiredService<MenuService>();
            await menuService.StartAsync();
        }
    }
}
