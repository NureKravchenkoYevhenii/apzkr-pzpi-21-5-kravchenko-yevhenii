using Infrastructure.Configs;
using Microsoft.EntityFrameworkCore;
using DAL.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Infrastructure.Resources;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Infrastructure.Constants;
using Parky.Infrastructure.HostedServices;

namespace Parky.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder SetDefaultConfiguration(
        this WebApplicationBuilder builder)
    {
        builder.Host.ConfigureAutofac();

        var connectionModel = builder.Configuration.GetSection("Connection").Get<ConnectionModel>()!;

        var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? connectionModel.Host;
        var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? connectionModel.Database;
        var dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD") ?? connectionModel.Password;

        var connectionString = string.Format(connectionModel.ConnectionString, dbHost, dbName, dbPassword);

        builder.Services.AddDbContext<ParkyDbContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddSingleton(connectionModel);

        var authOptions = builder.Configuration.GetSection("Auth").Get<AuthOptions>()!;
        authOptions.PrivateKeyString = builder.Configuration.GetSection("PrivateKeyString").Get<string>()!;

        builder.Services.AddSingleton(authOptions);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authOptions.PublicKey,
                    ClockSkew = TimeSpan.Zero
                };
            });

        var emailCreds = builder.Configuration.GetSection("EmailCreds").Get<EmailCreds>()!;
        var paymentCreds = builder.Configuration.GetSection("PaymentCreds").Get<PaymentCreds>()!;
        //var rfidReaderSettings = builder.Configuration.GetSection("RFIDReaderSettings").Get<RFIDReaderSettings>();
        builder.Services.AddSingleton(emailCreds);
        builder.Services.AddSingleton(paymentCreds);
        //builder.Services.AddSingleton(rfidReaderSettings);

        builder.Services.AddMapper();

        #region Localization

        builder.Services.AddLocalization();
        builder.Services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US")
                    {
                        DateTimeFormat = {
                            LongTimePattern = "dddd, MMMM d, yyyy h:mm:ss tt",
                            ShortTimePattern = "dddd, MMMM d, yyyy h:mm:ss tt"
                        }
                    },
                    new CultureInfo("uk-UA")
                    {
                        DateTimeFormat = {
                            LongTimePattern = "d MMMM yyyy 'р.'HH:mm:ss",
                            ShortTimePattern = "d MMMM yyyy 'р.'HH:mm:ss"
                        }
                    },
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "uk-UA", uiCulture: "uk-UA");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

        builder.Services.AddMvc()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(Resources));
            });

        #endregion

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });


        builder.Services.AddCors(option =>
        {
            option.AddPolicy(ParkyConstants.ALLOW_ANY_ORIGINS, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddHostedService<ExpiredBookingsCleaner>();

        return builder;
    }
}
