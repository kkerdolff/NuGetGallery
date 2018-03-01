﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NuGetGallery.Security
{
    public class RequireOrganizationTenantPolicy : UserSecurityPolicyHandler, IUserSecurityPolicySubscription
    {
        public const string _SubscriptionName = "AzureActiveDirectoryTenant";
        public const string PolicyName = nameof(RequireOrganizationTenantPolicy);

        public string SubscriptionName => _SubscriptionName;

        public IEnumerable<UserSecurityPolicy> Policies { get; set; }

        public class State
        {
            [JsonProperty("t")]
            public string Tenant { get; set; }
        }

        public RequireOrganizationTenantPolicy()
            : base(PolicyName, SecurityPolicyAction.JoinOrganization)
        {
        }

        private RequireOrganizationTenantPolicy(IEnumerable<UserSecurityPolicy> policies)
            : base(PolicyName, SecurityPolicyAction.JoinOrganization)
        {
            Policies = policies;
        }

        public static IUserSecurityPolicySubscription Create(string tenantId)
        {
            var value = JsonConvert.SerializeObject(new State()
            {
                Tenant = tenantId
            });

            return new RequireOrganizationTenantPolicy(new []
            {
                new UserSecurityPolicy(PolicyName, PolicyName, value)
            });
        }

        private static State GetPolicyState(UserSecurityPolicyEvaluationContext context)
        {
            return context.Policies
                .Select(p => JsonConvert.DeserializeObject<State>(p.Value))
                .FirstOrDefault();
        }

        public Task OnSubscribeAsync(UserSecurityPolicySubscriptionContext context)
        {
            return Task.CompletedTask;
        }

        public Task OnUnsubscribeAsync(UserSecurityPolicySubscriptionContext context)
        {
            return Task.CompletedTask;
        }

        public override SecurityPolicyResult Evaluate(UserSecurityPolicyEvaluationContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            var state = GetPolicyState(context);
            var targetAccount = context.TargetAccount;
            var targetCredential = targetAccount.Credentials.GetAzureActiveDirectoryCredential();

            if (targetCredential == null
                || !state.Tenant.Equals(targetCredential.TenantId, StringComparison.OrdinalIgnoreCase))
            {
                return SecurityPolicyResult.CreateErrorResult(string.Format(CultureInfo.CurrentCulture,
                        Strings.AddMember_UserDoesNotMeetOrganizationPolicy, targetAccount.Username));
            }

            return SecurityPolicyResult.SuccessResult;
        }
    }
}