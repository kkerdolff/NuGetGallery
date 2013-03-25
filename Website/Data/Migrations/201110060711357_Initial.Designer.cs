using System.Data.Entity.Migrations.Infrastructure;

namespace NuGetGallery.Data.Migrations
{
    public partial class Initial : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201110060711357_Initial"; }
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
