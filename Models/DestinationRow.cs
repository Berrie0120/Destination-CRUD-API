using Microsoft.EntityFrameworkCore;

namespace DestinationApi.Models
{
    public class DestinationRow
    {
        public int Id { get; set; }
        public string? Name { get; set; }  
        public string? Destination { get; set; }
        public int? Icon { get; set; }
        public string? Season {  get; set; }
        public string? Reason { get; set; }
    }
    class DestinationDb : DbContext
    {
        public DestinationDb(DbContextOptions options) : base(options) { }
        public DbSet<DestinationRow> DestinationRows { get; set; } = null!;
    }
}
