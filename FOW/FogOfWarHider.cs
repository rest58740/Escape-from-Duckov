using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace FOW
{
	// Token: 0x02000007 RID: 7
	public class FogOfWarHider : MonoBehaviour
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00004134 File Offset: 0x00002334
		private void OnEnable()
		{
			this.CalculateSamplePointData();
			this.RegisterHider();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004142 File Offset: 0x00002342
		private void OnDisable()
		{
			this.SetActive(true);
			this.DeregisterHider();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004154 File Offset: 0x00002354
		private void CalculateSamplePointData()
		{
			if (this.SamplePoints.Length == 0)
			{
				this.SamplePoints = new Transform[1];
				this.SamplePoints[0] = base.transform;
			}
			this.MaxDistBetweenSamplePoints = 0f;
			for (int i = 0; i < this.SamplePoints.Length; i++)
			{
				for (int j = i; j < this.SamplePoints.Length; j++)
				{
					this.MaxDistBetweenSamplePoints = Mathf.Max(this.MaxDistBetweenSamplePoints, Vector3.Distance(this.SamplePoints[i].position, this.SamplePoints[j].position));
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000041E5 File Offset: 0x000023E5
		public void RegisterHider()
		{
			this.CachedTransform = base.transform;
			if (!FogOfWarWorld.HidersList.Contains(this))
			{
				FogOfWarWorld.HidersList.Add(this);
				FogOfWarWorld.NumHiders++;
				this.SetActive(false);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004220 File Offset: 0x00002420
		public void DeregisterHider()
		{
			if (FogOfWarWorld.HidersList.Contains(this))
			{
				FogOfWarWorld.HidersList.Remove(this);
				FogOfWarWorld.NumHiders--;
				foreach (FogOfWarRevealer fogOfWarRevealer in this.Observers)
				{
					fogOfWarRevealer.HidersSeen.Remove(this);
				}
				this.NumObservers = 0;
				this.Observers.Clear();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000042B0 File Offset: 0x000024B0
		public void AddObserver(FogOfWarRevealer Observer)
		{
			if (this.PermanentlyReveal)
			{
				base.enabled = false;
				return;
			}
			this.Observers.Add(Observer);
			if (this.NumObservers == 0)
			{
				this.SetActive(true);
			}
			this.NumObservers++;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000042EB File Offset: 0x000024EB
		public void RemoveObserver(FogOfWarRevealer Observer)
		{
			this.Observers.Remove(Observer);
			this.NumObservers--;
			if (this.NumObservers == 0)
			{
				this.SetActive(false);
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000041 RID: 65 RVA: 0x00004318 File Offset: 0x00002518
		// (remove) Token: 0x06000042 RID: 66 RVA: 0x00004350 File Offset: 0x00002550
		public event FogOfWarHider.OnChangeActive OnActiveChanged;

		// Token: 0x06000043 RID: 67 RVA: 0x00004385 File Offset: 0x00002585
		private void SetActive(bool isActive)
		{
			FogOfWarHider.OnChangeActive onActiveChanged = this.OnActiveChanged;
			if (onActiveChanged == null)
			{
				return;
			}
			onActiveChanged(isActive);
		}

		// Token: 0x04000075 RID: 117
		[Tooltip("Leaving this empty will make the hider use its own transform as a sample point.")]
		[FormerlySerializedAs("samplePoints")]
		public Transform[] SamplePoints;

		// Token: 0x04000076 RID: 118
		[Tooltip("If Enabled, the hider will never be hidden again after being revealed once.")]
		public bool PermanentlyReveal;

		// Token: 0x04000077 RID: 119
		[HideInInspector]
		public float MaxDistBetweenSamplePoints;

		// Token: 0x04000078 RID: 120
		[HideInInspector]
		public int NumObservers;

		// Token: 0x04000079 RID: 121
		[HideInInspector]
		public List<FogOfWarRevealer> Observers = new List<FogOfWarRevealer>();

		// Token: 0x0400007A RID: 122
		[HideInInspector]
		public Transform CachedTransform;

		// Token: 0x02000023 RID: 35
		// (Invoke) Token: 0x060000BB RID: 187
		public delegate void OnChangeActive(bool isActive);
	}
}
