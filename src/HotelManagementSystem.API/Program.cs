using HotelManagementSystem.API.ExceptionHandlers;
using HotelManagementSystem.Common.Configs;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.DataAccess.DbContext;
using HotelManagementSystem.DataAccess.Repositories;
using HotelManagementSystem.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<ICategoriesRepo, CategoriesRepo>();
builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddScoped<IRoomsRepo, RoomsRepo>();
builder.Services.AddScoped<IBookingsService, BookingsService>();
builder.Services.AddScoped<IBookingsRepo, BookingsRepo>();

// Add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add configurations
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

//Add logging
builder.Services.AddLogging(configure => configure.AddConsole());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
