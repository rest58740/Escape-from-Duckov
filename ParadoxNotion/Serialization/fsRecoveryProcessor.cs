using System;
using System.Collections.Generic;
using System.Linq;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Services;

namespace ParadoxNotion.Serialization
{
	// Token: 0x02000086 RID: 134
	public class fsRecoveryProcessor<TCanProcess, TMissing> : fsObjectProcessor where TMissing : TCanProcess, IMissingRecoverable
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x00010053 File Offset: 0x0000E253
		public override bool CanProcess(Type type)
		{
			return typeof(TCanProcess).RTIsAssignableFrom(type);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00010068 File Offset: 0x0000E268
		public override void OnBeforeDeserialize(Type storageType, ref fsData data)
		{
			if (Threader.applicationIsPlaying)
			{
				return;
			}
			if (!data.IsDictionary)
			{
				return;
			}
			Dictionary<string, fsData> json = data.AsDictionary;
			fsData fsData;
			if (json.TryGetValue("$type", ref fsData))
			{
				Type type = ReflectionTools.GetType(fsData.AsString, storageType);
				if (type == null)
				{
					string asString = fsData.AsString;
					string str = fsJsonPrinter.PrettyJson(data);
					json["_missingType"] = new fsData(asString);
					json["_recoveryState"] = new fsData(str);
					json["$type"] = new fsData(typeof(TMissing).FullName);
				}
				if (type == typeof(TMissing))
				{
					Type type2 = ReflectionTools.GetType(json["_missingType"].AsString, storageType);
					if (type2 != null)
					{
						Dictionary<string, fsData> asDictionary = fsJsonParser.Parse(json["_recoveryState"].AsString).AsDictionary;
						json = json.Concat(from kvp in asDictionary
						where !json.ContainsKey(kvp.Key)
						select kvp).ToDictionary((KeyValuePair<string, fsData> c) => c.Key, (KeyValuePair<string, fsData> c) => c.Value);
						json["$type"] = new fsData(type2.FullName);
						data = new fsData(json);
					}
				}
			}
		}

		// Token: 0x040001BB RID: 443
		private const string FIELD_NAME_TYPE = "_missingType";

		// Token: 0x040001BC RID: 444
		private const string FIELD_NAME_STATE = "_recoveryState";
	}
}
