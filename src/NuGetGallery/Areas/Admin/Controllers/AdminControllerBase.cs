﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGetGallery.Filters;

namespace NuGetGallery.Areas.Admin.Controllers
{
    [BlockDiscontinuedPasswordAuthorize(Roles="Admins")]
    public class AdminControllerBase : AppController
    {
    }
}