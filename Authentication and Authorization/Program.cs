using Authentication_and_Authorization.Authorization;
using Microsoft.AspNetCore.Authorization;
using static Authentication_and_Authorization.Constants.AuthSchemes;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication().AddCookie(MyCookie, options =>
{
    options.Cookie.Name = MyCookie;
    // moge uzyc accessdeniedpath aby przekierowac uzytkownika w przypadku braku uprawnien do danej strony
    // options.AccessDeniedPath = "/Account/AccessDenied";

    // po 30 minutach bedzie sie trzeba znowu zalogowac. W przypadku zakniecia przegladarki cookie zniknie i bedzie trzeba sie znowu zalogowac
    // Trzeba dodatkowo ustawic persistent cookie jesli u¿ytkownik zaznaczy "Remember Me"
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBelongToHRDepartment", policy => policy.RequireClaim("Department", "HR"));
    options.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("HRManagerOnly", policy =>
    {
        policy.RequireClaim("Department", "HR");
        policy.RequireClaim("Manager");
        policy.Requirements.Add(new HRManagerProbationRequirement(3));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

// Rejestracja HttpClient do komunikacji z Web API w celu pobrania danych o pogodzie
builder.Services.AddHttpClient("TestWebAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7026/");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
