using System.Data.Entity.Migrations.Infrastructure;

namespace NuGetGallery.Data.Migrations
{
    public partial class PackageOwnerRequests : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201110230649210_PackageOwnerRequests"; }
        }

        string IMigrationMetadata.Source
        {
            get { return null; }
        }

        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }
    }
}
