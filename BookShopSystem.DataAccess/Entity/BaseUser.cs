namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaseUser")]
    public partial class BaseUser
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string Account { get; set; }

        [StringLength(50)]
        public string Pwd { get; set; }

        public int RoleType { get; set; }

        public long? ProfessionId { get; set; }
    }
}
