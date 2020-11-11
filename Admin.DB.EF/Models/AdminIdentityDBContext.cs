using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Admin.DB.EF.Models
{
    public partial class AdminIdentityDBContext : DbContext
    {
        public AdminIdentityDBContext()
        {
        }

        public AdminIdentityDBContext(DbContextOptions<AdminIdentityDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminUser> AdminUser { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UsersXroles> UsersXroles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=Admin.Identity.DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.UserCode);

                entity.HasComment("使用者");

                entity.HasIndex(e => e.Account)
                    .IsUnique();

                entity.Property(e => e.UserCode)
                    .HasMaxLength(128)
                    .HasComment("使用者識別");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasComment("使用者帳號");

                entity.Property(e => e.Address)
                    .HasMaxLength(256)
                    .HasComment("地址");

                entity.Property(e => e.CellPhone)
                    .HasMaxLength(32)
                    .HasComment("使用者手機");

                entity.Property(e => e.CreatTime)
                    .HasColumnType("datetime")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateUser)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('System')")
                    .HasComment("建立人員");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .HasComment("使用者電子信箱");

                entity.Property(e => e.IsActive).HasComment("是否停權");

                entity.Property(e => e.IsSystemPassword)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasDefaultValueSql("('en-US')")
                    .HasComment("語言");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasComment("密碼(加密)");

                entity.Property(e => e.Phone)
                    .HasMaxLength(32)
                    .HasComment("使用者電話");

                entity.Property(e => e.TimeZone)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasDefaultValueSql("('UTC')")
                    .HasComment("時區");

                entity.Property(e => e.TwoFactorEnable).HasComment("是否需要雙因子驗證");

                entity.Property(e => e.TwoFactorType).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新時間");

                entity.Property(e => e.UpdateUser)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('System')")
                    .HasComment("更新人員");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasComment("使用者名稱");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleCode);

                entity.HasComment("角色");

                entity.HasIndex(e => e.RoleName)
                    .HasName("UK_Roles")
                    .IsUnique();

                entity.Property(e => e.RoleCode).HasMaxLength(128);

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasComment("建立時間");

                entity.Property(e => e.CreateUser)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('System')")
                    .HasComment("建立人員");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasComment("角色名稱");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasComment("更新時間");

                entity.Property(e => e.UpdateUser)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('System')")
                    .HasComment("更新人員");
            });

            modelBuilder.Entity<UsersXroles>(entity =>
            {
                entity.HasKey(e => new { e.RoleCode, e.UserCode });

                entity.ToTable("UsersXRoles");

                entity.HasComment("使用者角色對應檔");

                entity.Property(e => e.RoleCode).HasMaxLength(128);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(128)
                    .HasComment("使用者識別");

                entity.HasOne(d => d.RoleCodeNavigation)
                    .WithMany(p => p.UsersXroles)
                    .HasForeignKey(d => d.RoleCode)
                    .HasConstraintName("FK_UsersXRoles_Roles");

                entity.HasOne(d => d.UserCodeNavigation)
                    .WithMany(p => p.UsersXroles)
                    .HasForeignKey(d => d.UserCode)
                    .HasConstraintName("FK_UsersXRoles_AdminUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
