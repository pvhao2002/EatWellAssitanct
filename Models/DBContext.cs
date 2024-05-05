using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace EatWellAssistant.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
    }
}
