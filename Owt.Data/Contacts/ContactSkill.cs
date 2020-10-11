namespace Owt.Data.Contacts
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ContactsSkills", Schema = "op")]
    public class ContactSkill : IDeletable, IOwnable
    {
        [Key]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int Id { get; private set; }

        public int ContactId { get; set; }

        public int SkillId { get; set; }

        public int ExpertiseLevelId { get; set; }

        public bool Deleted { get; set; }

        public string CreatedBy { get; set; }
    }
}