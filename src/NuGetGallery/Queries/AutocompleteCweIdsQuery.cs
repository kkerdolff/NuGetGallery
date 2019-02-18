﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Services.Entities;

namespace NuGetGallery
{
    public class AutocompleteCweIdsQuery
        : IAutocompleteCweIdsQuery
    {
        // Search results should be limited anywhere between 5 - 10 results.
        private const int MaxResults = 5;

        private readonly IEntitiesContext _entitiesContext;

        public AutocompleteCweIdsQuery(IEntitiesContext entitiesContext)
        {
            _entitiesContext = entitiesContext ?? throw new ArgumentNullException(nameof(entitiesContext));
        }

        public IReadOnlyCollection<CweIdAutocompleteQueryResult> Execute(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            // Validate search term and determine query type.
            var validatedSearchTerm = CweQueryStringValidator.Validate(queryString, out var queryMethod);

            // Query the database.
            // Only include listed CVE entities.
            IReadOnlyCollection<Cwe> queryResults;
            switch (queryMethod)
            {
                case CweQueryMethod.ByCweId:
                    queryResults = _entitiesContext.Cwes
                        .Where(e => e.CweId.StartsWith(validatedSearchTerm) && e.Listed == true)
                        .OrderBy(e => e.CweId)
                        .Take(MaxResults)
                        .ToList();
                    break;

                case CweQueryMethod.ByName:
                    queryResults = _entitiesContext.Cwes
                        .Where(e => e.Name.Contains(validatedSearchTerm) && e.Listed == true)
                        .OrderBy(e => e.CweId)
                        .Take(MaxResults)
                        .ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryMethod));
            }

            return queryResults
                .Select(e => new CweIdAutocompleteQueryResult(e.CweId, e.Name, e.Description))
                .OrderBy(e => CweIdHelper.GetCweIdNumericPartAsInteger(e.CweId))
                .ToList();
        }
    }
}