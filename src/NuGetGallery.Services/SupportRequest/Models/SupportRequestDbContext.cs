// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NuGetGallery.Areas.Admin.Models
{
    [DbConfigurationType(typeof(EntitiesConfiguration))]
    public class SupportRequestDbContext
        : DbContext, ISupportRequestDbContext
    {
        static SupportRequestDbContext()
        {
            // Don't run migrations, ever!
            Database.SetInitializer<SupportRequestDbContext>(null);
        }

        /// <summary>
        /// The NuGet Gallery code should not use this constructor.
        /// </summary>
        public SupportRequestDbContext()
            : base("name=Gallery.SupportRequestSqlServer")
        {
        }

        /// <summary>
        /// The NuGet Gallery code should usually use this constructor,
        /// so that we can configure the connection via the Cloud Service configuration.
        /// </summary>
        public SupportRequestDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public SupportRequestDbContext(DbConnection connection)
            : base(connection, contextOwnsConnection: true)
        {
        }

        public virtual IDbSet<Admin> Admins { get; set; }

        public virtual IDbSet<Issue> Issues { get; set; }

        public virtual IDbSet<IssueStatus> IssueStatus { get; set; }

        public virtual IDbSet<History> Histories { get; set; }

        public async Task CommitChangesAsync()
        {
            await SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Admin>()
                .Property(e => e.GalleryUsername).IsUnicode(false);
            modelBuilder.Entity<Models.Admin>()
                .HasMany(e => e.Issues)
                .WithOptional(e => e.AssignedTo)
                .HasForeignKey(e => e.AssignedToId);
            modelBuilder.Entity<Models.Admin>()
                .HasMany(e => e.HistoryEntries)
                .WithOptional()
                .HasForeignKey(e => e.AssignedToId);

            modelBuilder.Entity<History>()
               .Property(e => e.Comments).IsUnicode(false);
            modelBuilder.Entity<History>()
               .Property(e => e.EditedBy).IsUnicode(false);

            modelBuilder.Entity<Issue>()
                 .Property(e => e.CreatedBy).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.IssueTitle).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.Details).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.SiteRoot).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.PackageId).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.PackageVersion).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.OwnerEmail).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .Property(e => e.Reason).IsUnicode(false);
            modelBuilder.Entity<Issue>()
                .HasOptional(e => e.AssignedTo)
                .WithMany(e => e.Issues)
                .HasForeignKey(e => e.AssignedToId);
            modelBuilder.Entity<Issue>()
                .HasMany(e => e.HistoryEntries)
                .WithRequired()
                .HasForeignKey(e => e.IssueId);
            modelBuilder.Entity<Issue>()
                .HasRequired(e => e.IssueStatus)
                .WithMany(e => e.Issues)
                .HasForeignKey(e => e.IssueStatusId);

            modelBuilder.Entity<IssueStatus>()
                 .Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<IssueStatus>()
                .HasMany(e => e.Issues)
                .WithRequired(e => e.IssueStatus)
                .HasForeignKey(e => e.IssueStatusId);
        }
    }
}
