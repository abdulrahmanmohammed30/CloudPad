using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteTakingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ArabicName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alpha2Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Alpha3Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PhoneCode = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CountryId = table.Column<short>(type: "smallint", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.Sql("""
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Afghanistan', 'أفغانستان', 'AF', 'AFG', '93');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Åland Islands', 'جزر أولاند', 'AX', 'ALA', '358');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Albania', 'ألبانيا', 'AL', 'ALB', '355');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Algeria', 'الجزائر', 'DZ', 'DZA', '213');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('American Samoa', 'ساموا الأمريكية', 'AS', 'ASM', '684');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Andorra', 'أندورا', 'AD', 'AND', '376');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Angola', 'أنغولا', 'AO', 'AGO', '244');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Anguilla', 'أنغويلا', 'AI', 'AIA', '264');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Antarctica', 'القارة القطبية الجنوبية', 'AQ', 'ATA', '672');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Antigua and Barbuda', 'أنتيغوا وبربودا', 'AG', 'ATG', '268');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Argentina', 'الأرجنتين', 'AR', 'ARG', '54');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Armenia', 'أرمينيا', 'AM', 'ARM', '374');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Aruba', 'أروبا', 'AW', 'ABW', '297');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Australia', 'أستراليا', 'AU', 'AUS', '61');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Austria', 'النمسا', 'AT', 'AUT', '43');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Azerbaijan', 'أذربيجان', 'AZ', 'AZE', '994');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bahamas', 'باهاماس', 'BS', 'BHS', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bahrain', 'البحرين', 'BH', 'BHR', '973');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bangladesh', 'بنغلاديش', 'BD', 'BGD', '880');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Barbados', 'بربادوس', 'BB', 'BRB', '246');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Belarus', 'بيلاروسيا', 'BY', 'BLR', '375');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Belgium', 'بلجيكا', 'BE', 'BEL', '32');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Belize', 'بليز', 'BZ', 'BLZ', '501');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Benin', 'بنين', 'BJ', 'BEN', '229');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bermuda', 'برمودا', 'BM', 'BMU', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bhutan', 'بوتان', 'BT', 'BTN', '975');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bolivia', 'بوليفيا', 'BO', 'BOL', '591');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bosnia and Herzegovina', 'البوسنة والهرسك', 'BA', 'BIH', '387');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Botswana', 'بوتسوانا', 'BW', 'BWA', '267');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bouvet Island', 'جزيرة بوفيه', 'BV', 'BVT', '47');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Brazil', 'البرازيل', 'BR', 'BRA', '55');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('British Indian Ocean Territory', 'إقليم المحيط الهندي البريطاني', 'IO', 'IOT', '246');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('British Virgin Islands', 'جزر العذراء البريطانية', 'VG', 'VGB', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Brunei', 'بروناي', 'BN', 'BRN', '673');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Bulgaria', 'بلغاريا', 'BG', 'BGR', '359');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Burkina Faso', 'بوركينا فاسو', 'BF', 'BFA', '226');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Burundi', 'بوروندي', 'BI', 'BDI', '257');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cabo Verde', 'كابو فيردي', 'CV', 'CPV', '238');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cambodia', 'كمبوديا', 'KH', 'KHM', '855');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cameroon', 'الكاميرون', 'CM', 'CMR', '237');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Canada', 'كندا', 'CA', 'CAN', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Caribbean Netherlands', 'الجزر الكاريبية الهولندية', 'BQ', 'BES', '599');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cayman Islands', 'جزر كايمان', 'KY', 'CYM', '1345');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Central African Republic', 'جمهورية أفريقيا الوسطى', 'CF', 'CAF', '236');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Chad', 'تشاد', 'TD', 'TCD', '235');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Chile', 'تشيلي', 'CL', 'CHL', '56');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('China', 'الصين', 'CN', 'CHN', '86');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Christmas Island', 'جزيرة الكريسماس', 'CX', 'CXR', '61');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cocos (Keeling) Islands', 'جزر كوكوس', 'CC', 'CCK', '61');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Colombia', 'كولومبيا', 'CO', 'COL', '57');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Comoros', 'جزر القمر', 'KM', 'COM', '269');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Congo Republic', 'جمهورية الكونغو', 'CG', 'COG', '242');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cook Islands', 'جزر كوك', 'CK', 'COK', '682');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Costa Rica', 'كوستاريكا', 'CR', 'CRI', '506');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Croatia', 'كرواتيا', 'HR', 'HRV', '385');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cuba', 'كوبا', 'CU', 'CUB', '53');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Curaçao', 'كوراساو', 'CW', 'CUW', '599');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Cyprus', 'قبرص', 'CY', 'CYP', '357');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Czechia', 'التشيك', 'CZ', 'CZE', '420');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Denmark', 'الدنمارك', 'DK', 'DNK', '45');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Djibouti', 'جيبوتي', 'DJ', 'DJI', '253');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Dominica', 'دومينيكا', 'DM', 'DMA', '767');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Dominican Republic', 'جمهورية الدومينيكان', 'DO', 'DOM', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('DR Congo', 'جمهورية الكونغو الديمقراطية', 'CD', 'COD', '243');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Ecuador', 'الاكوادور', 'EC', 'ECU', '593');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Egypt', 'مصر', 'EG', 'EGY', '20');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('El Salvador', 'السلفادور', 'SV', 'SLV', '503');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Equatorial Guinea', 'غينيا الاستوائية', 'GQ', 'GNQ', '240');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Eritrea', 'إريتريا', 'ER', 'ERI', '291');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Estonia', 'إستونيا', 'EE', 'EST', '372');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Eswatini', 'إسواتيني', 'SZ', 'SWZ', '268');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Ethiopia', 'إثيوبيا', 'ET', 'ETH', '251');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Falkland Islands', 'جزر فوكلاند', 'FK', 'FLK', '500');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Faroe Islands', 'جزر فارو', 'FO', 'FRO', '298');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Fiji', 'فيجي', 'FJ', 'FJI', '679');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Finland', 'فنلندا', 'FI', 'FIN', '358');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('France', 'فرنسا', 'FR', 'FRA', '33');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('French Guiana', 'غويانا الفرنسية', 'GF', 'GUF', '594');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('French Polynesia', 'بولينزيا الفرنسية', 'PF', 'PYF', '689');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('French Southern Territories', 'أراض فرنسية جنوبية', 'TF', 'ATF', '262');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Gabon', 'الجابون', 'GA', 'GAB', '241');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Gambia', 'غامبيا', 'GM', 'GMB', '220');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Georgia', '‫جورجيا', 'GE', 'GEO', '995');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Germany', 'ألمانيا', 'DE', 'DEU', '49');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Ghana', 'غانا', 'GH', 'GHA', '233');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Gibraltar', 'جبل طارق', 'GI', 'GIB', '350');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Greece', 'اليونان', 'GR', 'GRC', '30');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Greenland', 'جرينلاند', 'GL', 'GRL', '299');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Grenada', 'غرينادا', 'GD', 'GRD', '473');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guadeloupe', 'غوادلوب', 'GP', 'GLP', '590');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guam', 'غوام', 'GU', 'GUM', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guatemala', 'غواتيمالا', 'GT', 'GTM', '502');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guernsey', 'غيرنزي', 'GG', 'GGY', '44');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guinea', 'غينيا', 'GN', 'GIN', '224');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guinea-Bissau', 'غينيا بيساو', 'GW', 'GNB', '245');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Guyana', 'غيانا', 'GY', 'GUY', '592');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Haiti', 'هايتي', 'HT', 'HTI', '509');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Heard Island and McDonald Islands', 'جزيرة هيرد وجزر ماكدونالد', 'HM', 'HMD', '672');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Honduras', 'هندوراس', 'HN', 'HND', '504');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Hong Kong', 'هونج كونج', 'HK', 'HKG', '852');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Hungary', 'هنجاريا', 'HU', 'HUN', '36');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Iceland', 'آيسلندا', 'IS', 'ISL', '354');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('India', 'الهند', 'IN', 'IND', '91');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Indonesia', 'أندونيسيا', 'ID', 'IDN', '62');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Iran', 'إيران', 'IR', 'IRN', '98');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Iraq', 'العراق', 'IQ', 'IRQ', '964');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Ireland', 'أيرلندا', 'IE', 'IRL', '353');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Isle of Man', 'جزيرة مان', 'IM', 'IMN', '44');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Italy', 'إيطاليا', 'IT', 'ITA', '390');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Ivory Coast', 'ساحل العاج', 'CI', 'CIV', '225');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Jamaica', 'جامايكا', 'JM', 'JAM', '1876');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Japan', 'اليابان', 'JP', 'JPN', '81');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Jersey', 'جيرسي', 'JE', 'JEY', '44');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Jordan', 'الأردن', 'JO', 'JOR', '962');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Kazakhstan', 'كازاخستان', 'KZ', 'KAZ', '7');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Kenya', 'كينيا', 'KE', 'KEN', '254');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Kiribati', 'كيريباتي', 'KI', 'KIR', '686');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Kosovo', 'كوسوفو', 'XK', 'XKX', '383');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Kuwait', 'الكويت', 'KW', 'KWT', '965');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Kyrgyzstan', 'قيرغيزستان', 'KG', 'KGZ', '996');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Laos', 'لاوس', 'LA', 'LAO', '856');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Latvia', 'لاتفيا', 'LV', 'LVA', '371');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Lebanon', 'لبنان', 'LB', 'LBN', '961');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Lesotho', 'ليسوتو', 'LS', 'LSO', '266');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Liberia', 'ليبيريا', 'LR', 'LBR', '231');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Libya', 'ليبيا', 'LY', 'LBY', '218');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Liechtenstein', 'ليختنشتاين', 'LI', 'LIE', '423');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Lithuania', 'ليتوانيا', 'LT', 'LTU', '370');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Luxembourg', 'لوكسمبورغ', 'LU', 'LUX', '352');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Macao', 'ماكاو', 'MO', 'MAC', '853');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Madagascar', 'مدغشقر', 'MG', 'MDG', '261');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Malawi', 'مالاوي', 'MW', 'MWI', '265');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Malaysia', 'ماليزيا', 'MY', 'MYS', '60');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Maldives', 'المالديف', 'MV', 'MDV', '960');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mali', 'مالي', 'ML', 'MLI', '223');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Malta', 'مالطا', 'MT', 'MLT', '356');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Marshall Islands', 'جزر مارشال', 'MH', 'MHL', '692');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Martinique', 'مارتينيك', 'MQ', 'MTQ', '33');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mauritania', 'موريتانيا', 'MR', 'MRT', '222');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mauritius', 'موريشيوس', 'MU', 'MUS', '230');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mayotte', 'مايوت', 'YT', 'MYT', '262');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mexico', 'المكسيك', 'MX', 'MEX', '52');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Micronesia', 'ولايات ميكرونيسيا المتحدة', 'FM', 'FSM', '691');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Moldova', 'مولدوفا', 'MD', 'MDA', '373');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Monaco', 'موناكو', 'MC', 'MCO', '377');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mongolia', 'منغوليا', 'MN', 'MNG', '976');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Montenegro', 'مونتينيغرو', 'ME', 'MNE', '382');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Montserrat', 'مونتسرات', 'MS', 'MSR', '1664');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Morocco', 'المغرب', 'MA', 'MAR', '212');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Mozambique', 'موزمبيق', 'MZ', 'MOZ', '258');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Myanmar', 'ميانمار', 'MM', 'MMR', '95');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Namibia', 'ناميبيا', 'NA', 'NAM', '264');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Nauru', 'ناورو', 'NR', 'NRU', '674');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Nepal', 'نيبال', 'NP', 'NPL', '977');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Netherlands', 'هولندا', 'NL', 'NLD', '31');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Netherlands Antilles', 'جزر الأنتيل الهولندية', 'AN', 'ANT', '599');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('New Caledonia', 'كاليدونيا الجديدة', 'NC', 'NCL', '687');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('New Zealand', 'نيوزيلندا', 'NZ', 'NZL', '64');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Nicaragua', 'نيكاراغوا', 'NI', 'NIC', '505');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Niger', 'النيجر', 'NE', 'NER', '227');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Nigeria', 'نيجيريا', 'NG', 'NGA', '234');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Niue', 'نييوي', 'NU', 'NIU', '683');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Norfolk Island', 'جزيرة نورفولك', 'NF', 'NFK', '6723');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('North Korea', 'كوريا الشمالية', 'KP', 'PRK', '850');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('North Macedonia', 'مقدونيا الشمالية', 'MK', 'MKD', '389');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Northern Mariana Islands', 'جزر ماريانا الشمالية', 'MP', 'MNP', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Norway', 'النرويج', 'NO', 'NOR', '47');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Oman', 'سلطنة عمان', 'OM', 'OMN', '968');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Pakistan', 'باكستان', 'PK', 'PAK', '92');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Palau', 'بالاو', 'PW', 'PLW', '680');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Palestine', 'فلسطين', 'PS', 'PSE', '970');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Panama', 'بنما', 'PA', 'PAN', '507');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Papua New Guinea', 'بابوا غينيا الجديدة', 'PG', 'PNG', '675');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Paraguay', 'باراغواي', 'PY', 'PRY', '595');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Peru', 'بيرو', 'PE', 'PER', '51');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Philippines', 'الفلبين', 'PH', 'PHL', '63');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Pitcairn Islands', 'جزر بيتكيرن', 'PN', 'PCN', '672');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Poland', 'بولندا', 'PL', 'POL', '48');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Portugal', 'البرتغال', 'PT', 'PRT', '351');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Puerto Rico', 'بورتوريكو', 'PR', 'PRI', '1787');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Qatar', 'قطر', 'QA', 'QAT', '974');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Réunion', 'ريونيون', 'RE', 'REU', '262');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Romania', 'رومانيا', 'RO', 'ROU', '40');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Russia', 'روسيا', 'RU', 'RUS', '7');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Rwanda', 'رواندا', 'RW', 'RWA', '250');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Barthélemy', 'سان بارتيلمي', 'BL', 'BLM', '590');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Helena', 'سانت هيلينا', 'SH', 'SHN', '290');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Kitts and Nevis', 'سانت كيتس ونيفيس', 'KN', 'KNA', '1869');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Lucia', 'سانت لوسيا', 'LC', 'LCA', '1758');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Martin', 'سانت مارتن', 'MF', 'MAF', '590');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Pierre and Miquelon', 'سان بيير وميكلون', 'PM', 'SPM', '508');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saint Vincent and the Grenadines', 'سانت فينسنت والغرينادين', 'VC', 'VCT', '784');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Samoa', 'ساموا', 'WS', 'WSM', '685');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('San Marino', 'سان مارينو', 'SM', 'SMR', '378');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('São Tomé and Príncipe', 'ساو تومي وبرينسيب', 'ST', 'STP', '239');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Saudi Arabia', 'السعودية', 'SA', 'SAU', '966');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Senegal', 'السنغال', 'SN', 'SEN', '221');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Serbia', 'صربيا', 'RS', 'SRB', '381');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Seychelles', 'سيشل', 'SC', 'SYC', '248');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Sierra Leone', 'سيراليون', 'SL', 'SLE', '232');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Singapore', 'سنغافورة', 'SG', 'SGP', '65');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Sint Maarten', 'سانت مارتن', 'SX', 'SXM', '599');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Slovakia', 'سلوفاكيا', 'SK', 'SVK', '421');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Slovenia', 'سلوفينيا', 'SI', 'SVN', '386');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Solomon Islands', 'جزر سليمان', 'SB', 'SLB', '677');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Somalia', 'الصومال', 'SO', 'SOM', '252');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('South Africa', 'جنوب أفريقيا', 'ZA', 'ZAF', '27');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('South Georgia and South Sandwich Islands', 'جورجيا الجنوبية وجزر ساندويتش الجنوبية', 'GS', 'SGS', '500');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('South Korea', 'كوريا الجنوبية', 'KR', 'KOR', '82');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('South Sudan', 'جنوب السودان', 'SS', 'SSD', '211');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Spain', 'إسبانيا', 'ES', 'ESP', '34');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Sri Lanka', 'سريلانكا', 'LK', 'LKA', '94');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Sudan', 'السودان', 'SD', 'SDN', '249');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Suriname', 'سورينام', 'SR', 'SUR', '597');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Svalbard and Jan Mayen', 'سفالبارد ويان ماين', 'SJ', 'SJM', '47');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Sweden', 'السويد', 'SE', 'SWE', '46');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Switzerland', 'سويسرا', 'CH', 'CHE', '41');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Syria', 'سوريا', 'SY', 'SYR', '963');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Taiwan', 'تايوان', 'TW', 'TWN', '886');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Tajikistan', 'طاجيكستان', 'TJ', 'TJK', '992');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Tanzania', 'تنزانيا', 'TZ', 'TZA', '255');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Thailand', 'تايلاند', 'TH', 'THA', '66');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Timor-Leste', 'تيمور الشرقية', 'TL', 'TLS', '670');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Togo', 'توجو', 'TG', 'TGO', '228');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Tokelau', 'توكيلاو', 'TK', 'TKL', '690');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Tonga', 'تونغا', 'TO', 'TON', '676');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Trinidad and Tobago', 'ترينيداد وتوباغو', 'TT', 'TTO', '868');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Tunisia', 'تونس', 'TN', 'TUN', '216');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Turkey', 'تركيا', 'TR', 'TUR', '90');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Turkmenistan', 'تركمانستان', 'TM', 'TKM', '993');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Turks and Caicos Islands', 'جزر توركس وكايكوس', 'TC', 'TCA', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Tuvalu', 'توفالو', 'TV', 'TUV', '688');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('U.S. Minor Outlying Islands', 'جزر الولايات المتحدة الصغيرة النائية', 'UM', 'UMI', '246');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('U.S. Virgin Islands', 'جزر العذراء الأمريكية', 'VI', 'VIR', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Uganda', 'أوغندا', 'UG', 'UGA', '256');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Ukraine', 'أوكرانيا', 'UA', 'UKR', '380');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('United Arab Emirates', 'الإمارات العربية المتحدة', 'AE', 'ARE', '971');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('United Kingdom', 'المملكة المتحدة', 'GB', 'GBR', '44');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('United States of America', 'الولايات المتحدة الأمريكية', 'US', 'USA', '1');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Uruguay', 'أوروغواي', 'UY', 'URY', '598');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Uzbekistan', 'أوزبكستان', 'UZ', 'UZB', '998');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Vanuatu', 'فانواتو', 'VU', 'VUT', '678');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Vatican City', 'مدينة الفاتيكان', 'VA', 'VAT', '379');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Venezuela', 'فنزويلا', 'VE', 'VEN', '58');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Vietnam', 'فيتنام', 'VN', 'VNM', '84');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Wallis and Futuna', 'واليس وفوتونا', 'WF', 'WLF', '681');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Western Sahara', 'الصحراء الغربية', 'EH', 'ESH', '212');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Yemen', 'اليمن', 'YE', 'YEM', '967');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Zambia', 'زامبيا', 'ZM', 'ZMB', '260');
                                 INSERT INTO COUNTRIES (ENGLISHNAME, ArabicName, Alpha2Code, Alpha3Code, PhoneCode) VALUES('Zimbabwe', 'زيمبابوي', 'ZW', 'ZWE', '263');	
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
