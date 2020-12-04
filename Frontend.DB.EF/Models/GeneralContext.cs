using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Frontend.DB.EF.Models
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

        public virtual DbSet<AmlScanRecord> AmlScanRecord { get; set; }
        public virtual DbSet<ArcScanRecord> ArcScanRecord { get; set; }
        public virtual DbSet<BussinessUnitRemitSetting> BussinessUnitRemitSetting { get; set; }
        public virtual DbSet<CurrencyCode> CurrencyCode { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<OftenBeneficiary> OftenBeneficiary { get; set; }
        public virtual DbSet<PayeeRelationType> PayeeRelationType { get; set; }
        public virtual DbSet<PayeeType> PayeeType { get; set; }
        public virtual DbSet<PushNotificationSetting> PushNotificationSetting { get; set; }
        public virtual DbSet<ReceiveBank> ReceiveBank { get; set; }
        public virtual DbSet<RemitAdminReviewLog> RemitAdminReviewLog { get; set; }
        public virtual DbSet<RemitRecord> RemitRecord { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserArc> UserArc { get; set; }
        public virtual DbSet<UserInfoView> UserInfoView { get; set; }
        public virtual DbSet<UserLoginLog> UserLoginLog { get; set; }
        public virtual DbSet<UserRegisterType> UserRegisterType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-DCDEKIUG;Database=General;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AmlScanRecord>(entity =>
            {
                entity.ToTable("aml_scan_record");

                entity.HasComment("會員AML系統掃描紀錄");

                entity.Property(e => e.AmlStatus).HasColumnName("aml_status");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasComment("事件0:註冊,1:匯款");

                entity.Property(e => e.ScanTime)
                    .HasColumnName("scan_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ArcScanRecord>(entity =>
            {
                entity.ToTable("arc_scan_record");

                entity.HasComment("會員KYC移民署系統掃描紀錄");

                entity.Property(e => e.ArcStatus)
                    .HasColumnName("arc_status")
                    .HasComment("系統移民屬ARC驗證-2:系統驗證失敗,-1:資料不符,0:未確認,1:資料符合");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasComment("事件0:註冊,1:匯款");

                entity.Property(e => e.ScanTime)
                    .HasColumnName("scan_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<BussinessUnitRemitSetting>(entity =>
            {
                entity.ToTable("bussiness_unit_remit_setting");

                entity.HasComment("服務所在國家的匯款相關設定");

                entity.HasIndex(e => e.Country)
                    .HasName("uq_country_Unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(10)
                    .HasComment("服務所在國家");

                entity.Property(e => e.DailyMax)
                    .HasColumnName("daily_max")
                    .HasComment("一天最大限額");

                entity.Property(e => e.MonthlyMax)
                    .HasColumnName("monthly_max")
                    .HasComment("一個月最大限額");

                entity.Property(e => e.RemitMax)
                    .HasColumnName("remit_max")
                    .HasComment("匯款最高金額");

                entity.Property(e => e.RemitMin)
                    .HasColumnName("remit_min")
                    .HasComment("匯款最低金額");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.YearlyMax)
                    .HasColumnName("yearly_max")
                    .HasComment("一年最大限額");
            });

            modelBuilder.Entity<CurrencyCode>(entity =>
            {
                entity.ToTable("currency_code");

                entity.HasComment("國家可使用貨幣幣別,比如越南可收美金和越南盾");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(10)
                    .HasComment("國家");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasColumnName("currency_name")
                    .HasMaxLength(255)
                    .HasComment("貨幣名稱");

                entity.Property(e => e.Fee)
                    .HasColumnName("fee")
                    .HasComment("收款幣種為此幣別時收的手續費(以匯出幣種為計價單位)");

                entity.Property(e => e.FeeType)
                    .HasColumnName("fee_type")
                    .HasComment("手續費計算方式\\\\n0:數量\\\\n1:百分比");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("discount");

                entity.HasComment("優惠券");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnName("effective_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpireDate)
                    .HasColumnName("expire_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UseStatus)
                    .HasColumnName("use_status")
                    .HasComment("-1:無效,0:可使用,1:已使用");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Discount)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ffk_discount_user");
            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.ToTable("exchange_rate");

                entity.HasIndex(e => e.CurrencyName)
                    .HasName("uq_currency_name_Unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasColumnName("currency_name")
                    .HasMaxLength(45);

                entity.Property(e => e.Rate).HasColumnName("rate");
            });

            modelBuilder.Entity<OftenBeneficiary>(entity =>
            {
                entity.ToTable("often_beneficiary");

                entity.HasComment("常用收款人");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .HasComment("收款人姓名");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')")
                    .HasComment("備註");

                entity.Property(e => e.PayeeAddress)
                    .IsRequired()
                    .HasColumnName("payee_address")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("根據type有不同格式");

                entity.Property(e => e.PayeeId)
                    .IsRequired()
                    .HasColumnName("payee_id")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("收款人的ID");

                entity.Property(e => e.PayeeRelationId)
                    .HasColumnName("payee_relation_id")
                    .HasComment("對應payee_relation_type的pk(與收款人的關係)");

                entity.Property(e => e.PayeeTypeId)
                    .HasColumnName("payee_type_id")
                    .HasComment("對照payee_type的pk(收款方式)");

                entity.Property(e => e.ReceiveBankId)
                    .HasColumnName("receive_bank_id")
                    .HasComment("對應receive_bank的pk(收款方銀行代號)");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.PayeeRelation)
                    .WithMany(p => p.OftenBeneficiary)
                    .HasForeignKey(d => d.PayeeRelationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_often_beneficiary_payee_relation");

                entity.HasOne(d => d.PayeeType)
                    .WithMany(p => p.OftenBeneficiary)
                    .HasForeignKey(d => d.PayeeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_often_beneficiary_payee_type");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OftenBeneficiary)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_often_beneficiary_user");
            });

            modelBuilder.Entity<PayeeRelationType>(entity =>
            {
                entity.ToTable("payee_relation_type");

                entity.HasComment("收款人關係");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<PayeeType>(entity =>
            {
                entity.ToTable("payee_type");

                entity.HasComment("收款方式");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(45)
                    .HasDefaultValueSql("('')")
                    .HasComment("收款方式描述");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasComment("收款方式");
            });

            modelBuilder.Entity<PushNotificationSetting>(entity =>
            {
                entity.ToTable("push_notification_setting");

                entity.HasIndex(e => e.UserId)
                    .HasName("uq_push_notification_setting_user_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsTurnOn)
                    .HasColumnName("is_turn_on")
                    .HasDefaultValueSql("((1))")
                    .HasComment("使用者通知開關\\\\n1:開啟\\\\n0:關閉\\\\n");

                entity.Property(e => e.MobileToken)
                    .HasColumnName("mobile_token")
                    .HasMaxLength(256)
                    .HasComment("推播通知所需的裝置識別token");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasComment("對應user的pk");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.PushNotificationSetting)
                    .HasForeignKey<PushNotificationSetting>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_push_notification_setting");
            });

            modelBuilder.Entity<ReceiveBank>(entity =>
            {
                entity.ToTable("receive_bank");

                entity.HasComment("可收款银行表");

                entity.HasIndex(e => e.SwiftCode)
                    .HasName("uq_swift_code_Unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(5)
                    .HasComment("台灣銀行代碼");

                entity.Property(e => e.EnName)
                    .IsRequired()
                    .HasColumnName("en_name")
                    .HasMaxLength(100)
                    .HasComment("名稱(英文)");

                entity.Property(e => e.SortNum)
                    .HasColumnName("sort_num")
                    .HasComment("排序");

                entity.Property(e => e.SwiftCode)
                    .IsRequired()
                    .HasColumnName("swift_code")
                    .HasMaxLength(15);

                entity.Property(e => e.TwName)
                    .IsRequired()
                    .HasColumnName("tw_name")
                    .HasMaxLength(100)
                    .HasComment("名稱(繁体中文)");

                entity.Property(e => e.VietName)
                    .IsRequired()
                    .HasColumnName("viet_name")
                    .HasMaxLength(100)
                    .HasComment("名稱(越南)");
            });

            modelBuilder.Entity<RemitAdminReviewLog>(entity =>
            {
                entity.ToTable("remit_admin_review_log");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FromTransactionStatus)
                    .HasColumnName("from_transaction_status")
                    .HasComment("原來的交易狀態");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("('')")
                    .HasComment("備註");

                entity.Property(e => e.RemitRecordId).HasColumnName("remit_record_id");

                entity.Property(e => e.ToTransactionStatus).HasColumnName("to_transaction_status");

                entity.HasOne(d => d.RemitRecord)
                    .WithMany(p => p.RemitAdminReviewLog)
                    .HasForeignKey(d => d.RemitRecordId)
                    .HasConstraintName("FK_remit_admin_review_log_ToTable");
            });

            modelBuilder.Entity<RemitRecord>(entity =>
            {
                entity.ToTable("remit_record");

                entity.HasComment("匯款紀錄");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AmlScanRecordId)
                    .HasColumnName("aml_scan_record_id")
                    .HasComment("對應的系統掃描AML紀錄id");

                entity.Property(e => e.ApplyExchangeRate)
                    .HasColumnName("apply_exchange_rate")
                    .HasComment("使用者申請時當下匯率");

                entity.Property(e => e.ArcName)
                    .IsRequired()
                    .HasColumnName("arc_name")
                    .HasMaxLength(255);

                entity.Property(e => e.ArcNo)
                    .IsRequired()
                    .HasColumnName("arc_no")
                    .HasMaxLength(255);

                entity.Property(e => e.ArcScanRecordId)
                    .HasColumnName("arc_scan_record_id")
                    .HasComment("對應的系統掃描arc紀錄id");

                entity.Property(e => e.BeneficiaryId).HasColumnName("beneficiary_id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnName("discount_amount")
                    .HasComment("總折扣金額");

                entity.Property(e => e.DiscountId).HasColumnName("discount_id");

                entity.Property(e => e.ESignature)
                    .IsRequired()
                    .HasColumnName("e_signature")
                    .HasMaxLength(512)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fee)
                    .HasColumnName("fee")
                    .HasComment("手續費要搭配fee_type");

                entity.Property(e => e.FeeType)
                    .HasColumnName("fee_type")
                    .HasComment("手續費計算方式\\n0:數量\\n1:百分比");

                entity.Property(e => e.FormalApplyTime)
                    .HasColumnName("formal_apply_time")
                    .HasColumnType("datetime")
                    .HasComment("送出匯款申請時間");

                entity.Property(e => e.FromAmount).HasColumnName("from_amount");

                entity.Property(e => e.FromCurrencyId)
                    .HasColumnName("from_currency_id")
                    .HasComment("匯出國家幣(對應currency_code的pk)");

                entity.Property(e => e.IdImageA)
                    .IsRequired()
                    .HasColumnName("id_image_a")
                    .HasMaxLength(512)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdImageB)
                    .IsRequired()
                    .HasColumnName("id_image_b")
                    .HasMaxLength(512)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.IdImageC)
                    .IsRequired()
                    .HasColumnName("id_image_c")
                    .HasMaxLength(512)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PayeeType)
                    .HasColumnName("payee_type")
                    .HasComment("收款方式,對應table:payee_type");

                entity.Property(e => e.PaymentCode)
                    .HasColumnName("payment_code")
                    .HasMaxLength(200)
                    .HasComment("繳款碼,給前端產生QR CODE用");

                entity.Property(e => e.PaymentTime)
                    .HasColumnName("payment_time")
                    .HasColumnType("datetime")
                    .HasComment("會員繳款時間");

                entity.Property(e => e.RealTimePic)
                    .IsRequired()
                    .HasColumnName("real_time_pic")
                    .HasMaxLength(512)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ToCurrencyId)
                    .HasColumnName("to_currency_id")
                    .HasComment("收款國家幣(對應currency_code的pk)");

                entity.Property(e => e.TransactionExchangeRate)
                    .HasColumnName("transaction_exchange_rate")
                    .HasComment("實際匯款時的匯率");

                entity.Property(e => e.TransactionStatus)
                    .HasColumnName("transaction_status")
                    .HasComment("-10:其他錯誤,-9: 審核失敗,-8: AML未通過,-7:交易逾期,0:草稿,1: 待ARC審核,2ARC審核成功,3:AML審核成功,4:營運人員確認OK,待會員繳款狀態,5: 已繳款,待營運人員處理,9:處理完成");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.ArcScanRecord)
                    .WithMany(p => p.RemitRecord)
                    .HasForeignKey(d => d.ArcScanRecordId)
                    .HasConstraintName("fk_remit_record_arc_scan_record");

                entity.HasOne(d => d.Beneficiary)
                    .WithMany(p => p.RemitRecord)
                    .HasForeignKey(d => d.BeneficiaryId)
                    .HasConstraintName("FK_often_beneficiary_remit_record");

                entity.HasOne(d => d.ToCurrency)
                    .WithMany(p => p.RemitRecord)
                    .HasForeignKey(d => d.ToCurrencyId)
                    .HasConstraintName("FK_remit_record_currency_code");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RemitRecord)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_remit_record_user1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasComment("用户");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')")
                    .HasComment("信箱");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasComment("0:其他(包括未填)\\\\n1:男\\\\n2:女\\\\n");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasComment("會員等級0:一般會員;1:VIP;9:高風險");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .HasComment("密碼");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')")
                    .HasComment("手機號碼");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("更新時間");
            });

            modelBuilder.Entity<UserArc>(entity =>
            {
                entity.ToTable("user_arc");

                entity.HasComment("使用者KYC資料");

                entity.HasIndex(e => e.UserId)
                    .HasName("uq_user_id_Unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArcExpireDate)
                    .HasColumnName("arc_expire_date")
                    .HasColumnType("date")
                    .HasComment("居留期限");

                entity.Property(e => e.ArcIssueDate)
                    .HasColumnName("arc_issue_date")
                    .HasColumnType("date")
                    .HasComment("發證日期");

                entity.Property(e => e.ArcName)
                    .IsRequired()
                    .HasColumnName("arc_name")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')")
                    .HasComment("ARC姓名");

                entity.Property(e => e.ArcNo)
                    .IsRequired()
                    .HasColumnName("arc_no")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("ARC ID");

                entity.Property(e => e.BackSequence)
                    .IsRequired()
                    .HasColumnName("back_sequence")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("背面序號");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')")
                    .HasComment("國家");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdImageA)
                    .IsRequired()
                    .HasColumnName("id_image_a")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("證件正面");

                entity.Property(e => e.IdImageB)
                    .IsRequired()
                    .HasColumnName("id_image_b")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("證件反面");

                entity.Property(e => e.IdImageC)
                    .IsRequired()
                    .HasColumnName("id_image_c")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("手持證件照");

                entity.Property(e => e.KycStatus)
                    .HasColumnName("kyc_status")
                    .HasComment("KYC審核狀態, -10:禁用,-9:KYC未通過,-8:AML未通過 ,0:草稿會員,1:待審核(註冊完),2:ARC驗證成功,3:AML通過,9:正式會員(KYC審核通過)");

                entity.Property(e => e.KycStatusUpdateTime)
                    .HasColumnName("kyc_status_update_time")
                    .HasColumnType("datetime")
                    .HasComment("審核時間");

                entity.Property(e => e.LastArcScanRecordId)
                    .HasColumnName("last_arc_scan_record_id")
                    .HasComment("最後一次的ARC掃描紀錄id");

                entity.Property(e => e.PassportId)
                    .IsRequired()
                    .HasColumnName("passport_id")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .HasComment("護照號碼");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasComment("對應user的pk");

                entity.HasOne(d => d.LastArcScanRecord)
                    .WithMany(p => p.UserArc)
                    .HasForeignKey(d => d.LastArcScanRecordId)
                    .HasConstraintName("fk_user_arc_scan_id");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserArc)
                    .HasForeignKey<UserArc>(d => d.UserId)
                    .HasConstraintName("FK_User_UserArc");
            });

            modelBuilder.Entity<UserInfoView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("user_info_view");

                entity.Property(e => e.ArcExpireDate)
                    .HasColumnName("arc_expire_date")
                    .HasColumnType("date");

                entity.Property(e => e.ArcIssueDate)
                    .HasColumnName("arc_issue_date")
                    .HasColumnType("date");

                entity.Property(e => e.ArcName)
                    .IsRequired()
                    .HasColumnName("arc_name")
                    .HasMaxLength(255);

                entity.Property(e => e.ArcNo)
                    .IsRequired()
                    .HasColumnName("arc_no")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AuthPlatformId)
                    .IsRequired()
                    .HasColumnName("auth_platform_id")
                    .HasMaxLength(45);

                entity.Property(e => e.BackSequence)
                    .IsRequired()
                    .HasColumnName("back_sequence")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(255);

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdImageA)
                    .IsRequired()
                    .HasColumnName("id_image_a")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdImageB)
                    .IsRequired()
                    .HasColumnName("id_image_b")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdImageC)
                    .IsRequired()
                    .HasColumnName("id_image_c")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.KycStatus).HasColumnName("kyc_status");

                entity.Property(e => e.KycStatusUpdateTime)
                    .HasColumnName("kyc_status_update_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.LoginPlatformEmal)
                    .HasColumnName("login_platform_emal")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.PassportId)
                    .IsRequired()
                    .HasColumnName("passport_id")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.Property(e => e.RegisterTime)
                    .HasColumnName("register_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.RegisterType).HasColumnName("register_type");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<UserLoginLog>(entity =>
            {
                entity.ToTable("user_login_log");

                entity.HasComment("用戶登入紀錄");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(1024)
                    .HasDefaultValueSql("('')")
                    .HasComment("login地區");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LoginTime)
                    .HasColumnName("login_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("登入時間");

                entity.Property(e => e.LoginType)
                    .HasColumnName("login_type")
                    .HasComment("0:平台本身\\\\n1:FB\\\\n2:apple\\\\n3:google\\\\n4:zalo");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<UserRegisterType>(entity =>
            {
                entity.ToTable("user_register_type");

                entity.HasComment("使用者註冊的方式");

                entity.HasIndex(e => new { e.UserId, e.AuthPlatformId })
                    .HasName("uni_user_id_platform_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthPlatformId)
                    .IsRequired()
                    .HasColumnName("auth_platform_id")
                    .HasMaxLength(45)
                    .HasComment("不同平台(FB,Apple...)的id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('')")
                    .HasComment("平台的名字");

                entity.Property(e => e.RegisterTime)
                    .HasColumnName("register_time")
                    .HasColumnType("datetime")
                    .HasComment("註冊時間");

                entity.Property(e => e.RegisterType)
                    .HasColumnName("register_type")
                    .HasComment("註冊方式\\\\\\\\n0:平台本身\\\\n1:FB\\\\n2:apple\\\\n3:google\\\\n4:zalo\\n");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("更新時間");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasComment("對應user的pk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRegisterType)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_user_register");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
