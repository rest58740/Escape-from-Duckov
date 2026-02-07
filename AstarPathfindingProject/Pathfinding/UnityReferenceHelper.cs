using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200016A RID: 362
	[ExecuteInEditMode]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/unityreferencehelper.html")]
	public class UnityReferenceHelper : MonoBehaviour
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x0003C86E File Offset: 0x0003AA6E
		public string GetGUID()
		{
			return this.guid;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0003C876 File Offset: 0x0003AA76
		public void Awake()
		{
			this.Reset();
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0003C880 File Offset: 0x0003AA80
		public void Reset()
		{
			if (string.IsNullOrEmpty(this.guid))
			{
				this.guid = Pathfinding.Util.Guid.NewGuid().ToString();
				Debug.Log("Created new GUID - " + this.guid, this);
				return;
			}
			if (base.gameObject.scene.name != null)
			{
				foreach (UnityReferenceHelper unityReferenceHelper in UnityCompatibility.FindObjectsByTypeUnsorted<UnityReferenceHelper>())
				{
					if (unityReferenceHelper != this && this.guid == unityReferenceHelper.guid)
					{
						this.guid = Pathfinding.Util.Guid.NewGuid().ToString();
						Debug.Log("Created new GUID - " + this.guid, this);
						return;
					}
				}
			}
		}

		// Token: 0x04000722 RID: 1826
		[HideInInspector]
		[SerializeField]
		private string guid;
	}
}
