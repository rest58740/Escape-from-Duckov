using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Duckov.Utilities;
using Saves;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002E9 RID: 745
	public class BubblePopper : MiniGameBehaviour
	{
		// Token: 0x060017CD RID: 6093 RVA: 0x000576D4 File Offset: 0x000558D4
		public void NextPallette()
		{
			this.palletteIndex++;
			if (this.palletteIndex >= this.pallettes.Count)
			{
				this.palletteIndex = 0;
			}
			if (this.palletteIndex >= this.pallettes.Count)
			{
				return;
			}
			BubblePopper.Pallette pallette = this.pallettes[this.palletteIndex];
			this.SetPallette(pallette.colors);
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x0005773B File Offset: 0x0005593B
		public int AvaliableColorCount
		{
			get
			{
				return this.colorPallette.Length;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00057745 File Offset: 0x00055945
		public BubblePopperLayout Layout
		{
			get
			{
				return this.layout;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x0005774D File Offset: 0x0005594D
		public float BubbleRadius
		{
			get
			{
				if (this.bubbleTemplate == null)
				{
					return 8f;
				}
				return this.bubbleTemplate.Radius;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0005776E File Offset: 0x0005596E
		public Bubble BubbleTemplate
		{
			get
			{
				return this.bubbleTemplate;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00057778 File Offset: 0x00055978
		private PrefabPool<Bubble> BubblePool
		{
			get
			{
				if (this._bubblePool == null)
				{
					this._bubblePool = new PrefabPool<Bubble>(this.bubbleTemplate, null, new Action<Bubble>(this.OnGetBubble), null, null, true, 10, 10000, null);
				}
				return this._bubblePool;
			}
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000577BC File Offset: 0x000559BC
		private void OnGetBubble(Bubble bubble)
		{
			bubble.Rest();
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x000577C4 File Offset: 0x000559C4
		// (set) Token: 0x060017D5 RID: 6101 RVA: 0x000577CC File Offset: 0x000559CC
		public BubblePopper.Status status { get; private set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x000577D5 File Offset: 0x000559D5
		// (set) Token: 0x060017D7 RID: 6103 RVA: 0x000577DD File Offset: 0x000559DD
		public int FloorStepETA { get; private set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x000577E6 File Offset: 0x000559E6
		// (set) Token: 0x060017D9 RID: 6105 RVA: 0x000577EE File Offset: 0x000559EE
		public int Score
		{
			get
			{
				return this._score;
			}
			private set
			{
				this._score = value;
				this.RefreshScoreText();
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x000577FD File Offset: 0x000559FD
		// (set) Token: 0x060017DB RID: 6107 RVA: 0x00057809 File Offset: 0x00055A09
		public static int HighScore
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/BubblePopper/HighScore");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/BubblePopper/HighScore", value);
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x00057816 File Offset: 0x00055A16
		// (set) Token: 0x060017DD RID: 6109 RVA: 0x00057822 File Offset: 0x00055A22
		public static int HighLevel
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/BubblePopper/HighLevel");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/BubblePopper/HighLevel", value);
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x0005782F File Offset: 0x00055A2F
		// (set) Token: 0x060017DF RID: 6111 RVA: 0x00057837 File Offset: 0x00055A37
		public bool Busy { get; private set; }

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x060017E0 RID: 6112 RVA: 0x00057840 File Offset: 0x00055A40
		// (remove) Token: 0x060017E1 RID: 6113 RVA: 0x00057874 File Offset: 0x00055A74
		public static event Action<int> OnLevelClear;

		// Token: 0x060017E2 RID: 6114 RVA: 0x000578A7 File Offset: 0x00055AA7
		protected override void Start()
		{
			base.Start();
			this.RefreshScoreText();
			this.RefreshLevelText();
			this.HideEndScreen();
			this.ShowStartScreen();
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000578C8 File Offset: 0x00055AC8
		private void RefreshScoreText()
		{
			this.scoreText.text = string.Format("{0}", this.Score);
			this.highScoreText.text = string.Format("{0}", BubblePopper.HighScore);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00057914 File Offset: 0x00055B14
		private void RefreshLevelText()
		{
			this.levelText.text = string.Format("{0}", this.levelIndex);
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00057936 File Offset: 0x00055B36
		protected override void OnUpdate(float deltaTime)
		{
			this.UpdateStatus(deltaTime);
			this.HandleInput(deltaTime);
			this.UpdateAimingLine();
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0005794C File Offset: 0x00055B4C
		private void ShowStartScreen()
		{
			this.startScreen.SetActive(true);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0005795A File Offset: 0x00055B5A
		private void HideStartScreen()
		{
			this.startScreen.SetActive(false);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00057968 File Offset: 0x00055B68
		private void ShowEndScreen()
		{
			this.endScreen.SetActive(true);
			this.endScreenLevelText.text = string.Format("LEVEL {0}", this.levelIndex);
			this.endScreenScoreText.text = string.Format("{0}", this.Score);
			this.failIndicator.SetActive(this.fail);
			this.clearIndicator.SetActive(this.clear);
			this.newRecordIndicator.SetActive(this.isHighScore);
			this.allLevelsClearIndicator.SetActive(this.allLevelsClear);
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00057A05 File Offset: 0x00055C05
		private void HideEndScreen()
		{
			this.endScreen.SetActive(false);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00057A14 File Offset: 0x00055C14
		private void NewGame()
		{
			this.playing = true;
			this.levelIndex = 0;
			this.Score = 0;
			this.isHighScore = false;
			this.HideStartScreen();
			this.HideEndScreen();
			int[] levelData = this.LoadLevelData(this.levelIndex);
			this.StartNewLevel(levelData);
			this.RefreshLevelText();
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00057A64 File Offset: 0x00055C64
		private void NextLevel()
		{
			this.levelIndex++;
			this.HideStartScreen();
			this.HideEndScreen();
			int[] levelData = this.LoadLevelData(this.levelIndex);
			this.StartNewLevel(levelData);
			this.RefreshLevelText();
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00057AA5 File Offset: 0x00055CA5
		private int[] LoadLevelData(int levelIndex)
		{
			return this.levelDataProvider.GetData(levelIndex);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00057AB4 File Offset: 0x00055CB4
		private Vector2Int LevelDataIndexToCoord(int index)
		{
			int num = this.layout.XCoordBorder.y - this.layout.XCoordBorder.x + 1;
			int num2 = index / num;
			return new Vector2Int(index % num, -num2);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00057AF4 File Offset: 0x00055CF4
		private void StartNewLevel(int[] levelData)
		{
			this.clear = false;
			this.fail = false;
			this.FloorStepETA = this.floorStepAfterShots;
			this.BubblePool.ReleaseAll();
			this.attachedBubbles.Clear();
			this.ResetFloor();
			for (int i = 0; i < levelData.Length; i++)
			{
				int num = levelData[i];
				if (num >= 0)
				{
					Vector2Int coord = this.LevelDataIndexToCoord(i);
					Bubble bubble = this.BubblePool.Get(null);
					bubble.Setup(this, num);
					this.Set(bubble, coord);
				}
			}
			this.PushRandomColor();
			this.PushRandomColor();
			this.SetStatus(BubblePopper.Status.Loaded);
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00057B85 File Offset: 0x00055D85
		private void ResetFloor()
		{
			this.floorYCoord = this.initialFloorYCoord;
			this.RefreshLayoutPosition();
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00057B99 File Offset: 0x00055D99
		private void StepFloor()
		{
			this.floorYCoord++;
			this.BeginMovingCeiling();
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00057BB0 File Offset: 0x00055DB0
		private void RefreshLayoutPosition()
		{
			Vector3 localPosition = this.layout.transform.localPosition;
			localPosition.y = (float)(-(float)(this.floorYCoord - this.initialFloorYCoord)) * this.BubbleRadius * BubblePopperLayout.YOffsetFactor;
			this.layout.transform.localPosition = localPosition;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00057C04 File Offset: 0x00055E04
		private void UpdateStatus(float deltaTime)
		{
			switch (this.status)
			{
			case BubblePopper.Status.Idle:
			case BubblePopper.Status.GameOver:
				if (base.Game.GetButtonDown(MiniGame.Button.Start))
				{
					if (!this.playing || this.fail || this.allLevelsClear)
					{
						this.NewGame();
						return;
					}
					this.NextLevel();
					return;
				}
				break;
			case BubblePopper.Status.Loaded:
				break;
			case BubblePopper.Status.Launched:
				this.UpdateLaunched(deltaTime);
				return;
			case BubblePopper.Status.Settled:
				this.UpdateSettled(deltaTime);
				break;
			default:
				return;
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00057C76 File Offset: 0x00055E76
		private void BeginMovingCeiling()
		{
			this.movingCeiling = true;
			this.moveCeilingT = 0f;
			this.originalCeilingPos = this.layout.transform.localPosition;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00057CA8 File Offset: 0x00055EA8
		private void UpdateMoveCeiling(float deltaTime)
		{
			this.moveCeilingT += deltaTime;
			if (this.moveCeilingT >= this.moveCeilingTime)
			{
				this.movingCeiling = false;
				this.RefreshLayoutPosition();
				return;
			}
			Vector3 vector = this.layout.transform.localPosition;
			Vector2 vector2;
			vector2..ctor(vector.x, (float)(-(float)(this.floorYCoord - this.initialFloorYCoord)) * this.BubbleRadius * BubblePopperLayout.YOffsetFactor);
			float num = this.moveCeilingCurve.Evaluate(this.moveCeilingT / this.moveCeilingTime);
			vector = Vector2.LerpUnclamped(this.originalCeilingPos, vector2, num);
			this.layout.transform.localPosition = vector;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00057D56 File Offset: 0x00055F56
		private void UpdateSettled(float deltaTime)
		{
			if (this.movingCeiling)
			{
				this.UpdateMoveCeiling(deltaTime);
				return;
			}
			if (this.CheckGameOver())
			{
				this.SetStatus(BubblePopper.Status.GameOver);
				return;
			}
			this.SetStatus(BubblePopper.Status.Loaded);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00057D80 File Offset: 0x00055F80
		private void HandleFloorStep()
		{
			int floorStepETA = this.FloorStepETA;
			this.FloorStepETA = floorStepETA - 1;
			if (this.FloorStepETA <= 0)
			{
				this.StepFloor();
				this.FloorStepETA = this.floorStepAfterShots;
			}
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00057DB8 File Offset: 0x00055FB8
		private bool CheckGameOver()
		{
			if (this.attachedBubbles.Count == 0)
			{
				this.clear = true;
				this.allLevelsClear = (this.levelIndex >= this.levelDataProvider.TotalLevels);
				if (this.clear)
				{
					if (this.levelIndex > BubblePopper.HighLevel)
					{
						BubblePopper.HighLevel = this.levelIndex;
					}
					Action<int> onLevelClear = BubblePopper.OnLevelClear;
					if (onLevelClear != null)
					{
						onLevelClear(this.levelIndex);
					}
				}
				return true;
			}
			if (this.attachedBubbles.Keys.Any((Vector2Int e) => e.y <= this.floorYCoord))
			{
				this.fail = true;
				return true;
			}
			return false;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00057E58 File Offset: 0x00056058
		private void SetStatus(BubblePopper.Status newStatus)
		{
			this.OnExitStatus(this.status);
			this.status = newStatus;
			switch (this.status)
			{
			case BubblePopper.Status.Idle:
			case BubblePopper.Status.Loaded:
			case BubblePopper.Status.Launched:
				break;
			case BubblePopper.Status.Settled:
				this.PushRandomColor();
				this.HandleFloorStep();
				return;
			case BubblePopper.Status.GameOver:
				if (this.Score > BubblePopper.HighScore)
				{
					BubblePopper.HighScore = this.Score;
					this.isHighScore = true;
				}
				this.ShowGameOverScreen();
				break;
			default:
				return;
			}
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00057ECC File Offset: 0x000560CC
		private void ShowGameOverScreen()
		{
			this.ShowEndScreen();
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00057ED4 File Offset: 0x000560D4
		private void OnExitStatus(BubblePopper.Status status)
		{
			switch (status)
			{
			default:
				return;
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00057EEC File Offset: 0x000560EC
		private void Set(Bubble bubble, Vector2Int coord)
		{
			this.attachedBubbles[coord] = bubble;
			bubble.NotifyAttached(coord);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00057F04 File Offset: 0x00056104
		private void Attach(Bubble bubble, Vector2Int coord)
		{
			Bubble bubble2;
			if (this.attachedBubbles.TryGetValue(coord, out bubble2))
			{
				Debug.LogError("Target coord is occupied!");
				return;
			}
			this.Set(bubble, coord);
			List<Vector2Int> continousCoords = this.GetContinousCoords(coord);
			if (continousCoords.Count >= 3)
			{
				HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
				int num = 0;
				foreach (Vector2Int vector2Int in continousCoords)
				{
					LinqExtensions.AddRange<Vector2Int>(hashSet, this.layout.GetAllNeighbourCoords(vector2Int, false));
					this.Explode(vector2Int, coord);
					num++;
				}
				this.PunchCamera();
				HashSet<Vector2Int> looseCoords = this.GetLooseCoords(hashSet);
				foreach (Vector2Int coord2 in looseCoords)
				{
					this.Detach(coord2);
				}
				this.CalculateAndAddScore(looseCoords, continousCoords);
			}
			this.Shockwave(coord, this.shockwaveStrength).Forget();
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00058018 File Offset: 0x00056218
		private void CalculateAndAddScore(HashSet<Vector2Int> detached, List<Vector2Int> exploded)
		{
			float count = (float)exploded.Count;
			int count2 = detached.Count;
			int num = Mathf.FloorToInt(Mathf.Pow(count, 2f)) * (1 + count2);
			this.Score += num;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00058058 File Offset: 0x00056258
		private void Explode(Vector2Int coord, Vector2Int origin)
		{
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(coord, out bubble))
			{
				return;
			}
			this.attachedBubbles.Remove(coord);
			if (bubble == null)
			{
				return;
			}
			bubble.NotifyExplode(origin);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00058094 File Offset: 0x00056294
		private List<Vector2Int> GetContinousCoords(Vector2Int root)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(root, out bubble))
			{
				return list;
			}
			if (bubble == null)
			{
				return list;
			}
			int colorIndex = bubble.ColorIndex;
			BubblePopper.<>c__DisplayClass121_0 CS$<>8__locals1;
			CS$<>8__locals1.visitedCoords = new HashSet<Vector2Int>();
			CS$<>8__locals1.coords = new Stack<Vector2Int>();
			BubblePopper.<GetContinousCoords>g__Push|121_0(root, ref CS$<>8__locals1);
			while (CS$<>8__locals1.coords.Count > 0)
			{
				Vector2Int vector2Int = CS$<>8__locals1.coords.Pop();
				Bubble bubble2;
				if (this.attachedBubbles.TryGetValue(vector2Int, out bubble2) && !(bubble2 == null) && bubble2.ColorIndex == colorIndex)
				{
					list.Add(vector2Int);
					foreach (Vector2Int vector2Int2 in this.layout.GetAllNeighbourCoords(vector2Int, false))
					{
						if (!CS$<>8__locals1.visitedCoords.Contains(vector2Int2))
						{
							BubblePopper.<GetContinousCoords>g__Push|121_0(vector2Int2, ref CS$<>8__locals1);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00058184 File Offset: 0x00056384
		private HashSet<Vector2Int> GetLooseCoords(HashSet<Vector2Int> roots)
		{
			BubblePopper.<>c__DisplayClass122_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pendingRoots = roots.ToList<Vector2Int>();
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			while (CS$<>8__locals1.pendingRoots.Count > 0)
			{
				Vector2Int root = this.<GetLooseCoords>g__PopRoot|122_0(ref CS$<>8__locals1);
				List<Vector2Int> list;
				if (this.<GetLooseCoords>g__CheckConnectedLoose|122_1(root, out list, ref CS$<>8__locals1))
				{
					LinqExtensions.AddRange<Vector2Int>(hashSet, list);
				}
			}
			return hashSet;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x000581DC File Offset: 0x000563DC
		private void Detach(Vector2Int coord)
		{
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(coord, out bubble))
			{
				return;
			}
			this.attachedBubbles.Remove(coord);
			bubble.NotifyDetached();
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00058210 File Offset: 0x00056410
		private void UpdateAimingLine()
		{
			this.aimingLine.gameObject.SetActive(this.status == BubblePopper.Status.Loaded);
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint(this.cannon.position);
			Vector3 vector2 = worldToLocalMatrix.MultiplyVector(this.cannon.up);
			Vector3 vector3 = vector2 * this.aimingDistance;
			BubblePopper.CastResult castResult = this.SlideCast(vector, vector3);
			vector.z = 0f;
			this.aimlinePoints[0] = vector;
			this.aimlinePoints[1] = castResult.endPosition;
			if (castResult.touchWall)
			{
				float num = Mathf.Max(this.aimingDistance - (castResult.endPosition - vector).magnitude, 0f);
				Vector2 vector4 = vector2;
				vector4.x *= -1f;
				this.aimlinePoints[2] = castResult.endPosition + vector4 * num;
			}
			else
			{
				this.aimlinePoints[2] = castResult.endPosition;
			}
			this.aimingLine.SetPositions(this.aimlinePoints);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0005835F File Offset: 0x0005655F
		private void UpdateLaunched(float deltaTime)
		{
			if (this.activeBubble == null || this.activeBubble.status != Bubble.Status.Moving)
			{
				this.activeBubble = null;
				this.SetStatus(BubblePopper.Status.Settled);
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0005838C File Offset: 0x0005658C
		private void HandleInput(float deltaTime)
		{
			float x = base.Game.GetAxis(0).x;
			this.cannonAngle = Mathf.Clamp(this.cannonAngle - x * this.cannonRotateSpeed * deltaTime, this.cannonAngleRange.x, this.cannonAngleRange.y);
			this.cannon.rotation = Quaternion.Euler(0f, 0f, this.cannonAngle);
			this.duckAnimator.SetInteger("MovementDirection", (x > 0.01f) ? 1 : ((x < -0.01f) ? -1 : 0));
			this.gear.Rotate(0f, 0f, x * this.cannonRotateSpeed * deltaTime);
			if (base.Game.GetButtonDown(MiniGame.Button.A))
			{
				this.LaunchBubble();
			}
			if (base.Game.GetButtonDown(MiniGame.Button.B))
			{
				this.NextPallette();
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0005846C File Offset: 0x0005666C
		public void MoveBubble(Bubble bubble, float deltaTime)
		{
			if (bubble == null)
			{
				return;
			}
			Vector2 moveDirection = bubble.MoveDirection;
			float num = deltaTime * this.bubbleMoveSpeed;
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Matrix4x4 localToWorldMatrix = this.layout.transform.localToWorldMatrix;
			Vector2 normalized = moveDirection.normalized;
			Vector2 origin = worldToLocalMatrix.MultiplyPoint(bubble.transform.position);
			Vector2 delta = worldToLocalMatrix.MultiplyVector(moveDirection.normalized) * num;
			BubblePopper.CastResult castResult = this.SlideCast(origin, delta);
			bubble.transform.position = localToWorldMatrix.MultiplyPoint(castResult.endPosition);
			if (!castResult.Collide)
			{
				return;
			}
			if (castResult.touchWall && (float)castResult.touchWallDirection * normalized.x > 0f)
			{
				moveDirection.x *= -1f;
				bubble.MoveDirection = moveDirection;
			}
			if (castResult.touchingBubble || castResult.touchCeiling)
			{
				this.Attach(bubble, castResult.endCoord);
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00058588 File Offset: 0x00056788
		private Bubble LaunchBubble(Vector2 origin, Vector2 direction, int colorIndex)
		{
			Bubble bubble = this.BubblePool.Get(null);
			bubble.transform.position = this.layout.transform.localToWorldMatrix.MultiplyPoint(origin);
			bubble.MoveDirection = direction;
			bubble.Setup(this, colorIndex);
			bubble.Launch(direction);
			return bubble;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000585E0 File Offset: 0x000567E0
		private void LaunchBubble()
		{
			if (this.status != BubblePopper.Status.Loaded)
			{
				return;
			}
			this.activeBubble = this.LaunchBubble(this.layout.transform.worldToLocalMatrix.MultiplyPoint(this.cannon.transform.position), this.layout.transform.worldToLocalMatrix.MultiplyVector(this.cannon.transform.up), this.loadedColor);
			this.loadedColor = -1;
			this.RefreshColorIndicators();
			this.SetStatus(BubblePopper.Status.Launched);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00058678 File Offset: 0x00056878
		private void PunchLoadedIndicator()
		{
			ShortcutExtensions.DOKill(this.loadedColorIndicator.transform, true);
			this.loadedColorIndicator.transform.localPosition = Vector2.left * 15f;
			ShortcutExtensions.DOLocalMove(this.loadedColorIndicator.transform, Vector3.zero, 0.1f, true);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000586D8 File Offset: 0x000568D8
		private void PunchWaitingIndicator()
		{
			this.waitingColorIndicator.transform.localPosition = Vector2.zero;
			ShortcutExtensions.DOKill(this.waitingColorIndicator.transform, true);
			ShortcutExtensions.DOPunchPosition(this.waitingColorIndicator.transform, Vector3.down * 5f, 0.5f, 10, 1f, true);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00058740 File Offset: 0x00056940
		private void PushRandomColor()
		{
			this.loadedColor = this.waitingColor;
			this.waitingColor = Random.Range(0, this.AvaliableColorCount);
			if (this.attachedBubbles.Count <= 0)
			{
				this.waitingColor = Random.Range(0, this.AvaliableColorCount);
			}
			List<int> list = (from e in this.attachedBubbles.Values
			group e by e.ColorIndex into g
			select g.Key).ToList<int>();
			this.waitingColor = list.GetRandom<int>();
			this.RefreshColorIndicators();
			this.PunchLoadedIndicator();
			this.PunchWaitingIndicator();
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00058802 File Offset: 0x00056A02
		private void RefreshColorIndicators()
		{
			this.loadedColorIndicator.color = this.GetDisplayColor(this.loadedColor);
			this.waitingColorIndicator.color = this.GetDisplayColor(this.waitingColor);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00058832 File Offset: 0x00056A32
		private bool IsCoordOccupied(Vector2Int coord, out Bubble touchingBubble, out bool ceiling)
		{
			ceiling = false;
			if (this.attachedBubbles.TryGetValue(coord, out touchingBubble))
			{
				return true;
			}
			if (coord.y > this.ceilingYCoord)
			{
				ceiling = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00058860 File Offset: 0x00056A60
		public BubblePopper.CastResult SlideCast(Vector2 origin, Vector2 delta)
		{
			float magnitude = delta.magnitude;
			Vector2 normalized = delta.normalized;
			float bubbleRadius = this.BubbleRadius;
			BubblePopper.CastResult castResult = default(BubblePopper.CastResult);
			castResult.origin = origin;
			castResult.castDirection = normalized;
			castResult.castDistance = magnitude;
			Vector2 vector = origin + delta;
			float num = 1f;
			float num2 = this.layout.XPositionBorder.x + bubbleRadius;
			float num3 = this.layout.XPositionBorder.y - bubbleRadius;
			if (origin.x < num2 || origin.x > num3)
			{
				Vector2 vector2 = origin;
				vector2.x = Mathf.Clamp(vector2.x, num2 + 0.001f, num3 - 0.001f);
				castResult.endPosition = vector2;
				castResult.clipWall = true;
				castResult.collide = true;
			}
			else
			{
				if (vector.x < num2)
				{
					castResult.touchWall = true;
					num = Mathf.Abs(origin.x - num2) / Mathf.Abs(delta.x);
					castResult.touchWallDirection = -1;
				}
				else if (vector.x > num3)
				{
					castResult.touchWall = true;
					num = Mathf.Abs(num3 - origin.x) / Mathf.Abs(delta.x);
					castResult.touchWallDirection = 1;
				}
				delta *= num;
				magnitude = delta.magnitude;
				castResult.endPosition = origin + delta;
				List<Vector2Int> allPassingCoords = this.layout.GetAllPassingCoords(origin, normalized, delta.magnitude);
				float num4 = magnitude;
				foreach (Vector2Int vector2Int in allPassingCoords)
				{
					Bubble touchingBubble;
					bool touchCeiling;
					Vector2 vector3;
					if (this.IsCoordOccupied(vector2Int, out touchingBubble, out touchCeiling) && this.BubbleCast(this.layout.CoordToLocalPosition(vector2Int), origin, normalized, magnitude, out vector3))
					{
						float magnitude2 = (vector3 - origin).magnitude;
						if (magnitude2 < num4)
						{
							castResult.collide = true;
							castResult.touchingBubble = touchingBubble;
							castResult.touchBubbleCoord = vector2Int;
							castResult.endPosition = vector3;
							castResult.touchCeiling = touchCeiling;
							num4 = magnitude2;
							castResult.touchWall = false;
						}
					}
				}
			}
			castResult.endCoord = this.layout.LocalPositionToCoord(castResult.endPosition);
			return castResult;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00058AB0 File Offset: 0x00056CB0
		private bool BubbleCast(Vector2 pos, Vector2 origin, Vector2 direction, float distance, out Vector2 hitCircleCenter)
		{
			float bubbleRadius = this.BubbleRadius;
			hitCircleCenter = origin;
			Vector2 vector = pos - origin;
			float sqrMagnitude = vector.sqrMagnitude;
			float magnitude = vector.magnitude;
			if (magnitude > distance + 2f * bubbleRadius)
			{
				return false;
			}
			if (magnitude <= bubbleRadius * 2f)
			{
				hitCircleCenter = pos - 2f * vector.normalized * bubbleRadius;
				return true;
			}
			if (Vector2.Dot(vector, direction) < 0f)
			{
				return false;
			}
			float num = 0.017453292f * Vector2.Angle(vector, direction);
			float num2 = vector.magnitude * Mathf.Sin(num);
			if (num2 > 2f * bubbleRadius)
			{
				return false;
			}
			float num3 = num2 * num2;
			float num4 = bubbleRadius * bubbleRadius * 2f * 2f;
			float num5 = Mathf.Sqrt(sqrMagnitude - num3) - Mathf.Sqrt(num4 - num3);
			if (num5 > distance)
			{
				return false;
			}
			hitCircleCenter = origin + direction * num5;
			return true;
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00058BAC File Offset: 0x00056DAC
		private void OnDrawGizmos()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			float bubbleRadius = this.BubbleRadius;
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint(this.cannon.position);
			Vector3 vector2 = worldToLocalMatrix.MultiplyVector(this.cannon.up);
			BubblePopper.CastResult castResult = this.SlideCast(vector, vector2 * this.distance);
			Gizmos.matrix = this.layout.transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
			for (int i = this.layout.XCoordBorder.x; i <= this.layout.XCoordBorder.y; i++)
			{
				for (int j = this.floorYCoord; j <= this.ceilingYCoord; j++)
				{
					new Vector2Int(i, j);
					this.layout.GizmosDrawCoord(new Vector2Int(i, j), 0.25f);
				}
			}
			Gizmos.color = (castResult.Collide ? Color.red : Color.green);
			Gizmos.DrawWireSphere(vector, bubbleRadius);
			Gizmos.DrawWireSphere(castResult.endPosition, bubbleRadius);
			Gizmos.DrawLine(vector, castResult.endPosition);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(this.layout.CoordToLocalPosition(castResult.endCoord), bubbleRadius * 0.8f);
			if (castResult.collide)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(this.layout.CoordToLocalPosition(castResult.touchBubbleCoord), bubbleRadius * 0.5f);
			}
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00058D65 File Offset: 0x00056F65
		internal void Release(Bubble bubble)
		{
			this.BubblePool.Release(bubble);
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00058D73 File Offset: 0x00056F73
		internal Color GetDisplayColor(int colorIndex)
		{
			if (colorIndex < 0)
			{
				return Color.clear;
			}
			if (colorIndex >= this.colorPallette.Length)
			{
				return Color.white;
			}
			return this.colorPallette[colorIndex];
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00058D9C File Offset: 0x00056F9C
		public void SetPallette(Color[] colors)
		{
			this.colorPallette = new Color[colors.Length];
			colors.CopyTo(this.colorPallette, 0);
			foreach (Bubble bubble in this.BubblePool.ActiveEntries)
			{
				bubble.RefreshColor();
			}
			this.RefreshColorIndicators();
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00058E0C File Offset: 0x0005700C
		private UniTask Shockwave(Vector2Int origin, float amplitude)
		{
			BubblePopper.<Shockwave>d__145 <Shockwave>d__;
			<Shockwave>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Shockwave>d__.<>4__this = this;
			<Shockwave>d__.origin = origin;
			<Shockwave>d__.amplitude = amplitude;
			<Shockwave>d__.<>1__state = -1;
			<Shockwave>d__.<>t__builder.Start<BubblePopper.<Shockwave>d__145>(ref <Shockwave>d__);
			return <Shockwave>d__.<>t__builder.Task;
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00058E60 File Offset: 0x00057060
		private void PunchCamera()
		{
			ShortcutExtensions.DOKill(this.cameraParent, true);
			ShortcutExtensions.DOShakePosition(this.cameraParent, 0.4f, 1f, 10, 90f, false, true);
			ShortcutExtensions.DOShakeRotation(this.cameraParent, 0.4f, Vector3.forward, 10, 90f, true);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x00058F58 File Offset: 0x00057158
		[CompilerGenerated]
		internal static void <GetContinousCoords>g__Push|121_0(Vector2Int coord, ref BubblePopper.<>c__DisplayClass121_0 A_1)
		{
			A_1.coords.Push(coord);
			A_1.visitedCoords.Add(coord);
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00058F73 File Offset: 0x00057173
		[CompilerGenerated]
		private Vector2Int <GetLooseCoords>g__PopRoot|122_0(ref BubblePopper.<>c__DisplayClass122_0 A_1)
		{
			Vector2Int result = A_1.pendingRoots[0];
			A_1.pendingRoots.RemoveAt(0);
			return result;
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00058F90 File Offset: 0x00057190
		[CompilerGenerated]
		private bool <GetLooseCoords>g__CheckConnectedLoose|122_1(Vector2Int root, out List<Vector2Int> connected, ref BubblePopper.<>c__DisplayClass122_0 A_3)
		{
			connected = new List<Vector2Int>();
			bool result = true;
			Stack<Vector2Int> stack = new Stack<Vector2Int>();
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			stack.Push(root);
			hashSet.Add(root);
			while (stack.Count > 0)
			{
				Vector2Int vector2Int = stack.Pop();
				A_3.pendingRoots.Remove(vector2Int);
				if (this.attachedBubbles.ContainsKey(vector2Int))
				{
					if (vector2Int.y >= this.ceilingYCoord)
					{
						result = false;
					}
					connected.Add(vector2Int);
					foreach (Vector2Int item in this.layout.GetAllNeighbourCoords(vector2Int, false))
					{
						if (!hashSet.Contains(item))
						{
							stack.Push(item);
							hashSet.Add(item);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400116A RID: 4458
		[SerializeField]
		private Bubble bubbleTemplate;

		// Token: 0x0400116B RID: 4459
		[SerializeField]
		private BubblePopperLayout layout;

		// Token: 0x0400116C RID: 4460
		[SerializeField]
		private Image waitingColorIndicator;

		// Token: 0x0400116D RID: 4461
		[SerializeField]
		private Image loadedColorIndicator;

		// Token: 0x0400116E RID: 4462
		[SerializeField]
		private Transform cannon;

		// Token: 0x0400116F RID: 4463
		[SerializeField]
		private LineRenderer aimingLine;

		// Token: 0x04001170 RID: 4464
		[SerializeField]
		private Transform cameraParent;

		// Token: 0x04001171 RID: 4465
		[SerializeField]
		private Animator duckAnimator;

		// Token: 0x04001172 RID: 4466
		[SerializeField]
		private Transform gear;

		// Token: 0x04001173 RID: 4467
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04001174 RID: 4468
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x04001175 RID: 4469
		[SerializeField]
		private TextMeshProUGUI highScoreText;

		// Token: 0x04001176 RID: 4470
		[SerializeField]
		private GameObject startScreen;

		// Token: 0x04001177 RID: 4471
		[SerializeField]
		private GameObject endScreen;

		// Token: 0x04001178 RID: 4472
		[SerializeField]
		private GameObject failIndicator;

		// Token: 0x04001179 RID: 4473
		[SerializeField]
		private GameObject clearIndicator;

		// Token: 0x0400117A RID: 4474
		[SerializeField]
		private GameObject newRecordIndicator;

		// Token: 0x0400117B RID: 4475
		[SerializeField]
		private GameObject allLevelsClearIndicator;

		// Token: 0x0400117C RID: 4476
		[SerializeField]
		private TextMeshProUGUI endScreenLevelText;

		// Token: 0x0400117D RID: 4477
		[SerializeField]
		private TextMeshProUGUI endScreenScoreText;

		// Token: 0x0400117E RID: 4478
		[SerializeField]
		private BubblePopperLevelDataProvider levelDataProvider;

		// Token: 0x0400117F RID: 4479
		[SerializeField]
		private Color[] colorPallette;

		// Token: 0x04001180 RID: 4480
		private int palletteIndex;

		// Token: 0x04001181 RID: 4481
		[SerializeField]
		public List<BubblePopper.Pallette> pallettes;

		// Token: 0x04001182 RID: 4482
		[SerializeField]
		private float aimingDistance = 100f;

		// Token: 0x04001183 RID: 4483
		[SerializeField]
		private Vector2 cannonAngleRange = new Vector2(-45f, 45f);

		// Token: 0x04001184 RID: 4484
		[SerializeField]
		private float cannonRotateSpeed = 20f;

		// Token: 0x04001185 RID: 4485
		[SerializeField]
		private int ceilingYCoord;

		// Token: 0x04001186 RID: 4486
		[SerializeField]
		private int initialFloorYCoord = -18;

		// Token: 0x04001187 RID: 4487
		[SerializeField]
		private int floorStepAfterShots = 4;

		// Token: 0x04001188 RID: 4488
		[SerializeField]
		private float bubbleMoveSpeed = 100f;

		// Token: 0x04001189 RID: 4489
		private float shockwaveStrength = 2f;

		// Token: 0x0400118A RID: 4490
		[SerializeField]
		private float moveCeilingTime = 1f;

		// Token: 0x0400118B RID: 4491
		[SerializeField]
		private AnimationCurve moveCeilingCurve;

		// Token: 0x0400118C RID: 4492
		private PrefabPool<Bubble> _bubblePool;

		// Token: 0x0400118D RID: 4493
		private Dictionary<Vector2Int, Bubble> attachedBubbles = new Dictionary<Vector2Int, Bubble>();

		// Token: 0x0400118E RID: 4494
		private float cannonAngle;

		// Token: 0x0400118F RID: 4495
		private int waitingColor;

		// Token: 0x04001190 RID: 4496
		private int loadedColor;

		// Token: 0x04001191 RID: 4497
		private Bubble activeBubble;

		// Token: 0x04001193 RID: 4499
		private bool clear;

		// Token: 0x04001194 RID: 4500
		private bool fail;

		// Token: 0x04001195 RID: 4501
		private bool allLevelsClear;

		// Token: 0x04001196 RID: 4502
		private bool playing;

		// Token: 0x04001197 RID: 4503
		[SerializeField]
		private int floorYCoord;

		// Token: 0x04001199 RID: 4505
		private int levelIndex;

		// Token: 0x0400119A RID: 4506
		private int _score;

		// Token: 0x0400119B RID: 4507
		private bool isHighScore;

		// Token: 0x0400119C RID: 4508
		private const string HighScoreSaveKey = "MiniGame/BubblePopper/HighScore";

		// Token: 0x0400119D RID: 4509
		private const string HighLevelSaveKey = "MiniGame/BubblePopper/HighLevel";

		// Token: 0x0400119F RID: 4511
		private const int CriticalCount = 3;

		// Token: 0x040011A1 RID: 4513
		private bool movingCeiling;

		// Token: 0x040011A2 RID: 4514
		private float moveCeilingT;

		// Token: 0x040011A3 RID: 4515
		private Vector2 originalCeilingPos;

		// Token: 0x040011A4 RID: 4516
		private Vector3[] aimlinePoints = new Vector3[3];

		// Token: 0x040011A5 RID: 4517
		[SerializeField]
		private bool drawGizmos = true;

		// Token: 0x040011A6 RID: 4518
		[SerializeField]
		private float distance;

		// Token: 0x02000597 RID: 1431
		[Serializable]
		public struct Pallette
		{
			// Token: 0x04002071 RID: 8305
			public Color[] colors;
		}

		// Token: 0x02000598 RID: 1432
		public enum Status
		{
			// Token: 0x04002073 RID: 8307
			Idle,
			// Token: 0x04002074 RID: 8308
			Loaded,
			// Token: 0x04002075 RID: 8309
			Launched,
			// Token: 0x04002076 RID: 8310
			Settled,
			// Token: 0x04002077 RID: 8311
			GameOver
		}

		// Token: 0x02000599 RID: 1433
		public struct CastResult
		{
			// Token: 0x1700079C RID: 1948
			// (get) Token: 0x0600298C RID: 10636 RVA: 0x0009A1EE File Offset: 0x000983EE
			public bool Collide
			{
				get
				{
					return this.collide || this.clipWall || this.touchWall || this.touchingBubble;
				}
			}

			// Token: 0x04002078 RID: 8312
			public Vector2 origin;

			// Token: 0x04002079 RID: 8313
			public Vector2 castDirection;

			// Token: 0x0400207A RID: 8314
			public float castDistance;

			// Token: 0x0400207B RID: 8315
			public bool clipWall;

			// Token: 0x0400207C RID: 8316
			public bool touchWall;

			// Token: 0x0400207D RID: 8317
			public int touchWallDirection;

			// Token: 0x0400207E RID: 8318
			public bool collide;

			// Token: 0x0400207F RID: 8319
			public Bubble touchingBubble;

			// Token: 0x04002080 RID: 8320
			public Vector2Int touchBubbleCoord;

			// Token: 0x04002081 RID: 8321
			public bool touchCeiling;

			// Token: 0x04002082 RID: 8322
			public Vector2 endPosition;

			// Token: 0x04002083 RID: 8323
			public Vector2Int endCoord;
		}
	}
}
