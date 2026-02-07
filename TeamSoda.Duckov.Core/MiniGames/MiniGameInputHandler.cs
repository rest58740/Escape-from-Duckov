using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.MiniGames
{
	// Token: 0x02000293 RID: 659
	public class MiniGameInputHandler : MonoBehaviour
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x0004F784 File Offset: 0x0004D984
		private void Awake()
		{
			InputActionAsset actions = GameManager.MainPlayerInput.actions;
			this.inputActionMove = actions["MoveAxis"];
			this.inputActionButtonA = actions["MiniGameA"];
			this.inputActionButtonB = actions["MiniGameB"];
			this.inputActionSelect = actions["MiniGameSelect"];
			this.inputActionStart = actions["MiniGameStart"];
			this.inputActionMouseDelta = actions["MouseDelta"];
			this.inputActionButtonA.actionMap.Enable();
			this.Bind(this.inputActionMove, new Action<InputAction.CallbackContext>(this.OnMove));
			this.Bind(this.inputActionButtonA, new Action<InputAction.CallbackContext>(this.OnButtonA));
			this.Bind(this.inputActionButtonB, new Action<InputAction.CallbackContext>(this.OnButtonB));
			this.Bind(this.inputActionSelect, new Action<InputAction.CallbackContext>(this.OnSelect));
			this.Bind(this.inputActionStart, new Action<InputAction.CallbackContext>(this.OnStart));
			this.Bind(this.inputActionMouseDelta, new Action<InputAction.CallbackContext>(this.OnMouseDelta));
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x0004F8A2 File Offset: 0x0004DAA2
		private void OnMouseDelta(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.game.SetInputAxis(context.ReadValue<Vector2>(), 1);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x0004F8CF File Offset: 0x0004DACF
		public void ClearInput()
		{
			MiniGame miniGame = this.game;
			if (miniGame == null)
			{
				return;
			}
			miniGame.ClearInput();
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0004F8E1 File Offset: 0x0004DAE1
		private void OnDisable()
		{
			this.ClearInput();
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0004F8E9 File Offset: 0x0004DAE9
		private void SetGameButtonByContext(MiniGame.Button button, InputAction.CallbackContext context)
		{
			if (context.started)
			{
				this.game.SetButton(button, true);
				return;
			}
			if (context.canceled)
			{
				this.game.SetButton(button, false);
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x0004F918 File Offset: 0x0004DB18
		private void OnStart(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.Start, context);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0004F93A File Offset: 0x0004DB3A
		private void OnSelect(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.Select, context);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0004F95C File Offset: 0x0004DB5C
		private void OnButtonB(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.B, context);
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0004F97E File Offset: 0x0004DB7E
		private void OnButtonA(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.A, context);
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0004F9A0 File Offset: 0x0004DBA0
		private void OnMove(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.game.SetInputAxis(context.ReadValue<Vector2>(), 0);
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0004F9D0 File Offset: 0x0004DBD0
		private void OnDestroy()
		{
			foreach (Action action in this.unbindCommands)
			{
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0004FA28 File Offset: 0x0004DC28
		private void Bind(InputAction inputAction, Action<InputAction.CallbackContext> action)
		{
			inputAction.Enable();
			inputAction.started += action;
			inputAction.performed += action;
			inputAction.canceled += action;
			this.unbindCommands.Add(delegate
			{
				inputAction.started -= action;
				inputAction.performed -= action;
				inputAction.canceled -= action;
			});
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004FA9E File Offset: 0x0004DC9E
		internal void SetGame(MiniGame game)
		{
			this.game = game;
		}

		// Token: 0x04000FA0 RID: 4000
		[SerializeField]
		private MiniGame game;

		// Token: 0x04000FA1 RID: 4001
		private InputAction inputActionMove;

		// Token: 0x04000FA2 RID: 4002
		private InputAction inputActionButtonA;

		// Token: 0x04000FA3 RID: 4003
		private InputAction inputActionButtonB;

		// Token: 0x04000FA4 RID: 4004
		private InputAction inputActionSelect;

		// Token: 0x04000FA5 RID: 4005
		private InputAction inputActionStart;

		// Token: 0x04000FA6 RID: 4006
		private InputAction inputActionMouseDelta;

		// Token: 0x04000FA7 RID: 4007
		private List<Action> unbindCommands = new List<Action>();
	}
}
