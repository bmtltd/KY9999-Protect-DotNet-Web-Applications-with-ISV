using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(x => {
    x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(x => {
    x.Cookie.HttpOnly = true;
    x.ExpireTimeSpan = TimeSpan.FromHours(2);
})
.AddOpenIdConnect(options =>
{
    options.ClientId = "{client_id}";
    options.ClientSecret = "{client_secret}";
    options.Authority = "{tenant_id}/oidc/endpoint/default";
    options.SignInScheme = "Cookies";
    options.ResponseType = "code";
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;
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
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); //<- Add this line
app.UseAuthorization();


app.MapRazorPages().RequireAuthorization();


app.Run();
