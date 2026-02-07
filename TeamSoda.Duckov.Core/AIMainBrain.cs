using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ParadoxNotion;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class AIMainBrain : MonoBehaviour
{
	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060008AC RID: 2220 RVA: 0x00027074 File Offset: 0x00025274
	private static CharacterMainControl mainCharacter
	{
		get
		{
			if (AIMainBrain._mc == null)
			{
				AIMainBrain._mc = CharacterMainControl.Main;
			}
			return AIMainBrain._mc;
		}
	}

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060008AD RID: 2221 RVA: 0x00027094 File Offset: 0x00025294
	// (remove) Token: 0x060008AE RID: 2222 RVA: 0x000270C8 File Offset: 0x000252C8
	public static event Action<AISound> OnSoundSpawned;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060008AF RID: 2223 RVA: 0x000270FC File Offset: 0x000252FC
	// (remove) Token: 0x060008B0 RID: 2224 RVA: 0x00027130 File Offset: 0x00025330
	public static event Action<AISound> OnPlayerHearSound;

	// Token: 0x060008B1 RID: 2225 RVA: 0x00027163 File Offset: 0x00025363
	public static void MakeSound(AISound sound)
	{
		Action<AISound> onSoundSpawned = AIMainBrain.OnSoundSpawned;
		if (onSoundSpawned != null)
		{
			onSoundSpawned(sound);
		}
		AIMainBrain.FilterPlayerHearSound(sound);
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0002717C File Offset: 0x0002537C
	private static void FilterPlayerHearSound(AISound sound)
	{
		if (!AIMainBrain.mainCharacter)
		{
			return;
		}
		if (!Team.IsEnemy(Teams.player, sound.fromTeam))
		{
			return;
		}
		if (sound.fromCharacter && sound.fromCharacter.characterModel && !sound.fromCharacter.characterModel.Hidden && !GameCamera.Instance.IsOffScreen(sound.pos))
		{
			return;
		}
		float num = Vector3.Distance(sound.pos, AIMainBrain.mainCharacter.transform.position);
		if (AIMainBrain.mainCharacter.SoundVisable < 0.2f)
		{
			return;
		}
		float hearingAbility = AIMainBrain.mainCharacter.HearingAbility;
		if (num > sound.radius * hearingAbility)
		{
			return;
		}
		Action<AISound> onPlayerHearSound = AIMainBrain.OnPlayerHearSound;
		if (onPlayerHearSound == null)
		{
			return;
		}
		onPlayerHearSound(sound);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002723D File Offset: 0x0002543D
	public void Awake()
	{
		this.searchTasks = new Queue<AIMainBrain.SearchTaskContext>();
		this.checkObsticleTasks = new Queue<AIMainBrain.CheckObsticleTaskContext>();
		this.fowBlockLayer = LayerMask.NameToLayer("FowBlock");
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00027268 File Offset: 0x00025468
	private void Start()
	{
		this.dmgReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
		this.interactLayers = 1 << LayerMask.NameToLayer("Interactable");
		this.obsticleLayers = GameplayDataSettings.Layers.fowBlockLayers;
		this.obsticleLayersWithThermal = GameplayDataSettings.Layers.fowBlockLayersWithThermal;
		this.cols = new Collider[15];
		this.ObsHits = new RaycastHit[15];
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x000272DC File Offset: 0x000254DC
	private void Update()
	{
		int num = 0;
		while (num < this.maxSeachCount && this.searchTasks.Count > 0)
		{
			this.DoSearch(this.searchTasks.Dequeue());
			num++;
		}
		int num2 = 0;
		while (num2 < this.maxCheckObsticleCount && this.checkObsticleTasks.Count > 0)
		{
			this.DoCheckObsticle(this.checkObsticleTasks.Dequeue());
			num2++;
		}
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0002734C File Offset: 0x0002554C
	private void DoSearch(AIMainBrain.SearchTaskContext context)
	{
		int num = Physics.OverlapSphereNonAlloc(context.searchCenter, context.searchDistance, this.cols, (context.searchPickupID > 0) ? (this.dmgReceiverLayers | this.interactLayers) : this.dmgReceiverLayers, 2);
		if (num <= 0)
		{
			context.onSearchFinishedCallback(null, null);
			return;
		}
		float num2 = 9999f;
		DamageReceiver arg = null;
		float num3 = 9999f;
		InteractablePickup arg2 = null;
		float num4 = 1.5f;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.cols[i];
			if (Mathf.Abs(context.searchCenter.y - collider.transform.position.y) <= 4f)
			{
				float num5 = Vector3.Distance(context.searchCenter, collider.transform.position);
				if (Vector3.Angle(context.searchDirection.normalized, (collider.transform.position - context.searchCenter).normalized) <= context.searchAngle * 0.5f || num5 <= num4)
				{
					this.dmgReceiverTemp = null;
					float num6 = 1f;
					if (ObjectUtils.IsInLayerMask(collider.gameObject, this.dmgReceiverLayers))
					{
						this.dmgReceiverTemp = collider.GetComponent<DamageReceiver>();
						if (this.dmgReceiverTemp != null && this.dmgReceiverTemp.health)
						{
							CharacterMainControl characterMainControl = this.dmgReceiverTemp.health.TryGetCharacter();
							if (characterMainControl)
							{
								num6 = characterMainControl.VisableDistanceFactor;
							}
						}
					}
					if (num5 <= context.searchDistance * num6 && (num5 < num2 || num5 < num3) && (!context.checkObsticle || num5 <= num4 || !this.CheckObsticle(context.searchCenter, collider.transform.position + Vector3.up * 1.5f, context.thermalOn, context.ignoreFowBlockLayer)))
					{
						if (this.dmgReceiverTemp)
						{
							if (!(this.dmgReceiverTemp.health == null) && Team.IsEnemy(context.selfTeam, this.dmgReceiverTemp.Team))
							{
								num2 = num5;
								arg = this.dmgReceiverTemp;
							}
						}
						else if (context.searchPickupID > 0)
						{
							InteractablePickup component = collider.GetComponent<InteractablePickup>();
							if (component && component.ItemAgent && component.ItemAgent.Item && component.ItemAgent.Item.TypeID == context.searchPickupID)
							{
								num3 = num5;
								arg2 = component;
							}
						}
					}
				}
			}
		}
		context.onSearchFinishedCallback(arg, arg2);
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00027604 File Offset: 0x00025804
	public void AddSearchTask(Vector3 center, Vector3 dir, float searchAngle, float searchDistance, Teams selfTeam, bool checkObsticle, bool thermalOn, bool ignoreFowBlockLayer, int searchPickupID, Action<DamageReceiver, InteractablePickup> callback)
	{
		AIMainBrain.SearchTaskContext item = new AIMainBrain.SearchTaskContext(center, dir, searchAngle, searchDistance, selfTeam, checkObsticle, thermalOn, ignoreFowBlockLayer, searchPickupID, callback);
		this.searchTasks.Enqueue(item);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00027638 File Offset: 0x00025838
	private void DoCheckObsticle(AIMainBrain.CheckObsticleTaskContext context)
	{
		bool obj = this.CheckObsticle(context.start, context.end, context.thermalOn, context.ignoreFowBlockLayer);
		context.onCheckFinishCallback(obj);
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00027670 File Offset: 0x00025870
	public void AddCheckObsticleTask(Vector3 start, Vector3 end, bool thermalOn, bool ignoreFowBlockLayer, Action<bool> callback)
	{
		AIMainBrain.CheckObsticleTaskContext item = new AIMainBrain.CheckObsticleTaskContext(start, end, thermalOn, ignoreFowBlockLayer, callback);
		this.checkObsticleTasks.Enqueue(item);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00027698 File Offset: 0x00025898
	private bool CheckObsticle(Vector3 startPoint, Vector3 endPoint, bool thermalOn, bool ignoreFowBlockLayer)
	{
		Ray ray = new Ray(startPoint, (endPoint - startPoint).normalized);
		LayerMask layerMask = thermalOn ? this.obsticleLayersWithThermal : this.obsticleLayers;
		if (ignoreFowBlockLayer)
		{
			layerMask &= ~(1 << this.fowBlockLayer);
		}
		return Physics.RaycastNonAlloc(ray, this.ObsHits, (endPoint - startPoint).magnitude, layerMask) > 0;
	}

	// Token: 0x040007EF RID: 2031
	private Queue<AIMainBrain.SearchTaskContext> searchTasks;

	// Token: 0x040007F0 RID: 2032
	private Queue<AIMainBrain.CheckObsticleTaskContext> checkObsticleTasks;

	// Token: 0x040007F1 RID: 2033
	private LayerMask dmgReceiverLayers;

	// Token: 0x040007F2 RID: 2034
	private LayerMask interactLayers;

	// Token: 0x040007F3 RID: 2035
	private LayerMask obsticleLayers;

	// Token: 0x040007F4 RID: 2036
	private LayerMask obsticleLayersWithThermal;

	// Token: 0x040007F5 RID: 2037
	private Collider[] cols;

	// Token: 0x040007F6 RID: 2038
	private RaycastHit[] ObsHits;

	// Token: 0x040007F7 RID: 2039
	public int maxSeachCount;

	// Token: 0x040007F8 RID: 2040
	public int maxCheckObsticleCount;

	// Token: 0x040007F9 RID: 2041
	private static CharacterMainControl _mc;

	// Token: 0x040007FC RID: 2044
	private int fowBlockLayer;

	// Token: 0x040007FD RID: 2045
	private DamageReceiver dmgReceiverTemp;

	// Token: 0x020004A3 RID: 1187
	public struct SearchTaskContext
	{
		// Token: 0x060027B7 RID: 10167 RVA: 0x0008D7F4 File Offset: 0x0008B9F4
		public SearchTaskContext(Vector3 center, Vector3 dir, float searchAngle, float searchDistance, Teams selfTeam, bool checkObsticle, bool thermalOn, bool ignoreFowBlockLayer, int searchPickupID, Action<DamageReceiver, InteractablePickup> callback)
		{
			this.searchCenter = center;
			this.searchDirection = dir;
			this.searchAngle = searchAngle;
			this.searchDistance = searchDistance;
			this.selfTeam = selfTeam;
			this.thermalOn = thermalOn;
			this.checkObsticle = checkObsticle;
			this.searchPickupID = searchPickupID;
			this.onSearchFinishedCallback = callback;
			this.ignoreFowBlockLayer = ignoreFowBlockLayer;
		}

		// Token: 0x04001C6F RID: 7279
		public Vector3 searchCenter;

		// Token: 0x04001C70 RID: 7280
		public Vector3 searchDirection;

		// Token: 0x04001C71 RID: 7281
		public float searchAngle;

		// Token: 0x04001C72 RID: 7282
		public float searchDistance;

		// Token: 0x04001C73 RID: 7283
		public Teams selfTeam;

		// Token: 0x04001C74 RID: 7284
		public bool checkObsticle;

		// Token: 0x04001C75 RID: 7285
		public bool thermalOn;

		// Token: 0x04001C76 RID: 7286
		public bool ignoreFowBlockLayer;

		// Token: 0x04001C77 RID: 7287
		public int searchPickupID;

		// Token: 0x04001C78 RID: 7288
		public Action<DamageReceiver, InteractablePickup> onSearchFinishedCallback;
	}

	// Token: 0x020004A4 RID: 1188
	public struct CheckObsticleTaskContext
	{
		// Token: 0x060027B8 RID: 10168 RVA: 0x0008D84E File Offset: 0x0008BA4E
		public CheckObsticleTaskContext(Vector3 start, Vector3 end, bool thermalOn, bool ignoreFowBlockLayer, Action<bool> onCheckFinishCallback)
		{
			this.start = start;
			this.end = end;
			this.thermalOn = thermalOn;
			this.onCheckFinishCallback = onCheckFinishCallback;
			this.ignoreFowBlockLayer = ignoreFowBlockLayer;
		}

		// Token: 0x04001C79 RID: 7289
		public Vector3 start;

		// Token: 0x04001C7A RID: 7290
		public Vector3 end;

		// Token: 0x04001C7B RID: 7291
		public bool thermalOn;

		// Token: 0x04001C7C RID: 7292
		public bool ignoreFowBlockLayer;

		// Token: 0x04001C7D RID: 7293
		public Action<bool> onCheckFinishCallback;
	}
}
