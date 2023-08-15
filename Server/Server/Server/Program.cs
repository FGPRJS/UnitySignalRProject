

using Server.Hubs;
using Server.Manager;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddSignalR();

builder.Services.AddSingleton<GameConstantManager>();
builder.Services.AddSingleton<GameServerManager>();

builder.Services.AddControllers();


// Configure the HTTP request pipeline.

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapHub<GameServerHub>("/gameserver");

app.Run();
