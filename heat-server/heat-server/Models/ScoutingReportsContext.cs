using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace heat_server.Models
{
    public partial class ScoutingReportsContext : DbContext
    {
        public ScoutingReportsContext()
        {
        }

        public ScoutingReportsContext(DbContextOptions<ScoutingReportsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<League> League { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamPlayer> TeamPlayer { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-L6VQTEJ\\SQLEXPRESS;Database=ScoutingReports;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<League>(entity =>
            {
                entity.HasKey(e => e.LeagueKey);

                entity.Property(e => e.LeagueKey).ValueGeneratedNever();

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.LeagueName).HasMaxLength(100);

                entity.Property(e => e.SearchDisplayFlag)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerKey);

                entity.Property(e => e.PlayerKey).ValueGeneratedNever();

                entity.Property(e => e.AgentName).HasMaxLength(200);

                entity.Property(e => e.AgentPhone).HasMaxLength(50);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.BodyFat).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.BodyFatSource)
                    .HasColumnName("BodyFat_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.CommittedTo).HasMaxLength(200);

                entity.Property(e => e.CourtRunTime34)
                    .HasColumnName("CourtRunTime_3_4")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CourtRunTime34Source)
                    .HasColumnName("CourtRunTime_3_4_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.DwhInsertDatetime)
                    .HasColumnName("dwh_insert_datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DwhUpdateDatetime)
                    .HasColumnName("dwh_update_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GlplayerKey).HasColumnName("GLPlayerKey");

                entity.Property(e => e.Hand).HasMaxLength(10);

                entity.Property(e => e.HandLength).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.HandWHSource)
                    .HasColumnName("Hand_W_H_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.HandWidth).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Handedness).HasMaxLength(10);

                entity.Property(e => e.HandednessSource)
                    .HasColumnName("Handedness_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.Height).HasColumnType("decimal(6, 4)");

                entity.Property(e => e.HeightSource)
                    .HasColumnName("Height_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StandingReach).HasColumnType("decimal(6, 4)");

                entity.Property(e => e.StandingReachSource)
                    .HasColumnName("StandingReach_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.Urlphoto)
                    .HasColumnName("URLPhoto")
                    .HasMaxLength(250);

                entity.Property(e => e.VerticalJumpMax).HasColumnType("decimal(6, 4)");

                entity.Property(e => e.VerticalJumpMaxSource)
                    .HasColumnName("VerticalJumpMax_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.VerticalJumpNoStep).HasColumnType("decimal(6, 4)");

                entity.Property(e => e.VerticalJumpNoStepSource)
                    .HasColumnName("VerticalJumpNoStep_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.Weight).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.WeightSource)
                    .HasColumnName("Weight_Source")
                    .HasMaxLength(100);

                entity.Property(e => e.Wing).HasColumnType("decimal(6, 4)");

                entity.Property(e => e.WingSource)
                    .HasColumnName("Wing_Source")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamKey);

                entity.HasIndex(e => new { e.TeamKey, e.LeagueKey })
                    .HasName("UK_DimTeam")
                    .IsUnique();

                entity.Property(e => e.TeamKey).ValueGeneratedNever();

                entity.Property(e => e.CoachName).HasMaxLength(100);

                entity.Property(e => e.Conference).HasMaxLength(100);

                entity.Property(e => e.CurrentNbateamFlg)
                    .HasColumnName("CurrentNBATeamFlg")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LeagueKeyDomestic).HasColumnName("LeagueKey_Domestic");

                entity.Property(e => e.SubConference).HasMaxLength(100);

                entity.Property(e => e.TeamCity).HasMaxLength(100);

                entity.Property(e => e.TeamCountry).HasMaxLength(100);

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TeamNickname).HasMaxLength(100);

                entity.Property(e => e.Urlphoto)
                    .HasColumnName("URLPhoto")
                    .HasMaxLength(250);

                entity.HasOne(d => d.LeagueKeyNavigation)
                    .WithMany(p => p.TeamLeagueKeyNavigation)
                    .HasForeignKey(d => d.LeagueKey)
                    .HasConstraintName("FK_Team_League");

                entity.HasOne(d => d.LeagueKeyDomesticNavigation)
                    .WithMany(p => p.TeamLeagueKeyDomesticNavigation)
                    .HasForeignKey(d => d.LeagueKeyDomestic)
                    .HasConstraintName("FK_Team_LeagueDomestic");
            });

            modelBuilder.Entity<TeamPlayer>(entity =>
            {
                entity.HasKey(e => new { e.PlayerKey, e.TeamKey, e.SeasonKey });

                entity.Property(e => e.DwhInsertDatetime)
                    .HasColumnName("dwh_insert_datetime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PlayerKeyNavigation)
                    .WithMany(p => p.TeamPlayer)
                    .HasForeignKey(d => d.PlayerKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamPlayer_Player");

                entity.HasOne(d => d.TeamKeyNavigation)
                    .WithMany(p => p.TeamPlayer)
                    .HasForeignKey(d => d.TeamKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamPlayer_Team");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.AzureAdUserId)
                    .HasName("PK__User__76BABBB64312D849");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
