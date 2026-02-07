using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x0200002B RID: 43
	[ExecuteInEditMode]
	public class GlobalBlackboard : Blackboard, IGlobalBlackboard, IBlackboard
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00007E4D File Offset: 0x0000604D
		public string identifier
		{
			get
			{
				return this._identifier;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00007E55 File Offset: 0x00006055
		public string UID
		{
			get
			{
				return this._UID;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00007E5D File Offset: 0x0000605D
		public new string name
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00007E65 File Offset: 0x00006065
		public static IEnumerable<GlobalBlackboard> GetAll()
		{
			return GlobalBlackboard._allGlobals;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00007E6C File Offset: 0x0000606C
		public static GlobalBlackboard Create()
		{
			GlobalBlackboard globalBlackboard = new GameObject("@GlobalBlackboard").AddComponent<GlobalBlackboard>();
			globalBlackboard._identifier = "Global";
			return globalBlackboard;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00007E88 File Offset: 0x00006088
		public static GlobalBlackboard Find(string name)
		{
			return GlobalBlackboard._allGlobals.Find((GlobalBlackboard b) => b.identifier == name);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00007EB8 File Offset: 0x000060B8
		protected void OnEnable()
		{
			if (this.IsPrefabAsset())
			{
				return;
			}
			if (string.IsNullOrEmpty(this._identifier))
			{
				this._identifier = base.gameObject.name;
			}
			if (Application.isPlaying)
			{
				if (GlobalBlackboard.Find(this.identifier) != null)
				{
					if (this._singletonMode == GlobalBlackboard.SingletonMode.DestroyComponentOnly)
					{
						Object.Destroy(this);
					}
					if (this._singletonMode == GlobalBlackboard.SingletonMode.DestroyEntireGameObject)
					{
						Object.Destroy(base.gameObject);
					}
					return;
				}
				if (this._dontDestroyOnLoad)
				{
					Object.DontDestroyOnLoad(base.gameObject);
				}
				this.InitializePropertiesBinding(((IBlackboard)this).propertiesBindTarget, false);
			}
			if (!GlobalBlackboard._allGlobals.Contains(this))
			{
				GlobalBlackboard._allGlobals.Add(this);
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00007F61 File Offset: 0x00006161
		protected void OnDisable()
		{
			if (this.IsPrefabAsset())
			{
				return;
			}
			GlobalBlackboard._allGlobals.Remove(this);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00007F78 File Offset: 0x00006178
		protected override void OnValidate()
		{
			base.OnValidate();
			if (Application.isPlaying || this.IsPrefabAsset())
			{
				return;
			}
			if (!GlobalBlackboard._allGlobals.Contains(this))
			{
				GlobalBlackboard._allGlobals.Add(this);
			}
			if (string.IsNullOrEmpty(this._identifier))
			{
				this._identifier = base.gameObject.name;
			}
			GlobalBlackboard x = GlobalBlackboard.Find(this.identifier);
			if (x != this)
			{
				x != null;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00007FEE File Offset: 0x000061EE
		public override string ToString()
		{
			return this.identifier;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00007FF6 File Offset: 0x000061F6
		private bool IsPrefabAsset()
		{
			return false;
		}

		// Token: 0x04000093 RID: 147
		[SerializeField]
		private string _UID = Guid.NewGuid().ToString();

		// Token: 0x04000094 RID: 148
		[Tooltip("A *unique* identifier of this Global Blackboard")]
		[SerializeField]
		private string _identifier;

		// Token: 0x04000095 RID: 149
		[Tooltip("If a duplicate with the same identifier is encountered, destroy the previous Global Blackboard component only, or the previous Global Blackboard gameobject entirely?")]
		[SerializeField]
		private GlobalBlackboard.SingletonMode _singletonMode;

		// Token: 0x04000096 RID: 150
		[Tooltip("If true, the Global Blackboard will not be destroyed when another scene is loaded.")]
		[SerializeField]
		private bool _dontDestroyOnLoad = true;

		// Token: 0x04000097 RID: 151
		private static List<GlobalBlackboard> _allGlobals = new List<GlobalBlackboard>();

		// Token: 0x0200010E RID: 270
		public enum SingletonMode
		{
			// Token: 0x040002A8 RID: 680
			DestroyComponentOnly,
			// Token: 0x040002A9 RID: 681
			DestroyEntireGameObject
		}
	}
}
