namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Area")]
    public partial class Area
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string RegionName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string RegionCode { get; set; }

        [StringLength(50)]
        public string ParentCode { get; set; }

        [StringLength(20)]
        public string Longitude { get; set; }

        [StringLength(20)]
        public string Latitude { get; set; }

        [StringLength(20)]
        public string Levels { get; set; }
    }
}
