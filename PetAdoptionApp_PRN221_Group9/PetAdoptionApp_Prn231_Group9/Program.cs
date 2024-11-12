using DataAccessObjects;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLogicLayer.Commons;
using PetAdoptionApp_Prn231_Group9.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PayPalCheckoutSdk.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using BusinessObjects.Enum;
using PetAdoptionApp_Prn231_Group9.Helpers;



var MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
/*var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
*/


var configuration = builder.Configuration.Get<AppConfiguration>() ?? new AppConfiguration();

// Add services to the container.
builder.Services.AddInfrastructuresServices(configuration.DatabaseConnection);
builder.Services.AddSingleton<AppConfiguration>();
builder.Services.Configure<AppConfiguration>(builder.Configuration);
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("JWTSection"));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.Configure<PaypalSettings>(builder.Configuration.GetSection("PayPal"));

// Add the service for the HttpClient and inject the PayPal settings
builder.Services.AddSingleton<PayPalHttpClient>(provider =>
{
    var paypalSettings = provider.GetRequiredService<IOptions<PaypalSettings>>().Value;

    // Configure PayPal environment
    var environment = new SandboxEnvironment(paypalSettings.ClientId, paypalSettings.SecretKey);

    return new PayPalHttpClient(environment);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(Role.Administrator), policy =>
        policy.RequireClaim("Role", nameof(Role.Administrator)));
    options.AddPolicy(nameof(Role.Staff), policy =>
        policy.RequireClaim("Role", nameof(Role.Staff)));
    options.AddPolicy(nameof(Role.User), policy =>
        policy.RequireClaim("Role", nameof(Role.User)));
});



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("http://localhost:5173", "https://localhost:5173"
            , "https://localhost:3000", "http://localhost:3000", "https://petadoptionsecondapi-ekaxg2c0ccg3fthq.southeastasia-01.azurewebsites.net"
            , "http://petadoptionsecondapi-ekaxg2c0ccg3fthq.southeastasia-01.azurewebsites.net"
            , "https://pet-adoption-admin.vercel.app")
        .WithHeaders(
                "Authorization",    // For bearer token
                "Content-Type",     // For application/json
                "Accept",          // For content negotiation
                "Origin",          // Required for CORS
                "X-Requested-With" // For AJAX requests
            )
        .WithMethods(
                "GET",
                "POST",
                "PUT",
                "DELETE",
                "PATCH",
                "OPTIONS"
            )
        .AllowCredentials();
    });
});

//validate your important field for token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration.JWTSection.Issuer,
            ValidAudience = configuration.JWTSection.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JWTSection.SecretKey)),
            RoleClaimType = "Role",
            ClockSkew = TimeSpan.Zero
        };
    });
    /*.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches()
            .AddDownstreamApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();*/





//customizing your swagger for bearer authorization
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Adoption API", Version = "v1" });
    setup.OperationFilter<SwaggerFileUploadOperationFilter>();
    setup.OperationFilter<SwaggerFileUploadOperationFilterEvent>();
    setup.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(MyAllowSpecificOrigins);
}



// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
//Use middleware to confirm account

app.UseMiddleware<ConfirmationMiddleware>();

app.MapControllers();

app.Run();
