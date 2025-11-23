using FluentValidation;
using Parking_Ticket_Management_App.Controllers;
using Parking_Ticket_Management_App.Controllers.Models.BuyMonthTicket;
using Parking_Ticket_Management_App.Controllers.Models.UseTicket;
using Parking_Ticket_Management_App.Controllers.Models.ValidateTicket;
using Parking_Ticket_Management_App.Logic;
using Parking_Ticket_Management_App.Memory;
using Parking_Ticket_Management_App.Utils;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Fix: Use AddFluentValidationAutoValidation and AddFluentValidationClientsideAdapters instead of AddFluentValidation
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IValidator<BuyTicketRequest>, BuyTicketRequestValidator>();
builder.Services.AddScoped<IValidator<ValidateTicketRequest>, ValidateTicketRequestValidator>();
builder.Services.AddScoped<IValidator<UseTicketRequest>, UseTicketRequestValidator>();

builder.Services.AddScoped<IPriceCalculationLogicHandler, PriceCalculationLogicHandler>();
builder.Services.AddScoped<IBuyTicketLogicHandler, BuyTicketLogicHandler>();
builder.Services.AddScoped<IParkingTicketHandler, ParkingTicketHandler>();
builder.Services.AddScoped<IValidateTicketLogicHandler, ValidateTicketLogicHandler>();
builder.Services.AddScoped<IUseTicketLogicHandler, UseTicketLogicHandler>();
builder.Services.AddScoped<ISystemDateTimeProvider, SystemDateTimeProvider>();

builder.Services.AddSingleton<IMemoryAccess, MemoryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
