using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHealthPassAuth.Entities
{
    [Table("authentication_log")]
    public class AuthenticationLog
    {
        [Key]
        [Column("authentication_log_id")]
        public int AuthenticationLogID { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }

        [Column("request_data")]
        public string RequestData { get; set; }

        [Column("user_agent")]
        public string UserAgent { get; set; }

        [Column("insert_date")]
        public Nullable<DateTime> InsertDate { get; set; }
    }
}
