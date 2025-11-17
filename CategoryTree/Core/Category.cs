using System.ComponentModel.DataAnnotations;

namespace CategoryTree.Core
{
    public class Category : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

#nullable disable
        [MaxLength(100)]        
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        
        public Guid? ParentId { get; set; }

        public virtual Category? Parent { get; set; }

        public virtual IList<Category>? SubCategories { get; set; }
        #nullable enable
    }
}
