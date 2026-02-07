using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Drawing
{
	// Token: 0x02000027 RID: 39
	public static class Draw
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000A1DE File Offset: 0x000083DE
		public static ref CommandBuilder ingame
		{
			get
			{
				DrawingManager.Init();
				return ref Draw.ingame_builder;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A1EA File Offset: 0x000083EA
		public static ref CommandBuilder editor
		{
			get
			{
				DrawingManager.Init();
				return ref Draw.builder;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000A1F6 File Offset: 0x000083F6
		public static CommandBuilder2D xy
		{
			get
			{
				DrawingManager.Init();
				return new CommandBuilder2D(Draw.builder, true);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A208 File Offset: 0x00008408
		public static CommandBuilder2D xz
		{
			get
			{
				DrawingManager.Init();
				return new CommandBuilder2D(Draw.builder, false);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A21C File Offset: 0x0000841C
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithMatrix(Matrix4x4 matrix)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A234 File Offset: 0x00008434
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithMatrix(float3x3 matrix)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A24C File Offset: 0x0000844C
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithColor(Color color)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A264 File Offset: 0x00008464
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithDuration(float duration)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A27C File Offset: 0x0000847C
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty WithLineWidth(float pixels, bool automaticJoins = true)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A294 File Offset: 0x00008494
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty InLocalSpace(Transform transform)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A2AC File Offset: 0x000084AC
		[BurstDiscard]
		public static CommandBuilder.ScopeEmpty InScreenSpace(Camera camera)
		{
			return default(CommandBuilder.ScopeEmpty);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushMatrix(Matrix4x4 matrix)
		{
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushMatrix(float4x4 matrix)
		{
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushSetMatrix(Matrix4x4 matrix)
		{
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushSetMatrix(float4x4 matrix)
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PopMatrix()
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushColor(Color color)
		{
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PopColor()
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushDuration(float duration)
		{
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PopDuration()
		{
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Renamed to PushDuration for consistency")]
		public static void PushPersist(float duration)
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Renamed to PopDuration for consistency")]
		public static void PopPersist()
		{
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PushLineWidth(float pixels, bool automaticJoins = true)
		{
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PopLineWidth()
		{
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(float3 a, float3 b)
		{
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(Vector3 a, Vector3 b)
		{
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(Vector3 a, Vector3 b, Color color)
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(float3 origin, float3 direction)
		{
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(Ray ray, float length)
		{
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arc(float3 center, float3 start, float3 end)
		{
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Circle instead")]
		public static void CircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Circle instead")]
		public static void CircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Circle(float3 center, float3 normal, float radius)
		{
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidArc(float3 center, float3 start, float3 end)
		{
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public static void SolidCircleXZ(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public static void SolidCircleXY(float3 center, float radius, float startAngle = 0f, float endAngle = 6.2831855f)
		{
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidCircle(float3 center, float3 normal, float radius)
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SphereOutline(float3 center, float radius)
		{
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 bottom, float3 top, float radius)
		{
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 position, float3 up, float height, float radius)
		{
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 start, float3 end, float radius)
		{
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 position, float3 direction, float length, float radius)
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireSphere(float3 position, float radius)
		{
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(List<Vector3> points, bool cycle = false)
		{
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(Vector3[] points, bool cycle = false)
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(float3[] points, bool cycle = false)
		{
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(NativeArray<float3> points, bool cycle = false)
		{
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void DashedLine(float3 a, float3 b, float dash, float gap)
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void DashedPolyline(List<Vector3> points, float dash, float gap)
		{
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, float3 size)
		{
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, quaternion rotation, float3 size)
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(Bounds bounds)
		{
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(Mesh mesh)
		{
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles)
		{
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidMesh(Mesh mesh)
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidMesh(List<Vector3> vertices, List<int> triangles, List<Color> colors)
		{
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidMesh(Vector3[] vertices, int[] triangles, Color[] colors, int vertexCount, int indexCount)
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Cross(float3 position, float size = 1f)
		{
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Cross instead")]
		public static void CrossXZ(float3 position, float size = 1f)
		{
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Cross instead")]
		public static void CrossXY(float3 position, float size = 1f)
		{
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Bezier(float3 p0, float3 p1, float3 p2, float3 p3)
		{
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void CatmullRom(List<Vector3> points)
		{
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3)
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to)
		{
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to, float3 up, float headSize)
		{
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction)
		{
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrowhead(float3 center, float3 direction, float radius)
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrowhead(float3 center, float3 direction, float3 up, float radius)
		{
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowheadArc(float3 origin, float3 direction, float offset, float width = 60f)
		{
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize)
		{
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireTriangle(float3 a, float3 b, float3 c)
		{
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.WireRectangle instead")]
		public static void WireRectangleXZ(float3 center, float2 size)
		{
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireRectangle(float3 center, quaternion rotation, float2 size)
		{
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public static void WireRectangle(Rect rect)
		{
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireTriangle(float3 center, quaternion rotation, float radius)
		{
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePentagon(float3 center, quaternion rotation, float radius)
		{
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireHexagon(float3 center, quaternion rotation, float radius)
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePolygon(float3 center, int vertices, quaternion rotation, float radius)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.SolidRectangle instead")]
		public static void SolidRectangle(Rect rect)
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidPlane(float3 center, float3 normal, float2 size)
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidPlane(float3 center, quaternion rotation, float2 size)
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, float3 normal, float2 size)
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, quaternion rotation, float2 size)
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PlaneWithNormal(float3 center, float3 normal, float2 size)
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PlaneWithNormal(float3 center, quaternion rotation, float2 size)
		{
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidTriangle(float3 a, float3 b, float3 c)
		{
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, float3 size)
		{
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(Bounds bounds)
		{
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, quaternion rotation, float3 size)
		{
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, string text, float size)
		{
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, string text, float size, LabelAlignment alignment)
		{
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, string text, float sizeInPixels = 14f)
		{
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment)
		{
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels = 14f)
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels = 14f)
		{
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels = 14f)
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels = 14f)
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment)
		{
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size)
		{
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size)
		{
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size)
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size)
		{
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, LabelAlignment alignment)
		{
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, LabelAlignment alignment)
		{
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, LabelAlignment alignment)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, LabelAlignment alignment)
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Line(float3 a, float3 b, Color color)
		{
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(float3 origin, float3 direction, Color color)
		{
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Ray(Ray ray, float length, Color color)
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arc(float3 center, float3 start, float3 end, Color color)
		{
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Circle instead")]
		public static void CircleXZ(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Circle instead")]
		public static void CircleXZ(float3 center, float radius, Color color)
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Circle instead")]
		public static void CircleXY(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Circle instead")]
		public static void CircleXY(float3 center, float radius, Color color)
		{
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Circle(float3 center, float3 normal, float radius, Color color)
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidArc(float3 center, float3 start, float3 end, Color color)
		{
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public static void SolidCircleXZ(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.SolidCircle instead")]
		public static void SolidCircleXZ(float3 center, float radius, Color color)
		{
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public static void SolidCircleXY(float3 center, float radius, float startAngle, float endAngle, Color color)
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.SolidCircle instead")]
		public static void SolidCircleXY(float3 center, float radius, Color color)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidCircle(float3 center, float3 normal, float radius, Color color)
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SphereOutline(float3 center, float radius, Color color)
		{
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 bottom, float3 top, float radius, Color color)
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCylinder(float3 position, float3 up, float height, float radius, Color color)
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 start, float3 end, float radius, Color color)
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireCapsule(float3 position, float3 direction, float length, float radius, Color color)
		{
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireSphere(float3 position, float radius, Color color)
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(List<Vector3> points, bool cycle, Color color)
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(List<Vector3> points, Color color)
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(Vector3[] points, bool cycle, Color color)
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(Vector3[] points, Color color)
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(float3[] points, bool cycle, Color color)
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(float3[] points, Color color)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(NativeArray<float3> points, bool cycle, Color color)
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Polyline(NativeArray<float3> points, Color color)
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void DashedLine(float3 a, float3 b, float dash, float gap, Color color)
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void DashedPolyline(List<Vector3> points, float dash, float gap, Color color)
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, float3 size, Color color)
		{
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(float3 center, quaternion rotation, float3 size, Color color)
		{
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireBox(Bounds bounds, Color color)
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(Mesh mesh, Color color)
		{
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireMesh(NativeArray<float3> vertices, NativeArray<int> triangles, Color color)
		{
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidMesh(Mesh mesh, Color color)
		{
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Cross(float3 position, float size, Color color)
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Cross(float3 position, Color color)
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Cross instead")]
		public static void CrossXZ(float3 position, float size, Color color)
		{
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.Cross instead")]
		public static void CrossXZ(float3 position, Color color)
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Cross instead")]
		public static void CrossXY(float3 position, float size, Color color)
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.Cross instead")]
		public static void CrossXY(float3 position, Color color)
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Bezier(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void CatmullRom(List<Vector3> points, Color color)
		{
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3, Color color)
		{
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to, Color color)
		{
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrow(float3 from, float3 to, float3 up, float headSize, Color color)
		{
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowRelativeSizeHead(float3 from, float3 to, float3 up, float headFraction, Color color)
		{
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrowhead(float3 center, float3 direction, float radius, Color color)
		{
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Arrowhead(float3 center, float3 direction, float3 up, float radius, Color color)
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowheadArc(float3 origin, float3 direction, float offset, float width, Color color)
		{
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void ArrowheadArc(float3 origin, float3 direction, float offset, Color color)
		{
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireGrid(float3 center, quaternion rotation, int2 cells, float2 totalSize, Color color)
		{
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireTriangle(float3 a, float3 b, float3 c, Color color)
		{
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xz.WireRectangle instead")]
		public static void WireRectangleXZ(float3 center, float2 size, Color color)
		{
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireRectangle(float3 center, quaternion rotation, float2 size, Color color)
		{
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.WireRectangle instead")]
		public static void WireRectangle(Rect rect, Color color)
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireTriangle(float3 center, quaternion rotation, float radius, Color color)
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePentagon(float3 center, quaternion rotation, float radius, Color color)
		{
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WireHexagon(float3 center, quaternion rotation, float radius, Color color)
		{
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePolygon(float3 center, int vertices, quaternion rotation, float radius, Color color)
		{
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		[Obsolete("Use Draw.xy.SolidRectangle instead")]
		public static void SolidRectangle(Rect rect, Color color)
		{
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidPlane(float3 center, float3 normal, float2 size, Color color)
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidPlane(float3 center, quaternion rotation, float2 size, Color color)
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, float3 normal, float2 size, Color color)
		{
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void WirePlane(float3 center, quaternion rotation, float2 size, Color color)
		{
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PlaneWithNormal(float3 center, float3 normal, float2 size, Color color)
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void PlaneWithNormal(float3 center, quaternion rotation, float2 size, Color color)
		{
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidTriangle(float3 a, float3 b, float3 c, Color color)
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, float3 size, Color color)
		{
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(Bounds bounds, Color color)
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void SolidBox(float3 center, quaternion rotation, float3 size, Color color)
		{
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, string text, float size, Color color)
		{
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, string text, float size, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, string text, float sizeInPixels, Color color)
		{
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, string text, Color color)
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, string text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, Color color)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString32Bytes text, Color color)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, Color color)
		{
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString64Bytes text, Color color)
		{
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, Color color)
		{
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString128Bytes text, Color color)
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, Color color)
		{
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString512Bytes text, Color color)
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString32Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString64Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString128Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label2D(float3 position, ref FixedString512Bytes text, float sizeInPixels, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, Color color)
		{
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, Color color)
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, Color color)
		{
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, Color color)
		{
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString32Bytes text, float size, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString64Bytes text, float size, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString128Bytes text, float size, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00002104 File Offset: 0x00000304
		[BurstDiscard]
		public static void Label3D(float3 position, quaternion rotation, ref FixedString512Bytes text, float size, LabelAlignment alignment, Color color)
		{
		}

		// Token: 0x0400007A RID: 122
		internal static CommandBuilder builder;

		// Token: 0x0400007B RID: 123
		internal static CommandBuilder ingame_builder;
	}
}
