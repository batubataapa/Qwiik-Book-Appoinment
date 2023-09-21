using FluentMigrator;

namespace BookAppoinment.Migrations.Databases.Migrations;

[Migration(20230921002)]
public class M20230921002_CreateAgenciesTable : Migration
{
    public override void Up()
    {
        Create.Table("Agencies")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Address").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable();
    }

    public override void Down()
    {
        // Let's keep the data just in case.
        Rename.Table("Agencies").To("Agencies_Old");
    }
}

