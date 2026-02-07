using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnityResonance
{
	// Token: 0x02000004 RID: 4
	[AddComponentMenu("ResonanceAudio/FmodResonanceAudioRoom")]
	public class FmodResonanceAudioRoom : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000261E File Offset: 0x0000081E
		private void OnEnable()
		{
			FmodResonanceAudio.UpdateAudioRoom(this, FmodResonanceAudio.IsListenerInsideRoom(this));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000262C File Offset: 0x0000082C
		private void OnDisable()
		{
			FmodResonanceAudio.UpdateAudioRoom(this, false);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002635 File Offset: 0x00000835
		private void Update()
		{
			FmodResonanceAudio.UpdateAudioRoom(this, FmodResonanceAudio.IsListenerInsideRoom(this));
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002643 File Offset: 0x00000843
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, this.Size);
		}

		// Token: 0x0400000F RID: 15
		[FormerlySerializedAs("leftWall")]
		public FmodResonanceAudioRoom.SurfaceMaterial LeftWall = FmodResonanceAudioRoom.SurfaceMaterial.ConcreteBlockCoarse;

		// Token: 0x04000010 RID: 16
		[FormerlySerializedAs("rightWall")]
		public FmodResonanceAudioRoom.SurfaceMaterial RightWall = FmodResonanceAudioRoom.SurfaceMaterial.ConcreteBlockCoarse;

		// Token: 0x04000011 RID: 17
		[FormerlySerializedAs("floor")]
		public FmodResonanceAudioRoom.SurfaceMaterial Floor = FmodResonanceAudioRoom.SurfaceMaterial.ParquetOnConcrete;

		// Token: 0x04000012 RID: 18
		[FormerlySerializedAs("ceiling")]
		public FmodResonanceAudioRoom.SurfaceMaterial Ceiling = FmodResonanceAudioRoom.SurfaceMaterial.PlasterRough;

		// Token: 0x04000013 RID: 19
		[FormerlySerializedAs("backWall")]
		public FmodResonanceAudioRoom.SurfaceMaterial BackWall = FmodResonanceAudioRoom.SurfaceMaterial.ConcreteBlockCoarse;

		// Token: 0x04000014 RID: 20
		[FormerlySerializedAs("frontWall")]
		public FmodResonanceAudioRoom.SurfaceMaterial FrontWall = FmodResonanceAudioRoom.SurfaceMaterial.ConcreteBlockCoarse;

		// Token: 0x04000015 RID: 21
		[FormerlySerializedAs("reflectivity")]
		public float Reflectivity = 1f;

		// Token: 0x04000016 RID: 22
		[FormerlySerializedAs("reverbGainDb")]
		public float ReverbGainDb;

		// Token: 0x04000017 RID: 23
		[FormerlySerializedAs("reverbBrightness")]
		public float ReverbBrightness;

		// Token: 0x04000018 RID: 24
		[FormerlySerializedAs("reverbTime")]
		public float ReverbTime = 1f;

		// Token: 0x04000019 RID: 25
		[FormerlySerializedAs("size")]
		public Vector3 Size = Vector3.one;

		// Token: 0x02000008 RID: 8
		public enum SurfaceMaterial
		{
			// Token: 0x04000036 RID: 54
			Transparent,
			// Token: 0x04000037 RID: 55
			AcousticCeilingTiles,
			// Token: 0x04000038 RID: 56
			BrickBare,
			// Token: 0x04000039 RID: 57
			BrickPainted,
			// Token: 0x0400003A RID: 58
			ConcreteBlockCoarse,
			// Token: 0x0400003B RID: 59
			ConcreteBlockPainted,
			// Token: 0x0400003C RID: 60
			CurtainHeavy,
			// Token: 0x0400003D RID: 61
			FiberglassInsulation,
			// Token: 0x0400003E RID: 62
			GlassThin,
			// Token: 0x0400003F RID: 63
			GlassThick,
			// Token: 0x04000040 RID: 64
			Grass,
			// Token: 0x04000041 RID: 65
			LinoleumOnConcrete,
			// Token: 0x04000042 RID: 66
			Marble,
			// Token: 0x04000043 RID: 67
			Metal,
			// Token: 0x04000044 RID: 68
			ParquetOnConcrete,
			// Token: 0x04000045 RID: 69
			PlasterRough,
			// Token: 0x04000046 RID: 70
			PlasterSmooth,
			// Token: 0x04000047 RID: 71
			PlywoodPanel,
			// Token: 0x04000048 RID: 72
			PolishedConcreteOrTile,
			// Token: 0x04000049 RID: 73
			Sheetrock,
			// Token: 0x0400004A RID: 74
			WaterOrIceSurface,
			// Token: 0x0400004B RID: 75
			WoodCeiling,
			// Token: 0x0400004C RID: 76
			WoodPanel
		}
	}
}
