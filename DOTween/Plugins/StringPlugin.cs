using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200002E RID: 46
	public class StringPlugin : ABSTweenPlugin<string, string, StringOptions>
	{
		// Token: 0x0600022E RID: 558 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		public override void SetFrom(TweenerCore<string, string, StringOptions> t, bool isRelative)
		{
			string endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = endValue;
			t.setter(t.startValue);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000C8E4 File Offset: 0x0000AAE4
		public override void SetFrom(TweenerCore<string, string, StringOptions> t, string fromValue, bool setImmediately, bool isRelative)
		{
			if (fromValue == null)
			{
				fromValue = "";
			}
			if (isRelative)
			{
				string str = t.getter();
				fromValue += str;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(fromValue);
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C92C File Offset: 0x0000AB2C
		public override void Reset(TweenerCore<string, string, StringOptions> t)
		{
			t.startValue = (t.endValue = (t.changeValue = ""));
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008F27 File Offset: 0x00007127
		public override string ConvertToStartValue(TweenerCore<string, string, StringOptions> t, string value)
		{
			return value;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void SetRelativeEndValue(TweenerCore<string, string, StringOptions> t)
		{
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C958 File Offset: 0x0000AB58
		public override void SetChangeValue(TweenerCore<string, string, StringOptions> t)
		{
			t.changeValue = t.endValue;
			bool flag = string.IsNullOrEmpty(t.startValue);
			bool flag2 = string.IsNullOrEmpty(t.changeValue);
			t.plugOptions.startValueStrippedLength = (flag ? 0 : Regex.Replace(t.startValue, "<[^>]*>", "").Length);
			t.plugOptions.changeValueStrippedLength = (flag2 ? 0 : Regex.Replace(t.changeValue, "<[^>]*>", "").Length);
			int num = flag ? 0 : t.startValue.Length;
			int num2 = flag2 ? 0 : t.changeValue.Length;
			if (num > 3 && t.startValue[num - 1] == '>')
			{
				int i = num - 3;
				while (i > -1)
				{
					if (t.startValue[i] == '<')
					{
						if (t.startValue[i + 1] != '/')
						{
							t.plugOptions.startValueStrippedLength = t.plugOptions.startValueStrippedLength + 1;
							break;
						}
						break;
					}
					else
					{
						i--;
					}
				}
			}
			if (num2 > 3 && t.changeValue[num2 - 1] == '>')
			{
				int j = num2 - 3;
				while (j > -1)
				{
					if (t.changeValue[j] == '<')
					{
						if (t.changeValue[j + 1] != '/')
						{
							t.plugOptions.changeValueStrippedLength = t.plugOptions.changeValueStrippedLength + 1;
							return;
						}
						break;
					}
					else
					{
						j--;
					}
				}
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000CAC0 File Offset: 0x0000ACC0
		public override float GetSpeedBasedDuration(StringOptions options, float unitsXSecond, string changeValue)
		{
			float num = (float)(options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length) / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CAF4 File Offset: 0x0000ACF4
		public override void EvaluateAndApply(StringOptions options, Tween t, bool isRelative, DOGetter<string> getter, DOSetter<string> setter, float elapsed, string startValue, string changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			StringPlugin._Buffer.Remove(0, StringPlugin._Buffer.Length);
			if (isRelative && t.loopType == LoopType.Incremental)
			{
				int num = t.isComplete ? (t.completedLoops - 1) : t.completedLoops;
				if (num > 0)
				{
					StringPlugin._Buffer.Append(startValue);
					for (int i = 0; i < num; i++)
					{
						StringPlugin._Buffer.Append(changeValue);
					}
					startValue = StringPlugin._Buffer.ToString();
					StringPlugin._Buffer.Remove(0, StringPlugin._Buffer.Length);
				}
			}
			int num2 = options.richTextEnabled ? options.startValueStrippedLength : (string.IsNullOrEmpty(startValue) ? 0 : startValue.Length);
			int num3 = options.richTextEnabled ? options.changeValueStrippedLength : changeValue.Length;
			int num4 = (int)Math.Round((double)((float)num3 * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
			if (num4 > num3)
			{
				num4 = num3;
			}
			else if (num4 < 0)
			{
				num4 = 0;
			}
			if (isRelative)
			{
				StringPlugin._Buffer.Append(startValue);
				if (options.scrambleMode != ScrambleMode.None)
				{
					setter(this.Append(changeValue, 0, num4, options.richTextEnabled).AppendScrambledChars(num3 - num4, this.ScrambledCharsToUse(options)).ToString());
					return;
				}
				setter(this.Append(changeValue, 0, num4, options.richTextEnabled).ToString());
				return;
			}
			else
			{
				if (options.scrambleMode != ScrambleMode.None)
				{
					setter(this.Append(changeValue, 0, num4, options.richTextEnabled).AppendScrambledChars(num3 - num4, this.ScrambledCharsToUse(options)).ToString());
					return;
				}
				int num5 = num2 - num3;
				int num6 = num2;
				if (num5 > 0)
				{
					float num7 = (float)num4 / (float)num3;
					num6 -= (int)((float)num6 * num7);
				}
				else
				{
					num6 -= num4;
				}
				this.Append(changeValue, 0, num4, options.richTextEnabled);
				if (num4 < num3 && num4 < num2)
				{
					this.Append(startValue, num4, options.richTextEnabled ? (num4 + num6) : num6, options.richTextEnabled);
				}
				setter(StringPlugin._Buffer.ToString());
				return;
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CD00 File Offset: 0x0000AF00
		private StringBuilder Append(string value, int startIndex, int length, bool richTextEnabled)
		{
			if (!richTextEnabled)
			{
				StringPlugin._Buffer.Append(value, startIndex, length);
				return StringPlugin._Buffer;
			}
			StringPlugin._OpenedTags.Clear();
			bool flag = false;
			int length2 = value.Length;
			int num = 0;
			while (num < length && num <= length2 - 1)
			{
				char c = value[num];
				if (c == '<')
				{
					bool flag2 = flag;
					char c2 = value[num + 1];
					flag = (num >= length2 - 1 || c2 != '/');
					if (flag)
					{
						StringPlugin._OpenedTags.Add((c2 == '#') ? 'c' : c2);
					}
					else
					{
						StringPlugin._OpenedTags.RemoveAt(StringPlugin._OpenedTags.Count - 1);
					}
					Match match = Regex.Match(value.Substring(num), "<.*?(>)");
					if (match.Success)
					{
						if (!flag && !flag2)
						{
							char c3 = value[num + 1];
							char[] array;
							if (c3 == 'c')
							{
								array = new char[]
								{
									'#',
									'c'
								};
							}
							else
							{
								array = new char[]
								{
									c3
								};
							}
							for (int i = num - 1; i > -1; i--)
							{
								if (value[i] == '<' && value[i + 1] != '/' && Array.IndexOf<char>(array, value[i + 2]) != -1)
								{
									StringPlugin._Buffer.Insert(0, value.Substring(i, value.IndexOf('>', i) + 1 - i));
									break;
								}
							}
						}
						StringPlugin._Buffer.Append(match.Value);
						int num2 = match.Groups[1].Index + 1;
						length += num2;
						startIndex += num2;
						num += num2 - 1;
					}
				}
				else if (num >= startIndex)
				{
					StringPlugin._Buffer.Append(c);
				}
				num++;
			}
			if (StringPlugin._OpenedTags.Count > 0 && num < length2 - 1)
			{
				while (StringPlugin._OpenedTags.Count > 0 && num < length2 - 1)
				{
					Match match2 = Regex.Match(value.Substring(num), "(</).*?>");
					if (!match2.Success)
					{
						break;
					}
					if (match2.Value[2] == StringPlugin._OpenedTags[StringPlugin._OpenedTags.Count - 1])
					{
						StringPlugin._Buffer.Append(match2.Value);
						StringPlugin._OpenedTags.RemoveAt(StringPlugin._OpenedTags.Count - 1);
					}
					num += match2.Value.Length;
				}
			}
			return StringPlugin._Buffer;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000CF74 File Offset: 0x0000B174
		private char[] ScrambledCharsToUse(StringOptions options)
		{
			switch (options.scrambleMode)
			{
			case ScrambleMode.Uppercase:
				return StringPluginExtensions.ScrambledCharsUppercase;
			case ScrambleMode.Lowercase:
				return StringPluginExtensions.ScrambledCharsLowercase;
			case ScrambleMode.Numerals:
				return StringPluginExtensions.ScrambledCharsNumerals;
			case ScrambleMode.Custom:
				return options.scrambledChars;
			default:
				return StringPluginExtensions.ScrambledCharsAll;
			}
		}

		// Token: 0x040000DF RID: 223
		private static readonly StringBuilder _Buffer = new StringBuilder();

		// Token: 0x040000E0 RID: 224
		private static readonly List<char> _OpenedTags = new List<char>();
	}
}
