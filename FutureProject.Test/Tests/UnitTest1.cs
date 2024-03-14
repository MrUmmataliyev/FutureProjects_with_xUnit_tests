using AutoMapper;
using FutureProjects.API.Controllers.UserControllers;
using FutureProjects.Application.Abstractions.IServices;
using FutureProjects.Application.Mappers;
using FutureProjects.Domain.Entities.DTOs;
using FutureProjects.Domain.Entities.Models;
using Microsoft.AspNetCore.Mvc;
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
        [Fact]
        public async Task GetUserById_Test()
        {
            var user = new User()
            {
                Id = 1,
                Name = "Patric",
                Email = "patric@gmail.com",
                Password = "123",
                Login = "patric123",
                Role = "Admin"
            };
            _userservice.Setup(x => x.GetById(1)).ReturnsAsync(user);
            var controller = new UserReadController(_userservice.Object);
            var result = await controller.GetById(1);



            Assert.True(CompareModels(result, user));
        }
        [Fact]
        public async void UpdateUser_Test()
        {
            int Id = 1;
            var user = new UserDTO()
            {
                
                Name = "Patric",
                Email = "patric@gmail.com",
                Password = "123",
                Login = "patric123",
                Role = "Admin"
            };
            string result = "";
            _userservice.Setup(x => x.Update(1, user)).ReturnsAsync(result);
             
            var controller = new UserUpdateController(_userservice.Object);

            var natija = await controller.Update(Id, user);
            
            Assert.Equal(result, natija);
        }
        [Fact]
        public async void DeleteUser()
        {
            int id = 3;
            var user = new User()
            {
                Id = 3,
                Name = "Patric",
                Email = "patric@gmail.com",
                Password = "123",
                Login = "patric123",
                Role = "Admin"
            };
            bool result = true;

            _userservice.Setup(x=> x.DeleteById(3)).ReturnsAsync(result);
            var controller = new UserDeleteController(_userservice.Object);
            var natija = await controller.DeleteUserById(id);
            Assert.Equal(result, natija);
        }


    }
}