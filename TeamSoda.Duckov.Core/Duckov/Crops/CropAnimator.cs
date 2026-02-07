using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002F8 RID: 760
	public class CropAnimator : MonoBehaviour
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x0005AE56 File Offset: 0x00059056
		private ParticleSystem PlantFX
		{
			get
			{
				return this.plantFX;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x0005AE5E File Offset: 0x0005905E
		private ParticleSystem StageChangeFX
		{
			get
			{
				return this.stageChangeFX;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x0005AE66 File Offset: 0x00059066
		private ParticleSystem RipenFX
		{
			get
			{
				return this.ripenFX;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x0005AE6E File Offset: 0x0005906E
		private ParticleSystem WaterFX
		{
			get
			{
				return this.waterFX;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0005AE76 File Offset: 0x00059076
		private ParticleSystem HarvestFX
		{
			get
			{
				return this.harvestFX;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0005AE7E File Offset: 0x0005907E
		private ParticleSystem DestroyFX
		{
			get
			{
				return this.destroyFX;
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0005AE88 File Offset: 0x00059088
		private void Awake()
		{
			if (this.crop == null)
			{
				this.crop = base.GetComponent<Crop>();
			}
			Crop crop = this.crop;
			crop.onPlant = (Action<Crop>)Delegate.Combine(crop.onPlant, new Action<Crop>(this.OnPlant));
			Crop crop2 = this.crop;
			crop2.onRipen = (Action<Crop>)Delegate.Combine(crop2.onRipen, new Action<Crop>(this.OnRipen));
			Crop crop3 = this.crop;
			crop3.onWater = (Action<Crop>)Delegate.Combine(crop3.onWater, new Action<Crop>(this.OnWater));
			Crop crop4 = this.crop;
			crop4.onHarvest = (Action<Crop>)Delegate.Combine(crop4.onHarvest, new Action<Crop>(this.OnHarvest));
			Crop crop5 = this.crop;
			crop5.onBeforeDestroy = (Action<Crop>)Delegate.Combine(crop5.onBeforeDestroy, new Action<Crop>(this.OnBeforeDestroy));
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0005AF72 File Offset: 0x00059172
		private void Update()
		{
			this.RefreshPosition(true);
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0005AF7C File Offset: 0x0005917C
		private void RefreshPosition(bool notifyStageChange = true)
		{
			float progress = this.crop.Progress;
			CropAnimator.Stage stage = default(CropAnimator.Stage);
			int? num = this.cachedStage;
			for (int i = 0; i < this.stages.Length; i++)
			{
				CropAnimator.Stage stage2 = this.stages[i];
				if (progress < this.stages[i].progress)
				{
					stage = stage2;
					this.cachedStage = new int?(i);
					break;
				}
			}
			this.displayParent.localPosition = Vector3.up * stage.position;
			if (!notifyStageChange)
			{
				return;
			}
			if (num == null)
			{
				return;
			}
			int value = num.Value;
			int? num2 = this.cachedStage;
			if (!(value == num2.GetValueOrDefault() & num2 != null))
			{
				this.OnStageChange();
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0005B03B File Offset: 0x0005923B
		private void OnStageChange()
		{
			FXPool.Play(this.StageChangeFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0005B05F File Offset: 0x0005925F
		private void OnWater(Crop crop)
		{
			FXPool.Play(this.WaterFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0005B083 File Offset: 0x00059283
		private void OnRipen(Crop crop)
		{
			FXPool.Play(this.RipenFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0005B0A7 File Offset: 0x000592A7
		private void OnHarvest(Crop crop)
		{
			FXPool.Play(this.HarvestFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0005B0CB File Offset: 0x000592CB
		private void OnPlant(Crop crop)
		{
			FXPool.Play(this.PlantFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0005B0EF File Offset: 0x000592EF
		private void OnBeforeDestroy(Crop crop)
		{
			FXPool.Play(this.DestroyFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x040011FE RID: 4606
		[SerializeField]
		private Crop crop;

		// Token: 0x040011FF RID: 4607
		[SerializeField]
		private Transform displayParent;

		// Token: 0x04001200 RID: 4608
		[SerializeField]
		private ParticleSystem plantFX;

		// Token: 0x04001201 RID: 4609
		[SerializeField]
		private ParticleSystem stageChangeFX;

		// Token: 0x04001202 RID: 4610
		[SerializeField]
		private ParticleSystem ripenFX;

		// Token: 0x04001203 RID: 4611
		[SerializeField]
		private ParticleSystem waterFX;

		// Token: 0x04001204 RID: 4612
		[SerializeField]
		private ParticleSystem harvestFX;

		// Token: 0x04001205 RID: 4613
		[SerializeField]
		private ParticleSystem destroyFX;

		// Token: 0x04001206 RID: 4614
		[SerializeField]
		private CropAnimator.Stage[] stages = new CropAnimator.Stage[]
		{
			new CropAnimator.Stage(0.333f, -0.4f),
			new CropAnimator.Stage(0.666f, -0.2f),
			new CropAnimator.Stage(0.999f, -0.1f)
		};

		// Token: 0x04001207 RID: 4615
		private int? cachedStage;

		// Token: 0x020005A5 RID: 1445
		[Serializable]
		private struct Stage
		{
			// Token: 0x0600299E RID: 10654 RVA: 0x0009A6BE File Offset: 0x000988BE
			public Stage(float progress, float position)
			{
				this.progress = progress;
				this.position = position;
			}

			// Token: 0x040020A9 RID: 8361
			public float progress;

			// Token: 0x040020AA RID: 8362
			public float position;
		}
	}
}
