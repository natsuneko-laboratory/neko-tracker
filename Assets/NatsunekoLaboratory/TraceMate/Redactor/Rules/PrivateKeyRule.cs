// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NatsunekoLaboratory.TraceMate.Redactor.Rules.Interfaces;

using Unity.Plastic.Antlr3.Runtime.Misc;

namespace NatsunekoLaboratory.TraceMate.Redactor.Rules
{
    internal class PrivateKeyRule : IRedactionRule
    {
        private static readonly Regex PrivateKeyPattern = new Regex("-----BEGIN\\s?(DSA|RSA|EC|PGP|OPENSSH|[A-Z]{2,16})?\\s?PRIVATE KEY(\\sBLOCK)?-----[\\s\\S]+-----", RegexOptions.Compiled | RegexOptions.Multiline);

        public List<Func<string, string, string>> Redactors()
        {
            return new ListStack<Func<string, string, string>>
            {
                RedactPrivateKey
            };
        }

        private string RedactPrivateKey(string str, string text)
        {
            return PrivateKeyPattern.IsMatch(str) ? PrivateKeyPattern.Replace(str, text) : str;
        }
    }
}