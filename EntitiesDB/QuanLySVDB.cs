using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BTQuanLySinhVien.EntitiesDB
{
    public partial class QuanLySVDB : DbContext
    {
        public QuanLySVDB()
            : base("name=QLSV")
        {
        }

        public virtual DbSet<Khoa> Khoas { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaSV)
                .IsUnicode(false);
        }
    }
}
