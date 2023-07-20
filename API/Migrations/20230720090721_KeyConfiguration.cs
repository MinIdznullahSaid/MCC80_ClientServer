using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class KeyConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_booking_employee_guid",
                table: "tb_tr_booking",
                column: "employee_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_booking_room_guid",
                table: "tb_tr_booking",
                column: "room_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_account_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_educations_university_guid",
                table: "tb_m_educations",
                column: "university_guid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_account_roles_tb_m_employees_guid",
                table: "tb_m_account_roles",
                column: "guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_educations_tb_m_employees_guid",
                table: "tb_m_educations",
                column: "guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_educations_tb_m_universities_university_guid",
                table: "tb_m_educations",
                column: "university_guid",
                principalTable: "tb_m_universities",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_account_roles_account_guid",
                table: "tb_tr_account_roles",
                column: "account_guid",
                principalTable: "tb_m_account_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                table: "tb_tr_account_roles",
                column: "role_guid",
                principalTable: "tb_m_roles",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_booking_tb_m_employees_employee_guid",
                table: "tb_tr_booking",
                column: "employee_guid",
                principalTable: "tb_m_employees",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_booking_tb_m_rooms_room_guid",
                table: "tb_tr_booking",
                column: "room_guid",
                principalTable: "tb_m_rooms",
                principalColumn: "guid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_account_roles_tb_m_employees_guid",
                table: "tb_m_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_educations_tb_m_employees_guid",
                table: "tb_m_educations");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_educations_tb_m_universities_university_guid",
                table: "tb_m_educations");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_account_roles_account_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_account_roles_tb_m_roles_role_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_booking_tb_m_employees_employee_guid",
                table: "tb_tr_booking");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_tr_booking_tb_m_rooms_room_guid",
                table: "tb_tr_booking");

            migrationBuilder.DropIndex(
                name: "IX_tb_tr_booking_employee_guid",
                table: "tb_tr_booking");

            migrationBuilder.DropIndex(
                name: "IX_tb_tr_booking_room_guid",
                table: "tb_tr_booking");

            migrationBuilder.DropIndex(
                name: "IX_tb_tr_account_roles_account_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropIndex(
                name: "IX_tb_tr_account_roles_role_guid",
                table: "tb_tr_account_roles");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_educations_university_guid",
                table: "tb_m_educations");
        }
    }
}
