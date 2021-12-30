# Protect .NET Core 3.1 Web Application (Razor pages) with IBM Security Verify

## Add OIDC protection to your web application

At this stage you should have a `clientId` and `clientSecret` issued from the ISV Admin Portal, or the ISV Developer Portal. Once you have those, you need add the neccessary libraries to your web application using NuGet. Then, add the OIDC settings required in the `Program.cs` file. Let's go through that process below.

### Getting Started

1.  Create a .NET Core new web application using the following command:
    ```
    dotnet new webapp --framework netcoreapp3.1
    ```

2.  Install the `Microsoft.AspNetCore.Authentication.OpenIdConnect` package using NuGet ([click here](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.OpenIdConnect)). Since we're using .NET 3.1, make sure it is version `3.1.X`.

3.  Add the following code to the top of the `Startup.cs` file.
    ``` c#  
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    ```

4.  In `Startup.cs` directly beneath `#!c# services.AddRazorPages();`, add the following lines of code within the `#!c# ConfigureServices()` method:
    ``` c#
    services.AddAuthentication(x => {
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
    ```
    In the above code, replace the following with the values ISV returned:
    -   Replace `#!c# "{client_id}"` with `clientId` value returned from ISV.
    -   Replace `#!c# "{client_secret}"` with the `clientSecret` value returned from ISV.
    -   Replace `#!c# "{tenant_id}/oidc/endpoint/default"` with your ISV tenant url (e.g. `https://demo.verify.ibm.com/oidc/endpoint/default`)

5.  Within `Startup.cs` add the highlighted text below into the `#!c# Configure()` method, in between `#!c# app.UseRouting();` and `#!c# app.UseAuthorization();`. When done, this area of the `Startup.cs` file should look as shown below:
    ``` c# hl_lines="2"
    app.UseRouting();
    app.UseAuthentication(); //<- Add this line
    app.UseAuthorization();
    ```


5.  **Optional:** By default .NET Core assumes all of the controllers should be accessible anonymously. Let's change that, and enable authorisation on ALL pages by default. This means as soon as a user attempts to visit any page in your web application, they will first need to authenticate before being permitted access to the app. To make this change, replace `#!c# app.MapRazorPages()` with `#!c# app.MapRazorPages().RequireAuthorization();` within file, `Startup.cs`. 

    When done, your `Startup.cs` file should look like this:
    ``` c#
    app.UseEndpoints(endpoints =>
    {
        //endpoints.MapRazorPages(); //<-- Comment this out
        endpoints.MapRazorPages().RequireAuthorization(); //<-- Add this line
    });
    ```
    With this added, this means all pages within your application require user authentication. Add the `#!c# [AllowAnonymous]` attribute above any method(s) or Controllers you want to allow anonymous access to. For more details, please see the [following Microsoft TechNet article](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/simple?view=aspnetcore-3.1).

6.  Run your web application using `dotnet run`. As soon as the application opens, you will immediately be redirected to IBM Security Verify to authenticate, then straight back to your web application. 


## Notes

### Where is the `GroupIds` user attribute?

ISV always assumes you want to protect end-user information. This is why, by default, ISV will not return all of the end-user's attributes (a.k.a. `claims`). There are a few key attributes that will be missing; for example, **Groups**. To control what attributes you want to share with your application, within the ISV Admin Portal go to `Applications` > Your App > `Sign-on` > `Attribute mappings`. You can specify each attribute you want to share wih the application. Or, for high-trust applications, select the `Send all known user attributes in the ID token` option which will include all of the end-user's attributes in ISV.

If you want to see what attributes are returned by default, add this to your `Index.cshtml` file:
``` c#
@{
    if(User.Identity != null)
    {
        @foreach (var claim in User.Claims)
        {
            <div><code>@claim.Type</code>: <strong>@claim.Value</strong></div>
        }
    }
}
```

### Troubleshooting common errors

Below is a list of commmon errors you could run into:


Error: **CSIAQ0167E**

`The redirection URI that is provided in the request is either invalid, or does not meet the matching criteria for the registered redirection URI.`

For security reasons, the URL to your web application must match one of the `Redirect URIs` defined in ISV for your OIDC web application. In our examples we assumed it would be `https://localhost:50001`. That is because when you run a .NET web application locally, .NET will almost always assign that URL to your web application. If, however, port `5001` is already being used, .NET will randomly assign a new port to your web application. If that occurs, you need to update the `Redirect URI` in the ISV Admin Portal or in the `IBM Security Verify Developer Portal`.

*[ISV]: IBM Security Verify
