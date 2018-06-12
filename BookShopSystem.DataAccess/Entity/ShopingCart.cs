namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShopingCart")]
    public partial class ShopingCart
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public long UserId { get; set; }

        public int Quantity { get; set; }
    }
}
