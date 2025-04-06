using Xunit;
using BCrypt.Net;

namespace LightInvest.Tests.Controllers
{
    public class PasswordEncryptionTests
    {
        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            
            string plainPassword = "Password123!";

            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            
            Assert.False(string.IsNullOrEmpty(hashedPassword));
            Assert.StartsWith("$2", hashedPassword); 
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            
            string plainPassword = "Password123!";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            
            bool isValid = BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);

            
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            
            string originalPassword = "Password123!";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(originalPassword);
            string wrongPassword = "WrongPass123";

            
            bool isValid = BCrypt.Net.BCrypt.Verify(wrongPassword, hashedPassword);

            
            Assert.False(isValid);
        }

        [Fact]
        public void ShouldRehashPassword_IfNotBCrypt()
        {
            
            string oldPasswordPlain = "OldPassword!";
            string storedPassword = "123456789"; 

           
            bool isHashed = storedPassword.StartsWith("$2a$") ||
                            storedPassword.StartsWith("$2b$") ||
                            storedPassword.StartsWith("$2y$");

            
            Assert.False(isHashed);
        }

        [Fact]
        public void ShouldNotRehashPassword_IfAlreadyHashed()
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123!");

           
            bool isHashed = hashedPassword.StartsWith("$2a$") ||
                            hashedPassword.StartsWith("$2b$") ||
                            hashedPassword.StartsWith("$2y$");

            
            Assert.True(isHashed);
        }
    }
}
