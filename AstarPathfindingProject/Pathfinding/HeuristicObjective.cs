using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Util;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x0200009F RID: 159
	[BurstCompile]
	public readonly struct HeuristicObjective
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00018B74 File Offset: 0x00016D74
		public bool hasHeuristic
		{
			get
			{
				return this.heuristic != Heuristic.None;
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00018B84 File Offset: 0x00016D84
		public HeuristicObjective(int3 point, Heuristic heuristic, float heuristicScale)
		{
			this.mx = point;
			this.mn = point;
			this.heuristic = heuristic;
			this.heuristicScale = heuristicScale;
			this.euclideanEmbeddingCosts = default(UnsafeSpan<uint>);
			this.euclideanEmbeddingPivots = 0U;
			this.targetNodeIndex = 0U;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00018BCC File Offset: 0x00016DCC
		public HeuristicObjective(int3 point, Heuristic heuristic, float heuristicScale, uint targetNodeIndex, EuclideanEmbedding euclideanEmbedding)
		{
			this.mx = point;
			this.mn = point;
			this.heuristic = heuristic;
			this.heuristicScale = heuristicScale;
			this.euclideanEmbeddingCosts = ((euclideanEmbedding != null) ? euclideanEmbedding.costs.AsUnsafeSpanNoChecks<uint>() : default(UnsafeSpan<uint>));
			this.euclideanEmbeddingPivots = (uint)((euclideanEmbedding != null) ? euclideanEmbedding.pivotCount : 0);
			this.targetNodeIndex = targetNodeIndex;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00018C34 File Offset: 0x00016E34
		public HeuristicObjective(int3 mn, int3 mx, Heuristic heuristic, float heuristicScale, uint targetNodeIndex, EuclideanEmbedding euclideanEmbedding)
		{
			this.mn = mn;
			this.mx = mx;
			this.heuristic = heuristic;
			this.heuristicScale = heuristicScale;
			this.euclideanEmbeddingCosts = ((euclideanEmbedding != null) ? euclideanEmbedding.costs.AsUnsafeSpanNoChecks<uint>() : default(UnsafeSpan<uint>));
			this.euclideanEmbeddingPivots = (uint)((euclideanEmbedding != null) ? euclideanEmbedding.pivotCount : 0);
			this.targetNodeIndex = targetNodeIndex;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00018C9B File Offset: 0x00016E9B
		public int Calculate(int3 point, uint nodeIndex)
		{
			return HeuristicObjective.Calculate(this, ref point, nodeIndex);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00018CA6 File Offset: 0x00016EA6
		[BurstCompile]
		public static int Calculate(in HeuristicObjective objective, ref int3 point, uint nodeIndex)
		{
			return HeuristicObjective.Calculate_000004C3$BurstDirectCall.Invoke(objective, ref point, nodeIndex);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00018CB0 File Offset: 0x00016EB0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int Calculate$BurstManaged(in HeuristicObjective objective, ref int3 point, uint nodeIndex)
		{
			int3 rhs = math.clamp(point, objective.mn, objective.mx);
			int3 @int = point - rhs;
			int num;
			switch (objective.heuristic)
			{
			case Heuristic.Manhattan:
				num = (int)((float)math.csum(math.abs(@int)) * objective.heuristicScale);
				goto IL_EC;
			case Heuristic.DiagonalManhattan:
			{
				@int = math.abs(@int);
				int x = @int.x;
				int y = @int.y;
				int z = @int.z;
				if (x > y)
				{
					Memory.Swap<int>(ref x, ref y);
				}
				if (y > z)
				{
					Memory.Swap<int>(ref y, ref z);
				}
				if (x > y)
				{
					Memory.Swap<int>(ref x, ref y);
				}
				num = (int)(objective.heuristicScale * (1.7321f * (float)x + 1.4142f * (float)(y - x) + (float)(z - y - x)));
				goto IL_EC;
			}
			case Heuristic.Euclidean:
				num = (int)(math.length(@int) * objective.heuristicScale);
				goto IL_EC;
			}
			num = 0;
			IL_EC:
			if (objective.euclideanEmbeddingPivots > 0U)
			{
				num = math.max(num, (int)EuclideanEmbedding.GetHeuristic(objective.euclideanEmbeddingCosts, objective.euclideanEmbeddingPivots, nodeIndex, objective.targetNodeIndex));
			}
			return num;
		}

		// Token: 0x0400034D RID: 845
		private readonly int3 mn;

		// Token: 0x0400034E RID: 846
		private readonly int3 mx;

		// Token: 0x0400034F RID: 847
		private readonly Heuristic heuristic;

		// Token: 0x04000350 RID: 848
		private readonly float heuristicScale;

		// Token: 0x04000351 RID: 849
		private readonly UnsafeSpan<uint> euclideanEmbeddingCosts;

		// Token: 0x04000352 RID: 850
		private readonly uint euclideanEmbeddingPivots;

		// Token: 0x04000353 RID: 851
		private readonly uint targetNodeIndex;

		// Token: 0x020000A0 RID: 160
		// (Invoke) Token: 0x060004FD RID: 1277
		internal delegate int Calculate_000004C3$PostfixBurstDelegate(in HeuristicObjective objective, ref int3 point, uint nodeIndex);

		// Token: 0x020000A1 RID: 161
		internal static class Calculate_000004C3$BurstDirectCall
		{
			// Token: 0x06000500 RID: 1280 RVA: 0x00018DD2 File Offset: 0x00016FD2
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (HeuristicObjective.Calculate_000004C3$BurstDirectCall.Pointer == 0)
				{
					HeuristicObjective.Calculate_000004C3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(HeuristicObjective.Calculate_000004C3$BurstDirectCall.DeferredCompilation, methodof(HeuristicObjective.Calculate$BurstManaged(HeuristicObjective*, int3*, uint)).MethodHandle, typeof(HeuristicObjective.Calculate_000004C3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = HeuristicObjective.Calculate_000004C3$BurstDirectCall.Pointer;
			}

			// Token: 0x06000501 RID: 1281 RVA: 0x00018E00 File Offset: 0x00017000
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				HeuristicObjective.Calculate_000004C3$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000502 RID: 1282 RVA: 0x00018E18 File Offset: 0x00017018
			public unsafe static void Constructor()
			{
				HeuristicObjective.Calculate_000004C3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(HeuristicObjective.Calculate(HeuristicObjective*, int3*, uint)).MethodHandle);
			}

			// Token: 0x06000503 RID: 1283 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x00018E29 File Offset: 0x00017029
			// Note: this type is marked as 'beforefieldinit'.
			static Calculate_000004C3$BurstDirectCall()
			{
				HeuristicObjective.Calculate_000004C3$BurstDirectCall.Constructor();
			}

			// Token: 0x06000505 RID: 1285 RVA: 0x00018E30 File Offset: 0x00017030
			public static int Invoke(in HeuristicObjective objective, ref int3 point, uint nodeIndex)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = HeuristicObjective.Calculate_000004C3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Pathfinding.HeuristicObjective&,Unity.Mathematics.int3&,System.UInt32), ref objective, ref point, nodeIndex, functionPointer);
					}
				}
				return HeuristicObjective.Calculate$BurstManaged(objective, ref point, nodeIndex);
			}

			// Token: 0x04000354 RID: 852
			private static IntPtr Pointer;

			// Token: 0x04000355 RID: 853
			private static IntPtr DeferredCompilation;
		}
	}
}
