using AutoMapper;
using FutureProjects.API.Controllers.UserControllers;
using FutureProjects.Application.Abstractions.IServices;
using FutureProjects.Application.Mappers;
using FutureProjects.Domain.Entities.DTOs;
using FutureProjects.Domain.Entities.Models;
using Moq;

namespace FutureProject.Test.Tests
{
    public class UnitTest1
    {
        private readonly Mock<IUserService> _userservice = new Mock<IUserService>();
        MapperConfiguration? _mockMapper = new MapperConfiguration(conf =>
        {
            conf.AddProfile(new MapperProfile());
        }
        );

        public static IEnumerable<object[]> GetUserFromDataGenerator()
        {
            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Alex",
                    Email = "alex@gmail.com",
                    Password = "123",
                    Login = "alex123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Alex",
                    Email = "alex@gmail.com",
                    Password = "123",
                    Login = "alex123",
                    Role = "Admin"
                }
            };
            yield return new object[]
{
                new UserDTO()
                {
                    Name = "Patric",
                    Email = "patric@gmail.com",
                    Password = "123",
                    Login = "patric123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Patric",
                    Email = "patric@gmail.com",
                    Password = "123",
                    Login = "patric123",
                    Role = "Admin"
                }
            }; 
        }


        [Theory]
        [MemberData(nameof(GetUserFromDataGenerator), MemberType = typeof(UnitTest1))]
        public async void Create_User_test(UserDTO inputUser, User expectedResult)
        {
            var myMapper = _mockMapper.CreateMapper();
            
            var result = myMapper.Map<User>(inputUser);
            
            _userservice.Setup(x=> x.Create(It.IsAny<UserDTO>())).ReturnsAsync(result);

            var controller = new UserCreateController(_userservice.Object);

            var createUser = await controller.CreateUser(inputUser);
            Assert.NotNull(createUser);
            Assert.True(CompareModels(expectedResult, createUser));
        }
        public static bool CompareModels(User inputUser, User user)
        {
            if (inputUser.Name == user.Name && inputUser.Email == user.Email && inputUser.Password == user.Password
            && inputUser.Login == user.Login && inputUser.Role == user.Role) return true;
            return false;
        }
    }
}