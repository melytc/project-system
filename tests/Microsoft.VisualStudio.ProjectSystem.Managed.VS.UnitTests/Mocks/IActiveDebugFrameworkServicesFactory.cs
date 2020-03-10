﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE file in the project root for more information.

namespace Microsoft.VisualStudio.ProjectSystem.Debug
{
    internal static class IActiveDebugFrameworkServicesFactory
    {
        public static IActiveDebugFrameworkServices ImplementGetConfiguredProjectForActiveFrameworkAsync(ConfiguredProject? project)
        {
            var service = new IActiveDebugFrameworkServicesMock();
            service.ImplementGetConfiguredProjectForActiveFrameworkAsync(project);

            return service.Object;
        }
    }
}
