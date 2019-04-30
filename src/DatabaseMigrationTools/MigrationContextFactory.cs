﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using NuGet.Jobs;
using NuGet.Jobs.Configuration;
using NuGet.Services.DatabaseMigration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuGetGallery.DatabaseMigrationTools
{
    public class MigrationContextFactory : IMigrationContextFactory
    {
        private IDictionary<string, Func<IServiceProvider, Task<IMigrationContext>>> _dictionary = new Dictionary<string, Func<IServiceProvider, Task<IMigrationContext>>>
        {
            {
                JobArgumentNames.GalleryDatabase, async(IServiceProvider serviceProvider) =>
                {
                    var sqlConnection = await serviceProvider.GetRequiredService<ISqlConnectionFactory<GalleryDbConfiguration>>().CreateAsync();
                    return new GalleryDbMigrationContext(sqlConnection);
                }
            },
            {
                JobArgumentNames.SupportRequestDatabase, async(IServiceProvider serviceProvider) =>
                {
                    var sqlConnection = await serviceProvider.GetRequiredService<ISqlConnectionFactory<GalleryDbConfiguration>>().CreateAsync();
                    return new GalleryDbMigrationContext(sqlConnection);
                }
            }
        };

        public async Task<IMigrationContext> CreateMigrationContextAsync(string migrationTargetDatabase, IServiceProvider serviceProvider)
        {
            return await _dictionary[migrationTargetDatabase](serviceProvider);
        }
    }
}
