using MyHealthPassAuth.Repository;
using MyHealthPassAuth.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealthPassAuthExampleInMemoryDb
{
    public class PopulateData
    {
        public static void Populate(MainDbContext context)
        {
            var unitOfWork = new UnitOfWork(context);

            unitOfWork.LocationRepository.Add(new Location
            {
                LocationID = 1,
                Country = "Trinidad",
                Region = "Chaguanas"
            });
            unitOfWork.LocationRepository.Add(new Location
            {
                LocationID = 2,
                Country = "Trinidad",
                Region = "Arima"
            });

            unitOfWork.AuthConfigRepository.Add( new AuthorizationConfig
            {
                AuthorizationConfigID = 1,
                PasswordLengthMax = 10,
                PasswordLengthMin = 1,
                PasswordAllowedUppercaseCount = 1,
                PasswordAllowedLowercaseCount = 1,
                PasswordAllowedDigitCount = 1,
                PasswordAllowedSpecialCharCount = 1,
                LoginAccountLockAttempts = 3,
                BruteForceBlockAttempts = 13,
                BruteForceIdentificationSeconds = 600,
                BruteForceBlockSeconds = 600,
                LocationID = 1
            });

            unitOfWork.UserRepository.Add(new User
            {
                UserID = 1, 
                Username = "keshav",
                Password = "password",
                FailedLoginAttempts = 0, 
                LocationID = 1, 
                AccountLocked = (int) AccountLockedEnum.NORMAL
            });

            unitOfWork.SaveChanges(); 
        }
    }
}
