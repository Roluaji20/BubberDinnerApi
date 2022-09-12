using BubberDinner.Api;
using BubberDinner.Api.Common.Errors;
using BubberDinner.Aplication;
using BubberDinner.Infraestructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddPresentation();
    builder.Services.AddAplication();
    builder.Services.AddInfraestructure(builder.Configuration);
    
}   

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization(); 
    app.MapControllers();
    app.Run();
}
