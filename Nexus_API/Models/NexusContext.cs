using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nexus_API.Models;

public partial class NexusContext : DbContext
{
    public NexusContext()
    {
    }

    public NexusContext(DbContextOptions<NexusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ВидКредита> ВидКредитаs { get; set; }

    public virtual DbSet<ГрафикПлатежей> ГрафикПлатежейs { get; set; }

    public virtual DbSet<Депозит> Депозитs { get; set; }

    public virtual DbSet<ДепозитныеСчета> ДепозитныеСчетаs { get; set; }

    public virtual DbSet<ИсторияТранзакций> ИсторияТранзакцийs { get; set; }

    public virtual DbSet<Карта> Картаs { get; set; }

    public virtual DbSet<КарточныйПул> КарточныйПулs { get; set; }

    public virtual DbSet<Клиент> Клиентs { get; set; }

    public virtual DbSet<Кредит> Кредитs { get; set; }

    public virtual DbSet<Счет> Счетs { get; set; }

    public virtual DbSet<СчетаКлиентов> СчетаКлиентовs { get; set; }

    public virtual DbSet<Транзакция> Транзакцияs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-KSEH8OL\\SQLEXPRESS;Database=Nexus;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ВидКредита>(entity =>
        {
            entity.HasKey(e => e.КодПродукта).HasName("PK_ВидКредита");

            entity.ToTable("Вид кредита");

            entity.Property(e => e.КодПродукта).HasColumnName("Код продукта");
            entity.Property(e => e.Активный).HasDefaultValue(true);
            entity.Property(e => e.БазоваяСтавка)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Базовая ставка");
            entity.Property(e => e.ДатаСоздания)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Дата создания");
            entity.Property(e => e.Категория)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.МаксимальнаяСумма)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Максимальная сумма");
            entity.Property(e => e.МаксимальныйСрок).HasColumnName("Максимальный срок");
            entity.Property(e => e.МинимальнаяСумма)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Минимальная сумма");
            entity.Property(e => e.МинимальныйСрок).HasColumnName("Минимальный срок");
            entity.Property(e => e.Название)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.НеобходимоСтрахование).HasColumnName("Необходимо страхование");
            entity.Property(e => e.Описание)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ГрафикПлатежей>(entity =>
        {
            entity.HasKey(e => e.НомерПлатежа).HasName("PK_ГрафикПлатежей");

            entity.ToTable("График платежей");

            entity.Property(e => e.НомерПлатежа).HasColumnName("Номер платежа");
            entity.Property(e => e.ДатаОплаты).HasColumnName("Дата оплаты");
            entity.Property(e => e.ДатаПлатежа).HasColumnName("Дата платежа");
            entity.Property(e => e.НомерДоговора)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер договора");
            entity.Property(e => e.ОсновнойДолг)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Основной долг");
            entity.Property(e => e.Проценты).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Ожидается");
            entity.Property(e => e.СуммаПлатежа)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Сумма платежа");

            entity.HasOne(d => d.НомерДоговораNavigation).WithMany(p => p.ГрафикПлатежейs)
                .HasForeignKey(d => d.НомерДоговора)
                .HasConstraintName("FK_ГрафикПлатежей_Кредит");
        });

        modelBuilder.Entity<Депозит>(entity =>
        {
            entity.HasKey(e => e.НомерДепозитногоДоговора);

            entity.ToTable("Депозит", tb => tb.HasTrigger("ТР_Закрытие_счетов_и_блокировка_карт"));

            entity.Property(e => e.НомерДепозитногоДоговора)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("(concat('DP',format(getdate(),'yyyyMMdd'),right('000000'+CONVERT([varchar](6),abs(checksum(newid()))%(1000000)),(6)),right('00'+CONVERT([varchar](2),abs(checksum(newid()))%(100)),(2))))")
                .IsFixedLength()
                .HasColumnName("Номер депозитного договора");
            entity.Property(e => e.Валюта)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("RUB")
                .IsFixedLength();
            entity.Property(e => e.ДатаЗакрытия).HasColumnName("Дата закрытия");
            entity.Property(e => e.ДатаОткрытия)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Дата открытия");
            entity.Property(e => e.ДатаСледующейКапитализации).HasColumnName("Дата следующей капитализации");
            entity.Property(e => e.МинимальныйСрок).HasColumnName("Минимальный срок");
            entity.Property(e => e.Название)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ПроцентНалога)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Процент налога");
            entity.Property(e => e.ПроцентнаяСтавка)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("Процентная ставка");
            entity.Property(e => e.СпособВыплаты)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Способ выплаты");
            entity.Property(e => e.СуммаДепозита)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Сумма депозита");
            entity.Property(e => e.ТипДепозита)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Тип депозита");
        });

