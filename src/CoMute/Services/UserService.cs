﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using CoMute.Web.Data;
using CoMute.Web.Models;
using CoMute.Web.Models.Dto;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CoMute.Web.Services
{
    public class UserService
    {
        private readonly CoMuteDbContext _coMuteDbContext;

        public UserService()
        {
            _coMuteDbContext = new CoMuteDbContext();
        }

        public async Task<bool> RegisterNewUser(RegistrationRequest request)
        {
            try
            {
                _coMuteDbContext.Users.Add(new User
                {
                    Email = request.EmailAddress,
                    Name = request.Name,
                    Surname = request.Surname,
                    PhoneNumber = request.PhoneNumber,
                    UserName = request.EmailAddress,
                    PasswordHash = HashPassword(request.Password)
                });
                await _coMuteDbContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
            
          
        }

        #region Private Methods
        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        #endregion
    }
}