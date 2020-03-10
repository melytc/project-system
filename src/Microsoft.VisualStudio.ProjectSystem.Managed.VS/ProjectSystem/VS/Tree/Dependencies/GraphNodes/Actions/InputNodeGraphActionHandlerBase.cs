﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE file in the project root for more information.

using System.IO;
using Microsoft.VisualStudio.GraphModel;
using Microsoft.VisualStudio.GraphModel.Schemas;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.GraphNodes.ViewProviders;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Snapshot;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.GraphNodes.Actions
{
    /// <summary>
    ///     Base class for graph action handlers that operate on a set of input nodes,
    ///     each of which is backed by an <see cref="IDependency"/>.
    /// </summary>
    internal abstract class InputNodeGraphActionHandlerBase : GraphActionHandlerBase
    {
        protected InputNodeGraphActionHandlerBase(IAggregateDependenciesSnapshotProvider aggregateSnapshotProvider)
            : base(aggregateSnapshotProvider)
        {
        }

        protected abstract bool CanHandle(IGraphContext graphContext);

        protected abstract void ProcessInputNode(
            IGraphContext graphContext,
            GraphNode inputGraphNode,
            IDependency dependency,
            DependenciesSnapshot snapshot,
            IDependenciesGraphViewProvider viewProvider,
            string projectPath,
            ref bool trackChanges);

        public sealed override bool TryHandleRequest(IGraphContext graphContext)
        {
            if (!CanHandle(graphContext))
            {
                return false;
            }

            bool trackChanges = false;

            foreach (GraphNode inputGraphNode in graphContext.InputNodes)
            {
                if (graphContext.CancelToken.IsCancellationRequested)
                {
                    return trackChanges;
                }

                string? projectPath = inputGraphNode.Id.GetValue(CodeGraphNodeIdName.Assembly);

                if (Strings.IsNullOrEmpty(projectPath))
                {
                    continue;
                }

                IDependency? dependency = FindDependency(inputGraphNode, out DependenciesSnapshot? snapshot);

                if (dependency == null || snapshot == null)
                {
                    continue;
                }

                IDependenciesGraphViewProvider? viewProvider = FindViewProvider(dependency);

                if (viewProvider == null)
                {
                    continue;
                }

                using var scope = new GraphTransactionScope();
                ProcessInputNode(graphContext, inputGraphNode, dependency, snapshot, viewProvider, projectPath, ref trackChanges);

                scope.Complete();
            }

            return trackChanges;
        }

        private IDependency? FindDependency(GraphNode inputGraphNode, out DependenciesSnapshot? snapshot)
        {
            string? projectPath = inputGraphNode.Id.GetValue(CodeGraphNodeIdName.Assembly);

            if (Strings.IsNullOrWhiteSpace(projectPath))
            {
                snapshot = null;
                return null;
            }

            string projectFolder = Path.GetDirectoryName(projectPath);

            if (projectFolder == null)
            {
                snapshot = null;
                return null;
            }

            string? id = inputGraphNode.GetValue<string>(DependenciesGraphSchema.DependencyIdProperty);

            bool topLevel;

            if (id == null)
            {
                // this is top level node and it contains full path 
                id = inputGraphNode.Id.GetValue(CodeGraphNodeIdName.File);

                if (id == null)
                {
                    // No full path, so this must be a node generated by a different provider.
                    snapshot = null;
                    return null;
                }

                if (id.StartsWith(projectFolder, StringComparisons.Paths))
                {
                    int startIndex = projectFolder.Length;

                    // Trim backslashes (without allocating)
                    while (startIndex < id.Length && id[startIndex] == '\\')
                    {
                        startIndex++;
                    }

                    id = id.Substring(startIndex);
                }

                topLevel = true;
            }
            else
            {
                topLevel = false;
            }

            snapshot = AggregateSnapshotProvider.GetSnapshot(projectPath);

            return snapshot?.FindDependency(id, topLevel);
        }
    }
}
