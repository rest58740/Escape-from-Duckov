using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000047 RID: 71
	[CreateAssetMenu(menuName = "Animancer/Directional Animation Set/4 Directions", order = 420)]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/DirectionalAnimationSet")]
	public class DirectionalAnimationSet : ScriptableObject, IAnimationClipSource
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000CA2A File Offset: 0x0000AC2A
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x0000CA32 File Offset: 0x0000AC32
		public AnimationClip Up
		{
			get
			{
				return this._Up;
			}
			set
			{
				this._Up = value;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0000CA3B File Offset: 0x0000AC3B
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x0000CA43 File Offset: 0x0000AC43
		public AnimationClip Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x0000CA54 File Offset: 0x0000AC54
		public AnimationClip Down
		{
			get
			{
				return this._Down;
			}
			set
			{
				this._Down = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000CA5D File Offset: 0x0000AC5D
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0000CA65 File Offset: 0x0000AC65
		public AnimationClip Left
		{
			get
			{
				return this._Left;
			}
			set
			{
				this._Left = value;
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000CA6E File Offset: 0x0000AC6E
		[Conditional("UNITY_ASSERTIONS")]
		public void AllowSetClips(bool allow = true)
		{
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000CA70 File Offset: 0x0000AC70
		[Conditional("UNITY_ASSERTIONS")]
		public void AssertCanSetClips()
		{
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000CA74 File Offset: 0x0000AC74
		public virtual AnimationClip GetClip(Vector2 direction)
		{
			if (direction.x >= 0f)
			{
				if (direction.y >= 0f)
				{
					if (direction.x <= direction.y)
					{
						return this._Up;
					}
					return this._Right;
				}
				else
				{
					if (direction.x <= -direction.y)
					{
						return this._Down;
					}
					return this._Right;
				}
			}
			else if (direction.y >= 0f)
			{
				if (direction.x >= -direction.y)
				{
					return this._Up;
				}
				return this._Left;
			}
			else
			{
				if (direction.x >= direction.y)
				{
					return this._Down;
				}
				return this._Left;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0000CB19 File Offset: 0x0000AD19
		public virtual int ClipCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		protected virtual string GetDirectionName(int direction)
		{
			DirectionalAnimationSet.Direction direction2 = (DirectionalAnimationSet.Direction)direction;
			return direction2.ToString();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000CB38 File Offset: 0x0000AD38
		public AnimationClip GetClip(DirectionalAnimationSet.Direction direction)
		{
			switch (direction)
			{
			case DirectionalAnimationSet.Direction.Up:
				return this._Up;
			case DirectionalAnimationSet.Direction.Right:
				return this._Right;
			case DirectionalAnimationSet.Direction.Down:
				return this._Down;
			case DirectionalAnimationSet.Direction.Left:
				return this._Left;
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<DirectionalAnimationSet.Direction>(direction);
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000CB74 File Offset: 0x0000AD74
		public virtual AnimationClip GetClip(int direction)
		{
			return this.GetClip((DirectionalAnimationSet.Direction)direction);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000CB7D File Offset: 0x0000AD7D
		public void SetClip(DirectionalAnimationSet.Direction direction, AnimationClip clip)
		{
			switch (direction)
			{
			case DirectionalAnimationSet.Direction.Up:
				this.Up = clip;
				return;
			case DirectionalAnimationSet.Direction.Right:
				this.Right = clip;
				return;
			case DirectionalAnimationSet.Direction.Down:
				this.Down = clip;
				return;
			case DirectionalAnimationSet.Direction.Left:
				this.Left = clip;
				return;
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<DirectionalAnimationSet.Direction>(direction);
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000CBBD File Offset: 0x0000ADBD
		public virtual void SetClip(int direction, AnimationClip clip)
		{
			this.SetClip((DirectionalAnimationSet.Direction)direction, clip);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000CBC7 File Offset: 0x0000ADC7
		public static Vector2 DirectionToVector(DirectionalAnimationSet.Direction direction)
		{
			switch (direction)
			{
			case DirectionalAnimationSet.Direction.Up:
				return Vector2.up;
			case DirectionalAnimationSet.Direction.Right:
				return Vector2.right;
			case DirectionalAnimationSet.Direction.Down:
				return Vector2.down;
			case DirectionalAnimationSet.Direction.Left:
				return Vector2.left;
			default:
				throw AnimancerUtilities.CreateUnsupportedArgumentException<DirectionalAnimationSet.Direction>(direction);
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000CBFF File Offset: 0x0000ADFF
		public virtual Vector2 GetDirection(int direction)
		{
			return DirectionalAnimationSet.DirectionToVector((DirectionalAnimationSet.Direction)direction);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000CC08 File Offset: 0x0000AE08
		public static DirectionalAnimationSet.Direction VectorToDirection(Vector2 vector)
		{
			if (vector.x >= 0f)
			{
				if (vector.y >= 0f)
				{
					if (vector.x <= vector.y)
					{
						return DirectionalAnimationSet.Direction.Up;
					}
					return DirectionalAnimationSet.Direction.Right;
				}
				else
				{
					if (vector.x <= -vector.y)
					{
						return DirectionalAnimationSet.Direction.Down;
					}
					return DirectionalAnimationSet.Direction.Right;
				}
			}
			else if (vector.y >= 0f)
			{
				if (vector.x >= -vector.y)
				{
					return DirectionalAnimationSet.Direction.Up;
				}
				return DirectionalAnimationSet.Direction.Left;
			}
			else
			{
				if (vector.x >= vector.y)
				{
					return DirectionalAnimationSet.Direction.Down;
				}
				return DirectionalAnimationSet.Direction.Left;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000CC88 File Offset: 0x0000AE88
		public static Vector2 SnapVectorToDirection(Vector2 vector)
		{
			float magnitude = vector.magnitude;
			vector = DirectionalAnimationSet.DirectionToVector(DirectionalAnimationSet.VectorToDirection(vector)) * magnitude;
			return vector;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000CCB1 File Offset: 0x0000AEB1
		public virtual Vector2 Snap(Vector2 vector)
		{
			return DirectionalAnimationSet.SnapVectorToDirection(vector);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000CCBC File Offset: 0x0000AEBC
		public void AddClips(AnimationClip[] clips, int index)
		{
			int clipCount = this.ClipCount;
			for (int i = 0; i < clipCount; i++)
			{
				clips[index + i] = this.GetClip(i);
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		public void GetAnimationClips(List<AnimationClip> clips)
		{
			int clipCount = this.ClipCount;
			for (int i = 0; i < clipCount; i++)
			{
				clips.Add(this.GetClip(i));
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000CD18 File Offset: 0x0000AF18
		public void AddDirections(Vector2[] directions, int index)
		{
			int clipCount = this.ClipCount;
			for (int i = 0; i < clipCount; i++)
			{
				directions[index + i] = this.GetDirection(i);
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000CD48 File Offset: 0x0000AF48
		public void AddClipsAndDirections(AnimationClip[] clips, Vector2[] directions, int index)
		{
			this.AddClips(clips, index);
			this.AddDirections(directions, index);
		}

		// Token: 0x040000BD RID: 189
		[SerializeField]
		private AnimationClip _Up;

		// Token: 0x040000BE RID: 190
		[SerializeField]
		private AnimationClip _Right;

		// Token: 0x040000BF RID: 191
		[SerializeField]
		private AnimationClip _Down;

		// Token: 0x040000C0 RID: 192
		[SerializeField]
		private AnimationClip _Left;

		// Token: 0x020000A5 RID: 165
		public enum Direction
		{
			// Token: 0x0400017A RID: 378
			Up,
			// Token: 0x0400017B RID: 379
			Right,
			// Token: 0x0400017C RID: 380
			Down,
			// Token: 0x0400017D RID: 381
			Left
		}
	}
}
