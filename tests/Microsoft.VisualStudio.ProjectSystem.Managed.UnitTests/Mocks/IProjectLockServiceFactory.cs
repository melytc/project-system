// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE file in the project root for more information.

using Moq;

namespace Microsoft.VisualStudio.ProjectSystem
{
    internal static class IProjectLockServiceFactory
    {
        public static IProjectLockService Create()
        {
            return Mock.Of<IProjectLockService>();
        }
    }
}
