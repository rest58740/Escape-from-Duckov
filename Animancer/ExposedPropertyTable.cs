using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x0200004B RID: 75
	[AddComponentMenu("Animancer/Exposed Property Table")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/ExposedPropertyTable")]
	[DefaultExecutionOrder(-10000)]
	public class ExposedPropertyTable : MonoBehaviour
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x0000D265 File Offset: 0x0000B465
		protected virtual void Reset()
		{
			this.OnValidate();
			if (this._Director == null)
			{
				this._Director = base.gameObject.AddComponent<PlayableDirector>();
			}
			this._Director.enabled = false;
			this._Director.playOnAwake = false;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
		protected virtual void OnValidate()
		{
			base.gameObject.GetComponentInParentOrChildren(ref this._Animancer);
			base.gameObject.GetComponentInParentOrChildren(ref this._Director);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000D2CC File Offset: 0x0000B4CC
		protected virtual void Awake()
		{
			this._Animancer.Playable.Graph.SetResolver(this._Director);
		}

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		private AnimancerComponent _Animancer;

		// Token: 0x040000C9 RID: 201
		[SerializeField]
		private PlayableDirector _Director;
	}
}
