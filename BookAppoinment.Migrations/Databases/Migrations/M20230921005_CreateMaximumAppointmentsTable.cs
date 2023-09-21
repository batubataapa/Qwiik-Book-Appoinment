using FluentMigrator;

namespace BookAppoinment.Migrations.Databases.Migrations;

[Migration(20230921005)]
public class M20230921005_CreateMaximumAppointmentsTable : Migration
{
    public override void Up()
    {
        Create.Table("MaximumAppointments")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("AgencyId").AsInt32().NotNullable()
            .WithColumn("AppointmentDate").AsDate().NotNullable()
            .WithColumn("MaximumAppointmentNumber").AsInt32().NotNullable()
            .WithColumn("MaximumAppointmentStatus").AsBoolean().NotNullable();
    }

    public override void Down()
    {
        // Let's keep the data just in case.
        Rename.Table("MaximumAppointments").To("MaximumAppointments_Old");
    }
}

