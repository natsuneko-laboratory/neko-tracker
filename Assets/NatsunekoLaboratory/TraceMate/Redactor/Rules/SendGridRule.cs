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
    internal class SendGridRule : IRedactionRule
    {
        private static readonly Regex SendGridKeyPattern = new Regex("SG\\.\\w{1,128}\\.\\w{1,128}-\\w{1,128}", RegexOptions.Compiled);

        public List<Func<string, string, string>> Redactors()
        {
            return new List<Func<string, string, string>>
            {
                RedactSendGridKey
            };
        }

        private string RedactSendGridKey(string str, string text)
        {
            return SendGridKeyPattern.IsMatch(str) ? SendGridKeyPattern.Replace(str, text) : str;
        }
    }
}