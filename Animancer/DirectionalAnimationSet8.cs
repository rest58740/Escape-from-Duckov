using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000048 RID: 72
	[CreateAssetMenu(menuName = "Animancer/Directional Animation Set/8 Directions", order = 421)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/DirectionalAnimationSet8")]
	public class DirectionalAnimationSet8 : DirectionalAnimationSet
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000CD62 File Offset: 0x0000AF62
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x0000CD6A File Offset: 0x0000AF6A
		public AnimationClip UpRight
		{
			get
			{
				return this._UpRight;
			}
			set
			{
				this._UpRight = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0000CD73 File Offset: 0x0000AF73
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x0000CD7B File Offset: 0x0000AF7B
		public AnimationClip DownRight
		{
			get
			{
				return this._DownRight;
			}
			set
			{
				this._DownRight = value;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000CD84 File Offset: 0x0000AF84
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x0000CD8C File Offset: 0x0000AF8C
		public AnimationClip DownLeft
		{
			get
			{
				return this._DownLeft;
			}
			set
			{
				this._DownLeft = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0000CD95 File Offset: 0x0000AF95
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x0000CD9D File Offset: 0x0000AF9D
		public AnimationClip UpLeft
		{
			get
			{
				return this._UpLeft;
			}
			set
			{
				this._UpLeft = value;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		public override AnimationClip GetClip(Vector2 direction)
		{
			float num = Mathf.Atan2(direction.y, direction.x);
			switch (Mathf.RoundToInt(8f * num / 6.2831855f + 8f) % 8)
			{
			case 0:
				return base.Right;
			case 1:
				return this._UpRight;
			case 2:
				return base.Up;
			case 3:
				return this._UpLeft;
			case 4:
				return base.Left;
			case 5:
				return this._DownLeft;
			case 6:
				return base.Down;
			case 7:
				return this._DownRight;
			default:
				throw new ArgumentOutOfRangeException("Invalid octant");
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000CE4C File Offset: 0x0000B04C
		public override int ClipCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000CE50 File Offset: 0x0000B050
		protected override string GetDirectionName(int direction)
		{
			DirectionalAnimationSet8.Direction direction2 = (DirectionalAnimationSet8.Direction)direction;
			return direction2.ToString();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public AnimationClip GetClip(DirectionalAnimationSet8.Direction direction)
		{
			switch (direction)
			{
			case DirectionalAnimationSet8.Direction.Up:
				return base.Up;
			case DirectionalAnimationSet8.Direction.Right:
				return base.Right;
			case DirectionalAnimationSet8.Direction.Down:
				return base.Down;
			case DirectionalAnimationSet8.Direction.Left:
				return base.Left;
			case DirectionalAnimationSet8.Direction.UpRight:
				return this._UpRight;
			case DirectionalAnimationSet8.Direction.DownRight:
				return this._DownRight;
			case DirectionalAnimationSet8.Direction.DownLeft:
				return this._DownLeft;
			case DirectionalAnimationSet8.Direction.UpLeft:
				return this._UpLeft;
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<DirectionalAnimationSet8.Direction>(direction);
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000CEDF File Offset: 0x0000B0DF
		public override AnimationClip GetClip(int direction)
		{
			return this.GetClip((DirectionalAnimationSet8.Direction)direction);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
		public void SetClip(DirectionalAnimationSet8.Direction direction, AnimationClip clip)
		{
			switch (direction)
			{
			case DirectionalAnimationSet8.Direction.Up:
				base.Up = clip;
				return;
			case DirectionalAnimationSet8.Direction.Right:
				base.Right = clip;
				return;
			case DirectionalAnimationSet8.Direction.Down:
				base.Down = clip;
				return;
			case DirectionalAnimationSet8.Direction.Left:
				base.Left = clip;
				return;
			case DirectionalAnimationSet8.Direction.UpRight:
				this.UpRight = clip;
				return;
			case DirectionalAnimationSet8.Direction.DownRight:
				this.DownRight = clip;
				return;
			case DirectionalAnimationSet8.Direction.DownLeft:
				this.DownLeft = clip;
				return;
			case DirectionalAnimationSet8.Direction.UpLeft:
				this.UpLeft = clip;
				return;
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<DirectionalAnimationSet8.Direction>(direction);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000CF63 File Offset: 0x0000B163
		public override void SetClip(int direction, AnimationClip clip)
		{
			this.SetClip((DirectionalAnimationSet8.Direction)direction, clip);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000CF70 File Offset: 0x0000B170
		public static Vector2 DirectionToVector(DirectionalAnimationSet8.Direction direction)
		{
			switch (direction)
			{
			case DirectionalAnimationSet8.Direction.Up:
				return Vector2.up;
			case DirectionalAnimationSet8.Direction.Right:
				return Vector2.right;
			case DirectionalAnimationSet8.Direction.Down:
				return Vector2.down;
			case DirectionalAnimationSet8.Direction.Left:
				return Vector2.left;
			case DirectionalAnimationSet8.Direction.UpRight:
				return DirectionalAnimationSet8.Diagonals.UpRight;
			case DirectionalAnimationSet8.Direction.DownRight:
				return DirectionalAnimationSet8.Diagonals.DownRight;
			case DirectionalAnimationSet8.Direction.DownLeft:
				return DirectionalAnimationSet8.Diagonals.DownLeft;
			case DirectionalAnimationSet8.Direction.UpLeft:
				return DirectionalAnimationSet8.Diagonals.UpLeft;
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<DirectionalAnimationSet8.Direction>(direction);
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000CFDB File Offset: 0x0000B1DB
		public override Vector2 GetDirection(int direction)
		{
			return DirectionalAnimationSet8.DirectionToVector((DirectionalAnimationSet8.Direction)direction);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		public new static DirectionalAnimationSet8.Direction VectorToDirection(Vector2 vector)
		{
			float num = Mathf.Atan2(vector.y, vector.x);
			switch (Mathf.RoundToInt(8f * num / 6.2831855f + 8f) % 8)
			{
			case 0:
				return DirectionalAnimationSet8.Direction.Right;
			case 1:
				return DirectionalAnimationSet8.Direction.UpRight;
			case 2:
				return DirectionalAnimationSet8.Direction.Up;
			case 3:
				return DirectionalAnimationSet8.Direction.UpLeft;
			case 4:
				return DirectionalAnimationSet8.Direction.Left;
			case 5:
				return DirectionalAnimationSet8.Direction.DownLeft;
			case 6:
				return DirectionalAnimationSet8.Direction.Down;
			case 7:
				return DirectionalAnimationSet8.Direction.DownRight;
			default:
				throw new ArgumentOutOfRangeException("Invalid octant");
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000D060 File Offset: 0x0000B260
		public new static Vector2 SnapVectorToDirection(Vector2 vector)
		{
			float magnitude = vector.magnitude;
			vector = DirectionalAnimationSet8.DirectionToVector(DirectionalAnimationSet8.VectorToDirection(vector)) * magnitude;
			return vector;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000D089 File Offset: 0x0000B289
		public override Vector2 Snap(Vector2 vector)
		{
			return DirectionalAnimationSet8.SnapVectorToDirection(vector);
		}

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		private AnimationClip _UpRight;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private AnimationClip _DownRight;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private AnimationClip _DownLeft;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private AnimationClip _UpLeft;

		// Token: 0x020000A6 RID: 166
		public static class Diagonals
		{
			// Token: 0x17000191 RID: 401
			// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000124CA File Offset: 0x000106CA
			public static Vector2 UpRight
			{
				get
				{
					return new Vector2(0.70710677f, 0.70710677f);
				}
			}

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000124DB File Offset: 0x000106DB
			public static Vector2 DownRight
			{
				get
				{
					return new Vector2(0.70710677f, -0.70710677f);
				}
			}

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000124EC File Offset: 0x000106EC
			public static Vector2 DownLeft
			{
				get
				{
					return new Vector2(-0.70710677f, -0.70710677f);
				}
			}

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000124FD File Offset: 0x000106FD
			public static Vector2 UpLeft
			{
				get
				{
					return new Vector2(-0.70710677f, 0.70710677f);
				}
			}

			// Token: 0x0400017E RID: 382
			public const float OneOverSqrt2 = 0.70710677f;
		}

		// Token: 0x020000A7 RID: 167
		public new enum Direction
		{
			// Token: 0x04000180 RID: 384
			Up,
			// Token: 0x04000181 RID: 385
			Right,
			// Token: 0x04000182 RID: 386
			Down,
			// Token: 0x04000183 RID: 387
			Left,
			// Token: 0x04000184 RID: 388
			UpRight,
			// Token: 0x04000185 RID: 389
			DownRight,
			// Token: 0x04000186 RID: 390
			DownLeft,
			// Token: 0x04000187 RID: 391
			UpLeft
		}
	}
}
