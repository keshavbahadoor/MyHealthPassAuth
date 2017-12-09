using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHealthPassAuth.Entities
{
    [Table("location")]
    public class Location
    {
        [Key]
        [Column("location_id")]
        public int LocationID { get; set; }

        [Column("country")]
        public string Country { get; set; }

        [Column("region")]
        public string Region { get; set; }
    }
}
