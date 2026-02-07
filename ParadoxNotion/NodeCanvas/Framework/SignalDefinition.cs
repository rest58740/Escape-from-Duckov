using System;
using System.Collections.Generic;
using ParadoxNotion;
using UnityEngine;

namespace NodeCanvas.Framework
{
	// Token: 0x02000007 RID: 7
	[CreateAssetMenu(menuName = "ParadoxNotion/CanvasCore/Signal Definition")]
	public class SignalDefinition : ScriptableObject
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000006 RID: 6 RVA: 0x000020E4 File Offset: 0x000002E4
		// (remove) Token: 0x06000007 RID: 7 RVA: 0x0000211C File Offset: 0x0000031C
		public event SignalDefinition.InvokeArguments onInvoke;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002151 File Offset: 0x00000351
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002159 File Offset: 0x00000359
		public List<DynamicParameterDefinition> parameters
		{
			get
			{
				return this._parameters;
			}
			private set
			{
				this._parameters = value;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002162 File Offset: 0x00000362
		public void Invoke(Transform sender, Transform receiver, bool isGlobal, params object[] args)
		{
			if (this.onInvoke != null)
			{
				this.onInvoke(sender, receiver, isGlobal, args);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000217C File Offset: 0x0000037C
		public void AddParameter(string name, Type type)
		{
			DynamicParameterDefinition dynamicParameterDefinition = new DynamicParameterDefinition(name, type);
			this._parameters.Add(dynamicParameterDefinition);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021A0 File Offset: 0x000003A0
		public void RemoveParameter(string name)
		{
			DynamicParameterDefinition dynamicParameterDefinition = this._parameters.Find((DynamicParameterDefinition p) => p.name == name);
			if (dynamicParameterDefinition != null)
			{
				this._parameters.Remove(dynamicParameterDefinition);
			}
		}

		// Token: 0x04000010 RID: 16
		[SerializeField]
		[HideInInspector]
		private List<DynamicParameterDefinition> _parameters = new List<DynamicParameterDefinition>();

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x0600077A RID: 1914
		public delegate void InvokeArguments(Transform sender, Transform receiver, bool isGlobal, params object[] args);
	}
}
