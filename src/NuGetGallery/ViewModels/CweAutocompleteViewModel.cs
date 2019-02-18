﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGetGallery
{
    public class CweAutocompleteViewModel
    {
        public CweAutocompleteViewModel(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        public CweAutocompleteViewModel(IReadOnlyCollection<AutocompleteCweIdQueryResult> results)
        {
            Success = true;
            Results = new List<AutocompleteCweIdQueryResult>(results);
        }

        public bool Success { get; }
        public string ErrorMessage { get; }

        public List<AutocompleteCweIdQueryResult> Results { get; }
    }
}