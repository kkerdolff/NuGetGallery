﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using NuGet.Services.DatabaseMigration;
using NuGetGallery.Areas.Admin;
using NuGetGallery.Areas.Admin.Models;

namespace NuGetGallery.DatabaseMigrationTools
{
    public class SupportRequestDbMigrationContext : BaseDbMigrationContext
    {
        public SupportRequestDbMigrationContext(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));

            var migrationsConfiguration = new SupportRequestMigrationsConfiguration();
            var migrationsDbContext = new SupportRequestDbContext(sqlConnection);
            GetDbMigrator = () => new DbMigrator(migrationsConfiguration, migrationsDbContext);
        }
    }
}
