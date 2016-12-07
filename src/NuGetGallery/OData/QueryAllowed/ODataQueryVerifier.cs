﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Web.Http.OData.Query;

namespace NuGetGallery.OData.QueryFilter
{
    public class ODataQueryVerifier
    {
        private static Lazy<ODataQueryFilter> _v2GetUpdates =
            new Lazy<ODataQueryFilter>(() => { return new ODataQueryFilter("apiv2getupdates.json"); },true);
        private static Lazy<ODataQueryFilter> _v2Packages =
            new Lazy<ODataQueryFilter>(() =>{ return new ODataQueryFilter("apiv2packages.json"); }, true);
        private static Lazy<ODataQueryFilter> _v2Search =
            new Lazy<ODataQueryFilter>(() =>{ return new ODataQueryFilter("apiv2search.json"); }, true);
        private static Lazy<ODataQueryFilter> _v1Packages =
            new Lazy<ODataQueryFilter>(() => { return new ODataQueryFilter("apiv1packages.json"); }, true);
        private static Lazy<ODataQueryFilter> _v1Search =
            new Lazy<ODataQueryFilter>(() => { return new ODataQueryFilter("apiv1search.json");}, true); 

        #region Filters for ODataV2FeedController
        /// <summary>
        /// The OData query filter for /api/v2/GetUpdates().
        /// </summary>
        public static ODataQueryFilter V2GetUpdates
        {
            get
            {
                return _v2GetUpdates.Value;
            }
            set
            {
                _v2GetUpdates = new Lazy<ODataQueryFilter>(() => { return value; });
            }
        }

        /// <summary>
        /// The OData query filter for /api/v2/Packages.
        /// </summary>
        public static ODataQueryFilter V2Packages
        {
            get
            {
                return _v2Packages.Value;
            }
            set
            {
                _v2Packages = new Lazy<ODataQueryFilter>(() => { return value; });
            }
        }

        /// <summary>
        /// The OData query filter for /api/v2/Search().
        /// </summary>
        public static ODataQueryFilter V2Search
        {
            get
            {
                return _v2Search.Value;
            }
            set
            {
                _v2Search = new Lazy<ODataQueryFilter>(() => { return value; });
            }
        }
        #endregion Filters for ODataV2FeedController

        #region Filters for ODataV1FeedController
        /// <summary>
        /// The OData query filter for /api/v1/Packages.
        /// </summary>
        public static ODataQueryFilter V1Packages
        {
            get
            {
                return _v1Packages.Value;
            }
            set
            {
                _v1Packages = new Lazy<ODataQueryFilter>(() => { return value; });
            }
        }

        /// <summary>
        /// The OData query filter for /api/v1/Search()
        /// </summary>
        public static ODataQueryFilter V1Search
        {
            get
            {
                return _v1Search.Value;
            }
            set
            {
                _v1Search = new Lazy<ODataQueryFilter>(() => { return value; });
            }
        }
        #endregion Filters for ODataV1FeedController

        /// <summary>
        /// Logic for validation if the odataOptions will be rejected or not.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="odataOptions">The odata options from the request.</param>
        /// <param name="allowedQueryStructure">Each web api will have their individual allowed set of query strutures.</param>
        /// <param name="telemetryContext">Information to be used by the telemetry events.</param>
        /// <returns>True if the odataOptions are allowed.</returns>
        public static bool AreODataOptionsAllowed<TEntity>(ODataQueryOptions<TEntity> odataOptions,
                                                             ODataQueryFilter allowedQueryStructure,
                                                             string telemetryContext)
        {
            var telemetryProperties = new Dictionary<string, string>();
            telemetryProperties.Add("CallContext", $"{telemetryContext}:{nameof(AreODataOptionsAllowed)}");
            //default is true; in case of exception the return value will be true and the request will not be rejected
            var result = true;

            try
            {
                result = allowedQueryStructure.IsAllowed(odataOptions);
            }
            catch (Exception ex)
            {
                //log and do not throw
                telemetryProperties.Add("Exception", ex.Message);
                Telemetry.TrackException(ex, telemetryProperties);
            }

            telemetryProperties.Add("IsAllowed", result.ToString());
            telemetryProperties.Add("QueryPattern", ODataQueryFilter.ODataOptionsMap(odataOptions).ToString());
            Telemetry.TrackEvent("ODataQueryFilter", telemetryProperties, metrics: null);
            return result;
        }

        internal static string GetValidationFailedMessage<T>(ODataQueryOptions<T> options)
        {
            return $"A query with \"{ODataQueryFilter.ODataOptionsMap(options)}\" set of operators is not supported. Please refer to : https://github.com/NuGet/Home/wiki/Filter-OData-query-requests for additional information.";
        }
    }
}