namespace Owt.Data.Contacts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Contacts", Schema = "op")]
    public class Contact : IDeletable, IOwnable
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key]
        public int Id { get; private set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string MobilePhoneNumber { get; set; }

        public bool Deleted { get; set; }

        public List<ContactSkill> ContactSkills { get; set; }

        public string CreatedBy { get; set; }
    }
}