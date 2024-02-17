using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using TodoWebService;
using TodoWebService.Services.Todo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var sinkOptions = new MSSqlServerSinkOptions()
{
    TableName = "logs",
    AutoCreateSqlTable = true,
};

var colOptions = new ColumnOptions()
{
    AdditionalColumns = new Collection<SqlColumn>()
    {
        new SqlColumn()
        {
            ColumnName = "user_name",
            DataType = System.Data.SqlDbType.NVarChar,
        },
    }
};

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(
    builder.Configuration.GetConnectionString("TodoConStr"),
    sinkOptions: sinkOptions,
    columnOptions: colOptions
    )
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(log);

//builder.Services.AddRedis();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddDomainServices();

builder.Services.AddTodoContext(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated is not null || true ? context.User.Identity.Name : null;
    Console.WriteLine(username);
    LogContext.PushProperty("user_name", username?.ToString() ?? null);
    await next.Invoke();
});

app.MapControllers();

app.Run();
