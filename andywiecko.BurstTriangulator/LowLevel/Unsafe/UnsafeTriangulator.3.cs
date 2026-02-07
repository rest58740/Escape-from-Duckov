using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200001C RID: 28
	internal readonly struct UnsafeTriangulator<[IsUnmanaged] T, [IsUnmanaged] T2, [IsUnmanaged] TBig, [IsUnmanaged] TTransform, [IsUnmanaged] TUtils> where T : struct, ValueType, IComparable<T> where T2 : struct, ValueType where TBig : struct, ValueType, IComparable<TBig> where TTransform : struct, ValueType, ITransform<TTransform, T, T2> where TUtils : struct, ValueType, IUtils<T, T2, TBig>
	{
		// Token: 0x06000080 RID: 128 RVA: 0x000031C0 File Offset: 0x000013C0
		public void Triangulate(InputData<T2> input, OutputData<T2> output, Args args, Allocator allocator)
		{
			NativeReference<Status> status = default(NativeReference<Status>);
			NativeList<T2> positions = default(NativeList<T2>);
			NativeList<int> halfedges = default(NativeList<int>);
			NativeList<HalfedgeState> constrainedHalfedges = default(NativeList<HalfedgeState>);
			if (!output.Status.IsCreated)
			{
				status = new NativeReference<Status>(allocator, NativeArrayOptions.ClearMemory);
				output.Status = status;
			}
			if (!output.Positions.IsCreated)
			{
				positions = new NativeList<T2>(16384, allocator);
				output.Positions = positions;
			}
			if (!output.Halfedges.IsCreated)
			{
				halfedges = new NativeList<int>(98304, allocator);
				output.Halfedges = halfedges;
			}
			if (!output.ConstrainedHalfedges.IsCreated)
			{
				constrainedHalfedges = new NativeList<HalfedgeState>(98304, allocator);
				output.ConstrainedHalfedges = constrainedHalfedges;
			}
			output.Status.Value = Status.Ok;
			output.Triangles.Clear();
			output.Positions.Clear();
			output.Halfedges.Clear();
			output.ConstrainedHalfedges.Clear();
			NativeArray<T2> localHoles;
			TTransform lt;
			this.PreProcessInputStep(input, output, args, out localHoles, out lt, allocator);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ValidateInputStep(input, output, args).Execute();
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.DelaunayTriangulationStep(output, args).Execute(allocator);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ConstrainEdgesStep(input, output, args).Execute(allocator);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep(output, args, localHoles).Execute(allocator, input.ConstraintEdges.IsCreated);
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep(output, args, lt).Execute(allocator, args.RefineMesh, !input.ConstraintEdges.IsCreated || !args.RestoreBoundary);
			this.PostProcessInputStep(output, args, lt);
			Status value = output.Status.Value;
			if (localHoles.IsCreated)
			{
				localHoles.Dispose();
			}
			if (status.IsCreated)
			{
				status.Dispose();
			}
			if (positions.IsCreated)
			{
				positions.Dispose();
			}
			if (halfedges.IsCreated)
			{
				halfedges.Dispose();
			}
			if (constrainedHalfedges.IsCreated)
			{
				constrainedHalfedges.Dispose();
			}
			if (args.Verbose && value.IsError)
			{
				Debug.LogError(value.ToFixedString());
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000033F4 File Offset: 0x000015F4
		public void PlantHoleSeeds(InputData<T2> input, OutputData<T2> output, Args args, Allocator allocator)
		{
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep(input, output, args).Execute(allocator, true);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003414 File Offset: 0x00001614
		public void RefineMesh(OutputData<T2> output, Allocator allocator, T area2Threshold, T angleThreshold, bool constrainBoundary = false)
		{
			new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep(output, area2Threshold, angleThreshold).Execute(allocator, true, constrainBoundary);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003438 File Offset: 0x00001638
		private void PreProcessInputStep(InputData<T2> input, OutputData<T2> output, Args args, out NativeArray<T2> localHoles, out TTransform lt, Allocator allocator)
		{
			using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.PreProcessInputStep.Auto())
			{
				NativeList<T2> positions = output.Positions;
				positions.ResizeUninitialized(input.Positions.Length);
				if (args.Preprocessor == Preprocessor.PCA || args.Preprocessor == Preprocessor.COM)
				{
					TTransform ttransform2;
					if (args.Preprocessor != Preprocessor.PCA)
					{
						TTransform ttransform = default(TTransform);
						ttransform2 = ttransform.CalculateLocalTransformation(input.Positions);
					}
					else
					{
						TTransform ttransform = default(TTransform);
						ttransform2 = ttransform.CalculatePCATransformation(input.Positions);
					}
					lt = ttransform2;
					for (int i = 0; i < input.Positions.Length; i++)
					{
						positions[i] = lt.Transform(input.Positions[i]);
					}
					localHoles = (input.HoleSeeds.IsCreated ? new NativeArray<T2>(input.HoleSeeds.Length, allocator, NativeArrayOptions.ClearMemory) : default(NativeArray<T2>));
					for (int j = 0; j < input.HoleSeeds.Length; j++)
					{
						localHoles[j] = lt.Transform(input.HoleSeeds[j]);
					}
				}
				else
				{
					if (args.Preprocessor != Preprocessor.None)
					{
						throw new ArgumentException();
					}
					positions.CopyFrom(input.Positions);
					localHoles = (input.HoleSeeds.IsCreated ? new NativeArray<T2>(input.HoleSeeds, allocator) : default(NativeArray<T2>));
					TTransform ttransform = default(TTransform);
					lt = ttransform.Identity;
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003610 File Offset: 0x00001810
		private void PostProcessInputStep(OutputData<T2> output, Args args, TTransform lt)
		{
			if (args.Preprocessor == Preprocessor.None)
			{
				return;
			}
			using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.PostProcessInputStep.Auto())
			{
				TTransform ttransform = lt.Inverse();
				for (int i = 0; i < output.Positions.Length; i++)
				{
					output.Positions[i] = ttransform.Transform(output.Positions[i]);
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000036A0 File Offset: 0x000018A0
		internal static bool AngleIsTooSmall(T2 pA, T2 pB, T2 pC, T minimumAngle)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T v = tutils.cos(minimumAngle);
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t = tutils.normalizesafe(tutils2.diff(pB, pA));
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t2 = tutils.normalizesafe(tutils2.diff(pC, pB));
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 t3 = tutils.normalizesafe(tutils2.diff(pA, pC));
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 a = t;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a2 = tutils2.dot(a, tutils3.neg(t3));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 a3 = t2;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T b = tutils2.dot(a3, tutils3.neg(t));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 a4 = t3;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.anygreaterthan(a2, b, tutils2.dot(a4, tutils3.neg(t2)), v);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000037D8 File Offset: 0x000019D8
		internal static T Area2(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 a2 = tutils2.diff(b, a);
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Cross(a2, tutils2.diff(c, a)));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003828 File Offset: 0x00001A28
		private static T Cross(T2 a, T2 b)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a2 = tutils4.X(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig a3 = tutils3.mul(a2, tutils4.Y(b));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a4 = tutils4.Y(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.Cast(tutils2.diff(a3, tutils3.mul(a4, tutils4.X(b))));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000038D4 File Offset: 0x00001AD4
		private static TBig CircumRadiusSq(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.distancesq(tutils2.CircumCenter(a, b, c), a);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000390C File Offset: 0x00001B0C
		private static ValueTuple<T2, T> CalculateCircumCircle(int i, int j, int k, NativeArray<T2> positions)
		{
			T2 t = positions[i];
			T2 t2 = positions[j];
			T2 t3 = positions[k];
			T2 a = t;
			T2 b = t2;
			T2 c = t3;
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T2 item = tutils.CircumCenter(a, b, c);
			tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return new ValueTuple<T2, T>(item, tutils.Cast(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CircumRadiusSq(a, b, c)));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003974 File Offset: 0x00001B74
		private static bool ccw(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a2 = tutils4.Y(c);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a3 = tutils3.diff(a2, tutils4.Y(a));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a4 = tutils4.X(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig a5 = tutils2.mul(a3, tutils3.diff(a4, tutils4.X(a)));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a6 = tutils4.Y(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a7 = tutils3.diff(a6, tutils4.Y(a));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a8 = tutils4.X(c);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.greater(a5, tutils2.mul(a7, tutils3.diff(a8, tutils4.X(a))));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003AA6 File Offset: 0x00001CA6
		internal static bool EdgeEdgeIntersection(T2 a0, T2 a1, T2 b0, T2 b1)
		{
			return UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(a0, a1, b0) != UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(a0, a1, b1) && UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(b0, b1, a0) != UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.ccw(b0, b1, a1);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003AD1 File Offset: 0x00001CD1
		private static int NextHalfedge(int he)
		{
			if (he % 3 != 2)
			{
				return he + 1;
			}
			return he - 2;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003AE0 File Offset: 0x00001CE0
		internal static bool IsConvexQuadrilateral(T2 a, T2 b, T2 c, T2 d)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig a2 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(a, c, b));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			if (tutils.greater(a2, tutils2.EPSILON()))
			{
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TBig a3 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(a, c, d));
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				if (tutils.greater(a3, tutils2.EPSILON()))
				{
					tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TBig a4 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(b, d, a));
					tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (tutils.greater(a4, tutils2.EPSILON()))
					{
						tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						TBig a5 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(b, d, c));
						tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (tutils.greater(a5, tutils2.EPSILON()))
						{
							return UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.EdgeEdgeIntersection(a, c, b, d);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003C0C File Offset: 0x00001E0C
		private static TBig Orient2dFast(T2 a, T2 b, T2 c)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a2 = tutils4.Y(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a3 = tutils3.diff(a2, tutils4.Y(c));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a4 = tutils4.X(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig a5 = tutils2.mul(a3, tutils3.diff(a4, tutils4.X(c)));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a6 = tutils4.X(a);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a7 = tutils3.diff(a6, tutils4.X(c));
			tutils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			T a8 = tutils4.Y(b);
			tutils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			return tutils.diff(a5, tutils2.mul(a7, tutils3.diff(a8, tutils4.Y(c))));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003D40 File Offset: 0x00001F40
		internal static bool PointLineSegmentIntersection(T2 a, T2 b0, T2 b1)
		{
			TUtils tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TUtils tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			TBig a2 = tutils2.abs(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(a, b0, b1));
			tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
			if (tutils.le(a2, tutils2.EPSILON()))
			{
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				bool2 lhs = tutils.ge(a, tutils2.min(b0, b1));
				tutils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				tutils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				return math.all(lhs & tutils.le(a, tutils2.max(b0, b1)));
			}
			return false;
		}

		// Token: 0x04000066 RID: 102
		private static readonly TUtils utils;

		// Token: 0x0200001D RID: 29
		private readonly struct Markers
		{
			// Token: 0x04000067 RID: 103
			public static readonly ProfilerMarker PreProcessInputStep = new ProfilerMarker("PreProcessInputStep");

			// Token: 0x04000068 RID: 104
			public static readonly ProfilerMarker PostProcessInputStep = new ProfilerMarker("PostProcessInputStep");

			// Token: 0x04000069 RID: 105
			public static readonly ProfilerMarker ValidateInputStep = new ProfilerMarker("ValidateInputStep");

			// Token: 0x0400006A RID: 106
			public static readonly ProfilerMarker DelaunayTriangulationStep = new ProfilerMarker("DelaunayTriangulationStep");

			// Token: 0x0400006B RID: 107
			public static readonly ProfilerMarker ConstrainEdgesStep = new ProfilerMarker("ConstrainEdgesStep");

			// Token: 0x0400006C RID: 108
			public static readonly ProfilerMarker PlantingSeedStep = new ProfilerMarker("PlantingSeedStep");

			// Token: 0x0400006D RID: 109
			public static readonly ProfilerMarker RefineMeshStep = new ProfilerMarker("RefineMeshStep");
		}

		// Token: 0x0200001E RID: 30
		private struct ValidateInputStep
		{
			// Token: 0x06000092 RID: 146 RVA: 0x00003E68 File Offset: 0x00002068
			public ValidateInputStep(InputData<T2> input, OutputData<T2> output, Args args)
			{
				this.positions = output.Positions.AsArray().AsReadOnly();
				this.status = output.Status;
				this.args = args;
				this.constraints = input.ConstraintEdges.AsReadOnly();
				this.constraintTypes = input.ConstraintEdgeTypes.AsReadOnly();
				this.holes = input.HoleSeeds.AsReadOnly();
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00003ED8 File Offset: 0x000020D8
			public void Execute()
			{
				if (!this.args.ValidateInput)
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.ValidateInputStep.Auto())
				{
					this.ValidateArgs();
					this.ValidatePositions();
					this.ValidateConstraints();
					this.ValidateHoles();
				}
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00003F3C File Offset: 0x0000213C
			private void ValidateArgs()
			{
				if (this.args.AutoHolesAndBoundary && !this.constraints.IsCreated)
				{
					this.status.Value = Status.ConstraintEdgesMissingForAutoHolesAndBoundary;
				}
				if (this.args.RestoreBoundary && !this.constraints.IsCreated)
				{
					this.status.Value = Status.ConstraintEdgesMissingForRestoreBoundary;
				}
				if (this.args.RefineMesh)
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (!utils.SupportsRefinement())
					{
						this.status.Value = Status.RefinementNotSupportedForCoordinateType;
					}
				}
				if (this.constraints.IsCreated && this.args.SloanMaxIters < 1)
				{
					this.status.Value = Status.SloanMaxItersMustBePositive(this.args.SloanMaxIters);
				}
				if (this.args.RefineMesh && this.args.RefinementThresholdArea < 0f)
				{
					this.status.Value = Status.RefinementThresholdAreaMustBePositive;
				}
				if ((this.args.RefineMesh && this.args.RefinementThresholdAngle < 0f) || this.args.RefinementThresholdAngle > 0.7853982f)
				{
					this.status.Value = Status.RefinementThresholdAngleOutOfRange;
				}
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00004078 File Offset: 0x00002278
			private void ValidatePositions()
			{
				if (this.positions.Length < 3)
				{
					this.status.Value = Status.PositionsLengthLessThan3(this.positions.Length);
					return;
				}
				for (int i = 0; i < this.positions.Length; i++)
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (math.any(!utils.isfinite(this.positions[i])))
					{
						this.status.Value = Status.PositionsMustBeFinite(i);
						return;
					}
					T2 v = this.positions[i];
					for (int j = i + 1; j < this.positions.Length; j++)
					{
						T2 w = this.positions[j];
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (math.all(utils.eq(v, w)))
						{
							this.status.Value = Status.DuplicatePosition(i);
							return;
						}
					}
				}
			}

			// Token: 0x06000096 RID: 150 RVA: 0x0000416C File Offset: 0x0000236C
			private void ValidateConstraints()
			{
				if (!this.constraints.IsCreated)
				{
					return;
				}
				if (this.constraints.Length % 2 != 0)
				{
					this.status.Value = Status.ConstraintsLengthNotDivisibleBy2(this.constraints.Length);
					return;
				}
				if (this.constraintTypes.IsCreated && this.constraintTypes.Length * 2 != this.constraints.Length)
				{
					this.status.Value = Status.ConstraintArrayLengthMismatch(this.constraints.Length, this.constraintTypes.Length);
					return;
				}
				for (int i = 0; i < this.constraints.Length / 2; i++)
				{
					int num = this.constraints[2 * i];
					int num2 = this.constraints[2 * i + 1];
					int num3 = num;
					int num4 = num2;
					int length = this.positions.Length;
					if (num3 >= length || num3 < 0 || num4 >= length || num4 < 0)
					{
						this.status.Value = Status.ConstraintOutOfBounds(i, new int2(num3, num4), length);
						return;
					}
					if (num3 == num4)
					{
						this.status.Value = Status.ConstraintSelfLoop(i, new int2(num3, num4));
						return;
					}
				}
				for (int j = 0; j < this.constraints.Length / 2; j++)
				{
					int num = this.constraints[2 * j];
					int num5 = this.constraints[2 * j + 1];
					int num6 = num;
					int num7 = num5;
					T2 t = this.positions[num6];
					T2 t2 = this.positions[num7];
					T2 t3 = t;
					T2 t4 = t2;
					for (int k = j + 1; k < this.constraints.Length / 2; k++)
					{
						num = this.constraints[2 * k];
						int num8 = this.constraints[2 * k + 1];
						int num9 = num;
						int num10 = num8;
						if ((num6 == num9 && num7 == num10) || (num6 == num10 && num7 == num9))
						{
							this.status.Value = Status.DuplicateConstraint(j, k);
							return;
						}
						if (num6 != num9 && num6 != num10 && num7 != num9 && num7 != num10)
						{
							t = this.positions[num9];
							T2 t5 = this.positions[num10];
							T2 t6 = t;
							T2 t7 = t5;
							if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.EdgeEdgeIntersection(t3, t4, t6, t7) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t3, t6, t7) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t4, t6, t7) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t6, t3, t4) && !UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(t7, t3, t4))
							{
								this.status.Value = Status.ConstraintIntersection(j, k);
								return;
							}
						}
					}
				}
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00004414 File Offset: 0x00002614
			private void ValidateHoles()
			{
				if (!this.holes.IsCreated)
				{
					return;
				}
				if (!this.constraints.IsCreated)
				{
					this.status.Value = Status.RedudantHolesArray;
				}
				for (int i = 0; i < this.holes.Length; i++)
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (math.any(!utils.isfinite(this.holes[i])))
					{
						this.status.Value = Status.HoleMustBeFinite(i);
						return;
					}
				}
			}

			// Token: 0x0400006E RID: 110
			private NativeArray<T2>.ReadOnly positions;

			// Token: 0x0400006F RID: 111
			private NativeReference<Status> status;

			// Token: 0x04000070 RID: 112
			private readonly Args args;

			// Token: 0x04000071 RID: 113
			private NativeArray<int>.ReadOnly constraints;

			// Token: 0x04000072 RID: 114
			private NativeArray<ConstraintType>.ReadOnly constraintTypes;

			// Token: 0x04000073 RID: 115
			private NativeArray<T2>.ReadOnly holes;
		}

		// Token: 0x0200001F RID: 31
		private struct DelaunayTriangulationStep
		{
			// Token: 0x06000098 RID: 152 RVA: 0x000044A0 File Offset: 0x000026A0
			public DelaunayTriangulationStep(OutputData<T2> output, Args args)
			{
				this.status = output.Status;
				this.positions = output.Positions.AsArray().AsReadOnly();
				this.triangles = output.Triangles;
				this.halfedges = output.Halfedges;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.hullStart = int.MaxValue;
				this.verbose = args.Verbose;
				this.hashSize = (int)math.ceil(math.sqrt((float)this.positions.Length));
				this.trianglesLen = 0;
				this.hullNext = default(NativeArray<int>);
				this.hullPrev = default(NativeArray<int>);
				this.hullTri = default(NativeArray<int>);
				this.hullHash = default(NativeArray<int>);
				this.EDGE_STACK = default(NativeArray<int>);
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00004570 File Offset: 0x00002770
			public void Execute(Allocator allocator)
			{
				if (this.status.Value.IsError)
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.DelaunayTriangulationStep.Auto())
				{
					int length = this.positions.Length;
					int num = math.max(2 * length - 5, 0);
					this.triangles.Length = 3 * num;
					this.halfedges.Length = 3 * num;
					NativeArray<int> array = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory);
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T2 t = utils.MaxValue2();
					utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T2 t2 = utils.MinValue2();
					for (int i = 0; i < this.positions.Length; i++)
					{
						T2 w = this.positions[i];
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t = utils.min(t, w);
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t2 = utils.max(t2, w);
						array[i] = i;
					}
					utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T2 a = utils.avg(t, t2);
					int num2 = int.MaxValue;
					int num3 = int.MaxValue;
					int num4 = int.MaxValue;
					utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TBig b = utils.MaxValue();
					for (int j = 0; j < this.positions.Length; j++)
					{
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						TBig tbig = utils.distancesq(a, this.positions[j]);
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (utils.less(tbig, b))
						{
							num2 = j;
							b = tbig;
						}
					}
					T2 t3 = this.positions[num2];
					utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					b = utils.MaxValue();
					for (int k = 0; k < this.positions.Length; k++)
					{
						if (k != num2)
						{
							utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							TBig tbig2 = utils.distancesq(t3, this.positions[k]);
							utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							if (utils.less(tbig2, b))
							{
								num3 = k;
								b = tbig2;
							}
						}
					}
					T2 t4 = this.positions[num3];
					utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TBig b2 = utils.MaxValue();
					for (int l = 0; l < this.positions.Length; l++)
					{
						if (l != num2 && l != num3)
						{
							T2 c = this.positions[l];
							TBig tbig3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CircumRadiusSq(t3, t4, c);
							utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							if (utils.less(tbig3, b2))
							{
								num4 = l;
								b2 = tbig3;
							}
						}
					}
					if (num4 != 2147483647)
					{
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						T2 v = utils2.CircumCenter(t3, t4, this.positions[num4]);
						utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (!math.any(utils.eq(v, utils2.MaxValue2())))
						{
							using (this.hullPrev = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory))
							{
								using (this.hullNext = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory))
								{
									using (this.hullTri = new NativeArray<int>(length, allocator, NativeArrayOptions.ClearMemory))
									{
										using (this.hullHash = new NativeArray<int>(this.hashSize, allocator, NativeArrayOptions.ClearMemory))
										{
											using (this.EDGE_STACK = new NativeArray<int>(math.min(3 * num, 512), allocator, NativeArrayOptions.ClearMemory))
											{
												NativeArray<TBig> dist = new NativeArray<TBig>(length, allocator, NativeArrayOptions.ClearMemory);
												T2 t5 = this.positions[num4];
												utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												TBig a2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t3, t4, t5);
												utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												if (utils.less(a2, utils2.ZeroTBig()))
												{
													int num5 = num4;
													int num6 = num3;
													num3 = num5;
													num4 = num6;
													T2 t6 = t5;
													T2 t7 = t4;
													t4 = t6;
													t5 = t7;
												}
												utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												T2 t8 = utils.CircumCenter(t3, t4, t5);
												for (int m = 0; m < this.positions.Length; m++)
												{
													int index = m;
													utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
													dist[index] = utils.distancesq(t8, this.positions[m]);
												}
												array.Sort(new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.DelaunayTriangulationStep.DistComparer(dist));
												this.hullStart = num2;
												this.hullNext[num2] = (this.hullPrev[num4] = num3);
												this.hullNext[num3] = (this.hullPrev[num2] = num4);
												this.hullNext[num4] = (this.hullPrev[num3] = num2);
												this.hullTri[num2] = 0;
												this.hullTri[num3] = 1;
												this.hullTri[num4] = 2;
												utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												this.hullHash[utils.hashkey(t3, t8, this.hashSize)] = num2;
												utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												this.hullHash[utils.hashkey(t4, t8, this.hashSize)] = num3;
												utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
												this.hullHash[utils.hashkey(t5, t8, this.hashSize)] = num4;
												this.AddTriangle(num2, num3, num4, -1, -1, -1);
												for (int n = 0; n < array.Length; n++)
												{
													int num7 = array[n];
													if (num7 != num2 && num7 != num3 && num7 != num4)
													{
														T2 t9 = this.positions[num7];
														int num8 = 0;
														utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
														int num9 = utils.hashkey(t9, t8, this.hashSize);
														for (int num10 = 0; num10 < this.hashSize; num10++)
														{
															num8 = this.hullHash[(num9 + num10) % this.hashSize];
															if (num8 != -1 && num8 != this.hullNext[num8])
															{
																break;
															}
														}
														num8 = this.hullPrev[num8];
														int num11 = num8;
														int num12 = this.hullNext[num11];
														for (;;)
														{
															utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															TBig a3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t9, this.positions[num11], this.positions[num12]);
															utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															if (utils.less(a3, utils2.ZeroTBig()))
															{
																break;
															}
															num11 = num12;
															if (num11 == num8)
															{
																goto Block_31;
															}
															num12 = this.hullNext[num11];
														}
														IL_6D5:
														if (num11 != 2147483647)
														{
															int num13 = this.AddTriangle(num11, num7, this.hullNext[num11], -1, -1, this.hullTri[num11]);
															this.hullTri[num7] = this.Legalize(num13 + 2);
															this.hullTri[num11] = num13;
															int num14 = this.hullNext[num11];
															num12 = this.hullNext[num14];
															for (;;)
															{
																utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																TBig a4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t9, this.positions[num14], this.positions[num12]);
																utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																if (!utils.less(a4, utils2.ZeroTBig()))
																{
																	break;
																}
																num13 = this.AddTriangle(num14, num7, num12, this.hullTri[num7], -1, this.hullTri[num14]);
																this.hullTri[num7] = this.Legalize(num13 + 2);
																this.hullNext[num14] = num14;
																num14 = num12;
																num12 = this.hullNext[num14];
															}
															if (num11 == num8)
															{
																num12 = this.hullPrev[num11];
																for (;;)
																{
																	utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																	TBig a5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Orient2dFast(t9, this.positions[num12], this.positions[num11]);
																	utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
																	if (!utils.less(a5, utils2.ZeroTBig()))
																	{
																		break;
																	}
																	num13 = this.AddTriangle(num12, num7, num11, -1, this.hullTri[num11], this.hullTri[num12]);
																	this.Legalize(num13 + 2);
																	this.hullTri[num12] = num13;
																	this.hullNext[num11] = num11;
																	num11 = num12;
																	num12 = this.hullPrev[num11];
																}
															}
															this.hullStart = (this.hullPrev[num7] = num11);
															this.hullNext[num11] = (this.hullPrev[num14] = num7);
															this.hullNext[num7] = num14;
															utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															this.hullHash[utils.hashkey(t9, t8, this.hashSize)] = num7;
															utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
															this.hullHash[utils.hashkey(this.positions[num11], t8, this.hashSize)] = num11;
															goto IL_977;
														}
														goto IL_977;
														Block_31:
														num11 = int.MaxValue;
														goto IL_6D5;
													}
													IL_977:;
												}
												this.triangles.Length = this.trianglesLen;
												this.halfedges.Length = this.trianglesLen;
												this.constrainedHalfedges.Length = this.trianglesLen;
												array.Dispose();
												dist.Dispose();
												return;
											}
										}
									}
								}
							}
						}
					}
					this.status.Value = Status.DegenerateInput;
					array.Dispose();
				}
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00005034 File Offset: 0x00003234
			private int Legalize(int a)
			{
				int num = 0;
				int num4;
				for (;;)
				{
					int num2 = this.halfedges[a];
					int num3 = a - a % 3;
					num4 = num3 + (a + 2) % 3;
					if (num2 == -1)
					{
						if (num == 0)
						{
							break;
						}
						a = this.EDGE_STACK[--num];
					}
					else
					{
						int num5 = num2 - num2 % 3;
						int index = num3 + (a + 1) % 3;
						int num6 = num5 + (num2 + 2) % 3;
						int num7 = this.triangles[num4];
						int index2 = this.triangles[a];
						int index3 = this.triangles[index];
						int num8 = this.triangles[num6];
						TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						if (utils.InCircle(this.positions[num7], this.positions[index2], this.positions[index3], this.positions[num8]))
						{
							this.triangles[a] = num8;
							this.triangles[num2] = num7;
							int num9 = this.halfedges[num6];
							if (num9 == -1)
							{
								int num10 = this.hullStart;
								while (this.hullTri[num10] != num6)
								{
									num10 = this.hullPrev[num10];
									if (num10 == this.hullStart)
									{
										goto IL_14E;
									}
								}
								this.hullTri[num10] = a;
							}
							IL_14E:
							this.Link(a, num9);
							this.Link(num2, this.halfedges[num4]);
							this.Link(num4, num6);
							int value = num5 + (num2 + 1) % 3;
							if (num < this.EDGE_STACK.Length)
							{
								this.EDGE_STACK[num++] = value;
							}
						}
						else
						{
							if (num == 0)
							{
								break;
							}
							a = this.EDGE_STACK[--num];
						}
					}
				}
				return num4;
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00005204 File Offset: 0x00003404
			private int AddTriangle(int i0, int i1, int i2, int a, int b, int c)
			{
				int num = this.trianglesLen;
				this.triangles[num] = i0;
				this.triangles[num + 1] = i1;
				this.triangles[num + 2] = i2;
				this.Link(num, a);
				this.Link(num + 1, b);
				this.Link(num + 2, c);
				this.trianglesLen += 3;
				return num;
			}

			// Token: 0x0600009C RID: 156 RVA: 0x00005271 File Offset: 0x00003471
			private void Link(int a, int b)
			{
				this.halfedges[a] = b;
				if (b != -1)
				{
					this.halfedges[b] = a;
				}
			}

			// Token: 0x04000074 RID: 116
			private NativeReference<Status> status;

			// Token: 0x04000075 RID: 117
			private NativeArray<T2>.ReadOnly positions;

			// Token: 0x04000076 RID: 118
			private NativeList<int> triangles;

			// Token: 0x04000077 RID: 119
			private NativeList<int> halfedges;

			// Token: 0x04000078 RID: 120
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x04000079 RID: 121
			private NativeArray<int> hullNext;

			// Token: 0x0400007A RID: 122
			private NativeArray<int> hullPrev;

			// Token: 0x0400007B RID: 123
			private NativeArray<int> hullTri;

			// Token: 0x0400007C RID: 124
			private NativeArray<int> hullHash;

			// Token: 0x0400007D RID: 125
			private NativeArray<int> EDGE_STACK;

			// Token: 0x0400007E RID: 126
			private readonly int hashSize;

			// Token: 0x0400007F RID: 127
			private readonly bool verbose;

			// Token: 0x04000080 RID: 128
			private int hullStart;

			// Token: 0x04000081 RID: 129
			private int trianglesLen;

			// Token: 0x02000020 RID: 32
			private struct DistComparer : IComparer<int>
			{
				// Token: 0x0600009D RID: 157 RVA: 0x00005291 File Offset: 0x00003491
				public DistComparer(NativeArray<TBig> dist)
				{
					this.dist = dist;
				}

				// Token: 0x0600009E RID: 158 RVA: 0x0000529C File Offset: 0x0000349C
				public int Compare(int x, int y)
				{
					TBig tbig = this.dist[x];
					return tbig.CompareTo(this.dist[y]);
				}

				// Token: 0x04000082 RID: 130
				private NativeArray<TBig> dist;
			}
		}

		// Token: 0x02000021 RID: 33
		private struct ConstrainEdgesStep
		{
			// Token: 0x0600009F RID: 159 RVA: 0x000052D0 File Offset: 0x000034D0
			public ConstrainEdgesStep(InputData<T2> input, OutputData<T2> output, Args args)
			{
				this.status = output.Status;
				this.positions = output.Positions.AsArray().AsReadOnly();
				this.triangles = output.Triangles.AsArray();
				this.inputConstraintEdges = input.ConstraintEdges.AsReadOnly();
				this.inputConstraintEdgeTypes = input.ConstraintEdgeTypes.AsReadOnly();
				this.halfedges = output.Halfedges;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.args = args;
				this.intersections = default(NativeList<int>);
				this.unresolvedIntersections = default(NativeList<int>);
				this.pointToHalfedge = default(NativeArray<int>);
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x0000537C File Offset: 0x0000357C
			public void Execute(Allocator allocator)
			{
				if (!this.inputConstraintEdges.IsCreated || this.status.Value.IsError)
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.ConstrainEdgesStep.Auto())
				{
					using (this.intersections = new NativeList<int>(allocator))
					{
						using (this.unresolvedIntersections = new NativeList<int>(allocator))
						{
							using (this.pointToHalfedge = new NativeArray<int>(this.positions.Length, allocator, NativeArrayOptions.ClearMemory))
							{
								for (int i = 0; i < this.triangles.Length; i++)
								{
									this.pointToHalfedge[this.triangles[i]] = i;
								}
								for (int j = 0; j < this.inputConstraintEdges.Length / 2; j++)
								{
									int2 @int = math.int2(this.inputConstraintEdges[2 * j], this.inputConstraintEdges[2 * j + 1]);
									@int = ((@int.x < @int.y) ? @int.xy : @int.yx);
									ConstraintType constraintType = this.inputConstraintEdgeTypes.IsCreated ? this.inputConstraintEdgeTypes[j] : ConstraintType.ConstrainedAndHoleBoundary;
									this.TryApplyConstraint(@int, (constraintType == ConstraintType.Constrained) ? HalfedgeState.Constrained : HalfedgeState.ConstrainedAndHoleBoundary);
								}
							}
						}
					}
				}
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x00005574 File Offset: 0x00003774
			private void TryResolveIntersections(int2 c, HalfedgeState constrainValue, ref int iter)
			{
				for (int i = 0; i < this.intersections.Length; i++)
				{
					int num = iter;
					iter = num + 1;
					if (this.IsMaxItersExceeded(num, this.args.SloanMaxIters))
					{
						return;
					}
					int num2 = this.intersections[i];
					int num3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num2);
					int num4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num3);
					int num5 = this.halfedges[num2];
					int num6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
					int num7 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
					int index = this.triangles[num2];
					int index2 = this.triangles[num3];
					int num8 = this.triangles[num4];
					int num9 = this.triangles[num7];
					T2 t = this.positions[index];
					T2 t2 = this.positions[num9];
					T2 t3 = this.positions[index2];
					T2 t4 = this.positions[num8];
					T2 a = t;
					T2 b = t2;
					T2 c2 = t3;
					T2 d = t4;
					if (!UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.IsConvexQuadrilateral(a, b, c2, d))
					{
						this.unresolvedIntersections.Add(num2);
					}
					else
					{
						this.triangles[num2] = num9;
						this.triangles[num5] = num8;
						this.pointToHalfedge[num9] = num2;
						this.pointToHalfedge[num8] = num5;
						this.pointToHalfedge[index] = num6;
						this.pointToHalfedge[index2] = num3;
						this.ReplaceHalfedge(num7, num2);
						this.ReplaceHalfedge(num4, num5);
						this.halfedges[num4] = num7;
						this.halfedges[num7] = num4;
						this.constrainedHalfedges[num4] = HalfedgeState.Unconstrained;
						this.constrainedHalfedges[num7] = HalfedgeState.Unconstrained;
						for (int j = i + 1; j < this.intersections.Length; j++)
						{
							int num10 = this.intersections[j];
							this.intersections[j] = ((num10 == num4) ? num5 : ((num10 == num7) ? num2 : num10));
						}
						for (int k = 0; k < this.unresolvedIntersections.Length; k++)
						{
							int num11 = this.unresolvedIntersections[k];
							this.unresolvedIntersections[k] = ((num11 == num4) ? num5 : ((num11 == num7) ? num2 : num11));
						}
						int2 e = math.int2(num8, num9);
						if (math.all(c.xy == e.xy) || math.all(c.xy == e.yx))
						{
							this.constrainedHalfedges[num4] = constrainValue;
							this.constrainedHalfedges[num7] = constrainValue;
						}
						if (this.EdgeEdgeIntersection(c, e))
						{
							this.unresolvedIntersections.Add(num4);
						}
					}
				}
				this.intersections.Clear();
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x00005850 File Offset: 0x00003A50
			private void ReplaceHalfedge(int h0, int h1)
			{
				int num = this.halfedges[h0];
				this.halfedges[h1] = num;
				this.constrainedHalfedges[h1] = this.constrainedHalfedges[h0];
				if (num != -1)
				{
					this.halfedges[num] = h1;
					this.constrainedHalfedges[num] = this.constrainedHalfedges[h0];
				}
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x000058B8 File Offset: 0x00003AB8
			private bool EdgeEdgeIntersection(int2 e1, int2 e2)
			{
				T2 t = this.positions[e1.x];
				T2 t2 = this.positions[e1.y];
				T2 a = t;
				T2 a2 = t2;
				t = this.positions[e2.x];
				T2 t3 = this.positions[e2.y];
				T2 b = t;
				T2 b2 = t3;
				return !math.any(e1.xy == e2.xy | e1.xy == e2.yx) && UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.EdgeEdgeIntersection(a, a2, b, b2);
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x00005954 File Offset: 0x00003B54
			private void MarkHalfedgeConstrained(int halfedge, HalfedgeState constrainValue)
			{
				constrainValue = (HalfedgeState)math.max((int)this.constrainedHalfedges[halfedge], (int)constrainValue);
				this.constrainedHalfedges[halfedge] = constrainValue;
				int num = this.halfedges[halfedge];
				if (num != -1)
				{
					this.constrainedHalfedges[num] = constrainValue;
				}
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x000059A4 File Offset: 0x00003BA4
			private void TryApplyConstraint(int2 edge, HalfedgeState constrainValue)
			{
				this.intersections.Clear();
				this.unresolvedIntersections.Clear();
				int num = -1;
				int x = edge.x;
				int y = edge.y;
				int num2 = x;
				int num3 = y;
				int num4 = this.pointToHalfedge[num2];
				int num5 = num4;
				int num6;
				int num7;
				for (;;)
				{
					num6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
					if (this.triangles[num6] == num3)
					{
						break;
					}
					num7 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
					if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num6]], this.positions[num2], this.positions[num3]))
					{
						goto Block_2;
					}
					if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num7]], this.positions[num2], this.positions[num3]) && this.triangles[num7] != num3)
					{
						goto Block_4;
					}
					if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num6], this.triangles[num7])))
					{
						goto Block_5;
					}
					num5 = this.halfedges[num7];
					if (num5 == -1)
					{
						goto Block_6;
					}
					if (num5 == num4)
					{
						goto IL_1A2;
					}
				}
				this.MarkHalfedgeConstrained(num5, constrainValue);
				goto IL_1A2;
				Block_2:
				this.MarkHalfedgeConstrained(num5, constrainValue);
				num3 = this.triangles[num6];
				goto IL_1A2;
				Block_4:
				this.MarkHalfedgeConstrained(num7, constrainValue);
				num3 = this.triangles[num7];
				goto IL_1A2;
				Block_5:
				this.unresolvedIntersections.Add(num6);
				num = this.halfedges[num6];
				goto IL_1A2;
				Block_6:
				if (this.triangles[num7] == num3)
				{
					this.MarkHalfedgeConstrained(num7, constrainValue);
				}
				IL_1A2:
				num5 = this.halfedges[num4];
				if (num == -1 && num5 != -1)
				{
					num5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
					int num8;
					int num9;
					for (;;)
					{
						num8 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
						if (this.triangles[num8] == num3)
						{
							break;
						}
						num9 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num8);
						if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num8]], this.positions[num2], this.positions[num3]))
						{
							goto Block_11;
						}
						if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[num9]], this.positions[num2], this.positions[num3]) && this.triangles[num9] != num3)
						{
							goto Block_13;
						}
						if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num8], this.triangles[num9])))
						{
							goto Block_14;
						}
						num5 = this.halfedges[num5];
						if (num5 == -1)
						{
							goto IL_431;
						}
						num5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num5);
						if (num5 == num4)
						{
							goto Block_16;
						}
					}
					this.MarkHalfedgeConstrained(num5, constrainValue);
					goto IL_431;
					Block_11:
					this.MarkHalfedgeConstrained(num5, constrainValue);
					num3 = this.triangles[num8];
					goto IL_431;
					Block_13:
					this.MarkHalfedgeConstrained(num9, constrainValue);
					num3 = this.triangles[num9];
					goto IL_431;
					Block_14:
					this.unresolvedIntersections.Add(num8);
					num = this.halfedges[num8];
					Block_16:;
				}
				IL_431:
				while (num != -1)
				{
					int num10 = num;
					num = -1;
					int num11 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num10);
					int index = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num11);
					if (this.triangles[index] == num3)
					{
						break;
					}
					if (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PointLineSegmentIntersection(this.positions[this.triangles[index]], this.positions[num2], this.positions[num3]))
					{
						num3 = this.triangles[index];
						break;
					}
					if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[num11], this.triangles[index])))
					{
						this.unresolvedIntersections.Add(num11);
						num = this.halfedges[num11];
					}
					else if (this.EdgeEdgeIntersection(new int2(num2, num3), new int2(this.triangles[index], this.triangles[num10])))
					{
						this.unresolvedIntersections.Add(index);
						num = this.halfedges[index];
					}
				}
				int num12 = 0;
				while (!this.status.Value.IsError)
				{
					NativeList<int> nativeList = this.unresolvedIntersections;
					NativeList<int> nativeList2 = this.intersections;
					this.intersections = nativeList;
					this.unresolvedIntersections = nativeList2;
					this.TryResolveIntersections(new int2(num2, num3), constrainValue, ref num12);
					if (this.unresolvedIntersections.IsEmpty)
					{
						if (edge.y != num3)
						{
							this.TryApplyConstraint(new int2(num3, edge.y), constrainValue);
						}
						return;
					}
				}
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00005E5C File Offset: 0x0000405C
			private bool IsMaxItersExceeded(int iter, int maxIters)
			{
				if (iter >= maxIters)
				{
					this.status.Value = Status.SloanMaxItersExceeded;
					return true;
				}
				return false;
			}

			// Token: 0x04000083 RID: 131
			private NativeReference<Status> status;

			// Token: 0x04000084 RID: 132
			private NativeArray<T2>.ReadOnly positions;

			// Token: 0x04000085 RID: 133
			private NativeArray<int> triangles;

			// Token: 0x04000086 RID: 134
			private NativeArray<int>.ReadOnly inputConstraintEdges;

			// Token: 0x04000087 RID: 135
			private NativeArray<ConstraintType>.ReadOnly inputConstraintEdgeTypes;

			// Token: 0x04000088 RID: 136
			private NativeList<int> halfedges;

			// Token: 0x04000089 RID: 137
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x0400008A RID: 138
			private readonly Args args;

			// Token: 0x0400008B RID: 139
			private NativeList<int> intersections;

			// Token: 0x0400008C RID: 140
			private NativeList<int> unresolvedIntersections;

			// Token: 0x0400008D RID: 141
			private NativeArray<int> pointToHalfedge;
		}

		// Token: 0x02000022 RID: 34
		private struct PlantingSeedStep
		{
			// Token: 0x060000A7 RID: 167 RVA: 0x00005E75 File Offset: 0x00004075
			public PlantingSeedStep(InputData<T2> input, OutputData<T2> output, Args args)
			{
				this = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep(output, args, input.HoleSeeds);
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00005E88 File Offset: 0x00004088
			public PlantingSeedStep(OutputData<T2> output, Args args, NativeArray<T2> localHoles)
			{
				this.status = output.Status;
				this.triangles = output.Triangles;
				this.positions = output.Positions;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.halfedges = output.Halfedges;
				this.holes = localHoles;
				this.args = args;
				this.shouldRemoveTriangle = default(NativeArray<bool>);
				this.trianglesQueue = default(NativeQueue<int>);
				this.anyRemovedTriangles = false;
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00005F00 File Offset: 0x00004100
			public void Execute(Allocator allocator, bool constraintsIsCreated)
			{
				if (!constraintsIsCreated || (this.status.IsCreated && this.status.Value.IsError))
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.PlantingSeedStep.Auto())
				{
					using (this.shouldRemoveTriangle = new NativeArray<bool>(this.triangles.Length / 3, allocator, NativeArrayOptions.ClearMemory))
					{
						if (this.args.AutoHolesAndBoundary)
						{
							this.PlantAuto(allocator);
						}
						if (this.holes.IsCreated || this.args.RestoreBoundary)
						{
							this.trianglesQueue = new NativeQueue<int>(allocator);
							if (this.holes.IsCreated)
							{
								this.PlantHoleSeeds(this.holes);
							}
							if (this.args.RestoreBoundary)
							{
								this.PlantBoundarySeeds();
							}
							this.trianglesQueue.Dispose();
						}
						this.RemoveVisitedTriangles(allocator);
					}
				}
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00006018 File Offset: 0x00004218
			private void PlantBoundarySeeds()
			{
				for (int i = 0; i < this.halfedges.Length; i++)
				{
					if (this.halfedges[i] == -1 && !this.shouldRemoveTriangle[i / 3] && this.constrainedHalfedges[i] != HalfedgeState.ConstrainedAndHoleBoundary)
					{
						this.PlantSeed(i / 3);
					}
				}
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00006074 File Offset: 0x00004274
			private void PlantHoleSeeds(NativeArray<T2> holeSeeds)
			{
				foreach (T2 p in holeSeeds)
				{
					int num = this.FindTriangle(p);
					if (num != -1)
					{
						this.PlantSeed(num);
					}
				}
			}

			// Token: 0x060000AC RID: 172 RVA: 0x000060D0 File Offset: 0x000042D0
			private void RemoveVisitedTriangles(Allocator allocator)
			{
				if (!this.anyRemovedTriangles)
				{
					return;
				}
				UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<>c__DisplayClass15_0 CS$<>8__locals1;
				CS$<>8__locals1.indexRemap = new NativeArray<int>(this.triangles.Length / 3, allocator, NativeArrayOptions.ClearMemory);
				int num = 0;
				for (int i = 0; i < this.shouldRemoveTriangle.Length; i++)
				{
					int index = i;
					int value;
					if (!this.shouldRemoveTriangle[i])
					{
						num = (value = num) + 1;
					}
					else
					{
						value = -1;
					}
					CS$<>8__locals1.indexRemap[index] = value;
				}
				NativeArray<bool3> nativeArray = this.constrainedHalfedges.AsArray().Reinterpret<bool3>(1);
				NativeArray<int3> nativeArray2 = this.triangles.AsArray().Reinterpret<int3>(4);
				for (int j = 0; j < CS$<>8__locals1.indexRemap.Length; j++)
				{
					int num2 = CS$<>8__locals1.indexRemap[j];
					if (num2 != -1)
					{
						nativeArray2[num2] = nativeArray2[j];
						nativeArray[num2] = nativeArray[j];
						this.halfedges[3 * num2] = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<RemoveVisitedTriangles>g__RemapHalfedge|15_0(this.halfedges[3 * j], ref CS$<>8__locals1);
						this.halfedges[3 * num2 + 1] = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<RemoveVisitedTriangles>g__RemapHalfedge|15_0(this.halfedges[3 * j + 1], ref CS$<>8__locals1);
						this.halfedges[3 * num2 + 2] = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<RemoveVisitedTriangles>g__RemapHalfedge|15_0(this.halfedges[3 * j + 2], ref CS$<>8__locals1);
					}
				}
				this.triangles.Length = 3 * num;
				this.constrainedHalfedges.Length = 3 * num;
				this.halfedges.Length = 3 * num;
				CS$<>8__locals1.indexRemap.Dispose();
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00006278 File Offset: 0x00004478
			private void PlantSeed(int tId)
			{
				NativeArray<bool> nativeArray = this.shouldRemoveTriangle;
				NativeQueue<int> nativeQueue = this.trianglesQueue;
				if (nativeArray[tId])
				{
					return;
				}
				nativeArray[tId] = true;
				nativeQueue.Enqueue(tId);
				this.anyRemovedTriangles = true;
				while (nativeQueue.TryDequeue(out tId))
				{
					for (int i = 0; i < 3; i++)
					{
						int index = 3 * tId + i;
						int num = this.halfedges[index];
						if (this.constrainedHalfedges[index] != HalfedgeState.ConstrainedAndHoleBoundary && num != -1)
						{
							int num2 = num / 3;
							if (!nativeArray[num2])
							{
								nativeArray[num2] = true;
								nativeQueue.Enqueue(num2);
							}
						}
					}
				}
			}

			// Token: 0x060000AE RID: 174 RVA: 0x0000631C File Offset: 0x0000451C
			private int FindTriangle(T2 p)
			{
				for (int i = 0; i < this.triangles.Length / 3; i++)
				{
					int num = this.triangles[3 * i];
					int num2 = this.triangles[3 * i + 1];
					int num3 = this.triangles[3 * i + 2];
					int index = num;
					int index2 = num2;
					int index3 = num3;
					T2 t = this.positions[index];
					T2 t2 = this.positions[index2];
					T2 t3 = this.positions[index3];
					T2 a = t;
					T2 b = t2;
					T2 c = t3;
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (utils.PointInsideTriangle(p, a, b, c))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060000AF RID: 175 RVA: 0x000063D4 File Offset: 0x000045D4
			private void PlantAuto(Allocator allocator)
			{
				int num = this.triangles.Length / 3;
				NativeQueue<int> nativeQueue = new NativeQueue<int>(allocator);
				NativeQueue<int> nativeQueue2 = new NativeQueue<int>(allocator);
				NativeArray<bool> nativeArray = new NativeArray<bool>(num, allocator, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						if (this.halfedges[3 * i + j] == -1)
						{
							((this.constrainedHalfedges[3 * i + j] == HalfedgeState.ConstrainedAndHoleBoundary) ? nativeQueue2 : nativeQueue).Enqueue(i);
							nativeArray[i] = true;
							break;
						}
					}
				}
				bool flag = false;
				bool flag2 = true;
				while (!nativeQueue.IsEmpty() || !nativeQueue2.IsEmpty())
				{
					int num2;
					while (nativeQueue.TryDequeue(out num2))
					{
						if (flag2)
						{
							this.shouldRemoveTriangle[num2] = true;
							flag = true;
						}
						for (int k = 0; k < 3; k++)
						{
							int index = 3 * num2 + k;
							int num3 = this.halfedges[index];
							int num4 = num3 / 3;
							if (num3 != -1 && !nativeArray[num4])
							{
								((this.constrainedHalfedges[index] == HalfedgeState.ConstrainedAndHoleBoundary) ? nativeQueue2 : nativeQueue).Enqueue(num4);
								nativeArray[num4] = true;
							}
						}
					}
					NativeQueue<int> nativeQueue3 = nativeQueue2;
					NativeQueue<int> nativeQueue4 = nativeQueue;
					nativeQueue = nativeQueue3;
					nativeQueue2 = nativeQueue4;
					flag2 = !flag2;
				}
				this.anyRemovedTriangles = flag;
				nativeQueue.Dispose();
				nativeQueue2.Dispose();
				nativeArray.Dispose();
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00006550 File Offset: 0x00004750
			[CompilerGenerated]
			internal static int <RemoveVisitedTriangles>g__RemapHalfedge|15_0(int he, ref UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.PlantingSeedStep.<>c__DisplayClass15_0 A_1)
			{
				if (he == -1)
				{
					return -1;
				}
				int num = A_1.indexRemap[he / 3];
				if (num != -1)
				{
					return 3 * num + he % 3;
				}
				return -1;
			}

			// Token: 0x0400008E RID: 142
			private NativeReference<Status> status;

			// Token: 0x0400008F RID: 143
			private NativeList<int> triangles;

			// Token: 0x04000090 RID: 144
			[ReadOnly]
			private NativeList<T2> positions;

			// Token: 0x04000091 RID: 145
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x04000092 RID: 146
			private NativeList<int> halfedges;

			// Token: 0x04000093 RID: 147
			private NativeArray<bool> shouldRemoveTriangle;

			// Token: 0x04000094 RID: 148
			private NativeQueue<int> trianglesQueue;

			// Token: 0x04000095 RID: 149
			private NativeArray<T2> holes;

			// Token: 0x04000096 RID: 150
			private bool anyRemovedTriangles;

			// Token: 0x04000097 RID: 151
			private readonly Args args;
		}

		// Token: 0x02000024 RID: 36
		private struct RefineMeshStep
		{
			// Token: 0x060000B1 RID: 177 RVA: 0x00006580 File Offset: 0x00004780
			public RefineMeshStep(OutputData<T2> output, Args args, TTransform lt)
			{
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils4 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T a = utils5.Const(2f);
				utils5 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T area2Threshold = utils.Cast(utils2.mul(utils3.Cast(utils4.mul(a, utils5.Const(args.RefinementThresholdArea))), lt.AreaScalingFactor));
				utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				this = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep(output, area2Threshold, utils.Const(args.RefinementThresholdAngle));
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x0000663C File Offset: 0x0000483C
			public RefineMeshStep(OutputData<T2> output, T area2Threshold, T angleThreshold)
			{
				this.status = output.Status;
				this.initialPointsCount = output.Positions.Length;
				this.maximumArea2 = area2Threshold;
				this.angleThreshold = angleThreshold;
				this.triangles = output.Triangles;
				this.outputPositions = output.Positions;
				this.halfedges = output.Halfedges;
				this.constrainedHalfedges = output.ConstrainedHalfedges;
				this.circles = default(NativeList<UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle>);
				this.trianglesQueue = default(NativeQueue<int>);
				this.badTriangles = default(NativeList<int>);
				this.pathPoints = default(NativeList<int>);
				this.pathHalfedges = default(NativeList<int>);
				this.visitedTriangles = default(NativeList<bool>);
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x000066F0 File Offset: 0x000048F0
			public void Execute(Allocator allocator, bool refineMesh, bool constrainBoundary)
			{
				if (!refineMesh || (this.status.IsCreated && this.status.Value.IsError))
				{
					return;
				}
				using (UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Markers.RefineMeshStep.Auto())
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (!utils.SupportsRefinement())
					{
						this.status.Value = Status.IntegersDoNotSupportMeshRefinement;
					}
					else
					{
						if (constrainBoundary)
						{
							for (int i = 0; i < this.constrainedHalfedges.Length; i++)
							{
								this.constrainedHalfedges[i] = ((this.halfedges[i] == -1) ? HalfedgeState.ConstrainedAndHoleBoundary : HalfedgeState.Unconstrained);
							}
						}
						using (this.circles = new NativeList<UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle>(allocator)
						{
							Length = this.triangles.Length / 3
						})
						{
							using (this.trianglesQueue = new NativeQueue<int>(allocator))
							{
								using (this.badTriangles = new NativeList<int>(this.triangles.Length / 3, allocator))
								{
									using (this.pathPoints = new NativeList<int>(allocator))
									{
										using (this.pathHalfedges = new NativeList<int>(allocator))
										{
											using (this.visitedTriangles = new NativeList<bool>(this.triangles.Length / 3, allocator))
											{
												using (NativeList<int> heQueue = new NativeList<int>(this.triangles.Length, allocator))
												{
													using (NativeList<int> tQueue = new NativeList<int>(this.triangles.Length, allocator))
													{
														for (int j = 0; j < this.triangles.Length / 3; j++)
														{
															int num = this.triangles[3 * j];
															int num2 = this.triangles[3 * j + 1];
															int num3 = this.triangles[3 * j + 2];
															int i2 = num;
															int j2 = num2;
															int k = num3;
															this.circles[j] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(i2, j2, k, this.outputPositions.AsArray()));
														}
														for (int l = 0; l < this.constrainedHalfedges.Length; l++)
														{
															if (this.constrainedHalfedges[l] >= HalfedgeState.Constrained && this.IsEncroached(l))
															{
																heQueue.Add(l);
															}
														}
														this.SplitEncroachedEdges(heQueue, default(NativeList<int>));
														for (int m = 0; m < this.triangles.Length / 3; m++)
														{
															if (this.IsBadTriangle(m))
															{
																tQueue.Add(m);
															}
														}
														for (int n = 0; n < tQueue.Length; n++)
														{
															int num4 = tQueue[n];
															if (num4 != -1)
															{
																this.SplitTriangle(num4, heQueue, tQueue, allocator);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x00006B1C File Offset: 0x00004D1C
			private void SplitEncroachedEdges(NativeList<int> heQueue, NativeList<int> tQueue)
			{
				for (int i = 0; i < heQueue.Length; i++)
				{
					int num = heQueue[i];
					if (num != -1)
					{
						this.SplitEdge(num, heQueue, tQueue);
					}
				}
				heQueue.Clear();
			}

			// Token: 0x060000B5 RID: 181 RVA: 0x00006B58 File Offset: 0x00004D58
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool IsEncroached(int he0)
			{
				int num = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(he0);
				int index = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num);
				T2 a = this.outputPositions[this.triangles[he0]];
				T2 a2 = this.outputPositions[this.triangles[num]];
				T2 b = this.outputPositions[this.triangles[index]];
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T2 a3 = utils3.diff(a, b);
				utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				T a4 = utils2.dot(a3, utils3.diff(a2, b));
				utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				return utils.le(a4, utils2.Zero());
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00006C28 File Offset: 0x00004E28
			private void SplitEdge(int he, NativeList<int> heQueue, NativeList<int> tQueue)
			{
				int num = this.triangles[he];
				int num2 = this.triangles[UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(he)];
				int num3 = num;
				int num4 = num2;
				T2 t = this.outputPositions[num3];
				T2 t2 = this.outputPositions[num4];
				T2 t3 = t;
				T2 t4 = t2;
				T2 p;
				if ((num3 < this.initialPointsCount && num4 < this.initialPointsCount) || (num3 >= this.initialPointsCount && num4 >= this.initialPointsCount))
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					p = utils.avg(t3, t4);
				}
				else
				{
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T concentricShellReferenceRadius = utils2.Const(0.001f);
					utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					T v = utils.alpha(concentricShellReferenceRadius, utils2.Cast(utils3.distancesq(t3, t4)));
					T2 t5;
					if (num3 >= this.initialPointsCount)
					{
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t5 = utils.lerp(t4, t3, v);
					}
					else
					{
						utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
						t5 = utils.lerp(t3, t4, v);
					}
					p = t5;
				}
				this.constrainedHalfedges[he] = HalfedgeState.Unconstrained;
				int num5 = this.halfedges[he];
				if (num5 != -1)
				{
					this.constrainedHalfedges[num5] = HalfedgeState.Unconstrained;
				}
				if (this.halfedges[he] != -1)
				{
					this.UnsafeInsertPointBulk(p, he / 3, heQueue, tQueue);
					int num6 = this.triangles.Length - 3;
					int num7 = -1;
					int num8 = -1;
					while (num7 == -1 || num8 == -1)
					{
						int num9 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
						if (this.triangles[num9] == num3)
						{
							num7 = num6;
						}
						if (this.triangles[num9] == num4)
						{
							num8 = num6;
						}
						int index = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num9);
						num6 = this.halfedges[index];
					}
					if (this.IsEncroached(num7))
					{
						heQueue.Add(num7);
					}
					int num10 = this.halfedges[num7];
					if (this.IsEncroached(num10))
					{
						heQueue.Add(num10);
					}
					if (this.IsEncroached(num8))
					{
						heQueue.Add(num8);
					}
					int num11 = this.halfedges[num8];
					if (this.IsEncroached(num11))
					{
						heQueue.Add(num11);
					}
					this.constrainedHalfedges[num7] = HalfedgeState.Constrained;
					this.constrainedHalfedges[num10] = HalfedgeState.Constrained;
					this.constrainedHalfedges[num8] = HalfedgeState.Constrained;
					this.constrainedHalfedges[num11] = HalfedgeState.Constrained;
					return;
				}
				this.UnsafeInsertPointBoundary(p, he, heQueue, tQueue);
				int num12 = 3 * (this.pathPoints.Length - 1);
				int num13 = this.halfedges.Length - 1;
				int num14 = this.halfedges.Length - num12;
				if (this.IsEncroached(num13))
				{
					heQueue.Add(num13);
				}
				if (this.IsEncroached(num14))
				{
					heQueue.Add(num14);
				}
				this.constrainedHalfedges[num13] = HalfedgeState.Constrained;
				this.constrainedHalfedges[num14] = HalfedgeState.Constrained;
			}

			// Token: 0x060000B7 RID: 183 RVA: 0x00006F24 File Offset: 0x00005124
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool IsBadTriangle(int tId)
			{
				int num = this.triangles[3 * tId];
				int num2 = this.triangles[3 * tId + 1];
				int num3 = this.triangles[3 * tId + 2];
				int index = num;
				int index2 = num2;
				int index3 = num3;
				T2 t = this.outputPositions[index];
				T2 t2 = this.outputPositions[index2];
				T2 t3 = this.outputPositions[index3];
				T2 a = t;
				T2 b = t2;
				T2 c = t3;
				T a2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Area2(a, b, c);
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				return utils.greater(a2, this.maximumArea2) || this.AngleIsTooSmall(tId, this.angleThreshold);
			}

			// Token: 0x060000B8 RID: 184 RVA: 0x00006FD4 File Offset: 0x000051D4
			private void SplitTriangle(int tId, NativeList<int> heQueue, NativeList<int> tQueue, Allocator allocator)
			{
				UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle circle = this.circles[tId];
				NativeList<int> nativeList = new NativeList<int>(allocator);
				for (int i = 0; i < this.constrainedHalfedges.Length; i++)
				{
					if (this.constrainedHalfedges[i] != HalfedgeState.Unconstrained)
					{
						int num = this.triangles[i];
						int num2 = this.triangles[UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(i)];
						int num3 = num;
						int num4 = num2;
						if (this.halfedges[i] == -1 || num3 < num4)
						{
							T2 t = this.outputPositions[num3];
							T2 t2 = this.outputPositions[num4];
							T2 a = t;
							T2 a2 = t2;
							TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							T2 a3 = utils3.diff(a, circle.Center);
							utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							T a4 = utils2.dot(a3, utils3.diff(a2, circle.Center));
							utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
							if (utils.le(a4, utils2.Zero()))
							{
								nativeList.Add(i);
							}
						}
					}
				}
				if (nativeList.IsEmpty)
				{
					this.UnsafeInsertPointBulk(circle.Center, tId, heQueue, tQueue);
				}
				else
				{
					int num = this.triangles[3 * tId];
					int num5 = this.triangles[3 * tId + 1];
					int num6 = this.triangles[3 * tId + 2];
					int index = num;
					int index2 = num5;
					int index3 = num6;
					T2 t = this.outputPositions[index];
					T2 t3 = this.outputPositions[index2];
					T2 t4 = this.outputPositions[index3];
					T2 a5 = t;
					T2 b = t3;
					T2 c = t4;
					T a6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.Area2(a5, b, c);
					TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
					if (utils.greater(a6, this.maximumArea2))
					{
						foreach (int num7 in nativeList.AsArray().AsReadOnly())
						{
							heQueue.Add(num7);
						}
					}
					if (!heQueue.IsEmpty)
					{
						tQueue.Add(tId);
						this.SplitEncroachedEdges(heQueue, tQueue);
					}
				}
				nativeList.Dispose();
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00007238 File Offset: 0x00005438
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool AngleIsTooSmall(int tId, T minimumAngle)
			{
				int num = this.triangles[3 * tId];
				int num2 = this.triangles[3 * tId + 1];
				int num3 = this.triangles[3 * tId + 2];
				int index = num;
				int index2 = num2;
				int index3 = num3;
				T2 t = this.outputPositions[index];
				T2 t2 = this.outputPositions[index2];
				T2 t3 = this.outputPositions[index3];
				T2 pA = t;
				T2 pB = t2;
				T2 pC = t3;
				return UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.AngleIsTooSmall(pA, pB, pC, minimumAngle);
			}

			// Token: 0x060000BA RID: 186 RVA: 0x000072BC File Offset: 0x000054BC
			private int UnsafeInsertPointCommon(T2 p, int initTriangle)
			{
				int length = this.outputPositions.Length;
				this.outputPositions.Add(p);
				this.badTriangles.Clear();
				this.trianglesQueue.Clear();
				this.pathPoints.Clear();
				this.pathHalfedges.Clear();
				this.visitedTriangles.Clear();
				this.visitedTriangles.Length = this.triangles.Length / 3;
				this.trianglesQueue.Enqueue(initTriangle);
				this.badTriangles.Add(initTriangle);
				this.visitedTriangles[initTriangle] = true;
				this.RecalculateBadTriangles(p);
				return length;
			}

			// Token: 0x060000BB RID: 187 RVA: 0x00007360 File Offset: 0x00005560
			private void UnsafeInsertPointBulk(T2 p, int initTriangle, NativeList<int> heQueue = default(NativeList<int>), NativeList<int> tQueue = default(NativeList<int>))
			{
				int pId = this.UnsafeInsertPointCommon(p, initTriangle);
				this.BuildStarPolygon();
				this.ProcessBadTriangles(heQueue, tQueue);
				this.BuildNewTrianglesForStar(pId, heQueue, tQueue);
			}

			// Token: 0x060000BC RID: 188 RVA: 0x00007390 File Offset: 0x00005590
			private void UnsafeInsertPointBoundary(T2 p, int initHe, NativeList<int> heQueue = default(NativeList<int>), NativeList<int> tQueue = default(NativeList<int>))
			{
				int pId = this.UnsafeInsertPointCommon(p, initHe / 3);
				this.BuildAmphitheaterPolygon(initHe);
				this.ProcessBadTriangles(heQueue, tQueue);
				this.BuildNewTrianglesForAmphitheater(pId, heQueue, tQueue);
			}

			// Token: 0x060000BD RID: 189 RVA: 0x000073C4 File Offset: 0x000055C4
			private void RecalculateBadTriangles(T2 p)
			{
				int num;
				while (this.trianglesQueue.TryDequeue(out num))
				{
					this.VisitEdge(p, 3 * num);
					this.VisitEdge(p, 3 * num + 1);
					this.VisitEdge(p, 3 * num + 2);
				}
			}

			// Token: 0x060000BE RID: 190 RVA: 0x00007404 File Offset: 0x00005604
			private void VisitEdge(T2 p, int t0)
			{
				int num = this.halfedges[t0];
				if (num == -1 || this.constrainedHalfedges[num] >= HalfedgeState.Constrained)
				{
					return;
				}
				int num2 = num / 3;
				if (this.visitedTriangles[num2])
				{
					return;
				}
				UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle circle = this.circles[num2];
				TUtils utils = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils2 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				TUtils utils3 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.utils;
				if (utils.le(utils2.Cast(utils3.distancesq(circle.Center, p)), circle.RadiusSq))
				{
					this.badTriangles.Add(num2);
					this.trianglesQueue.Enqueue(num2);
					this.visitedTriangles[num2] = true;
				}
			}

			// Token: 0x060000BF RID: 191 RVA: 0x000074C4 File Offset: 0x000056C4
			private void BuildAmphitheaterPolygon(int initHe)
			{
				int num = initHe;
				int num2 = this.triangles[num];
				int num4;
				for (;;)
				{
					num = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num);
					if (this.triangles[num] == num2)
					{
						break;
					}
					int num3 = this.halfedges[num];
					if (num3 == -1 || !this.badTriangles.Contains(num3 / 3))
					{
						num4 = this.triangles[num];
						this.pathPoints.Add(num4);
						this.pathHalfedges.Add(num3);
					}
					else
					{
						num = num3;
					}
				}
				num4 = this.triangles[initHe];
				this.pathPoints.Add(num4);
				num4 = -1;
				this.pathHalfedges.Add(num4);
			}

			// Token: 0x060000C0 RID: 192 RVA: 0x00007570 File Offset: 0x00005770
			private void BuildStarPolygon()
			{
				int num = -1;
				for (int i = 0; i < this.badTriangles.Length; i++)
				{
					int num2 = this.badTriangles[i];
					for (int j = 0; j < 3; j++)
					{
						int num3 = 3 * num2 + j;
						int num4 = this.halfedges[num3];
						if (num4 == -1 || !this.badTriangles.Contains(num4 / 3))
						{
							int num5 = this.triangles[num3];
							this.pathPoints.Add(num5);
							this.pathHalfedges.Add(num4);
							num = num3;
							break;
						}
					}
					if (num != -1)
					{
						break;
					}
				}
				int num6 = num;
				int num7 = this.pathPoints[0];
				for (;;)
				{
					num6 = UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.NextHalfedge(num6);
					if (this.triangles[num6] == num7)
					{
						break;
					}
					int num8 = this.halfedges[num6];
					if (num8 == -1 || !this.badTriangles.Contains(num8 / 3))
					{
						int num5 = this.triangles[num6];
						this.pathPoints.Add(num5);
						this.pathHalfedges.Add(num8);
					}
					else
					{
						num6 = num8;
					}
				}
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x00007694 File Offset: 0x00005894
			private void ProcessBadTriangles(NativeList<int> heQueue, NativeList<int> tQueue)
			{
				this.badTriangles.Sort<int>();
				for (int i = this.badTriangles.Length - 1; i >= 0; i--)
				{
					int num = this.badTriangles[i];
					this.triangles.RemoveAt(3 * num + 2);
					this.triangles.RemoveAt(3 * num + 1);
					this.triangles.RemoveAt(3 * num);
					this.circles.RemoveAt(num);
					this.RemoveHalfedge(3 * num + 2, 0);
					this.RemoveHalfedge(3 * num + 1, 1);
					this.RemoveHalfedge(3 * num, 2);
					this.constrainedHalfedges.RemoveAt(3 * num + 2);
					this.constrainedHalfedges.RemoveAt(3 * num + 1);
					this.constrainedHalfedges.RemoveAt(3 * num);
					for (int j = 3 * num; j < this.halfedges.Length; j++)
					{
						int num2 = this.halfedges[j];
						if (num2 != -1)
						{
							ref NativeList<int> ptr = ref this.halfedges;
							int index = (num2 < 3 * num) ? num2 : j;
							ptr[index] -= 3;
						}
					}
					for (int k = 0; k < this.pathHalfedges.Length; k++)
					{
						if (this.pathHalfedges[k] > 3 * num + 2)
						{
							ref NativeList<int> ptr = ref this.pathHalfedges;
							int index = k;
							ptr[index] -= 3;
						}
					}
					if (heQueue.IsCreated)
					{
						for (int l = 0; l < heQueue.Length; l++)
						{
							int num3 = heQueue[l];
							if (num3 == 3 * num || num3 == 3 * num + 1 || num3 == 3 * num + 2)
							{
								heQueue[l] = -1;
							}
							else if (num3 > 3 * num + 2)
							{
								ref NativeList<int> ptr = ref heQueue;
								int index = l;
								ptr[index] -= 3;
							}
						}
					}
					if (tQueue.IsCreated)
					{
						for (int m = 0; m < tQueue.Length; m++)
						{
							int num4 = tQueue[m];
							if (num4 == num)
							{
								tQueue[m] = -1;
							}
							else if (num4 > num)
							{
								int index = m;
								int num5 = tQueue[index];
								tQueue[index] = num5 - 1;
							}
						}
					}
				}
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x000078D0 File Offset: 0x00005AD0
			private void RemoveHalfedge(int he, int offset)
			{
				int num = this.halfedges[he];
				int num2 = (num > he) ? (num - offset) : num;
				if (num2 > -1)
				{
					this.halfedges[num2] = -1;
				}
				this.halfedges.RemoveAt(he);
			}

			// Token: 0x060000C3 RID: 195 RVA: 0x00007914 File Offset: 0x00005B14
			private void BuildNewTrianglesForStar(int pId, NativeList<int> heQueue, NativeList<int> tQueue)
			{
				int length = this.triangles.Length;
				this.triangles.Length = this.triangles.Length + 3 * this.pathPoints.Length;
				this.circles.Length = this.circles.Length + this.pathPoints.Length;
				for (int i = 0; i < this.pathPoints.Length - 1; i++)
				{
					this.triangles[length + 3 * i] = pId;
					this.triangles[length + 3 * i + 1] = this.pathPoints[i];
					this.triangles[length + 3 * i + 2] = this.pathPoints[i + 1];
					this.circles[length / 3 + i] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(pId, this.pathPoints[i], this.pathPoints[i + 1], this.outputPositions.AsArray()));
				}
				this.triangles[this.triangles.Length - 3] = pId;
				this.triangles[this.triangles.Length - 2] = this.pathPoints[this.pathPoints.Length - 1];
				this.triangles[this.triangles.Length - 1] = this.pathPoints[0];
				this.circles[this.circles.Length - 1] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(pId, this.pathPoints[this.pathPoints.Length - 1], this.pathPoints[0], this.outputPositions.AsArray()));
				int length2 = this.halfedges.Length;
				this.halfedges.Length = this.halfedges.Length + 3 * this.pathPoints.Length;
				this.constrainedHalfedges.Length = this.constrainedHalfedges.Length + 3 * this.pathPoints.Length;
				for (int j = 0; j < this.pathPoints.Length - 1; j++)
				{
					int num = this.pathHalfedges[j];
					this.halfedges[3 * j + 1 + length2] = num;
					if (num != -1)
					{
						this.halfedges[num] = 3 * j + 1 + length2;
						this.constrainedHalfedges[3 * j + 1 + length2] = this.constrainedHalfedges[num];
					}
					else
					{
						this.constrainedHalfedges[3 * j + 1 + length2] = HalfedgeState.Constrained;
					}
					this.halfedges[3 * j + 2 + length2] = 3 * j + 3 + length2;
					this.halfedges[3 * j + 3 + length2] = 3 * j + 2 + length2;
				}
				int num2 = this.pathHalfedges[this.pathHalfedges.Length - 1];
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 1) + 1] = num2;
				if (num2 != -1)
				{
					this.halfedges[num2] = length2 + 3 * (this.pathPoints.Length - 1) + 1;
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 1) + 1] = this.constrainedHalfedges[num2];
				}
				else
				{
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 1) + 1] = HalfedgeState.Constrained;
				}
				this.halfedges[length2] = length2 + 3 * (this.pathPoints.Length - 1) + 2;
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 1) + 2] = length2;
				if (heQueue.IsCreated)
				{
					for (int k = 0; k < this.pathPoints.Length - 1; k++)
					{
						int num3 = length2 + 3 * k + 1;
						if (this.constrainedHalfedges[num3] >= HalfedgeState.Constrained && this.IsEncroached(num3))
						{
							heQueue.Add(num3);
						}
						else if (tQueue.IsCreated && this.IsBadTriangle(num3 / 3))
						{
							int num4 = num3 / 3;
							tQueue.Add(num4);
						}
					}
				}
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00007D24 File Offset: 0x00005F24
			private void BuildNewTrianglesForAmphitheater(int pId, NativeList<int> heQueue, NativeList<int> tQueue)
			{
				int length = this.triangles.Length;
				this.triangles.Length = this.triangles.Length + 3 * (this.pathPoints.Length - 1);
				this.circles.Length = this.circles.Length + (this.pathPoints.Length - 1);
				for (int i = 0; i < this.pathPoints.Length - 1; i++)
				{
					this.triangles[length + 3 * i] = pId;
					this.triangles[length + 3 * i + 1] = this.pathPoints[i];
					this.triangles[length + 3 * i + 2] = this.pathPoints[i + 1];
					this.circles[length / 3 + i] = new UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle(UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.CalculateCircumCircle(pId, this.pathPoints[i], this.pathPoints[i + 1], this.outputPositions.AsArray()));
				}
				int length2 = this.halfedges.Length;
				this.halfedges.Length = this.halfedges.Length + 3 * (this.pathPoints.Length - 1);
				this.constrainedHalfedges.Length = this.constrainedHalfedges.Length + 3 * (this.pathPoints.Length - 1);
				for (int j = 0; j < this.pathPoints.Length - 2; j++)
				{
					int num = this.pathHalfedges[j];
					this.halfedges[3 * j + 1 + length2] = num;
					if (num != -1)
					{
						this.halfedges[num] = 3 * j + 1 + length2;
						this.constrainedHalfedges[3 * j + 1 + length2] = this.constrainedHalfedges[num];
					}
					else
					{
						this.constrainedHalfedges[3 * j + 1 + length2] = HalfedgeState.Constrained;
					}
					this.halfedges[3 * j + 2 + length2] = 3 * j + 3 + length2;
					this.halfedges[3 * j + 3 + length2] = 3 * j + 2 + length2;
				}
				int num2 = this.pathHalfedges[this.pathHalfedges.Length - 2];
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 2) + 1] = num2;
				if (num2 != -1)
				{
					this.halfedges[num2] = length2 + 3 * (this.pathPoints.Length - 2) + 1;
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 2) + 1] = this.constrainedHalfedges[num2];
				}
				else
				{
					this.constrainedHalfedges[length2 + 3 * (this.pathPoints.Length - 2) + 1] = HalfedgeState.Constrained;
				}
				this.halfedges[length2] = -1;
				this.halfedges[length2 + 3 * (this.pathPoints.Length - 2) + 2] = -1;
				if (heQueue.IsCreated)
				{
					for (int k = 0; k < this.pathPoints.Length - 1; k++)
					{
						int num3 = length2 + 3 * k + 1;
						if (this.constrainedHalfedges[num3] >= HalfedgeState.Constrained && this.IsEncroached(num3))
						{
							heQueue.Add(num3);
						}
						else if (tQueue.IsCreated && this.IsBadTriangle(num3 / 3))
						{
							int num4 = num3 / 3;
							tQueue.Add(num4);
						}
					}
				}
			}

			// Token: 0x04000099 RID: 153
			private NativeReference<Status> status;

			// Token: 0x0400009A RID: 154
			private NativeList<int> triangles;

			// Token: 0x0400009B RID: 155
			private NativeList<T2> outputPositions;

			// Token: 0x0400009C RID: 156
			private NativeList<int> halfedges;

			// Token: 0x0400009D RID: 157
			private NativeList<HalfedgeState> constrainedHalfedges;

			// Token: 0x0400009E RID: 158
			private NativeList<UnsafeTriangulator<T, T2, TBig, TTransform, TUtils>.RefineMeshStep.Circle> circles;

			// Token: 0x0400009F RID: 159
			private NativeQueue<int> trianglesQueue;

			// Token: 0x040000A0 RID: 160
			private NativeList<int> badTriangles;

			// Token: 0x040000A1 RID: 161
			private NativeList<int> pathPoints;

			// Token: 0x040000A2 RID: 162
			private NativeList<int> pathHalfedges;

			// Token: 0x040000A3 RID: 163
			private NativeList<bool> visitedTriangles;

			// Token: 0x040000A4 RID: 164
			private readonly T maximumArea2;

			// Token: 0x040000A5 RID: 165
			private readonly T angleThreshold;

			// Token: 0x040000A6 RID: 166
			private readonly int initialPointsCount;

			// Token: 0x040000A7 RID: 167
			private const float ConcentricShellReferenceRadius = 0.001f;

			// Token: 0x02000025 RID: 37
			private readonly struct Circle
			{
				// Token: 0x060000C5 RID: 197 RVA: 0x0000808C File Offset: 0x0000628C
				public Circle([TupleElementNames(new string[]
				{
					"center",
					"radiusSq"
				})] ValueTuple<T2, T> circle)
				{
					T2 item = circle.Item1;
					T item2 = circle.Item2;
					this.Center = item;
					this.RadiusSq = item2;
				}

				// Token: 0x040000A8 RID: 168
				public readonly T2 Center;

				// Token: 0x040000A9 RID: 169
				public readonly T RadiusSq;
			}
		}
	}
}
