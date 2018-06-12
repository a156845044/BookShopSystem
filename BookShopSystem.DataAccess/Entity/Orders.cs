namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNo { get; set; }

        [StringLength(50)]
        public string Contacts { get; set; }

        [StringLength(50)]
        public string Tel { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalPrice { get; set; }

        public int StateFlag { get; set; }

        public long UserId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
