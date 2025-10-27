using System;
using System.Collections.Generic;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Canaa.Infra.Entities.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WF_PARAM_MARMITum> WF_PARAM_MARMITAs { get; set; }

    public virtual DbSet<Z_TAREFA> Z_TAREFAs { get; set; }

    public virtual DbSet<Z_USUARIO> Z_USUARIOs { get; set; }

    public virtual DbSet<blog> blogs { get; set; }

    public virtual DbSet<blog_tag> blog_tags { get; set; }

    public virtual DbSet<card> cards { get; set; }

    public virtual DbSet<categorias_compra> categorias_compras { get; set; }

    public virtual DbSet<compras_cartao> compras_cartaos { get; set; }
    public virtual DbSet<ContatosEmail> ContatosEmail { get; set; }

    public virtual DbSet<draw> draws { get; set; }

    public virtual DbSet<favorite_news> favorite_news { get; set; }

    public virtual DbSet<gastos_mensais_notion> gastos_mensais_notions { get; set; }

    public virtual DbSet<gastosgerai> gastosgerais { get; set; }

    public virtual DbSet<mirante_categoria> mirante_categorias { get; set; }

    public virtual DbSet<mirante_cupon> mirante_cupons { get; set; }

    public virtual DbSet<mirante_imagen> mirante_imagens { get; set; }

    public virtual DbSet<mirante_produto> mirante_produtos { get; set; }

    public virtual DbSet<mirante_subcategoria> mirante_subcategorias { get; set; }

    public virtual DbSet<news> news { get; set; }

    public virtual DbSet<participant> participants { get; set; }

    public virtual DbSet<participante> participantes { get; set; }

    public virtual DbSet<produtosagro> produtosagros { get; set; }

    public virtual DbSet<register_estacionamento> register_estacionamentos { get; set; }

    public virtual DbSet<saldo_conta_notion> saldo_conta_notions { get; set; }

    public virtual DbSet<sis_usuario> sis_usuarios { get; set; }

    public virtual DbSet<sorteio> sorteios { get; set; }

    public virtual DbSet<tag> tags { get; set; }

    public virtual DbSet<thema_dark> thema_darks { get; set; }

    public virtual DbSet<users_C> users_Cs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql(Config.GetConnectionString(), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<WF_PARAM_MARMITum>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("WF_PARAM_MARMITA");

            entity.Property(e => e.DATA_ULTIMA_ALTERACAO)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.DIADASEMANA).HasMaxLength(45);
            entity.Property(e => e.INSTANCIA).HasMaxLength(45);
            entity.Property(e => e.TEXTO).HasMaxLength(250);
        });

        modelBuilder.Entity<Z_TAREFA>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity.ToTable("Z_TAREFAS");

            entity.Property(e => e.CATEGORIA).HasMaxLength(45);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ERRO).HasColumnType("text");
            entity.Property(e => e.FILEPATH).HasColumnType("text");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.URL).HasColumnType("text");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Z_USUARIO>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PRIMARY");

            entity
                .ToTable("Z_USUARIOS")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.EMAIL, "email").IsUnique();

            entity.Property(e => e.APELIDO).HasMaxLength(45);
            entity.Property(e => e.COMPANY)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.EMAIL).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.FOTO)
                .HasColumnType("text")
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.NOME)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.PAPEL)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.SENHA)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<blog>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.content).HasColumnType("text");
            entity.Property(e => e.cover_link).HasMaxLength(255);
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.description).HasColumnType("text");
            entity.Property(e => e.meta_description).HasColumnType("text");
            entity.Property(e => e.meta_title).HasMaxLength(255);
            entity.Property(e => e.title).HasMaxLength(255);
            entity.Property(e => e.type).HasMaxLength(45);
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<blog_tag>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.tag_value).HasMaxLength(45);
        });

        modelBuilder.Entity<card>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.cardNumber).HasMaxLength(45);
            entity.Property(e => e.cardType).HasMaxLength(45);
            entity.Property(e => e.cardValid).HasMaxLength(20);
        });

        modelBuilder.Entity<categorias_compra>(entity =>
        {
            entity.HasKey(e => e.id_categoria).HasName("PRIMARY");

            entity.Property(e => e.icon).HasMaxLength(45);
            entity.Property(e => e.nome_categoria).HasMaxLength(100);
        });

        modelBuilder.Entity<compras_cartao>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("compras_cartao");

            entity.Property(e => e.compra_hora).HasColumnType("time");
            entity.Property(e => e.compra_nome).HasMaxLength(255);
            entity.Property(e => e.compra_valor).HasPrecision(10, 2);
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.descricao).HasMaxLength(45);
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<draw>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasIndex(e => e.participant_id, "participant_id").IsUnique();

            entity.Property(e => e.drawn_name).HasMaxLength(255);

            entity.HasOne(d => d.participant).WithOne(p => p.draw)
                .HasForeignKey<draw>(d => d.participant_id)
                .HasConstraintName("draws_ibfk_1");
        });

        modelBuilder.Entity<favorite_news>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.content).HasColumnType("text");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.description).HasColumnType("text");
            entity.Property(e => e.image).HasMaxLength(255);
            entity.Property(e => e.publishedAt).HasMaxLength(255);
            entity.Property(e => e.source_name).HasMaxLength(255);
            entity.Property(e => e.source_url).HasMaxLength(255);
            entity.Property(e => e.title).HasMaxLength(255);
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.url).HasMaxLength(255);
        });

        modelBuilder.Entity<gastos_mensais_notion>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("gastos_mensais_notion");

            entity.Property(e => e.avatarImage).HasMaxLength(200);
            entity.Property(e => e.conta).HasMaxLength(36);
            entity.Property(e => e.conta_origem).HasMaxLength(45);
            entity.Property(e => e.data).HasColumnType("datetime");
            entity.Property(e => e.descricao).HasMaxLength(100);
            entity.Property(e => e.handleNotion).HasMaxLength(45);
            entity.Property(e => e.name).HasMaxLength(255);
            entity.Property(e => e.status).HasMaxLength(50);
            entity.Property(e => e.url).HasMaxLength(255);

            entity.HasMany(d => d.id_categoria).WithMany(p => p.id_compras)
                .UsingEntity<Dictionary<string, object>>(
                    "compra_categorium",
                    r => r.HasOne<categorias_compra>().WithMany()
                        .HasForeignKey("id_categoria")
                        .HasConstraintName("compra_categoria_ibfk_2"),
                    l => l.HasOne<gastos_mensais_notion>().WithMany()
                        .HasForeignKey("id_compra")
                        .HasConstraintName("compra_categoria_ibfk_1"),
                    j =>
                    {
                        j.HasKey("id_compra", "id_categoria")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("compra_categoria");
                        j.HasIndex(new[] { "id_categoria" }, "id_categoria");
                    });
        });

        modelBuilder.Entity<gastosgerai>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.categoria).HasMaxLength(45);
            entity.Property(e => e.descricao).HasMaxLength(45);
            entity.Property(e => e.valor).HasPrecision(10, 2);
        });

        modelBuilder.Entity<mirante_categoria>(entity =>
        {
            entity.HasKey(e => e.id_categoria).HasName("PRIMARY");

            entity.Property(e => e.nome_categoria).HasMaxLength(255);
        });

        modelBuilder.Entity<mirante_cupon>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.nome).HasMaxLength(45);
            entity.Property(e => e.status).HasMaxLength(45);
        });

        modelBuilder.Entity<mirante_imagen>(entity =>
        {
            entity.HasKey(e => e.id_imagem).HasName("PRIMARY");

            entity.Property(e => e.nome_arquivo).HasMaxLength(255);
            entity.Property(e => e.url_arquivo).HasMaxLength(255);
        });

        modelBuilder.Entity<mirante_produto>(entity =>
        {
            entity.HasKey(e => e.id_produto).HasName("PRIMARY");

            entity.Property(e => e.nome_produto).HasMaxLength(255);
        });

        modelBuilder.Entity<mirante_subcategoria>(entity =>
        {
            entity.HasKey(e => e.id_subcategoria).HasName("PRIMARY");

            entity.Property(e => e.nome_subcategoria).HasMaxLength(255);
        });

        modelBuilder.Entity<news>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.content).HasColumnType("text");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.description).HasColumnType("text");
            entity.Property(e => e.image).HasColumnType("text");
            entity.Property(e => e.publishedAt).HasMaxLength(255);
            entity.Property(e => e.q).HasMaxLength(45);
            entity.Property(e => e.source_name).HasMaxLength(255);
            entity.Property(e => e.source_url).HasMaxLength(255);
            entity.Property(e => e.title).HasMaxLength(255);
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.url).HasMaxLength(255);
        });

        modelBuilder.Entity<participant>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasIndex(e => e.name, "name").IsUnique();
        });

        modelBuilder.Entity<participante>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.nome).HasMaxLength(255);
        });

        modelBuilder.Entity<produtosagro>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity
                .ToTable("produtosagro")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.imagem_produto).HasMaxLength(255);
            entity.Property(e => e.name_produto).HasMaxLength(255);
            entity.Property(e => e.quantidade_produto).HasMaxLength(255);
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.valor_produto).HasMaxLength(255);
        });

        modelBuilder.Entity<register_estacionamento>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("register_estacionamento");

            entity.Property(e => e.register_estacionamento_chegada).HasColumnType("datetime");
            entity.Property(e => e.register_estacionamento_duracao).HasColumnType("time");
            entity.Property(e => e.register_estacionamento_pago).HasMaxLength(45);
            entity.Property(e => e.register_estacionamento_placa).HasMaxLength(45);
            entity.Property(e => e.register_estacionamento_preco).HasMaxLength(45);
            entity.Property(e => e.register_estacionamento_saida).HasColumnType("datetime");
        });

        modelBuilder.Entity<saldo_conta_notion>(entity =>
        {
            entity.HasKey(e => e.notion_id).HasName("PRIMARY");

            entity.ToTable("saldo_conta_notion");

            entity.Property(e => e.notion_id).HasMaxLength(45);
            entity.Property(e => e.notion_data).HasColumnType("datetime");
            entity.Property(e => e.notion_name).HasMaxLength(45);
            entity.Property(e => e.notion_property_conta).HasMaxLength(45);
            entity.Property(e => e.notion_receita).HasMaxLength(45);
            entity.Property(e => e.notion_status).HasMaxLength(45);
            entity.Property(e => e.notion_url).HasColumnType("text");
        });

        modelBuilder.Entity<sis_usuario>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("sis_usuario");

            entity.Property(e => e.ds_email).HasMaxLength(50);
            entity.Property(e => e.ds_nome).HasMaxLength(45);
            entity.Property(e => e.ds_telefone).HasMaxLength(45);
        });

        modelBuilder.Entity<sorteio>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasIndex(e => e.id_sorteador, "id_sorteador");

            entity.Property(e => e.nome_sorteado).HasMaxLength(255);

            entity.HasOne(d => d.id_sorteadorNavigation).WithMany(p => p.sorteios)
                .HasForeignKey(d => d.id_sorteador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sorteios_ibfk_1");
        });

        modelBuilder.Entity<tag>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.name).HasMaxLength(255);
        });

        modelBuilder.Entity<thema_dark>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("thema_dark");
        });

        modelBuilder.Entity<users_C>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("users_C");

            entity.Property(e => e.users_C_name).HasMaxLength(45);
            entity.Property(e => e.users_C_passwd).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
