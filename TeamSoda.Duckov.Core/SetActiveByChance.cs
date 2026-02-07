using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class SetActiveByChance : MonoBehaviour
{
	// Token: 0x060005EF RID: 1519 RVA: 0x0001AB74 File Offset: 0x00018D74
	private void Awake()
	{
		bool flag = Random.Range(0f, 1f) < this.activeChange;
		if (this.saveInLevel && MultiSceneCore.Instance)
		{
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(this.keyCached, out obj) && obj is bool)
			{
				bool flag2 = (bool)obj;
				Debug.Log(string.Format("存在门存档信息：{0}", flag2));
				flag = flag2;
			}
			MultiSceneCore.Instance.inLevelData[this.keyCached] = flag;
		}
		base.gameObject.SetActive(flag);
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001AC14 File Offset: 0x00018E14
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int num = Mathf.RoundToInt(vector.x);
		int num2 = Mathf.RoundToInt(vector.y);
		int num3 = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int;
		vector3Int..ctor(num, num2, num3);
		return string.Format("Door_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x0400057C RID: 1404
	public bool saveInLevel;

	// Token: 0x0400057D RID: 1405
	private int keyCached;

	// Token: 0x0400057E RID: 1406
	[Range(0f, 1f)]
	public float activeChange = 0.5f;
}
