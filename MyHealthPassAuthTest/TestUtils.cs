using MyHealthPassAuth.Entities;
using MyHealthPassAuth.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuthTest
{
    /// <summary>
    /// Utility class to support unit test and more specifically, 
    /// In memory data object creation. 
    /// </summary>
    public static class TestUtils
    {

        /// <summary>
        /// Adds a auth config to data context. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="passLengthMax"></param>
        /// <param name="passLengthMin"></param>
        /// <param name="upperCase"></param>
        /// <param name="lowerCase"></param>
        /// <param name="digit"></param>
        /// <param name="specialChars"></param>
        /// <param name="maxUserSessionSec"></param>
        /// <param name="loginAttemptCount"></param>
        /// <param name="bruteForceBlockAttempts"></param>
        /// <param name="bruteForceIdSeconds"></param>
        /// <param name="bruteForceBlockSec"></param>
        /// <param name="locationid"></param>
        public static void AddAuthConfig(MainDbContext context, int id, int passLengthMax, int passLengthMin,
            int upperCase, int lowerCase, int digit, int specialChars,
            int maxUserSessionSec, int loginAttemptCount, int bruteForceBlockAttempts,
            int bruteForceIdSeconds, int bruteForceBlockSec, int locationid)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.AuthConfigRepository.Add(new AuthorizationConfig
            {
                AuthorizationConfigID = id,
                PasswordLengthMax = passLengthMax,
                PasswordLengthMin = passLengthMin,
                PasswordAllowedUppercaseCount = upperCase,
                PasswordAllowedLowercaseCount = lowerCase,
                PasswordAllowedDigitCount = digit,
                PasswordAllowedSpecialCharCount = specialChars,
                MaxUserSessionSeconds = maxUserSessionSec,
                LoginAccountLockAttempts = loginAttemptCount,
                BruteForceBlockAttempts = bruteForceBlockAttempts,
                BruteForceIdentificationSeconds = bruteForceIdSeconds,
                BruteForceBlockSeconds = bruteForceBlockSec,
                LocationID = locationid
            });
            unitOfWork.SaveChanges();
        }


        /// <summary>
        /// Adds a specified location to the database using the given database context. 
        /// Data is committed 
        /// </summary>
        public static void AddAuthenticationLog(MainDbContext context, int id, string ipAddress, 
            string requestdata, string useragent, DateTime insertDate, string resultMessage)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.AuthLogRepository.Add(new AuthenticationLog
            {
                AuthenticationLogID = id,
                IpAddress = ipAddress,
                RequestData = requestdata,
                UserAgent = useragent,
                InsertDate = insertDate,
                ResultMessage = resultMessage
            });
            unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Adds a specified location to the database using the given database context. 
        /// Data is committed 
        /// </summary>
        public static void AddLocation(MainDbContext context, int id, string country, string region)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.LocationRepository.Add(new Location
            {
                LocationID = id,
                Country = country,
                Region = region
            });
            unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Adds a specified user to the database using the given database context. 
        /// Data is committed 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="failedAttempts"></param>
        /// <param name="locationid"></param>
        /// <param name="failedLoginDate"></param>
        public static void AddUserToRepository(MainDbContext context, int id, string username,
            string password, int failedAttempts, int locationid, DateTime failedLoginDate)
        {
            var unitOfWork = new UnitOfWork(context);
            unitOfWork.UserRepository.Add(new User
            {
                UserID = id,
                Username = username,
                FailedLoginAttempts = failedAttempts,
                FailedLoginDateTime = failedLoginDate,
                LocationID = locationid,
                Password = password
            });
            unitOfWork.SaveChanges();
        }
    }
}
