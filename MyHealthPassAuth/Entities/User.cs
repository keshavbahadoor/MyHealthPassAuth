using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHealthPassAuth.Entities
{
    public enum AccountLockedEnum
    {
        NORMAL = 0, 
        LOCKED = 1
    }

    [Table("user")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("failed_login_attempts")]
        public int FailedLoginAttempts { get; set; }

        [Column("failed_login_date_time")]
        public Nullable<DateTime> FailedLoginDateTime { get; set; }

        [Column("account_locked")]
        public int AccountLocked { get; set; }

        [Column("location_id")]
        public int LocationID { get; set; }
    }
}
