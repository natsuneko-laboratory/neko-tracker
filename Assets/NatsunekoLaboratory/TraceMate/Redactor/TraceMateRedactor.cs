// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NatsunekoLaboratory.TraceMate.Redactor.Rules.Interfaces;

namespace NatsunekoLaboratory.TraceMate.Redactor
{
    /// <summary>
    ///     TraceMate Redactor is redacting sensitive data from string.
    ///     based on the following repositories/services:
    ///     * https://github.com/secretlint/secretlint.
    ///     * https://docs.aws.amazon.com/AmazonCloudWatch/latest/logs/protect-sensitive-log-data-types.html
    ///     Logs SHOULD NOT contains the sensitive data, but not all developers are aware of it.
    ///     TraceMateRedactor aims to mask the relevant data as much as possible, even if there is confidential information in
    ///     the log data.
    /// </summary>
    internal class TraceMateRedactor
    {
        private static readonly List<Func<string, string, string>> Actions;

        static TraceMateRedactor()
        {
            Actions = new List<Func<string, string, string>>();

            foreach (var rule in GetRedactionRules())
                Actions.AddRange(rule.Redactors());
        }

        public static string Redact(string str)
        {
            return Actions.Aggregate(str, (current, action) => action(current, "[REDACTED]"));
        }

        private static List<IRedactionRule> GetRedactionRules()
        {
            return typeof(TraceMateRedactor).Assembly
                                            .GetTypes()
                                            .Where(w => typeof(IRedactionRule).IsAssignableFrom(w))
                                            .Where(w => w.GetConstructors().Any(c => c.GetParameters().Length == 0))
                                            .Select(Activator.CreateInstance)
                                            .Cast<IRedactionRule>()
                                            .ToList();
        }
    }
}