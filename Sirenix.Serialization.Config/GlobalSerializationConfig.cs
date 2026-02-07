using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000006 RID: 6
	[HideMonoScript]
	[SirenixGlobalConfig]
	public class GlobalSerializationConfig : GlobalConfig<GlobalSerializationConfig>
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020CC File Offset: 0x000002CC
		public ILogger Logger
		{
			get
			{
				return DefaultLoggers.UnityLogger;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002164 File Offset: 0x00000364
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000216C File Offset: 0x0000036C
		public DataFormat EditorSerializationFormat
		{
			get
			{
				return this.editorSerializationFormat;
			}
			set
			{
				this.editorSerializationFormat = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002175 File Offset: 0x00000375
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000217D File Offset: 0x0000037D
		public DataFormat BuildSerializationFormat
		{
			get
			{
				return this.buildSerializationFormat;
			}
			set
			{
				this.buildSerializationFormat = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002186 File Offset: 0x00000386
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000218E File Offset: 0x0000038E
		public LoggingPolicy LoggingPolicy
		{
			get
			{
				return this.loggingPolicy;
			}
			set
			{
				this.loggingPolicy = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002197 File Offset: 0x00000397
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000219F File Offset: 0x0000039F
		public ErrorHandlingPolicy ErrorHandlingPolicy
		{
			get
			{
				return this.errorHandlingPolicy;
			}
			set
			{
				this.errorHandlingPolicy = value;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021A8 File Offset: 0x000003A8
		[OnInspectorGUI]
		private void OnInspectorGUI()
		{
			GUIStyle style = new GUIStyle(GUI.skin.label)
			{
				richText = true
			};
			GUIStyle guistyle = new GUIStyle(GUI.skin.box);
			guistyle.padding = new RectOffset(7, 7, 7, 7);
			GUIStyle guistyle2 = new GUIStyle(GUI.skin.label);
			guistyle2.clipping = TextClipping.Overflow;
			guistyle2.wordWrap = true;
			GUILayout.Space(20f);
			GUILayout.BeginVertical(guistyle, Array.Empty<GUILayoutOption>());
			GUILayout.Label("<b>Serialization Formats</b>", style, Array.Empty<GUILayoutOption>());
			GUILayout.Label("The serialization format of the data in specially serialized Unity objects. Binary is recommended for builds; JSON has the benefit of being human-readable but has significantly worse performance.\n\nWith the special editor-only node format, the serialized data will be formatted in such a way that, if the asset is saved with Unity's text format (Edit -> Project Settings -> Editor -> Asset Serialization -> Mode), the data will be mergeable when using version control systems. This makes the custom serialized data a lot less fragile, but comes at a performance cost during serialization and deserialization. The node format is recommended in the editor.\n\nThis setting can be overridden on a per-instance basis.\n", guistyle2, Array.Empty<GUILayoutOption>());
			GUILayout.Label("<b>Error Handling Policy</b>", style, Array.Empty<GUILayoutOption>());
			GUILayout.Label("The policy for handling any errors and irregularities that crop up during deserialization. Resilient is the recommended option, as it will always try to recover as much data as possible from a corrupt serialization stream.\n", guistyle2, Array.Empty<GUILayoutOption>());
			GUILayout.Label("<b>Logging Policy</b>", style, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Use this to determine the criticality of the events that are logged by the serialization system. Recommended value is to log only errors, and to log warnings and errors when you suspect issues in the system.", guistyle2, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
		}

		// Token: 0x0400000E RID: 14
		public const string ODIN_SERIALIZATION_CAUTIONARY_WARNING_TEXT = "Odin's custom serialization protocol is stable and fast. It is built to be fast, reliable and resilient above all.\n\n*Words of caution* \nHowever, caveats apply - there is a reason Unity chose such a drastically limited serialization protocol. It keeps things simple and manageable, and limits how much complexity you can introduce into your data structures. It can be very easy to get carried away and shoot yourself in the foot when all limitations suddenly disappear, and hence we have included this cautionary warning.\n\nWarning words aside, there can of course be valid reasons to use a more powerful serialization protocol such as Odin's. However, we advise you to use it wisely and with restraint. After all, with great power comes great responsibility!";

		// Token: 0x0400000F RID: 15
		public const string ODIN_PREFAB_CAUTIONARY_WARNING_TEXT = "In 2018.3, Unity introduced a new prefab workflow, and in so doing, changed how all prefabs fundamentally work. Despite our best efforts, we have so far been unable to achieve a stable implementation of Odin-serialized prefab modifications on prefab instances and variants in the new prefab workflow.This has nothing to do with Odin serializer itself, which remains rock solid. Odin-serialized ScriptableObjects and non-prefab Components/Behaviours are still perfectly stable - you are only seeing this message because this is an Odin-serialized prefab asset or instance.\n\nUsing prefabs with Odin serialization in 2018.3 and above is considered a *deprecated feature* and is officially unsupported. In short, if you disregard this message and then experience issues, we will not be able to help or support you.\n\nPlease keep all this in mind, if you wish to continue using Odin-serialized prefabs.";

		// Token: 0x04000010 RID: 16
		public const string ODIN_SERIALIZATION_CAUTIONARY_WARNING_BUTTON_TEXT = "I know what I'm about, son. Hide message forever.";

		// Token: 0x04000011 RID: 17
		public const string ODIN_PREFAB_CAUTIONARY_WARNING_BUTTON_TEXT = "I understand that I'm on my own. Hide message forever.";

		// Token: 0x04000012 RID: 18
		private static readonly DataFormat[] BuildFormats = new DataFormat[]
		{
			DataFormat.Binary,
			DataFormat.JSON
		};

		// Token: 0x04000013 RID: 19
		[Title("Warning messages", null, 0, true, true)]
		[ToggleLeft]
		[DetailedInfoBox("Click to show warning message.", "Odin's custom serialization protocol is stable and fast. It is built to be fast, reliable and resilient above all.\n\n*Words of caution* \nHowever, caveats apply - there is a reason Unity chose such a drastically limited serialization protocol. It keeps things simple and manageable, and limits how much complexity you can introduce into your data structures. It can be very easy to get carried away and shoot yourself in the foot when all limitations suddenly disappear, and hence we have included this cautionary warning.\n\nWarning words aside, there can of course be valid reasons to use a more powerful serialization protocol such as Odin's. However, we advise you to use it wisely and with restraint. After all, with great power comes great responsibility!", 1, null)]
		public bool HideSerializationCautionaryMessage;

		// Token: 0x04000014 RID: 20
		[ToggleLeft]
		[DetailedInfoBox("Click to show warning message.", "In 2018.3, Unity introduced a new prefab workflow, and in so doing, changed how all prefabs fundamentally work. Despite our best efforts, we have so far been unable to achieve a stable implementation of Odin-serialized prefab modifications on prefab instances and variants in the new prefab workflow.This has nothing to do with Odin serializer itself, which remains rock solid. Odin-serialized ScriptableObjects and non-prefab Components/Behaviours are still perfectly stable - you are only seeing this message because this is an Odin-serialized prefab asset or instance.\n\nUsing prefabs with Odin serialization in 2018.3 and above is considered a *deprecated feature* and is officially unsupported. In short, if you disregard this message and then experience issues, we will not be able to help or support you.\n\nPlease keep all this in mind, if you wish to continue using Odin-serialized prefabs.", 1, null)]
		public bool HidePrefabCautionaryMessage;

		// Token: 0x04000015 RID: 21
		[ToggleLeft]
		[SerializeField]
		[InfoBox("Enabling this will hide all warning messages that will show up in the inspector when the OdinSerialize attribute potentially does not achieve the desired effect.", 1, null)]
		public bool HideOdinSerializeAttributeWarningMessages;

		// Token: 0x04000016 RID: 22
		[SerializeField]
		[ToggleLeft]
		[LabelText("Hide Non-Serialized SerializeField/ShowInInspector Warning Messages")]
		[InfoBox("Enabling this will hide all warning messages that show up when the SerializeField and the ShowInInspector attributes are used together on non-serialized fields or properties.", 1, null)]
		public bool HideNonSerializedShowInInspectorWarningMessages;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		[Title("Data formatting options", null, 0, true, true)]
		[ValueDropdown("BuildFormats")]
		private DataFormat buildSerializationFormat;

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private DataFormat editorSerializationFormat = DataFormat.Nodes;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		[Title("Logging and error handling", null, 0, true, true)]
		private LoggingPolicy loggingPolicy;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		private ErrorHandlingPolicy errorHandlingPolicy;
	}
}
