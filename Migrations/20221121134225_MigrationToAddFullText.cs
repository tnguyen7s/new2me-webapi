using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new2meapi.Migrations
{
    /// <inheritdoc />
    public partial class MigrationToAddFullText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX post_search_index ON Posts(Title, Location, Description)"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
