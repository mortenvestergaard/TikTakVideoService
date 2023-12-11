using Azure.Storage.Blobs;
using TikTakVideoService.Services;

namespace TikTakVideoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.
            builder.Services.AddScoped(client => new BlobServiceClient(config.GetConnectionString("AzureStorage")));
            builder.Services.AddGrpc();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            app.MapGrpcService<GreeterService>();
            app.MapGrpcService<VideoService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}