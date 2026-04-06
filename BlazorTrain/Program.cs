using BlazorTrain.Components;

using MudBlazor.Services;

using DiscordTrain.JMRIConnector.DependencyInjection;
using DiscordTrain.RPiConnector.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.local.json", true, true);

// Add MudBlazor services
builder.Services.AddMudServices();

//builder.Services.RegisterRPiConnector(builder.Configuration);
builder.Services.RegisterSimulatedRPiConnector(builder.Configuration);
builder.Services.RegisterJMRIConnector(builder.Configuration); 

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
