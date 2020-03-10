﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS;
using Microsoft.VisualStudio.ProjectSystem.VS.Interop;

namespace Microsoft.VisualStudio.Shell.Interop
{
    [ProjectSystemContract(ProjectSystemContractScope.ProjectService, ProjectSystemContractProvider.Private, Cardinality = ImportCardinality.ExactlyOne)]
    internal interface IVsShellUtilitiesHelper
    {
        /// <summary>
        /// Returns the version of VS as defined by <see cref="VSAPropID.VSAPROPID_ProductSemanticVersion"/> with the trailing sem version stripped, or <see langword="null"/> on failure.
        /// </summary>
        Task<Version?> GetVSVersionAsync(IVsService<IVsAppId> vsAppIdService);

        /// <summary>
        /// Returns the local app data folder as defined by <see cref="__VSSPROPID4.VSSPROPID_LocalAppDataDir"/>.
        /// </summary>
        Task<string?> GetLocalAppDataFolderAsync(IVsService<IVsShell> vsShellService);
    }
}
