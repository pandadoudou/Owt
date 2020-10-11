namespace Owt.Data
{
    using System.Data.Entity;

    using Owt.Data.Contacts;
    using Owt.Data.Lookups;

    public class OwtDbContext : DbContext
    {
        public OwtDbContext()
            : base("OwtDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OwtModelBuilder.Build(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<ContactSkill> ContactsSkills { get; set; }

        public DbSet<ExpertiseLevel> ExpertiseLevels { get; set; }
    }
}