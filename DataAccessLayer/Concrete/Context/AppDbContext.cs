using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<About> About { get; set; }
        public DbSet<About_L> About_L { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public DbSet<Campaign_L> Campaign_L { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Category_L> Category_L { get; set; }
        public DbSet<CategoryBranch> CategoryBranch { get; set; }
        public DbSet<CategoryTeacher> CategoryTeacher { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<MemberTeacher> MemberTeacher { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<Option_L> Option_L { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Policy> Policy { get; set; }
        public DbSet<Policy_L> Policy_L { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Product_L> Product_L { get; set; }
        public DbSet<ProductOption> ProductOption { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<TeacherCategoryBranch> TeacherCategoryBranch { get; set; }
        public DbSet<TeacherCertification> TeacherCertification { get; set; }
        public DbSet<TeacherBranch> TeacherBranch { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<ProductFeature> ProductFeature { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<Skill_L> Skill_L { get; set; }
        public DbSet<TeacherSkill> TeacherSkill { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //About
            modelBuilder.Entity<About_L>()
                .HasKey(al => new { al.AboutId, al.LanguageId });

            modelBuilder.Entity<About_L>()
               .HasOne(al => al.About)
               .WithMany(a => a.About_L)
               .HasForeignKey(al => al.AboutId);

            modelBuilder.Entity<About_L>()
                .HasOne(x => x.Language)
                .WithMany(x => x.About_L)
                .HasForeignKey(x => x.LanguageId);

            //CAMPAIGN
            modelBuilder.Entity<Campaign_L>()
                .HasKey(cl => new { cl.CampaignId, cl.LanguageId });

            modelBuilder.Entity<Campaign_L>()
               .HasOne(cl => cl.Campaign)
               .WithMany(c => c.Campaign_L)
               .HasForeignKey(cl => cl.CampaignId);

            modelBuilder.Entity<Campaign_L>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Campaign_L)
                .HasForeignKey(x => x.LanguageId);

            //CATEGORY
            modelBuilder.Entity<Category_L>()
                .HasKey(cl => new { cl.CategoryId, cl.LanguageId });

            modelBuilder.Entity<Category_L>()
               .HasOne(cl => cl.Category)
               .WithMany(c => c.Category_L)
               .HasForeignKey(cl => cl.CategoryId);

            modelBuilder.Entity<Category_L>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Category_L)
                .HasForeignKey(x => x.LanguageId);

            //OPTION
            modelBuilder.Entity<Option_L>()
                .HasKey(ol => new { ol.OptionId, ol.LanguageId });

            modelBuilder.Entity<Option_L>()
               .HasOne(ol => ol.Option)
               .WithMany(o => o.Option_L)
               .HasForeignKey(ol => ol.OptionId);

            modelBuilder.Entity<Option_L>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Option_L)
                .HasForeignKey(x => x.LanguageId);

            //POLICY
            modelBuilder.Entity<Policy_L>()
                .HasKey(pl => new { pl.PolicyId, pl.LanguageId });

            modelBuilder.Entity<Policy_L>()
               .HasOne(pl => pl.Policy)
               .WithMany(p => p.Policy_L)
               .HasForeignKey(pl => pl.PolicyId);

            modelBuilder.Entity<Policy_L>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Policy_L)
                .HasForeignKey(x => x.LanguageId);

            //PRODUCT
            modelBuilder.Entity<Product_L>()
                .HasKey(pl => new { pl.ProductId, pl.LanguageId });

            modelBuilder.Entity<Product_L>()
               .HasOne(pl => pl.Product)
               .WithMany(p => p.Product_L)
               .HasForeignKey(pl => pl.ProductId);

            modelBuilder.Entity<Product_L>()
                .HasOne(pl => pl.Language)
                .WithMany(l => l.Product_L)
                .HasForeignKey(pl => pl.LanguageId);

            modelBuilder.Entity<Product>()
                .HasOne(x => x.ParentProduct)
                .WithMany(x => x.SubProducts)
                .HasForeignKey(x => x.ParentProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //SKILL
            modelBuilder.Entity<Skill_L>()
                .HasKey(cl => new { cl.SkillId, cl.LanguageId });

            modelBuilder.Entity<Skill_L>()
               .HasOne(cl => cl.Skill)
               .WithMany(c => c.Skill_L)
               .HasForeignKey(cl => cl.SkillId);

            modelBuilder.Entity<Skill_L>()
                .HasOne(x => x.Language)
                .WithMany(x => x.Skill_L)
                .HasForeignKey(x => x.LanguageId);

            modelBuilder.Entity<TeacherSkill>()
                .HasKey(x => new { x.TeacherId, x.SkillId });

            modelBuilder.Entity<TeacherSkill>()
                .HasOne(x => x.Teacher)
                .WithMany(t => t.TeacherSkills)
                .HasForeignKey(x => x.TeacherId);

            modelBuilder.Entity<TeacherSkill>()
                .HasOne(x => x.Skill)
                .WithMany(s => s.TeacherSkills)
                .HasForeignKey(x => x.SkillId);

            //TEACHER SKILL
            modelBuilder.Entity<TeacherSkill>()
                .HasKey(x => new { x.TeacherId, x.SkillId });

            modelBuilder.Entity<TeacherSkill>()
                .HasOne(x => x.Teacher)
                .WithMany(t => t.TeacherSkills)
                .HasForeignKey(x => x.TeacherId);

            modelBuilder.Entity<TeacherSkill>()
                .HasOne(x => x.Skill)
                .WithMany(s => s.TeacherSkills)
                .HasForeignKey(x => x.SkillId);
        }
    }
}
