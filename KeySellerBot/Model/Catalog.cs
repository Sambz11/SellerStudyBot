using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeySellerBot.Model
{
    public static class Catalog
    {
        public static List<Course> Courses { get; private set; }
        public static int Count
        {
            get
            {
                if (!Directory.Exists(_catalogFilePath))
                    return 0;

                return Directory.GetFiles(_catalogFilePath, "*.crs").Length;
            }
        }

        private static string _catalogFilePath = @"D:\home\site\wwwroot\data\catalog\";

        public static async Task<Course> GetCourse(int ID)
        {
            using (var fs = File.OpenRead(_catalogFilePath + ID.ToString() + ".crs"))
            {
                try
                {
                    Course course = await JsonSerializer.DeserializeAsync<Course>(fs);
                    return course;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static async Task<bool> AddCourse(Course course)
        {
            if (!Directory.Exists(_catalogFilePath))
            {
                Directory.CreateDirectory(_catalogFilePath);
            }

            using (var fs = File.Create(_catalogFilePath + course.ID.ToString() + ".crs"))
            {
                try
                {
                    await JsonSerializer.SerializeAsync(fs, course);
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
            return true;
        }

        public static async Task AddBayer(Course course, int bayerId)
        {
            course.Buyers.Add(bayerId);
            await AddCourse(course);
        }
    }
}
