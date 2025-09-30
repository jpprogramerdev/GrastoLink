using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIGastroLink.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CATEGORIAS_PRATOS",
                columns: table => new
                {
                    CTP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CTP_CATEGORIA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIAS_PRATOS", x => x.CTP_ID);
                });

            migrationBuilder.CreateTable(
                name: "FORMAS_PAGAMENTO",
                columns: table => new
                {
                    FPG_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FPG_FORMA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORMAS_PAGAMENTO", x => x.FPG_ID);
                });

            migrationBuilder.CreateTable(
                name: "MESAS",
                columns: table => new
                {
                    MSA_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MSA_NUMERO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MSA_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MESAS", x => x.MSA_ID);
                });

            migrationBuilder.CreateTable(
                name: "TIPOS_USUARIOS",
                columns: table => new
                {
                    TPU_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TPU_TIPO = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPOS_USUARIOS", x => x.TPU_ID);
                });

            migrationBuilder.CreateTable(
                name: "PRATOS",
                columns: table => new
                {
                    PRA_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRA_NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRA_DESCRICAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRA_PRECO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PRA_TEMPO_MEDIO_PREPARO = table.Column<double>(type: "float", nullable: false),
                    PRA_DISPONIVEL = table.Column<bool>(type: "bit", nullable: false),
                    PRA_CTP_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRATOS", x => x.PRA_ID);
                    table.ForeignKey(
                        name: "FK_PRATOS_CATEGORIAS_PRATOS_PRA_CTP_ID",
                        column: x => x.PRA_CTP_ID,
                        principalTable: "CATEGORIAS_PRATOS",
                        principalColumn: "CTP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    USU_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USU_NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USU_CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USU_SENHA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USU_ATIVO = table.Column<bool>(type: "bit", nullable: false),
                    USU_TPU_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.USU_ID);
                    table.ForeignKey(
                        name: "FK_USUARIOS_TIPOS_USUARIOS_USU_TPU_ID",
                        column: x => x.USU_TPU_ID,
                        principalTable: "TIPOS_USUARIOS",
                        principalColumn: "TPU_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOG_PEDIDOS",
                columns: table => new
                {
                    LPD_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LPD_DATA_HORA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LPD_ACAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LPD_MSA_ID = table.Column<int>(type: "int", nullable: false),
                    LPD_USU_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG_PEDIDOS", x => x.LPD_ID);
                    table.ForeignKey(
                        name: "FK_LOG_PEDIDOS_MESAS_LPD_MSA_ID",
                        column: x => x.LPD_MSA_ID,
                        principalTable: "MESAS",
                        principalColumn: "MSA_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOG_PEDIDOS_USUARIOS_LPD_USU_ID",
                        column: x => x.LPD_USU_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "USU_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDOS",
                columns: table => new
                {
                    PED_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PED_DATA_HORA = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PED_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PED_VALOR_TOTAL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PED_USU_ID = table.Column<int>(type: "int", nullable: false),
                    PED_MSA_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDOS", x => x.PED_ID);
                    table.ForeignKey(
                        name: "FK_PEDIDOS_MESAS_PED_MSA_ID",
                        column: x => x.PED_MSA_ID,
                        principalTable: "MESAS",
                        principalColumn: "MSA_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PEDIDOS_USUARIOS_PED_USU_ID",
                        column: x => x.PED_USU_ID,
                        principalTable: "USUARIOS",
                        principalColumn: "USU_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ITENS_PEDIDO",
                columns: table => new
                {
                    ITP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ITP_QUANTIDADE = table.Column<int>(type: "int", nullable: false),
                    ITP_OBSERVACOES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ITP_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ITP_PRA_ID = table.Column<int>(type: "int", nullable: false),
                    ITP_PED_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITENS_PEDIDO", x => x.ITP_ID);
                    table.ForeignKey(
                        name: "FK_ITENS_PEDIDO_PEDIDOS_ITP_PED_ID",
                        column: x => x.ITP_PED_ID,
                        principalTable: "PEDIDOS",
                        principalColumn: "PED_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ITENS_PEDIDO_PRATOS_ITP_PRA_ID",
                        column: x => x.ITP_PRA_ID,
                        principalTable: "PRATOS",
                        principalColumn: "PRA_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAGAMENTOS",
                columns: table => new
                {
                    PAG_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PAG_VALOR_PAGO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PAG_DESCONTO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PAG_DATA_PAGAMENTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PAG_FPG_ID = table.Column<int>(type: "int", nullable: false),
                    PAG_PED_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAGAMENTOS", x => x.PAG_ID);
                    table.ForeignKey(
                        name: "FK_PAGAMENTOS_FORMAS_PAGAMENTO_PAG_FPG_ID",
                        column: x => x.PAG_FPG_ID,
                        principalTable: "FORMAS_PAGAMENTO",
                        principalColumn: "FPG_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PAGAMENTOS_PEDIDOS_PAG_PED_ID",
                        column: x => x.PAG_PED_ID,
                        principalTable: "PEDIDOS",
                        principalColumn: "PED_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ITENS_PEDIDO_ITP_PED_ID",
                table: "ITENS_PEDIDO",
                column: "ITP_PED_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ITENS_PEDIDO_ITP_PRA_ID",
                table: "ITENS_PEDIDO",
                column: "ITP_PRA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_PEDIDOS_LPD_MSA_ID",
                table: "LOG_PEDIDOS",
                column: "LPD_MSA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_PEDIDOS_LPD_USU_ID",
                table: "LOG_PEDIDOS",
                column: "LPD_USU_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PAGAMENTOS_PAG_FPG_ID",
                table: "PAGAMENTOS",
                column: "PAG_FPG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PAGAMENTOS_PAG_PED_ID",
                table: "PAGAMENTOS",
                column: "PAG_PED_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDOS_PED_MSA_ID",
                table: "PEDIDOS",
                column: "PED_MSA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDOS_PED_USU_ID",
                table: "PEDIDOS",
                column: "PED_USU_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRATOS_PRA_CTP_ID",
                table: "PRATOS",
                column: "PRA_CTP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOS_USU_TPU_ID",
                table: "USUARIOS",
                column: "USU_TPU_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITENS_PEDIDO");

            migrationBuilder.DropTable(
                name: "LOG_PEDIDOS");

            migrationBuilder.DropTable(
                name: "PAGAMENTOS");

            migrationBuilder.DropTable(
                name: "PRATOS");

            migrationBuilder.DropTable(
                name: "FORMAS_PAGAMENTO");

            migrationBuilder.DropTable(
                name: "PEDIDOS");

            migrationBuilder.DropTable(
                name: "CATEGORIAS_PRATOS");

            migrationBuilder.DropTable(
                name: "MESAS");

            migrationBuilder.DropTable(
                name: "USUARIOS");

            migrationBuilder.DropTable(
                name: "TIPOS_USUARIOS");
        }
    }
}
