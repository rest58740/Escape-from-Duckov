using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000008 RID: 8
	public class DOTweenTMPAnimator : IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003ADC File Offset: 0x00001CDC
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00003AE4 File Offset: 0x00001CE4
		public TMP_Text target { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003AED File Offset: 0x00001CED
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00003AF5 File Offset: 0x00001CF5
		public TMP_TextInfo textInfo { get; private set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00003B00 File Offset: 0x00001D00
		public DOTweenTMPAnimator(TMP_Text target)
		{
			if (target == null)
			{
				Debugger.LogError("DOTweenTMPAnimator target can't be null", null);
				return;
			}
			if (!target.gameObject.activeInHierarchy)
			{
				Debugger.LogError("You can't create a DOTweenTMPAnimator if its target is disabled", null);
				return;
			}
			if (DOTweenTMPAnimator._targetToAnimator.ContainsKey(target))
			{
				if (Debugger.logPriority >= 2)
				{
					Debugger.Log(string.Format("A DOTweenTMPAnimator for \"{0}\" already exists: disposing it because you can't have more than one DOTweenTMPAnimator for the same TextMesh Pro object. If you have tweens running on the disposed DOTweenTMPAnimator you should kill them manually", target));
				}
				DOTweenTMPAnimator._targetToAnimator[target].Dispose();
				DOTweenTMPAnimator._targetToAnimator.Remove(target);
			}
			this.target = target;
			DOTweenTMPAnimator._targetToAnimator.Add(target, this);
			this.Refresh();
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.OnTextChanged));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003BBC File Offset: 0x00001DBC
		public static void DisposeInstanceFor(TMP_Text target)
		{
			if (!DOTweenTMPAnimator._targetToAnimator.ContainsKey(target))
			{
				return;
			}
			DOTweenTMPAnimator._targetToAnimator[target].Dispose();
			DOTweenTMPAnimator._targetToAnimator.Remove(target);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003BE8 File Offset: 0x00001DE8
		public void Dispose()
		{
			this.target = null;
			this._charTransforms.Clear();
			this.textInfo = null;
			this._cachedMeshInfos = null;
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.OnTextChanged));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003C20 File Offset: 0x00001E20
		public void Refresh()
		{
			this._ignoreTextChangedEvent = true;
			this.target.ForceMeshUpdate(true, false);
			this.textInfo = this.target.textInfo;
			this._cachedMeshInfos = this.textInfo.CopyMeshInfoVertexData();
			int characterCount = this.textInfo.characterCount;
			int num = this._charTransforms.Count;
			if (num > characterCount)
			{
				this._charTransforms.RemoveRange(characterCount, num - characterCount);
				num = characterCount;
			}
			for (int i = 0; i < num; i++)
			{
				DOTweenTMPAnimator.CharTransform value = this._charTransforms[i];
				value.ResetTransformationData();
				value.Refresh(this.textInfo, this._cachedMeshInfos);
				this._charTransforms[i] = value;
			}
			for (int j = num; j < characterCount; j++)
			{
				this._charTransforms.Add(new DOTweenTMPAnimator.CharTransform(j, this.textInfo, this._cachedMeshInfos));
			}
			this._ignoreTextChangedEvent = false;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003D08 File Offset: 0x00001F08
		public void Reset()
		{
			int count = this._charTransforms.Count;
			for (int i = 0; i < count; i++)
			{
				this._charTransforms[i].ResetAll(this.target, this.textInfo.meshInfo, this._cachedMeshInfos);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003D58 File Offset: 0x00001F58
		private void OnTextChanged(UnityEngine.Object obj)
		{
			if (this._ignoreTextChangedEvent || this.target == null || obj != this.target)
			{
				return;
			}
			this.Refresh();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003D88 File Offset: 0x00001F88
		private bool ValidateChar(int charIndex, bool isTween = true)
		{
			if (this.textInfo.characterCount <= charIndex)
			{
				Debugger.LogError(string.Format("CharIndex {0} doesn't exist", charIndex), null);
				return false;
			}
			if (!this.textInfo.characterInfo[charIndex].isVisible)
			{
				if (Debugger.logPriority > 1)
				{
					if (isTween)
					{
						Debugger.Log(string.Format("CharIndex {0} isn't visible, ignoring it and returning an empty tween (TextMesh Pro will behave weirdly if invisible chars are included in the animation)", charIndex));
					}
					else
					{
						Debugger.Log(string.Format("CharIndex {0} isn't visible, ignoring it", charIndex));
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003E10 File Offset: 0x00002010
		private bool ValidateSpan(int fromCharIndex, int toCharIndex, out int firstVisibleCharIndex, out int lastVisibleCharIndex)
		{
			firstVisibleCharIndex = -1;
			lastVisibleCharIndex = -1;
			int characterCount = this.textInfo.characterCount;
			if (fromCharIndex >= characterCount)
			{
				return false;
			}
			if (toCharIndex >= characterCount)
			{
				toCharIndex = characterCount - 1;
			}
			for (int i = fromCharIndex; i < toCharIndex + 1; i++)
			{
				if (this._charTransforms[i].isVisible)
				{
					firstVisibleCharIndex = i;
					break;
				}
			}
			if (firstVisibleCharIndex == -1)
			{
				return false;
			}
			for (int j = toCharIndex; j > firstVisibleCharIndex - 1; j--)
			{
				if (this._charTransforms[j].isVisible)
				{
					lastVisibleCharIndex = j;
					break;
				}
			}
			return lastVisibleCharIndex != -1;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003EA4 File Offset: 0x000020A4
		public void SkewSpanX(int fromCharIndex, int toCharIndex, float skewFactor, bool skewTop = true)
		{
			int num;
			int num2;
			if (!this.ValidateSpan(fromCharIndex, toCharIndex, out num, out num2))
			{
				return;
			}
			for (int i = num; i < num2 + 1; i++)
			{
				if (this._charTransforms[i].isVisible)
				{
					this._charTransforms[i].GetVertices();
					this.SkewCharX(i, skewFactor, skewTop);
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003F04 File Offset: 0x00002104
		public void SkewSpanY(int fromCharIndex, int toCharIndex, float skewFactor, TMPSkewSpanMode mode = TMPSkewSpanMode.Default, bool skewRight = true)
		{
			int num;
			int num2;
			if (!this.ValidateSpan(fromCharIndex, toCharIndex, out num, out num2))
			{
				return;
			}
			if (mode == TMPSkewSpanMode.AsMaxSkewFactor)
			{
				DOTweenTMPAnimator.CharVertices vertices = this._charTransforms[num].GetVertices();
				DOTweenTMPAnimator.CharVertices vertices2 = this._charTransforms[num2].GetVertices();
				float num3 = Mathf.Abs(vertices2.bottomRight.x - vertices.bottomLeft.x);
				float num4 = Mathf.Abs(vertices2.topRight.y - vertices2.bottomRight.y) / num3;
				skewFactor *= num4;
			}
			float num5 = 0f;
			DOTweenTMPAnimator.CharVertices charVertices = default(DOTweenTMPAnimator.CharVertices);
			float num6 = 0f;
			if (skewRight)
			{
				for (int i = num; i < num2 + 1; i++)
				{
					if (this._charTransforms[i].isVisible)
					{
						DOTweenTMPAnimator.CharVertices vertices3 = this._charTransforms[i].GetVertices();
						float num7 = this.SkewCharY(i, skewFactor, skewRight, false);
						if (i > num)
						{
							float num8 = Mathf.Abs(charVertices.bottomLeft.x - charVertices.bottomRight.x);
							float num9 = Mathf.Abs(vertices3.bottomLeft.x - charVertices.bottomRight.x);
							num5 += num6 + num6 * num9 / num8;
							this.SetCharOffset(i, new Vector3(0f, this._charTransforms[i].offset.y + num5, 0f));
						}
						charVertices = vertices3;
						num6 = num7;
					}
				}
				return;
			}
			for (int j = num2; j > num - 1; j--)
			{
				if (this._charTransforms[j].isVisible)
				{
					DOTweenTMPAnimator.CharVertices vertices4 = this._charTransforms[j].GetVertices();
					float num10 = this.SkewCharY(j, skewFactor, skewRight, false);
					if (j < num2)
					{
						float num11 = Mathf.Abs(charVertices.bottomLeft.x - charVertices.bottomRight.x);
						float num12 = Mathf.Abs(vertices4.bottomRight.x - charVertices.bottomLeft.x);
						num5 += num6 + num6 * num12 / num11;
						this.SetCharOffset(j, new Vector3(0f, this._charTransforms[j].offset.y + num5, 0f));
					}
					charVertices = vertices4;
					num6 = num10;
				}
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004174 File Offset: 0x00002374
		public Color GetCharColor(int charIndex)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return Color.white;
			}
			return this._charTransforms[charIndex].GetColor(this.textInfo.meshInfo);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000041B5 File Offset: 0x000023B5
		public Vector3 GetCharOffset(int charIndex)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return Vector3.zero;
			}
			return this._charTransforms[charIndex].offset;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000041D8 File Offset: 0x000023D8
		public Vector3 GetCharRotation(int charIndex)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return Vector3.zero;
			}
			return this._charTransforms[charIndex].rotation.eulerAngles;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000420E File Offset: 0x0000240E
		public Vector3 GetCharScale(int charIndex)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return Vector3.zero;
			}
			return this._charTransforms[charIndex].scale;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004234 File Offset: 0x00002434
		public void SetCharColor(int charIndex, Color32 color)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return;
			}
			DOTweenTMPAnimator.CharTransform value = this._charTransforms[charIndex];
			value.UpdateColor(this.target, color, this.textInfo.meshInfo, true);
			this._charTransforms[charIndex] = value;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004280 File Offset: 0x00002480
		public void SetCharOffset(int charIndex, Vector3 offset)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return;
			}
			DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
			charTransform.UpdateGeometry(this.target, offset, charTransform.rotation, charTransform.scale, this._cachedMeshInfos, true);
			this._charTransforms[charIndex] = charTransform;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000042D4 File Offset: 0x000024D4
		public void SetCharRotation(int charIndex, Vector3 rotation)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return;
			}
			DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
			charTransform.UpdateGeometry(this.target, charTransform.offset, Quaternion.Euler(rotation), charTransform.scale, this._cachedMeshInfos, true);
			this._charTransforms[charIndex] = charTransform;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000432C File Offset: 0x0000252C
		public void SetCharScale(int charIndex, Vector3 scale)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return;
			}
			DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
			charTransform.UpdateGeometry(this.target, charTransform.offset, charTransform.rotation, scale, this._cachedMeshInfos, true);
			this._charTransforms[charIndex] = charTransform;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004380 File Offset: 0x00002580
		public void ShiftCharVertices(int charIndex, Vector3 topLeftShift, Vector3 topRightShift, Vector3 bottomLeftShift, Vector3 bottomRightShift)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return;
			}
			DOTweenTMPAnimator.CharTransform value = this._charTransforms[charIndex];
			value.ShiftVertices(this.target, topLeftShift, topRightShift, bottomLeftShift, bottomRightShift);
			this._charTransforms[charIndex] = value;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000043C8 File Offset: 0x000025C8
		public float SkewCharX(int charIndex, float skewFactor, bool skewTop = true)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return 0f;
			}
			Vector3 vector = new Vector3(skewFactor, 0f, 0f);
			DOTweenTMPAnimator.CharTransform value = this._charTransforms[charIndex];
			if (skewTop)
			{
				value.ShiftVertices(this.target, vector, vector, Vector3.zero, Vector3.zero);
			}
			else
			{
				value.ShiftVertices(this.target, Vector3.zero, Vector3.zero, vector, vector);
			}
			this._charTransforms[charIndex] = value;
			return skewFactor;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000444C File Offset: 0x0000264C
		public float SkewCharY(int charIndex, float skewFactor, bool skewRight = true, bool fixedSkew = false)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return 0f;
			}
			float num = fixedSkew ? skewFactor : (skewFactor * this.textInfo.characterInfo[charIndex].aspectRatio);
			Vector3 vector = new Vector3(0f, num, 0f);
			DOTweenTMPAnimator.CharTransform value = this._charTransforms[charIndex];
			if (skewRight)
			{
				value.ShiftVertices(this.target, Vector3.zero, vector, Vector3.zero, vector);
			}
			else
			{
				value.ShiftVertices(this.target, vector, Vector3.zero, vector, Vector3.zero);
			}
			this._charTransforms[charIndex] = value;
			return num;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000044F0 File Offset: 0x000026F0
		public void ResetVerticesShift(int charIndex)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return;
			}
			DOTweenTMPAnimator.CharTransform value = this._charTransforms[charIndex];
			value.ResetVerticesShift(this.target);
			this._charTransforms[charIndex] = value;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004530 File Offset: 0x00002730
		public TweenerCore<Color, Color, ColorOptions> DOFadeChar(int charIndex, float endValue, float duration)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			return DOTween.ToAlpha(() => this._charTransforms[charIndex].GetColor(this.textInfo.meshInfo), delegate(Color x)
			{
				this._charTransforms[charIndex].UpdateAlpha(this.target, x, this.textInfo.meshInfo, true);
			}, endValue, duration);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004584 File Offset: 0x00002784
		public TweenerCore<Color, Color, ColorOptions> DOColorChar(int charIndex, Color endValue, float duration)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			return DOTween.To(() => this._charTransforms[charIndex].GetColor(this.textInfo.meshInfo), delegate(Color x)
			{
				this._charTransforms[charIndex].UpdateColor(this.target, x, this.textInfo.meshInfo, true);
			}, endValue, duration);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000045D8 File Offset: 0x000027D8
		public TweenerCore<Vector3, Vector3, VectorOptions> DOOffsetChar(int charIndex, Vector3 endValue, float duration)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			return DOTween.To(() => this._charTransforms[charIndex].offset, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, x, charTransform.rotation, charTransform.scale, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, endValue, duration);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000462C File Offset: 0x0000282C
		public TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotateChar(int charIndex, Vector3 endValue, float duration, RotateMode mode = 0)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = DOTween.To(() => this._charTransforms[charIndex].rotation, delegate(Quaternion x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, charTransform.offset, x, charTransform.scale, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, endValue, duration);
			tweenerCore.plugOptions.rotateMode = mode;
			return tweenerCore;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000468A File Offset: 0x0000288A
		public TweenerCore<Vector3, Vector3, VectorOptions> DOScaleChar(int charIndex, float endValue, float duration)
		{
			return this.DOScaleChar(charIndex, new Vector3(endValue, endValue, endValue), duration);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000469C File Offset: 0x0000289C
		public TweenerCore<Vector3, Vector3, VectorOptions> DOScaleChar(int charIndex, Vector3 endValue, float duration)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			return DOTween.To(() => this._charTransforms[charIndex].scale, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, charTransform.offset, charTransform.rotation, x, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, endValue, duration);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000046F0 File Offset: 0x000028F0
		public Tweener DOPunchCharOffset(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => this._charTransforms[charIndex].offset, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, x, charTransform.rotation, charTransform.scale, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, punch, duration, vibrato, elasticity);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004764 File Offset: 0x00002964
		public Tweener DOPunchCharRotation(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => this._charTransforms[charIndex].rotation.eulerAngles, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, charTransform.offset, Quaternion.Euler(x), charTransform.scale, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, punch, duration, vibrato, elasticity);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000047D5 File Offset: 0x000029D5
		public Tweener DOPunchCharScale(int charIndex, float punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			return this.DOPunchCharScale(charIndex, new Vector3(punch, punch, punch), duration, vibrato, elasticity);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000047EC File Offset: 0x000029EC
		public Tweener DOPunchCharScale(int charIndex, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Punch(() => this._charTransforms[charIndex].scale, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, charTransform.offset, charTransform.rotation, x, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, punch, duration, vibrato, elasticity);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000485D File Offset: 0x00002A5D
		public Tweener DOShakeCharOffset(int charIndex, float duration, float strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			return this.DOShakeCharOffset(charIndex, duration, new Vector3(strength, strength, strength), vibrato, randomness, fadeOut);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004878 File Offset: 0x00002A78
		public Tweener DOShakeCharOffset(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => this._charTransforms[charIndex].offset, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, x, charTransform.rotation, charTransform.scale, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, duration, strength, vibrato, randomness, fadeOut);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000048EC File Offset: 0x00002AEC
		public Tweener DOShakeCharRotation(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => this._charTransforms[charIndex].rotation.eulerAngles, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, charTransform.offset, Quaternion.Euler(x), charTransform.scale, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, duration, strength, vibrato, randomness, fadeOut);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000495F File Offset: 0x00002B5F
		public Tweener DOShakeCharScale(int charIndex, float duration, float strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			return this.DOShakeCharScale(charIndex, duration, new Vector3(strength, strength, strength), vibrato, randomness, fadeOut);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004978 File Offset: 0x00002B78
		public Tweener DOShakeCharScale(int charIndex, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			if (!this.ValidateChar(charIndex, true))
			{
				return null;
			}
			if (duration <= 0f)
			{
				if (Debugger.logPriority > 0)
				{
					Debug.LogWarning("Duration can't be 0, returning NULL without creating a tween");
				}
				return null;
			}
			return DOTween.Shake(() => this._charTransforms[charIndex].scale, delegate(Vector3 x)
			{
				DOTweenTMPAnimator.CharTransform charTransform = this._charTransforms[charIndex];
				charTransform.UpdateGeometry(this.target, charTransform.offset, charTransform.rotation, x, this._cachedMeshInfos, true);
				this._charTransforms[charIndex] = charTransform;
			}, duration, strength, vibrato, randomness, fadeOut);
		}

		// Token: 0x0400002C RID: 44
		private static readonly Dictionary<TMP_Text, DOTweenTMPAnimator> _targetToAnimator = new Dictionary<TMP_Text, DOTweenTMPAnimator>();

		// Token: 0x0400002F RID: 47
		private readonly List<DOTweenTMPAnimator.CharTransform> _charTransforms = new List<DOTweenTMPAnimator.CharTransform>();

		// Token: 0x04000030 RID: 48
		private TMP_MeshInfo[] _cachedMeshInfos;

		// Token: 0x04000031 RID: 49
		private bool _ignoreTextChangedEvent;

		// Token: 0x02000019 RID: 25
		private struct CharVertices
		{
			// Token: 0x0600008C RID: 140 RVA: 0x00004BD7 File Offset: 0x00002DD7
			public CharVertices(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight)
			{
				this.bottomLeft = bottomLeft;
				this.topLeft = topLeft;
				this.topRight = topRight;
				this.bottomRight = bottomRight;
			}

			// Token: 0x04000070 RID: 112
			public Vector3 bottomLeft;

			// Token: 0x04000071 RID: 113
			public Vector3 topLeft;

			// Token: 0x04000072 RID: 114
			public Vector3 topRight;

			// Token: 0x04000073 RID: 115
			public Vector3 bottomRight;
		}

		// Token: 0x0200001A RID: 26
		private struct CharTransform
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600008D RID: 141 RVA: 0x00004BF6 File Offset: 0x00002DF6
			// (set) Token: 0x0600008E RID: 142 RVA: 0x00004BFE File Offset: 0x00002DFE
			public bool isVisible { readonly get; private set; }

			// Token: 0x0600008F RID: 143 RVA: 0x00004C07 File Offset: 0x00002E07
			public CharTransform(int charIndex, TMP_TextInfo textInfo, TMP_MeshInfo[] cachedMeshInfos)
			{
				this = default(DOTweenTMPAnimator.CharTransform);
				this.charIndex = charIndex;
				this.offset = Vector3.zero;
				this.rotation = Quaternion.identity;
				this.scale = Vector3.one;
				this.Refresh(textInfo, cachedMeshInfos);
			}

			// Token: 0x06000090 RID: 144 RVA: 0x00004C40 File Offset: 0x00002E40
			public void Refresh(TMP_TextInfo textInfo, TMP_MeshInfo[] cachedMeshInfos)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[this.charIndex];
				bool flag = tmp_CharacterInfo.character == ' ';
				this.isVisible = (tmp_CharacterInfo.isVisible && !flag);
				this._matIndex = tmp_CharacterInfo.materialReferenceIndex;
				this._firstVertexIndex = tmp_CharacterInfo.vertexIndex;
				this._meshInfo = textInfo.meshInfo[this._matIndex];
				Vector3[] vertices = cachedMeshInfos[this._matIndex].vertices;
				this._charMidBaselineOffset = (flag ? Vector3.zero : ((vertices[this._firstVertexIndex] + vertices[this._firstVertexIndex + 2]) * 0.5f));
			}

			// Token: 0x06000091 RID: 145 RVA: 0x00004CF9 File Offset: 0x00002EF9
			public void ResetAll(TMP_Text target, TMP_MeshInfo[] meshInfos, TMP_MeshInfo[] cachedMeshInfos)
			{
				this.ResetGeometry(target, cachedMeshInfos);
				this.ResetColors(target, meshInfos);
			}

			// Token: 0x06000092 RID: 146 RVA: 0x00004D0C File Offset: 0x00002F0C
			public void ResetTransformationData()
			{
				this.offset = Vector3.zero;
				this.rotation = Quaternion.identity;
				this.scale = Vector3.one;
				this._topLeftShift = (this._topRightShift = (this._bottomLeftShift = (this._bottomRightShift = Vector3.zero)));
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00004D60 File Offset: 0x00002F60
			public void ResetGeometry(TMP_Text target, TMP_MeshInfo[] cachedMeshInfos)
			{
				this.ResetTransformationData();
				Vector3[] vertices = this._meshInfo.vertices;
				Vector3[] vertices2 = cachedMeshInfos[this._matIndex].vertices;
				vertices[this._firstVertexIndex] = vertices2[this._firstVertexIndex];
				vertices[this._firstVertexIndex + 1] = vertices2[this._firstVertexIndex + 1];
				vertices[this._firstVertexIndex + 2] = vertices2[this._firstVertexIndex + 2];
				vertices[this._firstVertexIndex + 3] = vertices2[this._firstVertexIndex + 3];
				this._meshInfo.mesh.vertices = this._meshInfo.vertices;
				target.UpdateGeometry(this._meshInfo.mesh, this._matIndex);
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00004E30 File Offset: 0x00003030
			public void ResetColors(TMP_Text target, TMP_MeshInfo[] meshInfos)
			{
				Color color = target.color;
				Color32[] colors = meshInfos[this._matIndex].colors32;
				colors[this._firstVertexIndex] = color;
				colors[this._firstVertexIndex + 1] = color;
				colors[this._firstVertexIndex + 2] = color;
				colors[this._firstVertexIndex + 3] = color;
				target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}

			// Token: 0x06000095 RID: 149 RVA: 0x00004EAA File Offset: 0x000030AA
			public Color32 GetColor(TMP_MeshInfo[] meshInfos)
			{
				return meshInfos[this._matIndex].colors32[this._firstVertexIndex];
			}

			// Token: 0x06000096 RID: 150 RVA: 0x00004EC8 File Offset: 0x000030C8
			public DOTweenTMPAnimator.CharVertices GetVertices()
			{
				return new DOTweenTMPAnimator.CharVertices(this._meshInfo.vertices[this._firstVertexIndex], this._meshInfo.vertices[this._firstVertexIndex + 1], this._meshInfo.vertices[this._firstVertexIndex + 2], this._meshInfo.vertices[this._firstVertexIndex + 3]);
			}

			// Token: 0x06000097 RID: 151 RVA: 0x00004F38 File Offset: 0x00003138
			public void UpdateAlpha(TMP_Text target, Color alphaColor, TMP_MeshInfo[] meshInfos, bool apply = true)
			{
				byte a = (byte)(alphaColor.a * 255f);
				Color32[] colors = meshInfos[this._matIndex].colors32;
				colors[this._firstVertexIndex].a = a;
				colors[this._firstVertexIndex + 1].a = a;
				colors[this._firstVertexIndex + 2].a = a;
				colors[this._firstVertexIndex + 3].a = a;
				if (apply)
				{
					target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				}
			}

			// Token: 0x06000098 RID: 152 RVA: 0x00004FC0 File Offset: 0x000031C0
			public void UpdateColor(TMP_Text target, Color32 color, TMP_MeshInfo[] meshInfos, bool apply = true)
			{
				Color32[] colors = meshInfos[this._matIndex].colors32;
				colors[this._firstVertexIndex] = color;
				colors[this._firstVertexIndex + 1] = color;
				colors[this._firstVertexIndex + 2] = color;
				colors[this._firstVertexIndex + 3] = color;
				if (apply)
				{
					target.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				}
			}

			// Token: 0x06000099 RID: 153 RVA: 0x00005024 File Offset: 0x00003224
			public void UpdateGeometry(TMP_Text target, Vector3 offset, Quaternion rotation, Vector3 scale, TMP_MeshInfo[] cachedMeshInfos, bool apply = true)
			{
				this.offset = offset;
				this.rotation = rotation;
				this.scale = scale;
				if (!apply)
				{
					return;
				}
				Vector3[] vertices = this._meshInfo.vertices;
				Vector3[] vertices2 = cachedMeshInfos[this._matIndex].vertices;
				vertices[this._firstVertexIndex] = vertices2[this._firstVertexIndex] - this._charMidBaselineOffset;
				vertices[this._firstVertexIndex + 1] = vertices2[this._firstVertexIndex + 1] - this._charMidBaselineOffset;
				vertices[this._firstVertexIndex + 2] = vertices2[this._firstVertexIndex + 2] - this._charMidBaselineOffset;
				vertices[this._firstVertexIndex + 3] = vertices2[this._firstVertexIndex + 3] - this._charMidBaselineOffset;
				Matrix4x4 matrix4x = Matrix4x4.TRS(this.offset, this.rotation, this.scale);
				vertices[this._firstVertexIndex] = matrix4x.MultiplyPoint3x4(vertices[this._firstVertexIndex]) + this._charMidBaselineOffset + this._bottomLeftShift;
				vertices[this._firstVertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[this._firstVertexIndex + 1]) + this._charMidBaselineOffset + this._topLeftShift;
				vertices[this._firstVertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[this._firstVertexIndex + 2]) + this._charMidBaselineOffset + this._topRightShift;
				vertices[this._firstVertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[this._firstVertexIndex + 3]) + this._charMidBaselineOffset + this._bottomRightShift;
				this._meshInfo.mesh.vertices = this._meshInfo.vertices;
				target.UpdateGeometry(this._meshInfo.mesh, this._matIndex);
			}

			// Token: 0x0600009A RID: 154 RVA: 0x00005230 File Offset: 0x00003430
			public void ShiftVertices(TMP_Text target, Vector3 topLeftShift, Vector3 topRightShift, Vector3 bottomLeftShift, Vector3 bottomRightShift)
			{
				this._topLeftShift += topLeftShift;
				this._topRightShift += topRightShift;
				this._bottomLeftShift += bottomLeftShift;
				this._bottomRightShift += bottomRightShift;
				Vector3[] vertices = this._meshInfo.vertices;
				vertices[this._firstVertexIndex] = vertices[this._firstVertexIndex] + this._bottomLeftShift;
				vertices[this._firstVertexIndex + 1] = vertices[this._firstVertexIndex + 1] + this._topLeftShift;
				vertices[this._firstVertexIndex + 2] = vertices[this._firstVertexIndex + 2] + this._topRightShift;
				vertices[this._firstVertexIndex + 3] = vertices[this._firstVertexIndex + 3] + this._bottomRightShift;
				this._meshInfo.mesh.vertices = this._meshInfo.vertices;
				target.UpdateGeometry(this._meshInfo.mesh, this._matIndex);
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00005360 File Offset: 0x00003560
			public void ResetVerticesShift(TMP_Text target)
			{
				Vector3[] vertices = this._meshInfo.vertices;
				vertices[this._firstVertexIndex] = vertices[this._firstVertexIndex] - this._bottomLeftShift;
				vertices[this._firstVertexIndex + 1] = vertices[this._firstVertexIndex + 1] - this._topLeftShift;
				vertices[this._firstVertexIndex + 2] = vertices[this._firstVertexIndex + 2] - this._topRightShift;
				vertices[this._firstVertexIndex + 3] = vertices[this._firstVertexIndex + 3] - this._bottomRightShift;
				this._meshInfo.mesh.vertices = this._meshInfo.vertices;
				target.UpdateGeometry(this._meshInfo.mesh, this._matIndex);
				this._topLeftShift = (this._topRightShift = (this._bottomLeftShift = (this._bottomRightShift = Vector3.zero)));
			}

			// Token: 0x04000074 RID: 116
			public int charIndex;

			// Token: 0x04000076 RID: 118
			public Vector3 offset;

			// Token: 0x04000077 RID: 119
			public Quaternion rotation;

			// Token: 0x04000078 RID: 120
			public Vector3 scale;

			// Token: 0x04000079 RID: 121
			private Vector3 _topLeftShift;

			// Token: 0x0400007A RID: 122
			private Vector3 _topRightShift;

			// Token: 0x0400007B RID: 123
			private Vector3 _bottomLeftShift;

			// Token: 0x0400007C RID: 124
			private Vector3 _bottomRightShift;

			// Token: 0x0400007D RID: 125
			private Vector3 _charMidBaselineOffset;

			// Token: 0x0400007E RID: 126
			private int _matIndex;

			// Token: 0x0400007F RID: 127
			private int _firstVertexIndex;

			// Token: 0x04000080 RID: 128
			private TMP_MeshInfo _meshInfo;
		}
	}
}
