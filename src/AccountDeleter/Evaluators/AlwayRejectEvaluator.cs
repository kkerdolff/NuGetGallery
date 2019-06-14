﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using NuGet.Services.Entities;
using System;

namespace NuGetGallery.AccountDeleter
{
    public class AlwayRejectEvaluator : IUserEvaluator
    {
        private readonly Guid _id;
        private readonly ILogger<AlwayRejectEvaluator> _logger;

        public AlwayRejectEvaluator(ILogger<AlwayRejectEvaluator> logger)
        {
            _id = Guid.NewGuid();
            _logger = logger;
        }

        public string EvaluatorId
        {
            get
            {
                return _id.ToString();
            }
        }

        public bool CanUserBeDeleted(User user)
        {
            _logger.LogWarning("{Evaluator} User cannot be deleted for reason", nameof(AlwayRejectEvaluator));
            return false;
        }

    }
}
