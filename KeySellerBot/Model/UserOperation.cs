using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace KeySellerBot.Model
{
    public static class UserOperation
    {
        private static string _usersPath = @"D:\home\site\wwwroot\data\users\";
        public static async Task AddUser(long userId, long inviterId = 0)
        {
            if (!Directory.Exists(_usersPath))
            {
                Directory.CreateDirectory(_usersPath);
            }

            var user = new User()
            {
                Id = userId,
                Balance = 0,
                Referals = new List<long>(),
            };

            if (inviterId != 0)
            {
                await AddReferal(inviterId, user.Id);
            }

            using (var fileStream = File.Create(_usersPath + user.Id.ToString() + ".u"))
            {
                try
                {
                    await JsonSerializer.SerializeAsync(fileStream, user);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public static async Task<User> GetUser(long id)
        {
            if (!File.Exists(_usersPath + id.ToString() + ".u"))
            {
                return null;
            }

            User user = null;

            using (var fileStream = File.OpenRead(_usersPath + id.ToString() + ".u"))
            {
                try
                {
                    user = await JsonSerializer.DeserializeAsync<User>(fileStream);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return user;
        }

        public static bool Exist(long userId)
        {
            if (Directory.Exists(_usersPath))
            {
                if (File.Exists(_usersPath + userId.ToString() + ".u"))
                {
                    return true;
                }
            }
            return false;
        }

        private static async Task AddReferal(long InvaterId, long referalId)
        {
            var user = await GetUser(InvaterId);
            if (user != null)
            {
                user.Referals.Add(referalId);
                await user.Save();
            }
        }

        private static async Task<bool> Save(this User user)
        {
            using (var fileStream = File.OpenWrite(_usersPath + user.Id.ToString() + ".u"))
            {
                try
                {
                    await JsonSerializer.SerializeAsync(fileStream, user);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
