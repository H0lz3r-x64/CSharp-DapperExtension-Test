using DapperExtension;
using System.Configuration;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Dapper;

Console.WriteLine("Runtime Connection String Test");

Console.Write("Username: ");
string user = Console.ReadLine() ?? "";

Console.Write("Password: ");
string pwd = Console.ReadLine() ?? "";

String conString = BuildConnectionString("Default",user, pwd);
MySqlDataAccess dataAccess = new(conString);
DataAccessHandler h = new(dataAccess);

List<IDictionary<string, object>> res = await h.sp_getAllAsync("sp_category_select_all");
foreach (var item in res)
{
    Console.WriteLine(item);
}
Console.ReadKey();


static string BuildConnectionString(string connStringKey, string userName, string userPassword)
{
    // Retrieve the partial connection string named databaseConnection
    // from the application's app.config or web.config file.
    ConnectionStringSettings settings =
        ConfigurationManager.ConnectionStrings[connStringKey];

    if (null != settings)
    {
        // Retrieve the partial connection string.
        string connectString = settings.ConnectionString;
        Console.WriteLine("Original: {0}", connectString);

        // Create a new SqlConnectionStringBuilder based on the
        // partial connection string retrieved from the config file.
        SqlConnectionStringBuilder builder =
            new SqlConnectionStringBuilder(connectString);

        // Supply the additional values.
        builder.UserID = userName;
        builder.Password = userPassword;
        Console.WriteLine("Modified: {0}", builder.ConnectionString);
        return builder.ConnectionString;
    }
    throw new KeyNotFoundException("Key \"" + connStringKey +"\" not found.");
}