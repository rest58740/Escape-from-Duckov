using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000009 RID: 9
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Quad")]
	public class Quad : ShapeRenderer
	{
		// Token: 0x1700005C RID: 92
		public Vector3 this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.A;
				case 1:
					return this.B;
				case 2:
					return this.C;
				case 3:
					return this.D;
				default:
					throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to access element {0}", index));
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.A = value;
					return;
				case 1:
					this.B = value;
					return;
				case 2:
					this.C = value;
					return;
				case 3:
					this.D = value;
					return;
				default:
					throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to set element {0}", index));
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003EE2 File Offset: 0x000020E2
		public Vector3 GetQuadVertex(int index)
		{
			return this[index];
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00003EEC File Offset: 0x000020EC
		public Vector3 SetQuadVertex(int index, Vector3 value)
		{
			this[index] = value;
			return value;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003F04 File Offset: 0x00002104
		public Color GetQuadColor(int index)
		{
			switch (index)
			{
			case 0:
				return this.Color;
			case 1:
				return this.ColorB;
			case 2:
				return this.ColorC;
			case 3:
				return this.ColorD;
			default:
				throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to access element {0}", index));
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00003F5C File Offset: 0x0000215C
		public void SetQuadColor(int index, Color color)
		{
			switch (index)
			{
			case 0:
				this.Color = color;
				return;
			case 1:
				this.ColorB = color;
				return;
			case 2:
				this.ColorC = color;
				return;
			case 3:
				this.ColorD = color;
				return;
			default:
				throw new IndexOutOfRangeException(string.Format("Quad only has four vertices, 0 to 3, you tried to set element {0}", index));
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00003FB6 File Offset: 0x000021B6
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00003FBE File Offset: 0x000021BE
		public Quad.QuadColorMode ColorMode
		{
			get
			{
				return this.colorMode;
			}
			set
			{
				this.colorMode = value;
				base.ApplyProperties();
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003FCD File Offset: 0x000021CD
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00003FD8 File Offset: 0x000021D8
		public Vector3 A
		{
			get
			{
				return this.a;
			}
			set
			{
				int propA = ShapesMaterialUtils.propA;
				this.a = value;
				base.SetVector3Now(propA, value);
				this.CheckAutoSetD();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004000 File Offset: 0x00002200
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004008 File Offset: 0x00002208
		public Vector3 B
		{
			get
			{
				return this.b;
			}
			set
			{
				int propB = ShapesMaterialUtils.propB;
				this.b = value;
				base.SetVector3Now(propB, value);
				this.CheckAutoSetD();
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004030 File Offset: 0x00002230
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004038 File Offset: 0x00002238
		public Vector3 C
		{
			get
			{
				return this.c;
			}
			set
			{
				int propC = ShapesMaterialUtils.propC;
				this.c = value;
				base.SetVector3Now(propC, value);
				this.CheckAutoSetD();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004060 File Offset: 0x00002260
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004068 File Offset: 0x00002268
		public Vector3 D
		{
			get
			{
				return this.d;
			}
			set
			{
				if (this.autoSetD)
				{
					Debug.LogWarning("tried to set D when auto-set is enabled, you might want to turn off auto-set on this object", base.gameObject);
					return;
				}
				int propD = ShapesMaterialUtils.propD;
				this.d = value;
				base.SetVector3Now(propD, value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000040A3 File Offset: 0x000022A3
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000040AB File Offset: 0x000022AB
		public bool IsUsingAutoD
		{
			get
			{
				return this.autoSetD;
			}
			set
			{
				this.autoSetD = value;
				this.AutoSetD();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x000040BA File Offset: 0x000022BA
		public Vector3 DAuto
		{
			get
			{
				return this.A + (this.C - this.B);
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000040D8 File Offset: 0x000022D8
		private void AutoSetD()
		{
			base.SetVector3(ShapesMaterialUtils.propD, this.DAuto);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000040EB File Offset: 0x000022EB
		private void CheckAutoSetD()
		{
			if (this.autoSetD)
			{
				this.AutoSetD();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x000040FB File Offset: 0x000022FB
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004104 File Offset: 0x00002304
		public override Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColor(propColorB, value);
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColor(propColorC, value);
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColorNow(propColorD, value);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004165 File Offset: 0x00002365
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004170 File Offset: 0x00002370
		public Color ColorLeft
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColorNow(propColorB, value);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000041A7 File Offset: 0x000023A7
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000041B0 File Offset: 0x000023B0
		public Color ColorTop
		{
			get
			{
				return this.colorB;
			}
			set
			{
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColor(propColorB, value);
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000041E7 File Offset: 0x000023E7
		// (set) Token: 0x060000FC RID: 252 RVA: 0x000041F0 File Offset: 0x000023F0
		public Color ColorRight
		{
			get
			{
				return this.colorC;
			}
			set
			{
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColor(propColorC, value);
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColorNow(propColorD, value);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004227 File Offset: 0x00002427
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00004230 File Offset: 0x00002430
		public Color ColorBottom
		{
			get
			{
				return this.colorD;
			}
			set
			{
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColor(propColorD, value);
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004267 File Offset: 0x00002467
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00004270 File Offset: 0x00002470
		public Color ColorA
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00004292 File Offset: 0x00002492
		// (set) Token: 0x06000102 RID: 258 RVA: 0x0000429C File Offset: 0x0000249C
		public Color ColorB
		{
			get
			{
				return this.colorB;
			}
			set
			{
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColorNow(propColorB, value);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000042BE File Offset: 0x000024BE
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000042C8 File Offset: 0x000024C8
		public Color ColorC
		{
			get
			{
				return this.colorC;
			}
			set
			{
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000042EA File Offset: 0x000024EA
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000042F4 File Offset: 0x000024F4
		public Color ColorD
		{
			get
			{
				return this.colorD;
			}
			set
			{
				int propColorD = ShapesMaterialUtils.propColorD;
				this.colorD = value;
				base.SetColorNow(propColorD, value);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004318 File Offset: 0x00002518
		private protected override void SetAllMaterialProperties()
		{
			base.SetVector3(ShapesMaterialUtils.propA, this.a);
			base.SetVector3(ShapesMaterialUtils.propB, this.b);
			base.SetVector3(ShapesMaterialUtils.propC, this.c);
			if (this.autoSetD)
			{
				this.AutoSetD();
			}
			else
			{
				base.SetVector3(ShapesMaterialUtils.propD, this.d);
			}
			switch (this.colorMode)
			{
			case Quad.QuadColorMode.Single:
				base.SetColor(ShapesMaterialUtils.propColorB, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorC, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorD, this.Color);
				return;
			case Quad.QuadColorMode.Horizontal:
				base.SetColor(ShapesMaterialUtils.propColorB, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorC);
				base.SetColor(ShapesMaterialUtils.propColorD, this.colorC);
				return;
			case Quad.QuadColorMode.Vertical:
				base.SetColor(ShapesMaterialUtils.propColor, this.colorD);
				base.SetColor(ShapesMaterialUtils.propColorB, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorD, this.colorD);
				return;
			case Quad.QuadColorMode.PerCorner:
				base.SetColor(ShapesMaterialUtils.propColorB, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorC);
				base.SetColor(ShapesMaterialUtils.propColorD, this.colorD);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00004481 File Offset: 0x00002681
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00004484 File Offset: 0x00002684
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004487 File Offset: 0x00002687
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.QuadMesh[0];
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004490 File Offset: 0x00002690
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matQuad[base.BlendMode];
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000044A8 File Offset: 0x000026A8
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			Vector3 rhs = this.IsUsingAutoD ? this.DAuto : this.d;
			Vector3 vector = Vector3.Min(Vector3.Min(Vector3.Min(this.a, this.b), this.c), rhs);
			Vector3 vector2 = Vector3.Max(Vector3.Max(Vector3.Max(this.a, this.b), this.c), rhs);
			return new Bounds((vector + vector2) / 2f, ShapesMath.Abs(vector2 - vector));
		}

		// Token: 0x0400002D RID: 45
		[SerializeField]
		private Quad.QuadColorMode colorMode;

		// Token: 0x0400002E RID: 46
		[SerializeField]
		private Vector3 a = new Vector2(-0.5f, -0.5f);

		// Token: 0x0400002F RID: 47
		[SerializeField]
		private Vector3 b = new Vector2(-0.5f, 0.5f);

		// Token: 0x04000030 RID: 48
		[SerializeField]
		private Vector3 c = new Vector2(0.5f, 0.5f);

		// Token: 0x04000031 RID: 49
		[SerializeField]
		private Vector3 d = new Vector2(0.5f, -0.5f);

		// Token: 0x04000032 RID: 50
		[SerializeField]
		private bool autoSetD;

		// Token: 0x04000033 RID: 51
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorB = Color.white;

		// Token: 0x04000034 RID: 52
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorC = Color.white;

		// Token: 0x04000035 RID: 53
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorD = Color.white;

		// Token: 0x02000083 RID: 131
		public enum QuadColorMode
		{
			// Token: 0x04000326 RID: 806
			Single,
			// Token: 0x04000327 RID: 807
			Horizontal,
			// Token: 0x04000328 RID: 808
			Vertical,
			// Token: 0x04000329 RID: 809
			PerCorner
		}
	}
}
