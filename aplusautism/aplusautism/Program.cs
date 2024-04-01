using aplusautism.Service;
using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Bal.Logic;
using aplusautism.Data;
using aplusautism.Data.Models;
using aplusautism.Repository.Repository;
using aplusautism.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;
using aplusautism.ExceptionHandler;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); //Add this line of code


#region adding service
// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();


builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 1073741824;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1073741824; // if don't set default value is: 30 MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = 1073741824;
    options.MultipartBodyLengthLimit = 1073741824; // if don't set default value is: 128 MB
    options.MultipartHeadersLengthLimit = 1073741824;
});





builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors();


//gettiing Jwt Authentication key from Appsetting
var key = Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWT_Secret"].ToString());

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});


//builder.Services.AddAuthentication(IISServerDefaults.AuthenticationScheme);

builder.Services.AddDbContext<AplusautismDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  




//Here I setup to read appsettings with DTO AppSetting now we can discard helper models.
builder.Services.Configure<AppSettingsDTO>(builder.Configuration.GetSection("AppSettings"));

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IAzureStorage, AzureStorage>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddOptions();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#endregion



#region Add Dependency For Repository


builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AplusautismDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Auth/SignIn";
});
#endregion



#region adding DTO OR MODEL CLASS
builder.Services.AddScoped<IRepository<UserDTO>, Repository<UserDTO>>();
builder.Services.AddScoped<IRepository<LessonViewDTO>, Repository<LessonViewDTO>>();
builder.Services.AddScoped<IRepository<ForgetPasswordDTO>, Repository<ForgetPasswordDTO>>();

builder.Services.AddScoped<IRepository<SubscriptionSetup>, Repository<SubscriptionSetup>>();
builder.Services.AddScoped<IRepository<AB_Address>, Repository<AB_Address>>();

builder.Services.AddScoped<IRepository<AB_User>, Repository<AB_User>>();

builder.Services.AddScoped<IRepository<LessonSetupLanguage>, Repository<LessonSetupLanguage>>();
builder.Services.AddScoped<IRepository<LessonSetup>, Repository<LessonSetup>>();
builder.Services.AddScoped<IRepository<AB_Main>, Repository<AB_Main>>();
builder.Services.AddScoped<IRepository<GlobalCodeCategory>, Repository<GlobalCodeCategory>>();
builder.Services.AddScoped<IRepository<GlobalCodes>, Repository<GlobalCodes>>();

builder.Services.AddScoped<IRepository<PaymentsDTO>, Repository<PaymentsDTO>>();
builder.Services.AddScoped<IRepository<Payments>, Repository<Payments>>();
builder.Services.AddScoped<IRepository<LessonDetailsbycategoryDTO>, Repository<LessonDetailsbycategoryDTO>>();
builder.Services.AddScoped<IRepository<SubscriptionDTO>, Repository<SubscriptionDTO>>();
builder.Services.AddScoped<IRepository<ABuserDTO>, Repository<ABuserDTO>>();

    builder.Services.AddScoped<IRepository<ContactUsDTO>, Repository<ContactUsDTO>>();
builder.Services.AddScoped<IRepository<ContactLogDTO>, Repository<ContactLogDTO>>();

builder.Services.AddScoped<IRepository<DeviceTrackingDTO>, Repository<DeviceTrackingDTO>>();

builder.Services.AddScoped<IRepository<ClientActiveCountsDTO>, Repository<ClientActiveCountsDTO>>();
builder.Services.AddScoped<IRepository<GlobalCodesDTO>, Repository<GlobalCodesDTO>>();

builder.Services.AddScoped<IRepository<Cities>, Repository<Cities>>();
builder.Services.AddScoped<IRepository<Countries>, Repository<Countries>>();
builder.Services.AddScoped<IRepository<States>, Repository<States>>();
builder.Services.AddScoped<IRepository<ExceptionMessageTable>, Repository<ExceptionMessageTable>>();

#endregion

#region adding interface and logic
builder.Services.AddTransient<Iuserlogic, Userlogic>();
builder.Services.AddTransient<IGlobalCodeCategorylogic, GlobalCodeCategorylogic>();

builder.Services.AddTransient<ISubscriptionsetup, SubscriptionsetupLogic>();
builder.Services.AddTransient<Iclientlogic, clientlogic>();

builder.Services.AddTransient<IlessondetailsBycategory, LessonDetailsbyCategory>();

builder.Services.AddTransient<Iadmindashboard, AdmindashboardLogic>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion

// other code

// builder.Services.AddSession();

//Set Session Timeout. Default is 20 minutes.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(90);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

// global error handler

app.UseSession(); // use this before .UseEndpoints
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();

app.UseAuthorization();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});


app.MapControllerRoute(
    name: "areas",
  
    pattern: "{area:exists}/{controller=Auth}/{action=SignIn}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignIn}/{id?}");




app.Run();
