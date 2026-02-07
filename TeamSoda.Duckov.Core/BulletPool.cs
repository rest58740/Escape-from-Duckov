using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x02000112 RID: 274
public class BulletPool : MonoBehaviour
{
	// Token: 0x06000982 RID: 2434 RVA: 0x0002AB0F File Offset: 0x00028D0F
	private void Awake()
	{
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0002AB11 File Offset: 0x00028D11
	public Projectile GetABullet(Projectile bulletPrefab)
	{
		return this.GetAPool(bulletPrefab).Get();
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0002AB20 File Offset: 0x00028D20
	private ObjectPool<Projectile> GetAPool(Projectile pfb)
	{
		ObjectPool<Projectile> result;
		if (this.pools.TryGetValue(pfb, out result))
		{
			return result;
		}
		ObjectPool<Projectile> objectPool = new ObjectPool<Projectile>(() => this.CreateABulletInPool(pfb), new Action<Projectile>(this.OnGetABulletInPool), new Action<Projectile>(this.OnBulletRelease), null, true, 10, 10000);
		this.pools.Add(pfb, objectPool);
		return objectPool;
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0002ABA0 File Offset: 0x00028DA0
	private Projectile CreateABulletInPool(Projectile pfb)
	{
		Projectile projectile = Object.Instantiate<Projectile>(pfb);
		projectile.transform.SetParent(base.transform);
		ObjectPool<Projectile> apool = this.GetAPool(pfb);
		projectile.SetPool(apool);
		return projectile;
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0002ABD3 File Offset: 0x00028DD3
	private void OnGetABulletInPool(Projectile bulletToGet)
	{
		bulletToGet.gameObject.SetActive(true);
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0002ABE1 File Offset: 0x00028DE1
	private void OnBulletRelease(Projectile bulletToGet)
	{
		bulletToGet.transform.SetParent(base.transform);
		bulletToGet.gameObject.SetActive(false);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0002AC00 File Offset: 0x00028E00
	public bool Release(Projectile instance, Projectile prefab)
	{
		ObjectPool<Projectile> objectPool;
		if (this.pools.TryGetValue(prefab, out objectPool))
		{
			objectPool.Release(prefab);
			return true;
		}
		return false;
	}

	// Token: 0x04000899 RID: 2201
	public Dictionary<Projectile, ObjectPool<Projectile>> pools = new Dictionary<Projectile, ObjectPool<Projectile>>();
}
