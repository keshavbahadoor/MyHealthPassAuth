using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHealthPassAuth.Entities
{
    [Table("blacklist_log")]
    public class BlackListLog
    {
        [Key]
        [Column("blacklist_id")]
        public int BlackListID { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }

        [Column("user_agent")]
        public string UserAgent { get; set; }

        [Column("flag_date")]
        public Nullable<DateTime> FlagDate { get; set; }
    }
}
