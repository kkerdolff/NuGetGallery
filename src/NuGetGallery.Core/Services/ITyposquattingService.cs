﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Services.Entities;

namespace NuGetGallery
{
    /// <summary>
    /// This interface is used to check typo-squatting of uploaded package ID with the owner.  
    /// </summary>
    public interface ITyposquattingService
    {
        /// <summary>
        /// The function is used to check whether the uploaded package is a typo-squatting package.
        /// </summary>
        /// <param name="uploadedPackageId"> The package ID of the uploaded package. We check the pacakge ID with the packages in the gallery for typo-squatting issue</param>
        /// <param name="uploadedPackageOwner"> The package owner of the uploaded package.</param>
        /// <param name="typosquattingCheckCollisionIds"> The return collision package Id list if it exists</param>
        bool IsUploadedPackageIdTyposquatting(
            string uploadedPackageId, 
            User uploadedPackageOwner,
            IQueryable<PackageRegistration> allPackageRegistrations,
            int checkListConfiguredLength,
            TimeSpan checkListExpireTimeInHours,
            bool isIsTyposquattingEnabledForOwner,
            out List<string> typosquattingCheckCollisionIds, 
            out IDictionary<TyposquattingMetric, object> telemetry);
    }

    public enum TyposquattingMetric
    {
        TrackMetricForTyposquattingChecklistRetrievalTime,
        TrackMetricForTyposquattingAlgorithmProcessingTime,
        TrackMetricForTyposquattingOwnersCheckTime,
        TrackMetricForTyposquattingCheckResultAndTotalTime
    }

    public class TyposquattingCheckResultAndTotalTime
    {
        public TimeSpan TotalTime { get; set; }
        public bool WasUploadBlocked { get; set; }
        public IReadOnlyCollection<string> TyposquattingCheckCollisionIds { get; set; }
        public int PackageIdsCheckListCount { get; set; }
        public TimeSpan CheckListExpireTimeInHours { get; set; }
    }
}
