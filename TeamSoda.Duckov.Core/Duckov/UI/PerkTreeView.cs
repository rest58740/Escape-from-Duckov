using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.PerkTrees;
using Duckov.UI.Animations;
using Duckov.Utilities;
using NodeCanvas.Framework;
using TMPro;
using UI_Spline_Renderer;
using UnityEngine;
using UnityEngine.Splines;

namespace Duckov.UI
{
	// Token: 0x020003DA RID: 986
	public class PerkTreeView : View, ISingleSelectionMenu<PerkEntry>
	{
		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x0007DE20 File Offset: 0x0007C020
		public static PerkTreeView Instance
		{
			get
			{
				return View.GetViewInstance<PerkTreeView>();
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x0007DE28 File Offset: 0x0007C028
		private PrefabPool<PerkEntry> PerkEntryPool
		{
			get
			{
				if (this._perkEntryPool == null)
				{
					this._perkEntryPool = new PrefabPool<PerkEntry>(this.perkEntryPrefab, this.contentParent, null, null, null, true, 10, 10000, null);
				}
				return this._perkEntryPool;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x0007DE68 File Offset: 0x0007C068
		private PrefabPool<PerkLineEntry> PerkLinePool
		{
			get
			{
				if (this._perkLinePool == null)
				{
					this._perkLinePool = new PrefabPool<PerkLineEntry>(this.perkLinePrefab, this.contentParent, null, null, null, true, 10, 10000, null);
				}
				return this._perkLinePool;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060023E3 RID: 9187 RVA: 0x0007DEA6 File Offset: 0x0007C0A6
		protected override bool ShowOpenCloseButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x060023E4 RID: 9188 RVA: 0x0007DEAC File Offset: 0x0007C0AC
		// (remove) Token: 0x060023E5 RID: 9189 RVA: 0x0007DEE4 File Offset: 0x0007C0E4
		internal event Action<PerkEntry> onSelectionChanged;

		// Token: 0x060023E6 RID: 9190 RVA: 0x0007DF1C File Offset: 0x0007C11C
		private void PopulatePerks()
		{
			this.contentParent.ForceUpdateRectTransforms();
			this.PerkEntryPool.ReleaseAll();
			this.PerkLinePool.ReleaseAll();
			bool isDemo = GameMetaData.Instance.IsDemo;
			foreach (Perk perk in this.target.Perks)
			{
				if ((!isDemo || !perk.LockInDemo) && this.target.RelationGraphOwner.GetRelatedNode(perk) != null)
				{
					this.PerkEntryPool.Get(this.contentParent).Setup(this, perk);
				}
			}
			foreach (PerkLevelLineNode cur in this.target.RelationGraphOwner.graph.GetAllNodesOfType<PerkLevelLineNode>())
			{
				this.PerkLinePool.Get(this.contentParent).Setup(this, cur);
			}
			this.FitChildren();
			this.RefreshConnections();
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x0007E034 File Offset: 0x0007C234
		private void RefreshConnections()
		{
			bool isDemo = GameMetaData.Instance.IsDemo;
			this.activeConnectionsRenderer.enabled = false;
			this.inactiveConnectionsRenderer.enabled = false;
			SplineContainer splineContainer = this.activeConnectionsRenderer.splineContainer;
			SplineContainer splineContainer2 = this.inactiveConnectionsRenderer.splineContainer;
			PerkTreeView.<RefreshConnections>g__ClearSplines|27_0(splineContainer);
			PerkTreeView.<RefreshConnections>g__ClearSplines|27_0(splineContainer2);
			PerkTreeView.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.horizontal = this.target.Horizontal;
			CS$<>8__locals1.splineTangentVector = (CS$<>8__locals1.horizontal ? Vector3.left : Vector3.up) * this.splineTangent;
			foreach (Perk perk in this.target.Perks)
			{
				if (!isDemo || !perk.LockInDemo)
				{
					PerkRelationNode relatedNode = this.target.RelationGraphOwner.GetRelatedNode(perk);
					PerkEntry perkEntry = this.GetPerkEntry(perk);
					if (!(perkEntry == null) && relatedNode != null)
					{
						SplineContainer container = perk.Unlocked ? splineContainer : splineContainer2;
						foreach (Connection connection in relatedNode.outConnections)
						{
							PerkRelationNode perkRelationNode = connection.targetNode as PerkRelationNode;
							Perk relatedNode2 = perkRelationNode.relatedNode;
							if (relatedNode2 == null)
							{
								Debug.Log(string.Concat(new string[]
								{
									"Target Perk is Null (Connection from ",
									relatedNode.name,
									" to ",
									perkRelationNode.name,
									")"
								}));
							}
							else if (!isDemo || !relatedNode2.LockInDemo)
							{
								PerkEntry perkEntry2 = this.GetPerkEntry(relatedNode2);
								if (perkEntry2 == null)
								{
									Debug.Log(string.Concat(new string[]
									{
										"Target Perk Entry is Null (Connection from ",
										relatedNode.name,
										" to ",
										perkRelationNode.name,
										")"
									}));
								}
								else
								{
									PerkTreeView.<RefreshConnections>g__AddConnection|27_1(container, perkEntry.transform.localPosition, perkEntry2.transform.localPosition, ref CS$<>8__locals1);
								}
							}
						}
					}
				}
			}
			this.activeConnectionsRenderer.enabled = true;
			this.inactiveConnectionsRenderer.enabled = true;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0007E2B8 File Offset: 0x0007C4B8
		private PerkEntry GetPerkEntry(Perk ofPerk)
		{
			return this.PerkEntryPool.ActiveEntries.FirstOrDefault((PerkEntry e) => e != null && e.Target == ofPerk);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x0007E2F0 File Offset: 0x0007C4F0
		private void FitChildren()
		{
			this.contentParent.ForceUpdateRectTransforms();
			ReadOnlyCollection<PerkEntry> activeEntries = this.PerkEntryPool.ActiveEntries;
			float num2;
			float num = num2 = float.MaxValue;
			float num4;
			float num3 = num4 = float.MinValue;
			foreach (PerkEntry perkEntry in activeEntries)
			{
				RectTransform rectTransform = perkEntry.RectTransform;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.zero;
				Vector2 layoutPosition = perkEntry.GetLayoutPosition();
				layoutPosition.y *= -1f;
				Vector2 vector = layoutPosition * this.layoutFactor;
				rectTransform.anchoredPosition = vector;
				if (vector.x < num2)
				{
					num2 = vector.x;
				}
				if (vector.y < num)
				{
					num = vector.y;
				}
				if (vector.x > num4)
				{
					num4 = vector.x;
				}
				if (vector.y > num3)
				{
					num3 = vector.y;
				}
			}
			float num5 = num4 - num2;
			float num6 = num3 - num;
			Vector2 vector2 = -new Vector2(num2, num);
			RectTransform rectTransform2 = this.contentParent;
			Vector2 sizeDelta = rectTransform2.sizeDelta;
			sizeDelta.y = num6 + this.padding.y * 2f;
			rectTransform2.sizeDelta = sizeDelta;
			foreach (PerkEntry perkEntry2 in activeEntries)
			{
				RectTransform rectTransform3 = perkEntry2.RectTransform;
				Vector2 vector3 = rectTransform3.anchoredPosition + vector2;
				if (num5 == 0f)
				{
					vector3.x = (rectTransform2.rect.width - this.padding.x * 2f) / 2f;
				}
				else
				{
					float num7 = (rectTransform2.rect.width - this.padding.x * 2f) / num5;
					vector3.x *= num7;
				}
				vector3 += this.padding;
				rectTransform3.anchoredPosition = vector3;
			}
			foreach (PerkLineEntry perkLineEntry in this.PerkLinePool.ActiveEntries)
			{
				RectTransform rectTransform4 = perkLineEntry.RectTransform;
				Vector2 layoutPosition2 = perkLineEntry.GetLayoutPosition();
				layoutPosition2.y *= -1f;
				Vector2 vector4 = layoutPosition2 * this.layoutFactor;
				vector4 += this.padding;
				vector4.x = rectTransform4.anchoredPosition.x;
				rectTransform4.anchoredPosition = vector4;
				rectTransform4.SetAsFirstSibling();
			}
			this.contentParent.anchoredPosition = Vector2.zero;
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x0007E5D0 File Offset: 0x0007C7D0
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x0007E5E3 File Offset: 0x0007C7E3
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x0007E5F6 File Offset: 0x0007C7F6
		public PerkEntry GetSelection()
		{
			return this.selectedPerkEntry;
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0007E5FE File Offset: 0x0007C7FE
		public bool SetSelection(PerkEntry selection)
		{
			this.selectedPerkEntry = selection;
			this.OnSelectionChanged();
			return true;
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x0007E60E File Offset: 0x0007C80E
		private void OnSelectionChanged()
		{
			Action<PerkEntry> action = this.onSelectionChanged;
			if (action != null)
			{
				action(this.selectedPerkEntry);
			}
			this.RefreshDetails();
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0007E62D File Offset: 0x0007C82D
		private void RefreshDetails()
		{
			PerkDetails perkDetails = this.details;
			PerkEntry perkEntry = this.selectedPerkEntry;
			perkDetails.Setup((perkEntry != null) ? perkEntry.Target : null, true);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0007E64D File Offset: 0x0007C84D
		private void Show_Local(PerkTree target)
		{
			this.UnregisterEvents();
			this.SetSelection(null);
			this.target = target;
			this.title.text = target.DisplayName;
			this.ShowTask().Forget();
			this.RegisterEvents();
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0007E686 File Offset: 0x0007C886
		public static void Show(PerkTree target)
		{
			if (PerkTreeView.Instance == null)
			{
				return;
			}
			PerkTreeView.Instance.Show_Local(target);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0007E6A1 File Offset: 0x0007C8A1
		private void RegisterEvents()
		{
			if (this.target != null)
			{
				this.target.onPerkTreeStatusChanged += this.Refresh;
			}
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0007E6C8 File Offset: 0x0007C8C8
		private void UnregisterEvents()
		{
			if (this.target != null)
			{
				this.target.onPerkTreeStatusChanged -= this.Refresh;
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0007E6EF File Offset: 0x0007C8EF
		private void Refresh(PerkTree tree)
		{
			this.RefreshConnections();
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0007E6F8 File Offset: 0x0007C8F8
		private UniTask ShowTask()
		{
			PerkTreeView.<ShowTask>d__41 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<PerkTreeView.<ShowTask>d__41>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0007E73B File Offset: 0x0007C93B
		public void Hide()
		{
			base.Close();
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0007E743 File Offset: 0x0007C943
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0007E774 File Offset: 0x0007C974
		[CompilerGenerated]
		internal static void <RefreshConnections>g__ClearSplines|27_0(SplineContainer splineContainer)
		{
			while (splineContainer.Splines.Count > 0)
			{
				SplineUtility.RemoveSplineAt<SplineContainer>(splineContainer, 0);
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0007E790 File Offset: 0x0007C990
		[CompilerGenerated]
		internal static void <RefreshConnections>g__AddConnection|27_1(SplineContainer container, Vector2 from, Vector2 to, ref PerkTreeView.<>c__DisplayClass27_0 A_3)
		{
			if (A_3.horizontal)
			{
				SplineUtility.AddSpline<SplineContainer>(container, new Spline(new BezierKnot[]
				{
					new BezierKnot(from, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(from - A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(new Vector3(from.x, to.y) - 2f * A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(to, A_3.splineTangentVector, -A_3.splineTangentVector)
				}, false));
				return;
			}
			SplineUtility.AddSpline<SplineContainer>(container, new Spline(new BezierKnot[]
			{
				new BezierKnot(from, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(from - A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(new Vector3(to.x, from.y) - 2f * A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(to, A_3.splineTangentVector, -A_3.splineTangentVector)
			}, false));
		}

		// Token: 0x04001871 RID: 6257
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001872 RID: 6258
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001873 RID: 6259
		[SerializeField]
		private RectTransform contentParent;

		// Token: 0x04001874 RID: 6260
		[SerializeField]
		private PerkDetails details;

		// Token: 0x04001875 RID: 6261
		[SerializeField]
		private PerkEntry perkEntryPrefab;

		// Token: 0x04001876 RID: 6262
		[SerializeField]
		private PerkLineEntry perkLinePrefab;

		// Token: 0x04001877 RID: 6263
		[SerializeField]
		private UISplineRenderer activeConnectionsRenderer;

		// Token: 0x04001878 RID: 6264
		[SerializeField]
		private UISplineRenderer inactiveConnectionsRenderer;

		// Token: 0x04001879 RID: 6265
		[SerializeField]
		private float splineTangent = 100f;

		// Token: 0x0400187A RID: 6266
		[SerializeField]
		private PerkTree target;

		// Token: 0x0400187B RID: 6267
		private PrefabPool<PerkEntry> _perkEntryPool;

		// Token: 0x0400187C RID: 6268
		private PrefabPool<PerkLineEntry> _perkLinePool;

		// Token: 0x0400187D RID: 6269
		private PerkEntry selectedPerkEntry;

		// Token: 0x0400187E RID: 6270
		[SerializeField]
		private float layoutFactor = 10f;

		// Token: 0x0400187F RID: 6271
		[SerializeField]
		private Vector2 padding = Vector2.one;
	}
}
