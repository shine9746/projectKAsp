using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectK.Data;
using System;

namespace ProjectK.DataBase
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<prokjectKDbContext>
    {
        public prokjectKDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<prokjectKDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=projektK;user=projektKAdmin;password=projektK2025",
                ServerVersion.AutoDetect("server=localhost;port=3306;database=projektK;user=projektKAdmin;password=projektK2025")
            );

            return new prokjectKDbContext(optionsBuilder.Options);
        }
    }
}
