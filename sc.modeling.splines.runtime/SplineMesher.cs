using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.Splines.Interpolators;

namespace sc.modeling.splines.runtime
{
	// Token: 0x02000004 RID: 4
	[ExecuteInEditMode]
	[AddComponentMenu("Splines/Spline Mesher")]
	[HelpURL("https://staggart.xyz/sm-docs/")]
	[SelectionBase]
	public class SplineMesher : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000004 RID: 4 RVA: 0x00002118 File Offset: 0x00000318
		// (remove) Token: 0x06000005 RID: 5 RVA: 0x0000214C File Offset: 0x0000034C
		public static event SplineMesher.Action onPreRebuildMesh;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000006 RID: 6 RVA: 0x00002180 File Offset: 0x00000380
		// (remove) Token: 0x06000007 RID: 7 RVA: 0x000021B4 File Offset: 0x000003B4
		public static event SplineMesher.Action onPostRebuildMesh;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021F0 File Offset: 0x000003F0
		// (set) Token: 0x06000008 RID: 8 RVA: 0x000021E7 File Offset: 0x000003E7
		public MeshFilter meshFilter
		{
			get
			{
				return this.m_meshFilter;
			}
			private set
			{
				this.m_meshFilter = value;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021F8 File Offset: 0x000003F8
		private void Reset()
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
			if (this.meshFilter)
			{
				this.outputObject = this.meshFilter.gameObject;
			}
			this.splineContainer = base.GetComponentInParent<SplineContainer>();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002230 File Offset: 0x00000430
		private void Start()
		{
			if (this.rebuildTriggers.HasFlag(SplineMesher.RebuildTriggers.OnStart))
			{
				this.Rebuild();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002250 File Offset: 0x00000450
		private void OnEnable()
		{
			this.SubscribeSplineCallbacks();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002258 File Offset: 0x00000458
		private void OnDisable()
		{
			this.UnsubscribeSplineCallbacks();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002260 File Offset: 0x00000460
		private void SubscribeSplineCallbacks()
		{
			SplineContainer.SplineAdded += new Action<SplineContainer, int>(this.OnSplineAdded);
			SplineContainer.SplineRemoved += new Action<SplineContainer, int>(this.OnSplineRemoved);
			Spline.Changed += new Action<Spline, int, SplineModification>(this.OnSplineChanged);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002295 File Offset: 0x00000495
		private void UnsubscribeSplineCallbacks()
		{
			SplineContainer.SplineAdded -= new Action<SplineContainer, int>(this.OnSplineAdded);
			SplineContainer.SplineRemoved -= new Action<SplineContainer, int>(this.OnSplineRemoved);
			Spline.Changed -= new Action<Spline, int, SplineModification>(this.OnSplineChanged);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022CC File Offset: 0x000004CC
		public void UpdateCaps()
		{
			if (!this.splineContainer)
			{
				return;
			}
			bool flag = this.splineContainer.Splines.Count != this.splineCount;
			if (flag || this.startCap.RequiresRespawn())
			{
				this.startCap.Respawn(this.splineCount, base.transform);
			}
			if (flag || this.endCap.RequiresRespawn())
			{
				this.endCap.Respawn(this.splineCount, base.transform);
			}
			this.startCap.ApplyTransform(this);
			this.endCap.ApplyTransform(this);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002368 File Offset: 0x00000568
		public void ValidateOutput()
		{
			if (!this.outputObject)
			{
				return;
			}
			if (!this.meshFilter)
			{
				this.meshFilter = this.outputObject.GetComponent<MeshFilter>();
			}
			if (!this.meshFilter)
			{
				this.meshFilter = this.outputObject.AddComponent<MeshFilter>();
				if (!this.outputObject.GetComponent<MeshRenderer>())
				{
					this.outputObject.AddComponent<MeshRenderer>();
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023E0 File Offset: 0x000005E0
		public void Rebuild()
		{
			if (!this.splineContainer)
			{
				return;
			}
			if (!this.outputObject)
			{
				return;
			}
			bool flag = !this.settings.collision.enable || !this.settings.collision.colliderOnly;
			this.meshFilter = this.outputObject.GetComponent<MeshFilter>();
			if (flag && !this.meshFilter)
			{
				return;
			}
			SplineMesher.Action action = SplineMesher.onPreRebuildMesh;
			if (action != null)
			{
				action(this);
			}
			SplineMesher.RebuildEvent rebuildEvent = this.onPreRebuild;
			if (rebuildEvent != null)
			{
				rebuildEvent.Invoke();
			}
			this.ValidateData();
			if (!this.sourceMesh)
			{
				return;
			}
			if (Application.isPlaying && !this.sourceMesh.isReadable)
			{
				throw new Exception("[Spline Mesher] To use this at runtime, the mesh \"" + this.sourceMesh.name + "\" requires the Read/Write option enabled in its import settings. For procedurally created geometry, use \"Mesh.UploadMeshData(false)\"");
			}
			this.inputMesh = SplineMeshGenerator.TransformMesh(this.sourceMesh, this.rotation, this.settings.deforming.scale.x < 0f, this.settings.deforming.scale.y < 0f);
			if (flag)
			{
				bool flag2 = this.settings.collision.enable && this.meshCollider;
				if (flag2)
				{
					this.meshCollider.enabled = false;
				}
				this.meshFilter.sharedMesh = SplineMeshGenerator.CreateMesh(this.splineContainer, this.inputMesh, this.outputObject.transform.worldToLocalMatrix, this.settings, this.scaleData, this.rollData, this.vertexColorRedData, this.vertexColorGreenData, this.vertexColorBlueData, this.vertexColorAlphaData);
				if (flag2)
				{
					this.meshCollider.enabled = true;
				}
			}
			else if (this.meshFilter && this.meshFilter.sharedMesh)
			{
				this.meshFilter.sharedMesh = null;
			}
			this.CreateCollider();
			SplineMesher.Action action2 = SplineMesher.onPostRebuildMesh;
			if (action2 != null)
			{
				action2(this);
			}
			SplineMesher.RebuildEvent rebuildEvent2 = this.onPostRebuild;
			if (rebuildEvent2 == null)
			{
				return;
			}
			rebuildEvent2.Invoke();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002604 File Offset: 0x00000804
		public float GetLastRebuildTime()
		{
			return 0f;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000260C File Offset: 0x0000080C
		private void CreateCollider()
		{
			if (!this.splineContainer)
			{
				return;
			}
			if (!this.settings.collision.enable)
			{
				if (this.meshCollider)
				{
					Object.DestroyImmediate(this.meshCollider);
				}
				return;
			}
			if (!this.meshCollider)
			{
				this.meshCollider = this.outputObject.GetComponent<MeshCollider>();
			}
			if (!this.meshCollider)
			{
				this.meshCollider = this.outputObject.AddComponent<MeshCollider>();
			}
			Mesh mesh = this.settings.collision.collisionMesh;
			if (this.settings.collision.type == Settings.ColliderType.Box)
			{
				mesh = SplineMeshGenerator.CreateBoundsMesh(this.inputMesh, this.settings.collision.boxSubdivisions, false);
			}
			else if (this.settings.collision.collisionMesh)
			{
				mesh = SplineMeshGenerator.TransformMesh(this.settings.collision.collisionMesh, this.rotation, this.settings.deforming.scale.x < 0f, this.settings.deforming.scale.y < 0f);
			}
			if (!mesh)
			{
				this.meshCollider.sharedMesh = null;
				return;
			}
			if (mesh.GetHashCode() == this.sourceMesh.GetHashCode())
			{
				this.meshCollider.sharedMesh = this.meshFilter.sharedMesh;
				return;
			}
			this.meshCollider.sharedMesh = null;
			this.meshCollider.sharedMesh = SplineMeshGenerator.CreateMesh(this.splineContainer, mesh, this.meshCollider.transform.worldToLocalMatrix, this.settings, this.scaleData, this.rollData, null, null, null, null);
			Mesh sharedMesh = this.meshCollider.sharedMesh;
			sharedMesh.name += " Collider";
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000027F0 File Offset: 0x000009F0
		private void OnDrawGizmosSelected()
		{
			if (this.splineContainer && Time.frameCount % 2 == 0 && this.rebuildTriggers.HasFlag(SplineMesher.RebuildTriggers.OnSplineChanged))
			{
				Transform transform = this.splineContainer.transform;
				if (false | this.prevSplinePosition != transform.position | this.prevSplineRotation != transform.rotation | this.prevSplineScale != transform.lossyScale)
				{
					this.prevSplinePosition = transform.position;
					this.prevSplineRotation = transform.rotation;
					this.prevSplineScale = transform.lossyScale;
					this.Rebuild();
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000028A0 File Offset: 0x00000AA0
		private static float BlendVertexColorChannel(SplineMesher.VertexColorChannel data, float baseValue)
		{
			float result;
			if (data.blend)
			{
				result = baseValue + data.value;
			}
			else
			{
				result = data.value;
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028CC File Offset: 0x00000ACC
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (!this.splineContainer)
			{
				return;
			}
			if (!this.rebuildTriggers.HasFlag(SplineMesher.RebuildTriggers.OnSplineChanged))
			{
				return;
			}
			int num = Array.IndexOf<Spline>(this.splineContainer.Splines.ToArray<Spline>(), spline);
			if (num < 0)
			{
				return;
			}
			this.splineCount = this.splineContainer.Splines.Count;
			this.lastEditedSpline = spline;
			this.lastEditedSplineIndex = num;
			if (this.splineChangeMode == SplineMesher.SplineChangeReaction.WhenDone)
			{
				this.lastChangeTime = Time.realtimeSinceStartup;
				if (Application.isPlaying)
				{
					if (this.debounceCoroutine != null)
					{
						base.StopCoroutine(this.debounceCoroutine);
					}
					this.debounceCoroutine = base.StartCoroutine(this.DebounceCoroutine());
					return;
				}
				if (!this.isTrackingChanges)
				{
					this.isTrackingChanges = true;
					return;
				}
			}
			else if (this.splineChangeMode == SplineMesher.SplineChangeReaction.During)
			{
				this.ExecuteAfterSplineChanges();
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029A0 File Offset: 0x00000BA0
		private void EditorUpdate()
		{
			if (this.isTrackingChanges && Time.realtimeSinceStartup - this.lastChangeTime >= this.debounceTime)
			{
				this.ExecuteAfterSplineChanges();
				this.isTrackingChanges = false;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000029CB File Offset: 0x00000BCB
		private IEnumerator DebounceCoroutine()
		{
			yield return new WaitForSeconds(this.debounceTime);
			this.ExecuteAfterSplineChanges();
			yield break;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000029DA File Offset: 0x00000BDA
		private void ExecuteAfterSplineChanges()
		{
			if (this.lastEditedSplineIndex < 0)
			{
				return;
			}
			this.Rebuild();
			this.UpdateCaps();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000029F4 File Offset: 0x00000BF4
		private void OnSplineAdded(SplineContainer container, int index)
		{
			if (!this.splineContainer)
			{
				return;
			}
			if (!this.rebuildTriggers.HasFlag(SplineMesher.RebuildTriggers.OnSplineAdded))
			{
				return;
			}
			if (container.GetHashCode() != this.splineContainer.GetHashCode())
			{
				return;
			}
			this.splineCount = this.splineContainer.Splines.Count;
			this.Rebuild();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002A58 File Offset: 0x00000C58
		private void OnSplineRemoved(SplineContainer container, int index)
		{
			if (!this.splineContainer)
			{
				return;
			}
			if (!this.rebuildTriggers.HasFlag(SplineMesher.RebuildTriggers.OnSplineRemoved))
			{
				return;
			}
			if (container != this.splineContainer)
			{
				return;
			}
			this.splineCount = this.splineContainer.Splines.Count;
			if (index < this.scaleData.Count)
			{
				this.scaleData.RemoveAt(index);
			}
			if (index < this.rollData.Count)
			{
				this.rollData.RemoveAt(index);
			}
			if (index < this.vertexColorRedData.Count)
			{
				this.vertexColorRedData.RemoveAt(index);
			}
			if (index < this.vertexColorGreenData.Count)
			{
				this.vertexColorGreenData.RemoveAt(index);
			}
			if (index < this.vertexColorBlueData.Count)
			{
				this.vertexColorBlueData.RemoveAt(index);
			}
			if (index < this.vertexColorAlphaData.Count)
			{
				this.vertexColorAlphaData.RemoveAt(index);
			}
			this.Rebuild();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B53 File Offset: 0x00000D53
		public void ResetScaleData()
		{
			if (!this.splineContainer)
			{
				return;
			}
			this.scaleData.Clear();
			this.ValidateData();
			this.Rebuild();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B7A File Offset: 0x00000D7A
		public void ResetRollData()
		{
			if (!this.splineContainer)
			{
				return;
			}
			this.rollData.Clear();
			this.ValidateData();
			this.Rebuild();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002BA4 File Offset: 0x00000DA4
		public void ResetVertexColorData()
		{
			if (!this.splineContainer)
			{
				return;
			}
			this.vertexColorRedData.Clear();
			this.vertexColorGreenData.Clear();
			this.vertexColorBlueData.Clear();
			this.vertexColorAlphaData.Clear();
			this.ValidateData();
			this.Rebuild();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public void ReverseSpline()
		{
			if (!this.splineContainer)
			{
				return;
			}
			for (int i = 0; i < this.splineContainer.Splines.Count; i++)
			{
				SplineUtility.ReverseFlow(this.splineContainer.Splines[i]);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C44 File Offset: 0x00000E44
		public void ValidateData()
		{
			if (!this.splineContainer)
			{
				return;
			}
			this.splineCount = this.splineContainer.Splines.Count;
			this.ValidateScaleData();
			this.ValidateRollData();
			this.ValidateVertexColorData(ref this.vertexColorRedData);
			this.ValidateVertexColorData(ref this.vertexColorGreenData);
			this.ValidateVertexColorData(ref this.vertexColorBlueData);
			this.ValidateVertexColorData(ref this.vertexColorAlphaData);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002CB4 File Offset: 0x00000EB4
		private void ValidateScaleData()
		{
			if (this.scaleData.Count < this.splineCount)
			{
				int num = this.splineCount - this.scaleData.Count;
				for (int i = 0; i < num; i++)
				{
					SplineData<float3> splineData = new SplineData<float3>();
					splineData.DefaultValue = Vector3.one;
					splineData.PathIndexUnit = this.settings.deforming.scalePathIndexUnit;
					this.scaleData.Add(splineData);
				}
			}
			for (int j = 0; j < this.scaleData.Count; j++)
			{
				if (this.scaleData[j].PathIndexUnit != this.settings.deforming.scalePathIndexUnit)
				{
					this.ConvertIndexUnit<float3>(this.splineContainer.Splines[j], ref this.scaleData, j, this.settings.deforming.scalePathIndexUnit);
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D94 File Offset: 0x00000F94
		private void ValidateRollData()
		{
			if (this.rollData.Count < this.splineCount)
			{
				int num = this.splineCount - this.rollData.Count;
				for (int i = 0; i < num; i++)
				{
					SplineData<float> splineData = new SplineData<float>();
					splineData.DefaultValue = 0f;
					splineData.PathIndexUnit = this.settings.deforming.rollPathIndexUnit;
					this.rollData.Add(splineData);
				}
			}
			for (int j = 0; j < this.rollData.Count; j++)
			{
				if (this.rollData[j].PathIndexUnit != this.settings.deforming.rollPathIndexUnit)
				{
					this.ConvertIndexUnit<float>(this.splineContainer.Splines[j], ref this.rollData, j, this.settings.deforming.rollPathIndexUnit);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E70 File Offset: 0x00001070
		private void ConvertIndexUnit<T>(ISpline spline, ref List<SplineData<T>> data, int index, PathIndexUnit targetUnit)
		{
			for (int i = 0; i < data[index].Count; i++)
			{
				data[index].ConvertPathUnit<ISpline>(spline, targetUnit);
			}
			data[index].PathIndexUnit = targetUnit;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002EB4 File Offset: 0x000010B4
		private void ValidateVertexColorData(ref List<SplineData<SplineMesher.VertexColorChannel>> channel)
		{
			int num = this.splineCount - channel.Count;
			for (int i = 0; i < num; i++)
			{
				SplineData<SplineMesher.VertexColorChannel> splineData = new SplineData<SplineMesher.VertexColorChannel>();
				splineData.DefaultValue = new SplineMesher.VertexColorChannel
				{
					value = 0f,
					blend = true
				};
				splineData.PathIndexUnit = this.settings.color.pathIndexUnit;
				channel.Add(splineData);
			}
			for (int j = 0; j < channel.Count; j++)
			{
				if (channel[j].PathIndexUnit != this.settings.color.pathIndexUnit)
				{
					this.ConvertIndexUnit<SplineMesher.VertexColorChannel>(this.splineContainer.Splines[j], ref channel, j, this.settings.color.pathIndexUnit);
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F84 File Offset: 0x00001184
		public float3 SampleScale(float distance, int splineIndex)
		{
			float3 result = 1f;
			if (this.scaleData != null && this.scaleData[splineIndex].Count > 0)
			{
				result = this.scaleData[splineIndex].Evaluate<Spline, LerpFloat3>(this.splineContainer.Splines[splineIndex], distance, this.scaleData[splineIndex].PathIndexUnit, SplineMeshGenerator.Float3Interpolator);
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002FF4 File Offset: 0x000011F4
		public Quaternion SampleRollRotation(ISpline spline, Vector3 forward, float distance, int splineIndex)
		{
			float num = (this.settings.deforming.rollFrequency > 0f) ? (this.settings.deforming.rollFrequency * distance) : 1f;
			float num2 = this.settings.deforming.rollAngle * num;
			if (this.rollData != null && this.rollData[splineIndex].Count > 0)
			{
				num2 += this.rollData[splineIndex].Evaluate<ISpline, LerpFloat>(spline, this.splineContainer.Splines[splineIndex].ConvertIndexUnit(distance, PathIndexUnit.Distance, this.settings.deforming.rollPathIndexUnit), this.settings.deforming.rollPathIndexUnit, SplineMeshGenerator.FloatInterpolator);
			}
			return Quaternion.AngleAxis(-num2, forward);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000030C0 File Offset: 0x000012C0
		public float3 SampleScale(Vector3 worldPosition)
		{
			Vector3 v = this.splineContainer.transform.InverseTransformPoint(worldPosition);
			int num = 0;
			float3 @float;
			float value;
			SplineUtility.GetNearestPoint<Spline>(this.splineContainer.Splines[num], v, out @float, out value, 2, 2);
			float distance = this.splineContainer.Splines[num].ConvertIndexUnit(value, PathIndexUnit.Normalized, this.scaleData[num].PathIndexUnit);
			return this.SampleScale(distance, num);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003138 File Offset: 0x00001338
		[Obsolete("Use the native SplineUtility.FitSplineToPoints function instead.")]
		public void CreateSplineFromPoints(Vector3[] positions, bool smooth)
		{
		}

		// Token: 0x04000008 RID: 8
		public SplineMesher.Cap startCap = new SplineMesher.Cap(SplineMesher.Cap.Position.Start);

		// Token: 0x04000009 RID: 9
		public SplineMesher.Cap endCap = new SplineMesher.Cap(SplineMesher.Cap.Position.End);

		// Token: 0x0400000A RID: 10
		public const string VERSION = "1.2.2";

		// Token: 0x0400000B RID: 11
		public const string kPackageRoot = "Packages/com.staggartcreations.splinemesher";

		// Token: 0x0400000C RID: 12
		public Mesh sourceMesh;

		// Token: 0x0400000D RID: 13
		[Tooltip("The axis of the mesh that's considered to its forward direction.\n\nConventionally, the Z-axis is forward. If you have to change this it's strongly recommend to fix the mesh's orientation instead!")]
		public Vector3 rotation;

		// Token: 0x0400000E RID: 14
		[Tooltip("The GameObject to which a Mesh Filter component may be added. The output mesh will be assigned here.")]
		public GameObject outputObject;

		// Token: 0x0400000F RID: 15
		[Obsolete("Set the Rebuild Trigger flag \"On Start\" instead", false)]
		public bool rebuildOnStart;

		// Token: 0x04000010 RID: 16
		[Tooltip("Control which sort of events cause the mesh to be regenerated.\n\nFor instance when the spline changes (default), or on the component's Start() function.\n\nIf none are selected you need to call the Rebuild() function through script.")]
		public SplineMesher.RebuildTriggers rebuildTriggers = SplineMesher.RebuildTriggers.OnSplineChanged | SplineMesher.RebuildTriggers.OnSplineAdded | SplineMesher.RebuildTriggers.OnSplineRemoved | SplineMesher.RebuildTriggers.OnUIChange;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private MeshCollider meshCollider;

		// Token: 0x04000012 RID: 18
		public Settings settings = new Settings();

		// Token: 0x04000015 RID: 21
		[HideInInspector]
		public SplineMesher.RebuildEvent onPreRebuild;

		// Token: 0x04000016 RID: 22
		[HideInInspector]
		public SplineMesher.RebuildEvent onPostRebuild;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		[FormerlySerializedAs("meshFilter")]
		private MeshFilter m_meshFilter;

		// Token: 0x04000018 RID: 24
		private Mesh inputMesh;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		[HideInInspector]
		private Vector3 prevSplinePosition;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		[HideInInspector]
		private Quaternion prevSplineRotation;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		[HideInInspector]
		private Vector3 prevSplineScale;

		// Token: 0x0400001C RID: 28
		public SplineContainer splineContainer;

		// Token: 0x0400001D RID: 29
		[SerializeField]
		[HideInInspector]
		private int splineCount;

		// Token: 0x0400001E RID: 30
		[Tooltip("Determines when a change to the spline should be detected")]
		public SplineMesher.SplineChangeReaction splineChangeMode;

		// Token: 0x0400001F RID: 31
		public List<SplineData<float3>> scaleData = new List<SplineData<float3>>();

		// Token: 0x04000020 RID: 32
		public List<SplineData<float>> rollData = new List<SplineData<float>>();

		// Token: 0x04000021 RID: 33
		public List<SplineData<SplineMesher.VertexColorChannel>> vertexColorRedData = new List<SplineData<SplineMesher.VertexColorChannel>>();

		// Token: 0x04000022 RID: 34
		public List<SplineData<SplineMesher.VertexColorChannel>> vertexColorGreenData = new List<SplineData<SplineMesher.VertexColorChannel>>();

		// Token: 0x04000023 RID: 35
		public List<SplineData<SplineMesher.VertexColorChannel>> vertexColorBlueData = new List<SplineData<SplineMesher.VertexColorChannel>>();

		// Token: 0x04000024 RID: 36
		public List<SplineData<SplineMesher.VertexColorChannel>> vertexColorAlphaData = new List<SplineData<SplineMesher.VertexColorChannel>>();

		// Token: 0x04000025 RID: 37
		private Spline lastEditedSpline;

		// Token: 0x04000026 RID: 38
		private int lastEditedSplineIndex = -1;

		// Token: 0x04000027 RID: 39
		public float debounceTime = 0.1f;

		// Token: 0x04000028 RID: 40
		private float lastChangeTime = -1f;

		// Token: 0x04000029 RID: 41
		private bool isTrackingChanges;

		// Token: 0x0400002A RID: 42
		private Coroutine debounceCoroutine;

		// Token: 0x02000010 RID: 16
		[Serializable]
		public class Cap
		{
			// Token: 0x0600003A RID: 58 RVA: 0x000047B5 File Offset: 0x000029B5
			public Cap(SplineMesher.Cap.Position position)
			{
				this.position = position;
			}

			// Token: 0x0600003B RID: 59 RVA: 0x000047E8 File Offset: 0x000029E8
			public bool HasPrefabChanged()
			{
				if (this.prefab == null)
				{
					return true;
				}
				int hashCode = this.prefab.GetHashCode();
				if (hashCode != this.previousPrefabID)
				{
					this.previousPrefabID = hashCode;
					return true;
				}
				return false;
			}

			// Token: 0x0600003C RID: 60 RVA: 0x00004824 File Offset: 0x00002A24
			public bool RequiresRespawn()
			{
				return this.HasPrefabChanged() || this.HasNoInstances() || this.HasMissingInstances();
			}

			// Token: 0x0600003D RID: 61 RVA: 0x0000483E File Offset: 0x00002A3E
			public bool HasNoInstances()
			{
				return this.instances.Length == 0;
			}

			// Token: 0x0600003E RID: 62 RVA: 0x0000484C File Offset: 0x00002A4C
			public bool HasMissingInstances()
			{
				for (int i = 0; i < this.instances.Length; i++)
				{
					if (this.instances[i] == null)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600003F RID: 63 RVA: 0x00004880 File Offset: 0x00002A80
			public void DestroyInstances()
			{
				for (int i = 0; i < this.instances.Length; i++)
				{
					SplineMesher.Cap.DestroyInstance(this.instances[i]);
				}
			}

			// Token: 0x06000040 RID: 64 RVA: 0x000048AD File Offset: 0x00002AAD
			private static void DestroyInstance(Object obj)
			{
				Object.Destroy(obj);
			}

			// Token: 0x06000041 RID: 65 RVA: 0x000048B8 File Offset: 0x00002AB8
			public void Respawn(int splineCount, Transform parent)
			{
				this.DestroyInstances();
				if (this.prefab == null)
				{
					this.instances = Array.Empty<GameObject>();
					return;
				}
				this.instances = new GameObject[splineCount];
				for (int i = 0; i < this.instances.Length; i++)
				{
					GameObject gameObject = this.InstantiatePrefab(this.prefab);
					gameObject.transform.SetParent(parent);
					this.instances[i] = gameObject;
				}
			}

			// Token: 0x06000042 RID: 66 RVA: 0x00004928 File Offset: 0x00002B28
			private GameObject InstantiatePrefab(Object source)
			{
				bool flag = false;
				GameObject gameObject = null;
				if (!flag)
				{
					gameObject = (Object.Instantiate(source) as GameObject);
				}
				if (gameObject == null)
				{
					Debug.LogError(string.Format("Failed to spawn instance. Source is prefab: {0}", flag));
				}
				return gameObject;
			}

			// Token: 0x06000043 RID: 67 RVA: 0x00004968 File Offset: 0x00002B68
			public void ApplyTransform(SplineMesher splineMesher)
			{
				for (int i = 0; i < this.instances.Length; i++)
				{
					Transform transform = this.instances[i].transform;
					float num = (float)this.position;
					float num2 = splineMesher.splineContainer.Splines[i].CalculateLength(splineMesher.splineContainer.transform.localToWorldMatrix);
					float num3 = this.shift;
					if (this.position == SplineMesher.Cap.Position.Start)
					{
						num3 += splineMesher.settings.distribution.trimStart;
					}
					else if (this.position == SplineMesher.Cap.Position.End)
					{
						num3 += splineMesher.settings.distribution.trimEnd;
					}
					float num4 = num3 / num2;
					if (this.position == SplineMesher.Cap.Position.End)
					{
						num4 = -num4;
					}
					num += num4;
					num = Mathf.Clamp(num, 0.0001f, 0.9999f);
					float3 @float;
					float3 float2;
					float3 float3;
					splineMesher.splineContainer.Splines[i].Evaluate(num, out @float, out float2, out float3);
					float3 float4 = math.normalize(float2);
					float3 float5 = math.cross(float4, float3);
					Quaternion rhs = Quaternion.Euler(this.rotation);
					if (this.align)
					{
						rhs = Quaternion.LookRotation(float4, float3) * Quaternion.Euler(this.rotation);
						rhs = splineMesher.SampleRollRotation(splineMesher.splineContainer.Splines[i], float4, num * num2, i) * rhs;
						float5 = rhs * math.right();
						float3 = rhs * math.up();
						if (this.position == SplineMesher.Cap.Position.End || (this.position == SplineMesher.Cap.Position.Start && this.scale.z < 0f))
						{
							float5 = -float5;
						}
					}
					@float += float5 * (this.offset.x - splineMesher.settings.deforming.curveOffset.x);
					@float += float3 * (this.offset.y - splineMesher.settings.deforming.curveOffset.y);
					@float += float4 * this.offset.z;
					@float.x += splineMesher.settings.deforming.pivotOffset.x;
					@float.y += splineMesher.settings.deforming.pivotOffset.y;
					@float = splineMesher.splineContainer.transform.TransformPoint(@float);
					float3 float6;
					float3 up;
					if (splineMesher.settings.conforming.enable && SplineMeshGenerator.PerformConforming(@float, splineMesher.settings.conforming, 1f, out float6, out up))
					{
						@float.y = float6.y + this.offset.y;
						quaternion.LookRotationSafe(float2, up);
						bool flag = splineMesher.settings.conforming.align;
					}
					Vector3 localScale = this.scale;
					if (this.matchScale)
					{
						localScale.x *= splineMesher.settings.deforming.scale.x;
						localScale.y *= splineMesher.settings.deforming.scale.y;
						localScale.z *= splineMesher.settings.deforming.scale.z;
						float3 float7 = splineMesher.SampleScale(num * num2, i);
						localScale.x *= float7.x;
						localScale.y *= float7.y;
					}
					transform.localScale = localScale;
					transform.SetPositionAndRotation(@float, rhs);
					transform.hideFlags = HideFlags.NotEditable;
				}
			}

			// Token: 0x04000070 RID: 112
			public readonly SplineMesher.Cap.Position position;

			// Token: 0x04000071 RID: 113
			public GameObject prefab;

			// Token: 0x04000072 RID: 114
			[SerializeField]
			[HideInInspector]
			private int previousPrefabID;

			// Token: 0x04000073 RID: 115
			[Tooltip("Positional offset, relative to the curve's tangent")]
			public Vector3 offset;

			// Token: 0x04000074 RID: 116
			[Tooltip("Shifts the object along the spline curve by this many units")]
			[Min(0f)]
			public float shift;

			// Token: 0x04000075 RID: 117
			[Tooltip("Align the object's forward direction to the tangent and roll of the spline")]
			public bool align = true;

			// Token: 0x04000076 RID: 118
			[Tooltip("Rotation in degrees, added to the object's rotation")]
			public Vector3 rotation;

			// Token: 0x04000077 RID: 119
			[Tooltip("Factor in the scale configured under the Deforming section, as well as scale data points created in the editor.")]
			public bool matchScale = true;

			// Token: 0x04000078 RID: 120
			public Vector3 scale = Vector3.one;

			// Token: 0x04000079 RID: 121
			public GameObject[] instances = Array.Empty<GameObject>();

			// Token: 0x0200001D RID: 29
			public enum Position
			{
				// Token: 0x04000095 RID: 149
				Start,
				// Token: 0x04000096 RID: 150
				End
			}
		}

		// Token: 0x02000011 RID: 17
		[Flags]
		public enum RebuildTriggers
		{
			// Token: 0x0400007B RID: 123
			[InspectorName("Via scripting")]
			None = 0,
			// Token: 0x0400007C RID: 124
			[InspectorName("On Spline Change")]
			OnSplineChanged = 1,
			// Token: 0x0400007D RID: 125
			OnSplineAdded = 2,
			// Token: 0x0400007E RID: 126
			OnSplineRemoved = 4,
			// Token: 0x0400007F RID: 127
			[InspectorName("On Start()")]
			OnStart = 8,
			// Token: 0x04000080 RID: 128
			OnUIChange = 16
		}

		// Token: 0x02000012 RID: 18
		// (Invoke) Token: 0x06000045 RID: 69
		public delegate void Action(SplineMesher instance);

		// Token: 0x02000013 RID: 19
		[Serializable]
		public class RebuildEvent : UnityEvent
		{
		}

		// Token: 0x02000014 RID: 20
		public enum SplineChangeReaction
		{
			// Token: 0x04000082 RID: 130
			During,
			// Token: 0x04000083 RID: 131
			WhenDone
		}

		// Token: 0x02000015 RID: 21
		[Serializable]
		public struct VertexColorChannel
		{
			// Token: 0x06000049 RID: 73 RVA: 0x00004D35 File Offset: 0x00002F35
			public static implicit operator float(SplineMesher.VertexColorChannel value)
			{
				return value.value;
			}

			// Token: 0x0600004A RID: 74 RVA: 0x00004D40 File Offset: 0x00002F40
			public static implicit operator SplineMesher.VertexColorChannel(float value)
			{
				return new SplineMesher.VertexColorChannel
				{
					value = value
				};
			}

			// Token: 0x04000084 RID: 132
			public float value;

			// Token: 0x04000085 RID: 133
			public bool blend;

			// Token: 0x0200001E RID: 30
			public struct LerpVertexColorData : IInterpolator<SplineMesher.VertexColorChannel>
			{
				// Token: 0x06000051 RID: 81 RVA: 0x00004DDA File Offset: 0x00002FDA
				public LerpVertexColorData(float baseValue)
				{
					this.baseValue = baseValue;
				}

				// Token: 0x06000052 RID: 82 RVA: 0x00004DE4 File Offset: 0x00002FE4
				public SplineMesher.VertexColorChannel Interpolate(SplineMesher.VertexColorChannel a, SplineMesher.VertexColorChannel b, float t)
				{
					float a2 = SplineMesher.BlendVertexColorChannel(a, this.baseValue);
					float b2 = SplineMesher.BlendVertexColorChannel(b, this.baseValue);
					return Mathf.Lerp(a2, b2, t);
				}

				// Token: 0x04000097 RID: 151
				private readonly float baseValue;
			}
		}
	}
}
