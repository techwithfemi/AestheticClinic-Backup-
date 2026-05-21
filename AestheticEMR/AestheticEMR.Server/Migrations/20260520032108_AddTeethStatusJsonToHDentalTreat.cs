using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticEMR.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTeethStatusJsonToHDentalTreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "teethStatusJson",
                table: "hDentalTreat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "orthodonticsJson",
                table: "hDentalTreat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE h
                SET teethStatusJson = s.JsonText
                FROM hDentalTreat h
                OUTER APPLY (
                    SELECT CASE WHEN COUNT(1) = 0 THEN NULL ELSE '{' + STRING_AGG('"' + v.Tooth + '":{"present":true,"missing":false}', ',') + '}' END AS JsonText
                    FROM (VALUES
                        ('18', h.AURM3), ('17', h.AURM2), ('16', h.AURM1), ('15', h.AURPM2), ('14', h.AURPM1), ('13', h.AURC), ('12', h.AURI2), ('11', h.AURI1),
                        ('21', h.AULI1), ('22', h.AULI2), ('23', h.AULC), ('24', h.AULPM1), ('25', h.AULPM2), ('26', h.AULM1), ('27', h.AULM2), ('28', h.AULM3),
                        ('48', h.ALRM3), ('47', h.ALRM2), ('46', h.ALRM1), ('45', h.ALRPM2), ('44', h.ALRPM1), ('43', h.ALRC), ('42', h.ALRI2), ('41', h.ALRI1),
                        ('31', h.ALLI1), ('32', h.ALLI2), ('33', h.ALLC), ('34', h.ALLPM1), ('35', h.ALLPM2), ('36', h.ALLM1), ('37', h.ALLM2), ('38', h.ALLM3)
                    ) v(Tooth, Flag)
                    WHERE v.Flag = 1
                ) s
                WHERE h.teethStatusJson IS NULL
                  AND LOWER(LTRIM(RTRIM(ISNULL(h.dtype, '')))) = 'teeth present';

                UPDATE h
                SET teethStatusJson = s.JsonText
                FROM hDentalTreat h
                OUTER APPLY (
                    SELECT CASE WHEN COUNT(1) = 0 THEN NULL ELSE '{' + STRING_AGG('"' + v.Tooth + '":{"carious":true}', ',') + '}' END AS JsonText
                    FROM (VALUES
                        ('18', h.AURM3), ('17', h.AURM2), ('16', h.AURM1), ('15', h.AURPM2), ('14', h.AURPM1), ('13', h.AURC), ('12', h.AURI2), ('11', h.AURI1),
                        ('21', h.AULI1), ('22', h.AULI2), ('23', h.AULC), ('24', h.AULPM1), ('25', h.AULPM2), ('26', h.AULM1), ('27', h.AULM2), ('28', h.AULM3),
                        ('48', h.ALRM3), ('47', h.ALRM2), ('46', h.ALRM1), ('45', h.ALRPM2), ('44', h.ALRPM1), ('43', h.ALRC), ('42', h.ALRI2), ('41', h.ALRI1),
                        ('31', h.ALLI1), ('32', h.ALLI2), ('33', h.ALLC), ('34', h.ALLPM1), ('35', h.ALLPM2), ('36', h.ALLM1), ('37', h.ALLM2), ('38', h.ALLM3)
                    ) v(Tooth, Flag)
                    WHERE v.Flag = 1
                ) s
                WHERE h.teethStatusJson IS NULL
                  AND LOWER(LTRIM(RTRIM(ISNULL(h.dtype, '')))) = 'carious teeth';

                UPDATE h
                SET teethStatusJson = s.JsonText
                FROM hDentalTreat h
                OUTER APPLY (
                    SELECT CASE WHEN COUNT(1) = 0 THEN NULL ELSE '{' + STRING_AGG('"' + v.Tooth + '":{"missing":true,"present":false}', ',') + '}' END AS JsonText
                    FROM (VALUES
                        ('18', h.AURM3), ('17', h.AURM2), ('16', h.AURM1), ('15', h.AURPM2), ('14', h.AURPM1), ('13', h.AURC), ('12', h.AURI2), ('11', h.AURI1),
                        ('21', h.AULI1), ('22', h.AULI2), ('23', h.AULC), ('24', h.AULPM1), ('25', h.AULPM2), ('26', h.AULM1), ('27', h.AULM2), ('28', h.AULM3),
                        ('48', h.ALRM3), ('47', h.ALRM2), ('46', h.ALRM1), ('45', h.ALRPM2), ('44', h.ALRPM1), ('43', h.ALRC), ('42', h.ALRI2), ('41', h.ALRI1),
                        ('31', h.ALLI1), ('32', h.ALLI2), ('33', h.ALLC), ('34', h.ALLPM1), ('35', h.ALLPM2), ('36', h.ALLM1), ('37', h.ALLM2), ('38', h.ALLM3)
                    ) v(Tooth, Flag)
                    WHERE v.Flag = 1
                ) s
                WHERE h.teethStatusJson IS NULL
                  AND LOWER(LTRIM(RTRIM(ISNULL(h.dtype, '')))) = 'missing teeth';

                UPDATE h
                SET teethStatusJson = s.JsonText
                FROM hDentalTreat h
                OUTER APPLY (
                    SELECT CASE WHEN COUNT(1) = 0 THEN NULL ELSE '{' + STRING_AGG('"' + v.Tooth + '":{"filled":true}', ',') + '}' END AS JsonText
                    FROM (VALUES
                        ('18', h.AURM3), ('17', h.AURM2), ('16', h.AURM1), ('15', h.AURPM2), ('14', h.AURPM1), ('13', h.AURC), ('12', h.AURI2), ('11', h.AURI1),
                        ('21', h.AULI1), ('22', h.AULI2), ('23', h.AULC), ('24', h.AULPM1), ('25', h.AULPM2), ('26', h.AULM1), ('27', h.AULM2), ('28', h.AULM3),
                        ('48', h.ALRM3), ('47', h.ALRM2), ('46', h.ALRM1), ('45', h.ALRPM2), ('44', h.ALRPM1), ('43', h.ALRC), ('42', h.ALRI2), ('41', h.ALRI1),
                        ('31', h.ALLI1), ('32', h.ALLI2), ('33', h.ALLC), ('34', h.ALLPM1), ('35', h.ALLPM2), ('36', h.ALLM1), ('37', h.ALLM2), ('38', h.ALLM3)
                    ) v(Tooth, Flag)
                    WHERE v.Flag = 1
                ) s
                WHERE h.teethStatusJson IS NULL
                  AND LOWER(LTRIM(RTRIM(ISNULL(h.dtype, '')))) = 'filled teeth';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orthodonticsJson",
                table: "hDentalTreat");

            migrationBuilder.DropColumn(
                name: "teethStatusJson",
                table: "hDentalTreat");
        }
    }
}
