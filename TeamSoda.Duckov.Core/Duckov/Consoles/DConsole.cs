using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.Consoles
{
	// Token: 0x0200031A RID: 794
	public class DConsole : MonoBehaviour
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0005F3A3 File Offset: 0x0005D5A3
		// (set) Token: 0x06001A0F RID: 6671 RVA: 0x0005F3AA File Offset: 0x0005D5AA
		public static DConsole Instance { get; private set; }

		// Token: 0x06001A10 RID: 6672 RVA: 0x0005F3B4 File Offset: 0x0005D5B4
		private void Awake()
		{
			if (DConsole.Instance != null)
			{
				Debug.LogError("Multiple DConsole instances found in scene.");
			}
			DConsole.Instance = this;
			foreach (DCommand command in this.defaultCommands)
			{
				DConsole.RegisterCommand(command);
			}
			TMP_InputField tmp_InputField = this.inputField;
			tmp_InputField.onValidateInput = (TMP_InputField.OnValidateInput)Delegate.Combine(tmp_InputField.onValidateInput, new TMP_InputField.OnValidateInput(this.OnValidateInput));
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005F448 File Offset: 0x0005D648
		private char OnValidateInput(string text, int charIndex, char addedChar)
		{
			if (addedChar == '`')
			{
				this.HideConsole();
			}
			return addedChar;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0005F458 File Offset: 0x0005D658
		private void Update()
		{
			if (!CheatMode.Active)
			{
				return;
			}
			this.frameCounter++;
			if (Input.GetKeyDown(96))
			{
				if (this.uiFadeGroup.IsShown)
				{
					this.HideConsole();
				}
				else if (this.frameCounter > 1)
				{
					this.ShowConsole();
				}
			}
			if (this.uiFadeGroup.IsShown && Input.GetKeyDown(13))
			{
				string text = this.inputField.text;
				this.inputField.text = string.Empty;
				this.Process(text);
				this.uiFadeGroup.Hide();
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0005F4EC File Offset: 0x0005D6EC
		private void ShowConsole()
		{
			this.uiFadeGroup.Show();
			this.inputField.text = string.Empty;
			this.inputField.ActivateInputField();
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0005F514 File Offset: 0x0005D714
		private void HideConsole()
		{
			this.inputField.text = string.Empty;
			this.uiFadeGroup.Hide();
			this.frameCounter = 0;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0005F538 File Offset: 0x0005D738
		public static void RegisterCommand(IDCommand command)
		{
			if (string.IsNullOrWhiteSpace(command.CommandWord))
			{
				Debug.LogError("Empty command word.");
				return;
			}
			if (DConsole.commandDic.ContainsKey(command.CommandWord))
			{
				Debug.LogError("Duplicate command: " + command.CommandWord);
			}
			DConsole.commandDic[command.CommandWord] = command;
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x0005F598 File Offset: 0x0005D798
		public void Process(string command)
		{
			if (string.IsNullOrWhiteSpace(command))
			{
				return;
			}
			command = command.Trim();
			string[] array = command.Split(" ", StringSplitOptions.None);
			string text = array[0];
			IDCommand idcommand;
			if (!this.TryGetCommand(text, out idcommand))
			{
				this.EchoWarning("Invalid Command: " + text);
				return;
			}
			string[] array2 = new string[array.Length - 1];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = array[i + 1];
			}
			string result = idcommand.Execute(this, array2);
			this.Echo(result);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0005F61D File Offset: 0x0005D81D
		public void Echo(string result)
		{
			Debug.Log(result);
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0005F625 File Offset: 0x0005D825
		public void EchoWarning(string message)
		{
			Debug.LogWarning(message);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x0005F630 File Offset: 0x0005D830
		private bool TryGetCommand(string commandWord, out IDCommand dCommand)
		{
			commandWord = commandWord.Trim();
			IDCommand idcommand;
			if (!DConsole.commandDic.TryGetValue(commandWord, out idcommand))
			{
				dCommand = idcommand;
				return false;
			}
			dCommand = idcommand;
			return true;
		}

		// Token: 0x040012EE RID: 4846
		[SerializeField]
		private FadeGroup uiFadeGroup;

		// Token: 0x040012EF RID: 4847
		[SerializeField]
		private TMP_InputField inputField;

		// Token: 0x040012F0 RID: 4848
		public List<DCommand> defaultCommands = new List<DCommand>();

		// Token: 0x040012F1 RID: 4849
		private int frameCounter;

		// Token: 0x040012F2 RID: 4850
		public static Dictionary<string, IDCommand> commandDic = new Dictionary<string, IDCommand>();
	}
}
