using GasStationApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationApp.Tests
{
    public class UserTests
    {
        [Fact]
        public void LogIn_CorrectCredentials_ReturnsTrue()
        {
            var admin = new Administrator("admin", "Admin123");
            var result = admin.LogIn("admin", "Admin123");
            Assert.True(result);
        }

        [Fact]
        public void LogIn_WrongPassword_ReturnsFalse()
        {
            var admin = new Administrator("admin", "Admin123");
            var result = admin.LogIn("admin", "wrongpass");
            Assert.False(result);
        }

        [Fact]
        public void LogIn_ThreeFailedAttempts_BlocksUser()
        {
            var admin = new Administrator("admin", "Admin123");
            admin.LogIn("admin", "wrong");
            admin.LogIn("admin", "wrong");
            admin.LogIn("admin", "wrong");
            Assert.True(admin.IsBlocked);
        }

        [Fact]
        public void BlockDurationMinutes_Is5()
        {
           Assert.Equal(5, User.BlockDurationMinutes);
        }

        [Fact]
        public void IncrementFailedAttempts_IncreasesCounter()
        {
            var admin = new Administrator("admin", "Admin123");
            admin.IncrementFailedAttempts();
            Assert.Equal(1, admin.FailedAttempts);
        }

        [Fact]
        public void ResetFailedAttempts_ResetsToZero()
        {
            var admin = new Administrator("admin", "Admin123");
            admin.IncrementFailedAttempts();
            admin.IncrementFailedAttempts();
            admin.ResetFailedAttempts();
            Assert.Equal(0, admin.FailedAttempts);
        }
    }
}