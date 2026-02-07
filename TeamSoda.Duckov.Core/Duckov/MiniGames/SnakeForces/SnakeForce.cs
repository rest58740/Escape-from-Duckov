using System;
using System.Collections.Generic;
using DG.Tweening;
using Duckov.Utilities;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x02000298 RID: 664
	public class SnakeForce : MiniGameBehaviour
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001595 RID: 5525 RVA: 0x00050730 File Offset: 0x0004E930
		public List<SnakeForce.Part> Snake
		{
			get
			{
				return this.snake;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x00050738 File Offset: 0x0004E938
		public List<Vector2Int> Foods
		{
			get
			{
				return this.foods;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001597 RID: 5527 RVA: 0x00050740 File Offset: 0x0004E940
		// (set) Token: 0x06001598 RID: 5528 RVA: 0x00050748 File Offset: 0x0004E948
		public int Score
		{
			get
			{
				return this._score;
			}
			private set
			{
				this._score = value;
				Action<SnakeForce> onScoreChanged = this.OnScoreChanged;
				if (onScoreChanged == null)
				{
					return;
				}
				onScoreChanged(this);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001599 RID: 5529 RVA: 0x00050762 File Offset: 0x0004E962
		// (set) Token: 0x0600159A RID: 5530 RVA: 0x0005076E File Offset: 0x0004E96E
		public static int HighScore
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/Snake/HighScore");
			}
			private set
			{
				SavesSystem.Save<int>("MiniGame/Snake/HighScore", value);
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600159B RID: 5531 RVA: 0x0005077B File Offset: 0x0004E97B
		public SnakeForce.Part Head
		{
			get
			{
				if (this.snake.Count <= 0)
				{
					return null;
				}
				return this.snake[0];
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x00050799 File Offset: 0x0004E999
		public SnakeForce.Part Tail
		{
			get
			{
				if (this.snake.Count <= 0)
				{
					return null;
				}
				List<SnakeForce.Part> list = this.snake;
				return list[list.Count - 1];
			}
		}

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x0600159D RID: 5533 RVA: 0x000507C0 File Offset: 0x0004E9C0
		// (remove) Token: 0x0600159E RID: 5534 RVA: 0x000507F8 File Offset: 0x0004E9F8
		public event Action<SnakeForce.Part> OnAddPart;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x0600159F RID: 5535 RVA: 0x00050830 File Offset: 0x0004EA30
		// (remove) Token: 0x060015A0 RID: 5536 RVA: 0x00050868 File Offset: 0x0004EA68
		public event Action<SnakeForce.Part> OnRemovePart;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x060015A1 RID: 5537 RVA: 0x000508A0 File Offset: 0x0004EAA0
		// (remove) Token: 0x060015A2 RID: 5538 RVA: 0x000508D8 File Offset: 0x0004EAD8
		public event Action<SnakeForce> OnAfterTick;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x060015A3 RID: 5539 RVA: 0x00050910 File Offset: 0x0004EB10
		// (remove) Token: 0x060015A4 RID: 5540 RVA: 0x00050948 File Offset: 0x0004EB48
		public event Action<SnakeForce> OnScoreChanged;

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x060015A5 RID: 5541 RVA: 0x00050980 File Offset: 0x0004EB80
		// (remove) Token: 0x060015A6 RID: 5542 RVA: 0x000509B8 File Offset: 0x0004EBB8
		public event Action<SnakeForce> OnGameStart;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x060015A7 RID: 5543 RVA: 0x000509F0 File Offset: 0x0004EBF0
		// (remove) Token: 0x060015A8 RID: 5544 RVA: 0x00050A28 File Offset: 0x0004EC28
		public event Action<SnakeForce> OnGameOver;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060015A9 RID: 5545 RVA: 0x00050A60 File Offset: 0x0004EC60
		// (remove) Token: 0x060015AA RID: 5546 RVA: 0x00050A98 File Offset: 0x0004EC98
		public event Action<SnakeForce, Vector2Int> OnFoodEaten;

		// Token: 0x060015AB RID: 5547 RVA: 0x00050ACD File Offset: 0x0004ECCD
		protected override void Start()
		{
			base.Start();
			this.titleScreen.SetActive(true);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00050AE4 File Offset: 0x0004ECE4
		private void Restart()
		{
			this.Clear();
			this.gameOverScreen.SetActive(false);
			for (int i = this.borderXMin; i <= this.borderXMax; i++)
			{
				for (int j = this.borderYMin; j <= this.borderYMax; j++)
				{
					this.allCoords.Add(new Vector2Int(i, j));
				}
			}
			this.AddPart(new Vector2Int((this.borderXMax + this.borderXMin) / 2, (this.borderYMax + this.borderYMin) / 2), Vector2Int.up);
			this.Grow();
			this.Grow();
			this.AddFood(3);
			this.PunchCamera();
			this.playing = true;
			this.RefreshScoreText();
			this.highScoreText.text = string.Format("{0}", SnakeForce.HighScore);
			Action<SnakeForce> onGameStart = this.OnGameStart;
			if (onGameStart == null)
			{
				return;
			}
			onGameStart(this);
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00050BC8 File Offset: 0x0004EDC8
		private void AddFood(int count = 3)
		{
			List<Vector2Int> list = new List<Vector2Int>(this.allCoords);
			foreach (SnakeForce.Part part in this.snake)
			{
				list.Remove(part.coord);
			}
			if (list.Count <= 0)
			{
				this.Win();
				return;
			}
			foreach (Vector2Int item in list.GetRandomSubSet(count))
			{
				this.foods.Add(item);
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00050C70 File Offset: 0x0004EE70
		private void GameOver()
		{
			Action<SnakeForce> onGameOver = this.OnGameOver;
			if (onGameOver != null)
			{
				onGameOver(this);
			}
			bool active = this.Score > SnakeForce.HighScore;
			if (this.Score > SnakeForce.HighScore)
			{
				SnakeForce.HighScore = this.Score;
			}
			this.highScoreIndicator.SetActive(active);
			this.winIndicator.SetActive(this.won);
			this.scoreTextGameOver.text = string.Format("{0}", this.Score);
			this.gameOverScreen.SetActive(true);
			this.PunchCamera();
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00050D04 File Offset: 0x0004EF04
		private void Win()
		{
			this.won = true;
			this.GameOver();
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00050D14 File Offset: 0x0004EF14
		protected override void OnUpdate(float deltaTime)
		{
			Vector2 axis = base.Game.GetAxis(0);
			if (axis.sqrMagnitude > 0.1f)
			{
				Vector2Int vector2Int = default(Vector2Int);
				if (axis.x > 0f)
				{
					vector2Int = Vector2Int.right;
				}
				else if (axis.x < 0f)
				{
					vector2Int = Vector2Int.left;
				}
				else if (axis.y > 0f)
				{
					vector2Int = Vector2Int.up;
				}
				else if (axis.y < 0f)
				{
					vector2Int = Vector2Int.down;
				}
				if (this.lastFrameAxis != vector2Int)
				{
					this.axisInput = true;
				}
				this.lastFrameAxis = vector2Int;
			}
			else
			{
				this.lastFrameAxis = Vector2Int.zero;
			}
			if (this.freezeCountDown > 0.0)
			{
				this.freezeCountDown -= (double)Time.unscaledDeltaTime;
				return;
			}
			if (this.dead || this.won || !this.playing)
			{
				if (base.Game.GetButtonDown(MiniGame.Button.Start))
				{
					this.Restart();
				}
				return;
			}
			this.RefreshScoreText();
			bool flag = base.Game.GetButton(MiniGame.Button.B) || base.Game.GetButton(MiniGame.Button.A);
			this.tickETA -= deltaTime * (flag ? 10f : 1f);
			float num = (this.playTick < (ulong)((long)this.maxSpeedTick)) ? (this.playTick / (float)this.maxSpeedTick) : 1f;
			float num2 = Mathf.Lerp(this.tickIntervalFrom, this.tickIntervalTo, this.speedCurve.Evaluate(num));
			if (this.tickETA <= 0f || this.axisInput)
			{
				this.Tick();
				this.tickETA = num2;
				this.axisInput = false;
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00050EC7 File Offset: 0x0004F0C7
		private void RefreshScoreText()
		{
			this.scoreText.text = string.Format("{0}", this.Score);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00050EE9 File Offset: 0x0004F0E9
		private void Tick()
		{
			this.playTick += 1UL;
			if (this.Head == null)
			{
				return;
			}
			this.HandleMovement();
			this.DetectDeath();
			this.HandleEatAndGrow();
			Action<SnakeForce> onAfterTick = this.OnAfterTick;
			if (onAfterTick == null)
			{
				return;
			}
			onAfterTick(this);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00050F28 File Offset: 0x0004F128
		private void HandleMovement()
		{
			Vector2Int vector2Int = this.lastFrameAxis;
			if ((!(vector2Int == -this.Head.direction) || this.snake.Count <= 1) && vector2Int != Vector2Int.zero)
			{
				this.Head.direction = vector2Int;
			}
			for (int i = this.snake.Count - 1; i >= 0; i--)
			{
				SnakeForce.Part part = this.snake[i];
				Vector2Int coord = (i > 0) ? this.snake[i - 1].coord : (part.coord + part.direction);
				if (i > 0)
				{
					part.direction = this.snake[i - 1].direction;
				}
				if (coord.x > this.borderXMax)
				{
					coord.x = this.borderXMin;
				}
				if (coord.y > this.borderYMax)
				{
					coord.y = this.borderYMin;
				}
				if (coord.x < this.borderXMin)
				{
					coord.x = this.borderXMax;
				}
				if (coord.y < this.borderYMin)
				{
					coord.y = this.borderYMax;
				}
				part.MoveTo(coord);
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00051068 File Offset: 0x0004F268
		private void HandleEatAndGrow()
		{
			Vector2Int coord = this.Head.coord;
			if (this.foods.Remove(coord))
			{
				this.Grow();
				int score = this.Score;
				this.Score = score + 1;
				int num = 3 + Mathf.FloorToInt(Mathf.Log((float)this.Score, 2f));
				int count = Mathf.Max(0, num - this.foods.Count);
				this.AddFood(count);
				Action<SnakeForce, Vector2Int> onFoodEaten = this.OnFoodEaten;
				if (onFoodEaten != null)
				{
					onFoodEaten(this, coord);
				}
				this.PunchCamera();
			}
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000510F4 File Offset: 0x0004F2F4
		private void DetectDeath()
		{
			Vector2Int coord = this.Head.coord;
			for (int i = 1; i < this.snake.Count; i++)
			{
				if (this.snake[i].coord == coord)
				{
					this.dead = true;
					this.GameOver();
					return;
				}
			}
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0005114C File Offset: 0x0004F34C
		private SnakeForce.Part Grow()
		{
			if (this.snake.Count == 0)
			{
				Debug.LogError("Cannot grow the snake! It haven't been created yet.");
				return null;
			}
			SnakeForce.Part tail = this.Tail;
			Vector2Int coord = tail.coord - tail.direction;
			return this.AddPart(coord, tail.direction);
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x00051198 File Offset: 0x0004F398
		private SnakeForce.Part AddPart(Vector2Int coord, Vector2Int direction)
		{
			SnakeForce.Part part = new SnakeForce.Part(this, coord, direction);
			this.snake.Add(part);
			Action<SnakeForce.Part> onAddPart = this.OnAddPart;
			if (onAddPart != null)
			{
				onAddPart(part);
			}
			return part;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000511CD File Offset: 0x0004F3CD
		private bool RemovePart(SnakeForce.Part part)
		{
			if (!this.snake.Remove(part))
			{
				return false;
			}
			Action<SnakeForce.Part> onRemovePart = this.OnRemovePart;
			if (onRemovePart != null)
			{
				onRemovePart(part);
			}
			return true;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x000511F4 File Offset: 0x0004F3F4
		private void Clear()
		{
			this.titleScreen.SetActive(false);
			this.won = false;
			this.dead = false;
			this.Score = 0;
			this.playTick = 0UL;
			this.allCoords.Clear();
			this.foods.Clear();
			for (int i = this.snake.Count - 1; i >= 0; i--)
			{
				SnakeForce.Part part = this.snake[i];
				if (part == null)
				{
					this.snake.RemoveAt(i);
				}
				else
				{
					this.RemovePart(part);
				}
			}
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00051280 File Offset: 0x0004F480
		private void PunchCamera()
		{
			this.freezeCountDown = 0.10000000149011612;
			ShortcutExtensions.DOKill(this.cameraParent, true);
			ShortcutExtensions.DOShakePosition(this.cameraParent, 0.4f, 1f, 10, 90f, false, true);
			ShortcutExtensions.DOShakeRotation(this.cameraParent, 0.4f, Vector3.forward, 10, 90f, true);
		}

		// Token: 0x04000FD9 RID: 4057
		[SerializeField]
		private GameObject gameOverScreen;

		// Token: 0x04000FDA RID: 4058
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x04000FDB RID: 4059
		[SerializeField]
		private GameObject winIndicator;

		// Token: 0x04000FDC RID: 4060
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04000FDD RID: 4061
		[SerializeField]
		private TextMeshProUGUI highScoreText;

		// Token: 0x04000FDE RID: 4062
		[SerializeField]
		private GameObject highScoreIndicator;

		// Token: 0x04000FDF RID: 4063
		[SerializeField]
		private TextMeshProUGUI scoreTextGameOver;

		// Token: 0x04000FE0 RID: 4064
		[SerializeField]
		private Transform cameraParent;

		// Token: 0x04000FE1 RID: 4065
		[SerializeField]
		private float tickIntervalFrom = 0.5f;

		// Token: 0x04000FE2 RID: 4066
		[SerializeField]
		private float tickIntervalTo = 0.01f;

		// Token: 0x04000FE3 RID: 4067
		[SerializeField]
		private int maxSpeedTick = 4096;

		// Token: 0x04000FE4 RID: 4068
		[SerializeField]
		private AnimationCurve speedCurve;

		// Token: 0x04000FE5 RID: 4069
		[SerializeField]
		private int borderXMin = -10;

		// Token: 0x04000FE6 RID: 4070
		[SerializeField]
		private int borderXMax = 10;

		// Token: 0x04000FE7 RID: 4071
		[SerializeField]
		private int borderYMin = -10;

		// Token: 0x04000FE8 RID: 4072
		[SerializeField]
		private int borderYMax = 10;

		// Token: 0x04000FE9 RID: 4073
		private bool playing;

		// Token: 0x04000FEA RID: 4074
		private bool dead;

		// Token: 0x04000FEB RID: 4075
		private bool won;

		// Token: 0x04000FEC RID: 4076
		private List<SnakeForce.Part> snake = new List<SnakeForce.Part>();

		// Token: 0x04000FED RID: 4077
		private List<Vector2Int> foods = new List<Vector2Int>();

		// Token: 0x04000FEE RID: 4078
		private int _score;

		// Token: 0x04000FEF RID: 4079
		public const string HighScoreKey = "MiniGame/Snake/HighScore";

		// Token: 0x04000FF7 RID: 4087
		private float tickETA;

		// Token: 0x04000FF8 RID: 4088
		private List<Vector2Int> allCoords = new List<Vector2Int>();

		// Token: 0x04000FF9 RID: 4089
		private ulong playTick;

		// Token: 0x04000FFA RID: 4090
		private Vector2Int lastFrameAxis;

		// Token: 0x04000FFB RID: 4091
		private double freezeCountDown;

		// Token: 0x04000FFC RID: 4092
		private bool axisInput;

		// Token: 0x02000581 RID: 1409
		public class Part
		{
			// Token: 0x06002956 RID: 10582 RVA: 0x00098C1A File Offset: 0x00096E1A
			public Part(SnakeForce master, Vector2Int coord, Vector2Int direction)
			{
				this.Master = master;
				this.coord = coord;
				this.direction = direction;
			}

			// Token: 0x17000798 RID: 1944
			// (get) Token: 0x06002957 RID: 10583 RVA: 0x00098C37 File Offset: 0x00096E37
			public bool IsHead
			{
				get
				{
					return this == this.Master.Head;
				}
			}

			// Token: 0x17000799 RID: 1945
			// (get) Token: 0x06002958 RID: 10584 RVA: 0x00098C47 File Offset: 0x00096E47
			public bool IsTail
			{
				get
				{
					return this == this.Master.Tail;
				}
			}

			// Token: 0x06002959 RID: 10585 RVA: 0x00098C57 File Offset: 0x00096E57
			internal void MoveTo(Vector2Int coord)
			{
				this.coord = coord;
				Action<SnakeForce.Part> onMove = this.OnMove;
				if (onMove == null)
				{
					return;
				}
				onMove(this);
			}

			// Token: 0x14000103 RID: 259
			// (add) Token: 0x0600295A RID: 10586 RVA: 0x00098C74 File Offset: 0x00096E74
			// (remove) Token: 0x0600295B RID: 10587 RVA: 0x00098CAC File Offset: 0x00096EAC
			public event Action<SnakeForce.Part> OnMove;

			// Token: 0x0400200C RID: 8204
			public Vector2Int coord;

			// Token: 0x0400200D RID: 8205
			public Vector2Int direction;

			// Token: 0x0400200E RID: 8206
			public readonly SnakeForce Master;
		}
	}
}
