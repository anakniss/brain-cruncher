using backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<UserExam> UserExams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }
    public DbSet<Alternative> Alternatives { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>()
            .HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .IsRequired(false);
        
        modelBuilder.Entity<Question>()
            .HasOne(e => e.Exam)
            .WithMany()
            .HasForeignKey(e => e.ExamId)
            .IsRequired(false);
        
        modelBuilder.Entity<Alternative>()
            .HasOne(e => e.Question)
            .WithMany()
            .HasForeignKey(e => e.QuestionId)
            .IsRequired(false);
        
        modelBuilder.Entity<UserExam>()
            .HasKey(ue => new { ue.UserId, ue.ExamId });
        
        modelBuilder.Entity<UserExam>()
            .HasOne(er=> er.Exam)
            .WithMany()
            .HasForeignKey(er => er.ExamId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserExam>()
            .HasOne(er=> er.User)
            .WithMany()
            .HasForeignKey(er => er.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ExamResult>()
            .HasOne(er => er.Exam)
            .WithMany()
            .HasForeignKey(er => er.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExamResult>()
            .HasOne(er => er.Student)
            .WithMany()
            .HasForeignKey(er => er.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}