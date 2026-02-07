using System;
using System.Diagnostics;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000056 RID: 86
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/AnimancerTransitionAsset_1")]
	public class AnimancerTransitionAsset<TTransition> : AnimancerTransitionAssetBase where TTransition : ITransition
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0000DB7E File Offset: 0x0000BD7E
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0000DB86 File Offset: 0x0000BD86
		public TTransition Transition
		{
			get
			{
				return this._Transition;
			}
			set
			{
				this._Transition = value;
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000DB8F File Offset: 0x0000BD8F
		public override ITransition GetTransition()
		{
			return this._Transition;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0000DB9C File Offset: 0x0000BD9C
		public bool HasTransition
		{
			get
			{
				return this._Transition != null;
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000DBAC File Offset: 0x0000BDAC
		[Conditional("UNITY_ASSERTIONS")]
		private void AssertTransition()
		{
			if (this._Transition == null)
			{
				UnityEngine.Debug.LogError("'" + base.name + "' Transition is not assigned. HasTransition can be used to check without triggering this error.", this);
			}
		}

		// Token: 0x040000DB RID: 219
		[SerializeReference]
		private TTransition _Transition;
	}
}
