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
    public class PackageRecommendationService : IPackageRecommendationService
    {
        private readonly IPackageService _packageService;
        private readonly IReportService _reportService;

        public PackageRecommendationService(
            IPackageService packageService,
            IReportService reportService)
        {
            _packageService = packageService;
            _reportService = reportService;
        }

        public async Task<IEnumerable<Package>> GetRecommendedPackagesAsync(Package package)
        {
            string packageId = package.PackageRegistration.Id;
            string reportName = GetReportName(packageId);
            ReportBlob report;
            try
            {
                report = await _reportService.Load(reportName);
            }
            catch (ReportNotFoundException ex)
            {
                QuietLog.LogHandledException(ex);
                return Enumerable.Empty<Package>();
            }
            var recommendedPackages = JsonConvert.DeserializeObject<RecommendedPackages>(report.Content);

            string targetId = recommendedPackages.Id;
            Debug.Assert(string.Equals(
                targetId,
                package.PackageRegistration.Id,
                StringComparison.OrdinalIgnoreCase));

            var recommendationIds = recommendedPackages.Recommendations;
            return recommendationIds
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
            return $"Recommendations/{encodedId}.json";
        }
    }
}