        modelBuilder.Entity<ДепозитныеСчета>(entity =>
        {
            entity.HasKey(e => e.НомерДепозитногоДоговора).HasName("PK_ДепозитныеСчета");

            entity.ToTable("Депозитные счета");

            entity.Property(e => e.НомерДепозитногоДоговора)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер депозитного договора");
            entity.Property(e => e.НомерСчета)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер счета");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Активен");

            entity.HasOne(d => d.НомерДепозитногоДоговораNavigation).WithOne(p => p.ДепозитныеСчета)
                .HasForeignKey<ДепозитныеСчета>(d => d.НомерДепозитногоДоговора)
                .HasConstraintName("FK_ДепозитныеСчета_Депозит");

            entity.HasOne(d => d.НомерСчетаNavigation).WithMany(p => p.ДепозитныеСчетаs)
                .HasForeignKey(d => d.НомерСчета)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ДепозитныеСчета_Счет");
        });

        modelBuilder.Entity<ИсторияТранзакций>(entity =>
        {
            entity.HasKey(e => e.КодТранзакции).HasName("PK_ИсторияТранзакций");

            entity.ToTable("История транзакций", tb =>
                {
                    tb.HasTrigger("ТР_Проверка_блокировки_счетов_при_переводе");
                    tb.HasTrigger("ТР_Проверка_суммы_транзакции");
                });

            entity.Property(e => e.КодТранзакции)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Код транзакции");
            entity.Property(e => e.БикБанкаОтправителя)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("БИК банка отправителя");
            entity.Property(e => e.БикБанкаПолучателя)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("БИК банка получателя");
            entity.Property(e => e.ВнешнийСчетОтправителя)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Внешний счет отправителя");
            entity.Property(e => e.ВнешнийСчетПолучателя)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Внешний счет получателя");
            entity.Property(e => e.ДатаВыполнения)
                .HasColumnType("datetime")
                .HasColumnName("Дата выполнения");
            entity.Property(e => e.ДатаСоздания)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Дата создания");
            entity.Property(e => e.Комиссия).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Сумма).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.СчетОтправителя)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Счет отправителя");
            entity.Property(e => e.СчетПолучателя)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Счет получателя");

            entity.HasOne(d => d.КодТранзакцииNavigation).WithOne(p => p.ИсторияТранзакций)
                .HasForeignKey<ИсторияТранзакций>(d => d.КодТранзакции)
                .HasConstraintName("FK_ИсторияТранзакций_Транзакция");

            entity.HasOne(d => d.СчетОтправителяNavigation).WithMany(p => p.ИсторияТранзакцийСчетОтправителяNavigations)
                .HasForeignKey(d => d.СчетОтправителя)
                .HasConstraintName("FK_ИсторияТранзакций_СчетОтправителя");

            entity.HasOne(d => d.СчетПолучателяNavigation).WithMany(p => p.ИсторияТранзакцийСчетПолучателяNavigations)
                .HasForeignKey(d => d.СчетПолучателя)
                .HasConstraintName("FK_ИсторияТранзакций_СчетПолучателя");
        });

        modelBuilder.Entity<Карта>(entity =>
        {
            entity.HasKey(e => e.НомерКарты);

            entity.ToTable("Карта");

            entity.Property(e => e.НомерКарты)
                .HasMaxLength(16)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер карты");
            entity.Property(e => e.ДатаВыпуска).HasColumnName("Дата выпуска");
            entity.Property(e => e.КодБезопасности)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Код безопасности");
            entity.Property(e => e.Лимит).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.ПлатежнаяСистема)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Платежная система");
            entity.Property(e => e.СрокДействия)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Срок действия");
            entity.Property(e => e.ТипКарты)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Тип карты");
        });

        modelBuilder.Entity<КарточныйПул>(entity =>
        {
            entity.HasKey(e => e.НомерКарты).HasName("PK_КарточныйПул");

            entity.ToTable("Карточный пул");

            entity.Property(e => e.НомерКарты)
                .HasMaxLength(16)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер карты");
            entity.Property(e => e.НомерСчета)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер счета");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.НомерКартыNavigation).WithOne(p => p.КарточныйПул)
                .HasForeignKey<КарточныйПул>(d => d.НомерКарты)
                .HasConstraintName("FK_КарточныйПул_Карта");

            entity.HasOne(d => d.НомерСчетаNavigation).WithMany(p => p.КарточныйПулs)
                .HasForeignKey(d => d.НомерСчета)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_КарточныйПул_Счет");
        });

        modelBuilder.Entity<Клиент>(entity =>
        {
            entity.ToTable("Клиент", tb =>
                {
                    tb.HasTrigger("ТР_Закрытие_депозитных_счетов_при_неактивности_клиента");
                    tb.HasTrigger("ТР_Закрытие_счетов_при_удалении_клиента");
                });

            entity.HasIndex(e => e.Инн, "CK_Клиент_ИНН").IsUnique();

            entity.HasIndex(e => new { e.СерияПаспорта, e.НомерПаспорта }, "CK_Клиент_Паспорт").IsUnique();

            entity.HasIndex(e => e.Снилс, "CK_Клиент_СНИЛС").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.АдресПроживания)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Адрес проживания");
            entity.Property(e => e.АдресРегистрации)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Адрес регистрации");
            entity.Property(e => e.ДатаВыдачиПаспорта).HasColumnName("Дата выдачи паспорта");
            entity.Property(e => e.ДатаРегистрацииВБанке)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Дата регистрации в банке");
            entity.Property(e => e.ДатаРождения).HasColumnName("Дата рождения");
            entity.Property(e => e.ДополнительныеСведения)
                .IsUnicode(false)
                .HasColumnName("Дополнительные сведения");
            entity.Property(e => e.Имя)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Инн)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ИНН");
            entity.Property(e => e.ИсточникДохода)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Источник дохода");
            entity.Property(e => e.КемВыданПаспорт)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Кем выдан паспорт");
            entity.Property(e => e.МестоРождения)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Место рождения");
            entity.Property(e => e.НомерПаспорта)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер паспорта");
            entity.Property(e => e.НомерТелефона)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Номер телефона");
            entity.Property(e => e.Отчество)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Пол)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Работодатель)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.СемейноеПоложение)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Семейное положение");
            entity.Property(e => e.СерияПаспорта)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Серия паспорта");
            entity.Property(e => e.Снилс)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("СНИЛС");
            entity.Property(e => e.СтатусКлиента)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Активен")
                .HasColumnName("Статус клиента");
            entity.Property(e => e.УровеньДохода)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Уровень дохода");
            entity.Property(e => e.Фамилия)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Кредит>(entity =>
        {
            entity.HasKey(e => e.НомерДоговора);

            entity.ToTable("Кредит", tb => tb.HasTrigger("ТР_Отмена_платежей_при_досрочном_погашении_кредита"));

            entity.Property(e => e.НомерДоговора)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер договора");
            entity.Property(e => e.ВидКредита).HasColumnName("Вид кредита");
            entity.Property(e => e.ДатаВыдачи)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("Дата выдачи");
            entity.Property(e => e.ДатаЗакрытия).HasColumnName("Дата закрытия");
            entity.Property(e => e.НомерСчета)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер счета");
            entity.Property(e => e.Ставка).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Активен");
            entity.Property(e => e.Сумма).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.ЦельКредита)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Цель кредита");

            entity.HasOne(d => d.ВидКредитаNavigation).WithMany(p => p.Кредитs)
                .HasForeignKey(d => d.ВидКредита)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Кредит_ВидКредита");

            entity.HasOne(d => d.НомерСчетаNavigation).WithMany(p => p.Кредитs)
                .HasForeignKey(d => d.НомерСчета)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Кредит_Счет");
        });

        modelBuilder.Entity<Счет>(entity =>
        {
            entity.HasKey(e => e.НомерСчета);

            entity.ToTable("Счет");

            entity.Property(e => e.НомерСчета)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("(concat('4',right('0000'+CONVERT([varchar](4),abs(checksum(newid()))%(10000)),(4)),right('000000000'+CONVERT([varchar](9),abs(checksum(newid()))%(1000000000)),(9)),right('00000000000'+CONVERT([varchar](2),abs(checksum(newid()))%(100)),(2))))")
                .IsFixedLength()
                .HasColumnName("Номер счета");
            entity.Property(e => e.БикБанка)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("БИК банка");
            entity.Property(e => e.ДатаЗакрытия).HasColumnName("Дата закрытия");
            entity.Property(e => e.ДатаОткрытия)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Дата открытия");
            entity.Property(e => e.КодФилиала)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Код филиала");
            entity.Property(e => e.НаименованиеСчета)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Наименование счета");
            entity.Property(e => e.ТипСчета)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Тип счета");
        });

        modelBuilder.Entity<СчетаКлиентов>(entity =>
        {
            entity.HasKey(e => e.НомерСчета).HasName("PK_СчетаКлиентов");

            entity.ToTable("Счета клиентов");

            entity.Property(e => e.НомерСчета)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Номер счета");
            entity.Property(e => e.Статус)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.КлиентNavigation).WithMany(p => p.СчетаКлиентовs)
                .HasForeignKey(d => d.Клиент)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_СчетаКлиентов_Клиент");

            entity.HasOne(d => d.НомерСчетаNavigation).WithOne(p => p.СчетаКлиентов)
                .HasForeignKey<СчетаКлиентов>(d => d.НомерСчета)
                .HasConstraintName("FK_СчетаКлиентов_Счет");
        });

        modelBuilder.Entity<Транзакция>(entity =>
        {
            entity.HasKey(e => e.КодТранзакции);

            entity.ToTable("Транзакция");

            entity.Property(e => e.КодТранзакции)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Код транзакции");
            entity.Property(e => e.Направление)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ТипТранзакции)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Тип транзакции");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
