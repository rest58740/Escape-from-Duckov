using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParadoxNotion.Serialization.FullSerializer.Internal.DirectConverters
{
	// Token: 0x020000BF RID: 191
	public class GUIStyle_DirectConverter : fsDirectConverter<GUIStyle>
	{
		// Token: 0x060006FE RID: 1790 RVA: 0x00015B60 File Offset: 0x00013D60
		protected override fsResult DoSerialize(GUIStyle model, Dictionary<string, fsData> serialized)
		{
			return fsResult.Success + base.SerializeMember<GUIStyleState>(serialized, null, "active", model.active) + base.SerializeMember<TextAnchor>(serialized, null, "alignment", model.alignment) + base.SerializeMember<RectOffset>(serialized, null, "border", model.border) + base.SerializeMember<TextClipping>(serialized, null, "clipping", model.clipping) + base.SerializeMember<Vector2>(serialized, null, "contentOffset", model.contentOffset) + base.SerializeMember<float>(serialized, null, "fixedHeight", model.fixedHeight) + base.SerializeMember<float>(serialized, null, "fixedWidth", model.fixedWidth) + base.SerializeMember<GUIStyleState>(serialized, null, "focused", model.focused) + base.SerializeMember<Font>(serialized, null, "font", model.font) + base.SerializeMember<int>(serialized, null, "fontSize", model.fontSize) + base.SerializeMember<FontStyle>(serialized, null, "fontStyle", model.fontStyle) + base.SerializeMember<GUIStyleState>(serialized, null, "hover", model.hover) + base.SerializeMember<ImagePosition>(serialized, null, "imagePosition", model.imagePosition) + base.SerializeMember<RectOffset>(serialized, null, "margin", model.margin) + base.SerializeMember<string>(serialized, null, "name", model.name) + base.SerializeMember<GUIStyleState>(serialized, null, "normal", model.normal) + base.SerializeMember<GUIStyleState>(serialized, null, "onActive", model.onActive) + base.SerializeMember<GUIStyleState>(serialized, null, "onFocused", model.onFocused) + base.SerializeMember<GUIStyleState>(serialized, null, "onHover", model.onHover) + base.SerializeMember<GUIStyleState>(serialized, null, "onNormal", model.onNormal) + base.SerializeMember<RectOffset>(serialized, null, "overflow", model.overflow) + base.SerializeMember<RectOffset>(serialized, null, "padding", model.padding) + base.SerializeMember<bool>(serialized, null, "richText", model.richText) + base.SerializeMember<bool>(serialized, null, "stretchHeight", model.stretchHeight) + base.SerializeMember<bool>(serialized, null, "stretchWidth", model.stretchWidth) + base.SerializeMember<bool>(serialized, null, "wordWrap", model.wordWrap);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00015DE4 File Offset: 0x00013FE4
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref GUIStyle model)
		{
			fsResult success = fsResult.Success;
			GUIStyleState active = model.active;
			fsResult a = success + base.DeserializeMember<GUIStyleState>(data, null, "active", out active);
			model.active = active;
			TextAnchor alignment = model.alignment;
			fsResult a2 = a + base.DeserializeMember<TextAnchor>(data, null, "alignment", out alignment);
			model.alignment = alignment;
			RectOffset border = model.border;
			fsResult a3 = a2 + base.DeserializeMember<RectOffset>(data, null, "border", out border);
			model.border = border;
			TextClipping clipping = model.clipping;
			fsResult a4 = a3 + base.DeserializeMember<TextClipping>(data, null, "clipping", out clipping);
			model.clipping = clipping;
			Vector2 contentOffset = model.contentOffset;
			fsResult a5 = a4 + base.DeserializeMember<Vector2>(data, null, "contentOffset", out contentOffset);
			model.contentOffset = contentOffset;
			float fixedHeight = model.fixedHeight;
			fsResult a6 = a5 + base.DeserializeMember<float>(data, null, "fixedHeight", out fixedHeight);
			model.fixedHeight = fixedHeight;
			float fixedWidth = model.fixedWidth;
			fsResult a7 = a6 + base.DeserializeMember<float>(data, null, "fixedWidth", out fixedWidth);
			model.fixedWidth = fixedWidth;
			GUIStyleState focused = model.focused;
			fsResult a8 = a7 + base.DeserializeMember<GUIStyleState>(data, null, "focused", out focused);
			model.focused = focused;
			Font font = model.font;
			fsResult a9 = a8 + base.DeserializeMember<Font>(data, null, "font", out font);
			model.font = font;
			int fontSize = model.fontSize;
			fsResult a10 = a9 + base.DeserializeMember<int>(data, null, "fontSize", out fontSize);
			model.fontSize = fontSize;
			FontStyle fontStyle = model.fontStyle;
			fsResult a11 = a10 + base.DeserializeMember<FontStyle>(data, null, "fontStyle", out fontStyle);
			model.fontStyle = fontStyle;
			GUIStyleState hover = model.hover;
			fsResult a12 = a11 + base.DeserializeMember<GUIStyleState>(data, null, "hover", out hover);
			model.hover = hover;
			ImagePosition imagePosition = model.imagePosition;
			fsResult a13 = a12 + base.DeserializeMember<ImagePosition>(data, null, "imagePosition", out imagePosition);
			model.imagePosition = imagePosition;
			RectOffset margin = model.margin;
			fsResult a14 = a13 + base.DeserializeMember<RectOffset>(data, null, "margin", out margin);
			model.margin = margin;
			string name = model.name;
			fsResult a15 = a14 + base.DeserializeMember<string>(data, null, "name", out name);
			model.name = name;
			GUIStyleState normal = model.normal;
			fsResult a16 = a15 + base.DeserializeMember<GUIStyleState>(data, null, "normal", out normal);
			model.normal = normal;
			GUIStyleState onActive = model.onActive;
			fsResult a17 = a16 + base.DeserializeMember<GUIStyleState>(data, null, "onActive", out onActive);
			model.onActive = onActive;
			GUIStyleState onFocused = model.onFocused;
			fsResult a18 = a17 + base.DeserializeMember<GUIStyleState>(data, null, "onFocused", out onFocused);
			model.onFocused = onFocused;
			GUIStyleState onHover = model.onHover;
			fsResult a19 = a18 + base.DeserializeMember<GUIStyleState>(data, null, "onHover", out onHover);
			model.onHover = onHover;
			GUIStyleState onNormal = model.onNormal;
			fsResult a20 = a19 + base.DeserializeMember<GUIStyleState>(data, null, "onNormal", out onNormal);
			model.onNormal = onNormal;
			RectOffset overflow = model.overflow;
			fsResult a21 = a20 + base.DeserializeMember<RectOffset>(data, null, "overflow", out overflow);
			model.overflow = overflow;
			RectOffset padding = model.padding;
			fsResult a22 = a21 + base.DeserializeMember<RectOffset>(data, null, "padding", out padding);
			model.padding = padding;
			bool richText = model.richText;
			fsResult a23 = a22 + base.DeserializeMember<bool>(data, null, "richText", out richText);
			model.richText = richText;
			bool stretchHeight = model.stretchHeight;
			fsResult a24 = a23 + base.DeserializeMember<bool>(data, null, "stretchHeight", out stretchHeight);
			model.stretchHeight = stretchHeight;
			bool stretchWidth = model.stretchWidth;
			fsResult a25 = a24 + base.DeserializeMember<bool>(data, null, "stretchWidth", out stretchWidth);
			model.stretchWidth = stretchWidth;
			bool wordWrap = model.wordWrap;
			fsResult result = a25 + base.DeserializeMember<bool>(data, null, "wordWrap", out wordWrap);
			model.wordWrap = wordWrap;
			return result;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000161CA File Offset: 0x000143CA
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new GUIStyle();
		}
	}
}
