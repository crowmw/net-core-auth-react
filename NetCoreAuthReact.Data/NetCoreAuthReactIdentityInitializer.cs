using NetCoreAuthReact.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAuthReact.Data
{
    public class NetCoreAuthReactIdentityInitializer
    {
        private UserManager<User> _userMgr;
        private RoleManager<IdentityRole> _roleMgr;

        public NetCoreAuthReactIdentityInitializer(UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr; 
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("admin");

            if(user == null)
            {
                //create admin role
                if(!(await _roleMgr.RoleExistsAsync("Admin")))
                {
                    var role = new IdentityRole
                    {
                        Name = "Admin",
                    };
                    var roleCheck = await _roleMgr.CreateAsync(role);
                    if (roleCheck.Succeeded)
                    {
                        await _roleMgr.AddClaimAsync(role, new System.Security.Claims.Claim("IsAdmin", "True"));
                    }
                }

                //add admin user
                user = new User()
                {
                    UserName = "admin",
                    FirstName = "Mateusz",
                    LastName = "Woźniak",
                    City = "Gniewkowo",
                    Email = "crow.mw@gmail.com"
                };


                var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
                var claimResult = await _userMgr.AddClaimAsync(user, new System.Security.Claims.Claim("SuperUser", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build admin user and roles");
                }
            }

            user = await _userMgr.FindByNameAsync("crowmw");

            if(user == null)
            {
                //add user role
                if (!(await _roleMgr.RoleExistsAsync("User")))
                {
                    var role = new IdentityRole
                    {
                        Name = "User"
                    };
                    await _roleMgr.CreateAsync(role);
                }

                //add user
                user = new User()
                {
                    UserName = "crowmw",
                    FirstName = "Mateusz",
                    LastName = "Woźniak",
                    City = "Toruń",
                    Email = "crowmw@gmail.com"
                };

                var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                var roleResult = await _userMgr.AddToRoleAsync(user, "User");

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }

        }
    }
}
