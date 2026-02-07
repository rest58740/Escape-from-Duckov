using System;
using System.Reflection;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x0200009E RID: 158
	public class WeakColorBlockFormatter : WeakBaseFormatter
	{
		// Token: 0x0600049A RID: 1178 RVA: 0x0002087C File Offset: 0x0001EA7C
		public WeakColorBlockFormatter(Type colorBlockType) : base(colorBlockType)
		{
			this.normalColor = colorBlockType.GetProperty("normalColor");
			this.highlightedColor = colorBlockType.GetProperty("highlightedColor");
			this.pressedColor = colorBlockType.GetProperty("pressedColor");
			this.disabledColor = colorBlockType.GetProperty("disabledColor");
			this.colorMultiplier = colorBlockType.GetProperty("colorMultiplier");
			this.fadeDuration = colorBlockType.GetProperty("fadeDuration");
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000208F8 File Offset: 0x0001EAF8
		protected override void DeserializeImplementation(ref object value, IDataReader reader)
		{
			this.normalColor.SetValue(value, WeakColorBlockFormatter.ColorSerializer.ReadValue(reader), null);
			this.highlightedColor.SetValue(value, WeakColorBlockFormatter.ColorSerializer.ReadValue(reader), null);
			this.pressedColor.SetValue(value, WeakColorBlockFormatter.ColorSerializer.ReadValue(reader), null);
			this.disabledColor.SetValue(value, WeakColorBlockFormatter.ColorSerializer.ReadValue(reader), null);
			this.colorMultiplier.SetValue(value, WeakColorBlockFormatter.FloatSerializer.ReadValue(reader), null);
			this.fadeDuration.SetValue(value, WeakColorBlockFormatter.FloatSerializer.ReadValue(reader), null);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000209BC File Offset: 0x0001EBBC
		protected override void SerializeImplementation(ref object value, IDataWriter writer)
		{
			WeakColorBlockFormatter.ColorSerializer.WriteValue((Color)this.normalColor.GetValue(value, null), writer);
			WeakColorBlockFormatter.ColorSerializer.WriteValue((Color)this.highlightedColor.GetValue(value, null), writer);
			WeakColorBlockFormatter.ColorSerializer.WriteValue((Color)this.pressedColor.GetValue(value, null), writer);
			WeakColorBlockFormatter.ColorSerializer.WriteValue((Color)this.disabledColor.GetValue(value, null), writer);
			WeakColorBlockFormatter.FloatSerializer.WriteValue((float)this.colorMultiplier.GetValue(value, null), writer);
			WeakColorBlockFormatter.FloatSerializer.WriteValue((float)this.fadeDuration.GetValue(value, null), writer);
		}

		// Token: 0x0400019C RID: 412
		private static readonly Serializer<float> FloatSerializer = Serializer.Get<float>();

		// Token: 0x0400019D RID: 413
		private static readonly Serializer<Color> ColorSerializer = Serializer.Get<Color>();

		// Token: 0x0400019E RID: 414
		private readonly PropertyInfo normalColor;

		// Token: 0x0400019F RID: 415
		private readonly PropertyInfo highlightedColor;

		// Token: 0x040001A0 RID: 416
		private readonly PropertyInfo pressedColor;

		// Token: 0x040001A1 RID: 417
		private readonly PropertyInfo disabledColor;

		// Token: 0x040001A2 RID: 418
		private readonly PropertyInfo colorMultiplier;

		// Token: 0x040001A3 RID: 419
		private readonly PropertyInfo fadeDuration;
	}
}
