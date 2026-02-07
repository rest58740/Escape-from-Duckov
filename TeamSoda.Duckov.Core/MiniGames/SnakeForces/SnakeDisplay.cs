using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x02000297 RID: 663
	public class SnakeDisplay : MiniGameBehaviour
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x000503B4 File Offset: 0x0004E5B4
		private PrefabPool<SnakePartDisplay> PartPool
		{
			get
			{
				if (this._partPool == null)
				{
					this._partPool = new PrefabPool<SnakePartDisplay>(this.partDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._partPool;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x000503F0 File Offset: 0x0004E5F0
		private PrefabPool<Transform> FoodPool
		{
			get
			{
				if (this._foodPool == null)
				{
					this._foodPool = new PrefabPool<Transform>(this.foodDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._foodPool;
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0005042C File Offset: 0x0004E62C
		private void Awake()
		{
			this.master.OnAddPart += this.OnAddPart;
			this.master.OnGameStart += this.OnGameStart;
			this.master.OnRemovePart += this.OnRemovePart;
			this.master.OnAfterTick += this.OnAfterTick;
			this.master.OnFoodEaten += this.OnFoodEaten;
			this.partDisplayTemplate.gameObject.SetActive(false);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x000504BD File Offset: 0x0004E6BD
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.HandlePunchColor();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x000504CC File Offset: 0x0004E6CC
		private void HandlePunchColor()
		{
			if (!this.punchingColor)
			{
				return;
			}
			if (this.punchColorIndex >= this.master.Snake.Count)
			{
				this.punchingColor = false;
				return;
			}
			SnakePartDisplay snakePartDisplay = this.PartPool.ActiveEntries.First((SnakePartDisplay e) => e.Target == this.master.Snake[this.punchColorIndex]);
			if (snakePartDisplay)
			{
				snakePartDisplay.PunchColor(Color.HSVToRGB((float)this.punchColorIndex % 12f / 12f, 1f, 1f));
			}
			this.punchColorIndex++;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0005055D File Offset: 0x0004E75D
		private void OnGameStart(SnakeForce force)
		{
			this.RefreshFood();
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00050568 File Offset: 0x0004E768
		private void OnFoodEaten(SnakeForce force, Vector2Int coord)
		{
			FXPool.Play(this.eatFXPrefab, this.GetWorldPosition(coord), Quaternion.LookRotation((Vector3Int)this.master.Head.direction, Vector3.forward));
			foreach (SnakePartDisplay snakePartDisplay in this.PartPool.ActiveEntries)
			{
				snakePartDisplay.Punch();
			}
			this.StartPunchingColor();
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x000505F4 File Offset: 0x0004E7F4
		private void StartPunchingColor()
		{
			this.punchingColor = true;
			this.punchColorIndex = 0;
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00050604 File Offset: 0x0004E804
		private void OnAfterTick(SnakeForce force)
		{
			this.RefreshFood();
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0005060C File Offset: 0x0004E80C
		private void RefreshFood()
		{
			this.FoodPool.ReleaseAll();
			foreach (Vector2Int coord in this.master.Foods)
			{
				this.FoodPool.Get(null).localPosition = this.GetPosition(coord);
			}
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00050680 File Offset: 0x0004E880
		private void OnRemovePart(SnakeForce.Part part)
		{
			this.PartPool.ReleaseAll((SnakePartDisplay e) => e.Target == part);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x000506B2 File Offset: 0x0004E8B2
		private void OnAddPart(SnakeForce.Part part)
		{
			this.PartPool.Get(null).Setup(this, part);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000506C7 File Offset: 0x0004E8C7
		internal Vector3 GetPosition(Vector2Int coord)
		{
			return coord * this.gridSize;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x000506E0 File Offset: 0x0004E8E0
		internal Vector3 GetWorldPosition(Vector2Int coord)
		{
			Vector3 position = this.GetPosition(coord);
			return base.transform.TransformPoint(position);
		}

		// Token: 0x04000FCF RID: 4047
		[SerializeField]
		private SnakeForce master;

		// Token: 0x04000FD0 RID: 4048
		[SerializeField]
		private SnakePartDisplay partDisplayTemplate;

		// Token: 0x04000FD1 RID: 4049
		[SerializeField]
		private Transform foodDisplayTemplate;

		// Token: 0x04000FD2 RID: 4050
		[SerializeField]
		private Transform exitDisplayTemplte;

		// Token: 0x04000FD3 RID: 4051
		[SerializeField]
		private ParticleSystem eatFXPrefab;

		// Token: 0x04000FD4 RID: 4052
		[SerializeField]
		private int gridSize = 8;

		// Token: 0x04000FD5 RID: 4053
		private PrefabPool<SnakePartDisplay> _partPool;

		// Token: 0x04000FD6 RID: 4054
		private PrefabPool<Transform> _foodPool;

		// Token: 0x04000FD7 RID: 4055
		private bool punchingColor;

		// Token: 0x04000FD8 RID: 4056
		private int punchColorIndex;
	}
}
