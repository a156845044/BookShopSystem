namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAddress")]
    public partial class UserAddress
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        [StringLength(50)]
        public string ProvinceCode { get; set; }

        [StringLength(50)]
        public string CityCode { get; set; }

        [StringLength(50)]
        public string DistrictCode { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Contacts { get; set; }

        [StringLength(50)]
        public string Tel { get; set; }

        public bool IsDefault { get; set; }
    }
}
