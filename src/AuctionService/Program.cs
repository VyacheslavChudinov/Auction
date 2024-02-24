using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AuctionDbContext>(config =>
    {
        config.QueryDelay = TimeSpan.FromSeconds(10);

        config.UsePostgres();
        config.UseBusOutbox();
    });
    x.UsingRabbitMq((context, config) => { config.ConfigureEndpoints(context); });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServiceUrl"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.NameClaimType = "username";
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    DbInitializer.Initialize(app);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

app.Run();