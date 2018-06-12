namespace BookShopSystem.DataAccess.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileUpload")]
    public partial class FileUpload
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string FileName { get; set; }

        [StringLength(500)]
        public string FilePath { get; set; }

        [StringLength(50)]
        public string MediaType { get; set; }

        [StringLength(50)]
        public string SourceId { get; set; }

        public int SourceType { get; set; }

        public bool IsDefault { get; set; }
    }
}
