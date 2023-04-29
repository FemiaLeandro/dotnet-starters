using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Templates.DockerWebAppWithAuth.Data
{
    public class WebAppDbContext : IdentityDbContext
    {
        public WebAppDbContext(DbContextOptions<WebAppDbContext> options) : base(options)
        {
            // Add your tables here
        }
    }
}