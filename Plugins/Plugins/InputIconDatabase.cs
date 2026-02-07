using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

// Token: 0x02000003 RID: 3
[CreateAssetMenu]
public class InputIconDatabase : ScriptableObject
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	public static InputIconDatabase Instance
	{
		get
		{
			if (InputIconDatabase._instance == null)
			{
				InputIconDatabase._instance = Resources.Load<InputIconDatabase>("InputIconDatabase");
				if (InputIconDatabase._instance == null)
				{
					Debug.LogError("没找到InputIconDatabase");
				}
			}
			return InputIconDatabase._instance;
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002094 File Offset: 0x00000294
	private InputIconDatabase.Entry Find(string deviceLayoutName, string controlPath)
	{
		InputIconDatabase.DeviceGroup deviceGroup = this.groups.Find((InputIconDatabase.DeviceGroup g) => g.deviceLayoutName == deviceLayoutName);
		if (deviceGroup == null)
		{
			return null;
		}
		return deviceGroup.Find(controlPath);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020D4 File Offset: 0x000002D4
	private Sprite MGetIcon(string deviceLayoutName, string controlPath)
	{
		InputIconDatabase.Entry entry = this.Find(deviceLayoutName, controlPath);
		if (entry == null)
		{
			return null;
		}
		return entry.Value;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020F5 File Offset: 0x000002F5
	public static Sprite GetIcon(string deviceLayoutName, string controlPath)
	{
		if (InputIconDatabase.Instance == null)
		{
			return null;
		}
		return InputIconDatabase.Instance.MGetIcon(deviceLayoutName, controlPath);
	}

	// Token: 0x04000002 RID: 2
	private static InputIconDatabase _instance;

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private List<InputIconDatabase.DeviceGroup> groups = new List<InputIconDatabase.DeviceGroup>();

	// Token: 0x02000015 RID: 21
	[Serializable]
	private class DeviceGroup
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000033B8 File Offset: 0x000015B8
		public InputIconDatabase.Entry Find(string key)
		{
			return this.entries.Find((InputIconDatabase.Entry e) => e.Key == key);
		}

		// Token: 0x04000026 RID: 38
		public string deviceLayoutName;

		// Token: 0x04000027 RID: 39
		public List<InputIconDatabase.Entry> entries = new List<InputIconDatabase.Entry>();
	}

	// Token: 0x02000016 RID: 22
	[Serializable]
	private class Entry
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000033FC File Offset: 0x000015FC
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003404 File Offset: 0x00001604
		public Sprite Value
		{
			get
			{
				if (this.forceText)
				{
					return null;
				}
				if (!this.gamepadAlternatives)
				{
					return this.value;
				}
				Gamepad current = Gamepad.current;
				if (current == null)
				{
					return this.value;
				}
				string altKey = InputIconDatabase.Entry.GetGamepadAltKey(current);
				InputIconDatabase.Entry.AlternativeDeviceSprite alternativeDeviceSprite = this.alternatives.FirstOrDefault((InputIconDatabase.Entry.AlternativeDeviceSprite e) => e.deviceName == altKey);
				if (alternativeDeviceSprite.value == null)
				{
					return this.value;
				}
				return alternativeDeviceSprite.value;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000347F File Offset: 0x0000167F
		public static string GetGamepadAltKey(Gamepad gamepad)
		{
			if (gamepad is XInputController)
			{
				return "XInputController";
			}
			if (gamepad is DualShockGamepad)
			{
				return "DualShockGamepad";
			}
			if (gamepad is SwitchProControllerHID)
			{
				return "SwitchProControllerHID";
			}
			return null;
		}

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private string key;

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private Sprite value;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private bool gamepadAlternatives;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		private InputIconDatabase.Entry.AlternativeDeviceSprite[] alternatives;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private bool forceText;

		// Token: 0x0200001E RID: 30
		[Serializable]
		private struct AlternativeDeviceSprite
		{
			// Token: 0x0400003B RID: 59
			public string deviceName;

			// Token: 0x0400003C RID: 60
			public Sprite value;
		}
	}
}
