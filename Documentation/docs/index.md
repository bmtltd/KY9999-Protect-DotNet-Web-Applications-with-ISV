# An introduction to protecting your C# applications with IBM Security Verify

This how-to guide is meant to show simple examples to help you get started with protecting your web applications using IBM Security Verify (ISV). To the left you'll find guides for protecting MVC web applications using OIDC. You will also see instructions on how to protect your APIs using OAuth introspection.

All of our examples start from a blank slate, then at the end you have a working product. That said, it is easier, feel free to download copies of our code from the Github repo linked below.

[:material-github: Go to Github Repo](https://github.com/bmtltd/KY9999-Protect-DotNet-Web-Applications-with-ISV/){ .md-button .md-button--primary .bmt-btn-github }



## Why use IBM Security Verify?

Our blog post coverts this in greater detail ([click here](https://www.bmt.ky/protect-your-custom-web-application-with-ibm-security-verify/)), but just to summarise, if your development strategy still uses Windows Authentication, you need to start rethinking how you're developing. When you IBM Security Verify to protect your web applications you get things like conditional-access policies, behaviour-based/risk-based MFA, Access Review and other neat features not included with Windows Authentication, or even Azure AD. 

## What materials do I need to follow these examples?

While the principals are the same for other programming languages, all of the exampels here are focused on .NET. 

To follow these tutorials you will need:

-   `.NET Core 3.1+` or `.NET 6.0+`
-   An IBM Security Verify subscription (included with your MaaS360 subscription).
-   Visual Studio Core, or or the full version of Visual Studio.

Because their are subtle differences between the various versions of .NET, we've tried our best to include examples for each version where the code difference are pronoucned enough to merit a separate tutorial. If we left something out, it isn't that ISV doesn't support it. It's likely we haven't yet gotten around to writting an article to cover it. We're only human, after all.

## Simple examples to get you started

Remember, these are meant to be simple authentication/authorisation examples just to get you started. Once you've got the fundamentals down, you can easily expand upon these examples an include things like [Role-based authorisation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0), [Claims-based authorisation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-6.0), [Policy-based authorisation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0), or [Resource-based authorisation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?view=aspnetcore-6.0). Because those are .NET technologies not specific to ISV, we've stuck with simple authorisation examples in our tutorials. Just know they are all possible and compatabile with ISV.


## See mistakes/bugs in our examples?

If you notice an issue with our documentation, please let us know. We want to fix everything. Just raise an issue in Github, and we'll address it.