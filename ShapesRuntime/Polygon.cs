using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shapes
{
	// Token: 0x02000007 RID: 7
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Polygon")]
	public class Polygon : ShapeRenderer, IFillable
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003303 File Offset: 0x00001503
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000330B File Offset: 0x0000150B
		public PolygonTriangulation Triangulation
		{
			get
			{
				return this.triangulation;
			}
			set
			{
				this.triangulation = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000097 RID: 151 RVA: 0x0000331B File Offset: 0x0000151B
		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000043 RID: 67
		public Vector2 this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000334C File Offset: 0x0000154C
		public void SetPointPosition(int index, Vector2 position)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			this.points[index] = position;
			this.meshOutOfDate = true;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003375 File Offset: 0x00001575
		public void SetPoints(IEnumerable<Vector2> points)
		{
			this.points.Clear();
			this.AddPoints(points);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003389 File Offset: 0x00001589
		public void AddPoints(IEnumerable<Vector2> points)
		{
			this.points.AddRange(points);
			this.meshOutOfDate = true;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000339E File Offset: 0x0000159E
		public void AddPoint(Vector2 point)
		{
			this.points.Add(point);
			this.meshOutOfDate = true;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000033B3 File Offset: 0x000015B3
		private protected override bool UseCamOnPreCull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000033B6 File Offset: 0x000015B6
		internal override void CamOnPreCull()
		{
			if (this.meshOutOfDate)
			{
				this.meshOutOfDate = false;
				base.UpdateMesh(true);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000033CE File Offset: 0x000015CE
		private protected override void SetAllMaterialProperties()
		{
			this.SetFillProperties();
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000033D6 File Offset: 0x000015D6
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000033D9 File Offset: 0x000015D9
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000033DC File Offset: 0x000015DC
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.matPolygon[base.BlendMode];
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000033F1 File Offset: 0x000015F1
		private protected override MeshUpdateMode MeshUpdateMode
		{
			get
			{
				return MeshUpdateMode.SelfGenerated;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000033F4 File Offset: 0x000015F4
		private protected override void GenerateMesh()
		{
			ShapesMeshGen.GenPolygonMesh(base.Mesh, this.points, this.triangulation);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003410 File Offset: 0x00001610
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			if (this.points.Count < 2)
			{
				return default(Bounds);
			}
			Vector2 vector = Vector2.one * float.MaxValue;
			Vector2 vector2 = Vector2.one * float.MinValue;
			foreach (Vector2 rhs in this.points)
			{
				vector = Vector2.Min(vector, rhs);
				vector2 = Vector2.Max(vector2, rhs);
			}
			return new Bounds((vector2 + vector) * 0.5f, vector2 - vector);
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000034D0 File Offset: 0x000016D0
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000034D8 File Offset: 0x000016D8
		public GradientFill Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				this.fill = value;
				this.SetFillProperties();
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000034E7 File Offset: 0x000016E7
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000034EF File Offset: 0x000016EF
		public bool UseFill
		{
			get
			{
				return this.useFill;
			}
			set
			{
				this.useFill = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.fill.GetShaderFillTypeInt(this.useFill));
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003514 File Offset: 0x00001714
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003521 File Offset: 0x00001721
		public FillType FillType
		{
			get
			{
				return this.fill.type;
			}
			set
			{
				this.fill.type = value;
				base.SetIntNow(ShapesMaterialUtils.propFillType, this.fill.GetShaderFillTypeInt(this.useFill));
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000354B File Offset: 0x0000174B
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003558 File Offset: 0x00001758
		public FillSpace FillSpace
		{
			get
			{
				return this.fill.space;
			}
			set
			{
				int propFillSpace = ShapesMaterialUtils.propFillSpace;
				this.fill.space = value;
				base.SetIntNow(propFillSpace, (int)value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000357F File Offset: 0x0000177F
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000358C File Offset: 0x0000178C
		public Vector3 FillRadialOrigin
		{
			get
			{
				return this.fill.radialOrigin;
			}
			set
			{
				this.fill.radialOrigin = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000035B0 File Offset: 0x000017B0
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000035BD File Offset: 0x000017BD
		public float FillRadialRadius
		{
			get
			{
				return this.fill.radialRadius;
			}
			set
			{
				this.fill.radialRadius = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x000035E1 File Offset: 0x000017E1
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x000035EE File Offset: 0x000017EE
		public Vector3 FillLinearStart
		{
			get
			{
				return this.fill.linearStart;
			}
			set
			{
				this.fill.linearStart = value;
				base.SetVector4Now(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003612 File Offset: 0x00001812
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003620 File Offset: 0x00001820
		public Vector3 FillLinearEnd
		{
			get
			{
				return this.fill.linearEnd;
			}
			set
			{
				int propFillEnd = ShapesMaterialUtils.propFillEnd;
				this.fill.linearEnd = value;
				base.SetVector3Now(propFillEnd, value);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003647 File Offset: 0x00001847
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003654 File Offset: 0x00001854
		public Color FillColorStart
		{
			get
			{
				return this.fill.colorStart;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.fill.colorStart = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000367B File Offset: 0x0000187B
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003688 File Offset: 0x00001888
		public Color FillColorEnd
		{
			get
			{
				return this.fill.colorEnd;
			}
			set
			{
				int propColorEnd = ShapesMaterialUtils.propColorEnd;
				this.fill.colorEnd = value;
				base.SetColorNow(propColorEnd, value);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000036B0 File Offset: 0x000018B0
		private void SetFillProperties()
		{
			if (this.useFill)
			{
				base.SetInt(ShapesMaterialUtils.propFillSpace, (int)this.fill.space);
				base.SetVector4(ShapesMaterialUtils.propFillStart, this.fill.GetShaderStartVector());
				base.SetVector3(ShapesMaterialUtils.propFillEnd, this.fill.linearEnd);
				base.SetColor(ShapesMaterialUtils.propColor, this.fill.colorStart);
				base.SetColor(ShapesMaterialUtils.propColorEnd, this.fill.colorEnd);
			}
			base.SetInt(ShapesMaterialUtils.propFillType, this.fill.GetShaderFillTypeInt(this.useFill));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003750 File Offset: 0x00001950
		public Polygon()
		{
			List<Vector2> list = new List<Vector2>();
			list.Add(new Vector2(1f, 0f));
			list.Add(new Vector2(0.5f, 0.86602545f));
			list.Add(new Vector2(-0.5f, 0.8660254f));
			list.Add(new Vector2(-1f, 0f));
			list.Add(new Vector2(-0.5f, -0.86602545f));
			list.Add(new Vector2(0.5f, -0.86602545f));
			this.points = list;
			this.triangulation = PolygonTriangulation.EarClipping;
			this.fill = GradientFill.defaultFill;
			base..ctor();
		}

		// Token: 0x04000023 RID: 35
		[FormerlySerializedAs("polyPoints")]
		[SerializeField]
		public List<Vector2> points;

		// Token: 0x04000024 RID: 36
		[SerializeField]
		private PolygonTriangulation triangulation;

		// Token: 0x04000025 RID: 37
		[SerializeField]
		private protected GradientFill fill;

		// Token: 0x04000026 RID: 38
		[SerializeField]
		private protected bool useFill;
	}
}
