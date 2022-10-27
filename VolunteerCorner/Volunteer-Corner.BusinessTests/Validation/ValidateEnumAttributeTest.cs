using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Enums;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Business.Validation;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities.Identity;


namespace Volunteer_Corner.BusinessTests.Validation
{

    [TestFixture]
    public class ValidateEnumAttributeTest
    {

        private ValidationContext _validationContext;
        private ValidationAttribute _validationAttribute;
        private Mock<ValidateEnumAttribute> _validateEnumAttribute;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

            // This method is called BEFORE ANY OF THE tests will be launched
            // Do common initializing stuff
        }

        [SetUp]
        public void SetUp()
        {

            // This method is called BEFORE EACH OF THE tests will be launched
            // Do initializing stuff that needs to be applied before each test

        }
         
        [TearDown]
        public void TearDown()
        {
            // This method is called AFTER EVERY test had been launched
            // Do all stuff that needs to be applied after unit tests will end its work 

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // This method is called AFTER ANY OF THE tests had been launched
            // Do all stuff that needs to be applied after unit tests will end its work 
        }


        // Please use Triple A convention (Arrange, Act, Assert)
        // Naming convention
        // <MethodName>_<WhenSomeActionOccurs>_<DoSomeExpectedResult>

        //[Test]
        //public async Task IsValid_WhenNotAnEnum_ThrowValidationMessage()
        //{
        //    //Arrange
             
        //    User user = new User();

        //    _validateEnumAttribute.Setup(m => m.IsValid(It.IsAny<Object>()))
        //        .Returns(ValidationResult());

        //    //Act
        //    var action = async () =>
        //    {
        //        _validateEnumAttribute.Object.IsValid(user);
        //    };

        //    // Assert

        //   await action.Should().NotThrowAsync();
        //}
    }
}
