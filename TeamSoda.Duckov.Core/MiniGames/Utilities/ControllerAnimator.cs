using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x02000294 RID: 660
	public class ControllerAnimator : MonoBehaviour
	{
		// Token: 0x06001562 RID: 5474 RVA: 0x0004FABA File Offset: 0x0004DCBA
		private void OnEnable()
		{
			MiniGame.OnInput += this.OnMiniGameInput;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0004FACD File Offset: 0x0004DCCD
		private void OnDisable()
		{
			MiniGame.OnInput -= this.OnMiniGameInput;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0004FAE0 File Offset: 0x0004DCE0
		private void OnMiniGameInput(MiniGame game, MiniGame.MiniGameInputEventContext context)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.Game != game)
			{
				return;
			}
			this.HandleInput(context);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0004FB0C File Offset: 0x0004DD0C
		private void HandleInput(MiniGame.MiniGameInputEventContext context)
		{
			if (context.isButtonEvent)
			{
				this.HandleButtonEvent(context);
				return;
			}
			if (context.isAxisEvent)
			{
				this.HandleAxisEvent(context);
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0004FB2D File Offset: 0x0004DD2D
		private void HandleAxisEvent(MiniGame.MiniGameInputEventContext context)
		{
			if (context.axisIndex != 0)
			{
				return;
			}
			this.SetAxis(context.axisValue);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0004FB44 File Offset: 0x0004DD44
		private void HandleButtonEvent(MiniGame.MiniGameInputEventContext context)
		{
			switch (context.button)
			{
			case MiniGame.Button.A:
				this.HandleBtnPushRest(this.btn_A, context.pressing);
				break;
			case MiniGame.Button.B:
				this.HandleBtnPushRest(this.btn_B, context.pressing);
				break;
			case MiniGame.Button.Start:
				this.HandleBtnPushRest(this.btn_Start, context.pressing);
				break;
			case MiniGame.Button.Select:
				this.HandleBtnPushRest(this.btn_Select, context.pressing);
				break;
			case MiniGame.Button.Left:
			case MiniGame.Button.Right:
			case MiniGame.Button.Up:
			case MiniGame.Button.Down:
				this.PlayAxisPressReleaseFX(context.button, context.pressing);
				break;
			}
			if (context.pressing)
			{
				switch (context.button)
				{
				case MiniGame.Button.None:
					break;
				case MiniGame.Button.A:
					this.ApplyTorque(1f, -0.5f);
					return;
				case MiniGame.Button.B:
					this.ApplyTorque(1f, --0f);
					return;
				case MiniGame.Button.Start:
					this.ApplyTorque(0.5f, -0.5f);
					return;
				case MiniGame.Button.Select:
					this.ApplyTorque(-0.5f, -0.5f);
					return;
				case MiniGame.Button.Left:
					this.ApplyTorque(-1f, 0f);
					return;
				case MiniGame.Button.Right:
					this.ApplyTorque(-0.5f, 0f);
					return;
				case MiniGame.Button.Up:
					this.ApplyTorque(-1f, 0.5f);
					return;
				case MiniGame.Button.Down:
					this.ApplyTorque(-1f, -0.5f);
					return;
				default:
					return;
				}
			}
			else
			{
				this.ApplyTorque(Random.insideUnitCircle * 0.25f);
			}
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0004FCC0 File Offset: 0x0004DEC0
		private void PlayAxisPressReleaseFX(MiniGame.Button button, bool pressing)
		{
			Transform transform = null;
			switch (button)
			{
			case MiniGame.Button.Left:
				transform = this.fxPos_Left;
				break;
			case MiniGame.Button.Right:
				transform = this.fxPos_Right;
				break;
			case MiniGame.Button.Up:
				transform = this.fxPos_Up;
				break;
			case MiniGame.Button.Down:
				transform = this.fxPos_Down;
				break;
			}
			if (transform == null)
			{
				return;
			}
			if (pressing)
			{
				FXPool.Play(this.buttonPressFX, transform.position, transform.rotation);
				return;
			}
			FXPool.Play(this.buttonRestFX, transform.position, transform.rotation);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0004FD4C File Offset: 0x0004DF4C
		private void ApplyTorque(float x, float y)
		{
			if (this.mainTransform == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(this.mainTransform, false);
			Vector3 vector = new Vector3(-y, -x, 0f) * this.torqueStrength;
			this.mainTransform.localRotation = Quaternion.identity;
			ShortcutExtensions.DOPunchRotation(this.mainTransform, vector, this.torqueDuration, this.torqueVibrato, this.torqueElasticity);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0004FDBE File Offset: 0x0004DFBE
		private void ApplyTorque(Vector2 torque)
		{
			this.ApplyTorque(torque.x, torque.y);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0004FDD2 File Offset: 0x0004DFD2
		private void HandleBtnPushRest(Transform btnTrans, bool pressed)
		{
			if (pressed)
			{
				this.Push(btnTrans);
				return;
			}
			this.Rest(btnTrans);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0004FDE6 File Offset: 0x0004DFE6
		internal void SetConsole(GamingConsole master)
		{
			this.master = master;
			this.RefreshAll();
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0004FDF8 File Offset: 0x0004DFF8
		private void RefreshAll()
		{
			this.RestAll();
			if (this.master == null)
			{
				return;
			}
			MiniGame game = this.master.Game;
			if (game == null)
			{
				return;
			}
			if (game.GetButton(MiniGame.Button.A))
			{
				this.Push(this.btn_A);
			}
			if (game.GetButton(MiniGame.Button.B))
			{
				this.Push(this.btn_B);
			}
			if (game.GetButton(MiniGame.Button.Select))
			{
				this.Push(this.btn_Select);
			}
			if (game.GetButton(MiniGame.Button.Start))
			{
				this.Push(this.btn_Start);
			}
			this.SetAxis(game.GetAxis(0));
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0004FE94 File Offset: 0x0004E094
		private void RestAll()
		{
			this.Rest(this.btn_A);
			this.Rest(this.btn_B);
			this.Rest(this.btn_Start);
			this.Rest(this.btn_Select);
			this.Rest(this.btn_Axis);
			this.SetAxis(Vector2.zero);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0004FEE8 File Offset: 0x0004E0E8
		private void SetAxis(Vector2 axis)
		{
			if (this.btn_Axis == null)
			{
				return;
			}
			axis = axis.normalized;
			Vector3 vector = new Vector3(0f, -axis.x * this.axisAmp, axis.y * this.axisAmp);
			Quaternion localRotation = this.btn_Axis.localRotation;
			Quaternion quaternion = Quaternion.Euler(vector);
			quaternion * Quaternion.Inverse(localRotation);
			this.btn_Axis.localRotation = quaternion;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0004FF60 File Offset: 0x0004E160
		private void Push(Transform btnTransform)
		{
			if (btnTransform == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(btnTransform, false);
			TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveX(btnTransform, -this.btnDepth, this.transitionDuration, false), 24);
			if (this.buttonPressFX)
			{
				FXPool.Play(this.buttonPressFX, btnTransform.position, btnTransform.rotation);
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0004FFC0 File Offset: 0x0004E1C0
		private void Rest(Transform btnTransform)
		{
			if (btnTransform == null)
			{
				return;
			}
			ShortcutExtensions.DOKill(btnTransform, false);
			TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveX(btnTransform, 0f, this.transitionDuration, false), 24);
			if (this.buttonRestFX)
			{
				FXPool.Play(this.buttonRestFX, btnTransform.position, btnTransform.rotation);
			}
		}

		// Token: 0x04000FA8 RID: 4008
		private GamingConsole master;

		// Token: 0x04000FA9 RID: 4009
		public Transform mainTransform;

		// Token: 0x04000FAA RID: 4010
		public Transform btn_A;

		// Token: 0x04000FAB RID: 4011
		public Transform btn_B;

		// Token: 0x04000FAC RID: 4012
		public Transform btn_Start;

		// Token: 0x04000FAD RID: 4013
		public Transform btn_Select;

		// Token: 0x04000FAE RID: 4014
		public Transform btn_Axis;

		// Token: 0x04000FAF RID: 4015
		public Transform fxPos_Up;

		// Token: 0x04000FB0 RID: 4016
		public Transform fxPos_Right;

		// Token: 0x04000FB1 RID: 4017
		public Transform fxPos_Down;

		// Token: 0x04000FB2 RID: 4018
		public Transform fxPos_Left;

		// Token: 0x04000FB3 RID: 4019
		[SerializeField]
		private float transitionDuration = 0.2f;

		// Token: 0x04000FB4 RID: 4020
		[SerializeField]
		private float axisAmp = 10f;

		// Token: 0x04000FB5 RID: 4021
		[SerializeField]
		private float btnDepth = 0.003f;

		// Token: 0x04000FB6 RID: 4022
		[SerializeField]
		private float torqueStrength = 5f;

		// Token: 0x04000FB7 RID: 4023
		[SerializeField]
		private float torqueDuration = 0.5f;

		// Token: 0x04000FB8 RID: 4024
		[SerializeField]
		private int torqueVibrato = 1;

		// Token: 0x04000FB9 RID: 4025
		[SerializeField]
		private float torqueElasticity = 1f;

		// Token: 0x04000FBA RID: 4026
		[SerializeField]
		private ParticleSystem buttonPressFX;

		// Token: 0x04000FBB RID: 4027
		[SerializeField]
		private ParticleSystem buttonRestFX;
	}
}
