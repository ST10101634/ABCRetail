using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using Azure.Data.Tables;
using Azure.Storage.Blobs; // Ensure this namespace is included
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ABCRetail.Service;

var builder = WebApplication.CreateBuilder(args);

// Access Configuration from builder.
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Get the connection string from configuration
string azureStorageConnectionString = configuration.GetConnectionString("AzureStorageConnectionString");

// Register ShareClient with the connection string and share name
builder.Services.AddSingleton<ShareClient>(sp =>
    new ShareClient(azureStorageConnectionString, "Share"));

// Register QueueServiceClient
builder.Services.AddSingleton<QueueServiceClient>(sp =>
    new QueueServiceClient(azureStorageConnectionString));

// Register TableServiceClient
builder.Services.AddSingleton<TableServiceClient>(sp =>
    new TableServiceClient(azureStorageConnectionString));

// Register BlobServiceClient
builder.Services.AddSingleton<BlobServiceClient>(sp =>
    new BlobServiceClient(azureStorageConnectionString));

// Register FileStorageService with ShareClient
builder.Services.AddSingleton<FileStorageService>(sp =>
    new FileStorageService(sp.GetRequiredService<ShareClient>()));

// Register BlobStorageService with BlobServiceClient
builder.Services.AddSingleton<BlobStorageService>(sp =>
    new BlobStorageService(sp.GetRequiredService<BlobServiceClient>()));

// Register QueueStorageService with QueueServiceClient
builder.Services.AddSingleton<QueueStorageService>(sp =>
    new QueueStorageService(sp.GetRequiredService<QueueServiceClient>()));

// Register DataTable with TableServiceClient
builder.Services.AddSingleton<DataTable>(sp =>
    new DataTable(sp.GetRequiredService<TableServiceClient>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
