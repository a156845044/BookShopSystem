namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Profession")]
    public partial class Profession
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
