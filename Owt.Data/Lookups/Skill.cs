namespace Owt.Data.Lookups
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Skills", Schema = "cfg")]
    public class Skill : ILookup
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [Key]
        public int Id { get; private set; }

        public string Name { get; set; }

        public bool Deleted { get; set; }
    }
}