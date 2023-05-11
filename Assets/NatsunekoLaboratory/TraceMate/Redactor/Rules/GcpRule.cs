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
    internal class GcpRule : IRedactionRule
    {
        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactPrivateKey
            };
        }

        #region Private Key

        private const string Key = "(private_key_id|private_key)";
        private const string Body = "-----BEGIN\\s*(DSA|RSA|EC|PGP|OPENSSH)?\\s*PRIVATE KEY";
        private const string Connect = "\\\\s*(?::|:|=>|=)\\\\s*";
        private static readonly Regex JsonLikePattern = new Regex($"{Key}{Connect}{Body}", RegexOptions.Compiled | RegexOptions.Multiline);

        private string RedactPrivateKey(string str, string text)
        {
            return JsonLikePattern.IsMatch(str) ? JsonLikePattern.Replace(str, text) : str;
        }

        #endregion
    }
}