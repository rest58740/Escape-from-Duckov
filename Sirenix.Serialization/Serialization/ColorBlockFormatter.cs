using System;
using System.Reflection;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200009D RID: 157
	public class ColorBlockFormatter<T> : MinimalBaseFormatter<T>
	{
		// Token: 0x06000496 RID: 1174 RVA: 0x000205F8 File Offset: 0x0001E7F8
		protected override void Read(ref T value, IDataReader reader)
		{
			object obj = value;
			ColorBlockFormatter<T>.normalColor.SetValue(obj, ColorBlockFormatter<T>.ColorSerializer.ReadValue(reader), null);
			ColorBlockFormatter<T>.highlightedColor.SetValue(obj, ColorBlockFormatter<T>.ColorSerializer.ReadValue(reader), null);
			ColorBlockFormatter<T>.pressedColor.SetValue(obj, ColorBlockFormatter<T>.ColorSerializer.ReadValue(reader), null);
			ColorBlockFormatter<T>.disabledColor.SetValue(obj, ColorBlockFormatter<T>.ColorSerializer.ReadValue(reader), null);
			ColorBlockFormatter<T>.colorMultiplier.SetValue(obj, ColorBlockFormatter<T>.FloatSerializer.ReadValue(reader), null);
			ColorBlockFormatter<T>.fadeDuration.SetValue(obj, ColorBlockFormatter<T>.FloatSerializer.ReadValue(reader), null);
			value = (T)((object)obj);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000206C8 File Offset: 0x0001E8C8
		protected override void Write(ref T value, IDataWriter writer)
		{
			ColorBlockFormatter<T>.ColorSerializer.WriteValue((Color)ColorBlockFormatter<T>.normalColor.GetValue(value, null), writer);
			ColorBlockFormatter<T>.ColorSerializer.WriteValue((Color)ColorBlockFormatter<T>.highlightedColor.GetValue(value, null), writer);
			ColorBlockFormatter<T>.ColorSerializer.WriteValue((Color)ColorBlockFormatter<T>.pressedColor.GetValue(value, null), writer);
			ColorBlockFormatter<T>.ColorSerializer.WriteValue((Color)ColorBlockFormatter<T>.disabledColor.GetValue(value, null), writer);
			ColorBlockFormatter<T>.FloatSerializer.WriteValue((float)ColorBlockFormatter<T>.colorMultiplier.GetValue(value, null), writer);
			ColorBlockFormatter<T>.FloatSerializer.WriteValue((float)ColorBlockFormatter<T>.fadeDuration.GetValue(value, null), writer);
		}

		// Token: 0x04000194 RID: 404
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();

		// Token: 0x04000195 RID: 405
		private static readonly Serializer<Color> ColorSerializer = Serializer.Get<Color>();

		// Token: 0x04000196 RID: 406
		private static readonly PropertyInfo normalColor = typeof(T).GetProperty("normalColor");

		// Token: 0x04000197 RID: 407
		private static readonly PropertyInfo highlightedColor = typeof(T).GetProperty("highlightedColor");

		// Token: 0x04000198 RID: 408
		private static readonly PropertyInfo pressedColor = typeof(T).GetProperty("pressedColor");

		// Token: 0x04000199 RID: 409
		private static readonly PropertyInfo disabledColor = typeof(T).GetProperty("disabledColor");

		// Token: 0x0400019A RID: 410
		private static readonly PropertyInfo colorMultiplier = typeof(T).GetProperty("colorMultiplier");

		// Token: 0x0400019B RID: 411
		private static readonly PropertyInfo fadeDuration = typeof(T).GetProperty("fadeDuration");
	}
}
