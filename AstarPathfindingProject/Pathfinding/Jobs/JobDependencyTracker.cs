using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x02000184 RID: 388
	public class JobDependencyTracker : IAstarPooledObject
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0003D9A3 File Offset: 0x0003BBA3
		public bool forceLinearDependencies
		{
			get
			{
				if (this.linearDependencies == LinearDependencies.Check)
				{
					this.SetLinearDependencies(false);
				}
				return this.linearDependencies == LinearDependencies.Enabled;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
		public JobHandle AllWritesDependency
		{
			get
			{
				NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(this.slots.Count, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < this.slots.Count; i++)
				{
					jobs[i] = this.slots[i].lastWrite.handle;
				}
				JobHandle result = JobHandle.CombineDependencies(jobs);
				jobs.Dispose();
				return result;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0003DA22 File Offset: 0x0003BC22
		private bool supportsMultithreading
		{
			get
			{
				return JobsUtility.JobWorkerCount > 0;
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003DA2C File Offset: 0x0003BC2C
		public void SetLinearDependencies(bool linearDependencies)
		{
			if (!this.supportsMultithreading)
			{
				linearDependencies = true;
			}
			if (linearDependencies)
			{
				this.AllWritesDependency.Complete();
			}
			this.linearDependencies = (linearDependencies ? LinearDependencies.Enabled : LinearDependencies.Disabled);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003DA64 File Offset: 0x0003BC64
		public NativeArray<T> NewNativeArray<[IsUnmanaged] T>(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = new NativeArray<T>(length, allocator, options);
			this.Track<T>(nativeArray, options == NativeArrayOptions.ClearMemory);
			return nativeArray;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003DA88 File Offset: 0x0003BC88
		public void Track<[IsUnmanaged] T>(NativeArray<T> array, bool initialized = true) where T : struct, ValueType
		{
			this.slots.Add(new JobDependencyTracker.NativeArraySlot
			{
				hash = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(array),
				lastWrite = default(JobDependencyTracker.JobInstance),
				lastReads = ListPool<JobDependencyTracker.JobInstance>.Claim(),
				initialized = initialized
			});
			if (this.arena == null)
			{
				this.arena = new DisposeArena();
			}
			this.arena.Add<T>(array);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003DAF7 File Offset: 0x0003BCF7
		public void Persist<[IsUnmanaged] T>(NativeArray<T> array) where T : struct, ValueType
		{
			if (this.arena == null)
			{
				return;
			}
			this.arena.Remove<T>(array);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003DB10 File Offset: 0x0003BD10
		public JobHandle ScheduleBatch(NativeArray<RaycastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				RaycastCommand.ScheduleBatch(commands, results, minCommandsPerJob, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobRaycastCommandDummy jobRaycastCommandDummy = new JobDependencyTracker.JobRaycastCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobRaycastCommandDummy>.GetDependencies(ref jobRaycastCommandDummy, this);
			JobHandle jobHandle = RaycastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobRaycastCommandDummy>.Scheduled(ref jobRaycastCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003DB80 File Offset: 0x0003BD80
		public JobHandle ScheduleBatch(NativeArray<SpherecastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				SpherecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobSpherecastCommandDummy jobSpherecastCommandDummy = new JobDependencyTracker.JobSpherecastCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobSpherecastCommandDummy>.GetDependencies(ref jobSpherecastCommandDummy, this);
			JobHandle jobHandle = SpherecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobSpherecastCommandDummy>.Scheduled(ref jobSpherecastCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003DBF0 File Offset: 0x0003BDF0
		public JobHandle ScheduleBatch(NativeArray<OverlapCapsuleCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				OverlapCapsuleCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobOverlapCapsuleCommandDummy jobOverlapCapsuleCommandDummy = new JobDependencyTracker.JobOverlapCapsuleCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobOverlapCapsuleCommandDummy>.GetDependencies(ref jobOverlapCapsuleCommandDummy, this);
			JobHandle jobHandle = OverlapCapsuleCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobOverlapCapsuleCommandDummy>.Scheduled(ref jobOverlapCapsuleCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003DC64 File Offset: 0x0003BE64
		public JobHandle ScheduleBatch(NativeArray<OverlapSphereCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				OverlapSphereCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobOverlapSphereCommandDummy jobOverlapSphereCommandDummy = new JobDependencyTracker.JobOverlapSphereCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobOverlapSphereCommandDummy>.GetDependencies(ref jobOverlapSphereCommandDummy, this);
			JobHandle jobHandle = OverlapSphereCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobOverlapSphereCommandDummy>.Scheduled(ref jobOverlapSphereCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003DCD6 File Offset: 0x0003BED6
		public void DeferFree(GCHandle handle, JobHandle dependsOn)
		{
			if (this.arena == null)
			{
				this.arena = new DisposeArena();
			}
			this.arena.Add(handle);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003DCF8 File Offset: 0x0003BEF8
		internal void JobReadsFrom(JobHandle job, long nativeArrayHash, int jobHash)
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				JobDependencyTracker.NativeArraySlot nativeArraySlot = this.slots[i];
				if (nativeArraySlot.hash == nativeArrayHash)
				{
					nativeArraySlot.lastReads.Add(new JobDependencyTracker.JobInstance
					{
						handle = job,
						hash = jobHash
					});
					return;
				}
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003DD58 File Offset: 0x0003BF58
		internal void JobWritesTo(JobHandle job, long nativeArrayHash, int jobHash)
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				JobDependencyTracker.NativeArraySlot nativeArraySlot = this.slots[i];
				if (nativeArraySlot.hash == nativeArrayHash)
				{
					nativeArraySlot.lastWrite = new JobDependencyTracker.JobInstance
					{
						handle = job,
						hash = jobHash
					};
					nativeArraySlot.lastReads.Clear();
					nativeArraySlot.initialized = true;
					nativeArraySlot.hasWrite = true;
					this.slots[i] = nativeArraySlot;
					return;
				}
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003DDDC File Offset: 0x0003BFDC
		private void Dispose()
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				ListPool<JobDependencyTracker.JobInstance>.Release(this.slots[i].lastReads);
			}
			this.slots.Clear();
			if (this.arena != null)
			{
				this.arena.DisposeAll();
			}
			this.linearDependencies = LinearDependencies.Check;
			if (this.dependenciesScratchBuffer.IsCreated)
			{
				this.dependenciesScratchBuffer.Dispose();
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003DE54 File Offset: 0x0003C054
		public void ClearMemory()
		{
			this.AllWritesDependency.Complete();
			this.Dispose();
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003DE75 File Offset: 0x0003C075
		void IAstarPooledObject.OnEnterPool()
		{
			this.Dispose();
		}

		// Token: 0x04000760 RID: 1888
		internal List<JobDependencyTracker.NativeArraySlot> slots = ListPool<JobDependencyTracker.NativeArraySlot>.Claim();

		// Token: 0x04000761 RID: 1889
		private DisposeArena arena;

		// Token: 0x04000762 RID: 1890
		internal NativeArray<JobHandle> dependenciesScratchBuffer;

		// Token: 0x04000763 RID: 1891
		private LinearDependencies linearDependencies;

		// Token: 0x04000764 RID: 1892
		internal TimeSlice timeSlice = TimeSlice.Infinite;

		// Token: 0x02000185 RID: 389
		internal struct JobInstance
		{
			// Token: 0x04000765 RID: 1893
			public JobHandle handle;

			// Token: 0x04000766 RID: 1894
			public int hash;
		}

		// Token: 0x02000186 RID: 390
		internal struct NativeArraySlot
		{
			// Token: 0x04000767 RID: 1895
			public long hash;

			// Token: 0x04000768 RID: 1896
			public JobDependencyTracker.JobInstance lastWrite;

			// Token: 0x04000769 RID: 1897
			public List<JobDependencyTracker.JobInstance> lastReads;

			// Token: 0x0400076A RID: 1898
			public bool initialized;

			// Token: 0x0400076B RID: 1899
			public bool hasWrite;
		}

		// Token: 0x02000187 RID: 391
		private struct JobRaycastCommandDummy : IJob
		{
			// Token: 0x06000B01 RID: 2817 RVA: 0x000035CE File Offset: 0x000017CE
			public void Execute()
			{
			}

			// Token: 0x0400076C RID: 1900
			[ReadOnly]
			public NativeArray<RaycastCommand> commands;

			// Token: 0x0400076D RID: 1901
			[WriteOnly]
			public NativeArray<RaycastHit> results;
		}

		// Token: 0x02000188 RID: 392
		private struct JobSpherecastCommandDummy : IJob
		{
			// Token: 0x06000B02 RID: 2818 RVA: 0x000035CE File Offset: 0x000017CE
			public void Execute()
			{
			}

			// Token: 0x0400076E RID: 1902
			[ReadOnly]
			public NativeArray<SpherecastCommand> commands;

			// Token: 0x0400076F RID: 1903
			[WriteOnly]
			public NativeArray<RaycastHit> results;
		}

		// Token: 0x02000189 RID: 393
		private struct JobOverlapCapsuleCommandDummy : IJob
		{
			// Token: 0x06000B03 RID: 2819 RVA: 0x000035CE File Offset: 0x000017CE
			public void Execute()
			{
			}

			// Token: 0x04000770 RID: 1904
			[ReadOnly]
			public NativeArray<OverlapCapsuleCommand> commands;

			// Token: 0x04000771 RID: 1905
			[WriteOnly]
			public NativeArray<ColliderHit> results;
		}

		// Token: 0x0200018A RID: 394
		private struct JobOverlapSphereCommandDummy : IJob
		{
			// Token: 0x06000B04 RID: 2820 RVA: 0x000035CE File Offset: 0x000017CE
			public void Execute()
			{
			}

			// Token: 0x04000772 RID: 1906
			[ReadOnly]
			public NativeArray<OverlapSphereCommand> commands;

			// Token: 0x04000773 RID: 1907
			[WriteOnly]
			public NativeArray<ColliderHit> results;
		}
	}
}
