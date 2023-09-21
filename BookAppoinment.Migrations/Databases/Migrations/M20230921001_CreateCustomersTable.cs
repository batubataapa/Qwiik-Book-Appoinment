using FluentMigrator;

namespace BookAppoinment.Migrations.Databases.Migrations;

[Migration(20230921001)]
public class M20230921001_CreateCustomersTable : Migration
{
    public override void Up()
    {
        Create.Table("Customers")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString().NotNullable()
            .WithColumn("LastName").AsString().NotNullable()
            .WithColumn("Address").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable();
    }

    public override void Down()
    {
        // Let's keep the data just in case.
        Rename.Table("Customers").To("Customers_Old");
    }
}

