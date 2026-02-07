using System;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000099 RID: 153
	public class AnimationCurveFormatter : MinimalBaseFormatter<AnimationCurve>
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override AnimationCurve GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000203B8 File Offset: 0x0001E5B8
		protected override void Read(ref AnimationCurve value, IDataReader reader)
		{
			Keyframe[] keys = AnimationCurveFormatter.KeyframeSerializer.ReadValue(reader);
			value = new AnimationCurve(keys);
			value.preWrapMode = AnimationCurveFormatter.WrapModeSerializer.ReadValue(reader);
			value.postWrapMode = AnimationCurveFormatter.WrapModeSerializer.ReadValue(reader);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000203FD File Offset: 0x0001E5FD
		protected override void Write(ref AnimationCurve value, IDataWriter writer)
		{
			AnimationCurveFormatter.KeyframeSerializer.WriteValue(value.keys, writer);
			AnimationCurveFormatter.WrapModeSerializer.WriteValue(value.preWrapMode, writer);
			AnimationCurveFormatter.WrapModeSerializer.WriteValue(value.postWrapMode, writer);
		}

		// Token: 0x04000190 RID: 400
		private static readonly Serializer<Keyframe[]> KeyframeSerializer = Serializer.Get<Keyframe[]>();

		// Token: 0x04000191 RID: 401
		private static readonly Serializer<WrapMode> WrapModeSerializer = Serializer.Get<WrapMode>();
	}
}
