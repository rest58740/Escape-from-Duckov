using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000022 RID: 34
	[AddComponentMenu("Pathfinding/Seeker")]
	[DisallowMultipleComponent]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/seeker.html")]
	public class Seeker : VersionedMonoBehaviour
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x000092AC File Offset: 0x000074AC
		public Seeker()
		{
			this.onPathDelegate = new OnPathDelegate(this.OnPathComplete);
			this.onPartialPathDelegate = new OnPathDelegate(this.OnPartialPathComplete);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009326 File Offset: 0x00007526
		protected override void Awake()
		{
			base.Awake();
			this.startEndModifier.Awake(this);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000933A File Offset: 0x0000753A
		public Path GetCurrentPath()
		{
			return this.path;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009342 File Offset: 0x00007542
		public void CancelCurrentPathRequest(bool pool = true)
		{
			if (!this.IsDone())
			{
				this.path.FailWithError("Canceled by script (Seeker.CancelCurrentPathRequest)");
				if (pool)
				{
					this.path.Claim(this.path);
					this.path.Release(this.path, false);
				}
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009382 File Offset: 0x00007582
		private void OnDestroy()
		{
			this.ReleaseClaimedPath();
			this.startEndModifier.OnDestroy(this);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009396 File Offset: 0x00007596
		private void ReleaseClaimedPath()
		{
			if (this.prevPath != null)
			{
				this.prevPath.Release(this, true);
				this.prevPath = null;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000093B4 File Offset: 0x000075B4
		public void RegisterModifier(IPathModifier modifier)
		{
			if (!this.modifiers.Contains(modifier))
			{
				this.modifiers.Add(modifier);
				this.modifiers.Sort((IPathModifier a, IPathModifier b) => a.Order.CompareTo(b.Order));
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009405 File Offset: 0x00007605
		public void DeregisterModifier(IPathModifier modifier)
		{
			this.modifiers.Remove(modifier);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009414 File Offset: 0x00007614
		private void ForceRegisterModifiers()
		{
			base.GetComponents<IPathModifier>(this.modifiers);
			this.modifiers.Sort((IPathModifier a, IPathModifier b) => a.Order.CompareTo(b.Order));
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000944C File Offset: 0x0000764C
		public void PostProcess(Path path)
		{
			this.RunModifiers(Seeker.ModifierPass.PostProcess, path);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009458 File Offset: 0x00007658
		public void RunModifiers(Seeker.ModifierPass pass, Path path)
		{
			if (!Application.isPlaying)
			{
				this.ForceRegisterModifiers();
			}
			if (pass == Seeker.ModifierPass.PreProcess)
			{
				if (this.preProcessPath != null)
				{
					this.preProcessPath(path);
				}
				for (int i = 0; i < this.modifiers.Count; i++)
				{
					this.modifiers[i].PreProcess(path);
				}
				return;
			}
			if (pass == Seeker.ModifierPass.PostProcess)
			{
				if (this.postProcessPath != null)
				{
					this.postProcessPath(path);
				}
				for (int j = 0; j < this.modifiers.Count; j++)
				{
					this.modifiers[j].Apply(path);
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000094F2 File Offset: 0x000076F2
		public bool IsDone()
		{
			return this.path == null || this.path.PipelineState >= PathState.Returning;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000950F File Offset: 0x0000770F
		private void OnPathComplete(Path path)
		{
			this.OnPathComplete(path, true, true);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000951C File Offset: 0x0000771C
		private void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
		{
			if (p != null && p != this.path && sendCallbacks)
			{
				return;
			}
			if (this == null || p == null || p != this.path)
			{
				return;
			}
			if (!this.path.error && runModifiers)
			{
				this.RunModifiers(Seeker.ModifierPass.PostProcess, this.path);
			}
			if (sendCallbacks)
			{
				p.Claim(this);
				if (this.tmpPathCallback != null || this.pathCallback != null)
				{
					if (this.tmpPathCallback != null)
					{
						this.tmpPathCallback(p);
					}
					if (this.pathCallback != null)
					{
						this.pathCallback(p);
					}
				}
				if (this.prevPath != null)
				{
					this.prevPath.Release(this, true);
				}
				this.prevPath = p;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000095D3 File Offset: 0x000077D3
		private void OnPartialPathComplete(Path p)
		{
			this.OnPathComplete(p, true, false);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000095DE File Offset: 0x000077DE
		private void OnMultiPathComplete(Path p)
		{
			this.OnPathComplete(p, false, true);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000095E9 File Offset: 0x000077E9
		[Obsolete("Use the overload that takes a callback instead")]
		public Path StartPath(Vector3 start, Vector3 end)
		{
			return this.StartPath(start, end, null);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000095F4 File Offset: 0x000077F4
		public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
		{
			return this.StartPath(ABPath.Construct(start, end, null), callback);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009605 File Offset: 0x00007805
		public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, GraphMask graphMask)
		{
			return this.StartPath(ABPath.Construct(start, end, null), callback, graphMask);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009618 File Offset: 0x00007818
		public Path StartPath(Path p)
		{
			return this.StartPath(p, null);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009622 File Offset: 0x00007822
		public Path StartPath(Path p, OnPathDelegate callback)
		{
			if (p.nnConstraint.graphMask == -1)
			{
				p.nnConstraint.graphMask = this.graphMask;
			}
			this.StartPathInternal(p, callback);
			return p;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009651 File Offset: 0x00007851
		public Path StartPath(Path p, OnPathDelegate callback, GraphMask graphMask)
		{
			p.nnConstraint.graphMask = graphMask;
			this.StartPathInternal(p, callback);
			return p;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009668 File Offset: 0x00007868
		private void StartPathInternal(Path p, OnPathDelegate callback)
		{
			MultiTargetPath multiTargetPath = p as MultiTargetPath;
			if (multiTargetPath != null)
			{
				OnPathDelegate[] array = new OnPathDelegate[multiTargetPath.targetPoints.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.onPartialPathDelegate;
				}
				multiTargetPath.callbacks = array;
				p.callback = (OnPathDelegate)Delegate.Combine(p.callback, new OnPathDelegate(this.OnMultiPathComplete));
			}
			else
			{
				p.callback = (OnPathDelegate)Delegate.Combine(p.callback, this.onPathDelegate);
			}
			p.enabledTags = this.traversableTags;
			p.tagPenalties = this.tagPenalties;
			if (this.traversalProvider != null)
			{
				p.traversalProvider = this.traversalProvider;
			}
			if (this.path != null && this.path.PipelineState <= PathState.Processing && this.path.CompleteState != PathCompleteState.Error && this.lastPathID == (uint)this.path.pathID)
			{
				this.path.FailWithError("Canceled path because a new one was requested.\nThis happens when a new path is requested from the seeker when one was already being calculated.\nFor example if a unit got a new order, you might request a new path directly instead of waiting for the now invalid path to be calculated. Which is probably what you want.\nIf you are getting this a lot, you might want to consider how you are scheduling path requests.");
			}
			this.path = p;
			this.tmpPathCallback = callback;
			this.lastPathID = (uint)this.path.pathID;
			this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
			AstarPath.StartPath(this.path, false, false);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009798 File Offset: 0x00007998
		public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback, GraphMask graphMask)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(start, endPoints, null, null);
			multiTargetPath.pathsForAll = pathsForAll;
			this.StartPath(multiTargetPath, callback, graphMask);
			return multiTargetPath;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000097C3 File Offset: 0x000079C3
		public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback)
		{
			return this.StartMultiTargetPath(start, endPoints, pathsForAll, callback, this.graphMask);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000097D8 File Offset: 0x000079D8
		public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback, GraphMask graphMask)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(startPoints, end, null, null);
			multiTargetPath.pathsForAll = pathsForAll;
			this.StartPath(multiTargetPath, callback, graphMask);
			return multiTargetPath;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00009803 File Offset: 0x00007A03
		public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback)
		{
			return this.StartMultiTargetPath(startPoints, end, pathsForAll, callback, this.graphMask);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009816 File Offset: 0x00007A16
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			if (this.graphMaskCompatibility != -1)
			{
				this.graphMask = this.graphMaskCompatibility;
				this.graphMaskCompatibility = -1;
			}
			base.OnUpgradeSerializedData(ref migrations, unityThread);
		}

		// Token: 0x04000109 RID: 265
		public bool drawGizmos = true;

		// Token: 0x0400010A RID: 266
		public bool detailedGizmos;

		// Token: 0x0400010B RID: 267
		[HideInInspector]
		public StartEndModifier startEndModifier = new StartEndModifier();

		// Token: 0x0400010C RID: 268
		[HideInInspector]
		public int traversableTags = -1;

		// Token: 0x0400010D RID: 269
		[HideInInspector]
		public int[] tagPenalties = new int[32];

		// Token: 0x0400010E RID: 270
		[HideInInspector]
		public GraphMask graphMask = GraphMask.everything;

		// Token: 0x0400010F RID: 271
		public ITraversalProvider traversalProvider;

		// Token: 0x04000110 RID: 272
		[FormerlySerializedAs("graphMask")]
		private int graphMaskCompatibility = -1;

		// Token: 0x04000111 RID: 273
		[Obsolete("Pass a callback every time to the StartPath method instead, or use ai.SetPath+ai.pathPending on the movement script. You can cache it in your own script if you want to avoid the GC allocation of creating a new delegate.")]
		public OnPathDelegate pathCallback;

		// Token: 0x04000112 RID: 274
		public OnPathDelegate preProcessPath;

		// Token: 0x04000113 RID: 275
		public OnPathDelegate postProcessPath;

		// Token: 0x04000114 RID: 276
		[NonSerialized]
		protected Path path;

		// Token: 0x04000115 RID: 277
		[NonSerialized]
		private Path prevPath;

		// Token: 0x04000116 RID: 278
		private readonly OnPathDelegate onPathDelegate;

		// Token: 0x04000117 RID: 279
		private readonly OnPathDelegate onPartialPathDelegate;

		// Token: 0x04000118 RID: 280
		private OnPathDelegate tmpPathCallback;

		// Token: 0x04000119 RID: 281
		protected uint lastPathID;

		// Token: 0x0400011A RID: 282
		private readonly List<IPathModifier> modifiers = new List<IPathModifier>();

		// Token: 0x02000023 RID: 35
		public enum ModifierPass
		{
			// Token: 0x0400011C RID: 284
			PreProcess,
			// Token: 0x0400011D RID: 285
			PostProcess = 2
		}
	}
}
