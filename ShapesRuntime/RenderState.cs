using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000068 RID: 104
	internal struct RenderState : IEquatable<RenderState>
	{
		// Token: 0x06000CA8 RID: 3240 RVA: 0x000195EC File Offset: 0x000177EC
		public Material CreateMaterial()
		{
			Material material = new Material(this.shader)
			{
				shaderKeywords = this.keywords
			};
			material.SetInt_Shapes(this.isTextMaterial ? ShapesMaterialUtils.propZTestTMP : ShapesMaterialUtils.propZTest, (int)this.zTest);
			if (!this.isTextMaterial)
			{
				material.SetFloat(ShapesMaterialUtils.propZOffsetFactor, this.zOffsetFactor);
				material.SetInt_Shapes(ShapesMaterialUtils.propZOffsetUnits, this.zOffsetUnits);
			}
			material.SetInt_Shapes(ShapesMaterialUtils.propColorMask, (int)this.colorMask);
			material.SetInt_Shapes(ShapesMaterialUtils.propStencilComp, (int)this.stencilComp);
			material.SetInt_Shapes(ShapesMaterialUtils.propStencilOpPass, (int)this.stencilOpPass);
			material.SetInt_Shapes(this.isTextMaterial ? ShapesMaterialUtils.propStencilIDTMP : ShapesMaterialUtils.propStencilID, (int)this.stencilRefID);
			material.SetInt_Shapes(ShapesMaterialUtils.propStencilReadMask, (int)this.stencilReadMask);
			material.SetInt_Shapes(ShapesMaterialUtils.propStencilWriteMask, (int)this.stencilWriteMask);
			material.enableInstancing = true;
			Object.DontDestroyOnLoad(material);
			return material;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x000196E0 File Offset: 0x000178E0
		private static bool StrArrEquals(string[] a, string[] b)
		{
			if (a == null || b == null)
			{
				return a == b;
			}
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00019720 File Offset: 0x00017920
		public bool Equals(RenderState other)
		{
			return object.Equals(this.shader, other.shader) && RenderState.StrArrEquals(this.keywords, other.keywords) && this.zTest == other.zTest && this.zOffsetFactor.Equals(other.zOffsetFactor) && this.zOffsetUnits == other.zOffsetUnits && this.colorMask == other.colorMask && this.stencilComp == other.stencilComp && this.stencilOpPass == other.stencilOpPass && this.stencilRefID == other.stencilRefID && this.stencilReadMask == other.stencilReadMask && this.stencilWriteMask == other.stencilWriteMask;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x000197E0 File Offset: 0x000179E0
		public override bool Equals(object obj)
		{
			if (obj is RenderState)
			{
				RenderState other = (RenderState)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00019808 File Offset: 0x00017A08
		public override int GetHashCode()
		{
			int num = (this.shader != null) ? this.shader.GetHashCode() : 0;
			if (this.keywords != null)
			{
				foreach (string text in this.keywords)
				{
					num = (num * 397 ^ ((text != null) ? text.GetHashCode() : 0));
				}
			}
			num = (num * 397 ^ (int)this.zTest);
			num = (num * 397 ^ this.zOffsetFactor.GetHashCode());
			num = (num * 397 ^ this.zOffsetUnits);
			num = (num * 397 ^ (int)this.colorMask);
			num = (num * 397 ^ (int)this.stencilComp);
			num = (num * 397 ^ (int)this.stencilOpPass);
			num = (num * 397 ^ this.stencilRefID.GetHashCode());
			num = (num * 397 ^ this.stencilReadMask.GetHashCode());
			return num * 397 ^ this.stencilWriteMask.GetHashCode();
		}

		// Token: 0x0400021F RID: 543
		public Shader shader;

		// Token: 0x04000220 RID: 544
		public string[] keywords;

		// Token: 0x04000221 RID: 545
		public bool isTextMaterial;

		// Token: 0x04000222 RID: 546
		public CompareFunction zTest;

		// Token: 0x04000223 RID: 547
		public float zOffsetFactor;

		// Token: 0x04000224 RID: 548
		public int zOffsetUnits;

		// Token: 0x04000225 RID: 549
		public ColorWriteMask colorMask;

		// Token: 0x04000226 RID: 550
		public CompareFunction stencilComp;

		// Token: 0x04000227 RID: 551
		public StencilOp stencilOpPass;

		// Token: 0x04000228 RID: 552
		public byte stencilRefID;

		// Token: 0x04000229 RID: 553
		public byte stencilReadMask;

		// Token: 0x0400022A RID: 554
		public byte stencilWriteMask;
	}
}
