﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE file in the project root for more information.

using Xunit;

namespace Microsoft.VisualStudio.ProjectSystem.VS.LanguageServices.VisualBasic
{
    public class VisualBasicCodeDomProviderTests
    {
        [Fact]
        public void Constructor_DoesNotThrow()
        {
            new VisualBasicCodeDomProvider();
        }
    }
}
