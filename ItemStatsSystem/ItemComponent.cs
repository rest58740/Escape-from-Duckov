using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000C RID: 12
	public class ItemComponent : MonoBehaviour, ISelfValidator
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000302A File Offset: 0x0000122A
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003032 File Offset: 0x00001232
		public Item Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000303B File Offset: 0x0000123B
		private void Awake()
		{
			if (!this.initialized && this.Master != null)
			{
				this.Initialize();
			}
			this.OnAwake();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000305F File Offset: 0x0000125F
		private void Start()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000306F File Offset: 0x0000126F
		protected virtual void OnAwake()
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003071 File Offset: 0x00001271
		internal void Initialize()
		{
			this.initialized = true;
			if (this.master == null)
			{
				this.master = base.GetComponent<Item>();
			}
			this.OnInitialize();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000309A File Offset: 0x0000129A
		internal virtual void OnInitialize()
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000309C File Offset: 0x0000129C
		public void Validate(SelfValidationResult result)
		{
			if (this.Master == null)
			{
				result.AddError("这个组件依赖Item，Item不可以留空。").WithFix("设置为本Game Object上的Item", delegate()
				{
					this.master = base.GetComponent<Item>();
				}, true);
				return;
			}
			if (this.Master.gameObject != base.gameObject)
			{
				result.AddError("Master需要和本组件处于同一个Game Object上。").WithFix("设置为本Game Object上的Item", delegate()
				{
					this.master = base.GetComponent<Item>();
				}, true);
			}
		}

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private Item master;

		// Token: 0x04000029 RID: 41
		private bool initialized;
	}
}
