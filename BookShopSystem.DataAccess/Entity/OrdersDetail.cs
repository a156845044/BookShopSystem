namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdersDetail")]
    public partial class OrdersDetail
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public int Quantity { get; set; }

        public long OrderId { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
    }
}
