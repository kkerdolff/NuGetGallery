﻿using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace NuGetGallery.ViewModels
{
    public class PackageViewModelFacts
    {
        [Fact]
        public void LicenseNamesAreParsedByCommas()
        {
            var package = new Package
            {
                LicenseNames = "l1,l2, l3 ,l4  ,  l5 ",
            };
            var packageViewModel = new PackageViewModel(package);
            Assert.Equal(new string[] { "l1", "l2", "l3", "l4", "l5" }, packageViewModel.LicenseNames);
        }

        [Fact]
        public void LicenseReportFieldsKeptWhenLicenseReportDisabled()
        {
            var package = new Package
            {
                HideLicenseReport = true,
                LicenseNames = "l1",
                LicenseReportUrl = "url" 
            };
            var packageViewModel = new PackageViewModel(package);
            Assert.NotNull(packageViewModel.LicenseNames);
            Assert.NotNull(packageViewModel.LicenseReportUrl);
        }

        [Fact]
        public void LicenseReportUrlKeptWhenLicenseReportEnabled()
        {
            var package = new Package
            {
                HideLicenseReport = false,
                LicenseReportUrl = "url" 
            };
            var packageViewModel = new PackageViewModel(package);
            Assert.NotNull(packageViewModel.LicenseReportUrl);
        }

        [Fact]
        public void LicenseNamesKeptWhenLicenseReportEnabled()
        {
            var package = new Package
            {
                HideLicenseReport = false,
                LicenseNames = "l1" 
            };
            var packageViewModel = new PackageViewModel(package);
            Assert.NotNull(packageViewModel.LicenseNames);
        }

        [Fact]
        public void LicenseUrlKeptWhenLicenseReportDisabled()
        {
            var package = new Package
            {
                HideLicenseReport = true,
                LicenseUrl = "url"
            };
            var packageViewModel = new PackageViewModel(package);
            Assert.NotNull(packageViewModel.LicenseUrl);
        }

        [Theory]
        [InlineData("http://foo", "//foo")]
        [InlineData("https://foo", "//foo")]
        [InlineData("foo/bar/baz", "foo/bar/baz")]
        [InlineData("ftp://foo/bar/baz", "//foo/bar/baz")]
        [InlineData("gopher://foo/bar/baz", "//foo/bar/baz")] // Because why not :)
        public void IconUrlIsRewritten(string input, string result)
        {
            var package = new Package()
            {
                IconUrl = input
            };
            var viewModel = new PackageViewModel(package);
            Assert.Equal(result, viewModel.IconUrl);
        }
    }
}
