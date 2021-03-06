﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyHealthPassAuth.Entities
{
    [Table("authorization_config")]
    public class AuthorizationConfig
    {
        [Key]
        [Column("authorizatoin_config_id")]
        public int AuthorizationConfigID { get; set; }

        [Column("password_length_max")]
        public int PasswordLengthMax { get; set; }

        [Column("password_length_min")]
        public int PasswordLengthMin { get; set; }

        [Column("password_allowed_uppercase_count")]
        public int PasswordAllowedUppercaseCount { get; set; }

        [Column("password_allowed_lowercase_count")]
        public int PasswordAllowedLowercaseCount { get; set; }

        [Column("password_allowed_digit_count")]
        public int PasswordAllowedDigitCount { get; set; }

        [Column("password_allowed_special_char_count")]
        public int PasswordAllowedSpecialCharCount { get; set; }

        [Column("max_user_session_seconds")]
        public int MaxUserSessionSeconds { get; set; }

        /// <summary>
        /// Amount of attempts before a user account is to be locked 
        /// </summary>
        [Column("login_account_lock_attempts")]
        public int LoginAccountLockAttempts { get; set; }

        /// <summary>
        /// Amount of identified brute force attempts before client is blacklisted 
        /// </summary>
        [Column("brute_force_block_attempts")]
        public int BruteForceBlockAttempts { get; set; }

        /// <summary>
        /// Length of time in seconds to satisfy the criteria of an identified brute force attempt
        /// </summary>
        [Column("brute_force_identification_seconds")]
        public int BruteForceIdentificationSeconds { get; set; }

        /// <summary>
        /// Length of time in seconds that a client is blocked from accessing the system 
        /// </summary>
        [Column("brute_force_block_seconds")]
        public int BruteForceBlockSeconds { get; set; }

        [Column("location_id")]
        public int LocationID { get; set; }
        
    }
}
