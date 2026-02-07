using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000025 RID: 37
	internal abstract class MetaMpb : IDisposable
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0001630C File Offset: 0x0001450C
		public bool HasContent
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00016314 File Offset: 0x00014514
		private bool HasMultipleInstances
		{
			get
			{
				return this.instanceCount > 1;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00016320 File Offset: 0x00014520
		[MethodImpl(256)]
		internal static void ApplyColorOrFill<T>(T fillable, Color baseColor) where T : MetaMpb, IFillableMpb
		{
			if (Draw.style.useGradients)
			{
				GradientFill gradientFill = Draw.style.gradientFill;
				fillable.color.Add(gradientFill.colorStart.ColorSpaceAdjusted());
				fillable.fillType.Add((float)gradientFill.type);
				fillable.fillSpace.Add((float)gradientFill.space);
				fillable.fillStart.Add(gradientFill.GetShaderStartVector());
				fillable.fillColorEnd.Add(gradientFill.colorEnd.ColorSpaceAdjusted());
				fillable.fillEnd.Add(gradientFill.linearEnd);
				return;
			}
			fillable.color.Add(baseColor.ColorSpaceAdjusted());
			fillable.fillType.Add(-1f);
			fillable.fillSpace.Add(0f);
			fillable.fillStart.Add(default(Vector4));
			fillable.fillColorEnd.Add(default(Vector4));
			fillable.fillEnd.Add(default(Vector4));
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00016478 File Offset: 0x00014678
		[MethodImpl(256)]
		internal static void ApplyDashSettings<T>(T dashable, float thickness) where T : MetaMpb, IDashableMpb
		{
			if (Draw.UseDashes && Draw.DashStyle.size > 0f)
			{
				DashStyle dashStyle = Draw.DashStyle;
				dashable.dashSize.Add(dashStyle.GetNetAbsoluteSize(true, thickness));
				dashable.dashType.Add((float)dashStyle.type);
				dashable.dashShapeModifier.Add(dashStyle.shapeModifier);
				dashable.dashSpace.Add((float)dashStyle.space);
				dashable.dashSnap.Add((float)dashStyle.snap);
				dashable.dashOffset.Add(dashStyle.offset);
				dashable.dashSpacing.Add(dashStyle.GetNetAbsoluteSpacing(true, thickness));
				return;
			}
			dashable.dashSize.Add(0f);
			dashable.dashType.Add(0f);
			dashable.dashShapeModifier.Add(0f);
			dashable.dashSpace.Add(0f);
			dashable.dashSnap.Add(0f);
			dashable.dashOffset.Add(0f);
			dashable.dashSpacing.Add(0f);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x000165E0 File Offset: 0x000147E0
		internal static List<T> InitList<T>()
		{
			return new List<T>(1023);
		}

		// Token: 0x06000B5B RID: 2907
		protected abstract void TransferShapeProperties();

		// Token: 0x06000B5C RID: 2908 RVA: 0x000165EC File Offset: 0x000147EC
		protected void Transfer(int propertyID, List<Vector4> listVec)
		{
			if (this.directMaterialApply)
			{
				this.drawState.mat.SetVector(propertyID, listVec[0]);
			}
			else if (this.HasMultipleInstances)
			{
				this.sdc.mpb.SetVectorArray(propertyID, listVec);
			}
			else
			{
				this.sdc.mpb.SetVector(propertyID, listVec[0]);
			}
			listVec.Clear();
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00016658 File Offset: 0x00014858
		protected void Transfer(int propertyID, List<float> listFloat)
		{
			if (this.directMaterialApply)
			{
				this.drawState.mat.SetFloat(propertyID, listFloat[0]);
			}
			else if (this.HasMultipleInstances)
			{
				this.sdc.mpb.SetFloatArray(propertyID, listFloat);
			}
			else
			{
				this.sdc.mpb.SetFloat(propertyID, listFloat[0]);
			}
			listFloat.Clear();
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000166C1 File Offset: 0x000148C1
		protected void Transfer(int propertyID, ref Texture tex)
		{
			if (this.directMaterialApply)
			{
				this.drawState.mat.SetTexture(propertyID, tex);
			}
			else
			{
				this.sdc.mpb.SetTexture(propertyID, tex);
			}
			tex = null;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x000166F8 File Offset: 0x000148F8
		public bool PreAppendCheck(ShapeDrawState additionDrawState, Matrix4x4 mtx)
		{
			bool flag = false;
			if (!this.initialized)
			{
				this.initialized = true;
				this.drawState = additionDrawState;
				flag = true;
			}
			else if (this.instanceCount < 1023 && this.drawState.CompatibleWith(additionDrawState))
			{
				flag = true;
			}
			if (flag)
			{
				Matrix4x4[] array = this.matrices;
				int num = this.instanceCount;
				this.instanceCount = num + 1;
				array[num] = mtx;
			}
			return flag;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00016760 File Offset: 0x00014960
		public ShapeDrawCall ExtractDrawCall()
		{
			bool flag = this.mpbOverride != null && this is MpbCustomMesh;
			if (this.HasMultipleInstances)
			{
				this.sdc = new ShapeDrawCall(this.drawState, this.instanceCount, this.matrices, flag ? this.mpbOverride : null);
				this.matrices = ArrayPool<Matrix4x4>.Alloc(1023);
			}
			else
			{
				this.sdc = new ShapeDrawCall(this.drawState, this.matrices[0], flag ? this.mpbOverride : null);
			}
			if (!flag)
			{
				this.TransferAllProperties();
			}
			this.Dispose();
			return this.sdc;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00016803 File Offset: 0x00014A03
		public void ApplyDirectlyToMaterial()
		{
			this.directMaterialApply = true;
			this.TransferAllProperties();
			this.directMaterialApply = false;
			this.Dispose();
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00016820 File Offset: 0x00014A20
		internal void TransferAllProperties()
		{
			if (this is MpbCustomMesh)
			{
				return;
			}
			if (!(this is MpbText))
			{
				this.Transfer(ShapesMaterialUtils.propColor, this.color);
			}
			IFillableMpb fillableMpb = this as IFillableMpb;
			if (fillableMpb != null)
			{
				this.Transfer(ShapesMaterialUtils.propFillType, fillableMpb.fillType);
				this.Transfer(ShapesMaterialUtils.propFillSpace, fillableMpb.fillSpace);
				this.Transfer(ShapesMaterialUtils.propFillStart, fillableMpb.fillStart);
				this.Transfer(ShapesMaterialUtils.propColorEnd, fillableMpb.fillColorEnd);
				this.Transfer(ShapesMaterialUtils.propFillEnd, fillableMpb.fillEnd);
			}
			IDashableMpb dashableMpb = this as IDashableMpb;
			if (dashableMpb != null)
			{
				this.Transfer(ShapesMaterialUtils.propDashSize, dashableMpb.dashSize);
				this.Transfer(ShapesMaterialUtils.propDashType, dashableMpb.dashType);
				this.Transfer(ShapesMaterialUtils.propDashShapeModifier, dashableMpb.dashShapeModifier);
				this.Transfer(ShapesMaterialUtils.propDashSpace, dashableMpb.dashSpace);
				this.Transfer(ShapesMaterialUtils.propDashSnap, dashableMpb.dashSnap);
				this.Transfer(ShapesMaterialUtils.propDashOffset, dashableMpb.dashOffset);
				this.Transfer(ShapesMaterialUtils.propDashSpacing, dashableMpb.dashSpacing);
			}
			this.TransferShapeProperties();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00016935 File Offset: 0x00014B35
		public void Dispose()
		{
			this.initialized = false;
			this.drawState = default(ShapeDrawState);
			this.instanceCount = 0;
		}

		// Token: 0x04000109 RID: 265
		private bool initialized;

		// Token: 0x0400010A RID: 266
		private int instanceCount;

		// Token: 0x0400010B RID: 267
		private ShapeDrawState drawState;

		// Token: 0x0400010C RID: 268
		public MaterialPropertyBlock mpbOverride;

		// Token: 0x0400010D RID: 269
		private Matrix4x4[] matrices = ArrayPool<Matrix4x4>.Alloc(1023);

		// Token: 0x0400010E RID: 270
		private bool directMaterialApply;

		// Token: 0x0400010F RID: 271
		internal List<Vector4> color = MetaMpb.InitList<Vector4>();

		// Token: 0x04000110 RID: 272
		private ShapeDrawCall sdc;
	}
}
