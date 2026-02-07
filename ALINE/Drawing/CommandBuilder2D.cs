using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace Drawing
{
	// Token: 0x02000026 RID: 38
	public struct CommandBuilder2D
	{
		// Token: 0x06000125 RID: 293 RVA: 0x000070BF File Offset: 0x000052BF
		public CommandBuilder2D(CommandBuilder draw, bool xy)
		{
			this.draw = draw;
			this.xy = xy;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000070D0 File Offset: 0x000052D0
		public unsafe void Line(float2 a, float2 b)
		{
			this.draw.Reserve<CommandBuilder.LineData>();
			UnsafeAppendBuffer* buffer = this.draw.buffer;
			int length = buffer->Length;
			int length2 = length + 4 + 24;
			byte* ptr = buffer->Ptr + length;
			*(int*)ptr = 5;
			CommandBuilder.LineData* ptr2 = (CommandBuilder.LineData*)(ptr + 4);
			if (this.xy)
			{
				ptr2->a = new float3(a, 0f);
				ptr2->b = new float3(b, 0f);
			}
			else
			{
				ptr2->a = new float3(a.x, 0f, a.y);
				ptr2->b = new float3(b.x, 0f, b.y);
			}
			buffer->Length = length2;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000717C File Offset: 0x0000537C
		public unsafe void Line(float2 a, float2 b, Color color)
		{
			this.draw.Reserve<Color32, CommandBuilder.LineData>();
			UnsafeAppendBuffer* buffer = this.draw.buffer;
			int length = buffer->Length;
			int length2 = length + 4 + 24 + 4;
			byte* ptr = buffer->Ptr + length;
			*(int*)ptr = 261;
			*(int*)(ptr + 4) = (int)CommandBuilder.ConvertColor(color);
			CommandBuilder.LineData* ptr2 = (CommandBuilder.LineData*)(ptr + 8);
			if (this.xy)
			{
				ptr2->a = new float3(a, 0f);
				ptr2->b = new float3(b, 0f);
			}
			else
			{
				ptr2->a = new float3(a.x, 0f, a.y);
				ptr2->b = new float3(b.x, 0f, b.y);
			}
			buffer->Length = length2;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007235 File Offset: 0x00005435
		public void Line(float3 a, float3 b)
		{
			this.draw.Line(a, b);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007244 File Offset: 0x00005444
		public void Circle(float2 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.Circle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000727C File Offset: 0x0000547C
		public void Circle(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
				this.draw.CircleXZInternal(new float3(center.x, center.z, center.y), radius, startAngle, endAngle);
				this.draw.PopMatrix();
				return;
			}
			this.draw.CircleXZInternal(center, radius, startAngle, endAngle);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000072E3 File Offset: 0x000054E3
		public void SolidCircle(float2 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.SolidCircle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000731C File Offset: 0x0000551C
		public void SolidCircle(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
			}
			this.draw.SolidCircleXZInternal(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			if (this.xy)
			{
				this.draw.PopMatrix();
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000737B File Offset: 0x0000557B
		public void WirePill(float2 a, float2 b, float radius)
		{
			this.WirePill(a, b - a, math.length(b - a), radius);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007398 File Offset: 0x00005598
		public void WirePill(float2 position, float2 direction, float length, float radius)
		{
			direction = math.normalizesafe(direction, default(float2));
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
				return;
			}
			if (length <= 0f || math.all(direction == 0f))
			{
				this.Circle(position, radius, 0f, 6.2831855f);
				return;
			}
			float4x4 matrix;
			if (this.xy)
			{
				matrix = new float4x4(new float4(direction, 0f, 0f), new float4(math.cross(new float3(direction, 0f), CommandBuilder2D.XY_UP), 0f), new float4(0f, 0f, 1f, 0f), new float4(position, 0f, 1f));
			}
			else
			{
				matrix = new float4x4(new float4(direction.x, 0f, direction.y, 0f), new float4(0f, 1f, 0f, 0f), new float4(math.cross(new float3(direction.x, 0f, direction.y), CommandBuilder2D.XZ_UP), 0f), new float4(position.x, 0f, position.y, 1f));
			}
			this.draw.PushMatrix(matrix);
			this.Circle(new float2(0f, 0f), radius, 1.5707964f, 4.712389f);
			this.Line(new float2(0f, -radius), new float2(length, -radius));
			this.Circle(new float2(length, 0f), radius, -1.5707964f, 1.5707964f);
			this.Line(new float2(0f, radius), new float2(length, radius));
			this.draw.PopMatrix();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000757C File Offset: 0x0000577C
		[BurstDiscard]
		public void Polyline(List<Vector2> points, bool cycle = false)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000075DC File Offset: 0x000057DC
		[BurstDiscard]
		public void Polyline(Vector2[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007630 File Offset: 0x00005830
		[BurstDiscard]
		public void Polyline(float2[] points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007684 File Offset: 0x00005884
		public void Polyline(NativeArray<float2> points, bool cycle = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000076E8 File Offset: 0x000058E8
		public void Cross(float2 position, float size = 1f)
		{
			size *= 0.5f;
			this.Line(position - new float2(size, 0f), position + new float2(size, 0f));
			this.Line(position - new float2(0f, size), position + new float2(0f, size));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000774E File Offset: 0x0000594E
		public void WireRectangle(float3 center, float2 size)
		{
			this.draw.WirePlane(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, size);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007774 File Offset: 0x00005974
		public void WireRectangle(Rect rect)
		{
			float2 @float = rect.min;
			float2 float2 = rect.max;
			this.Line(new float2(@float.x, @float.y), new float2(float2.x, @float.y));
			this.Line(new float2(float2.x, @float.y), new float2(float2.x, float2.y));
			this.Line(new float2(float2.x, float2.y), new float2(@float.x, float2.y));
			this.Line(new float2(@float.x, float2.y), new float2(@float.x, @float.y));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000783C File Offset: 0x00005A3C
		public void SolidRectangle(Rect rect)
		{
			this.draw.SolidPlane(new float3(rect.center.x, rect.center.y, 0f), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, new float2(rect.width, rect.height));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000078A0 File Offset: 0x00005AA0
		public void WireGrid(float2 center, int2 cells, float2 totalSize)
		{
			this.draw.WireGrid(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000078F9 File Offset: 0x00005AF9
		public void WireGrid(float3 center, int2 cells, float2 totalSize)
		{
			this.draw.WireGrid(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000791D File Offset: 0x00005B1D
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(Matrix4x4 matrix)
		{
			return this.draw.WithMatrix(matrix);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000792B File Offset: 0x00005B2B
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix WithMatrix(float3x3 matrix)
		{
			return this.draw.WithMatrix(matrix);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007939 File Offset: 0x00005B39
		[BurstDiscard]
		public CommandBuilder.ScopeColor WithColor(Color color)
		{
			return this.draw.WithColor(color);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007947 File Offset: 0x00005B47
		[BurstDiscard]
		public CommandBuilder.ScopePersist WithDuration(float duration)
		{
			return this.draw.WithDuration(duration);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007955 File Offset: 0x00005B55
		[BurstDiscard]
		public CommandBuilder.ScopeLineWidth WithLineWidth(float pixels, bool automaticJoins = true)
		{
			return this.draw.WithLineWidth(pixels, automaticJoins);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007964 File Offset: 0x00005B64
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix InLocalSpace(Transform transform)
		{
			return this.draw.InLocalSpace(transform);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007972 File Offset: 0x00005B72
		[BurstDiscard]
		public CommandBuilder.ScopeMatrix InScreenSpace(Camera camera)
		{
			return this.draw.InScreenSpace(camera);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007980 File Offset: 0x00005B80
		public void PushMatrix(Matrix4x4 matrix)
		{
			this.draw.PushMatrix(matrix);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000798E File Offset: 0x00005B8E
		public void PushMatrix(float4x4 matrix)
		{
			this.draw.PushMatrix(matrix);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000799C File Offset: 0x00005B9C
		public void PushSetMatrix(Matrix4x4 matrix)
		{
			this.draw.PushSetMatrix(matrix);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000079AA File Offset: 0x00005BAA
		public void PushSetMatrix(float4x4 matrix)
		{
			this.draw.PushSetMatrix(matrix);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000079B8 File Offset: 0x00005BB8
		public void PopMatrix()
		{
			this.draw.PopMatrix();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000079C5 File Offset: 0x00005BC5
		public void PushColor(Color color)
		{
			this.draw.PushColor(color);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000079D3 File Offset: 0x00005BD3
		public void PopColor()
		{
			this.draw.PopColor();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000079E0 File Offset: 0x00005BE0
		public void PushDuration(float duration)
		{
			this.draw.PushDuration(duration);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000079EE File Offset: 0x00005BEE
		public void PopDuration()
		{
			this.draw.PopDuration();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000079FB File Offset: 0x00005BFB
		[Obsolete("Renamed to PushDuration for consistency")]
		public void PushPersist(float duration)
		{
			this.draw.PushPersist(duration);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007A09 File Offset: 0x00005C09
		[Obsolete("Renamed to PopDuration for consistency")]
		public void PopPersist()
		{
			this.draw.PopPersist();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007A16 File Offset: 0x00005C16
		public void PushLineWidth(float pixels, bool automaticJoins = true)
		{
			this.draw.PushLineWidth(pixels, automaticJoins);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007A25 File Offset: 0x00005C25
		public void PopLineWidth()
		{
			this.draw.PopLineWidth();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007A32 File Offset: 0x00005C32
		public void Line(Vector3 a, Vector3 b)
		{
			this.draw.Line(a, b);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007A44 File Offset: 0x00005C44
		public void Line(Vector2 a, Vector2 b)
		{
			this.Line(this.xy ? new Vector3(a.x, a.y, 0f) : new Vector3(a.x, 0f, a.y), this.xy ? new Vector3(b.x, b.y, 0f) : new Vector3(b.x, 0f, b.y));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007AC3 File Offset: 0x00005CC3
		public void Line(Vector3 a, Vector3 b, Color color)
		{
			this.draw.Line(a, b, color);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007AD4 File Offset: 0x00005CD4
		public void Line(Vector2 a, Vector2 b, Color color)
		{
			this.Line(this.xy ? new Vector3(a.x, a.y, 0f) : new Vector3(a.x, 0f, a.y), this.xy ? new Vector3(b.x, b.y, 0f) : new Vector3(b.x, 0f, b.y), color);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007B54 File Offset: 0x00005D54
		public void Ray(float3 origin, float3 direction)
		{
			this.draw.Ray(origin, direction);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007B64 File Offset: 0x00005D64
		public void Ray(float2 origin, float2 direction)
		{
			this.Ray(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007BCD File Offset: 0x00005DCD
		public void Ray(Ray ray, float length)
		{
			this.draw.Ray(ray, length);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007BDC File Offset: 0x00005DDC
		public void Arc(float3 center, float3 start, float3 end)
		{
			this.draw.Arc(center, start, end);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007BEC File Offset: 0x00005DEC
		public void Arc(float2 center, float2 start, float2 end)
		{
			this.Arc(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(start, 0f) : new float3(start.x, 0f, start.y), this.xy ? new float3(end, 0f) : new float3(end.x, 0f, end.y));
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007C80 File Offset: 0x00005E80
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.draw.CircleXY(center, radius, startAngle, endAngle);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007C92 File Offset: 0x00005E92
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float2 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
			this.CircleXY(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007CC9 File Offset: 0x00005EC9
		public void SolidArc(float3 center, float3 start, float3 end)
		{
			this.draw.SolidArc(center, start, end);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007CDC File Offset: 0x00005EDC
		public void SolidArc(float2 center, float2 start, float2 end)
		{
			this.SolidArc(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(start, 0f) : new float3(start.x, 0f, start.y), this.xy ? new float3(end, 0f) : new float3(end.x, 0f, end.y));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007D70 File Offset: 0x00005F70
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007D7F File Offset: 0x00005F7F
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007D8E File Offset: 0x00005F8E
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007D9D File Offset: 0x00005F9D
		public void Polyline(NativeArray<float3> points, bool cycle = false)
		{
			this.draw.Polyline(points, cycle);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007DAC File Offset: 0x00005FAC
		public void DashedLine(float3 a, float3 b, float dash, float gap)
		{
			this.draw.DashedLine(a, b, dash, gap);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007DC0 File Offset: 0x00005FC0
		public void DashedLine(float2 a, float2 b, float dash, float gap)
		{
			this.DashedLine(this.xy ? new float3(a, 0f) : new float3(a.x, 0f, a.y), this.xy ? new float3(b, 0f) : new float3(b.x, 0f, b.y), dash, gap);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007E2C File Offset: 0x0000602C
		public void DashedPolyline(List<Vector3> points, float dash, float gap)
		{
			this.draw.DashedPolyline(points, dash, gap);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00007E3C File Offset: 0x0000603C
		public void Cross(float3 position, float size = 1f)
		{
			this.draw.Cross(position, size);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007E4B File Offset: 0x0000604B
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			this.draw.Bezier(p0, p1, p2, p3);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007E60 File Offset: 0x00006060
		public void Bezier(float2 p0, float2 p1, float2 p2, float2 p3)
		{
			this.Bezier(this.xy ? new float3(p0, 0f) : new float3(p0.x, 0f, p0.y), this.xy ? new float3(p1, 0f) : new float3(p1.x, 0f, p1.y), this.xy ? new float3(p2, 0f) : new float3(p2.x, 0f, p2.y), this.xy ? new float3(p3, 0f) : new float3(p3.x, 0f, p3.y));
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007F22 File Offset: 0x00006122
		public void CatmullRom(List<Vector3> points)
		{
			this.draw.CatmullRom(points);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007F30 File Offset: 0x00006130
		public void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3)
		{
			this.draw.CatmullRom(p0, p1, p2, p3);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007F44 File Offset: 0x00006144
		public void CatmullRom(float2 p0, float2 p1, float2 p2, float2 p3)
		{
			this.CatmullRom(this.xy ? new float3(p0, 0f) : new float3(p0.x, 0f, p0.y), this.xy ? new float3(p1, 0f) : new float3(p1.x, 0f, p1.y), this.xy ? new float3(p2, 0f) : new float3(p2.x, 0f, p2.y), this.xy ? new float3(p3, 0f) : new float3(p3.x, 0f, p3.y));
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008006 File Offset: 0x00006206
		public void Arrow(float3 from, float3 to)
		{
			this.ArrowRelativeSizeHead(from, to, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP, 0.2f);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000802C File Offset: 0x0000622C
		public void Arrow(float2 from, float2 to)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y));
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008095 File Offset: 0x00006295
		public void Arrow(float3 from, float3 to, float3 up, float headSize)
		{
			this.draw.Arrow(from, to, up, headSize);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000080A8 File Offset: 0x000062A8
		public void Arrow(float2 from, float2 to, float2 up, float headSize)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headSize);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000813E File Offset: 0x0000633E
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction)
		{
			this.draw.ArrowRelativeSizeHead(from, to, up, headFraction);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008150 File Offset: 0x00006350
		public void ArrowRelativeSizeHead(float2 from, float2 to, float2 up, float headFraction)
		{
			this.ArrowRelativeSizeHead(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headFraction);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000081E6 File Offset: 0x000063E6
		public void Arrowhead(float3 center, float3 direction, float radius)
		{
			this.Arrowhead(center, direction, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP, radius);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008208 File Offset: 0x00006408
		public void Arrowhead(float2 center, float2 direction, float radius)
		{
			this.Arrowhead(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), radius);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008272 File Offset: 0x00006472
		public void Arrowhead(float3 center, float3 direction, float3 up, float radius)
		{
			this.draw.Arrowhead(center, direction, up, radius);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008284 File Offset: 0x00006484
		public void Arrowhead(float2 center, float2 direction, float2 up, float radius)
		{
			this.Arrowhead(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), radius);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000831C File Offset: 0x0000651C
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width = 60f)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			Quaternion q = Quaternion.LookRotation(direction, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, q, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.draw.CircleXZInternal(float3.zero, offset, num, num2);
			float3 a = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 b = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 @float = new float3(0f, 0f, 1.4142f * offset);
			this.Line(a, @float);
			this.Line(@float, b);
			this.PopMatrix();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008428 File Offset: 0x00006628
		public void ArrowheadArc(float2 origin, float2 direction, float offset, float width = 60f)
		{
			this.ArrowheadArc(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), offset, width);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008494 File Offset: 0x00006694
		public void WireTriangle(float3 a, float3 b, float3 c)
		{
			this.draw.WireTriangle(a, b, c);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000084A4 File Offset: 0x000066A4
		public void WireTriangle(float2 a, float2 b, float2 c)
		{
			this.WireTriangle(this.xy ? new float3(a, 0f) : new float3(a.x, 0f, a.y), this.xy ? new float3(b, 0f) : new float3(b.x, 0f, b.y), this.xy ? new float3(c, 0f) : new float3(c.x, 0f, c.y));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008538 File Offset: 0x00006738
		public void WireRectangle(float3 center, quaternion rotation, float2 size)
		{
			this.draw.WireRectangle(center, rotation, size);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008548 File Offset: 0x00006748
		public void WireRectangle(float2 center, quaternion rotation, float2 size)
		{
			this.WireRectangle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), rotation, size);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000857D File Offset: 0x0000677D
		public void WireTriangle(float3 center, quaternion rotation, float radius)
		{
			this.draw.WireTriangle(center, rotation, radius);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000858D File Offset: 0x0000678D
		public void WireTriangle(float2 center, quaternion rotation, float radius)
		{
			this.WireTriangle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), rotation, radius);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000085C2 File Offset: 0x000067C2
		public void SolidTriangle(float3 a, float3 b, float3 c)
		{
			this.draw.SolidTriangle(a, b, c);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000085D4 File Offset: 0x000067D4
		public void SolidTriangle(float2 a, float2 b, float2 c)
		{
			this.SolidTriangle(this.xy ? new float3(a, 0f) : new float3(a.x, 0f, a.y), this.xy ? new float3(b, 0f) : new float3(b.x, 0f, b.y), this.xy ? new float3(c, 0f) : new float3(c.x, 0f, c.y));
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008668 File Offset: 0x00006868
		public void Label2D(float3 position, string text, float sizeInPixels = 14f)
		{
			this.draw.Label2D(position, text, sizeInPixels);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008678 File Offset: 0x00006878
		public void Label2D(float2 position, string text, float sizeInPixels = 14f)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), text, sizeInPixels);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000086AD File Offset: 0x000068AD
		public void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment)
		{
			this.draw.Label2D(position, text, sizeInPixels, alignment);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000086BF File Offset: 0x000068BF
		public void Label2D(float2 position, string text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), text, sizeInPixels, alignment);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000086F6 File Offset: 0x000068F6
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels = 14f)
		{
			this.draw.Label2D(position, ref text, sizeInPixels);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008706 File Offset: 0x00006906
		public void Label2D(float2 position, ref FixedString32Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000873B File Offset: 0x0000693B
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels = 14f)
		{
			this.draw.Label2D(position, ref text, sizeInPixels);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000874B File Offset: 0x0000694B
		public void Label2D(float2 position, ref FixedString64Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008780 File Offset: 0x00006980
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels = 14f)
		{
			this.draw.Label2D(position, ref text, sizeInPixels);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008790 File Offset: 0x00006990
		public void Label2D(float2 position, ref FixedString128Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000087C5 File Offset: 0x000069C5
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels = 14f)
		{
			this.draw.Label2D(position, ref text, sizeInPixels);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000087D5 File Offset: 0x000069D5
		public void Label2D(float2 position, ref FixedString512Bytes text, float sizeInPixels = 14f)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000880A File Offset: 0x00006A0A
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000881C File Offset: 0x00006A1C
		public void Label2D(float2 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008853 File Offset: 0x00006A53
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008865 File Offset: 0x00006A65
		public void Label2D(float2 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000889C File Offset: 0x00006A9C
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000088AE File Offset: 0x00006AAE
		public void Label2D(float2 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000088E5 File Offset: 0x00006AE5
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000088F7 File Offset: 0x00006AF7
		public void Label2D(float2 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000892E File Offset: 0x00006B2E
		public void Ray(float3 origin, float3 direction, Color color)
		{
			this.draw.Ray(origin, direction, color);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008940 File Offset: 0x00006B40
		public void Ray(float2 origin, float2 direction, Color color)
		{
			this.Ray(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), color);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000089AA File Offset: 0x00006BAA
		public void Ray(Ray ray, float length, Color color)
		{
			this.draw.Ray(ray, length, color);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000089BA File Offset: 0x00006BBA
		public void Arc(float3 center, float3 start, float3 end, Color color)
		{
			this.draw.Arc(center, start, end, color);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000089CC File Offset: 0x00006BCC
		public void Arc(float2 center, float2 start, float2 end, Color color)
		{
			this.Arc(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(start, 0f) : new float3(start.x, 0f, start.y), this.xy ? new float3(end, 0f) : new float3(end.x, 0f, end.y), color);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008A62 File Offset: 0x00006C62
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.draw.CircleXY(center, radius, startAngle, endAngle, color);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008A76 File Offset: 0x00006C76
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float3 center, float radius, Color color)
		{
			this.CircleXY(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008A8B File Offset: 0x00006C8B
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float2 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.CircleXY(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle, color);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008AC4 File Offset: 0x00006CC4
		[Obsolete("Use Draw.xy.Circle instead")]
		public void CircleXY(float2 center, float radius, Color color)
		{
			this.CircleXY(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00008AD9 File Offset: 0x00006CD9
		public void SolidArc(float3 center, float3 start, float3 end, Color color)
		{
			this.draw.SolidArc(center, start, end, color);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008AEC File Offset: 0x00006CEC
		public void SolidArc(float2 center, float2 start, float2 end, Color color)
		{
			this.SolidArc(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(start, 0f) : new float3(start.x, 0f, start.y), this.xy ? new float3(end, 0f) : new float3(end.x, 0f, end.y), color);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008B82 File Offset: 0x00006D82
		[BurstDiscard]
		public void Polyline(List<Vector3> points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008B92 File Offset: 0x00006D92
		[BurstDiscard]
		public void Polyline(List<Vector3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008B9D File Offset: 0x00006D9D
		[BurstDiscard]
		public void Polyline(Vector3[] points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008BAD File Offset: 0x00006DAD
		[BurstDiscard]
		public void Polyline(Vector3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008BB8 File Offset: 0x00006DB8
		[BurstDiscard]
		public void Polyline(float3[] points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008BC8 File Offset: 0x00006DC8
		[BurstDiscard]
		public void Polyline(float3[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008BD3 File Offset: 0x00006DD3
		public void Polyline(NativeArray<float3> points, bool cycle, Color color)
		{
			this.draw.Polyline(points, cycle, color);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008BE3 File Offset: 0x00006DE3
		public void Polyline(NativeArray<float3> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008BEE File Offset: 0x00006DEE
		public void DashedLine(float3 a, float3 b, float dash, float gap, Color color)
		{
			this.draw.DashedLine(a, b, dash, gap, color);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008C04 File Offset: 0x00006E04
		public void DashedLine(float2 a, float2 b, float dash, float gap, Color color)
		{
			this.DashedLine(this.xy ? new float3(a, 0f) : new float3(a.x, 0f, a.y), this.xy ? new float3(b, 0f) : new float3(b.x, 0f, b.y), dash, gap, color);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008C72 File Offset: 0x00006E72
		public void DashedPolyline(List<Vector3> points, float dash, float gap, Color color)
		{
			this.draw.DashedPolyline(points, dash, gap, color);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008C84 File Offset: 0x00006E84
		public void Cross(float3 position, float size, Color color)
		{
			this.draw.Cross(position, size, color);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008C94 File Offset: 0x00006E94
		public void Cross(float3 position, Color color)
		{
			this.Cross(position, 1f, color);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008CA3 File Offset: 0x00006EA3
		public void Bezier(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
			this.draw.Bezier(p0, p1, p2, p3, color);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008CB8 File Offset: 0x00006EB8
		public void Bezier(float2 p0, float2 p1, float2 p2, float2 p3, Color color)
		{
			this.Bezier(this.xy ? new float3(p0, 0f) : new float3(p0.x, 0f, p0.y), this.xy ? new float3(p1, 0f) : new float3(p1.x, 0f, p1.y), this.xy ? new float3(p2, 0f) : new float3(p2.x, 0f, p2.y), this.xy ? new float3(p3, 0f) : new float3(p3.x, 0f, p3.y), color);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008D7C File Offset: 0x00006F7C
		public void CatmullRom(List<Vector3> points, Color color)
		{
			this.draw.CatmullRom(points, color);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008D8B File Offset: 0x00006F8B
		public void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
			this.draw.CatmullRom(p0, p1, p2, p3, color);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008DA0 File Offset: 0x00006FA0
		public void CatmullRom(float2 p0, float2 p1, float2 p2, float2 p3, Color color)
		{
			this.CatmullRom(this.xy ? new float3(p0, 0f) : new float3(p0.x, 0f, p0.y), this.xy ? new float3(p1, 0f) : new float3(p1.x, 0f, p1.y), this.xy ? new float3(p2, 0f) : new float3(p2.x, 0f, p2.y), this.xy ? new float3(p3, 0f) : new float3(p3.x, 0f, p3.y), color);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008E64 File Offset: 0x00007064
		public void Arrow(float3 from, float3 to, Color color)
		{
			this.ArrowRelativeSizeHead(from, to, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP, 0.2f, color);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008E88 File Offset: 0x00007088
		public void Arrow(float2 from, float2 to, Color color)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), color);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008EF2 File Offset: 0x000070F2
		public void Arrow(float3 from, float3 to, float3 up, float headSize, Color color)
		{
			this.draw.Arrow(from, to, up, headSize, color);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008F08 File Offset: 0x00007108
		public void Arrow(float2 from, float2 to, float2 up, float headSize, Color color)
		{
			this.Arrow(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headSize, color);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008FA0 File Offset: 0x000071A0
		public void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction, Color color)
		{
			this.draw.ArrowRelativeSizeHead(from, to, up, headFraction, color);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008FB4 File Offset: 0x000071B4
		public void ArrowRelativeSizeHead(float2 from, float2 to, float2 up, float headFraction, Color color)
		{
			this.ArrowRelativeSizeHead(this.xy ? new float3(from, 0f) : new float3(from.x, 0f, from.y), this.xy ? new float3(to, 0f) : new float3(to.x, 0f, to.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), headFraction, color);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000904C File Offset: 0x0000724C
		public void Arrowhead(float3 center, float3 direction, float radius, Color color)
		{
			this.Arrowhead(center, direction, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP, radius, color);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009070 File Offset: 0x00007270
		public void Arrowhead(float2 center, float2 direction, float radius, Color color)
		{
			this.Arrowhead(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), radius, color);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000090DC File Offset: 0x000072DC
		public void Arrowhead(float3 center, float3 direction, float3 up, float radius, Color color)
		{
			this.draw.Arrowhead(center, direction, up, radius, color);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000090F0 File Offset: 0x000072F0
		public void Arrowhead(float2 center, float2 direction, float2 up, float radius, Color color)
		{
			this.Arrowhead(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), this.xy ? new float3(up, 0f) : new float3(up.x, 0f, up.y), radius, color);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009188 File Offset: 0x00007388
		public void ArrowheadArc(float3 origin, float3 direction, float offset, float width, Color color)
		{
			if (!math.any(direction))
			{
				return;
			}
			if (offset < 0f)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset == 0f)
			{
				return;
			}
			this.draw.PushColor(color);
			Quaternion q = Quaternion.LookRotation(direction, this.xy ? CommandBuilder2D.XY_UP : CommandBuilder2D.XZ_UP);
			this.PushMatrix(Matrix4x4.TRS(origin, q, Vector3.one));
			float num = 1.5707964f - width * 0.008726646f;
			float num2 = 1.5707964f + width * 0.008726646f;
			this.draw.CircleXZInternal(float3.zero, offset, num, num2);
			float3 a = new float3(math.cos(num), 0f, math.sin(num)) * offset;
			float3 b = new float3(math.cos(num2), 0f, math.sin(num2)) * offset;
			float3 @float = new float3(0f, 0f, 1.4142f * offset);
			this.Line(a, @float);
			this.Line(@float, b);
			this.PopMatrix();
			this.draw.PopColor();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000092AB File Offset: 0x000074AB
		public void ArrowheadArc(float3 origin, float3 direction, float offset, Color color)
		{
			this.ArrowheadArc(origin, direction, offset, 60f, color);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000092C0 File Offset: 0x000074C0
		public void ArrowheadArc(float2 origin, float2 direction, float offset, float width, Color color)
		{
			this.ArrowheadArc(this.xy ? new float3(origin, 0f) : new float3(origin.x, 0f, origin.y), this.xy ? new float3(direction, 0f) : new float3(direction.x, 0f, direction.y), offset, width, color);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000932E File Offset: 0x0000752E
		public void ArrowheadArc(float2 origin, float2 direction, float offset, Color color)
		{
			this.ArrowheadArc(origin, direction, offset, 60f, color);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009340 File Offset: 0x00007540
		public void WireTriangle(float3 a, float3 b, float3 c, Color color)
		{
			this.draw.WireTriangle(a, b, c, color);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009354 File Offset: 0x00007554
		public void WireTriangle(float2 a, float2 b, float2 c, Color color)
		{
			this.WireTriangle(this.xy ? new float3(a, 0f) : new float3(a.x, 0f, a.y), this.xy ? new float3(b, 0f) : new float3(b.x, 0f, b.y), this.xy ? new float3(c, 0f) : new float3(c.x, 0f, c.y), color);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000093EA File Offset: 0x000075EA
		public void WireRectangle(float3 center, quaternion rotation, float2 size, Color color)
		{
			this.draw.WireRectangle(center, rotation, size, color);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000093FC File Offset: 0x000075FC
		public void WireRectangle(float2 center, quaternion rotation, float2 size, Color color)
		{
			this.WireRectangle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), rotation, size, color);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009433 File Offset: 0x00007633
		public void WireTriangle(float3 center, quaternion rotation, float radius, Color color)
		{
			this.draw.WireTriangle(center, rotation, radius, color);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009445 File Offset: 0x00007645
		public void WireTriangle(float2 center, quaternion rotation, float radius, Color color)
		{
			this.WireTriangle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), rotation, radius, color);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000947C File Offset: 0x0000767C
		public void SolidTriangle(float3 a, float3 b, float3 c, Color color)
		{
			this.draw.SolidTriangle(a, b, c, color);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009490 File Offset: 0x00007690
		public void SolidTriangle(float2 a, float2 b, float2 c, Color color)
		{
			this.SolidTriangle(this.xy ? new float3(a, 0f) : new float3(a.x, 0f, a.y), this.xy ? new float3(b, 0f) : new float3(b.x, 0f, b.y), this.xy ? new float3(c, 0f) : new float3(c.x, 0f, c.y), color);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009526 File Offset: 0x00007726
		public void Label2D(float3 position, string text, float sizeInPixels, Color color)
		{
			this.draw.Label2D(position, text, sizeInPixels, color);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009538 File Offset: 0x00007738
		public void Label2D(float3 position, string text, Color color)
		{
			this.Label2D(position, text, 14f, color);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009548 File Offset: 0x00007748
		public void Label2D(float2 position, string text, float sizeInPixels, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), text, sizeInPixels, color);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000957F File Offset: 0x0000777F
		public void Label2D(float2 position, string text, Color color)
		{
			this.Label2D(position, text, 14f, color);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000958F File Offset: 0x0000778F
		public void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.draw.Label2D(position, text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000095A3 File Offset: 0x000077A3
		public void Label2D(float2 position, string text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000095DC File Offset: 0x000077DC
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, color);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000095EE File Offset: 0x000077EE
		public void Label2D(float3 position, ref FixedString32Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000095FE File Offset: 0x000077FE
		public void Label2D(float2 position, ref FixedString32Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, color);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009635 File Offset: 0x00007835
		public void Label2D(float2 position, ref FixedString32Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009645 File Offset: 0x00007845
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, color);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009657 File Offset: 0x00007857
		public void Label2D(float3 position, ref FixedString64Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009667 File Offset: 0x00007867
		public void Label2D(float2 position, ref FixedString64Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, color);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000969E File Offset: 0x0000789E
		public void Label2D(float2 position, ref FixedString64Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000096AE File Offset: 0x000078AE
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, color);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000096C0 File Offset: 0x000078C0
		public void Label2D(float3 position, ref FixedString128Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000096D0 File Offset: 0x000078D0
		public void Label2D(float2 position, ref FixedString128Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, color);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009707 File Offset: 0x00007907
		public void Label2D(float2 position, ref FixedString128Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00009717 File Offset: 0x00007917
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, color);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009729 File Offset: 0x00007929
		public void Label2D(float3 position, ref FixedString512Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00009739 File Offset: 0x00007939
		public void Label2D(float2 position, ref FixedString512Bytes text, float sizeInPixels, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, color);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009770 File Offset: 0x00007970
		public void Label2D(float2 position, ref FixedString512Bytes text, Color color)
		{
			this.Label2D(position, ref text, 14f, color);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009780 File Offset: 0x00007980
		public void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009794 File Offset: 0x00007994
		public void Label2D(float2 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000097CD File Offset: 0x000079CD
		public void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000097E1 File Offset: 0x000079E1
		public void Label2D(float2 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000981A File Offset: 0x00007A1A
		public void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000982E File Offset: 0x00007A2E
		public void Label2D(float2 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00009867 File Offset: 0x00007A67
		public void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.draw.Label2D(position, ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000987B File Offset: 0x00007A7B
		public void Label2D(float2 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
			this.Label2D(this.xy ? new float3(position, 0f) : new float3(position.x, 0f, position.y), ref text, sizeInPixels, alignment, color);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000098B4 File Offset: 0x00007AB4
		public void Line(float3 a, float3 b, Color color)
		{
			this.draw.Line(a, b, color);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000098C4 File Offset: 0x00007AC4
		public void Circle(float2 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.Circle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle, color);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000098FD File Offset: 0x00007AFD
		public void Circle(float2 center, float radius, Color color)
		{
			this.Circle(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009914 File Offset: 0x00007B14
		public void Circle(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.draw.PushColor(color);
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
				this.draw.CircleXZInternal(new float3(center.x, center.z, center.y), radius, startAngle, endAngle);
				this.draw.PopMatrix();
			}
			else
			{
				this.draw.CircleXZInternal(center, radius, startAngle, endAngle);
			}
			this.draw.PopColor();
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009994 File Offset: 0x00007B94
		public void Circle(float3 center, float radius, Color color)
		{
			this.Circle(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000099A9 File Offset: 0x00007BA9
		public void SolidCircle(float2 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.SolidCircle(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), radius, startAngle, endAngle, color);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000099E2 File Offset: 0x00007BE2
		public void SolidCircle(float2 center, float radius, Color color)
		{
			this.SolidCircle(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000099F8 File Offset: 0x00007BF8
		public void SolidCircle(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
			this.draw.PushColor(color);
			if (this.xy)
			{
				this.draw.PushMatrix(CommandBuilder2D.XZ_TO_XY_MATRIX);
			}
			this.draw.SolidCircleXZInternal(new float3(center.x, -center.z, center.y), radius, startAngle, endAngle);
			if (this.xy)
			{
				this.draw.PopMatrix();
			}
			this.draw.PopColor();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00009A6F File Offset: 0x00007C6F
		public void SolidCircle(float3 center, float radius, Color color)
		{
			this.SolidCircle(center, radius, 0f, 6.2831855f, color);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00009A84 File Offset: 0x00007C84
		public void WirePill(float2 a, float2 b, float radius, Color color)
		{
			this.WirePill(a, b - a, math.length(b - a), radius, color);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009AA4 File Offset: 0x00007CA4
		public void WirePill(float2 position, float2 direction, float length, float radius, Color color)
		{
			this.draw.PushColor(color);
			direction = math.normalizesafe(direction, default(float2));
			if (radius <= 0f)
			{
				this.Line(position, position + direction * length);
			}
			else if (length <= 0f || math.all(direction == 0f))
			{
				this.Circle(position, radius, 0f, 6.2831855f);
			}
			else
			{
				float4x4 matrix;
				if (this.xy)
				{
					matrix = new float4x4(new float4(direction, 0f, 0f), new float4(math.cross(new float3(direction, 0f), CommandBuilder2D.XY_UP), 0f), new float4(0f, 0f, 1f, 0f), new float4(position, 0f, 1f));
				}
				else
				{
					matrix = new float4x4(new float4(direction.x, 0f, direction.y, 0f), new float4(0f, 1f, 0f, 0f), new float4(math.cross(new float3(direction.x, 0f, direction.y), CommandBuilder2D.XZ_UP), 0f), new float4(position.x, 0f, position.y, 1f));
				}
				this.draw.PushMatrix(matrix);
				this.Circle(new float2(0f, 0f), radius, 1.5707964f, 4.712389f);
				this.Line(new float2(0f, -radius), new float2(length, -radius));
				this.Circle(new float2(length, 0f), radius, -1.5707964f, 1.5707964f);
				this.Line(new float2(0f, radius), new float2(length, radius));
				this.draw.PopMatrix();
			}
			this.draw.PopColor();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00009CA8 File Offset: 0x00007EA8
		[BurstDiscard]
		public void Polyline(List<Vector2> points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Count - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Count > 1)
			{
				this.Line(points[points.Count - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00009D1C File Offset: 0x00007F1C
		[BurstDiscard]
		public void Polyline(List<Vector2> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009D28 File Offset: 0x00007F28
		[BurstDiscard]
		public void Polyline(Vector2[] points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009D93 File Offset: 0x00007F93
		[BurstDiscard]
		public void Polyline(Vector2[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009DA0 File Offset: 0x00007FA0
		[BurstDiscard]
		public void Polyline(float2[] points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009E0B File Offset: 0x0000800B
		[BurstDiscard]
		public void Polyline(float2[] points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009E18 File Offset: 0x00008018
		public void Polyline(NativeArray<float2> points, bool cycle, Color color)
		{
			this.draw.PushColor(color);
			for (int i = 0; i < points.Length - 1; i++)
			{
				this.Line(points[i], points[i + 1]);
			}
			if (cycle && points.Length > 1)
			{
				this.Line(points[points.Length - 1], points[0]);
			}
			this.draw.PopColor();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009E93 File Offset: 0x00008093
		public void Polyline(NativeArray<float2> points, Color color)
		{
			this.Polyline(points, false, color);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009EA0 File Offset: 0x000080A0
		public void Cross(float2 position, float size, Color color)
		{
			this.draw.PushColor(color);
			size *= 0.5f;
			this.Line(position - new float2(size, 0f), position + new float2(size, 0f));
			this.Line(position - new float2(0f, size), position + new float2(0f, size));
			this.draw.PopColor();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009F1D File Offset: 0x0000811D
		public void Cross(float2 position, Color color)
		{
			this.Cross(position, 1f, color);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009F2C File Offset: 0x0000812C
		public void WireRectangle(float3 center, float2 size, Color color)
		{
			this.draw.WirePlane(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, size, color);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009F50 File Offset: 0x00008150
		public void WireRectangle(Rect rect, Color color)
		{
			this.draw.PushColor(color);
			float2 @float = rect.min;
			float2 float2 = rect.max;
			this.Line(new float2(@float.x, @float.y), new float2(float2.x, @float.y));
			this.Line(new float2(float2.x, @float.y), new float2(float2.x, float2.y));
			this.Line(new float2(float2.x, float2.y), new float2(@float.x, float2.y));
			this.Line(new float2(@float.x, float2.y), new float2(@float.x, @float.y));
			this.draw.PopColor();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A030 File Offset: 0x00008230
		public void SolidRectangle(Rect rect, Color color)
		{
			this.draw.SolidPlane(new float3(rect.center.x, rect.center.y, 0f), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, new float2(rect.width, rect.height), color);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A094 File Offset: 0x00008294
		public void WireGrid(float2 center, int2 cells, float2 totalSize, Color color)
		{
			this.draw.WireGrid(this.xy ? new float3(center, 0f) : new float3(center.x, 0f, center.y), this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize, color);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000A0EF File Offset: 0x000082EF
		public void WireGrid(float3 center, int2 cells, float2 totalSize, Color color)
		{
			this.draw.WireGrid(center, this.xy ? CommandBuilder2D.XY_TO_XZ_ROTATION : CommandBuilder2D.XZ_TO_XZ_ROTATION, cells, totalSize, color);
		}

		// Token: 0x04000073 RID: 115
		private CommandBuilder draw;

		// Token: 0x04000074 RID: 116
		private bool xy;

		// Token: 0x04000075 RID: 117
		private static readonly float3 XY_UP = new float3(0f, 0f, 1f);

		// Token: 0x04000076 RID: 118
		private static readonly float3 XZ_UP = new float3(0f, 1f, 0f);

		// Token: 0x04000077 RID: 119
		private static readonly quaternion XY_TO_XZ_ROTATION = quaternion.RotateX(-1.5707964f);

		// Token: 0x04000078 RID: 120
		private static readonly quaternion XZ_TO_XZ_ROTATION = quaternion.identity;

		// Token: 0x04000079 RID: 121
		private static readonly float4x4 XZ_TO_XY_MATRIX = new float4x4(new float4(1f, 0f, 0f, 0f), new float4(0f, 0f, 1f, 0f), new float4(0f, 1f, 0f, 0f), new float4(0f, 0f, 0f, 1f));
	}
}
