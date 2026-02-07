using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using Saves;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x0200029E RID: 670
	public class GoldMiner : MiniGameBehaviour
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x00051DFA File Offset: 0x0004FFFA
		public Hook Hook
		{
			get
			{
				return this.hook;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x00051E02 File Offset: 0x00050002
		public Bounds Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x00051E0A File Offset: 0x0005000A
		public int Money
		{
			get
			{
				if (this.run == null)
				{
					return 0;
				}
				return this.run.money;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x00051E21 File Offset: 0x00050021
		public ReadOnlyCollection<GoldMinerArtifact> ArtifactPrefabs
		{
			get
			{
				if (this.artifactPrefabs_ReadOnly == null)
				{
					this.artifactPrefabs_ReadOnly = new ReadOnlyCollection<GoldMinerArtifact>(this.artifactPrefabs);
				}
				return this.artifactPrefabs_ReadOnly;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001603 RID: 5635 RVA: 0x00051E42 File Offset: 0x00050042
		// (set) Token: 0x06001604 RID: 5636 RVA: 0x00051E4E File Offset: 0x0005004E
		public static int HighLevel
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/GoldMiner/HighLevel");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/GoldMiner/HighLevel", value);
			}
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00051E5C File Offset: 0x0005005C
		private void Awake()
		{
			this.Hook.OnBeginRetrieve += this.OnHookBeginRetrieve;
			this.Hook.OnEndRetrieve += this.OnHookEndRetrieve;
			this.Hook.OnLaunch += this.OnHookLaunch;
			this.Hook.OnResolveTarget += this.OnHookResolveEntity;
			this.Hook.OnAttach += this.OnHookAttach;
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00051EDC File Offset: 0x000500DC
		protected override void Start()
		{
			base.Start();
			this.hook.BeginSwing();
			this.Main().Forget();
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00051EFA File Offset: 0x000500FA
		internal bool PayMoney(int price)
		{
			if (this.run.money < price)
			{
				return false;
			}
			this.run.money -= price;
			return true;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00051F20 File Offset: 0x00050120
		// (set) Token: 0x06001609 RID: 5641 RVA: 0x00051F28 File Offset: 0x00050128
		public GoldMinerRunData run { get; private set; }

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x0600160A RID: 5642 RVA: 0x00051F34 File Offset: 0x00050134
		// (remove) Token: 0x0600160B RID: 5643 RVA: 0x00051F68 File Offset: 0x00050168
		public static event Action<int> OnLevelClear;

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x00051F9B File Offset: 0x0005019B
		private bool ShouldQuit
		{
			get
			{
				return this.isBeingDestroyed;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x00051FA8 File Offset: 0x000501A8
		public float GlobalPriceFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00051FB0 File Offset: 0x000501B0
		private UniTask Main()
		{
			GoldMiner.<Main>d__51 <Main>d__;
			<Main>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Main>d__.<>4__this = this;
			<Main>d__.<>1__state = -1;
			<Main>d__.<>t__builder.Start<GoldMiner.<Main>d__51>(ref <Main>d__);
			return <Main>d__.<>t__builder.Task;
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00051FF4 File Offset: 0x000501F4
		private UniTask DoTitleScreen()
		{
			GoldMiner.<DoTitleScreen>d__53 <DoTitleScreen>d__;
			<DoTitleScreen>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoTitleScreen>d__.<>4__this = this;
			<DoTitleScreen>d__.<>1__state = -1;
			<DoTitleScreen>d__.<>t__builder.Start<GoldMiner.<DoTitleScreen>d__53>(ref <DoTitleScreen>d__);
			return <DoTitleScreen>d__.<>t__builder.Task;
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00052038 File Offset: 0x00050238
		private UniTask DoGameOver()
		{
			GoldMiner.<DoGameOver>d__55 <DoGameOver>d__;
			<DoGameOver>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoGameOver>d__.<>4__this = this;
			<DoGameOver>d__.<>1__state = -1;
			<DoGameOver>d__.<>t__builder.Start<GoldMiner.<DoGameOver>d__55>(ref <DoGameOver>d__);
			return <DoGameOver>d__.<>t__builder.Task;
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0005207B File Offset: 0x0005027B
		public void Cleanup()
		{
			if (this.run != null)
			{
				this.run.Cleanup();
			}
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00052090 File Offset: 0x00050290
		private void GenerateLevel()
		{
			GoldMiner.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			for (int i = 0; i < this.activeEntities.Count; i++)
			{
				GoldMinerEntity goldMinerEntity = this.activeEntities[i];
				if (!(goldMinerEntity == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(goldMinerEntity.gameObject);
					}
					else
					{
						Object.DestroyImmediate(goldMinerEntity.gameObject);
					}
				}
			}
			this.activeEntities.Clear();
			for (int j = 0; j < this.resolvedEntities.Count; j++)
			{
				GoldMinerEntity goldMinerEntity2 = this.activeEntities[j];
				if (!(goldMinerEntity2 == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(goldMinerEntity2.gameObject);
					}
					else
					{
						Object.DestroyImmediate(goldMinerEntity2.gameObject);
					}
				}
			}
			this.resolvedEntities.Clear();
			int seed = this.run.levelRandom.Next();
			CS$<>8__locals1.levelGenRandom = new Random(seed);
			int minValue = 10;
			int maxValue = 20;
			int num = CS$<>8__locals1.levelGenRandom.Next(minValue, maxValue);
			for (int k = 0; k < num; k++)
			{
				GoldMinerEntity random = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, 0f);
				this.<GenerateLevel>g__Generate|58_0(random, ref CS$<>8__locals1);
			}
			for (float num2 = this.run.extraRocks; num2 > 0f; num2 -= 1f)
			{
				if (num2 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num2)
				{
					GoldMinerEntity random2 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Rock), 0f);
					this.<GenerateLevel>g__Generate|58_0(random2, ref CS$<>8__locals1);
				}
			}
			for (float num3 = this.run.extraGold; num3 > 0f; num3 -= 1f)
			{
				if (num3 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num3)
				{
					GoldMinerEntity random3 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Gold), 0f);
					this.<GenerateLevel>g__Generate|58_0(random3, ref CS$<>8__locals1);
				}
			}
			for (float num4 = this.run.extraDiamond; num4 > 0f; num4 -= 1f)
			{
				if (num4 > 1f || CS$<>8__locals1.levelGenRandom.NextDouble() < (double)num4)
				{
					GoldMinerEntity random4 = this.entities.GetRandom(CS$<>8__locals1.levelGenRandom, (GoldMinerEntity e) => e.tags.Contains(GoldMinerEntity.Tag.Diamond), 0f);
					this.<GenerateLevel>g__Generate|58_0(random4, ref CS$<>8__locals1);
				}
			}
			this.run.shopRandom = new Random(this.run.seed + CS$<>8__locals1.levelGenRandom.Next());
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00052364 File Offset: 0x00050564
		private Vector3 NormalizedPosToLocalPos(Vector2 posNormalized)
		{
			float num = Mathf.Lerp(this.bounds.min.x, this.bounds.max.x, posNormalized.x);
			float num2 = Mathf.Lerp(this.bounds.min.y, this.bounds.max.y, posNormalized.y);
			return new Vector3(num, num2, 0f);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000523D3 File Offset: 0x000505D3
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = this.levelLayout.localToWorldMatrix;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.extents * 2f);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0005240C File Offset: 0x0005060C
		private UniTask Run(int seed = 0)
		{
			GoldMiner.<Run>d__61 <Run>d__;
			<Run>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Run>d__.<>4__this = this;
			<Run>d__.seed = seed;
			<Run>d__.<>1__state = -1;
			<Run>d__.<>t__builder.Start<GoldMiner.<Run>d__61>(ref <Run>d__);
			return <Run>d__.<>t__builder.Task;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00052458 File Offset: 0x00050658
		private UniTask<bool> SettleLevel()
		{
			GoldMiner.<SettleLevel>d__62 <SettleLevel>d__;
			<SettleLevel>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SettleLevel>d__.<>4__this = this;
			<SettleLevel>d__.<>1__state = -1;
			<SettleLevel>d__.<>t__builder.Start<GoldMiner.<SettleLevel>d__62>(ref <SettleLevel>d__);
			return <SettleLevel>d__.<>t__builder.Task;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0005249C File Offset: 0x0005069C
		private UniTask DoLevel()
		{
			GoldMiner.<DoLevel>d__65 <DoLevel>d__;
			<DoLevel>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoLevel>d__.<>4__this = this;
			<DoLevel>d__.<>1__state = -1;
			<DoLevel>d__.<>t__builder.Start<GoldMiner.<DoLevel>d__65>(ref <DoLevel>d__);
			return <DoLevel>d__.<>t__builder.Task;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000524DF File Offset: 0x000506DF
		protected override void OnUpdate(float deltaTime)
		{
			if (this.levelPlaying)
			{
				this.UpdateLevelPlaying(deltaTime);
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000524F0 File Offset: 0x000506F0
		private void UpdateLevelPlaying(float deltaTime)
		{
			Action<GoldMiner> action = this.onEarlyLevelPlayTick;
			if (action != null)
			{
				action(this);
			}
			this.Hook.SetParameters(this.run.GameSpeedFactor, this.run.emptySpeed.Value, this.run.strength.Value);
			this.Hook.Tick(deltaTime);
			Hook.HookStatus status = this.Hook.Status;
			if (status != Hook.HookStatus.Swinging)
			{
				if (status == Hook.HookStatus.Retrieving)
				{
					this.run.stamina -= deltaTime * this.run.staminaDrain.Value;
				}
			}
			else if (this.launchHook)
			{
				this.Hook.Launch();
			}
			Action<GoldMiner> action2 = this.onLateLevelPlayTick;
			if (action2 != null)
			{
				action2(this);
			}
			this.launchHook = false;
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000525B9 File Offset: 0x000507B9
		public void LaunchHook()
		{
			this.launchHook = true;
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000525C4 File Offset: 0x000507C4
		private bool IsLevelOver()
		{
			this.activeEntities.RemoveAll((GoldMinerEntity e) => e == null);
			return this.activeEntities.Count <= 0 || (this.hook.Status == Hook.HookStatus.Swinging && this.run.stamina <= 0f) || (this.Hook.Status == Hook.HookStatus.Retrieving && this.run.stamina < -this.run.extraStamina.Value);
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00052660 File Offset: 0x00050860
		private UniTask DoShop()
		{
			GoldMiner.<DoShop>d__71 <DoShop>d__;
			<DoShop>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoShop>d__.<>4__this = this;
			<DoShop>d__.<>1__state = -1;
			<DoShop>d__.<>t__builder.Start<GoldMiner.<DoShop>d__71>(ref <DoShop>d__);
			return <DoShop>d__.<>t__builder.Task;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x000526A4 File Offset: 0x000508A4
		private void OnHookResolveEntity(Hook hook, GoldMinerEntity entity)
		{
			entity.NotifyResolved(this);
			entity.gameObject.SetActive(false);
			this.activeEntities.Remove(entity);
			this.resolvedEntities.Add(entity);
			if (this.run.IsRock(entity))
			{
				entity.Value = Mathf.CeilToInt((float)entity.Value * this.run.rockValueFactor.Value);
			}
			if (this.run.IsGold(entity))
			{
				entity.Value = Mathf.CeilToInt((float)entity.Value * this.run.goldValueFactor.Value);
			}
			this.popText.Pop(string.Format("${0}", entity.Value), hook.Axis.position);
			Action<GoldMiner, GoldMinerEntity> action = this.onResolveEntity;
			if (action != null)
			{
				action(this, entity);
			}
			Action<GoldMiner, GoldMinerEntity> action2 = this.onAfterResolveEntity;
			if (action2 == null)
			{
				return;
			}
			action2(this, entity);
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0005278F File Offset: 0x0005098F
		private void OnHookBeginRetrieve(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookBeginRetrieve;
			if (action == null)
			{
				return;
			}
			action(this, hook);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x000527A3 File Offset: 0x000509A3
		private void OnHookEndRetrieve(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookEndRetrieve;
			if (action != null)
			{
				action(this, hook);
			}
			if (this.run.StrengthPotionActivated)
			{
				this.run.DeactivateStrengthPotion();
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x000527D0 File Offset: 0x000509D0
		private void OnHookLaunch(Hook hook)
		{
			Action<GoldMiner, Hook> action = this.onHookLaunch;
			if (action != null)
			{
				action(this, hook);
			}
			if (this.run.EagleEyeActivated)
			{
				this.run.DeactivateEagleEye();
			}
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x000527FD File Offset: 0x000509FD
		private void OnHookAttach(Hook hook, GoldMinerEntity entity)
		{
			Action<GoldMiner, Hook, GoldMinerEntity> action = this.onHookAttach;
			if (action == null)
			{
				return;
			}
			action(this, hook, entity);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00052812 File Offset: 0x00050A12
		public bool UseStrengthPotion()
		{
			if (this.run.strengthPotion <= 0)
			{
				return false;
			}
			if (this.run.StrengthPotionActivated)
			{
				return false;
			}
			this.run.strengthPotion--;
			this.run.ActivateStrengthPotion();
			return true;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00052852 File Offset: 0x00050A52
		public bool UseEagleEyePotion()
		{
			if (this.run.eagleEyePotion <= 0)
			{
				return false;
			}
			if (this.run.EagleEyeActivated)
			{
				return false;
			}
			this.run.eagleEyePotion--;
			this.run.ActivateEagleEye();
			return true;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00052894 File Offset: 0x00050A94
		public GoldMinerArtifact GetArtifactPrefab(string id)
		{
			return this.artifactPrefabs.Find((GoldMinerArtifact e) => e != null && e.ID == id);
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000528C8 File Offset: 0x00050AC8
		internal bool UseBomb()
		{
			if (this.run.bomb <= 0)
			{
				return false;
			}
			this.run.bomb--;
			Object.Instantiate<Bomb>(this.bombPrefab, this.hook.Axis.transform.position, Quaternion.FromToRotation(Vector3.up, -this.hook.Axis.transform.up), base.transform);
			return true;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00052944 File Offset: 0x00050B44
		internal void NotifyArtifactChange()
		{
			Action<GoldMiner> action = this.onArtifactChange;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x00052957 File Offset: 0x00050B57
		// (set) Token: 0x06001628 RID: 5672 RVA: 0x0005295F File Offset: 0x00050B5F
		public bool isBeingDestroyed { get; private set; }

		// Token: 0x06001629 RID: 5673 RVA: 0x00052968 File Offset: 0x00050B68
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0005299C File Offset: 0x00050B9C
		[CompilerGenerated]
		private void <GenerateLevel>g__Generate|58_0(GoldMinerEntity entityPrefab, ref GoldMiner.<>c__DisplayClass58_0 A_2)
		{
			if (entityPrefab == null)
			{
				return;
			}
			Vector2 posNormalized;
			posNormalized..ctor((float)A_2.levelGenRandom.NextDouble(), (float)A_2.levelGenRandom.NextDouble());
			GoldMinerEntity goldMinerEntity = Object.Instantiate<GoldMinerEntity>(entityPrefab, this.levelLayout);
			Vector3 localPosition = this.NormalizedPosToLocalPos(posNormalized);
			Quaternion localRotation = Quaternion.AngleAxis((float)A_2.levelGenRandom.NextDouble() * 360f, Vector3.forward);
			goldMinerEntity.transform.localPosition = localPosition;
			goldMinerEntity.transform.localRotation = localRotation;
			goldMinerEntity.SetMaster(this);
			this.activeEntities.Add(goldMinerEntity);
		}

		// Token: 0x0400103E RID: 4158
		[SerializeField]
		private Hook hook;

		// Token: 0x0400103F RID: 4159
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x04001040 RID: 4160
		[SerializeField]
		private LevelSettlementUI settlementUI;

		// Token: 0x04001041 RID: 4161
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x04001042 RID: 4162
		[SerializeField]
		private GameObject gameoverScreen;

		// Token: 0x04001043 RID: 4163
		[SerializeField]
		private GoldMiner_PopText popText;

		// Token: 0x04001044 RID: 4164
		[SerializeField]
		private Transform levelLayout;

		// Token: 0x04001045 RID: 4165
		[SerializeField]
		private Bounds bounds;

		// Token: 0x04001046 RID: 4166
		[SerializeField]
		private Bomb bombPrefab;

		// Token: 0x04001047 RID: 4167
		[SerializeField]
		private RandomContainer<GoldMinerEntity> entities;

		// Token: 0x04001048 RID: 4168
		[SerializeField]
		private List<GoldMinerArtifact> artifactPrefabs = new List<GoldMinerArtifact>();

		// Token: 0x04001049 RID: 4169
		private ReadOnlyCollection<GoldMinerArtifact> artifactPrefabs_ReadOnly;

		// Token: 0x0400104A RID: 4170
		public Action<GoldMiner> onLevelBegin;

		// Token: 0x0400104B RID: 4171
		public Action<GoldMiner> onLevelEnd;

		// Token: 0x0400104C RID: 4172
		public Action<GoldMiner> onShopBegin;

		// Token: 0x0400104D RID: 4173
		public Action<GoldMiner> onShopEnd;

		// Token: 0x0400104E RID: 4174
		public Action<GoldMiner> onEarlyLevelPlayTick;

		// Token: 0x0400104F RID: 4175
		public Action<GoldMiner> onLateLevelPlayTick;

		// Token: 0x04001050 RID: 4176
		public Action<GoldMiner, Hook> onHookLaunch;

		// Token: 0x04001051 RID: 4177
		public Action<GoldMiner, Hook> onHookBeginRetrieve;

		// Token: 0x04001052 RID: 4178
		public Action<GoldMiner, Hook> onHookEndRetrieve;

		// Token: 0x04001053 RID: 4179
		public Action<GoldMiner, Hook, GoldMinerEntity> onHookAttach;

		// Token: 0x04001054 RID: 4180
		public Action<GoldMiner, GoldMinerEntity> onResolveEntity;

		// Token: 0x04001055 RID: 4181
		public Action<GoldMiner, GoldMinerEntity> onAfterResolveEntity;

		// Token: 0x04001056 RID: 4182
		public Action<GoldMiner> onArtifactChange;

		// Token: 0x04001057 RID: 4183
		private const string HighLevelSaveKey = "MiniGame/GoldMiner/HighLevel";

		// Token: 0x0400105A RID: 4186
		private bool titleConfirmed;

		// Token: 0x0400105B RID: 4187
		private bool gameOverConfirmed;

		// Token: 0x0400105C RID: 4188
		public List<GoldMinerEntity> activeEntities = new List<GoldMinerEntity>();

		// Token: 0x0400105D RID: 4189
		private bool levelPlaying;

		// Token: 0x0400105E RID: 4190
		public List<GoldMinerEntity> resolvedEntities = new List<GoldMinerEntity>();

		// Token: 0x0400105F RID: 4191
		private bool launchHook;
	}
}
