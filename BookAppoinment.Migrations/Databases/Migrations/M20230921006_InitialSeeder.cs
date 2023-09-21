using System;
using FluentMigrator;

namespace BookAppoinment.Migrations.Databases.Migrations;

[Migration(20230921006)]
public class M20230921006_InitialSeeder : Migration
{
    public override void Up()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
        {
            Execute.EmbeddedScript("BookAppoinment.Migrations.Databases.Seeder.M20230921006_InitialSeeder.sql");
        }
    }

    public override void Down()
    {
        //empty, not using
    }
}