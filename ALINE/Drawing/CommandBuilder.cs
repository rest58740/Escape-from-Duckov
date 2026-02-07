using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Drawing
{
	// Token: 0x0200000B RID: 11
	[BurstCompile]
	public struct CommandBuilder : IDisposable
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002409 File Offset: 0x00000609
		internal unsafe CommandBuilder(UnsafeAppendBuffer* buffer, GCHandle gizmos, int threadIndex, DrawingData.BuilderData.BitPackedMeta uniqueID)
		{
			this.buffer = buffer;
			this.gizmos = gizmos;
			this.threadIndex = threadIndex;
			this.uniqueID = uniqueID;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002428 File Offset: 0x00000628
		internal CommandBuilder(DrawingData gizmos, DrawingData.Hasher hasher, RedrawScope frameRedrawScope, RedrawScope customRedrawScope, bool isGizmos, bool isBuiltInCommandBuilder, int sceneModeVersion)
		{
			this.gizmos = GCHandle.Alloc(gizmos, GCHandleType.Normal);
			this.threadIndex = 0;
			this.uniqueID = gizmos.data.Reserve(isBuiltInCommandBuilder);
			gizmos.data.Get(this.uniqueID).Init(hasher, frameRedrawScope, customRedrawScope, isGizmos, gizmos.GetNextDrawOrderIndex(), sceneModeVersion);
			this.buffer = gizmos.data.Get(this.uniqueID).bufferPtr;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000249C File Offset: 0x0000069C
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000024A9 File Offset: 0x000006A9
		internal unsafe int BufferSize
		{
			get
			{
				return this.buffer->Length;
			}
			set
			{
				this.buffer->Length = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000024B7 File Offset: 0x000006B7
		public CommandBuilder2D xy
		{
			get
			{
				return new CommandBuilder2D(this, true);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000024C5 File Offset: 0x000006C5
		public CommandBuilder2D xz
		{
			get
			{
				return new CommandBuilder2D(this, false);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000024D4 File Offset: 0x000006D4
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002548 File Offset: 0x00000748
		public Camera[] cameraTargets
		{
			get
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (drawingData.data.StillExists(this.uniqueID))
					{
						return drawingData.data.Get(this.uniqueID).meta.cameraTargets;
					}
				}
				throw new Exception("Cannot get cameraTargets because the command builder has already been disposed or does not exist.");
			}
			set
			{
				if (this.uniqueID.isBuiltInCommandBuilder)
				{
					throw new Exception("You cannot set the camera targets for a built-in command builder. Create a custom command builder instead.");
				}
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot set cameraTargets because the command builder has already been disposed or does not exist.");
					}
					drawingData.data.Get(this.uniqueID).meta.cameraTargets = value;
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025D0 File Offset: 0x000007D0
		public void Dispose()
		{
			if (this.uniqueID.isBuiltInCommandBuilder)
			{
				throw new Exception("You cannot dispose a built-in command builder");
			}
			this.DisposeInternal();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025F0 File Offset: 0x000007F0
		public void DisposeAfter(JobHandle dependency, AllowedDelay allowedDelay = AllowedDelay.EndOfFrame)
		{
			if (!this.gizmos.IsAllocated)
			{
				throw new Exception("You cannot dispose an invalid command builder. Are you trying to dispose it twice?");
			}
			try
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot dispose the command builder because the drawing manager has been destroyed");
					}
					drawingData.data.Get(this.uniqueID).SubmitWithDependency(this.gizmos, dependency, allowedDelay);
				}
			}
			finally
			{
				this = default(CommandBuilder);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002698 File Offset: 0x00000898
		internal void DisposeInternal()
		{
			if (!this.gizmos.IsAllocated)
			{
				throw new Exception("You cannot dispose an invalid command builder. Are you trying to dispose it twice?");
			}
			try
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot dispose the command builder because the drawing manager has been destroyed");
					}
					drawingData.data.Get(this.uniqueID).Submit(this.gizmos.Target as DrawingData);
				}
			}
			finally
			{
				this.gizmos.Free();
				this = default(CommandBuilder);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002750 File Offset: 0x00000950
		public void DiscardAndDispose()
		{
			if (this.uniqueID.isBuiltInCommandBuilder)
			{
				throw new Exception("You cannot dispose a built-in command builder");
			}
			this.DiscardAndDisposeInternal();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002770 File Offset: 0x00000970
		internal void DiscardAndDisposeInternal()
		{
			try
			{
				if (this.gizmos.IsAllocated && this.gizmos.Target != null)
				{
					DrawingData drawingData = this.gizmos.Target as DrawingData;
					if (!drawingData.data.StillExists(this.uniqueID))
					{
						throw new Exception("Cannot dispose the command builder because the drawing manager has been destroyed");
					}
					drawingData.data.Release(this.uniqueID);
				}
			}
			finally
			{
				if (this.gizmos.IsAllocated)
				{
					this.gizmos.Free();
				}
				this = default(CommandBuilder);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002808 File Offset: 0x00000A08
		public void Preallocate(int size)
		{
			this.Reserve(size);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002814 File Offset: 0x00000A14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void Reserve(int additionalSpace)
		{
			if (Hint.Unlikely(this.threadIndex >= 0))
			{
				this.buffer += this.threadIndex;
				this.threadIndex = -1;
			}
			int num = this.buffer->Length + additionalSpace;
			if (num > this.buffer->Capacity)
			{
				this.buffer->SetCapacity(math.max(num, this.buffer->Length * 2));
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002890 File Offset: 0x00000A90
		[BurstDiscard]
		private void AssertBufferExists()
		{
			if (!this.gizmos.IsAllocated || this.gizmos.Target == null || !(this.gizmos.Target as DrawingData).data.StillExists(this.uniqueID))
			{
				this = default(CommandBuilder);
				throw new Exception("This command builder no longer exists. Are you trying to draw to a command builder which has already been disposed?");
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028EB File Offset: 0x00000AEB
		[BurstDiscard]
		private static void AssertNotRendering()
		{
			if (!GizmoContext.drawingGizmos && !JobsUtility.IsExecutingJob && (Time.renderedFrameCount & 127) == 0 && StackTraceUtility.ExtractStackTrace().Contains("OnDrawGizmos"))
			{
				throw new Exception("You are trying to use Draw.* functions from within Unity's OnDrawGizmos function. Use this package's gizmo callbacks instead (see the documentation).");
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002921 File Offset: 0x00000B21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Reserve<A>() where A : struct
		{
			this.Reserve(UnsafeUtility.SizeOf<CommandBuilder.Command>() + UnsafeUtility.SizeOf<A>());
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002934 File Offset: 0x00000B34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Reserve<A, B>() where A : struct where B : struct
		{
			this.Reserve(UnsafeUtility.SizeOf<CommandBuilder.Command>() * 2 + UnsafeUtility.SizeOf<A>() + UnsafeUtility.SizeOf<B>());
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000294F File Offset: 0x00000B4F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Reserve<A, B, C>() where A : struct where B : struct where C : struct
		{
			this.Reserve(UnsafeUtility.SizeOf<CommandBuilder.Command>() * 3 + UnsafeUtility.SizeOf<A>() + UnsafeUtility.SizeOf<B>() + UnsafeUtility.SizeOf<C>());
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002970 File Offset: 0x00000B70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static uint ConvertColor(Color color)
		{
			if (X86.Sse2.IsSse2Supported)
			{
				int4 @int = (int4)(255f * new float4(color.r, color.g, color.b, color.a) + 0.5f);
				v128 v = new v128(@int.x, @int.y, @int.z, @int.w);
				v128 v2 = X86.Sse2.packs_epi32(v, v);
				return X86.Sse2.packus_epi16(v2, v2).UInt0;
			}
			uint num = (uint)Mathf.Clamp((int)(color.r * 255f + 0.5f), 0, 255);
			uint num2 = (uint)Mathf.Clamp((int)(color.g * 255f + 0.5f), 0, 255);
			uint num3 = (uint)Mathf.Clamp((int)(color.b * 255f + 0.5f), 0, 255);
			return (uint)(Mathf.Clamp((int)(color.a * 255f + 0.5f), 0, 255) << 24 | (int)((int)num3 << 16) | (int)((int)num2 << 8) | (int)num);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A74 File Offset: 0x00000C74
		internal unsafe void Add<T>(T value) where T : struct
		{
			int num = UnsafeUtility.SizeOf<T>();
			UnsafeAppendBuffer* ptr = this.buffer;
			int length = ptr->Length;
			Hint.Assume(ptr->Ptr != null);
			Hint.Assume(ptr->Ptr + length != null);
			UnsafeUtility.CopyStructureToPtr<T>(ref value, (void*)(ptr->Ptr + length));
			ptr->Length = length + num;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002AD4 File Offset: 0x00000CD4
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(Matrix4x4 matrix)
		{
			this.PushMatrix(matrix);
			return new CommandBuilder.ScopeMatrix
			{
				builder = this
			};
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B00 File Offset: 0x00000D00
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(float3x3 matrix)
		{
			this.PushMatrix(new float4x4(matrix, float3.zero));
			return new CommandBuilder.ScopeMatrix
			{
				builder = this
			};
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B34 File Offset: 0x00000D34
		[BurstDiscard]
		public CommandBuilder.ScopeColor WithColor(Color color)
		{
			this.PushColor(color);
			return new CommandBuilder.ScopeColor
			{
				builder = this
			};
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B60 File Offset: 0x00000D60
		[BurstDiscard]
		public CommandBuilder.ScopePersist WithDuration(float duration)
		{
			this.PushDuration(duration);
			return new CommandBuilder.ScopePersist
			{
				builder = this
			};
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B8C File Offset: 0x00000D8C
		[BurstDiscard]
		public CommandBuilder.ScopeLineWidth WithLineWidth(float pixels, bool automaticJoins = true)
		{
			this.PushLineWidth(pixels, automaticJoins);
			return new CommandBuilder.ScopeLineWidth
			{
				builder = this
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002BB7 File Offset: 0x00000DB7
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix InLocalSpace(Transform transform)
		{
			return this.WithMatrix(transform.localToWorldMatrix);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002BC8 File Offset: 0x00000DC8
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix InScreenSpace(Camera camera)
		{
			return this.WithMatrix(camera.cameraToWorldMatrix * camera.nonJitteredProjectionMatrix.inverse * Matrix4x4.TRS(new Vector3(-1f, -1f, 0f), Quaternion.identity, new Vector3(2f / (float)camera.pixelWidth, 2f / (float)camera.pixelHeight, 1f)));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002C3B File Offset: 0x00000E3B
		public void PushMatrix(Matrix4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushMatrix);
			this.Add<Matrix4x4>(matrix);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C51 File Offset: 0x00000E51
		public void PushMatrix(float4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushMatrix);
			this.Add<float4x4>(matrix);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C67 File Offset: 0x00000E67
		public void PushSetMatrix(Matrix4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushSetMatrix);
			this.Add<float4x4>(matrix);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C82 File Offset: 0x00000E82
		public void PushSetMatrix(float4x4 matrix)
		{
			this.Reserve<float4x4>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushSetMatrix);
			this.Add<float4x4>(matrix);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002C98 File Offset: 0x00000E98
		public void PopMatrix()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopMatrix);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public void PushColor(Color color)
		{
			this.Reserve<Color32>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColor);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CC3 File Offset: 0x00000EC3
		public void PopColor()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopColor);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public unsafe void PushDuration(float duration)
		{
			this.Reserve<CommandBuilder.PersistData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushPersist);
			this.Add<CommandBuilder.PersistData>(new CommandBuilder.PersistData
			{
				endTime = *SharedDrawingData.BurstTime.Data + duration
			});
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002D12 File Offset: 0x00000F12
		public void PopDuration()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopPersist);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002D23 File Offset: 0x00000F23
		[Obsolete("Renamed to PushDuration for consistency")]
		public void PushPersist(float duration)
		{
			this.PushDuration(duration);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D2C File Offset: 0x00000F2C
		[Obsolete("Renamed to PopDuration for consistency")]
		public void PopPersist()
		{
			this.PopDuration();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D34 File Offset: 0x00000F34
		public void PushLineWidth(float pixels, bool automaticJoins = true)
		{
			if (pixels < 0f)
			{
				throw new ArgumentOutOfRangeException("pixels", "Line width must be positive");
			}
			this.Reserve<CommandBuilder.LineWidthData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushLineWidth);
			this.Add<CommandBuilder.LineWidthData>(new CommandBuilder.LineWidthData
			{
				pixels = pixels,
				automaticJoins = automaticJoins
			});
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D86 File Offset: 0x00000F86
		public void PopLineWidth()
		{
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PopLineWidth);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002D98 File Offset: 0x00000F98
		public void Line(float3 a, float3 b)
		{
			this.Reserve<CommandBuilder.LineData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Line);
			this.Add<CommandBuilder.LineData>(new CommandBuilder.LineData
			{
				a = a,
				b = b
			});
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public unsafe void Line(Vector3 a, Vector3 b)
		{
			this.Reserve<CommandBuilder.LineData>();
			int bufferSize = this.BufferSize;
			int length = bufferSize + 4 + 24;
			byte* ptr = this.buffer->Ptr + bufferSize;
			*(int*)ptr = 5;
			CommandBuilder.LineDataV3* ptr2 = (CommandBuilder.LineDataV3*)(ptr + 4);
			ptr2->a = a;
			ptr2->b = b;
			this.buffer->Length = length;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E24 File Offset: 0x00001024
		public unsafe void Line(Vector3 a, Vector3 b, Color color)
		{
			this.Reserve<Color32, CommandBuilder.LineData>();
			int bufferSize = this.BufferSize;
			int length = bufferSize + 4 + 24 + 4;
			byte* ptr = this.buffer->Ptr + bufferSize;
			*(int*)ptr = 261;
			*(int*)(ptr + 4) = (int)CommandBuilder.ConvertColor(color);
			CommandBuilder.LineDataV3* ptr2 = (CommandBuilder.LineDataV3*)(ptr + 8);
			ptr2->a = a;
			ptr2->b = b;
			this.buffer->Length = length;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002E82 File Offset: 0x00001082
		public void Ray(float3 origin, float3 direction)
		{
			this.Line(origin, origin + direction);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E92 File Offset: 0x00001092
		public void Ray(Ray ray, float length)
		{
			this.Line(ray.origin, ray.origin + ray.direction * length);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002EBC File Offset: 0x000010BC
		public void Arc(float3 center, float3 start, float3 end)
		{
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3 != 0f) && math.all(math.isfinite(float3)))
			{
				Matrix4x4 matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix);
				this.CircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F6C File Offset: 0x0000116C
		[Obsolete("Use Draw.xz.Circle instead")]
		public void CircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.CircleXZInternal(center, radius, startAngle, endAngle);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F7C File Offset: 0x0000117C
		internal void CircleXZInternal(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.Reserve<CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.CircleXZ);
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FC8 File Offset: 0x000011C8
		internal void CircleXZInternal(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.PopMatrix);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003023 File Offset: 0x00001223
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.CircleXZ(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			this.PopMatrix();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003058 File Offset: 0x00001258
		public void Circle(float3 center, float3 normal, float radius)
		{
			this.Reserve<CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Circle);
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000309C File Offset: 0x0000129C
		public void SolidArc(float3 center, float3 start, float3 end)
		{
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3))
			{
				Matrix4x4 matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix);
				this.SolidCircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003135 File Offset: 0x00001335
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public void SolidCircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.SolidCircleXZInternal(center, radius, startAngle, endAngle);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003144 File Offset: 0x00001344
		internal void SolidCircleXZInternal(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.Reserve<CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.DiscXZ);
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003190 File Offset: 0x00001390
		internal void SolidCircleXZInternal(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleXZData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleXZData>(new CommandBuilder.CircleXZData
			{
				center = center,
				radius = radius,
				startAngle = startAngle,
				endAngle = endAngle
			});
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000031EB File Offset: 0x000013EB
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public void SolidCircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.SolidCircleXZInternal(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			this.PopMatrix();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003220 File Offset: 0x00001420
		public void SolidCircle(float3 center, float3 normal, float radius)
		{
			this.Reserve<CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Disc);
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003264 File Offset: 0x00001464
		public void SphereOutline(float3 center, float radius)
		{
			this.Reserve<CommandBuilder.SphereData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.SphereOutline);
			this.Add<CommandBuilder.SphereData>(new CommandBuilder.SphereData
			{
				center = center,
				radius = radius
			});
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000329E File Offset: 0x0000149E
		public void WireCylinder(float3 bottom, float3 top, float radius)
		{
			this.WireCylinder(bottom, top - bottom, math.length(top - bottom), radius);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000032BC File Offset: 0x000014BC
		public void WireCylinder(float3 position, float3 up, float height, float radius)
		{
			up = math.normalizesafe(up, default(float3));
			if (math.all(up == 0f) || math.any(math.isnan(up)) || math.isnan(height) || math.isnan(radius))
			{
				return;
			}
			float3 lhs;
			float3 lhs2;
			CommandBuilder.OrthonormalBasis(up, out lhs, out lhs2);
			this.PushMatrix(new float4x4(new float4(lhs * radius, 0f), new float4(up * height, 0f), new float4(lhs2 * radius, 0f), new float4(position, 1f)));
			this.CircleXZInternal(float3.zero, 1f, 0f, 6.2831855f);
			if (height > 0f)
			{
				this.CircleXZInternal(new float3(0f, 1f, 0f), 1f, 0f, 6.2831855f);
				this.Line(new float3(1f, 0f, 0f), new float3(1f, 1f, 0f));
				this.Line(new float3(-1f, 0f, 0f), new float3(-1f, 1f, 0f));
				this.Line(new float3(0f, 0f, 1f), new float3(0f, 1f, 1f));
				this.Line(new float3(0f, 0f, -1f), new float3(0f, 1f, -1f));
			}
			this.PopMatrix();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000346C File Offset: 0x0000166C
		private static void OrthonormalBasis(float3 normal, out float3 basis1, out float3 basis2)
		{
			basis1 = math.cross(normal, new float3(1f, 1f, 1f));
			if (math.all(basis1 == 0f))
			{
				basis1 = math.cross(normal, new float3(-1f, 1f, 1f));
			}
			basis1 = math.normalizesafe(basis1, default(float3));
			basis2 = math.cross(normal, basis1);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000034FC File Offset: 0x000016FC
		public void WireCapsule(float3 start, float3 end, float radius)
		{
			float3 @float = end - start;
			float num = math.length(@float);
			if ((double)num < 0.0001)
			{
				this.WireSphere(start, radius);
				return;
			}
			float3 float2 = @float / num;
			this.WireCapsule(start - float2 * radius, float2, num + 2f * radius, radius);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003554 File Offset: 0x00001754
		public void WireCapsule(float3 position, float3 direction, float length, float radius)
		{
			direction = math.normalizesafe(direction, default(float3));
			if (math.all(direction == 0f) || math.any(math.isnan(direction)) || math.isnan(length) || math.isnan(radius))
			{
				return;
			}
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
				return;
			}
			length = math.max(length, radius * 2f);
			float3 xyz;
			float3 xyz2;
			CommandBuilder.OrthonormalBasis(direction, out xyz, out xyz2);
			this.PushMatrix(new float4x4(new float4(xyz, 0f), new float4(direction, 0f), new float4(xyz2, 0f), new float4(position, 1f)));
			this.CircleXZInternal(new float3(0f, radius, 0f), radius, 0f, 6.2831855f);
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.CircleXZInternal(new float3(0f, 0f, radius), radius, 3.1415927f, 6.2831855f);
			this.PopMatrix();
			this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
			this.CircleXZInternal(new float3(radius, 0f, 0f), radius, 1.5707964f, 4.712389f);
			this.PopMatrix();
			if (length > 0f)
			{
				float num = length - radius;
				this.CircleXZInternal(new float3(0f, num, 0f), radius, 0f, 6.2831855f);
				this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
				this.CircleXZInternal(new float3(0f, 0f, num), radius, 0f, 3.1415927f);
				this.PopMatrix();
				this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
				this.CircleXZInternal(new float3(num, 0f, 0f), radius, -1.5707964f, 1.5707964f);
				this.PopMatrix();
				this.Line(new float3(radius, radius, 0f), new float3(radius, num, 0f));
				this.Line(new float3(-radius, radius, 0f), new float3(-radius, num, 0f));
				this.Line(new float3(0f, radius, radius), new float3(0f, num, radius));
				this.Line(new float3(0f, radius, -radius), new float3(0f, num, -radius));
			}
			this.PopMatrix();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000037C8 File Offset: 0x000019C8
		public void WireSphere(float3 position, float radius)
		{
			this.SphereOutline(position, radius);
			this.Circle(position, new float3(1f, 0f, 0f), radius);
			this.Circle(position, new float3(0f, 1f, 0f), radius);
			this.Circle(position, new float3(0f, 0f, 1f), radius);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003834 File Offset: 0x00001A34
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle = false)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003894 File Offset: 0x00001A94
		public void Polyline<T>(T points, bool cycle = false) where T : IReadOnlyList<float3>
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003924 File Offset: 0x00001B24
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003978 File Offset: 0x00001B78
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000039CC File Offset: 0x00001BCC
		public void Polyline(NativeArray<float3> points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003A30 File Offset: 0x00001C30
		public void DashedLine(float3 a, float3 b, float dash, float gap)
		{
			CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.None, gap, 0f, dash + gap, false);
			polylineWithSymbol.MoveTo(ref this, a);
			polylineWithSymbol.MoveTo(ref this, b);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003A64 File Offset: 0x00001C64
		public void DashedPolyline(List<Vector3> points, float dash, float gap)
		{
			CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.None, gap, 0f, dash + gap, false);
			for (int i = 0; i < points.Count; i++)
			{
				polylineWithSymbol.MoveTo(ref this, points[i]);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003AA8 File Offset: 0x00001CA8
		public void WireBox(float3 center, float3 size)
		{
			this.Reserve<CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.WireBox);
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003AE2 File Offset: 0x00001CE2
		public void WireBox(float3 center, quaternion rotation, float3 size)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.WireBox(float3.zero, new float3(1f, 1f, 1f));
			this.PopMatrix();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003B17 File Offset: 0x00001D17
		public void WireBox(Bounds bounds)
		{
			this.WireBox(bounds.center, bounds.size);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003B38 File Offset: 0x00001D38
		public void WireMesh(Mesh mesh)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException();
			}
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
			Mesh.MeshData meshData = meshDataArray[0];
			CommandBuilder.JobWireMesh.JobWireMeshFunctionPointer(ref meshData, ref this);
			meshDataArray.Dispose();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003B78 File Offset: 0x00001D78
		public unsafe void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles)
		{
			CommandBuilder.JobWireMesh.WireMesh((float3*)vertices.GetUnsafeReadOnlyPtr<float3>(), (int*)triangles.GetUnsafeReadOnlyPtr<int>(), vertices.Length, triangles.Length, ref this);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003B9A File Offset: 0x00001D9A
		public void SolidMesh(Mesh mesh)
		{
			this.SolidMeshInternal(mesh, false);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003BA4 File Offset: 0x00001DA4
		private void SolidMeshInternal(Mesh mesh, bool temporary, Color color)
		{
			this.PushColor(color);
			this.SolidMeshInternal(mesh, temporary);
			this.PopColor();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003BBC File Offset: 0x00001DBC
		private void SolidMeshInternal(Mesh mesh, bool temporary)
		{
			(this.gizmos.Target as DrawingData).data.Get(this.uniqueID).meshes.Add(new DrawingData.SubmittedMesh
			{
				mesh = mesh,
				temporary = temporary
			});
			this.Reserve(4);
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.CaptureState);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003C1C File Offset: 0x00001E1C
		[BurstDiscard]
		public void SolidMesh(List<Vector3> vertices, List<int> triangles, List<Color> colors)
		{
			if (vertices.Count != colors.Count)
			{
				throw new ArgumentException("Number of colors must be the same as the number of vertices");
			}
			Mesh mesh = (this.gizmos.Target as DrawingData).GetMesh(vertices.Count);
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);
			mesh.SetColors(colors);
			mesh.UploadMeshData(false);
			this.SolidMeshInternal(mesh, true);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003C8C File Offset: 0x00001E8C
		[BurstDiscard]
		public void SolidMesh(Vector3[] vertices, int[] triangles, Color[] colors, int vertexCount, int indexCount)
		{
			if (vertices.Length != colors.Length)
			{
				throw new ArgumentException("Number of colors must be the same as the number of vertices");
			}
			Mesh mesh = (this.gizmos.Target as DrawingData).GetMesh(vertices.Length);
			mesh.Clear();
			mesh.SetVertices(vertices, 0, vertexCount);
			mesh.SetTriangles(triangles, 0, indexCount, 0, true, 0);
			mesh.SetColors(colors, 0, vertexCount);
			mesh.UploadMeshData(false);
			this.SolidMeshInternal(mesh, true);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003CFC File Offset: 0x00001EFC
		public void Cross(float3 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003DA8 File Offset: 0x00001FA8
		[Obsolete("Use Draw.xz.Cross instead")]
		public void CrossXZ(float3 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003E24 File Offset: 0x00002024
		[Obsolete("Use Draw.xy.Cross instead")]
		public void CrossXY(float3 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003EA0 File Offset: 0x000020A0
		public static float3 EvaluateCubicBezier(float3 p0, float3 p1, float3 p2, float3 p3, float t)
		{
			t = math.clamp(t, 0f, 1f);
			float num = 1f - t;
			return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003F18 File Offset: 0x00002118
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			float3 a = p0;
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				float3 @float = CommandBuilder.EvaluateCubicBezier(p0, p1, p2, p3, t);
				this.Line(a, @float);
				a = @float;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003F54 File Offset: 0x00002154
		public void CatmullRom(List<Vector3> points)
		{
			if (points.Count < 2)
			{
				return;
			}
			if (points.Count == 2)
			{
				this.Line(points[0], points[1]);
				return;
			}
			int count = points.Count;
			this.CatmullRom(points[0], points[0], points[1], points[2]);
			int num = 0;
			while (num + 3 < count)
			{
				this.CatmullRom(points[num], points[num + 1], points[num + 2], points[num + 3]);
				num++;
			}
			this.CatmullRom(points[count - 3], points[count - 2], points[count - 1], points[count - 1]);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004050 File Offset: 0x00002250
		public void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			float3 p4 = (-p0 + 6f * p1 + 1f * p2) * 0.16666667f;
			float3 p5 = (p1 + 6f * p2 - p3) * 0.16666667f;
			this.Bezier(p1, p4, p5, p2);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000040BF File Offset: 0x000022BF
		public void Arrow(float3 from, float3 to)
		{
			this.ArrowRelativeSizeHead(from, to, CommandBuilder.DEFAULT_UP, 0.2f);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000040D4 File Offset: 0x000022D4
		public void Arrow(float3 from, float3 to, float3 up, float headSize)
		{
			float num = math.lengthsq(to - from);
			if (num > 1E-06f)
			{
				this.ArrowRelativeSizeHead(from, to, up, headSize * math.rsqrt(num));
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004108 File Offset: 0x00002308
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction)
		{
			this.Line(from, to);
			float3 @float = to - from;
			float3 float2 = math.cross(@float, up);
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(1f, 0f, 0f), @float);
			}
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(0f, 1f, 0f), @float);
			}
			float2 = math.normalizesafe(float2, default(float3)) * math.length(@float);
			this.Line(to, to - (@float + float2) * headFraction);
			this.Line(to, to - (@float - float2) * headFraction);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000041D8 File Offset: 0x000023D8
		public void Arrowhead(float3 center, float3 direction, float radius)
		{
			this.Arrowhead(center, direction, CommandBuilder.DEFAULT_UP, radius);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000041E8 File Offset: 0x000023E8
		public void Arrowhead(float3 center, float3 direction, float3 up, float radius)
		{
			if (math.all(direction == 0f))
			{
				return;
			}
			direction = math.normalizesafe(direction, default(float3));
			float3 rhs = math.cross(direction, up);
			float3 @float = center - radius * 0.5f * 0.5f * direction;
			float3 float2 = @float + radius * direction;
			float3 float3 = @float - radius * 0.5f * direction + radius * 0.866025f * rhs;
			float3 float4 = @float - radius * 0.5f * direction - radius * 0.866025f * rhs;
			this.Line(float2, float3);
			this.Line(float3, @float);
			this.Line(@float, float4);
			this.Line(float4, float2);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000042C0 File Offset: 0x000024C0
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width = 60f)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			Quaternion q = Quaternion.LookRotation(direction, CommandBuilder.DEFAULT_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, q, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.CircleXZInternal(float3.zero, offset, num, num2);
			float3 a = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 b = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 @float = new float3(0f, 0f, 1.4142f * offset);
			this.Line(a, @float);
			this.Line(@float, b);
			this.PopMatrix();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000043B8 File Offset: 0x000025B8
		public void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize)
		{
			cells = math.max(cells, new int2(1, 1));
			this.PushMatrix(float4x4.TRS(center, rotation, new Vector3(totalSize.x, 0f, totalSize.y)));
			int x = cells.x;
			int y = cells.y;
			for (int i = 0; i <= x; i++)
			{
				this.Line(new float3((float)i / (float)x - 0.5f, 0f, -0.5f), new float3((float)i / (float)x - 0.5f, 0f, 0.5f));
			}
			for (int j = 0; j <= y; j++)
			{
				this.Line(new float3(-0.5f, 0f, (float)j / (float)y - 0.5f), new float3(0.5f, 0f, (float)j / (float)y - 0.5f));
			}
			this.PopMatrix();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000449E File Offset: 0x0000269E
		public void WireTriangle(float3 a, float3 b, float3 c)
		{
			this.Line(a, b);
			this.Line(b, c);
			this.Line(c, a);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000044B8 File Offset: 0x000026B8
		[Obsolete("Use Draw.xz.WireRectangle instead")]
		public void WireRectangleXZ(float3 center, float2 size)
		{
			this.WireRectangle(center, quaternion.identity, size);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000044C7 File Offset: 0x000026C7
		public void WireRectangle(float3 center, quaternion rotation, float2 size)
		{
			this.WirePlane(center, rotation, size);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000044D4 File Offset: 0x000026D4
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public void WireRectangle(Rect rect)
		{
			this.xy.WireRectangle(rect);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000044F0 File Offset: 0x000026F0
		public void WireTriangle(float3 center, quaternion rotation, float radius)
		{
			this.WirePolygon(center, 3, rotation, radius);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000044FC File Offset: 0x000026FC
		public void WirePentagon(float3 center, quaternion rotation, float radius)
		{
			this.WirePolygon(center, 5, rotation, radius);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004508 File Offset: 0x00002708
		public void WireHexagon(float3 center, quaternion rotation, float radius)
		{
			this.WirePolygon(center, 6, rotation, radius);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004514 File Offset: 0x00002714
		public void WirePolygon(float3 center, int vertices, quaternion rotation, float radius)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, new float3(radius, radius, radius)));
			float3 a = new float3(0f, 0f, 1f);
			for (int i = 1; i <= vertices; i++)
			{
				float x = 6.2831855f * ((float)i / (float)vertices);
				float3 @float = new float3(math.sin(x), 0f, math.cos(x));
				this.Line(a, @float);
				a = @float;
			}
			this.PopMatrix();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004590 File Offset: 0x00002790
		[Obsolete("Use Draw.xy.SolidRectangle instead")]
		public void SolidRectangle(Rect rect)
		{
			this.xy.SolidRectangle(rect);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000045AC File Offset: 0x000027AC
		public void SolidPlane(float3 center, float3 normal, float2 size)
		{
			if (math.any(normal))
			{
				this.SolidPlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000045DC File Offset: 0x000027DC
		public void SolidPlane(float3 center, quaternion rotation, float2 size)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, new float3(size.x, 0f, size.y)));
			this.Reserve<CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Box);
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = 0,
				size = 1
			});
			this.PopMatrix();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000464C File Offset: 0x0000284C
		private static float3 calculateTangent(float3 normal)
		{
			float3 @float = math.cross(new float3(0f, 1f, 0f), normal);
			if (math.all(@float == 0f))
			{
				@float = math.cross(new float3(1f, 0f, 0f), normal);
			}
			return @float;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000046A2 File Offset: 0x000028A2
		public void WirePlane(float3 center, float3 normal, float2 size)
		{
			if (math.any(normal))
			{
				this.WirePlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000046D0 File Offset: 0x000028D0
		public void WirePlane(float3 center, quaternion rotation, float2 size)
		{
			this.Reserve<CommandBuilder.PlaneData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.WirePlane);
			this.Add<CommandBuilder.PlaneData>(new CommandBuilder.PlaneData
			{
				center = center,
				rotation = rotation,
				size = size
			});
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004712 File Offset: 0x00002912
		public void PlaneWithNormal(float3 center, float3 normal, float2 size)
		{
			if (math.any(normal))
			{
				this.PlaneWithNormal(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004740 File Offset: 0x00002940
		public void PlaneWithNormal(float3 center, quaternion rotation, float2 size)
		{
			this.SolidPlane(center, rotation, size);
			this.WirePlane(center, rotation, size);
			this.ArrowRelativeSizeHead(center, center + math.mul(rotation, new float3(0f, 1f, 0f)) * 0.5f, math.mul(rotation, new float3(0f, 0f, 1f)), 0.2f);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000047B0 File Offset: 0x000029B0
		public void SolidTriangle(float3 a, float3 b, float3 c)
		{
			this.Reserve<CommandBuilder.TriangleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.SolidTriangle);
			this.Add<CommandBuilder.TriangleData>(new CommandBuilder.TriangleData
			{
				a = a,
				b = b,
				c = c
			});
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000047F4 File Offset: 0x000029F4
		public void SolidBox(float3 center, float3 size)
		{
			this.Reserve<CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Box);
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000482E File Offset: 0x00002A2E
		public void SolidBox(Bounds bounds)
		{
			this.SolidBox(bounds.center, bounds.size);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000484E File Offset: 0x00002A4E
		public void SolidBox(float3 center, quaternion rotation, float3 size)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.SolidBox(float3.zero, Vector3.one);
			this.PopMatrix();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004879 File Offset: 0x00002A79
		public void Label3D(float3 position, quaternion rotation, string text, float size)
		{
			this.Label3D(position, rotation, text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000488C File Offset: 0x00002A8C
		public void Label3D(float3 position, quaternion rotation, string text, float size, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			this.Reserve<CommandBuilder.TextData3D>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text3D);
			this.Add<CommandBuilder.TextData3D>(new CommandBuilder.TextData3D
			{
				center = position,
				rotation = rotation,
				numCharacters = text.Length,
				size = size,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * text.Length);
			foreach (char c in text)
			{
				ushort value = (ushort)drawingData.fontData.GetIndex(c);
				this.Add<ushort>(value);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000493E File Offset: 0x00002B3E
		public void Label2D(float3 position, string text, float sizeInPixels = 14f)
		{
			this.Label2D(position, text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004950 File Offset: 0x00002B50
		public void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			this.Reserve<CommandBuilder.TextData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text);
			this.Add<CommandBuilder.TextData>(new CommandBuilder.TextData
			{
				center = position,
				numCharacters = text.Length,
				sizeInPixels = sizeInPixels,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * text.Length);
			foreach (char c in text)
			{
				ushort value = (ushort)drawingData.fontData.GetIndex(c);
				this.Add<ushort>(value);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000049F9 File Offset: 0x00002BF9
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004A09 File Offset: 0x00002C09
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004A19 File Offset: 0x00002C19
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004A29 File Offset: 0x00002C29
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004A39 File Offset: 0x00002C39
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004A51 File Offset: 0x00002C51
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004A69 File Offset: 0x00002C69
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004A81 File Offset: 0x00002C81
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004A9C File Offset: 0x00002C9C
		internal unsafe void Label2D(float3 position, byte* text, int byteCount, float sizeInPixels, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			this.Reserve<CommandBuilder.TextData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text);
			this.Add<CommandBuilder.TextData>(new CommandBuilder.TextData
			{
				center = position,
				numCharacters = byteCount,
				sizeInPixels = sizeInPixels,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * byteCount);
			for (int i = 0; i < byteCount; i++)
			{
				ushort num = (ushort)text[i];
				if (num >= 128)
				{
					num = 63;
				}
				if (num == 10)
				{
					num = ushort.MaxValue;
				}
				if (num != 13)
				{
					this.Add<ushort>(num);
				}
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004B2E File Offset: 0x00002D2E
		public void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004B40 File Offset: 0x00002D40
		public void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004B52 File Offset: 0x00002D52
		public void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004B64 File Offset: 0x00002D64
		public void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004B76 File Offset: 0x00002D76
		public void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004B90 File Offset: 0x00002D90
		public void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004BAA File Offset: 0x00002DAA
		public void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004BC4 File Offset: 0x00002DC4
		public void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, LabelAlignment alignment)
		{
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004BE0 File Offset: 0x00002DE0
		internal unsafe void Label3D(float3 position, quaternion rotation, byte* text, int byteCount, float size, LabelAlignment alignment)
		{
			this.AssertBufferExists();
			this.Reserve<CommandBuilder.TextData3D>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.Text3D);
			this.Add<CommandBuilder.TextData3D>(new CommandBuilder.TextData3D
			{
				center = position,
				rotation = rotation,
				numCharacters = byteCount,
				size = size,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * byteCount);
			for (int i = 0; i < byteCount; i++)
			{
				ushort num = (ushort)text[i];
				if (num >= 128)
				{
					num = 63;
				}
				if (num == 10)
				{
					num = ushort.MaxValue;
				}
				this.Add<ushort>(num);
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004C78 File Offset: 0x00002E78
		public void Line(float3 a, float3 b, Color color)
		{
			this.Reserve<Color32, CommandBuilder.LineData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PopMatrix);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.LineData>(new CommandBuilder.LineData
			{
				a = a,
				b = b
			});
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004CC1 File Offset: 0x00002EC1
		public void Ray(float3 origin, float3 direction, Color color)
		{
			this.Line(origin, origin + direction, color);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004CD2 File Offset: 0x00002ED2
		public void Ray(Ray ray, float length, Color color)
		{
			this.Line(ray.origin, ray.origin + ray.direction * length, color);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004CFC File Offset: 0x00002EFC
		public void Arc(float3 center, float3 start, float3 end, Color color)
		{
			this.PushColor(color);
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3 != 0f) && math.all(math.isfinite(float3)))
			{
				Matrix4x4 matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix);
				this.CircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
			this.PopColor();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004DBA File Offset: 0x00002FBA
		[Obsolete("Use Draw.xz.Circle instead")]
		public void CircleXZ(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.CircleXZInternal(center, radius, startAngle, endAngle, color);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004DC9 File Offset: 0x00002FC9
		[Obsolete("Use Draw.xz.Circle instead")]
		public void CircleXZ(float3 center, float radius, Color color)
		{
			this.CircleXZ(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004DE0 File Offset: 0x00002FE0
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.CircleXZ(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004E2E File Offset: 0x0000302E
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, Color color)
		{
			this.CircleXY(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E44 File Offset: 0x00003044
		public void Circle(float3 center, float3 normal, float radius, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.PopMatrix);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004E98 File Offset: 0x00003098
		public void SolidArc(float3 center, float3 start, float3 end, Color color)
		{
			this.PushColor(color);
			float3 @float = start - center;
			float3 float2 = end - center;
			float3 float3 = math.cross(float2, @float);
			if (math.any(float3))
			{
				Matrix4x4 matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(@float, float3), Vector3.one);
				float num = Vector3.SignedAngle(@float, float2, float3) * 0.017453292f;
				this.PushMatrix(matrix);
				this.SolidCircleXZInternal(float3.zero, math.length(@float), 1.5707964f, 1.5707964f - num);
				this.PopMatrix();
			}
			this.PopColor();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004F3F File Offset: 0x0000313F
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public void SolidCircleXZ(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.SolidCircleXZInternal(center, radius, startAngle, endAngle, color);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004F4E File Offset: 0x0000314E
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public void SolidCircleXZ(float3 center, float radius, Color color)
		{
			this.SolidCircleXZ(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004F64 File Offset: 0x00003164
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public void SolidCircleXY(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
			this.SolidCircleXZInternal(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004FB2 File Offset: 0x000031B2
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public void SolidCircleXY(float3 center, float radius, Color color)
		{
			this.SolidCircleXY(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004FC8 File Offset: 0x000031C8
		public void SolidCircle(float3 center, float3 normal, float radius, Color color)
		{
			this.Reserve<Color32, CommandBuilder.CircleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.CircleData>(new CommandBuilder.CircleData
			{
				center = center,
				normal = normal,
				radius = radius
			});
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000501C File Offset: 0x0000321C
		public void SphereOutline(float3 center, float radius, Color color)
		{
			this.Reserve<Color32, CommandBuilder.SphereData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.SphereData>(new CommandBuilder.SphereData
			{
				center = center,
				radius = radius
			});
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005065 File Offset: 0x00003265
		public void WireCylinder(float3 bottom, float3 top, float radius, Color color)
		{
			this.WireCylinder(bottom, top - bottom, math.length(top - bottom), radius, color);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005084 File Offset: 0x00003284
		public void WireCylinder(float3 position, float3 up, float height, float radius, Color color)
		{
			up = math.normalizesafe(up, default(float3));
			if (math.all(up == 0f) || math.any(math.isnan(up)) || math.isnan(height) || math.isnan(radius))
			{
				return;
			}
			this.PushColor(color);
			float3 lhs;
			float3 lhs2;
			CommandBuilder.OrthonormalBasis(up, out lhs, out lhs2);
			this.PushMatrix(new float4x4(new float4(lhs * radius, 0f), new float4(up * height, 0f), new float4(lhs2 * radius, 0f), new float4(position, 1f)));
			this.CircleXZInternal(float3.zero, 1f, 0f, 6.2831855f);
			if (height > 0f)
			{
				this.CircleXZInternal(new float3(0f, 1f, 0f), 1f, 0f, 6.2831855f);
				this.Line(new float3(1f, 0f, 0f), new float3(1f, 1f, 0f));
				this.Line(new float3(-1f, 0f, 0f), new float3(-1f, 1f, 0f));
				this.Line(new float3(0f, 0f, 1f), new float3(0f, 1f, 1f));
				this.Line(new float3(0f, 0f, -1f), new float3(0f, 1f, -1f));
			}
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005244 File Offset: 0x00003444
		public void WireCapsule(float3 start, float3 end, float radius, Color color)
		{
			this.PushColor(color);
			float3 @float = end - start;
			float num = math.length(@float);
			if ((double)num < 0.0001)
			{
				this.WireSphere(start, radius);
			}
			else
			{
				float3 float2 = @float / num;
				this.WireCapsule(start - float2 * radius, float2, num + 2f * radius, radius);
			}
			this.PopColor();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000052AC File Offset: 0x000034AC
		public void WireCapsule(float3 position, float3 direction, float length, float radius, Color color)
		{
			direction = math.normalizesafe(direction, default(float3));
			if (math.all(direction == 0f) || math.any(math.isnan(direction)) || math.isnan(length) || math.isnan(radius))
			{
				return;
			}
			this.PushColor(color);
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
			}
			else
			{
				length = math.max(length, radius * 2f);
				float3 xyz;
				float3 xyz2;
				CommandBuilder.OrthonormalBasis(direction, out xyz, out xyz2);
				this.PushMatrix(new float4x4(new float4(xyz, 0f), new float4(direction, 0f), new float4(xyz2, 0f), new float4(position, 1f)));
				this.CircleXZInternal(new float3(0f, radius, 0f), radius, 0f, 6.2831855f);
				this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
				this.CircleXZInternal(new float3(0f, 0f, radius), radius, 3.1415927f, 6.2831855f);
				this.PopMatrix();
				this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
				this.CircleXZInternal(new float3(radius, 0f, 0f), radius, 1.5707964f, 4.712389f);
				this.PopMatrix();
				if (length > 0f)
				{
					float num = length - radius;
					this.CircleXZInternal(new float3(0f, num, 0f), radius, 0f, 6.2831855f);
					this.PushMatrix(CommandBuilder.XZtoXYPlaneMatrix);
					this.CircleXZInternal(new float3(0f, 0f, num), radius, 0f, 3.1415927f);
					this.PopMatrix();
					this.PushMatrix(CommandBuilder.XZtoYZPlaneMatrix);
					this.CircleXZInternal(new float3(num, 0f, 0f), radius, -1.5707964f, 1.5707964f);
					this.PopMatrix();
					this.Line(new float3(radius, radius, 0f), new float3(radius, num, 0f));
					this.Line(new float3(-radius, radius, 0f), new float3(-radius, num, 0f));
					this.Line(new float3(0f, radius, radius), new float3(0f, num, radius));
					this.Line(new float3(0f, radius, -radius), new float3(0f, num, -radius));
				}
				this.PopMatrix();
			}
			this.PopColor();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005534 File Offset: 0x00003734
		public void WireSphere(float3 position, float radius, Color color)
		{
			this.PushColor(color);
			this.SphereOutline(position, radius);
			this.Circle(position, new float3(1f, 0f, 0f), radius);
			this.Circle(position, new float3(0f, 1f, 0f), radius);
			this.Circle(position, new float3(0f, 0f, 1f), radius);
			this.PopColor();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000055AC File Offset: 0x000037AC
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005616 File Offset: 0x00003816
		[BurstDiscard]
		public void Polyline(List<Vector3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005624 File Offset: 0x00003824
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005685 File Offset: 0x00003885
		[BurstDiscard]
		public void Polyline(Vector3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005690 File Offset: 0x00003890
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000056F1 File Offset: 0x000038F1
		[BurstDiscard]
		public void Polyline(float3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000056FC File Offset: 0x000038FC
		public void Polyline(NativeArray<float3> points, bool cycle, Color color)
		{
			this.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.PopColor();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000576D File Offset: 0x0000396D
		public void Polyline(NativeArray<float3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005778 File Offset: 0x00003978
		public void DashedLine(float3 a, float3 b, float dash, float gap, Color color)
		{
			this.PushColor(color);
			CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.None, gap, 0f, dash + gap, false);
			polylineWithSymbol.MoveTo(ref this, a);
			polylineWithSymbol.MoveTo(ref this, b);
			this.PopColor();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000057BC File Offset: 0x000039BC
		public void DashedPolyline(List<Vector3> points, float dash, float gap, Color color)
		{
			this.PushColor(color);
			CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.None, gap, 0f, dash + gap, false);
			for (int i = 0; i < points.Count; i++)
			{
				polylineWithSymbol.MoveTo(ref this, points[i]);
			}
			this.PopColor();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005810 File Offset: 0x00003A10
		public void WireBox(float3 center, float3 size, Color color)
		{
			this.Reserve<Color32, CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PopMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000585C File Offset: 0x00003A5C
		public void WireBox(float3 center, quaternion rotation, float3 size, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.WireBox(float3.zero, new float3(1f, 1f, 1f));
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000058AA File Offset: 0x00003AAA
		public void WireBox(Bounds bounds, Color color)
		{
			this.WireBox(bounds.center, bounds.size, color);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000058CC File Offset: 0x00003ACC
		public void WireMesh(Mesh mesh, Color color)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException();
			}
			this.PushColor(color);
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
			Mesh.MeshData meshData = meshDataArray[0];
			CommandBuilder.JobWireMesh.JobWireMeshFunctionPointer(ref meshData, ref this);
			meshDataArray.Dispose();
			this.PopColor();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005919 File Offset: 0x00003B19
		public unsafe void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles, Color color)
		{
			this.PushColor(color);
			CommandBuilder.JobWireMesh.WireMesh((float3*)vertices.GetUnsafeReadOnlyPtr<float3>(), (int*)triangles.GetUnsafeReadOnlyPtr<int>(), vertices.Length, triangles.Length, ref this);
			this.PopColor();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005948 File Offset: 0x00003B48
		public void SolidMesh(Mesh mesh, Color color)
		{
			this.SolidMeshInternal(mesh, false, color);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005954 File Offset: 0x00003B54
		public void Cross(float3 position, float size, Color color)
		{
			this.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
			this.PopColor();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005A0D File Offset: 0x00003C0D
		public void Cross(float3 position, Color color)
		{
			this.Cross(position, 1f, color);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005A1C File Offset: 0x00003C1C
		[Obsolete("Use Draw.xz.Cross instead")]
		public void CrossXZ(float3 position, float size, Color color)
		{
			this.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, 0f, size), position + new float3(0f, 0f, size));
			this.PopColor();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005AA3 File Offset: 0x00003CA3
		[Obsolete("Use Draw.xz.Cross instead")]
		public void CrossXZ(float3 position, Color color)
		{
			this.CrossXZ(position, 1f, color);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005AB4 File Offset: 0x00003CB4
		[Obsolete("Use Draw.xy.Cross instead")]
		public void CrossXY(float3 position, float size, Color color)
		{
			this.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float3(size, 0f, 0f), position + new float3(size, 0f, 0f));
			this.Line(position - new float3(0f, size, 0f), position + new float3(0f, size, 0f));
			this.PopColor();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005B3B File Offset: 0x00003D3B
		[Obsolete("Use Draw.xy.Cross instead")]
		public void CrossXY(float3 position, Color color)
		{
			this.CrossXY(position, 1f, color);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005B4C File Offset: 0x00003D4C
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
			this.PushColor(color);
			float3 a = p0;
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				float3 @float = CommandBuilder.EvaluateCubicBezier(p0, p1, p2, p3, t);
				this.Line(a, @float);
				a = @float;
			}
			this.PopColor();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005B98 File Offset: 0x00003D98
		public void CatmullRom(List<Vector3> points, Color color)
		{
			if (points.Count < 2)
			{
				return;
			}
			this.PushColor(color);
			if (points.Count == 2)
			{
				this.Line(points[0], points[1]);
			}
			else
			{
				int count = points.Count;
				this.CatmullRom(points[0], points[0], points[1], points[2]);
				int num = 0;
				while (num + 3 < count)
				{
					this.CatmullRom(points[num], points[num + 1], points[num + 2], points[num + 3]);
					num++;
				}
				this.CatmullRom(points[count - 3], points[count - 2], points[count - 1], points[count - 1]);
			}
			this.PopColor();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
			this.PushColor(color);
			float3 p4 = (-p0 + 6f * p1 + 1f * p2) * 0.16666667f;
			float3 p5 = (p1 + 6f * p2 - p3) * 0.16666667f;
			this.Bezier(p1, p4, p5, p2);
			this.PopColor();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005D21 File Offset: 0x00003F21
		public void Arrow(float3 from, float3 to, Color color)
		{
			this.ArrowRelativeSizeHead(from, to, CommandBuilder.DEFAULT_UP, 0.2f, color);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005D38 File Offset: 0x00003F38
		public void Arrow(float3 from, float3 to, float3 up, float headSize, Color color)
		{
			this.PushColor(color);
			float num = math.lengthsq(to - from);
			if (num > 1E-06f)
			{
				this.ArrowRelativeSizeHead(from, to, up, headSize * math.rsqrt(num));
			}
			this.PopColor();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005D7C File Offset: 0x00003F7C
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction, Color color)
		{
			this.PushColor(color);
			this.Line(from, to);
			float3 @float = to - from;
			float3 float2 = math.cross(@float, up);
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(1f, 0f, 0f), @float);
			}
			if (math.all(float2 == 0f))
			{
				float2 = math.cross(new float3(0f, 1f, 0f), @float);
			}
			float2 = math.normalizesafe(float2, default(float3)) * math.length(@float);
			this.Line(to, to - (@float + float2) * headFraction);
			this.Line(to, to - (@float - float2) * headFraction);
			this.PopColor();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005E5A File Offset: 0x0000405A
		public void Arrowhead(float3 center, float3 direction, float radius, Color color)
		{
			this.Arrowhead(center, direction, CommandBuilder.DEFAULT_UP, radius, color);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005E6C File Offset: 0x0000406C
		public void Arrowhead(float3 center, float3 direction, float3 up, float radius, Color color)
		{
			if (math.all(direction == 0f))
			{
				return;
			}
			this.PushColor(color);
			direction = math.normalizesafe(direction, default(float3));
			float3 rhs = math.cross(direction, up);
			float3 @float = center - radius * 0.5f * 0.5f * direction;
			float3 float2 = @float + radius * direction;
			float3 float3 = @float - radius * 0.5f * direction + radius * 0.866025f * rhs;
			float3 float4 = @float - radius * 0.5f * direction - radius * 0.866025f * rhs;
			this.Line(float2, float3);
			this.Line(float3, @float);
			this.Line(@float, float4);
			this.Line(float4, float2);
			this.PopColor();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005F54 File Offset: 0x00004154
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width, Color color)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			this.PushColor(color);
			Quaternion q = Quaternion.LookRotation(direction, CommandBuilder.DEFAULT_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, q, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.CircleXZInternal(float3.zero, offset, num, num2);
			float3 a = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 b = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 @float = new float3(0f, 0f, 1.4142f * offset);
			this.Line(a, @float);
			this.Line(@float, b);
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006059 File Offset: 0x00004259
		public void ArrowheadArc(float3 origin, float3 direction, float offset, Color color)
		{
			this.ArrowheadArc(origin, direction, offset, 60f, color);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000606C File Offset: 0x0000426C
		public void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize, Color color)
		{
			this.PushColor(color);
			cells = math.max(cells, new int2(1, 1));
			this.PushMatrix(float4x4.TRS(center, rotation, new Vector3(totalSize.x, 0f, totalSize.y)));
			int x = cells.x;
			int y = cells.y;
			for (int i = 0; i <= x; i++)
			{
				this.Line(new float3((float)i / (float)x - 0.5f, 0f, -0.5f), new float3((float)i / (float)x - 0.5f, 0f, 0.5f));
			}
			for (int j = 0; j <= y; j++)
			{
				this.Line(new float3(-0.5f, 0f, (float)j / (float)y - 0.5f), new float3(0.5f, 0f, (float)j / (float)y - 0.5f));
			}
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006160 File Offset: 0x00004360
		public void WireTriangle(float3 a, float3 b, float3 c, Color color)
		{
			this.PushColor(color);
			this.Line(a, b);
			this.Line(b, c);
			this.Line(c, a);
			this.PopColor();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006188 File Offset: 0x00004388
		[Obsolete("Use Draw.xz.WireRectangle instead")]
		public void WireRectangleXZ(float3 center, float2 size, Color color)
		{
			this.WireRectangle(center, quaternion.identity, size, color);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00006198 File Offset: 0x00004398
		public void WireRectangle(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.WirePlane(center, rotation, size, color);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000061A8 File Offset: 0x000043A8
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public void WireRectangle(Rect rect, Color color)
		{
			this.xy.WireRectangle(rect, color);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000061C5 File Offset: 0x000043C5
		public void WireTriangle(float3 center, quaternion rotation, float radius, Color color)
		{
			this.WirePolygon(center, 3, rotation, radius, color);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000061D3 File Offset: 0x000043D3
		public void WirePentagon(float3 center, quaternion rotation, float radius, Color color)
		{
			this.WirePolygon(center, 5, rotation, radius, color);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000061E1 File Offset: 0x000043E1
		public void WireHexagon(float3 center, quaternion rotation, float radius, Color color)
		{
			this.WirePolygon(center, 6, rotation, radius, color);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000061F0 File Offset: 0x000043F0
		public void WirePolygon(float3 center, int vertices, quaternion rotation, float radius, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(float4x4.TRS(center, rotation, new float3(radius, radius, radius)));
			float3 a = new float3(0f, 0f, 1f);
			for (int i = 1; i <= vertices; i++)
			{
				float x = 6.2831855f * ((float)i / (float)vertices);
				float3 @float = new float3(math.sin(x), 0f, math.cos(x));
				this.Line(a, @float);
				a = @float;
			}
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000627C File Offset: 0x0000447C
		[Obsolete("Use Draw.xy.SolidRectangle instead")]
		public void SolidRectangle(Rect rect, Color color)
		{
			this.xy.SolidRectangle(rect, color);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006299 File Offset: 0x00004499
		public void SolidPlane(float3 center, float3 normal, float2 size, Color color)
		{
			this.PushColor(color);
			if (math.any(normal))
			{
				this.SolidPlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
			this.PopColor();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000062D4 File Offset: 0x000044D4
		public void SolidPlane(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.PushMatrix(float4x4.TRS(center, rotation, new float3(size.x, 0f, size.y)));
			this.Reserve<Color32, CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = 0,
				size = 1
			});
			this.PopMatrix();
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006351 File Offset: 0x00004551
		public void WirePlane(float3 center, float3 normal, float2 size, Color color)
		{
			this.PushColor(color);
			if (math.any(normal))
			{
				this.WirePlane(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
			this.PopColor();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000638C File Offset: 0x0000458C
		public void WirePlane(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.Reserve<Color32, CommandBuilder.PlaneData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.PlaneData>(new CommandBuilder.PlaneData
			{
				center = center,
				rotation = rotation,
				size = size
			});
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000063DE File Offset: 0x000045DE
		public void PlaneWithNormal(float3 center, float3 normal, float2 size, Color color)
		{
			this.PushColor(color);
			if (math.any(normal))
			{
				this.PlaneWithNormal(center, Quaternion.LookRotation(CommandBuilder.calculateTangent(normal), normal), size);
			}
			this.PopColor();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000641C File Offset: 0x0000461C
		public void PlaneWithNormal(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.PushColor(color);
			this.SolidPlane(center, rotation, size);
			this.WirePlane(center, rotation, size);
			this.ArrowRelativeSizeHead(center, center + math.mul(rotation, new float3(0f, 1f, 0f)) * 0.5f, math.mul(rotation, new float3(0f, 0f, 1f)), 0.2f);
			this.PopColor();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000649C File Offset: 0x0000469C
		public void SolidTriangle(float3 a, float3 b, float3 c, Color color)
		{
			this.Reserve<Color32, CommandBuilder.TriangleData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.PopMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.TriangleData>(new CommandBuilder.TriangleData
			{
				a = a,
				b = b,
				c = c
			});
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000064F0 File Offset: 0x000046F0
		public void SolidBox(float3 center, float3 size, Color color)
		{
			this.Reserve<Color32, CommandBuilder.BoxData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.Disc);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.BoxData>(new CommandBuilder.BoxData
			{
				center = center,
				size = size
			});
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006539 File Offset: 0x00004739
		public void SolidBox(Bounds bounds, Color color)
		{
			this.SolidBox(bounds.center, bounds.size, color);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000655A File Offset: 0x0000475A
		public void SolidBox(float3 center, quaternion rotation, float3 size, Color color)
		{
			this.PushColor(color);
			this.PushMatrix(float4x4.TRS(center, rotation, size));
			this.SolidBox(float3.zero, Vector3.one);
			this.PopMatrix();
			this.PopColor();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006593 File Offset: 0x00004793
		public void Label3D(float3 position, quaternion rotation, string text, float size, Color color)
		{
			this.Label3D(position, rotation, text, size, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000065A8 File Offset: 0x000047A8
		public void Label3D(float3 position, quaternion rotation, string text, float size, LabelAlignment alignment, Color color)
		{
			this.AssertBufferExists();
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			this.Reserve<Color32, CommandBuilder.TextData3D>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PushMatrix | CommandBuilder.Command.PopPersist);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.TextData3D>(new CommandBuilder.TextData3D
			{
				center = position,
				rotation = rotation,
				numCharacters = text.Length,
				size = size,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * text.Length);
			foreach (char c in text)
			{
				ushort value = (ushort)drawingData.fontData.GetIndex(c);
				this.Add<ushort>(value);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000666A File Offset: 0x0000486A
		public void Label2D(float3 position, string text, float sizeInPixels, Color color)
		{
			this.Label2D(position, text, sizeInPixels, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000667C File Offset: 0x0000487C
		public void Label2D(float3 position, string text, Color color)
		{
			this.Label2D(position, text, 14f, color);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000668C File Offset: 0x0000488C
		public void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.AssertBufferExists();
			DrawingData drawingData = this.gizmos.Target as DrawingData;
			this.Reserve<Color32, CommandBuilder.TextData>();
			this.Add<CommandBuilder.Command>(CommandBuilder.Command.PushColorInline | CommandBuilder.Command.PopColor | CommandBuilder.Command.PopPersist);
			this.Add<uint>(CommandBuilder.ConvertColor(color));
			this.Add<CommandBuilder.TextData>(new CommandBuilder.TextData
			{
				center = position,
				numCharacters = text.Length,
				sizeInPixels = sizeInPixels,
				alignment = alignment
			});
			this.Reserve(UnsafeUtility.SizeOf<ushort>() * text.Length);
			foreach (char c in text)
			{
				ushort value = (ushort)drawingData.fontData.GetIndex(c);
				this.Add<ushort>(value);
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006745 File Offset: 0x00004945
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006757 File Offset: 0x00004957
		public void Label2D(float3 position, ref FixedString32Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006767 File Offset: 0x00004967
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006779 File Offset: 0x00004979
		public void Label2D(float3 position, ref FixedString64Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006789 File Offset: 0x00004989
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000679B File Offset: 0x0000499B
		public void Label2D(float3 position, ref FixedString128Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000067AB File Offset: 0x000049AB
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(position, ref text, sizeInPixels, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000067BD File Offset: 0x000049BD
		public void Label2D(float3 position, ref FixedString512Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000067CD File Offset: 0x000049CD
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
			this.PopColor();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000067F3 File Offset: 0x000049F3
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
			this.PopColor();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006819 File Offset: 0x00004A19
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
			this.PopColor();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000683F File Offset: 0x00004A3F
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label2D(position, text.GetUnsafePtr(), text.Length, sizeInPixels, alignment);
			this.PopColor();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006865 File Offset: 0x00004A65
		public void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, Color color)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006879 File Offset: 0x00004A79
		public void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, Color color)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000688D File Offset: 0x00004A8D
		public void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, Color color)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000068A1 File Offset: 0x00004AA1
		public void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, Color color)
		{
			this.Label3D(position, rotation, ref text, size, LabelAlignment.MiddleLeft, color);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000068B5 File Offset: 0x00004AB5
		public void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
			this.PopColor();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000068DD File Offset: 0x00004ADD
		public void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
			this.PopColor();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006905 File Offset: 0x00004B05
		public void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
			this.PopColor();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000692D File Offset: 0x00004B2D
		public void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, LabelAlignment alignment, Color color)
		{
			this.PushColor(color);
			this.Label3D(position, rotation, text.GetUnsafePtr(), text.Length, size, alignment);
			this.PopColor();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000698E File Offset: 0x00004B8E
		public static void Initialize$JobWireMesh_WireMesh_00000105$BurstDirectCall()
		{
			CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.Initialize();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006995 File Offset: 0x00004B95
		public static void Initialize$JobWireMesh_Execute_00000106$BurstDirectCall()
		{
			CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.Initialize();
		}

		// Token: 0x0400001D RID: 29
		[NativeDisableUnsafePtrRestriction]
		internal unsafe UnsafeAppendBuffer* buffer;

		// Token: 0x0400001E RID: 30
		private GCHandle gizmos;

		// Token: 0x0400001F RID: 31
		[NativeSetThreadIndex]
		private int threadIndex;

		// Token: 0x04000020 RID: 32
		private DrawingData.BuilderData.BitPackedMeta uniqueID;

		// Token: 0x04000021 RID: 33
		private static readonly float3 DEFAULT_UP = new float3(0f, 1f, 0f);

		// Token: 0x04000022 RID: 34
		internal static readonly float4x4 XZtoXYPlaneMatrix = float4x4.RotateX(-1.5707964f);

		// Token: 0x04000023 RID: 35
		internal static readonly float4x4 XZtoYZPlaneMatrix = float4x4.RotateZ(1.5707964f);

		// Token: 0x0200000C RID: 12
		[Flags]
		internal enum Command
		{
			// Token: 0x04000025 RID: 37
			PushColorInline = 256,
			// Token: 0x04000026 RID: 38
			PushColor = 0,
			// Token: 0x04000027 RID: 39
			PopColor = 1,
			// Token: 0x04000028 RID: 40
			PushMatrix = 2,
			// Token: 0x04000029 RID: 41
			PushSetMatrix = 3,
			// Token: 0x0400002A RID: 42
			PopMatrix = 4,
			// Token: 0x0400002B RID: 43
			Line = 5,
			// Token: 0x0400002C RID: 44
			Circle = 6,
			// Token: 0x0400002D RID: 45
			CircleXZ = 7,
			// Token: 0x0400002E RID: 46
			Disc = 8,
			// Token: 0x0400002F RID: 47
			DiscXZ = 9,
			// Token: 0x04000030 RID: 48
			SphereOutline = 10,
			// Token: 0x04000031 RID: 49
			Box = 11,
			// Token: 0x04000032 RID: 50
			WirePlane = 12,
			// Token: 0x04000033 RID: 51
			WireBox = 13,
			// Token: 0x04000034 RID: 52
			SolidTriangle = 14,
			// Token: 0x04000035 RID: 53
			PushPersist = 15,
			// Token: 0x04000036 RID: 54
			PopPersist = 16,
			// Token: 0x04000037 RID: 55
			Text = 17,
			// Token: 0x04000038 RID: 56
			Text3D = 18,
			// Token: 0x04000039 RID: 57
			PushLineWidth = 19,
			// Token: 0x0400003A RID: 58
			PopLineWidth = 20,
			// Token: 0x0400003B RID: 59
			CaptureState = 21
		}

		// Token: 0x0200000D RID: 13
		internal struct TriangleData
		{
			// Token: 0x0400003C RID: 60
			public float3 a;

			// Token: 0x0400003D RID: 61
			public float3 b;

			// Token: 0x0400003E RID: 62
			public float3 c;
		}

		// Token: 0x0200000E RID: 14
		internal struct LineData
		{
			// Token: 0x0400003F RID: 63
			public float3 a;

			// Token: 0x04000040 RID: 64
			public float3 b;
		}

		// Token: 0x0200000F RID: 15
		internal struct LineDataV3
		{
			// Token: 0x04000041 RID: 65
			public Vector3 a;

			// Token: 0x04000042 RID: 66
			public Vector3 b;
		}

		// Token: 0x02000010 RID: 16
		internal struct CircleXZData
		{
			// Token: 0x04000043 RID: 67
			public float3 center;

			// Token: 0x04000044 RID: 68
			public float radius;

			// Token: 0x04000045 RID: 69
			public float startAngle;

			// Token: 0x04000046 RID: 70
			public float endAngle;
		}

		// Token: 0x02000011 RID: 17
		internal struct CircleData
		{
			// Token: 0x04000047 RID: 71
			public float3 center;

			// Token: 0x04000048 RID: 72
			public float3 normal;

			// Token: 0x04000049 RID: 73
			public float radius;
		}

		// Token: 0x02000012 RID: 18
		internal struct SphereData
		{
			// Token: 0x0400004A RID: 74
			public float3 center;

			// Token: 0x0400004B RID: 75
			public float radius;
		}

		// Token: 0x02000013 RID: 19
		internal struct BoxData
		{
			// Token: 0x0400004C RID: 76
			public float3 center;

			// Token: 0x0400004D RID: 77
			public float3 size;
		}

		// Token: 0x02000014 RID: 20
		internal struct PlaneData
		{
			// Token: 0x0400004E RID: 78
			public float3 center;

			// Token: 0x0400004F RID: 79
			public quaternion rotation;

			// Token: 0x04000050 RID: 80
			public float2 size;
		}

		// Token: 0x02000015 RID: 21
		internal struct PersistData
		{
			// Token: 0x04000051 RID: 81
			public float endTime;
		}

		// Token: 0x02000016 RID: 22
		internal struct LineWidthData
		{
			// Token: 0x04000052 RID: 82
			public float pixels;

			// Token: 0x04000053 RID: 83
			public bool automaticJoins;
		}

		// Token: 0x02000017 RID: 23
		internal struct TextData
		{
			// Token: 0x04000054 RID: 84
			public float3 center;

			// Token: 0x04000055 RID: 85
			public LabelAlignment alignment;

			// Token: 0x04000056 RID: 86
			public float sizeInPixels;

			// Token: 0x04000057 RID: 87
			public int numCharacters;
		}

		// Token: 0x02000018 RID: 24
		internal struct TextData3D
		{
			// Token: 0x04000058 RID: 88
			public float3 center;

			// Token: 0x04000059 RID: 89
			public quaternion rotation;

			// Token: 0x0400005A RID: 90
			public LabelAlignment alignment;

			// Token: 0x0400005B RID: 91
			public float size;

			// Token: 0x0400005C RID: 92
			public int numCharacters;
		}

		// Token: 0x02000019 RID: 25
		public struct ScopeMatrix : IDisposable
		{
			// Token: 0x06000100 RID: 256 RVA: 0x0000699C File Offset: 0x00004B9C
			public void Dispose()
			{
				this.builder.PopMatrix();
				this.builder.buffer = null;
			}

			// Token: 0x0400005D RID: 93
			internal CommandBuilder builder;
		}

		// Token: 0x0200001A RID: 26
		public struct ScopeColor : IDisposable
		{
			// Token: 0x06000101 RID: 257 RVA: 0x000069B6 File Offset: 0x00004BB6
			public void Dispose()
			{
				this.builder.PopColor();
				this.builder.buffer = null;
			}

			// Token: 0x0400005E RID: 94
			internal CommandBuilder builder;
		}

		// Token: 0x0200001B RID: 27
		public struct ScopePersist : IDisposable
		{
			// Token: 0x06000102 RID: 258 RVA: 0x000069D0 File Offset: 0x00004BD0
			public void Dispose()
			{
				this.builder.PopDuration();
				this.builder.buffer = null;
			}

			// Token: 0x0400005F RID: 95
			internal CommandBuilder builder;
		}

		// Token: 0x0200001C RID: 28
		public struct ScopeEmpty : IDisposable
		{
			// Token: 0x06000103 RID: 259 RVA: 0x00002104 File Offset: 0x00000304
			public void Dispose()
			{
			}
		}

		// Token: 0x0200001D RID: 29
		public struct ScopeLineWidth : IDisposable
		{
			// Token: 0x06000104 RID: 260 RVA: 0x000069EA File Offset: 0x00004BEA
			public void Dispose()
			{
				this.builder.PopLineWidth();
				this.builder.buffer = null;
			}

			// Token: 0x04000060 RID: 96
			internal CommandBuilder builder;
		}

		// Token: 0x0200001E RID: 30
		public enum SymbolDecoration
		{
			// Token: 0x04000062 RID: 98
			None,
			// Token: 0x04000063 RID: 99
			ArrowHead,
			// Token: 0x04000064 RID: 100
			Circle
		}

		// Token: 0x0200001F RID: 31
		public struct PolylineWithSymbol
		{
			// Token: 0x06000105 RID: 261 RVA: 0x00006A04 File Offset: 0x00004C04
			public PolylineWithSymbol(CommandBuilder.SymbolDecoration symbol, float symbolSize, float symbolPadding, float symbolSpacing, bool reverseSymbols = false)
			{
				if (symbolSpacing <= 1.1754944E-38f)
				{
					throw new ArgumentOutOfRangeException("symbolSpacing", "Symbol spacing must be greater than zero");
				}
				if (symbolSize <= 1.1754944E-38f)
				{
					throw new ArgumentOutOfRangeException("symbolSize", "Symbol size must be greater than zero");
				}
				if (symbolPadding < 0f)
				{
					throw new ArgumentOutOfRangeException("symbolPadding", "Symbol padding must non-negative");
				}
				this.prev = float3.zero;
				this.symbol = symbol;
				this.symbolSize = symbolSize;
				this.symbolPadding = symbolPadding;
				this.symbolSpacing = math.max(0f, symbolSpacing - symbolPadding * 2f - symbolSize);
				this.reverseSymbols = reverseSymbols;
				this.symbolOffset = ((symbol == CommandBuilder.SymbolDecoration.ArrowHead) ? (-0.25f * symbolSize) : 0f);
				if (reverseSymbols)
				{
					this.symbolOffset = -this.symbolOffset;
				}
				this.symbolOffset += 0.5f * symbolSize;
				this.offset = -1f;
				this.odd = false;
			}

			// Token: 0x06000106 RID: 262 RVA: 0x00006AF0 File Offset: 0x00004CF0
			public void MoveTo(ref CommandBuilder draw, float3 next)
			{
				if (this.offset == -1f)
				{
					this.offset = this.symbolSpacing * 0.5f;
					this.prev = next;
					return;
				}
				float num = math.length(next - this.prev);
				float num2 = math.rcp(num);
				float3 @float = next - this.prev;
				float3 float2 = default(float3);
				if (this.symbol != CommandBuilder.SymbolDecoration.None)
				{
					float2 = math.normalizesafe(math.cross(@float, math.cross(@float, new float3(0f, 1f, 0f))), default(float3));
					if (math.all(float2 == 0f))
					{
						float2 = new float3(0f, 0f, 1f);
					}
				}
				if (this.reverseSymbols)
				{
					@float = -@float;
				}
				if (this.offset > 0f && !this.odd)
				{
					draw.Line(this.prev, math.lerp(this.prev, next, math.min(this.offset * num2, 1f)));
				}
				while (this.offset < num)
				{
					if (!this.odd)
					{
						float3 center = math.lerp(this.prev, next, (this.offset + this.symbolOffset) * num2);
						switch (this.symbol)
						{
						case CommandBuilder.SymbolDecoration.None:
							break;
						case CommandBuilder.SymbolDecoration.ArrowHead:
							draw.Arrowhead(center, @float, float2, this.symbolSize);
							break;
						case CommandBuilder.SymbolDecoration.Circle:
							goto IL_1CA;
						default:
							goto IL_1CA;
						}
						IL_1DF:
						this.offset += this.symbolSize + this.symbolPadding;
						goto IL_1F9;
						IL_1CA:
						draw.Circle(center, float2, this.symbolSize * 0.5f);
						goto IL_1DF;
					}
					float3 a = math.lerp(this.prev, next, this.offset * num2);
					this.offset += this.symbolSpacing;
					float3 b = math.lerp(this.prev, next, math.min(this.offset * num2, 1f));
					draw.Line(a, b);
					this.offset += this.symbolPadding;
					IL_1F9:
					this.odd = !this.odd;
				}
				this.offset -= num;
				this.prev = next;
			}

			// Token: 0x04000065 RID: 101
			private float3 prev;

			// Token: 0x04000066 RID: 102
			private float offset;

			// Token: 0x04000067 RID: 103
			private readonly float symbolSize;

			// Token: 0x04000068 RID: 104
			private readonly float symbolSpacing;

			// Token: 0x04000069 RID: 105
			private readonly float symbolPadding;

			// Token: 0x0400006A RID: 106
			private readonly float symbolOffset;

			// Token: 0x0400006B RID: 107
			private readonly CommandBuilder.SymbolDecoration symbol;

			// Token: 0x0400006C RID: 108
			private readonly bool reverseSymbols;

			// Token: 0x0400006D RID: 109
			private bool odd;
		}

		// Token: 0x02000020 RID: 32
		[BurstCompile]
		private class JobWireMesh
		{
			// Token: 0x06000107 RID: 263 RVA: 0x00006D26 File Offset: 0x00004F26
			[BurstCompile]
			public unsafe static void WireMesh(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw)
			{
				CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.Invoke(verts, indices, vertexCount, indexCount, ref draw);
			}

			// Token: 0x06000108 RID: 264 RVA: 0x00006D33 File Offset: 0x00004F33
			[BurstCompile]
			[MonoPInvokeCallback(typeof(CommandBuilder.JobWireMesh.JobWireMeshDelegate))]
			private static void Execute(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw)
			{
				CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.Invoke(ref rawMeshData, ref draw);
			}

			// Token: 0x0600010B RID: 267 RVA: 0x00006D68 File Offset: 0x00004F68
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void WireMesh$BurstManaged(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw)
			{
				NativeHashMap<int2, bool> nativeHashMap = new NativeHashMap<int2, bool>(indexCount, Allocator.Temp);
				for (int i = 0; i < indexCount; i += 3)
				{
					int num = indices[i];
					int num2 = indices[i + 1];
					int num3 = indices[i + 2];
					if (num < 0 || num2 < 0 || num3 < 0 || num >= vertexCount || num2 >= vertexCount || num3 >= vertexCount)
					{
						throw new Exception("Invalid vertex index. Index out of bounds");
					}
					int num4 = math.min(num, num2);
					int num5 = math.max(num, num2);
					if (!nativeHashMap.ContainsKey(new int2(num4, num5)))
					{
						nativeHashMap.Add(new int2(num4, num5), true);
						draw.Line(verts[num4], verts[num5]);
					}
					num4 = math.min(num2, num3);
					num5 = math.max(num2, num3);
					if (!nativeHashMap.ContainsKey(new int2(num4, num5)))
					{
						nativeHashMap.Add(new int2(num4, num5), true);
						draw.Line(verts[num4], verts[num5]);
					}
					num4 = math.min(num3, num);
					num5 = math.max(num3, num);
					if (!nativeHashMap.ContainsKey(new int2(num4, num5)))
					{
						nativeHashMap.Add(new int2(num4, num5), true);
						draw.Line(verts[num4], verts[num5]);
					}
				}
			}

			// Token: 0x0600010C RID: 268 RVA: 0x00006EF8 File Offset: 0x000050F8
			[BurstCompile]
			[MonoPInvokeCallback(typeof(CommandBuilder.JobWireMesh.JobWireMeshDelegate))]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void Execute$BurstManaged(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw)
			{
				int num = 0;
				for (int i = 0; i < rawMeshData.subMeshCount; i++)
				{
					num = math.max(num, rawMeshData.GetSubMesh(i).indexCount);
				}
				NativeArray<int> nativeArray = new NativeArray<int>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				NativeArray<Vector3> nativeArray2 = new NativeArray<Vector3>(rawMeshData.vertexCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				rawMeshData.GetVertices(nativeArray2);
				for (int j = 0; j < rawMeshData.subMeshCount; j++)
				{
					SubMeshDescriptor subMesh = rawMeshData.GetSubMesh(j);
					rawMeshData.GetIndices(nativeArray, j, true);
					CommandBuilder.JobWireMesh.WireMesh((float3*)nativeArray2.GetUnsafeReadOnlyPtr<Vector3>(), (int*)nativeArray.GetUnsafeReadOnlyPtr<int>(), nativeArray2.Length, subMesh.indexCount, ref draw);
				}
			}

			// Token: 0x0400006E RID: 110
			public static readonly CommandBuilder.JobWireMesh.JobWireMeshDelegate JobWireMeshFunctionPointer = BurstCompiler.CompileFunctionPointer<CommandBuilder.JobWireMesh.JobWireMeshDelegate>(new CommandBuilder.JobWireMesh.JobWireMeshDelegate(CommandBuilder.JobWireMesh.Execute)).Invoke;

			// Token: 0x02000021 RID: 33
			// (Invoke) Token: 0x0600010E RID: 270
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void JobWireMeshDelegate(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw);

			// Token: 0x02000022 RID: 34
			// (Invoke) Token: 0x06000112 RID: 274
			internal unsafe delegate void WireMesh_00000105$PostfixBurstDelegate(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw);

			// Token: 0x02000023 RID: 35
			internal static class WireMesh_00000105$BurstDirectCall
			{
				// Token: 0x06000115 RID: 277 RVA: 0x00006F98 File Offset: 0x00005198
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.Pointer == 0)
					{
						CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.DeferredCompilation, methodof(CommandBuilder.JobWireMesh.WireMesh$BurstManaged(float3*, int*, int, int, CommandBuilder*)).MethodHandle, typeof(CommandBuilder.JobWireMesh.WireMesh_00000105$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.Pointer;
				}

				// Token: 0x06000116 RID: 278 RVA: 0x00006FC4 File Offset: 0x000051C4
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06000117 RID: 279 RVA: 0x00006FDC File Offset: 0x000051DC
				public unsafe static void Constructor()
				{
					CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(CommandBuilder.JobWireMesh.WireMesh(float3*, int*, int, int, CommandBuilder*)).MethodHandle);
				}

				// Token: 0x06000118 RID: 280 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x06000119 RID: 281 RVA: 0x00006FED File Offset: 0x000051ED
				// Note: this type is marked as 'beforefieldinit'.
				static WireMesh_00000105$BurstDirectCall()
				{
					CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.Constructor();
				}

				// Token: 0x0600011A RID: 282 RVA: 0x00006FF4 File Offset: 0x000051F4
				public unsafe static void Invoke(float3* verts, int* indices, int vertexCount, int indexCount, ref CommandBuilder draw)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = CommandBuilder.JobWireMesh.WireMesh_00000105$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Unity.Mathematics.float3*,System.Int32*,System.Int32,System.Int32,Drawing.CommandBuilder&), verts, indices, vertexCount, indexCount, ref draw, functionPointer);
							return;
						}
					}
					CommandBuilder.JobWireMesh.WireMesh$BurstManaged(verts, indices, vertexCount, indexCount, ref draw);
				}

				// Token: 0x0400006F RID: 111
				private static IntPtr Pointer;

				// Token: 0x04000070 RID: 112
				private static IntPtr DeferredCompilation;
			}

			// Token: 0x02000024 RID: 36
			// (Invoke) Token: 0x0600011C RID: 284
			internal delegate void Execute_00000106$PostfixBurstDelegate(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw);

			// Token: 0x02000025 RID: 37
			internal static class Execute_00000106$BurstDirectCall
			{
				// Token: 0x0600011F RID: 287 RVA: 0x0000702F File Offset: 0x0000522F
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.Pointer == 0)
					{
						CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.DeferredCompilation, methodof(CommandBuilder.JobWireMesh.Execute$BurstManaged(Mesh.MeshData*, CommandBuilder*)).MethodHandle, typeof(CommandBuilder.JobWireMesh.Execute_00000106$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.Pointer;
				}

				// Token: 0x06000120 RID: 288 RVA: 0x0000705C File Offset: 0x0000525C
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x06000121 RID: 289 RVA: 0x00007074 File Offset: 0x00005274
				public unsafe static void Constructor()
				{
					CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(CommandBuilder.JobWireMesh.Execute(Mesh.MeshData*, CommandBuilder*)).MethodHandle);
				}

				// Token: 0x06000122 RID: 290 RVA: 0x00002104 File Offset: 0x00000304
				public static void Initialize()
				{
				}

				// Token: 0x06000123 RID: 291 RVA: 0x00007085 File Offset: 0x00005285
				// Note: this type is marked as 'beforefieldinit'.
				static Execute_00000106$BurstDirectCall()
				{
					CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.Constructor();
				}

				// Token: 0x06000124 RID: 292 RVA: 0x0000708C File Offset: 0x0000528C
				public static void Invoke(ref Mesh.MeshData rawMeshData, ref CommandBuilder draw)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = CommandBuilder.JobWireMesh.Execute_00000106$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(UnityEngine.Mesh/MeshData&,Drawing.CommandBuilder&), ref rawMeshData, ref draw, functionPointer);
							return;
						}
					}
					CommandBuilder.JobWireMesh.Execute$BurstManaged(ref rawMeshData, ref draw);
				}

				// Token: 0x04000071 RID: 113
				private static IntPtr Pointer;

				// Token: 0x04000072 RID: 114
				private static IntPtr DeferredCompilation;
			}
		}
	}
}
