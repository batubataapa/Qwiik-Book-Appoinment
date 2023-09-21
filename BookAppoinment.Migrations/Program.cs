namespace BookAppoinment.Migrations;


public static class Program
{
    public static void Main(string[] args)
    {
        var usage = @"Example usage:
            dotnet run up 
            dotnet run down [version]";

        try
        {
            var version = args[0] switch
            {
                "up" => 1,
                "down" => long.Parse(args[1]),
                _ => throw new Exception()
            };

            Databases.Database.RunMigrations(args[0], version);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.WriteLine("");
            Console.WriteLine(usage);
        }
    }
}