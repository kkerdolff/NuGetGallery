﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGetGallery
{
    public class TyposquattingCheckListCacheService : ITyposquattingCheckListCacheService
    {
        private readonly object Locker = new object();

        private List<string> Cache;
        private DateTime LastRefreshTime;

        private int TyposquattingCheckListConfiguredLength;

        public TyposquattingCheckListCacheService()
        {
            TyposquattingCheckListConfiguredLength = -1;
            LastRefreshTime = DateTime.MinValue;
        }

        public IReadOnlyCollection<string> GetTyposquattingCheckList(int checkListConfiguredLength, double checkListExpireTimeInHours, IPackageService packageService)
        {
            if (checkListConfiguredLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(checkListConfiguredLength), "Negative values are not supported.");
            }
            if (packageService == null)
            {
                throw new ArgumentNullException(nameof(packageService));
            }

            if (ShouldCacheBeUpdated(checkListConfiguredLength, checkListExpireTimeInHours))
            {
                lock (Locker)
                {
                    if (ShouldCacheBeUpdated(checkListConfiguredLength, checkListExpireTimeInHours))
                    {
                        TyposquattingCheckListConfiguredLength = checkListConfiguredLength;

                        Cache = packageService.GetAllPackageRegistrations()
                            .OrderByDescending(pr => pr.IsVerified)
                            .ThenByDescending(pr => pr.DownloadCount)
                            .Select(pr => pr.Id)
                            .Take(TyposquattingCheckListConfiguredLength)
                            .ToList();

                        LastRefreshTime = DateTime.UtcNow;
                    }
                }
            }

            return Cache;
        }

        private bool ShouldCacheBeUpdated(int checkListConfiguredLength, double checkListExpireTimeInHours)
        {
            return Cache == null || checkListConfiguredLength != TyposquattingCheckListConfiguredLength || IsCheckListCacheExpired(checkListExpireTimeInHours);
        }

        private bool IsCheckListCacheExpired(double checkListExpireTimeInHours)
        {
            return DateTime.UtcNow >= LastRefreshTime.Add(TimeSpan.FromHours(checkListExpireTimeInHours));
        }
    }
}