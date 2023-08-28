using Microsoft.AspNetCore.StaticFiles;
using Server.Hubs;
using Server.Manager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

builder.Services.AddSignalR();

builder.Services.AddSingleton<GameConstantManager>();
builder.Services.AddSingleton<GameServerManager>();

builder.Services.AddControllers();


// Configure the HTTP request pipeline.

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = new FileExtensionContentTypeProvider()
    {
        Mappings =
        {
            [".wasm"] = "application/octet-stream",
            [".data"] = "application/octet-stream"
        }
    }
});

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameServerHub>("/gameserver");
app.MapGet("/", () =>
{
    var html = File.ReadAllText(@"./wwwroot/index.html");

    return Results.Text(html, "text/html");
});

app.Run();
