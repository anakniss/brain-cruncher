using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<UserExam> UserExams { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserExam>()
            .HasKey(ue => new { ue.UserId, ue.ExamId }); // Define a chave composta

        modelBuilder.Entity<UserExam>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserExams)
            .HasForeignKey(ue => ue.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserExam>()
            .HasOne(ue => ue.Exam)
            .WithMany(e => e.UserExams)
            .HasForeignKey(ue => ue.ExamId)
            .OnDelete(DeleteBehavior.Restrict); 

        base.OnModelCreating(modelBuilder);
    }
}