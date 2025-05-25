# IpShieldMiddleware

`IpShieldMiddleware` is a lightweight ASP.NET Core middleware for IP-based request filtering. It allows whitelisting or blacklisting specific IP addresses with customizable error messages and logging.

This middleware intercepts incoming HTTP requests and performs IP filtering based on the configured whitelist and blacklist IP's.  

-Requests from whitelisted IPs are allowed.  
-Requests from blacklisted IPs are blocked, returning a response of 403 (Forbidden) with the default message in case no custom message was provided.

## 🔧 Features

- ✅ Whitelist specific IP addresses
- 🚫 Blacklist unwanted IP addresses
- ✍️ Optional custom error messages with IP placeholder (`{IP}`) for both response and logging (if not provided a default message will be used)
- 📝 Built-in logging support
- 📦 Plug-and-play for ASP.NET Core projects

---

## 📦 Installation

dotnet add package IpShieldMiddleware

## 🚀 Getting Started

```csharp
builder.Services.Configure<IpShieldOptions>(options =>
{
    options.WhitelistedIps = new List<string> { "127.0.0.1", "::1" };
    options.BlacklistedIps = new List<string> { "192.168.1.100" };
    options.DeniedMessageTemplate = "Access denied: '{ip}' is not allowed.";
    options.LogMessageTemplate = "Blocked request from IP: {ip}";
});

var app = builder.Build();
app.UseIpShield();
```

or if you want to keep configs in appsettings

```appsettings
"IpShieldOptions": {
  "BlacklistedIps": [ "192.168.1.10" ],
  "WhitelistedIps": [ "127.0.0.1", "localhost", "::1" ],
  "CustomLogMessage": "This is a custom log message to log for ip: {IP}",
  "CustomErrorMessage": "This is a custom response message to log for ip: {IP}"
}
```

then in your program.cs 
```csharp
builder.Services.Configure<IpShieldOptions>(builder.Configuration.GetSection("IpShieldOptions"));

var app = builder.Build();
app.UseIpShield();
```


This middleware intercepts incoming HTTP requests and performs IP filtering based on the configured whitelist and blacklist:

-Requests from whitelisted IPs are allowed.

-Requests from blacklisted IPs are blocked, returning a response of 403 (Forbidden) with the default message in case no custom message was provided.