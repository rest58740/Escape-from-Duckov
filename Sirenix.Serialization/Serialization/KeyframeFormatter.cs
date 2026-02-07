using System;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A4 RID: 164
	public class KeyframeFormatter : MinimalBaseFormatter<Keyframe>
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x00020DB4 File Offset: 0x0001EFB4
		static KeyframeFormatter()
		{
			if (KeyframeFormatter.Is_In_2018_1_Or_Above)
			{
				if (EmitUtilities.CanEmit)
				{
					KeyframeFormatter.Formatter = (IFormatter<Keyframe>)FormatterEmitter.GetEmittedFormatter(typeof(Keyframe), SerializationPolicies.Everything);
					return;
				}
				KeyframeFormatter.Formatter = new ReflectionFormatter<Keyframe>(SerializationPolicies.Everything);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00020E30 File Offset: 0x0001F030
		protected override void Read(ref Keyframe value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.Integer && text == "ver")
			{
				if (KeyframeFormatter.Formatter == null)
				{
					KeyframeFormatter.Formatter = new ReflectionFormatter<Keyframe>(SerializationPolicies.Everything);
				}
				int num;
				reader.ReadInt32(out num);
				value = KeyframeFormatter.Formatter.Deserialize(reader);
				return;
			}
			value.inTangent = KeyframeFormatter.FloatSerializer.ReadValue(reader);
			value.outTangent = KeyframeFormatter.FloatSerializer.ReadValue(reader);
			value.time = KeyframeFormatter.FloatSerializer.ReadValue(reader);
			value.value = KeyframeFormatter.FloatSerializer.ReadValue(reader);
			value.tangentMode = KeyframeFormatter.IntSerializer.ReadValue(reader);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00020EE0 File Offset: 0x0001F0E0
		protected override void Write(ref Keyframe value, IDataWriter writer)
		{
			if (KeyframeFormatter.Is_In_2018_1_Or_Above)
			{
				writer.WriteInt32("ver", 1);
				KeyframeFormatter.Formatter.Serialize(value, writer);
				return;
			}
			KeyframeFormatter.FloatSerializer.WriteValue(value.inTangent, writer);
			KeyframeFormatter.FloatSerializer.WriteValue(value.outTangent, writer);
			KeyframeFormatter.FloatSerializer.WriteValue(value.time, writer);
			KeyframeFormatter.FloatSerializer.WriteValue(value.value, writer);
			KeyframeFormatter.IntSerializer.WriteValue(value.tangentMode, writer);
		}

		// Token: 0x040001AC RID: 428
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();

		// Token: 0x040001AD RID: 429
		private static readonly Serializer<int> IntSerializer = Serializer.Get<int>();

		// Token: 0x040001AE RID: 430
		private static readonly bool Is_In_2018_1_Or_Above = typeof(Keyframe).GetProperty("weightedMode") != null;

		// Token: 0x040001AF RID: 431
		private static IFormatter<Keyframe> Formatter;
	}
}
