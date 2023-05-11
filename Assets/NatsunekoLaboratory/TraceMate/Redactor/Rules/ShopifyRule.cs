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
    internal class ShopifyRule : IRedactionRule
    {
        private static readonly Regex KeyPattern = new Regex("(shppa|shpca|shpat|shpss)_[a-zA-Z0-9]{32,64}", RegexOptions.Compiled);

        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactKey
            };
        }

        private string RedactKey(string str, string text)
        {
            return KeyPattern.IsMatch(str) ? KeyPattern.Replace(str, text) : str;
        }
    }
}