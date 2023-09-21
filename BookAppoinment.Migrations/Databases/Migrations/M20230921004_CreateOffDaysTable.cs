using FluentMigrator;

namespace BookAppoinment.Migrations.Databases.Migrations;

[Migration(20230921004)]
public class M20230921004_CreateOffDaysTable : Migration
{
    public override void Up()
    {
        Create.Table("OffDays")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("AgencyId").AsInt32().NotNullable()
            .WithColumn("AppointmentDate").AsDate().NotNullable()
            .WithColumn("OffDayStatus").AsBoolean().NotNullable();
    }

    public override void Down()
    {
        // Let's keep the data just in case.
        Rename.Table("OffDays").To("OffDays_Old");
    }
}

