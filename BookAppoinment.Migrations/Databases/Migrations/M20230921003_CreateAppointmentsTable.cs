using FluentMigrator;

namespace BookAppoinment.Migrations.Databases.Migrations;

[Migration(20230921003)]
public class M20230921003_CreateAppointmentsTable : Migration
{
    public override void Up()
    {
        Create.Table("Appointments")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("CustomerId").AsInt32().NotNullable()
            .WithColumn("AgencyId").AsInt32().NotNullable()
            .WithColumn("Token").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn("AppointmentDate").AsDate().NotNullable();
    }

    public override void Down()
    {
        // Let's keep the data just in case.
        Rename.Table("Appointments").To("Appointments_Old");
    }
}

