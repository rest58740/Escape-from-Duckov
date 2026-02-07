using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x02000074 RID: 116
	[Preserve]
	[ES3Properties(new string[]
	{
		"keys",
		"preWrapMode",
		"postWrapMode"
	})]
	public class ES3Type_AnimationCurve : ES3Type
	{
		// Token: 0x06000308 RID: 776 RVA: 0x0000ED90 File Offset: 0x0000CF90
		public ES3Type_AnimationCurve() : base(typeof(AnimationCurve))
		{
			ES3Type_AnimationCurve.Instance = this;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
		public override void Write(object obj, ES3Writer writer)
		{
			AnimationCurve animationCurve = (AnimationCurve)obj;
			writer.WriteProperty("keys", animationCurve.keys, ES3Type_KeyframeArray.Instance);
			writer.WriteProperty("preWrapMode", animationCurve.preWrapMode);
			writer.WriteProperty("postWrapMode", animationCurve.postWrapMode);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000EE00 File Offset: 0x0000D000
		public override object Read<T>(ES3Reader reader)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			this.ReadInto<T>(reader, animationCurve);
			return animationCurve;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000EE1C File Offset: 0x0000D01C
		public override void ReadInto<T>(ES3Reader reader, object obj)
		{
			AnimationCurve animationCurve = (AnimationCurve)obj;
			string a;
			while ((a = reader.ReadPropertyName()) != null)
			{
				if (!(a == "keys"))
				{
					if (!(a == "preWrapMode"))
					{
						if (!(a == "postWrapMode"))
						{
							reader.Skip();
						}
						else
						{
							animationCurve.postWrapMode = reader.Read<WrapMode>();
						}
					}
					else
					{
						animationCurve.preWrapMode = reader.Read<WrapMode>();
					}
				}
				else
				{
					animationCurve.keys = reader.Read<Keyframe[]>();
				}
			}
		}

		// Token: 0x040000B4 RID: 180
		public static ES3Type Instance;
	}
}
