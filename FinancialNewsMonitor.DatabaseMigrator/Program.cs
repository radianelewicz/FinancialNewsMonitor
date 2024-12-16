using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

const string CONFIGURATION_FILENAME = "appsettings.json";
const string CONNECTIONSTRING_NAME = "FinancialConnectionString";

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile(CONFIGURATION_FILENAME)
    .Build();

var connectionString = configurationBuilder.GetConnectionString(CONNECTIONSTRING_NAME);

EnsureDatabase.For.SqlDatabase(connectionString);

var upgradeEngine = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .WithTransactionPerScript()
    .Build();

var result = upgradeEngine.PerformUpgrade();

if (result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.WriteLine("Failed!");
}

Console.ResetColor();