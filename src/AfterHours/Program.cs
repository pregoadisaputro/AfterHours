using AfterHours.Components;
using AfterHours.Data;
using AfterHours.Features.Media;
using AfterHours.Services.Tmdb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddMemoryCache();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddTmdbClient(builder.Configuration);
builder.Services.AddMediaFeature();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
