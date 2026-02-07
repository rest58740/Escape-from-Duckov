using System;
using System.Collections.Generic;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Input
{
	// Token: 0x02000014 RID: 20
	public class UserInputController : MonoBehaviour
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002738 File Offset: 0x00000938
		public UserInputConfig GetConfig()
		{
			return this.inputConfig;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002740 File Offset: 0x00000940
		protected virtual void Update()
		{
			if (this._floatsToInterpolate == null)
			{
				return;
			}
			foreach (ValueTuple<int, float, float> valueTuple in this._floatsToInterpolate)
			{
				float num = (float)this._inputProperties[valueTuple.Item1];
				if (Mathf.Approximately(num, valueTuple.Item3))
				{
					num = valueTuple.Item3;
				}
				else
				{
					float t = KMath.ExpDecayAlpha(Time.deltaTime, valueTuple.Item2);
					num = Mathf.LerpUnclamped(num, valueTuple.Item3, t);
				}
				this._inputProperties[valueTuple.Item1] = num;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027DC File Offset: 0x000009DC
		public virtual void Initialize()
		{
			this._inputProperties = new List<object>();
			this._inputPropertyMap = new Dictionary<string, int>();
			List<ValueTuple<int, float, float>> list = new List<ValueTuple<int, float, float>>();
			int num = 0;
			foreach (BoolProperty boolProperty in this.inputConfig.boolProperties)
			{
				this._inputProperties.Add(boolProperty.defaultValue);
				this._inputPropertyMap.TryAdd(boolProperty.name, num);
				num++;
			}
			foreach (IntProperty intProperty in this.inputConfig.intProperties)
			{
				this._inputProperties.Add(intProperty.defaultValue);
				this._inputPropertyMap.TryAdd(intProperty.name, num);
				num++;
			}
			foreach (FloatProperty floatProperty in this.inputConfig.floatProperties)
			{
				this._inputProperties.Add(floatProperty.defaultValue);
				this._inputPropertyMap.TryAdd(floatProperty.name, num);
				if (!Mathf.Approximately(floatProperty.interpolationSpeed, 0f))
				{
					list.Add(new ValueTuple<int, float, float>(num, floatProperty.interpolationSpeed, floatProperty.defaultValue));
				}
				num++;
			}
			if (list.Count > 0)
			{
				this._floatsToInterpolate = list.ToArray();
			}
			foreach (VectorProperty vectorProperty in this.inputConfig.vectorProperties)
			{
				this._inputProperties.Add(vectorProperty.defaultValue);
				this._inputPropertyMap.TryAdd(vectorProperty.name, num);
				num++;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A10 File Offset: 0x00000C10
		public int GetPropertyIndex(string propertyName)
		{
			int result;
			if (this._inputPropertyMap.TryGetValue(propertyName, out result))
			{
				return result;
			}
			return -1;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A30 File Offset: 0x00000C30
		public virtual void SetValue(string propertyName, object value)
		{
			this.SetValue(this.GetPropertyIndex(propertyName), value);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A40 File Offset: 0x00000C40
		public virtual T GetValue<T>(string propertyName)
		{
			return this.GetValue<T>(this.GetPropertyIndex(propertyName));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002A50 File Offset: 0x00000C50
		public virtual void SetValue(int propertyIndex, object value)
		{
			if (propertyIndex < 0 || propertyIndex > this._inputProperties.Count - 1)
			{
				return;
			}
			if (this._floatsToInterpolate != null)
			{
				int num = -1;
				for (int i = 0; i < this._floatsToInterpolate.Length; i++)
				{
					if (this._floatsToInterpolate[i].Item1 == propertyIndex)
					{
						num = i;
					}
				}
				if (num != -1)
				{
					ValueTuple<int, float, float> valueTuple = this._floatsToInterpolate[num];
					valueTuple.Item3 = (float)value;
					this._floatsToInterpolate[num] = valueTuple;
					return;
				}
			}
			this._inputProperties[propertyIndex] = value;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public virtual T GetValue<T>(int propertyIndex)
		{
			if (propertyIndex < 0 || propertyIndex > this._inputProperties.Count - 1)
			{
				return default(T);
			}
			return (T)((object)this._inputProperties[propertyIndex]);
		}

		// Token: 0x0400002D RID: 45
		[SerializeField]
		public UserInputConfig inputConfig;

		// Token: 0x0400002E RID: 46
		protected List<object> _inputProperties;

		// Token: 0x0400002F RID: 47
		protected Dictionary<string, int> _inputPropertyMap;

		// Token: 0x04000030 RID: 48
		protected ValueTuple<int, float, float>[] _floatsToInterpolate;
	}
}
