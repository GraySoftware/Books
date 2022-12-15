using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    // the below line will create a table named Categories using our category.cs file
    // note that in order for it to know what file to use we had to add the using BooksWeb.Models;
    // statement at the top to point to the models folder
    public DbSet<Category> Categories { get; set; }

    public DbSet<CoverType> CoverTypes { get; set; }
}

