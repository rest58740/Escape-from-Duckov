using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000B3 RID: 179
	public sealed class UnityReferenceResolver : IExternalIndexReferenceResolver, ICacheNotificationReceiver
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x0002151D File Offset: 0x0001F71D
		public UnityReferenceResolver()
		{
			this.referencedUnityObjects = new List<Object>();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00021542 File Offset: 0x0001F742
		public UnityReferenceResolver(List<Object> referencedUnityObjects)
		{
			this.SetReferencedUnityObjects(referencedUnityObjects);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00021563 File Offset: 0x0001F763
		public List<Object> GetReferencedUnityObjects()
		{
			return this.referencedUnityObjects;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0002156C File Offset: 0x0001F76C
		public void SetReferencedUnityObjects(List<Object> referencedUnityObjects)
		{
			if (referencedUnityObjects == null)
			{
				referencedUnityObjects = new List<Object>();
			}
			this.referencedUnityObjects = referencedUnityObjects;
			this.referenceIndexMapping.Clear();
			for (int i = 0; i < this.referencedUnityObjects.Count; i++)
			{
				if (this.referencedUnityObjects[i] != null && !this.referenceIndexMapping.ContainsKey(this.referencedUnityObjects[i]))
				{
					this.referenceIndexMapping.Add(this.referencedUnityObjects[i], i);
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000215EC File Offset: 0x0001F7EC
		public bool CanReference(object value, out int index)
		{
			if (this.referencedUnityObjects == null)
			{
				this.referencedUnityObjects = new List<Object>(32);
			}
			Object @object = value as Object;
			if (@object != null)
			{
				if (!this.referenceIndexMapping.TryGetValue(@object, ref index))
				{
					index = this.referencedUnityObjects.Count;
					this.referenceIndexMapping.Add(@object, index);
					this.referencedUnityObjects.Add(@object);
				}
				return true;
			}
			index = -1;
			return false;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00021654 File Offset: 0x0001F854
		public bool TryResolveReference(int index, out object value)
		{
			if (this.referencedUnityObjects == null || index < 0 || index >= this.referencedUnityObjects.Count)
			{
				value = null;
				return true;
			}
			value = this.referencedUnityObjects[index];
			return true;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00021684 File Offset: 0x0001F884
		public void Reset()
		{
			this.referencedUnityObjects = null;
			this.referenceIndexMapping.Clear();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00021698 File Offset: 0x0001F898
		void ICacheNotificationReceiver.OnFreed()
		{
			this.Reset();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000021B8 File Offset: 0x000003B8
		void ICacheNotificationReceiver.OnClaimed()
		{
		}

		// Token: 0x040001C3 RID: 451
		private Dictionary<Object, int> referenceIndexMapping = new Dictionary<Object, int>(32, ReferenceEqualityComparer<Object>.Default);

		// Token: 0x040001C4 RID: 452
		private List<Object> referencedUnityObjects;
	}
}
