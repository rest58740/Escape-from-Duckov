using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shapes
{
	// Token: 0x02000008 RID: 8
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Polyline")]
	public class Polyline : ShapeRenderer
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000037FE File Offset: 0x000019FE
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003806 File Offset: 0x00001A06
		public PolylineGeometry Geometry
		{
			get
			{
				return this.geometry;
			}
			set
			{
				this.geometry = value;
				base.SetIntNow(ShapesMaterialUtils.propAlignment, (int)this.geometry);
				base.UpdateMaterial();
				base.ApplyProperties();
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000BF RID: 191 RVA: 0x0000382C File Offset: 0x00001A2C
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003834 File Offset: 0x00001A34
		public PolylineJoins Joins
		{
			get
			{
				return this.joins;
			}
			set
			{
				this.joins = value;
				this.meshOutOfDate = true;
				base.UpdateMaterial();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000384A File Offset: 0x00001A4A
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003852 File Offset: 0x00001A52
		public bool Closed
		{
			get
			{
				return this.closed;
			}
			set
			{
				this.closed = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003862 File Offset: 0x00001A62
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x0000386C File Offset: 0x00001A6C
		public float Thickness
		{
			get
			{
				return this.thickness;
			}
			set
			{
				int propThickness = ShapesMaterialUtils.propThickness;
				this.thickness = value;
				base.SetFloatNow(propThickness, value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000388E File Offset: 0x00001A8E
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003898 File Offset: 0x00001A98
		public ThicknessSpace ThicknessSpace
		{
			get
			{
				return this.thicknessSpace;
			}
			set
			{
				int propThicknessSpace = ShapesMaterialUtils.propThicknessSpace;
				this.thicknessSpace = value;
				base.SetIntNow(propThicknessSpace, (int)value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000038BA File Offset: 0x00001ABA
		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000058 RID: 88
		public PolylinePoint this[int i]
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

		// Token: 0x060000CA RID: 202 RVA: 0x000038EC File Offset: 0x00001AEC
		public void SetPointPosition(int index, Vector3 position)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			PolylinePoint polylinePoint = this.points[index];
			polylinePoint.point = position;
			this.points[index] = polylinePoint;
			this.meshOutOfDate = true;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003938 File Offset: 0x00001B38
		public void SetPointColor(int index, Color color)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			PolylinePoint polylinePoint = this.points[index];
			polylinePoint.color = color;
			this.points[index] = polylinePoint;
			this.meshOutOfDate = true;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003984 File Offset: 0x00001B84
		public void SetPointThickness(int index, float thickness)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			PolylinePoint polylinePoint = this.points[index];
			polylinePoint.thickness = thickness;
			this.points[index] = polylinePoint;
			this.meshOutOfDate = true;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000039D0 File Offset: 0x00001BD0
		public void SetPoints(IReadOnlyCollection<Vector3> points, IReadOnlyCollection<Color> colors = null)
		{
			this.points.Clear();
			if (colors == null)
			{
				this.AddPoints(from p in points
				select new PolylinePoint(p, Color.white));
				return;
			}
			if (points.Count != colors.Count)
			{
				throw new ArgumentException("point.Count != color.Count");
			}
			this.AddPoints(points.Zip(colors, (Vector3 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003A5C File Offset: 0x00001C5C
		public void SetPoints(IReadOnlyCollection<Vector2> points, IReadOnlyCollection<Color> colors = null)
		{
			this.meshOutOfDate = true;
			this.points.Clear();
			if (colors == null)
			{
				this.AddPoints(from p in points
				select new PolylinePoint(p, Color.white));
				return;
			}
			if (points.Count != colors.Count)
			{
				throw new ArgumentException("point.Count != color.Count");
			}
			this.AddPoints(points.Zip(colors, (Vector2 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003AEF File Offset: 0x00001CEF
		public void SetPoints(IEnumerable<PolylinePoint> points)
		{
			this.points.Clear();
			this.AddPoints(points);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003B03 File Offset: 0x00001D03
		public void AddPoints(IEnumerable<PolylinePoint> points)
		{
			this.points.AddRange(points);
			this.meshOutOfDate = true;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003B18 File Offset: 0x00001D18
		public void AddPoint(Vector3 position)
		{
			this.AddPoint(new PolylinePoint(position));
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003B26 File Offset: 0x00001D26
		public void AddPoint(Vector3 position, Color color)
		{
			this.AddPoint(new PolylinePoint(position, color));
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003B35 File Offset: 0x00001D35
		public void AddPoint(Vector3 position, Color color, float thickness)
		{
			this.AddPoint(new PolylinePoint(position, color, thickness));
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003B45 File Offset: 0x00001D45
		public void AddPoint(Vector3 position, float thickness)
		{
			this.AddPoint(new PolylinePoint(position, Color.white, thickness));
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003B59 File Offset: 0x00001D59
		public void AddPoint(PolylinePoint point)
		{
			this.points.Add(point);
			this.meshOutOfDate = true;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003B6E File Offset: 0x00001D6E
		private protected override bool UseCamOnPreCull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003B71 File Offset: 0x00001D71
		internal override void CamOnPreCull()
		{
			if (this.meshOutOfDate)
			{
				this.meshOutOfDate = false;
				base.UpdateMesh(true);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003B89 File Offset: 0x00001D89
		private protected override MeshUpdateMode MeshUpdateMode
		{
			get
			{
				return MeshUpdateMode.SelfGenerated;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003B8C File Offset: 0x00001D8C
		private protected override void GenerateMesh()
		{
			ShapesMeshGen.GenPolylineMesh(base.Mesh, this.points, this.closed, this.joins, this.geometry == PolylineGeometry.Flat2D, true);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003BB5 File Offset: 0x00001DB5
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetInt(ShapesMaterialUtils.propAlignment, (int)this.geometry);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003BEA File Offset: 0x00001DEA
		private protected override void ShapeClampRanges()
		{
			this.thickness = Mathf.Max(0f, this.thickness);
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00003C02 File Offset: 0x00001E02
		private protected override int MaterialCount
		{
			get
			{
				if (!this.joins.HasJoinMesh())
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003C14 File Offset: 0x00001E14
		private protected override void GetMaterials(Material[] mats)
		{
			mats[0] = ShapesMaterialUtils.GetPolylineMat(this.joins)[base.BlendMode];
			if (this.MaterialCount == 2)
			{
				mats[1] = ShapesMaterialUtils.GetPolylineJoinsMat(this.joins)[base.BlendMode];
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003C54 File Offset: 0x00001E54
		private protected override Bounds GetUnpaddedLocalBounds_Internal()
		{
			if (this.points.Count < 2)
			{
				return default(Bounds);
			}
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			foreach (Vector3 rhs in from p in this.points
			select p.point)
			{
				vector = Vector3.Min(vector, rhs);
				vector2 = Vector3.Max(vector2, rhs);
			}
			if (this.geometry == PolylineGeometry.Flat2D)
			{
				vector.z = (vector2.z = 0f);
			}
			float num = (this.joins == PolylineJoins.Miter) ? 2.4142137f : 1f;
			float d = (this.thicknessSpace == ThicknessSpace.Meters) ? (this.thickness * num) : 0f;
			return new Bounds((vector2 + vector) * 0.5f, vector2 - vector + Vector3.one * d);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003D8C File Offset: 0x00001F8C
		public Polyline()
		{
			List<PolylinePoint> list = new List<PolylinePoint>();
			list.Add(new PolylinePoint(new Vector3(0f, 1f, 0f), Color.white));
			list.Add(new PolylinePoint(new Vector3(0.8660254f, -0.5f, 0f), Color.white));
			list.Add(new PolylinePoint(new Vector3(-0.8660254f, -0.5f, 0f), Color.white));
			this.points = list;
			this.joins = PolylineJoins.Miter;
			this.closed = true;
			this.thickness = 0.125f;
			base..ctor();
		}

		// Token: 0x04000027 RID: 39
		[FormerlySerializedAs("polyPoints")]
		[SerializeField]
		public List<PolylinePoint> points;

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private PolylineGeometry geometry;

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private PolylineJoins joins;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private bool closed;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		private float thickness;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private ThicknessSpace thicknessSpace;
	}
}
