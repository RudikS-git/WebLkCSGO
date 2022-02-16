using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public class TriggerLastChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER `UserLastChangedInsert` BEFORE INSERT ON `User`
                                   FOR EACH ROW SET New.LastChanged = 	CURRENT_TIMESTAMP() + INTERVAL 8 HOUR");
            
            migrationBuilder.Sql(@"CREATE TRIGGER `UserLastChangedUpdate` BEFORE UPDATE ON `User`
                                   FOR EACH ROW SET New.LastChanged = CURRENT_TIMESTAMP() + INTERVAL 8 HOUR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
