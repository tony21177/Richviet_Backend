using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Richviet.Services.Models
{
    public partial class GeneralContext : DbContext
    {
        public GeneralContext()
        {
        }

        public GeneralContext(DbContextOptions<GeneralContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BussinessUnitRemitSetting> BussinessUnitRemitSetting { get; set; }
        public virtual DbSet<CurrencyCode> CurrencyCode { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<OftenBeneficiar> OftenBeneficiar { get; set; }
        public virtual DbSet<PayeeRelationType> PayeeRelationType { get; set; }
        public virtual DbSet<PayeeType> PayeeType { get; set; }
        public virtual DbSet<ReceiveBank> ReceiveBank { get; set; }
        public virtual DbSet<RemitRecord> RemitRecord { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserArc> UserArc { get; set; }
        public virtual DbSet<UserInfoView> UserInfoView { get; set; }
        public virtual DbSet<UserLoginLog> UserLoginLog { get; set; }
        public virtual DbSet<UserRegisterType> UserRegisterType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BussinessUnitRemitSetting>(entity =>
            {
                entity.ToTable("bussiness_unit_remit_setting");

                entity.HasComment("服務所在國家的匯款相關設定");

                entity.HasIndex(e => e.Country)
                    .HasName("country_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("服務所在國家");

                entity.Property(e => e.RemitMax)
                    .HasColumnName("remit_max")
                    .HasComment("匯款最高金額");

                entity.Property(e => e.RemitMin)
                    .HasColumnName("remit_min")
                    .HasComment("匯款最低金額");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<CurrencyCode>(entity =>
            {
                entity.ToTable("currency_code");

                entity.HasComment("國家可使用貨幣幣別,比如越南可收美金和越南盾");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("國家");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasColumnName("currency_name")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("貨幣名稱");

                entity.Property(e => e.Fee)
                    .HasColumnName("fee")
                    .HasComment("收款幣種為此幣別時收的手續費(以匯出幣種為計價單位)");

                entity.Property(e => e.FeeType)
                    .HasColumnName("fee_type")
                    .HasColumnType("tinyint(1)")
                    .HasComment("手續費計算方式\\n0:數量\\n1:百分比");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("discount");

                entity.HasComment("優惠券");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_discount_user_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnName("effective_date")
                    .HasColumnType("date");

                entity.Property(e => e.ExpireDate)
                    .HasColumnName("expire_date")
                    .HasColumnType("date");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UseStatus)
                    .HasColumnName("use_status")
                    .HasColumnType("tinyint(2)")
                    .HasComment("0:可使用,1:已使用,2:無效");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Discount)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_discount_user");
            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.ToTable("exchange_rate");

                entity.HasComment("台幣對幣別匯率");

                entity.HasIndex(e => e.CurrencyName)
                    .HasName("currency_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CurrencyName)
                    .HasColumnName("currency_name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnName("rate");
            });

            modelBuilder.Entity<OftenBeneficiar>(entity =>
            {
                entity.ToTable("often_beneficiar");

                entity.HasComment("常用收款人");

                entity.HasIndex(e => e.PayeeRelationId)
                    .HasName("fk_often_beneficiar_payee_relation_idx");

                entity.HasIndex(e => e.PayeeTypeId)
                    .HasName("fk_often_beneficiar_payee_type1_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_often_beneficiar_user1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("收款人姓名");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("備註");

                entity.Property(e => e.PayeeAddress)
                    .IsRequired()
                    .HasColumnName("payee_address")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment(@"根據type有不同格式
");

                entity.Property(e => e.PayeeId)
                    .IsRequired()
                    .HasColumnName("payee_id")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("收款人的ID");

                entity.Property(e => e.PayeeRelationId)
                    .HasColumnName("payee_relation_id")
                    .HasColumnType("int(11)")
                    .HasComment(@"對應payee_relation_type的pk(與收款人的關係)
");

                entity.Property(e => e.PayeeTypeId)
                    .HasColumnName("payee_type_id")
                    .HasColumnType("int(11)")
                    .HasComment("對照payee_type的pk(收款方式)");

                entity.Property(e => e.ReceiveBankId)
                    .HasColumnName("receive_bank_id")
                    .HasColumnType("int(11)")
                    .HasComment("對應receive_bank的pk(收款方銀行代號)\\n");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.PayeeRelation)
                    .WithMany(p => p.OftenBeneficiar)
                    .HasForeignKey(d => d.PayeeRelationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_often_beneficiar_payee_relation");

                entity.HasOne(d => d.PayeeType)
                    .WithMany(p => p.OftenBeneficiar)
                    .HasForeignKey(d => d.PayeeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_often_beneficiar_payee_type");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OftenBeneficiar)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_often_beneficiar_user");
            });

            modelBuilder.Entity<PayeeRelationType>(entity =>
            {
                entity.ToTable("payee_relation_type");

                entity.HasComment("收款人關係");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(2)");
            });

            modelBuilder.Entity<PayeeType>(entity =>
            {
                entity.ToTable("payee_type");

                entity.HasComment("收款方式");

                entity.HasIndex(e => e.Type)
                    .HasName("type_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("收款方式描述");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(4)")
                    .HasComment(@"收款方式
0:銀行");
            });

            modelBuilder.Entity<ReceiveBank>(entity =>
            {
                entity.ToTable("receive_bank");

                entity.HasComment("可收款银行表");

                entity.HasIndex(e => e.SwiftCode)
                    .HasName("uk_swift_code")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasComment("台灣銀行代碼");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasColumnName("en_name")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("名稱(英文)");

                entity.Property(e => e.SortNum)
                    .HasColumnName("sort_num")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("排序");

                entity.Property(e => e.SwiftCode)
                    .IsRequired()
                    .HasColumnName("swift_code")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasComment("Swift Code");

                entity.Property(e => e.TwName)
                    .IsRequired()
                    .HasColumnName("tw_name")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("名稱(繁体中文)");

                entity.Property(e => e.VietName)
                    .IsRequired()
                    .HasColumnName("viet_name")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("名稱(越南)");
            });

            modelBuilder.Entity<RemitRecord>(entity =>
            {
                entity.ToTable("remit_record");

                entity.HasComment("匯款紀錄");

                entity.HasIndex(e => e.BeneficiarId)
                    .HasName("fk_remit_record_beneficiar_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_remit_record_user1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApplyExchangeRate)
                    .HasColumnName("apply_exchange_rate")
                    .HasComment(@"使用者申請時當下匯率
");

                entity.Property(e => e.ArcName)
                    .IsRequired()
                    .HasColumnName("arc_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArcNo)
                    .IsRequired()
                    .HasColumnName("arc_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ArcStatus)
                    .HasColumnName("arc_status")
                    .HasColumnType("tinyint(2)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("0:arc未審核,1:系統自動審核arc成功");

                entity.Property(e => e.ArcVerifyTime)
                    .HasColumnName("arc_verify_time")
                    .HasComment("系統自動審核移名屬ARC時間");

                entity.Property(e => e.BeneficiarId)
                    .HasColumnName("beneficiar_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnName("discount_amount")
                    .HasComment("總折扣金額");

                entity.Property(e => e.DiscountId)
                    .HasColumnName("discount_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ESignature)
                    .IsRequired()
                    .HasColumnName("e-signature")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("電子簽名");

                entity.Property(e => e.Fee)
                    .HasColumnName("fee")
                    .HasComment(@"手續費要搭配fee_type
");

                entity.Property(e => e.FeeType)
                    .HasColumnName("fee_type")
                    .HasColumnType("tinyint(1)")
                    .HasComment(@"手續費計算方式
0:數量
1:百分比");

                entity.Property(e => e.FromAmount).HasColumnName("from_amount");

                entity.Property(e => e.FromCurrencyId)
                    .HasColumnName("from_currency_id")
                    .HasColumnType("int(11)")
                    .HasComment("匯出國家幣(對應currency_code的pk)");

                entity.Property(e => e.IdImageA)
                    .IsRequired()
                    .HasColumnName("id_image_a")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.IdImageB)
                    .IsRequired()
                    .HasColumnName("id_image_b")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.IdImageC)
                    .IsRequired()
                    .HasColumnName("id_image_c")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PayeeType)
                    .HasColumnName("payee_type")
                    .HasColumnType("tinyint(4)")
                    .HasComment(@"收款方式,對應table:payee_type
");

                entity.Property(e => e.PaymentCode)
                    .HasColumnName("payment_code")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("繳款碼,給前端產生QR CODE用");

                entity.Property(e => e.PaymentTime)
                    .HasColumnName("payment_time")
                    .HasComment("會員繳款時間");

                entity.Property(e => e.RealTimePic)
                    .IsRequired()
                    .HasColumnName("real_time_pic")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("即時拍照");

                entity.Property(e => e.ToCurrencyId)
                    .HasColumnName("to_currency_id")
                    .HasColumnType("int(11)")
                    .HasComment("收款國家幣(對應currency_code的pk)");

                entity.Property(e => e.TransactionExchangeRate)
                    .HasColumnName("transaction_exchange_rate")
                    .HasComment(@"實際匯款時的匯率
");

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .HasColumnType("tinyint(4)")
                    .HasComment("99:其他錯誤\\\\n9: 審核失敗\\\\n0: 待審核(系統進入arc_status流程)\\\\n1: 待繳款\\\\n2: 已繳款\\\\n3:處理完成");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Beneficiar)
                    .WithMany(p => p.RemitRecord)
                    .HasForeignKey(d => d.BeneficiarId)
                    .HasConstraintName("fk_remit_record_beneficiar");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RemitRecord)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_remit_record_user1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasComment("用户表");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("信箱");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("tinyint(2)")
                    .HasComment("0:其他(包括未填)\\n1:男\\n2:女\\n");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("密碼");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("手機號碼");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(2)")
                    .HasComment("會員狀態\\\\n0:草稿會員\\\\n1:正式會員");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("更新时间")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<UserArc>(entity =>
            {
                entity.ToTable("user_arc");

                entity.HasComment("使用者KYC資料");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ArcIssueDate)
                    .HasColumnName("arc_issue_date")
                    .HasColumnType("date")
                    .HasComment("發證日期");

                entity.Property(e => e.ArcName)
                    .IsRequired()
                    .HasColumnName("arc_name")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("ARC姓名");

                entity.Property(e => e.ArcNo)
                    .IsRequired()
                    .HasColumnName("arc_no")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("ARC ID");

                entity.Property(e => e.BackSequence)
                    .IsRequired()
                    .HasColumnName("back_sequence")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("背面序號");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("國家");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IdImageA)
                    .IsRequired()
                    .HasColumnName("id_image_a")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("證件正面");

                entity.Property(e => e.IdImageB)
                    .IsRequired()
                    .HasColumnName("id_image_b")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("證件反面");

                entity.Property(e => e.IdImageC)
                    .IsRequired()
                    .HasColumnName("id_image_c")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("手持證件照");

                entity.Property(e => e.KycStatus)
                    .HasColumnName("kyc_status")
                    .HasColumnType("tinyint(2)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("KYC審核狀態, \\\\r\\\\n9:未通過, \\\\r\\\\n0:未認證,\\\\r\\\\n1:待審核,\\\\r\\\\n2:審核通過;");

                entity.Property(e => e.KycStatusUpdateTime)
                    .HasColumnName("kyc_status_update_time")
                    .HasComment("LV2审核通过时间");

                entity.Property(e => e.PassportId)
                    .IsRequired()
                    .HasColumnName("passport_id")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("護照號碼");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("更新时间")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasComment("對應user的pk");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserArc)
                    .HasForeignKey<UserArc>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_arc");
            });

            modelBuilder.Entity<UserInfoView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("user_info_view");

                entity.Property(e => e.ArcName)
                    .IsRequired()
                    .HasColumnName("arc_name")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("ARC姓名");

                entity.Property(e => e.ArcNo)
                    .IsRequired()
                    .HasColumnName("arc_no")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("ARC ID");

                entity.Property(e => e.AuthPlatformId)
                    .IsRequired()
                    .HasColumnName("auth_platform_id")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("不同平台(FB,Apple...)的id");

                entity.Property(e => e.BackSequence)
                    .IsRequired()
                    .HasColumnName("back_sequence")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("背面序號");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("國家");

                entity.Property(e => e.CreateTime).HasColumnName("create_time");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("信箱");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("tinyint(2)")
                    .HasComment("0:其他(包括未填)\\n1:男\\n2:女\\n");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdImageA)
                    .IsRequired()
                    .HasColumnName("id_image_a")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("證件正面");

                entity.Property(e => e.IdImageB)
                    .IsRequired()
                    .HasColumnName("id_image_b")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("證件反面");

                entity.Property(e => e.IdImageC)
                    .IsRequired()
                    .HasColumnName("id_image_c")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("手持證件照");

                entity.Property(e => e.KycStatus)
                    .HasColumnName("kyc_status")
                    .HasColumnType("tinyint(2)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("KYC審核狀態, \\\\r\\\\n9:未通過, \\\\r\\\\n0:未認證,\\\\r\\\\n1:待審核,\\\\r\\\\n2:審核通過;");

                entity.Property(e => e.KycStatusUpdateTime)
                    .HasColumnName("kyc_status_update_time")
                    .HasComment("LV2审核通过时间");

                entity.Property(e => e.LoginPlatformEmal)
                    .HasColumnName("login_platform_emal")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("平台的名字");

                entity.Property(e => e.PassportId)
                    .IsRequired()
                    .HasColumnName("passport_id")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("護照號碼");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("手機號碼");

                entity.Property(e => e.RegisterTime)
                    .HasColumnName("register_time")
                    .HasComment("注册时间");

                entity.Property(e => e.RegisterType)
                    .HasColumnName("register_type")
                    .HasColumnType("tinyint(2)")
                    .HasComment(@"註冊方式\\n0:平台本身\n1:FB\n2:apple\n3:google\n4:zalo
");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(2)")
                    .HasComment("會員狀態\\\\n0:草稿會員\\\\n1:正式會員");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasComment("更新时间");
            });

            modelBuilder.Entity<UserLoginLog>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.UserId })
                    .HasName("PRIMARY");

                entity.ToTable("user_login_log");

                entity.HasComment("用戶登入紀錄");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_login_log_user_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("login地區");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("IP");

                entity.Property(e => e.LoginTime)
                    .HasColumnName("login_time")
                    .HasComment("登入時間");

                entity.Property(e => e.LoginType)
                    .HasColumnName("login_type")
                    .HasColumnType("tinyint(2)")
                    .HasComment("0:平台本身\\n1:FB\\n2:apple\\n3:google\\n4:zalo");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLoginLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_login_log_user");
            });

            modelBuilder.Entity<UserRegisterType>(entity =>
            {
                entity.ToTable("user_register_type");

                entity.HasComment("使用者註冊的方式");

                entity.HasIndex(e => new { e.UserId, e.AuthPlatformId })
                    .HasName("uni_user_id_platform_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AuthPlatformId)
                    .IsRequired()
                    .HasColumnName("auth_platform_id")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasComment("不同平台(FB,Apple...)的id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("''")
                    .HasComment("平台的名字");

                entity.Property(e => e.RegisterTime)
                    .HasColumnName("register_time")
                    .HasComment("注册时间");

                entity.Property(e => e.RegisterType)
                    .HasColumnName("register_type")
                    .HasColumnType("tinyint(2)")
                    .HasComment(@"註冊方式\\n0:平台本身\n1:FB\n2:apple\n3:google\n4:zalo
");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("更新时间")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .HasComment("對應user的pk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRegisterType)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_register");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
