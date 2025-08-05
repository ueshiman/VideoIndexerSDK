using AspireApp1.ServiceDefaults;
using BlazorAppWidgets.Components;
using BlazorAppWidgets.Components.Services;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessExtension.Service;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddVideoIndexerAccess();
builder.Services.AddScoped<VideoPlayerService>();

IAccounApitAccess accountApiAccess = builder.Services.BuildServiceProvider().GetRequiredService<IAccounApitAccess>();
accountApiAccess.GetAccountAsync()
    .GetAwaiter()
    .GetResult();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
