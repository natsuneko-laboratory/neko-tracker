// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NatsunekoLaboratory.TraceMate.Redactor.Rules.Interfaces
{
    internal interface IRedactionRule
    {
        List<Func<string, string, string>> Redactors();
    }
}