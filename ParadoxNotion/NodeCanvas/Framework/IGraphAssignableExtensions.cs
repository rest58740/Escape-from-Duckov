using System;
using System.Collections.Generic;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Services;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200000F RID: 15
	public static class IGraphAssignableExtensions
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00004F74 File Offset: 0x00003174
		public static Graph CheckInstance(this IGraphAssignable assignable)
		{
			if (assignable.subGraph == assignable.currentInstance)
			{
				return assignable.currentInstance;
			}
			Graph graph = null;
			if (assignable.instances == null)
			{
				assignable.instances = new Dictionary<Graph, Graph>();
			}
			if (!assignable.instances.TryGetValue(assignable.subGraph, ref graph))
			{
				graph = Graph.Clone<Graph>(assignable.subGraph, assignable.graph);
				assignable.instances[assignable.subGraph] = graph;
			}
			assignable.subGraph = graph;
			assignable.currentInstance = graph;
			return graph;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00004FF8 File Offset: 0x000031F8
		public static bool TryStartSubGraph(this IGraphAssignable assignable, Component agent, Action<bool> callback = null)
		{
			assignable.currentInstance = assignable.CheckInstance();
			if (assignable.currentInstance != null)
			{
				assignable.TryWriteAndBindMappedVariables();
				assignable.currentInstance.StartGraph(agent, assignable.graph.blackboard.parent, Graph.UpdateMode.Manual, delegate(bool result)
				{
					if (assignable.status == Status.Running)
					{
						assignable.TryReadAndUnbindMappedVariables();
					}
					if (callback != null)
					{
						callback.Invoke(result);
					}
				});
				return true;
			}
			return false;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005083 File Offset: 0x00003283
		public static bool TryStopSubGraph(this IGraphAssignable assignable)
		{
			if (assignable.currentInstance != null)
			{
				assignable.currentInstance.Stop(true);
				return true;
			}
			return false;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000050A2 File Offset: 0x000032A2
		public static bool TryPauseSubGraph(this IGraphAssignable assignable)
		{
			if (assignable.currentInstance != null)
			{
				assignable.currentInstance.Pause();
				return true;
			}
			return false;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000050C0 File Offset: 0x000032C0
		public static bool TryResumeSubGraph(this IGraphAssignable assignable)
		{
			if (assignable.currentInstance != null)
			{
				assignable.currentInstance.Resume();
				return true;
			}
			return false;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000050DE File Offset: 0x000032DE
		public static bool TryUpdateSubGraph(this IGraphAssignable assignable)
		{
			if (assignable.currentInstance != null && assignable.currentInstance.isRunning)
			{
				assignable.currentInstance.UpdateGraph(assignable.graph.deltaTime);
				return true;
			}
			return false;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005114 File Offset: 0x00003314
		public static void TryWriteAndBindMappedVariables(this IGraphAssignable assignable)
		{
			if (!assignable.currentInstance.allowBlackboardOverrides || assignable.variablesMap == null)
			{
				return;
			}
			for (int i = 0; i < assignable.variablesMap.Count; i++)
			{
				BBMappingParameter bbmappingParameter = assignable.variablesMap[i];
				if (!bbmappingParameter.isNone)
				{
					Variable variableByID = assignable.currentInstance.blackboard.GetVariableByID(bbmappingParameter.targetSubGraphVariableID);
					if (variableByID != null && variableByID.isExposedPublic && !variableByID.isPropertyBound)
					{
						if (bbmappingParameter.canWrite)
						{
							variableByID.value = bbmappingParameter.value;
						}
						if (bbmappingParameter.canRead)
						{
							variableByID.onValueChanged -= new Action<object>(bbmappingParameter.SetValue);
							variableByID.onValueChanged += new Action<object>(bbmappingParameter.SetValue);
						}
					}
				}
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000051D4 File Offset: 0x000033D4
		public static void TryReadAndUnbindMappedVariables(this IGraphAssignable assignable)
		{
			if (!assignable.currentInstance.allowBlackboardOverrides || assignable.variablesMap == null)
			{
				return;
			}
			for (int i = 0; i < assignable.variablesMap.Count; i++)
			{
				BBMappingParameter bbmappingParameter = assignable.variablesMap[i];
				if (!bbmappingParameter.isNone)
				{
					Variable variableByID = assignable.currentInstance.blackboard.GetVariableByID(bbmappingParameter.targetSubGraphVariableID);
					if (variableByID != null && variableByID.isExposedPublic && !variableByID.isPropertyBound)
					{
						if (bbmappingParameter.canRead)
						{
							bbmappingParameter.value = variableByID.value;
						}
						variableByID.onValueChanged -= new Action<object>(bbmappingParameter.SetValue);
					}
				}
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005274 File Offset: 0x00003474
		public static void ValidateSubGraphAndParameters(this IGraphAssignable assignable)
		{
			if (!Threader.applicationIsPlaying && (assignable.subGraph == null || !assignable.subGraph.allowBlackboardOverrides || assignable.subGraph.blackboard.variables.Count == 0))
			{
				assignable.variablesMap = null;
			}
		}
	}
}
