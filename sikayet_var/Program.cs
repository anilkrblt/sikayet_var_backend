using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using sikayet_var.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;
var builder = WebApplication.CreateBuilder(args);



LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureCors();

builder.Services.ConfigureISSIntegration();

builder.Services.ConfigureLoggerService();

builder.Services.ConfigureRepositoryManager();

builder.Services.ConfigureServiceManager();

builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

// So to enable our custom responses from the actions,
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
.Services.BuildServiceProvider()
.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
.OfType<NewtonsoftJsonPatchInputFormatter>().First();

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    /*
    tells 
    the server that if the client tries to negotiate for the media type the 
    server doesn’t support, it should return the 406 Not Acceptable status 
    code.
    */
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
}).AddXmlDataContractSerializerFormatters()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();  //it also interferes with our error handler middleware. 
}
/*
    app.UseHsts() will add middleware for using HSTS, which adds the 
    Strict-Transport-Security header. 

*/

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();


/*
    app.UseStaticFiles() enables using static files for the request. If 
    we don’t set a path to the static files directory, it will use a wwwroot 
    folder in our project by default.
*/


app.UseStaticFiles();

/*
    app.UseForwardedHeaders() will forward proxy headers to the 
    current request. This will help us during application deployment.
*/

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();


app.Run();


