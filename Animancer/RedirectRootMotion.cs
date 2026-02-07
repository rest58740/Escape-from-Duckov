using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200004C RID: 76
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/RedirectRootMotion_1")]
	[RequireComponent(typeof(Animator))]
	public abstract class RedirectRootMotion<T> : MonoBehaviour
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0000D2FF File Offset: 0x0000B4FF
		public ref Animator Animator
		{
			get
			{
				return ref this._Animator;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0000D307 File Offset: 0x0000B507
		public ref T Target
		{
			get
			{
				return ref this._Target;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0000D30F File Offset: 0x0000B50F
		public unsafe bool ApplyRootMotion
		{
			get
			{
				return *this.Target != null && *this.Animator != null && this.Animator->applyRootMotion;
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000D340 File Offset: 0x0000B540
		protected virtual void OnValidate()
		{
			base.TryGetComponent<Animator>(out this._Animator);
			if (this._Target == null)
			{
				this._Target = base.transform.parent.GetComponentInParent<T>();
			}
		}

		// Token: 0x060004B1 RID: 1201
		protected abstract void OnAnimatorMove();

		// Token: 0x040000CA RID: 202
		[SerializeField]
		[Tooltip("The Animator which provides the root motion")]
		private Animator _Animator;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		[Tooltip("The object which the root motion will be applied to")]
		private T _Target;
	}
}
