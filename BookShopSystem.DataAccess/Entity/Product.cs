namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Author { get; set; }

        public int PublisherId { get; set; }

        public long? ProfessionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime PublishDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ISBN { get; set; }

        public int WordsCount { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public string ContentDescription { get; set; }

        public string AurhorDescription { get; set; }

        public string EditorComment { get; set; }

        public string TOC { get; set; }

        public long CategoryId { get; set; }

        public int? Quantity { get; set; }

        public long ClickCount { get; set; }
    }
}
