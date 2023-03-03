using Application.Interfaces.Contexts;
using Application.Repositories;
using Application.Services;
using MainEndpoint.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Infrastructure.IdentityConfig;
using MainEndpoint.Token;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
string connection = Configuration["ConnectionString:SqlServer"];
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
builder.Services.AddIdentityService(Configuration);
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
builder.Services.AddTransient<NoteRepository, NoteRepository>();
builder.Services.AddTransient<UserTokenRepository, UserTokenRepository>();
builder.Services.AddTransient<ITokenValidator, TokenValidate>();
builder.Services.AddTransient<INoteServices, NoteServices>();
builder.Services.AddScoped<CreateToken, CreateToken>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureJWT(Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
