using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Entities;
using RaceRoute = RaceDay.Api.Entities.Route;

namespace RaceDay.Api.Data;

public class RaceDayDbContext : DbContext
{
    public RaceDayDbContext(
        DbContextOptions<RaceDayDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Event> Events => Set<Event>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Enrolment> Enrolments => Set<Enrolment>();

    public DbSet<Result> Results => Set<Result>();

    public DbSet<RaceRoute> Routes => Set<RaceRoute>();

    public DbSet<WeatherForecast> WeatherForecasts =>
        Set<WeatherForecast>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .HasOne(raceEvent => raceEvent.Organiser)
            .WithMany(user => user.OrganisedEvents)
            .HasForeignKey(raceEvent => raceEvent.OrganiserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Event>()
            .HasMany(raceEvent => raceEvent.Categories)
            .WithOne(category => category.Event)
            .HasForeignKey(category => category.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Event>()
            .HasOne(raceEvent => raceEvent.Route)
            .WithOne(route => route.Event)
            .HasForeignKey<RaceRoute>(
                route => route.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Event>()
            .HasMany(raceEvent => raceEvent.WeatherForecasts)
            .WithOne(forecast => forecast.Event)
            .HasForeignKey(forecast => forecast.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasMany(category => category.Enrolments)
            .WithOne(enrolment => enrolment.Category)
            .HasForeignKey(enrolment => enrolment.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(user => user.Enrolments)
            .WithOne(enrolment => enrolment.Participant)
            .HasForeignKey(enrolment => enrolment.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrolment>()
            .HasOne(enrolment => enrolment.Result)
            .WithOne(result => result.Enrolment)
            .HasForeignKey<Result>(
                result => result.EnrolmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrolment>()
            .HasIndex(enrolment => new
            {
                enrolment.CategoryId,
                enrolment.ParticipantId
            })
            .IsUnique();

        modelBuilder.Entity<Result>()
            .HasIndex(result => result.EnrolmentId)
            .IsUnique();
    }
}

