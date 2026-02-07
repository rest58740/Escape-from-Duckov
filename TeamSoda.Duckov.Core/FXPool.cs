using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x020001C1 RID: 449
public class FXPool : MonoBehaviour
{
	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00038CA0 File Offset: 0x00036EA0
	// (set) Token: 0x06000D79 RID: 3449 RVA: 0x00038CA7 File Offset: 0x00036EA7
	public static FXPool Instance { get; private set; }

	// Token: 0x06000D7A RID: 3450 RVA: 0x00038CAF File Offset: 0x00036EAF
	private void Awake()
	{
		FXPool.Instance = this;
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x00038CB8 File Offset: 0x00036EB8
	private void FixedUpdate()
	{
		if (this.poolsDic == null)
		{
			return;
		}
		foreach (FXPool.Pool pool in this.poolsDic.Values)
		{
			pool.Tick();
		}
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x00038D18 File Offset: 0x00036F18
	private FXPool.Pool GetOrCreatePool(ParticleSystem prefab)
	{
		if (this.poolsDic == null)
		{
			this.poolsDic = new Dictionary<ParticleSystem, FXPool.Pool>();
		}
		FXPool.Pool result;
		if (this.poolsDic.TryGetValue(prefab, out result))
		{
			return result;
		}
		FXPool.Pool pool = new FXPool.Pool(prefab, base.transform, null, null, null, null, true, 10, 100);
		this.poolsDic[prefab] = pool;
		return pool;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00038D6E File Offset: 0x00036F6E
	private static ParticleSystem Get(ParticleSystem prefab)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		return FXPool.Instance.GetOrCreatePool(prefab).Get();
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x00038D90 File Offset: 0x00036F90
	public static ParticleSystem Play(ParticleSystem prefab, Vector3 postion, Quaternion rotation)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		if (prefab == null)
		{
			return null;
		}
		ParticleSystem particleSystem = FXPool.Get(prefab);
		particleSystem.transform.position = postion;
		particleSystem.transform.rotation = rotation;
		particleSystem.gameObject.SetActive(true);
		particleSystem.Play();
		return particleSystem;
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x00038DE8 File Offset: 0x00036FE8
	public static ParticleSystem Play(ParticleSystem prefab, Vector3 postion, Quaternion rotation, Color color)
	{
		if (FXPool.Instance == null)
		{
			return null;
		}
		if (prefab == null)
		{
			return null;
		}
		ParticleSystem particleSystem = FXPool.Get(prefab);
		particleSystem.transform.position = postion;
		particleSystem.transform.rotation = rotation;
		particleSystem.gameObject.SetActive(true);
		particleSystem.main.startColor = color;
		particleSystem.Play();
		return particleSystem;
	}

	// Token: 0x04000BB8 RID: 3000
	private Dictionary<ParticleSystem, FXPool.Pool> poolsDic;

	// Token: 0x020004EE RID: 1262
	private class Pool
	{
		// Token: 0x06002834 RID: 10292 RVA: 0x000927D0 File Offset: 0x000909D0
		public Pool(ParticleSystem prefab, Transform parent, Action<ParticleSystem> onCreate = null, Action<ParticleSystem> onGet = null, Action<ParticleSystem> onRelease = null, Action<ParticleSystem> onDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 100)
		{
			this.prefab = prefab;
			this.parent = parent;
			this.pool = new ObjectPool<ParticleSystem>(new Func<ParticleSystem>(this.Create), new Action<ParticleSystem>(this.OnEntryGet), new Action<ParticleSystem>(this.OnEntryRelease), new Action<ParticleSystem>(this.OnEntryDestroy), collectionCheck, defaultCapacity, maxSize);
			this.onCreate = onCreate;
			this.onGet = onGet;
			this.onRelease = onRelease;
			this.onDestroy = onDestroy;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x0009285C File Offset: 0x00090A5C
		private ParticleSystem Create()
		{
			ParticleSystem particleSystem = Object.Instantiate<ParticleSystem>(this.prefab, this.parent);
			Action<ParticleSystem> action = this.onCreate;
			if (action != null)
			{
				action(particleSystem);
			}
			return particleSystem;
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x0009288E File Offset: 0x00090A8E
		public void OnEntryGet(ParticleSystem obj)
		{
			this.activeEntries.Add(obj);
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x0009289C File Offset: 0x00090A9C
		public void OnEntryRelease(ParticleSystem obj)
		{
			this.activeEntries.Remove(obj);
			obj.gameObject.SetActive(false);
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000928B7 File Offset: 0x00090AB7
		public void OnEntryDestroy(ParticleSystem obj)
		{
			Action<ParticleSystem> action = this.onDestroy;
			if (action == null)
			{
				return;
			}
			action(obj);
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000928CA File Offset: 0x00090ACA
		public ParticleSystem Get()
		{
			return this.pool.Get();
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000928D7 File Offset: 0x00090AD7
		public void Release(ParticleSystem obj)
		{
			this.pool.Release(obj);
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000928E8 File Offset: 0x00090AE8
		public void Tick()
		{
			List<ParticleSystem> list = new List<ParticleSystem>();
			foreach (ParticleSystem particleSystem in this.activeEntries)
			{
				if (!particleSystem.isPlaying)
				{
					list.Add(particleSystem);
				}
			}
			foreach (ParticleSystem obj in list)
			{
				this.Release(obj);
			}
		}

		// Token: 0x04001DB6 RID: 7606
		private ParticleSystem prefab;

		// Token: 0x04001DB7 RID: 7607
		private Transform parent;

		// Token: 0x04001DB8 RID: 7608
		private ObjectPool<ParticleSystem> pool;

		// Token: 0x04001DB9 RID: 7609
		private Action<ParticleSystem> onCreate;

		// Token: 0x04001DBA RID: 7610
		private Action<ParticleSystem> onGet;

		// Token: 0x04001DBB RID: 7611
		private Action<ParticleSystem> onRelease;

		// Token: 0x04001DBC RID: 7612
		private Action<ParticleSystem> onDestroy;

		// Token: 0x04001DBD RID: 7613
		private List<ParticleSystem> activeEntries = new List<ParticleSystem>();
	}
}
