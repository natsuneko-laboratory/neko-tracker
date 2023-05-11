// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using NatsunekoLaboratory.TraceMate.Redactor.Rules.Interfaces;

namespace NatsunekoLaboratory.TraceMate.Redactor.Rules
{
    internal class UsernameRule : IRedactionRule
    {
        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactUsername,
                RedactShortUsername,
                RedactDomainUsername,
                RedactComputerName
            };
        }

        private string RedactUsername(string text, string str)
        {
            var user = Environment.UserName;
            return text.Replace(user, str);
        }

        private string RedactShortUsername(string text, string str)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var user = Environment.UserName;
                if (user.Length > 8)
                    return text; // already redacted
                if (user.Length == 8)
                    return text.Replace(user.ToUpperInvariant(), str);

                var @short = new Regex($"{user.Substring(0, 6).ToUpperInvariant()}~\\d");
                return @short.IsMatch(text) ? @short.Replace(text, str) : text;
            }

            return text;
        }

        private string RedactDomainUsername(string text, string str)
        {
            try
            {
                var domain = Environment.UserDomainName;
                return text.Replace(domain, str);
            }
            catch
            {
                return text;
            }
        }

        private string RedactComputerName(string text, string str)
        {
            try
            {
                var name = Environment.MachineName;
                return text.Replace(name, str);
            }
            catch
            {
                return text;
            }
        }
    }
}