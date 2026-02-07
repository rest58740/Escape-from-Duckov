using System;

namespace Pathfinding
{
	// Token: 0x0200010B RID: 267
	[Obsolete("Has been renamed to RecastNavmeshModifier")]
	public interface RecastMeshObj
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600088E RID: 2190
		// (set) Token: 0x0600088F RID: 2191
		bool enabled { get; set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000890 RID: 2192
		// (set) Token: 0x06000891 RID: 2193
		bool dynamic { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000892 RID: 2194
		// (set) Token: 0x06000893 RID: 2195
		bool solid { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000894 RID: 2196
		// (set) Token: 0x06000895 RID: 2197
		RecastNavmeshModifier.GeometrySource geometrySource { get; set; }

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000896 RID: 2198
		// (set) Token: 0x06000897 RID: 2199
		RecastNavmeshModifier.ScanInclusion includeInScan { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000898 RID: 2200
		// (set) Token: 0x06000899 RID: 2201
		int surfaceID { get; set; }

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600089A RID: 2202
		// (set) Token: 0x0600089B RID: 2203
		RecastNavmeshModifier.Mode mode { get; set; }
	}
}
