// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using NatsunekoLaboratory.TraceMate.Redactor.Rules.Interfaces;

namespace NatsunekoLaboratory.TraceMate.Redactor.Rules
{
    internal class AwsRule : IRedactionRule
    {
        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactAwsAccessKey,
                RedactAwsSecretAccessKey
            };
        }

        #region AccessKey

        // default value: https://docs.aws.amazon.com/ja_jp/general/latest/gr/aws-access-keys-best-practices.html
        private const string ExampleAwsAccessKey = "AKIAIOSFODNN7EXAMPLE";
        private static readonly Regex AwsAccessKeyPattern = new Regex("\b(A3T[A-Z0-9]|AKIA|AGPA|AIDA|AROA|AIPA|ANPA|ANVA|ASIA)[A-Z0-9]{16}\b", RegexOptions.Compiled);

        private string RedactAwsAccessKey(string str, string text)
        {
            if (AwsAccessKeyPattern.IsMatch(str))
            {
                var matches = AwsAccessKeyPattern.Matches(str);
                str = matches.Cast<Match>().Where(match => match.Value != ExampleAwsAccessKey).Aggregate(str, (current, match) => current.Replace(match.Value, text));
            }

            return str;
        }

        #endregion

        #region AccessSecretKey

        // default value: https://docs.aws.amazon.com/ja_jp/general/latest/gr/aws-access-keys-best-practices.html
        private const string ExampleAwsSecretAccessKey = "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY";

        // https://github.com/awslabs/git-secrets/blob/5e28df337746db4f070c84f7069d365bfd0d72a8/git-secrets#L239
        private const string Aws = "(?:AWS|aws|Aws)?_?";
        private const string Quote = "[\"']?";
        private const string Connect = "\\\\s*(?::|=>|=)\\\\s*";
        private static readonly Regex AwsSecretAccessKeyPattern = new Regex($"{Quote}{Aws}(?:SECRET|secret|Secret)_?(?:ACCESS|access|Access)_?(?:KEY|key|Key){Quote}{Connect}{Quote}([A-Za-z0-9/\\+=]{{40}}){Quote}\b", RegexOptions.Compiled);

        private string RedactAwsSecretAccessKey(string str, string text)
        {
            if (AwsSecretAccessKeyPattern.IsMatch(str))
            {
                var matches = AwsSecretAccessKeyPattern.Matches(str);
                str = matches.Cast<Match>().Where(match => match.Value != ExampleAwsSecretAccessKey).Aggregate(str, (current, match) => current.Replace(match.Value, text));
            }

            return str;
        }

        #endregion
    }
}