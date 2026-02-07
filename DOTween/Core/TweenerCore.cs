using System;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000058 RID: 88
	public class TweenerCore<T1, T2, TPlugOptions> : Tweener where TPlugOptions : struct, IPlugOptions
	{
		// Token: 0x06000317 RID: 791 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		internal TweenerCore()
		{
			this.typeofT1 = typeof(T1);
			this.typeofT2 = typeof(T2);
			this.typeofTPlugOptions = typeof(TPlugOptions);
			this.tweenType = TweenType.Tweener;
			this.Reset();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00011D54 File Offset: 0x0000FF54
		public override Tweener ChangeStartValue(object newStartValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				Debugger.LogError("You cannot change the values of a tween contained inside a Sequence", this);
				return this;
			}
			Type type = newStartValue.GetType();
			bool flag;
			if (!this.ValidateChangeValueType(type, out flag))
			{
				string[] array = new string[5];
				array[0] = "ChangeStartValue: incorrect newStartValue type (is ";
				int num = 1;
				Type type2 = type;
				array[num] = ((type2 != null) ? type2.ToString() : null);
				array[2] = ", should be ";
				int num2 = 3;
				Type typeofT = this.typeofT2;
				array[num2] = ((typeofT != null) ? typeofT.ToString() : null);
				array[4] = ")";
				Debugger.LogError(string.Concat(array), this);
				return this;
			}
			if (flag)
			{
				return Tweener.DoChangeStartValue<T1, T2, TPlugOptions>(this, (T2)((object)((Color32)newStartValue)), newDuration);
			}
			return Tweener.DoChangeStartValue<T1, T2, TPlugOptions>(this, (T2)((object)newStartValue), newDuration);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011E05 File Offset: 0x00010005
		public override Tweener ChangeEndValue(object newEndValue, bool snapStartValue)
		{
			return this.ChangeEndValue(newEndValue, -1f, snapStartValue);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00011E14 File Offset: 0x00010014
		public override Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false)
		{
			if (this.isSequenced)
			{
				Debugger.LogError("You cannot change the values of a tween contained inside a Sequence", this);
				return this;
			}
			Type type = newEndValue.GetType();
			bool flag;
			if (!this.ValidateChangeValueType(type, out flag))
			{
				string[] array = new string[5];
				array[0] = "ChangeEndValue: incorrect newEndValue type (is ";
				int num = 1;
				Type type2 = type;
				array[num] = ((type2 != null) ? type2.ToString() : null);
				array[2] = ", should be ";
				int num2 = 3;
				Type typeofT = this.typeofT2;
				array[num2] = ((typeofT != null) ? typeofT.ToString() : null);
				array[4] = ")";
				Debugger.LogError(string.Concat(array), this);
				return this;
			}
			if (flag)
			{
				return Tweener.DoChangeEndValue<T1, T2, TPlugOptions>(this, (T2)((object)((Color32)newEndValue)), newDuration, snapStartValue);
			}
			return Tweener.DoChangeEndValue<T1, T2, TPlugOptions>(this, (T2)((object)newEndValue), newDuration, snapStartValue);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00011EC8 File Offset: 0x000100C8
		public override Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				Debugger.LogError("You cannot change the values of a tween contained inside a Sequence", this);
				return this;
			}
			Type type = newStartValue.GetType();
			Type type2 = newEndValue.GetType();
			bool flag;
			if (!this.ValidateChangeValueType(type, out flag))
			{
				string[] array = new string[5];
				array[0] = "ChangeValues: incorrect value type (is ";
				int num = 1;
				Type type3 = type;
				array[num] = ((type3 != null) ? type3.ToString() : null);
				array[2] = ", should be ";
				int num2 = 3;
				Type typeofT = this.typeofT2;
				array[num2] = ((typeofT != null) ? typeofT.ToString() : null);
				array[4] = ")";
				Debugger.LogError(string.Concat(array), this);
				return this;
			}
			if (!this.ValidateChangeValueType(type2, out flag))
			{
				string[] array2 = new string[5];
				array2[0] = "ChangeValues: incorrect value type (is ";
				int num3 = 1;
				Type type4 = type2;
				array2[num3] = ((type4 != null) ? type4.ToString() : null);
				array2[2] = ", should be ";
				int num4 = 3;
				Type typeofT2 = this.typeofT2;
				array2[num4] = ((typeofT2 != null) ? typeofT2.ToString() : null);
				array2[4] = ")";
				Debugger.LogError(string.Concat(array2), this);
				return this;
			}
			if (flag)
			{
				return Tweener.DoChangeValues<T1, T2, TPlugOptions>(this, (T2)((object)((Color32)newStartValue)), (T2)((object)((Color32)newEndValue)), newDuration);
			}
			return Tweener.DoChangeValues<T1, T2, TPlugOptions>(this, (T2)((object)newStartValue), (T2)((object)newEndValue), newDuration);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00011FF6 File Offset: 0x000101F6
		public TweenerCore<T1, T2, TPlugOptions> ChangeStartValue(T2 newStartValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				Debugger.LogError("You cannot change the values of a tween contained inside a Sequence", this);
				return this;
			}
			return Tweener.DoChangeStartValue<T1, T2, TPlugOptions>(this, newStartValue, newDuration);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00012015 File Offset: 0x00010215
		public TweenerCore<T1, T2, TPlugOptions> ChangeEndValue(T2 newEndValue, bool snapStartValue)
		{
			return this.ChangeEndValue(newEndValue, -1f, snapStartValue);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00012024 File Offset: 0x00010224
		public TweenerCore<T1, T2, TPlugOptions> ChangeEndValue(T2 newEndValue, float newDuration = -1f, bool snapStartValue = false)
		{
			if (this.isSequenced)
			{
				Debugger.LogError("You cannot change the values of a tween contained inside a Sequence", this);
				return this;
			}
			return Tweener.DoChangeEndValue<T1, T2, TPlugOptions>(this, newEndValue, newDuration, snapStartValue);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00012044 File Offset: 0x00010244
		public TweenerCore<T1, T2, TPlugOptions> ChangeValues(T2 newStartValue, T2 newEndValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				Debugger.LogError("You cannot change the values of a tween contained inside a Sequence", this);
				return this;
			}
			return Tweener.DoChangeValues<T1, T2, TPlugOptions>(this, newStartValue, newEndValue, newDuration);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00012064 File Offset: 0x00010264
		internal override Tweener SetFrom(bool relative)
		{
			this.tweenPlugin.SetFrom(this, relative);
			this.hasManuallySetStartValue = true;
			return this;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001207B File Offset: 0x0001027B
		internal Tweener SetFrom(T2 fromValue, bool setImmediately, bool relative)
		{
			this.tweenPlugin.SetFrom(this, fromValue, setImmediately, relative);
			this.hasManuallySetStartValue = true;
			return this;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00012094 File Offset: 0x00010294
		internal sealed override void Reset()
		{
			base.Reset();
			if (this.tweenPlugin != null)
			{
				this.tweenPlugin.Reset(this);
			}
			this.plugOptions.Reset();
			this.getter = null;
			this.setter = null;
			this.hasManuallySetStartValue = false;
			this.isFromAllowed = true;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000120E8 File Offset: 0x000102E8
		internal override bool Validate()
		{
			try
			{
				this.getter();
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0001211C File Offset: 0x0001031C
		private bool ValidateChangeValueType(Type newType, out bool isColor32ToColor)
		{
			if (newType == this.typeofT2)
			{
				isColor32ToColor = false;
				return true;
			}
			if (this.typeofT2 == this._colorType && newType == this._color32Type)
			{
				isColor32ToColor = true;
				return true;
			}
			isColor32ToColor = false;
			return false;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001214C File Offset: 0x0001034C
		internal override float UpdateDelay(float elapsed)
		{
			return Tweener.DoUpdateDelay<T1, T2, TPlugOptions>(this, elapsed);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00012155 File Offset: 0x00010355
		internal override bool Startup()
		{
			return Tweener.DoStartup<T1, T2, TPlugOptions>(this);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00012160 File Offset: 0x00010360
		internal override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
		{
			if (this.isInverted)
			{
				useInversePosition = !useInversePosition;
			}
			float elapsed = useInversePosition ? (this.duration - base.position) : base.position;
			if (DOTween.useSafeMode)
			{
				try
				{
					this.tweenPlugin.EvaluateAndApply(this.plugOptions, this, base.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
					return false;
				}
				catch (Exception ex)
				{
					if (Debugger.ShouldLogSafeModeCapturedError())
					{
						Debugger.LogSafeModeCapturedError(string.Format("Target or field is missing/null ({0}) ► {1}\n\n{2}\n\n", ex.TargetSite, ex.Message, ex.StackTrace), this);
					}
					DOTween.safeModeReport.Add(SafeModeReport.SafeModeReportType.TargetOrFieldMissing);
					return true;
				}
			}
			this.tweenPlugin.EvaluateAndApply(this.plugOptions, this, base.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
			return false;
		}

		// Token: 0x0400019C RID: 412
		public T2 startValue;

		// Token: 0x0400019D RID: 413
		public T2 endValue;

		// Token: 0x0400019E RID: 414
		public T2 changeValue;

		// Token: 0x0400019F RID: 415
		public TPlugOptions plugOptions;

		// Token: 0x040001A0 RID: 416
		public DOGetter<T1> getter;

		// Token: 0x040001A1 RID: 417
		public DOSetter<T1> setter;

		// Token: 0x040001A2 RID: 418
		internal ABSTweenPlugin<T1, T2, TPlugOptions> tweenPlugin;

		// Token: 0x040001A3 RID: 419
		private const string _TxtCantChangeSequencedValues = "You cannot change the values of a tween contained inside a Sequence";

		// Token: 0x040001A4 RID: 420
		private Type _colorType = typeof(Color);

		// Token: 0x040001A5 RID: 421
		private Type _color32Type = typeof(Color32);
	}
}
