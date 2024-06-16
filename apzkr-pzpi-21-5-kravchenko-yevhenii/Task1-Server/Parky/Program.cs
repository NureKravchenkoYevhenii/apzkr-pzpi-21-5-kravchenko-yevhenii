using Infrastructure.Constants;
using Microsoft.Extensions.Options;
using Parky.Infrastructure.Extensions;
using Parky.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.SetDefaultConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions!.Value);

app.UseErrorHandler();

app.UseHttpsRedirection();

app.UseCors(ParkyConstants.ALLOW_ANY_ORIGINS);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
