using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorUnavailability> DoctorUnavailabilities { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceRow> InvoiceRows { get; set; }
        public DbSet<AppointmentDocument> AppointmentDocuments { get; set; }

        //To forbid cascade delete
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Appointment relations
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict); //Will not delete automatically

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            //Forbid cascade delete Doctor -> DoctorUnavailability 
            modelBuilder.Entity<DoctorUnavailability>()
                .HasOne(du => du.Doctor)
                .WithMany(d => d.Unavailabilities)
                .HasForeignKey(du => du.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            //Invoice relations
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Appointment)
                .WithOne(a => a.Invoice)
                .HasForeignKey<Invoice>(i => i.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //AppointmentDocument relations
            modelBuilder.Entity<AppointmentDocument>()
                .HasOne(ad => ad.Appointment)
                .WithMany(a => a.Documents)
                .HasForeignKey(ad => ad.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //InvoiceRow relations
            modelBuilder.Entity<InvoiceRow>()
                .HasOne(ir => ir.Invoice)
                .WithMany(i => i.InvoiceRows)
                .HasForeignKey(ir => ir.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
