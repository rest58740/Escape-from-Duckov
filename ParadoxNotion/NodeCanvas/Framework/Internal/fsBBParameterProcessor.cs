using System;
using System.Collections.Generic;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Services;

namespace NodeCanvas.Framework.Internal
{
	// Token: 0x0200004D RID: 77
	public class fsBBParameterProcessor : fsRecoveryProcessor<BBParameter, MissingBBParameterType>
	{
		// Token: 0x060003AA RID: 938 RVA: 0x0000A2E8 File Offset: 0x000084E8
		public override void OnBeforeDeserializeAfterInstanceCreation(Type storageType, object instance, ref fsData data)
		{
			if (Threader.applicationIsPlaying)
			{
				return;
			}
			if (data.IsDictionary)
			{
				Dictionary<string, fsData> asDictionary = data.AsDictionary;
				if (asDictionary.Count == 0 || asDictionary.ContainsKey("_value") || asDictionary.ContainsKey("_name"))
				{
					return;
				}
			}
			BBParameter bbparameter = instance as BBParameter;
			if (bbparameter != null && bbparameter.GetType().RTIsGenericType())
			{
				Type varType = bbparameter.varType;
				fsSerializer fsSerializer = new fsSerializer();
				object obj = null;
				if (fsSerializer.TryDeserialize(data, varType, ref obj).Succeeded && obj != null && varType.RTIsAssignableFrom(obj.GetType()))
				{
					bbparameter.value = obj;
					fsSerializer.TrySerialize(storageType, instance, out data);
				}
			}
		}
	}
}
