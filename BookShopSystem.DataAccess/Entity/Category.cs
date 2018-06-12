namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }

        public long? ParentId { get; set; }

        public int Levels { get; set; }
    }
}
