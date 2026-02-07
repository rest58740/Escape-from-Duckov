using System;
using Pathfinding;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh;
using Pathfinding.RVO;
using Pathfinding.Util;
using UnityEngine;

// Token: 0x020002DB RID: 731
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06001122 RID: 4386 RVA: 0x0006B388 File Offset: 0x00069588
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		Polygon.ContainsPoint_000002E3$BurstDirectCall.Initialize();
		Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.Initialize();
		Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.Initialize();
		BinaryHeap.Add_000002FF$BurstDirectCall.Initialize();
		BinaryHeap.Remove_00000302$BurstDirectCall.Initialize();
		HeuristicObjective.Calculate_000004C3$BurstDirectCall.Initialize();
		Path.OpenCandidateConnectionBurst_00000500$BurstDirectCall.Initialize();
		TriangleMeshNode.InterpolateEdge_00000761$BurstDirectCall.Initialize();
		TriangleMeshNode.OpenSingleEdgeBurst_00000766$BurstDirectCall.Initialize();
		TriangleMeshNode.CalculateBestEdgePosition_00000767$BurstDirectCall.Initialize();
		NavmeshCutJobs.CalculateContour_000008B4$BurstDirectCall.Initialize();
		Funnel.Calculate_00000954$BurstDirectCall.Initialize();
		Funnel.FunnelState.PushStart_00000960$BurstDirectCall.Initialize();
		Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.Initialize();
		PathTracer.ContainsAndProject_00000992$BurstDirectCall.Initialize();
		PathTracer.EstimateRemainingPath_000009A5$BurstDirectCall.Initialize();
		PathTracer.RemainingDistanceLowerBound_000009A9$BurstDirectCall.Initialize();
		ColliderMeshBuilder2D.GenerateMeshesFromShapes_00000A8B$BurstDirectCall.Initialize();
		RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.Initialize();
		RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.Initialize();
		TileHandler.ConvertVerticesAndSnapToTileBoundaries_00000AD4$BurstDirectCall.Initialize();
		TileHandler.CutTiles_00000AD5$BurstDirectCall.Initialize();
		HierarchicalBitset.Iterator.MoveNextBurst_00000D4C$BurstDirectCall.Initialize();
		BBTree.Build_00000DDE$BurstDirectCall.Initialize();
		BBTree.ProjectionParams.SquaredRectPointDistanceOnPlane_00000DE9$BurstDirectCall.Initialize();
		BBTree.Initialize$NearbyNodesIterator_MoveNext_00000DF0$BurstDirectCall();
		MeshUtility.MakeTrianglesClockwise_00000EBC$BurstDirectCall.Initialize();
		RVOObstacleCache.TraceContours_00000F8F$BurstDirectCall.Initialize();
	}
}
