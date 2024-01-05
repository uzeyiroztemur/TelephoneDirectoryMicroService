using Core.Utilities.Security.Hashing;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.DataContext.EntityFramework.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            HashingHelper.CreatePasswordHash("123456aA!", out byte[] passwordHash, out byte[] passwordSalt);

            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "FirstName", "LastName", "UserName", "PasswordSalt", "PasswordHash", "LastPasswordChangeDate", "AccessFailedCount", "LockoutEnabled", "IsActive", "CreatedOn", "CreatedBy", "IsDeleted" },  // Tablonuzun sütun adlarını buraya yazın
            values: new object[] { Guid.NewGuid(), "Üzeyir", "Öztemür", "uzeyiroztemur@gmail.com", passwordSalt, passwordHash, DateTime.UtcNow, 0, false, true, DateTime.UtcNow, Guid.Empty, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
