﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Services.Search.Client
{
    public class SearchClientConfiguration
    {
        public static string SearchPrimaryInstance = "SearchPrimary";
        public static string SearchSecondaryInstance = "SearchSecondary";
        public static int SearchRetryCount = 3;
        // Time for service remediation.
        public static int SearchCircuitBreakerDelayInSeconds = 10*60;
        // Try to have searchRetryCount*retryInterval to be close to 1 second in order to keep the user still engaged. https://www.nngroup.com/articles/website-response-times/
        public static int WaitAndRetryDefaultIntervalInMilliseconds = 500;
    }
}