﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NuGet.Services.Search.Client;
using NuGet.Services.Search.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace NuGetGallery.Infrastructure.Lucene
{
    public class GallerySearchServiceFacts
    {
        [Fact]
        public void CtorNullArgException()
        {
            // Arrange + Act + Assert
            Assert.Throws<ArgumentNullException>(() => new GallerySearchClient(null));
        }

        [Fact]
        public async Task GetDiagnosticsUsesTheCorrectPath()
        {
            // Arrange 
            var gallerySearchClient = new GallerySearchClient(ResilientClientForTest.GetTestInstance(HttpStatusCode.OK));

            // Act
            var response = await gallerySearchClient.GetDiagnostics();

            var httpResponseContentAsString = await response.HttpResponse.Content.ReadAsStringAsync();
            var path = JObject.Parse(httpResponseContentAsString)["path"].Value<string>();
            var queryString = JObject.Parse(httpResponseContentAsString)["queryString"].Value<string>();
            var statusCode = response.StatusCode;

            // Assert
            Assert.Equal(200, (int)statusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("search/diag", path);
            Assert.Null(queryString);
        }

        [Theory]
        [InlineData(null, null, false, SortOrder.Relevance, 1, 10, false, false, false, false, null, null )]
        [InlineData("query", "projectTypeFilter", true, SortOrder.LastEdited, 1, 10, true, true, true, true, "supportedFramework", "semVerLevel")]
        [InlineData("query", "projectTypeFilter", true, SortOrder.Published, 1, 10, true, true, true, true, "supportedFramework", "semVerLevel")]
        [InlineData("query", "projectTypeFilter", true, SortOrder.TitleAscending, 1, 10, true, true, true, true, "supportedFramework", "semVerLevel")]
        [InlineData("query", "projectTypeFilter", true, SortOrder.TitleDescending, 1, 10, true, true, true, true, "supportedFramework", "semVerLevel")]

        public async Task SearchArgumentsAreCorrectSet(string query,
            string projectTypeFilter,
            bool includePrerelease,
            SortOrder sortBy,
            int skip,
            int take,
            bool isLuceneQuery,
            bool countOnly ,
            bool explain,
            bool getAllVersions,
            string supportedFramework,
            string semVerLevel)
        {
            // Arrange 
            var gallerySearchClient = new GallerySearchClient(ResilientClientForTest.GetTestInstance(HttpStatusCode.OK));

            // Act
            var response = await gallerySearchClient.Search(query,
                projectTypeFilter,
                includePrerelease,
                sortBy,
                skip,
                take,
                isLuceneQuery,
                countOnly,
                explain,
                getAllVersions,
                supportedFramework,
                semVerLevel);

            var httpResponseContentAsString = await response.HttpResponse.Content.ReadAsStringAsync();
            var path = JObject.Parse(httpResponseContentAsString)["path"].Value<string>();
            var queryString = JObject.Parse(httpResponseContentAsString)["queryString"].Value<string>();
            var statusCode = response.StatusCode;

            // Assert
            Assert.Equal(200, (int)statusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("search/query", path);
            Assert.Equal(await GetExpectedSeacrhQueryString(query,
                projectTypeFilter,
                includePrerelease,
                sortBy,
                skip,
                take,
                isLuceneQuery,
                countOnly,
                explain,
                getAllVersions,
                supportedFramework,
                semVerLevel), queryString);
        }

        private async Task<string> GetExpectedSeacrhQueryString(string query,
            string projectTypeFilter,
            bool includePrerelease,
            SortOrder sortBy,
            int skip,
            int take,
            bool isLuceneQuery,
            bool countOnly,
            bool explain,
            bool getAllVersions,
            string supportedFramework,
            string semVerLevel)
        {
            IDictionary<string, string> nameValue = new Dictionary<string, string>();
            nameValue.Add("q", query);
            nameValue.Add("skip", skip.ToString());
            nameValue.Add("take", take.ToString());
            nameValue.Add("sortBy", SortNames[sortBy]);

            if (!String.IsNullOrEmpty(semVerLevel))
            {
                nameValue.Add("semVerLevel", semVerLevel);
            }

            if (!String.IsNullOrEmpty(supportedFramework))
            {
                nameValue.Add("supportedFramework", supportedFramework);
            }

            if (!String.IsNullOrEmpty(projectTypeFilter))
            {
                nameValue.Add("projectType", projectTypeFilter);
            }

            if (includePrerelease)
            {
                nameValue.Add("prerelease", "true");
            }

            if (!isLuceneQuery)
            {
                nameValue.Add("luceneQuery", "false");
            }

            if (explain)
            {
                nameValue.Add("explanation", "true");
            }

            if (getAllVersions)
            {
                nameValue.Add("ignoreFilter", "true");
            }

            if (countOnly)
            {
                nameValue.Add("countOnly", "true");
            }

            var qs = new FormUrlEncodedContent(nameValue);
            return await qs.ReadAsStringAsync();
        }

        private static readonly Dictionary<SortOrder, string> SortNames = new Dictionary<SortOrder, string>
        {
            {SortOrder.LastEdited, "lastEdited"},
            {SortOrder.Relevance, "relevance"},
            {SortOrder.Published, "published"},
            {SortOrder.TitleAscending, "title-asc"},
            {SortOrder.TitleDescending, "title-desc"}
        };

        public class ResilientClientForTest : IResilientSearchClient
        {
            HttpStatusCode _statusCodeForGetAsync;

            private ResilientClientForTest(HttpStatusCode statusCodeForGetAsync)
            {
                _statusCodeForGetAsync = statusCodeForGetAsync;
            }

            public static ResilientClientForTest GetTestInstance(HttpStatusCode statusCodeForGetAsync)
            {
                return new ResilientClientForTest(statusCodeForGetAsync);
            }

            public async Task<HttpResponseMessage> GetAsync(string path, string queryString)
            {
                await Task.Yield();

                var content = new JObject(
                            new JProperty("queryString", queryString),
                            new JProperty("path", path));

                return new HttpResponseMessage()
                {
                    Content = new StringContent(content.ToString(), Encoding.UTF8, CoreConstants.TextContentType),
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{path}/{queryString}"),
                    StatusCode = _statusCodeForGetAsync
                };
            }

            public async Task<string> GetStringAsync(string path, string queryString)
            {
                await Task.Yield();
                return $"{path} {queryString}";
            }
        }
    }
}
