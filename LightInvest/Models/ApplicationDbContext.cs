using LightInvest.Models;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }

		// Adicione esta propriedade para representar a tabela de ROICalculators
		public DbSet<ROICalculator> ROICalculators { get; set; }

		// Se você tiver outras entidades, adicione-as aqui também
		public DbSet<User> Users { get; set; }
	}
}