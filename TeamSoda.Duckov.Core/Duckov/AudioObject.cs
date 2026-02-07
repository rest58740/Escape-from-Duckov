using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000237 RID: 567
	public class AudioObject : MonoBehaviour
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00045957 File Offset: 0x00043B57
		// (set) Token: 0x060011D4 RID: 4564 RVA: 0x0004595F File Offset: 0x00043B5F
		public AudioManager.VoiceType VoiceType
		{
			get
			{
				return this.voiceType;
			}
			set
			{
				this.voiceType = value;
			}
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00045968 File Offset: 0x00043B68
		internal static AudioObject GetOrCreate(GameObject from)
		{
			AudioObject component = from.GetComponent<AudioObject>();
			if (component != null)
			{
				return component;
			}
			return from.AddComponent<AudioObject>();
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00045990 File Offset: 0x00043B90
		public EventInstance? PostQuak(string soundKey)
		{
			string eventName = "Char/Voice/vo_" + this.voiceType.ToString().ToLower() + "_" + soundKey;
			return this.Post(eventName, true);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x000459CC File Offset: 0x00043BCC
		public EventInstance? Post(string eventName, bool doRelease = true)
		{
			EventInstance eventInstance;
			if (!AudioManager.TryCreateEventInstance(eventName ?? "", out eventInstance))
			{
				return null;
			}
			eventInstance.setCallback(new EVENT_CALLBACK(AudioObject.EventCallback), -1);
			this.events.Add(eventInstance);
			eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(base.gameObject.transform.position));
			this.ApplyParameters(eventInstance);
			eventInstance.start();
			if (doRelease)
			{
				eventInstance.release();
			}
			return new EventInstance?(eventInstance);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00045A54 File Offset: 0x00043C54
		public EventInstance? PostCustomSFX(string filePath, bool doRelease = true, bool loop = false)
		{
			string eventPath = loop ? "SFX/custom_loop" : "SFX/custom";
			return this.PostFile(eventPath, filePath, doRelease);
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00045A7C File Offset: 0x00043C7C
		public EventInstance? PostFile(string eventPath, string filePath, bool doRelease = true)
		{
			if (!File.Exists(filePath))
			{
				Debug.Log("[Audio] File don't exist: " + filePath);
			}
			EventInstance eventInstance;
			if (!AudioManager.TryCreateEventInstance(eventPath, out eventInstance))
			{
				return null;
			}
			this.events.Add(eventInstance);
			GCHandle value = GCHandle.Alloc(filePath);
			eventInstance.setUserData(GCHandle.ToIntPtr(value));
			eventInstance.setCallback(new EVENT_CALLBACK(AudioObject.CustomSFXCallback), -1);
			eventInstance.start();
			if (doRelease)
			{
				eventInstance.release();
			}
			return new EventInstance?(eventInstance);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00045B04 File Offset: 0x00043D04
		private static RESULT CustomSFXCallback(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
		{
			EventInstance eventInstance;
			eventInstance..ctor(_event);
			IntPtr value;
			eventInstance.getUserData(ref value);
			GCHandle gchandle = GCHandle.FromIntPtr(value);
			string text = gchandle.Target as string;
			if (type != 2)
			{
				if (type != 128)
				{
					if (type == 256)
					{
						PROGRAMMER_SOUND_PROPERTIES programmer_SOUND_PROPERTIES = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(PROGRAMMER_SOUND_PROPERTIES));
						Sound sound;
						sound..ctor(programmer_SOUND_PROPERTIES.sound);
						sound.release();
					}
				}
				else
				{
					MODE mode = 66050;
					PROGRAMMER_SOUND_PROPERTIES structure = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(PROGRAMMER_SOUND_PROPERTIES));
					Sound sound2;
					if (RuntimeManager.CoreSystem.createSound(text, mode, ref sound2) == null)
					{
						structure.sound = sound2.handle;
						structure.subsoundIndex = -1;
						Marshal.StructureToPtr<PROGRAMMER_SOUND_PROPERTIES>(structure, parameters, false);
					}
				}
			}
			else
			{
				gchandle.Free();
			}
			return 0;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00045BE0 File Offset: 0x00043DE0
		public void Stop(string eventName, STOP_MODE mode)
		{
			foreach (EventInstance eventInstance in this.events)
			{
				EventDescription eventDescription;
				string str;
				if (eventInstance.getDescription(ref eventDescription) == null && eventDescription.getPath(ref str) == null && !("event:/" + str != eventName))
				{
					eventInstance.stop(mode);
					break;
				}
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00045C60 File Offset: 0x00043E60
		private static RESULT EventCallback(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
		{
			if (type <= 1024)
			{
				if (type <= 32)
				{
					if (type <= 8)
					{
						switch (type)
						{
						case 1:
						case 2:
						case 3:
						case 4:
							break;
						default:
							if (type != 8)
							{
							}
							break;
						}
					}
					else if (type != 16 && type != 32)
					{
					}
				}
				else if (type <= 128)
				{
					if (type != 64 && type != 128)
					{
					}
				}
				else if (type != 256 && type != 512 && type != 1024)
				{
				}
			}
			else if (type <= 16384)
			{
				if (type <= 4096)
				{
					if (type != 2048 && type != 4096)
					{
					}
				}
				else if (type != 8192 && type != 16384)
				{
				}
			}
			else if (type <= 65536)
			{
				if (type != 32768 && type != 65536)
				{
				}
			}
			else if (type == 131072 || type != 262144)
			{
			}
			return 0;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00045D50 File Offset: 0x00043F50
		private void FixedUpdate()
		{
			if (this == null)
			{
				return;
			}
			if (base.transform == null)
			{
				return;
			}
			if (this.events == null)
			{
				return;
			}
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(base.transform.position));
				}
			}
			if (this.needCleanup)
			{
				this.events.RemoveAll((EventInstance e) => !e.isValid());
				this.needCleanup = false;
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00045E24 File Offset: 0x00044024
		internal void SetParameterByName(string parameter, float value)
		{
			this.parameters[parameter] = value;
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.setParameterByName(parameter, value, false);
				}
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00045E9C File Offset: 0x0004409C
		internal void SetParameterByNameWithLabel(string parameter, string label)
		{
			this.strParameters[parameter] = label;
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.setParameterByNameWithLabel(parameter, label, false);
				}
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00045F14 File Offset: 0x00044114
		private void ApplyParameters(EventInstance eventInstance)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
			{
				eventInstance.setParameterByName(keyValuePair.Key, keyValuePair.Value, false);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in this.strParameters)
			{
				eventInstance.setParameterByNameWithLabel(keyValuePair2.Key, keyValuePair2.Value, false);
			}
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00045FCC File Offset: 0x000441CC
		internal void StopAll(STOP_MODE mode = 1)
		{
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.stop(mode);
				}
			}
		}

		// Token: 0x04000DEB RID: 3563
		private Dictionary<string, float> parameters = new Dictionary<string, float>();

		// Token: 0x04000DEC RID: 3564
		private Dictionary<string, string> strParameters = new Dictionary<string, string>();

		// Token: 0x04000DED RID: 3565
		private AudioManager.VoiceType voiceType;

		// Token: 0x04000DEE RID: 3566
		public List<EventInstance> events = new List<EventInstance>();

		// Token: 0x04000DEF RID: 3567
		private bool needCleanup;
	}
}
