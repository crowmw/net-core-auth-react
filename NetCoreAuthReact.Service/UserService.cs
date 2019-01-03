using AutoMapper;
using NetCoreAuthReact.Data;
using NetCoreAuthReact.Data.Interfaces;
using NetCoreAuthReact.Data.Models;
using NetCoreAuthReact.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace NetCoreAuthReact.Service
{
    public class UserService : IUser
    {
        private readonly NetCoreAuthReactDbContext _ctx;
        private readonly UserManager<User> _usrMgr;
        private readonly SignInManager<User> _signInMgr;
        private readonly IMapper _mapper;

        public UserService(NetCoreAuthReactDbContext ctx, UserManager<User> usrMgr, SignInManager<User> signInMgr, IMapper mapper)
        {
            _ctx = ctx;
            _usrMgr = usrMgr;
            _signInMgr = signInMgr;
            _mapper = mapper;
        }

        public async Task<UserDto> Login(string userNameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Password is required");

            if (await _usrMgr.FindByNameAsync(userNameOrEmail) == null)
                throw new InvalidOperationException($"Username {userNameOrEmail} does not exists. Try Register first.");

            var user = await _usrMgr.FindByEmailAsync(userNameOrEmail);

            var signInResult = new SignInResult();
            if(user != null)
            {
                var passwordCheck = await _signInMgr.CheckPasswordSignInAsync(user, password, false);
                if (!passwordCheck.Succeeded)
                    throw new InvalidOperationException("Incorrect password.");

                signInResult = await _signInMgr.PasswordSignInAsync(user.UserName, password, false, false);
            } else
            {
                user = await _usrMgr.FindByNameAsync(userNameOrEmail);
                var passwordCheck = await _signInMgr.CheckPasswordSignInAsync(user, password, false);
                if (!passwordCheck.Succeeded)
                    throw new InvalidOperationException("Incorrect password.");

                signInResult = await _signInMgr.PasswordSignInAsync(userNameOrEmail, password, false, false);
            }

            if (signInResult.Succeeded)
            {
                return _mapper.Map<UserDto>(user);
            }

            throw new InvalidOperationException("Login failed.");
        }

        public async Task<UserDto> Register(string userName, string email, string password, string passwordConfirm)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Password is required");

            if (password != passwordConfirm)
                throw new InvalidOperationException("Passwords do not match");

            if (await _usrMgr.FindByNameAsync(userName) != null)
                throw new InvalidOperationException($"Username {userName} is already taken.");

            if (await _usrMgr.FindByEmailAsync(email) != null)
                throw new InvalidOperationException($"Email address {email} is already registered.");

            var user = new User
            {
                UserName = userName,
                Email = email
            };

            var userResult = await _usrMgr.CreateAsync(user, password);
            var roleResult = await _usrMgr.AddToRoleAsync(user, "User");

            if (!userResult.Succeeded || !roleResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to register user");
            }

            user = await _usrMgr.FindByNameAsync(userName);

            return _mapper.Map<UserDto>(user);
        }

        public async Task Logout()
        {
            await _signInMgr.SignOutAsync();
        } 
    }
}
