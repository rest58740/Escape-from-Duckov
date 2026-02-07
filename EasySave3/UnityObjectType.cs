using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class UnityObjectType : MonoBehaviour
{
	// Token: 0x06000150 RID: 336 RVA: 0x00005BA8 File Offset: 0x00003DA8
	private void Start()
	{
		if (!ES3.KeyExists("this"))
		{
			ES3.Save<UnityObjectType>("this", this);
		}
		else
		{
			ES3.LoadInto<UnityObjectType>("this", this);
		}
		foreach (UnityEngine.Object message in this.objs)
		{
			Debug.Log(message);
		}
	}

	// Token: 0x04000049 RID: 73
	public List<UnityEngine.Object> objs;
}
