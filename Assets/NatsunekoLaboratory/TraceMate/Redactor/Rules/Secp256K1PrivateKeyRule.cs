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
    internal class Secp256K1PrivateKeyRule : IRedactionRule
    {
        private static readonly Regex Secp256KPrivateKeyPattern = new Regex("[0-9a-f]{64}", RegexOptions.Compiled);

        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactPrivateKey
            };
        }

        private string RedactPrivateKey(string str, string text)
        {
            return Secp256KPrivateKeyPattern.IsMatch(str) ? Secp256KPrivateKeyPattern.Replace(str, text) : str;
        }
    }
}