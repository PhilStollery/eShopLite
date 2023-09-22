using Microsoft.Extensions.Http.Resilience;
using Store.Components;
using Store.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductService>();
Console.WriteLine("**Setting up HTTPClient");
builder.Services.AddHttpClient<ProductService>(c =>
{
    var url = builder.Configuration["ProductEndpoint"] ?? throw new InvalidOperationException("ProductEndpoint is not set");

    c.BaseAddress = new(url);
})
.AddStandardResilienceHandler( options => 
{
    //Retry is working
    options.TotalRequestTimeoutOptions.Timeout = TimeSpan.FromMinutes(5);   
    options.RetryOptions.RetryCount = 10;
    options.RetryOptions.BackoffType = Polly.Retry.RetryBackoffType.Linear;
    options.RetryOptions.BaseDelay = TimeSpan.FromSeconds(1);

    // How do I switch on the circuit breaker?
    options.CircuitBreakerOptions.BreakDuration = TimeSpan.FromMinutes(1);
    options.CircuitBreakerOptions.FailureRatio = 0.9;
    options.CircuitBreakerOptions.OnOpened = async args => {
        Console.WriteLine("**Circuit open and stopping requests");
        await Task.CompletedTask;  
    };
    options.CircuitBreakerOptions.OnClosed = async args => {
        Console.WriteLine("**Circuit closed allowing requests");
        await Task.CompletedTask;  
    };
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents();

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

app.MapRazorComponents<App>()
    .AddServerRenderMode();

 app.Run();

