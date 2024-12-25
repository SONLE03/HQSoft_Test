using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HQSoft_EX01.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "BooksWithAuthors",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Bio", "Name" },
                values: new object[,]
                {
                    { 1, "A prolific author in programming.", "John Doe" },
                    { 2, "Specializes in fiction.", "Jane Smith" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorId", "Price", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, 1, 29.99m, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn C#" },
                    { 2, 1, 39.99m, new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mastering ASP.NET" },
                    { 3, 2, 19.99m, new DateTime(2023, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fictional Worlds" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetAuthorsPaged
                    @PageNumber INT,
                    @PageSize INT
                AS
                BEGIN
                    SELECT AuthorId, Name, Bio
                    FROM Authors
                    ORDER BY Name
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetAuthorById
                    @AuthorId INT
                AS
                BEGIN
                    SELECT AuthorId, Name, Bio
                    FROM Authors
                    WHERE AuthorId = @AuthorId;
                END
            ");

            migrationBuilder.Sql(@"
               CREATE PROCEDURE GetAllBooksPaged
                    @PageNumber INT,
                    @PageSize INT
                AS
                BEGIN
                    SELECT 
                        b.AuthorId, 
                        b.Name, 
                        b.Bio,
                        a.BookId,
                        a.Title,
                        a.Price,
                        a.PublishedDate
                    FROM Books a
                    INNER JOIN Authors b ON a.AuthorId = b.AuthorId
                    ORDER BY a.Title
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetBookById
                    @BookId INT
                AS
                BEGIN
                    SELECT 
                        b.AuthorId, 
                        b.Name, 
                        b.Bio,
                        a.BookId,
                        a.Title,
                        a.Price,
                        a.PublishedDate
                    FROM Books a
                    INNER JOIN Authors b ON a.AuthorId = b.AuthorId
                    WHERE a.BookId = @BookId;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetBooksByFilters
                    @SearchKey NVARCHAR(100) = NULL,  -- Search key for the book title or description
                    @AuthorId INT = NULL,             -- Author ID filter
                    @FromPublishedDate DATETIME = NULL, -- Start date of publication
                    @ToPublishedDate DATETIME = NULL,   -- End date of publication
                    @PageSize INT = 10,               -- Number of records per page
                    @PageIndex INT = 1                -- Page index (1-based)
                AS
                BEGIN
                    -- Declare variables for pagination
                    DECLARE @Offset INT = (@PageIndex - 1) * @PageSize;

                    -- Select books with applied filters
                    SELECT 
                        b.BookId,
                        b.Title,
                        b.Price,
                        b.PublishedDate,
                        a.AuthorId,
                        a.Name,
                        a.Bio
                    FROM Books b
                    INNER JOIN Authors a ON b.AuthorId = a.AuthorId
                    WHERE 
                        (@SearchKey IS NULL OR b.Title LIKE '%' + @SearchKey + '%')  -- Search by book title
                        AND (@AuthorId IS NULL OR b.AuthorId = @AuthorId)               -- Filter by author
                        AND (@FromPublishedDate IS NULL OR b.PublishedDate >= @FromPublishedDate) -- Filter by start date
                        AND (@ToPublishedDate IS NULL OR b.PublishedDate <= @ToPublishedDate)     -- Filter by end date
                    ORDER BY b.Title
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                    -- You can also return total count if needed for pagination info
                    SELECT COUNT(*) AS TotalCount
                    FROM Books b
                    WHERE 
                        (@SearchKey IS NULL OR b.Title LIKE '%' + @SearchKey + '%')
                        AND (@AuthorId IS NULL OR b.AuthorId = @AuthorId)
                        AND (@FromPublishedDate IS NULL OR b.PublishedDate >= @FromPublishedDate)
                        AND (@ToPublishedDate IS NULL OR b.PublishedDate <= @ToPublishedDate);
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "BooksWithAuthors");

            migrationBuilder.DropTable(
                name: "Authors");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAuthorsPaged");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAuthorById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBooksPaged");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBookById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetBooksByFilters");

        }
    }
}
