using System;
using Pathfinding.RVO;
using Unity.Collections;

namespace Pathfinding.ECS.RVO
{
	// Token: 0x0200024C RID: 588
	public readonly struct AgentIndex
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00056FE3 File Offset: 0x000551E3
		internal int Index
		{
			get
			{
				return this.packedAgentIndex & 16777215;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x00056FF1 File Offset: 0x000551F1
		private int Version
		{
			get
			{
				return this.packedAgentIndex & 2130706432;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x00056FFF File Offset: 0x000551FF
		internal bool Valid
		{
			get
			{
				return (this.packedAgentIndex & int.MinValue) == 0;
			}
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00057010 File Offset: 0x00055210
		internal AgentIndex(int packedAgentIndex)
		{
			this.packedAgentIndex = packedAgentIndex;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00057019 File Offset: 0x00055219
		internal AgentIndex(int version, int index)
		{
			version <<= 24;
			this.packedAgentIndex = ((version & 2130706432) | (index & 16777215));
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00057036 File Offset: 0x00055236
		internal AgentIndex WithIncrementedVersion()
		{
			return new AgentIndex(((this.packedAgentIndex & 2130706432) + 16777216 & 2130706432) | this.Index);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0005705C File Offset: 0x0005525C
		internal AgentIndex WithDeleted()
		{
			return new AgentIndex(this.packedAgentIndex | int.MinValue);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00057070 File Offset: 0x00055270
		public bool Exists(ref SimulatorBurst.AgentData agentData)
		{
			int num;
			return this.TryGetIndex(ref agentData, out num);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00057086 File Offset: 0x00055286
		public bool TryGetIndex(ref SimulatorBurst.AgentData agentData, out int index)
		{
			return this.TryGetIndex(ref agentData.version, out index);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00057098 File Offset: 0x00055298
		public bool TryGetIndex(ref NativeArray<AgentIndex> agentDataVersions, out int index)
		{
			int index2 = this.Index;
			index = -1;
			if (!agentDataVersions.IsCreated)
			{
				return false;
			}
			if (index2 >= agentDataVersions.Length)
			{
				return false;
			}
			if (agentDataVersions[index2].Version != this.Version)
			{
				return false;
			}
			index = index2;
			return true;
		}

		// Token: 0x04000A97 RID: 2711
		private const int DeletedBit = -2147483648;

		// Token: 0x04000A98 RID: 2712
		private const int IndexMask = 16777215;

		// Token: 0x04000A99 RID: 2713
		private const int VersionOffset = 24;

		// Token: 0x04000A9A RID: 2714
		private const int VersionMask = 2130706432;

		// Token: 0x04000A9B RID: 2715
		private readonly int packedAgentIndex;
	}
}
