using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartMapper.Core;
using System;

namespace SmartMapper.Tests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void FluentMapping_Should_Map_All_Properties_Correctly()
        {
            // Fluent Mapping Konfigürasyonu
            TypeAdapterConfig<UserDto, UserEntity>
                .ForType()
                // Özel Alanlar
                .ForMember(dest => dest.EmailAddress, src => src.Email)
                .ForMember(dest => dest.Status, src => src.IsActive ? "Active" : "Inactive")
                .ForMember(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
                .Register();

            // Test verisi
            var userDto = new UserDto
            {
                UserID = 101,
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                Email = "ahmet@example.com",
                IsActive = true
            };

            // Act
            var userEntity = userDto.Adapt<UserEntity>();

            // Assert
            Assert.AreEqual("Ahmet Yılmaz", userEntity.FullName); 
        }
    }

    // Test Sınıfları
    public class UserDto
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
    }

    public class UserEntity
    {
        public int UserId { get; set; } // UserID -> UserId (case-insensitive)
        public string FullName { get; set; } // FirstName + LastName (otomatik)
        public string EmailAddress { get; set; } // Özel eşleme
        public DateTime CreatedAt { get; set; } // Otomatik eşleme
        public string Status { get; set; } // Özel dönüşüm: IsActive -> "Active"/"Inactive"
        public string PasswordHash { get; set; } // Ignore edilecek
    }
}