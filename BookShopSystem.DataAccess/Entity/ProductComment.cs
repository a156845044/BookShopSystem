namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductComment")]
    public partial class ProductComment
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        [StringLength(50)]
        public string OrderNum { get; set; }

        public long ProductId { get; set; }

        [Column(TypeName = "text")]
        public string Contents { get; set; }

        public int StateFlag { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
