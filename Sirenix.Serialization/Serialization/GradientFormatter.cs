using System;
using System.Reflection;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x020000A3 RID: 163
	public class GradientFormatter : MinimalBaseFormatter<Gradient>
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x00020C1B File Offset: 0x0001EE1B
		protected override Gradient GetUninitializedObject()
		{
			return new Gradient();
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00020C24 File Offset: 0x0001EE24
		protected override void Read(ref Gradient value, IDataReader reader)
		{
			value.alphaKeys = GradientFormatter.AlphaKeysSerializer.ReadValue(reader);
			value.colorKeys = GradientFormatter.ColorKeysSerializer.ReadValue(reader);
			string text;
			reader.PeekEntry(out text);
			if (text == "mode")
			{
				try
				{
					if (GradientFormatter.ModeProperty != null)
					{
						GradientFormatter.ModeProperty.SetValue(value, GradientFormatter.EnumSerializer.ReadValue(reader), null);
					}
					else
					{
						reader.SkipEntry();
					}
				}
				catch (Exception)
				{
					reader.Context.Config.DebugContext.LogWarning("Failed to read Gradient.mode, due to Unity's API disallowing setting of this member on other threads than the main thread. Gradient.mode value will have been lost.");
				}
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00020CC8 File Offset: 0x0001EEC8
		protected override void Write(ref Gradient value, IDataWriter writer)
		{
			GradientFormatter.AlphaKeysSerializer.WriteValue(value.alphaKeys, writer);
			GradientFormatter.ColorKeysSerializer.WriteValue(value.colorKeys, writer);
			if (GradientFormatter.ModeProperty != null)
			{
				try
				{
					GradientFormatter.EnumSerializer.WriteValue("mode", GradientFormatter.ModeProperty.GetValue(value, null), writer);
				}
				catch (Exception)
				{
					writer.Context.Config.DebugContext.LogWarning("Failed to write Gradient.mode, due to Unity's API disallowing setting of this member on other threads than the main thread. Gradient.mode will have been lost upon deserialization.");
				}
			}
		}

		// Token: 0x040001A8 RID: 424
		private static readonly Serializer<GradientAlphaKey[]> AlphaKeysSerializer = Serializer.Get<GradientAlphaKey[]>();

		// Token: 0x040001A9 RID: 425
		private static readonly Serializer<GradientColorKey[]> ColorKeysSerializer = Serializer.Get<GradientColorKey[]>();

		// Token: 0x040001AA RID: 426
		private static readonly PropertyInfo ModeProperty = typeof(Gradient).GetProperty("mode", 52);

		// Token: 0x040001AB RID: 427
		private static readonly Serializer<object> EnumSerializer = (GradientFormatter.ModeProperty != null) ? Serializer.Get<object>() : null;
	}
}
