using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace sc.modeling.splines.runtime
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class Settings
	{
		// Token: 0x04000001 RID: 1
		public Settings.Collision collision = new Settings.Collision();

		// Token: 0x04000002 RID: 2
		public Settings.Distribution distribution = new Settings.Distribution();

		// Token: 0x04000003 RID: 3
		public Settings.Deforming deforming = new Settings.Deforming();

		// Token: 0x04000004 RID: 4
		public Settings.UV uv = new Settings.UV();

		// Token: 0x04000005 RID: 5
		public Settings.Color color;

		// Token: 0x04000006 RID: 6
		public Settings.Conforming conforming = new Settings.Conforming();

		// Token: 0x04000007 RID: 7
		public Settings.OutputMesh mesh = new Settings.OutputMesh();

		// Token: 0x02000008 RID: 8
		public enum ColliderType
		{
			// Token: 0x0400004B RID: 75
			Box,
			// Token: 0x0400004C RID: 76
			Mesh
		}

		// Token: 0x02000009 RID: 9
		[Serializable]
		public class Collision
		{
			// Token: 0x0400004D RID: 77
			[Tooltip("Add a Mesh Collider component and also generate a collision mesh for it")]
			public bool enable;

			// Token: 0x0400004E RID: 78
			[Tooltip("Do not create a visible mesh, but only create the collision mesh")]
			public bool colliderOnly;

			// Token: 0x0400004F RID: 79
			[Tooltip("The \"Box\" type is an automatically created collider mesh, based on the source mesh's bounding box.")]
			public Settings.ColliderType type;

			// Token: 0x04000050 RID: 80
			[Min(0f)]
			[Tooltip("Subdivide the collision box, ensures it bends better in curves.")]
			public int boxSubdivisions;

			// Token: 0x04000051 RID: 81
			public Mesh collisionMesh;
		}

		// Token: 0x0200000A RID: 10
		[Serializable]
		public class Distribution
		{
			// Token: 0x04000052 RID: 82
			[Min(1f)]
			public int segments = 1;

			// Token: 0x04000053 RID: 83
			[Tooltip("Automatically calculate the number of segments based on the length of the spline")]
			public bool autoSegmentCount = true;

			// Token: 0x04000054 RID: 84
			[Tooltip("Stretch the segments so that they fit exactly over the entire spline")]
			public bool stretchToFit = true;

			// Token: 0x04000055 RID: 85
			[Tooltip("Ensure the input mesh is repeated evenly, instead of cutting it off when it doesn't fit on the remainder of the spline.")]
			public bool evenOnly;

			// Token: 0x04000056 RID: 86
			[Min(0f)]
			[Tooltip("Shift the mesh X number of units from the start of the spline")]
			public float trimStart;

			// Token: 0x04000057 RID: 87
			[Min(0f)]
			[Tooltip("Shift the mesh X number of units from the end of the spline")]
			public float trimEnd;

			// Token: 0x04000058 RID: 88
			[Tooltip("Space between each mesh segment")]
			public float spacing;
		}

		// Token: 0x0200000B RID: 11
		[Serializable]
		public class Deforming
		{
			// Token: 0x04000059 RID: 89
			[Tooltip("Note that offsetting can cause vertices to sort of bunch up.\n\nFor the best results, create a separate spline parallel to the one you are trying to offset from.")]
			[FormerlySerializedAs("offset")]
			public Vector2 curveOffset;

			// Token: 0x0400005A RID: 90
			[Tooltip("Adds a global offset to all vertices, effectively moving its pivot.\n\nNote: if the pivot is already centered, this appears to do exactly the same as the Curve Offset parameter")]
			public Vector2 pivotOffset;

			// Token: 0x0400005B RID: 91
			public Vector3 scale = Vector3.one;

			// Token: 0x0400005C RID: 92
			public PathIndexUnit scalePathIndexUnit;

			// Token: 0x0400005D RID: 93
			[FormerlySerializedAs("ignoreRoll")]
			[Tooltip("Ignore the spline's roll rotation and ensure the geometry stays flat")]
			public bool ignoreKnotRotation;

			// Token: 0x0400005E RID: 94
			[Tooltip("Specify if the rotation roll is calculated for every vertex, or once and applied over the entire segment")]
			public Settings.Deforming.RollMode rollMode;

			// Token: 0x0400005F RID: 95
			[Min(0f)]
			public float rollFrequency = 0.1f;

			// Token: 0x04000060 RID: 96
			[Range(-360f, 360f)]
			public float rollAngle;

			// Token: 0x04000061 RID: 97
			public PathIndexUnit rollPathIndexUnit;

			// Token: 0x0200001B RID: 27
			[Tooltip("The amount of times a complete rotation is completed over this distance. With a value of 1, a complete roll is created over 1 unit over the spline curve")]
			public enum RollMode
			{
				// Token: 0x0400008E RID: 142
				PerVertex,
				// Token: 0x0400008F RID: 143
				PerSegment
			}
		}

		// Token: 0x0200000C RID: 12
		[Serializable]
		public class UV
		{
			// Token: 0x04000062 RID: 98
			public Vector2 scale = Vector2.one;

			// Token: 0x04000063 RID: 99
			public Vector2 offset = Vector2.zero;

			// Token: 0x04000064 RID: 100
			[Tooltip("Overwrite the target UV value with that of the vertex position over the spline (normalized 0-1 value)")]
			public Settings.UV.StretchMode stretchMode;

			// Token: 0x0200001C RID: 28
			public enum StretchMode
			{
				// Token: 0x04000091 RID: 145
				None,
				// Token: 0x04000092 RID: 146
				[InspectorName("U (X)")]
				U,
				// Token: 0x04000093 RID: 147
				[InspectorName("V (Y)")]
				V
			}
		}

		// Token: 0x0200000D RID: 13
		[Serializable]
		public class Color
		{
			// Token: 0x04000065 RID: 101
			public PathIndexUnit pathIndexUnit;
		}

		// Token: 0x0200000E RID: 14
		[Serializable]
		public class Conforming
		{
			// Token: 0x04000066 RID: 102
			[Tooltip("Project the spline curve into the geometry underneath it. Relies on physics raycasts.")]
			public bool enable;

			// Token: 0x04000067 RID: 103
			[Tooltip("A ray is shot this high above every vertex, and reach this much units below it.\n\nIf a spline is dug into the terrain too much, increase this value to still get valid raycast hits.\n\nInternally, the minimum distance is always higher than the mesh's total height.")]
			public float seekDistance = 5f;

			// Token: 0x04000068 RID: 104
			[Tooltip("Ignore raycast hits from colliders that aren't from a Terrain")]
			public bool terrainOnly;

			// Token: 0x04000069 RID: 105
			[Tooltip("Only accept raycast hits from colliders on these layers")]
			public LayerMask layerMask = -1;

			// Token: 0x0400006A RID: 106
			[Tooltip("Rotate the geometry to match the orientation of the surface beneath it")]
			public bool align = true;

			// Token: 0x0400006B RID: 107
			[Tooltip("Reorient the geometry normals to match the surface hit, for correct lighting")]
			public bool blendNormal = true;
		}

		// Token: 0x0200000F RID: 15
		[Serializable]
		public class OutputMesh
		{
			// Token: 0x0400006C RID: 108
			[Tooltip("If enabled, Unity will keep a readable copy of the mesh around in memory. Allowing other scripts to access its data, and possible alter it.")]
			public bool keepReadable;

			// Token: 0x0400006D RID: 109
			[Tooltip("Save relative vertex positions in the (assumingly) unused UV components. If disabled, the source mesh's original values are retained.\n\n[UV0.Z]: (0-1) distance over spline length\n[UV0.W]: (0-1) distance over height of mesh\n\nThis data may be used in shaders for tailored effects, such as animations.")]
			public bool storeGradientsInUV = true;

			// Token: 0x0400006E RID: 110
			[Tooltip("Multiplier for the pack-margin value. A value of 1 equates to 1 texel")]
			[Min(0.01f)]
			public float lightmapUVMarginMultiplier = 1f;

			// Token: 0x0400006F RID: 111
			[Range(15f, 90f)]
			[Tooltip("This angle (in degrees) or greater between triangles will cause UV seam to be created.")]
			public float lightmapUVAngleThreshold = 88f;
		}
	}
}
