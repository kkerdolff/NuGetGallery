// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NuGetGallery
{
    public class RelatedPackagesService : IRelatedPackagesService
    {
        internal const string ContainerName = "nuget-relatedpackages";

        private readonly IPackageService _packageService;
        private readonly IReportService _reportService;

        private IReportContainer _reportContainer;

        public RelatedPackagesService(
            IPackageService packageService,
            IReportService reportService)
        {
            _packageService = packageService ?? throw new ArgumentNullException(nameof(packageService));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }

        public async Task<IEnumerable<Package>> GetRelatedPackagesAsync(Package package)
        {
            await EnsureReportContainerLoaded();

            RelatedPackages relatedPackages;
            try
            {
                string reportName = GetReportName(package.PackageRegistration.Id);
                var report = await _reportContainer.Load(reportName);
                relatedPackages = JsonConvert.DeserializeObject<RelatedPackages>(report.Content);
            }
            catch (ReportNotFoundException ex)
            {
                QuietLog.LogHandledException(ex);
                return Enumerable.Empty<Package>();
            }

            string targetId = relatedPackages.Id;
            Debug.Assert(string.Equals(
                targetId,
                package.PackageRegistration.Id,
                StringComparison.OrdinalIgnoreCase));

            var relatedPackageIds = relatedPackages.Recommendations;
            return relatedPackageIds
                .Select(id => _packageService.FindAbsoluteLatestPackageById(id))
                .Where(p => p != null);
        }

        internal static string GetReportName(string packageId)
        {
            string GetHexadecimalString(byte[] bytes)
            {
                var sb = new StringBuilder(capacity: bytes.Length * 2);
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }

            string encodedId = GetHexadecimalString(Encoding.UTF8.GetBytes(packageId));
            return $"RelatedPackages/{encodedId}.json";
        }

        private async Task EnsureReportContainerLoaded()
        {
            const string ContainerName = "nuget-relatedpackages";

            if (_reportContainer == null)
            {
                _reportContainer = await _reportService.GetContainer(ContainerName);
            }
        }
    }
}
