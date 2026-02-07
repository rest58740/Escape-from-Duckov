using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000291 RID: 657
	public class MiniGame : MonoBehaviour
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x0004F1DD File Offset: 0x0004D3DD
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0004F1E5 File Offset: 0x0004D3E5
		public Camera Camera
		{
			get
			{
				return this.camera;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0004F1ED File Offset: 0x0004D3ED
		public Camera UICamera
		{
			get
			{
				return this.uiCamera;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x0004F1F5 File Offset: 0x0004D3F5
		public RenderTexture RenderTexture
		{
			get
			{
				return this.renderTexture;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0004F1FD File Offset: 0x0004D3FD
		public GamingConsole Console
		{
			get
			{
				return this.console;
			}
		}

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06001537 RID: 5431 RVA: 0x0004F208 File Offset: 0x0004D408
		// (remove) Token: 0x06001538 RID: 5432 RVA: 0x0004F23C File Offset: 0x0004D43C
		public static event Action<MiniGame, MiniGame.MiniGameInputEventContext> OnInput;

		// Token: 0x06001539 RID: 5433 RVA: 0x0004F26F File Offset: 0x0004D46F
		public void SetRenderTexture(RenderTexture texture)
		{
			this.camera.targetTexture = texture;
			if (this.uiCamera)
			{
				this.uiCamera.targetTexture = texture;
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0004F298 File Offset: 0x0004D498
		public RenderTexture CreateAndSetRenderTexture(int width, int height)
		{
			RenderTexture result = new RenderTexture(width, height, 32);
			this.SetRenderTexture(result);
			return result;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0004F2B7 File Offset: 0x0004D4B7
		private void Awake()
		{
			if (this.renderTexture != null)
			{
				this.SetRenderTexture(this.renderTexture);
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0004F2D4 File Offset: 0x0004D4D4
		public void SetInputAxis(Vector2 axis, int index = 0)
		{
			Vector2 vector = this.inputAxis_0;
			if (index == 0)
			{
				this.inputAxis_0 = axis;
			}
			if (index == 1)
			{
				this.inputAxis_1 = axis;
			}
			if (index == 0)
			{
				bool flag = axis.x < -0.1f;
				bool flag2 = axis.x > 0.1f;
				bool flag3 = axis.y > 0.1f;
				bool flag4 = axis.y < -0.1f;
				bool flag5 = vector.x < -0.1f;
				bool flag6 = vector.x > 0.1f;
				bool flag7 = vector.y > 0.1f;
				bool flag8 = vector.y < -0.1f;
				if (flag != flag5)
				{
					this.SetButton(MiniGame.Button.Left, flag);
				}
				if (flag2 != flag6)
				{
					this.SetButton(MiniGame.Button.Right, flag2);
				}
				if (flag3 != flag7)
				{
					this.SetButton(MiniGame.Button.Up, flag3);
				}
				if (flag4 != flag8)
				{
					this.SetButton(MiniGame.Button.Down, flag4);
				}
			}
			Action<MiniGame, MiniGame.MiniGameInputEventContext> onInput = MiniGame.OnInput;
			if (onInput == null)
			{
				return;
			}
			onInput(this, new MiniGame.MiniGameInputEventContext
			{
				isAxisEvent = true,
				axisIndex = index,
				axisValue = axis
			});
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0004F3E0 File Offset: 0x0004D5E0
		public void SetButton(MiniGame.Button button, bool down)
		{
			MiniGame.ButtonStatus buttonStatus;
			if (!this.buttons.TryGetValue(button, out buttonStatus))
			{
				buttonStatus = new MiniGame.ButtonStatus();
				this.buttons[button] = buttonStatus;
			}
			if (down)
			{
				buttonStatus.justPressed = true;
				buttonStatus.pressed = true;
			}
			else
			{
				buttonStatus.pressed = false;
				buttonStatus.justReleased = true;
			}
			this.buttons[button] = buttonStatus;
			Action<MiniGame, MiniGame.MiniGameInputEventContext> onInput = MiniGame.OnInput;
			if (onInput == null)
			{
				return;
			}
			onInput(this, new MiniGame.MiniGameInputEventContext
			{
				isButtonEvent = true,
				button = button,
				pressing = buttonStatus.pressed,
				buttonDown = buttonStatus.justPressed,
				buttonUp = buttonStatus.justReleased
			});
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0004F490 File Offset: 0x0004D690
		public bool GetButton(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.pressed;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0004F4B8 File Offset: 0x0004D6B8
		public bool GetButtonDown(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.justPressed;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0004F4E0 File Offset: 0x0004D6E0
		public bool GetButtonUp(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.justReleased;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0004F508 File Offset: 0x0004D708
		public Vector2 GetAxis(int index = 0)
		{
			if (index == 0)
			{
				return this.inputAxis_0;
			}
			if (index == 1)
			{
				return this.inputAxis_1;
			}
			return default(Vector2);
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0004F533 File Offset: 0x0004D733
		private void Tick(float deltaTime)
		{
			this.UpdateLogic(deltaTime);
			this.Cleanup();
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0004F542 File Offset: 0x0004D742
		private void UpdateLogic(float deltaTime)
		{
			Action<MiniGame, float> action = MiniGame.onUpdateLogic;
			if (action == null)
			{
				return;
			}
			action(this, deltaTime);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0004F558 File Offset: 0x0004D758
		private void Cleanup()
		{
			foreach (MiniGame.ButtonStatus buttonStatus in this.buttons.Values)
			{
				buttonStatus.justPressed = false;
				buttonStatus.justReleased = false;
			}
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0004F5B8 File Offset: 0x0004D7B8
		private void Update()
		{
			if (this.tickTiming == MiniGame.TickTiming.Update)
			{
				this.Tick(Time.deltaTime);
			}
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0004F5CE File Offset: 0x0004D7CE
		private void FixedUpdate()
		{
			if (this.tickTiming == MiniGame.TickTiming.FixedUpdate)
			{
				this.Tick(Time.fixedDeltaTime);
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0004F5E4 File Offset: 0x0004D7E4
		private void LateUpdate()
		{
			if (this.tickTiming == MiniGame.TickTiming.FixedUpdate)
			{
				this.Tick(Time.deltaTime);
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0004F5FC File Offset: 0x0004D7FC
		public void ClearInput()
		{
			foreach (MiniGame.ButtonStatus buttonStatus in this.buttons.Values)
			{
				if (buttonStatus.pressed)
				{
					buttonStatus.justReleased = true;
				}
				buttonStatus.pressed = false;
			}
			this.SetInputAxis(default(Vector2), 0);
			this.SetInputAxis(default(Vector2), 1);
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0004F684 File Offset: 0x0004D884
		internal void SetConsole(GamingConsole console)
		{
			this.console = console;
		}

		// Token: 0x04000F94 RID: 3988
		[SerializeField]
		private string id;

		// Token: 0x04000F95 RID: 3989
		public MiniGame.TickTiming tickTiming;

		// Token: 0x04000F96 RID: 3990
		[SerializeField]
		private Camera camera;

		// Token: 0x04000F97 RID: 3991
		[SerializeField]
		private Camera uiCamera;

		// Token: 0x04000F98 RID: 3992
		[SerializeField]
		private RenderTexture renderTexture;

		// Token: 0x04000F99 RID: 3993
		public static Action<MiniGame, float> onUpdateLogic;

		// Token: 0x04000F9A RID: 3994
		private GamingConsole console;

		// Token: 0x04000F9B RID: 3995
		private Vector2 inputAxis_0;

		// Token: 0x04000F9C RID: 3996
		private Vector2 inputAxis_1;

		// Token: 0x04000F9D RID: 3997
		private Dictionary<MiniGame.Button, MiniGame.ButtonStatus> buttons = new Dictionary<MiniGame.Button, MiniGame.ButtonStatus>();

		// Token: 0x02000579 RID: 1401
		public enum TickTiming
		{
			// Token: 0x04001FDA RID: 8154
			Manual,
			// Token: 0x04001FDB RID: 8155
			Update,
			// Token: 0x04001FDC RID: 8156
			FixedUpdate,
			// Token: 0x04001FDD RID: 8157
			LateUpdate
		}

		// Token: 0x0200057A RID: 1402
		public enum Button
		{
			// Token: 0x04001FDF RID: 8159
			None,
			// Token: 0x04001FE0 RID: 8160
			A,
			// Token: 0x04001FE1 RID: 8161
			B,
			// Token: 0x04001FE2 RID: 8162
			Start,
			// Token: 0x04001FE3 RID: 8163
			Select,
			// Token: 0x04001FE4 RID: 8164
			Left,
			// Token: 0x04001FE5 RID: 8165
			Right,
			// Token: 0x04001FE6 RID: 8166
			Up,
			// Token: 0x04001FE7 RID: 8167
			Down
		}

		// Token: 0x0200057B RID: 1403
		public class ButtonStatus
		{
			// Token: 0x04001FE8 RID: 8168
			public bool pressed;

			// Token: 0x04001FE9 RID: 8169
			public bool justPressed;

			// Token: 0x04001FEA RID: 8170
			public bool justReleased;
		}

		// Token: 0x0200057C RID: 1404
		public struct MiniGameInputEventContext
		{
			// Token: 0x04001FEB RID: 8171
			public bool isButtonEvent;

			// Token: 0x04001FEC RID: 8172
			public MiniGame.Button button;

			// Token: 0x04001FED RID: 8173
			public bool pressing;

			// Token: 0x04001FEE RID: 8174
			public bool buttonDown;

			// Token: 0x04001FEF RID: 8175
			public bool buttonUp;

			// Token: 0x04001FF0 RID: 8176
			public bool isAxisEvent;

			// Token: 0x04001FF1 RID: 8177
			public int axisIndex;

			// Token: 0x04001FF2 RID: 8178
			public Vector2 axisValue;
		}
	}
}
