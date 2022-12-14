using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using new2me_api.Dtos;
using new2me_api.Models;

namespace new2me_api.Data.Query
{
    public partial class Query:IQuery
    {
        public async Task<User> Authenticate(string usernameOrEmail, string password){
            // get the user of the username or email
            var user =  await this.GetUserByUsername(usernameOrEmail);
            if (user==null){
                user =  await this.GetUserByEmail(usernameOrEmail);
            }

            if (user==null || user.PasswordKey==null){
                return null;
            }

            // compute the hash for the entered password using the password key in the db
            byte[] loginPassHash;
            using (var hmac = new HMACSHA256(user.PasswordKey)){
                loginPassHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            // compare the hash of the entered password and the has in the db
            for (int i=0; i<loginPassHash.Count(); i++){
                if (loginPassHash[i]!=user.Password[i]){
                    return null;
                }
            }
            return user; 
        }

        public async Task<User> SignUp(string username, string password, string email){
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA256()){
                passwordKey = hmac.Key; // random key generated by the class
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };

            User user = new User {
                Username = username,
                Password = passwordHash,
                PasswordKey = passwordKey,
                Email = email
            };

            await this.new2meDb.AddAsync(user);
            await this.new2meDb.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UsernameExists(string username){
            return await this.new2meDb.Users.AnyAsync(u => u.Username==username);
        }

        public async Task<bool> EmailExists(string email){
            return await this.new2meDb.Users.AnyAsync(u=> u.Email == email);
        }

        public async Task<User> GetUserByEmail(string email){
            return await this.new2meDb.Users.Where(user=>user.Email==email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(int id){
            return await this.new2meDb.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsername(string username){
            return await this.new2meDb.Users.Where(user=>user.Username==username).FirstOrDefaultAsync();
        }

        public async Task UpdateUser(User user){
            await this.new2meDb.SaveChangesAsync();
        }

        public async Task<User> resetUserPassword(User user, string pass){
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA256()){
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }

            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            await this.new2meDb.SaveChangesAsync();

            return user;
        }
    }
}