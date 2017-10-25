﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGetGallery
{
    /// <summary>
    /// Tracks membership of a User account in an Organization account.
    /// </summary>
    public class Membership
    {
        /// <summary>
        /// Organization foreign key.
        /// </summary>
        public int OrganizationKey { get; set; }

        /// <summary>
        /// The <see cref="Organization"/> that contains members.
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Member (User) foreign key.
        /// </summary>
        public int MemberKey { get; set; }

        /// <summary>
        /// The <see cref="User"/> that is a member of the <see cref="Organization"/>.
        /// 
        /// Note that there is no database contraint preventing memberships of Organizations into other
        /// Organizations. For now this is restricted by the Gallery, but could be considered in the
        /// future if we want to support Organization teams.
        /// </summary>
        public virtual User Member { get; set; }

        /// <summary>
        /// Whether the <see cref="Member"/> is an administrator for the <see cref="Organization"/>.
        /// 
        /// Administrators have the following capabilities:
        /// - Organization management (e.g., settings, membership)
        /// - Pushing new package registrations
        /// - All collaborator capabilities
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Whether the <see cref="Member"/> is a collaborator for the <see cref="Organization"/>.
        /// 
        /// Collaborators have the following capabilities:
        /// - Pushing new package versions
        /// - Unlisting packages
        /// </summary>
        public bool IsCollaborator
        {
            get
            {
                return !IsAdmin;
            }
        }
    }
}
