using App.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Database.Context
{
	public class ApplicationContext : DbContext
	{
		public IConfiguration _configuration { get; }

		#region DbEntities
		public virtual DbSet<CivilStatusEntity> CivilStatuses { get; set; }
		public virtual DbSet<UserEntity> Users { get; set; }
		#endregion

		public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration) : base(options)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("App.Presentation"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CivilStatusEntity>(entity =>
			{
				entity.HasKey(e => e.CivilStatusId);
				entity.ToTable("CivilStatus");
				entity.Property(e => e.Description).HasMaxLength(50);
			});

			modelBuilder.Entity<UserEntity>(entity =>
			{
				entity.HasKey(e => e.UserId);
				entity.Property(e => e.UserId).ValueGeneratedNever();
				entity.Property(e => e.Birthdate).HasColumnType("date").HasColumnName("Birthday");
				entity.Property(e => e.FileName).HasMaxLength(100);
				entity.Property(e => e.Lastname).HasMaxLength(50);
				entity.Property(e => e.Name).HasMaxLength(50);
				entity.Property(e => e.UniqueFileName).HasMaxLength(120);
				entity.HasOne(d => d.CivilStatus).WithMany(p => p.Users).HasForeignKey(d => d.CivilStatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Users_Users");
			});
			base.OnModelCreating(modelBuilder);
		}
	}
}
