﻿using System.Text.RegularExpressions;
using Xunit;
using Xunit.Extensions;

namespace NuGetGallery
{
    /// <summary>
    /// Notes, we do not follow strictly the RFCs at this time, and we choose not to support many obscure email address variants, 
    /// such as those with quotes and parentheses.
    /// We may add internationalization or single-letter domain support in the future..?
    /// </summary>
    public class EmailValidationRegex
    {
        [Fact]
        public void TheWholeWorks()
        {
            var match = new Regex(RegisterRequest.EmailValidationRegex).IsMatch("fred@fred.com");
            Assert.True(match);
        }

        [Theory]
        [InlineData("fred@@fred.com")]
        [InlineData("fred@fred@fred.com")]
        public void TheWholeDoesntAllow(string testWhole)
        {
            var match = new Regex(RegisterRequest.EmailValidationRegex).IsMatch(testWhole);
            Assert.False(match);
        }

        [Theory]
        [InlineData("fred")]
        [InlineData(".fred")]
        [InlineData("fred.")]
        [InlineData("fr.ed")]
        [InlineData("fr..ed")]
        [InlineData("!#$%&'*+-/=?^_`{}|~")] // thanks Wikipedia
        [InlineData("fred~`'.baz")]
        public void TheFirstPartMatches(string testFirstPart)
        {
            var match = new Regex("^" + RegisterRequest.FirstPart + "$").IsMatch(testFirstPart);
            Assert.True(match);
        }

        [Theory]
        [InlineData("fr@ed")]
        [InlineData("fr\\ed")]
        [InlineData("fr\"ed")]
        [InlineData("fr[]ed")]
        [InlineData("abc\"defghi\"xyz")] // thanks Wikipedia
        [InlineData("abc.\"defghi\".xyz")] // thanks Wikipedia, but in practice nobody uses these email addresses.
        [InlineData("abc.\"def\\\"\"ghi\".xyz")] // thanks Wikipedia, but in practice nobody uses these email addresses.
        public void TheFirstPartDoesntAllow(string testFirstPart)
        {
            var match = new Regex("^" + RegisterRequest.FirstPart + "$").IsMatch(testFirstPart);
            Assert.False(match);
        }

        [Theory]
        [InlineData("XYZ.com")]
        [InlineData("xyz.govt.nz")]
        [InlineData("X1-Y2-Z3.net")]
        public void TheSecondPartMatches(string testSecondPart)
        {
            var match = new Regex("^" + RegisterRequest.SecondPart + "$").IsMatch(testSecondPart);
            Assert.True(match);
        }

        [Theory]
        [InlineData("i.com")] // single letter domains do exist, but we have never supported them yet...
        [InlineData("com")] //no top level domains
        [InlineData("mailserver1")] //no hostname without top level domain
        [InlineData("[1.1.1.1]")] //no IP addresses
        [InlineData("[IPv6:2001:db8:1ff::a0b:dbd0]")] //no IP addresses
        public void TheSecondPartDoesntAllow(string testSecondPart)
        {
            var match = new Regex("^" + RegisterRequest.SecondPart + "$").IsMatch(testSecondPart);
            Assert.False(match);
        }
    }
}
