// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NatsunekoLaboratory.TraceMate.Redactor.Rules.Interfaces;

namespace NatsunekoLaboratory.TraceMate.Redactor.Rules
{
    internal class NpmRule : IRedactionRule
    {
        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactGitHubXAuthToken,
                RedactNpmAuthToken,
                RedactNpmAccessToken
            };
        }

        #region GitHub XAuth Pattern

        // https://github.blog/2012-09-21-easier-builds-and-deployments-using-git-over-https-and-oauth/
        private static readonly Regex XOauthPattern = new Regex("https?:\\/\\/(.*?):x-oauth-basic@github.com.*", RegexOptions.Compiled);

        private string RedactGitHubXAuthToken(string str, string text)
        {
            return XOauthPattern.IsMatch(str) ? XOauthPattern.Replace(str, text) : str;
        }

        #endregion

        #region NPM AuthToken Pattern

        // https://blog.npmjs.org/post/118393368555/deploying-with-npm-private-modules
        private static readonly Regex NpmTokenPattern = new Regex("_authToken=([^$].*)", RegexOptions.Compiled);

        private string RedactNpmAuthToken(string str, string text)
        {
            return NpmTokenPattern.IsMatch(str) ? NpmTokenPattern.Replace(str, text) : str;
        }

        #endregion

        #region NPM Access Token

        // https://github.blog/2021-09-23-announcing-npms-new-access-token-format/
        // https://github.blog/2021-04-05-behind-githubs-new-authentication-token-formats/
        private static readonly Regex NpmAccessTokenPattern = new Regex("npm_[A-Za-z0-9_]{36}", RegexOptions.Compiled);

        private string RedactNpmAccessToken(string str, string text)
        {
            return NpmAccessTokenPattern.IsMatch(str) ? NpmAccessTokenPattern.Replace(str, text) : str;
        }

        #endregion
    }
}