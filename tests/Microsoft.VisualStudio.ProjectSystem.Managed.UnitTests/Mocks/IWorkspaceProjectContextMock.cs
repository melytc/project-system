﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE file in the project root for more information.

using Moq;

namespace Microsoft.VisualStudio.LanguageServices.ProjectSystem
{
    internal class IWorkspaceProjectContextMock : AbstractMock<IWorkspaceProjectContext>
    {
        public IWorkspaceProjectContextMock()
        {
            SetupAllProperties();
        }
    }
}
