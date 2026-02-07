using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AOT;
using FMOD;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FMODUnity
{
	// Token: 0x02000113 RID: 275
	[AddComponentMenu("")]
	public class RuntimeManager : MonoBehaviour
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x00008669 File Offset: 0x00006869
		static RuntimeManager()
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			RuntimeManager.masterBusPrefix = utf8Encoding.GetBytes("bus:/, ");
			RuntimeManager.eventSet3DAttributes = utf8Encoding.GetBytes("EventInstance::set3DAttributes");
			RuntimeManager.systemGetBus = utf8Encoding.GetBytes("System::getBus");
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0000869F File Offset: 0x0000689F
		public static bool IsMuted
		{
			get
			{
				return RuntimeManager.Instance.isMuted;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000086AC File Offset: 0x000068AC
		[MonoPInvokeCallback(typeof(DEBUG_CALLBACK))]
		private static RESULT DEBUG_CALLBACK(DEBUG_FLAGS flags, IntPtr filePtr, int line, IntPtr funcPtr, IntPtr messagePtr)
		{
			new StringWrapper(filePtr);
			StringWrapper fstring = new StringWrapper(funcPtr);
			StringWrapper fstring2 = new StringWrapper(messagePtr);
			if (flags == DEBUG_FLAGS.ERROR)
			{
				RuntimeUtils.DebugLogError(string.Format("[FMOD] {0} : {1}", fstring, fstring2));
			}
			else if (flags == DEBUG_FLAGS.WARNING)
			{
				RuntimeUtils.DebugLogWarning(string.Format("[FMOD] {0} : {1}", fstring, fstring2));
			}
			else if (flags == DEBUG_FLAGS.LOG)
			{
				RuntimeUtils.DebugLog(string.Format("[FMOD] {0} : {1}", fstring, fstring2));
			}
			return RESULT.OK;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00008734 File Offset: 0x00006934
		[MonoPInvokeCallback(typeof(FMOD.SYSTEM_CALLBACK))]
		private static RESULT ERROR_CALLBACK(IntPtr system, FMOD.SYSTEM_CALLBACK_TYPE type, IntPtr commanddata1, IntPtr commanddata2, IntPtr userdata)
		{
			ERRORCALLBACK_INFO errorcallback_INFO = Marshal.PtrToStructure<ERRORCALLBACK_INFO>(commanddata1);
			if ((errorcallback_INFO.instancetype == ERRORCALLBACK_INSTANCETYPE.CHANNEL || errorcallback_INFO.instancetype == ERRORCALLBACK_INSTANCETYPE.CHANNELCONTROL) && (errorcallback_INFO.result == RESULT.ERR_INVALID_HANDLE || errorcallback_INFO.result == RESULT.ERR_CHANNEL_STOLEN))
			{
				return RESULT.OK;
			}
			if (errorcallback_INFO.instancetype == ERRORCALLBACK_INSTANCETYPE.STUDIO_EVENTINSTANCE && errorcallback_INFO.functionname.Equals(RuntimeManager.eventSet3DAttributes) && errorcallback_INFO.result == RESULT.ERR_INVALID_HANDLE)
			{
				return RESULT.OK;
			}
			if (errorcallback_INFO.instancetype == ERRORCALLBACK_INSTANCETYPE.STUDIO_SYSTEM && errorcallback_INFO.functionname.Equals(RuntimeManager.systemGetBus) && errorcallback_INFO.result == RESULT.ERR_EVENT_NOTFOUND && errorcallback_INFO.functionparams.StartsWith(RuntimeManager.masterBusPrefix))
			{
				return RESULT.OK;
			}
			RuntimeUtils.DebugLogError(string.Format("[FMOD] {0}({1}) returned {2} for {3} (0x{4}).", new object[]
			{
				errorcallback_INFO.functionname,
				errorcallback_INFO.functionparams,
				errorcallback_INFO.result,
				errorcallback_INFO.instancetype,
				errorcallback_INFO.instance.ToString("X")
			}));
			return RESULT.OK;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00008838 File Offset: 0x00006A38
		private static RuntimeManager Instance
		{
			get
			{
				if (RuntimeManager.initException != null)
				{
					throw RuntimeManager.initException;
				}
				if (RuntimeManager.instance == null)
				{
					if (!Application.isPlaying)
					{
						UnityEngine.Debug.LogError("[FMOD] RuntimeManager accessed outside of runtime. Do not use RuntimeManager for Editor-only functionality, create your own System objects instead.");
						return null;
					}
					RuntimeManager[] array = Resources.FindObjectsOfTypeAll<RuntimeManager>();
					for (int i = 0; i < array.Length; i++)
					{
						UnityEngine.Object.DestroyImmediate(array[i].gameObject);
					}
					GameObject gameObject = new GameObject("FMOD.UnityIntegration.RuntimeManager");
					RuntimeManager.instance = gameObject.AddComponent<RuntimeManager>();
					if (Application.isPlaying)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					try
					{
						RuntimeUtils.EnforceLibraryOrder();
						RuntimeManager.instance.Initialize();
					}
					catch (Exception ex)
					{
						RuntimeManager.initException = (ex as SystemNotInitializedException);
						if (RuntimeManager.initException == null)
						{
							RuntimeManager.initException = new SystemNotInitializedException(ex);
						}
						throw RuntimeManager.initException;
					}
				}
				return RuntimeManager.instance;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0000890C File Offset: 0x00006B0C
		public static FMOD.Studio.System StudioSystem
		{
			get
			{
				return RuntimeManager.Instance.studioSystem;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00008918 File Offset: 0x00006B18
		public static FMOD.System CoreSystem
		{
			get
			{
				return RuntimeManager.Instance.coreSystem;
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00008924 File Offset: 0x00006B24
		private void CheckInitResult(RESULT result, string cause)
		{
			if (result != RESULT.OK)
			{
				this.ReleaseStudioSystem();
				throw new SystemNotInitializedException(result, cause);
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00008937 File Offset: 0x00006B37
		private void ReleaseStudioSystem()
		{
			if (this.studioSystem.isValid())
			{
				this.studioSystem.release();
				this.studioSystem.clearHandle();
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00008960 File Offset: 0x00006B60
		private RESULT Initialize()
		{
			RESULT result = RESULT.OK;
			Settings settings = Settings.Instance;
			this.currentPlatform = settings.FindCurrentPlatform();
			int sampleRate = this.currentPlatform.SampleRate;
			int softwareChannels = Math.Min(this.currentPlatform.RealChannelCount, 256);
			int virtualChannelCount = this.currentPlatform.VirtualChannelCount;
			uint dspbufferLength = (uint)this.currentPlatform.DSPBufferLength;
			int dspbufferCount = this.currentPlatform.DSPBufferCount;
			SPEAKERMODE speakerMode = this.currentPlatform.SpeakerMode;
			OUTPUTTYPE output = this.currentPlatform.GetOutputType();
			FMOD.ADVANCEDSETTINGS advancedsettings = default(FMOD.ADVANCEDSETTINGS);
			advancedsettings.randomSeed = (uint)DateTime.UtcNow.Ticks;
			advancedsettings.maxAT9Codecs = this.GetChannelCountForFormat(CodecType.AT9);
			advancedsettings.maxFADPCMCodecs = this.GetChannelCountForFormat(CodecType.FADPCM);
			advancedsettings.maxOpusCodecs = this.GetChannelCountForFormat(CodecType.Opus);
			advancedsettings.maxVorbisCodecs = this.GetChannelCountForFormat(CodecType.Vorbis);
			advancedsettings.maxXMACodecs = this.GetChannelCountForFormat(CodecType.XMA);
			RuntimeManager.SetThreadAffinities(this.currentPlatform);
			this.currentPlatform.PreSystemCreate(new Action<RESULT, string>(this.CheckInitResult));
			FMOD.Studio.INITFLAGS initflags = FMOD.Studio.INITFLAGS.DEFERRED_CALLBACKS;
			if (this.currentPlatform.IsLiveUpdateEnabled)
			{
				initflags |= FMOD.Studio.INITFLAGS.LIVEUPDATE;
				advancedsettings.profilePort = (ushort)this.currentPlatform.LiveUpdatePort;
			}
			for (;;)
			{
				RESULT result2 = FMOD.Studio.System.create(out this.studioSystem);
				this.CheckInitResult(result2, "FMOD.Studio.System.create");
				result2 = this.studioSystem.getCoreSystem(out this.coreSystem);
				this.CheckInitResult(result2, "FMOD.Studio.System.getCoreSystem");
				result2 = this.coreSystem.setOutput(output);
				this.CheckInitResult(result2, "FMOD.System.setOutput");
				result2 = this.coreSystem.setSoftwareChannels(softwareChannels);
				this.CheckInitResult(result2, "FMOD.System.setSoftwareChannels");
				result2 = this.coreSystem.setSoftwareFormat(sampleRate, speakerMode, 0);
				this.CheckInitResult(result2, "FMOD.System.setSoftwareFormat");
				if (dspbufferLength > 0U && dspbufferCount > 0)
				{
					result2 = this.coreSystem.setDSPBufferSize(dspbufferLength, dspbufferCount);
					this.CheckInitResult(result2, "FMOD.System.setDSPBufferSize");
				}
				result2 = this.coreSystem.setAdvancedSettings(ref advancedsettings);
				this.CheckInitResult(result2, "FMOD.System.setAdvancedSettings");
				if (settings.EnableErrorCallback)
				{
					this.errorCallback = new FMOD.SYSTEM_CALLBACK(RuntimeManager.ERROR_CALLBACK);
					result2 = this.coreSystem.setCallback(this.errorCallback, FMOD.SYSTEM_CALLBACK_TYPE.ERROR);
					this.CheckInitResult(result2, "FMOD.System.setCallback");
				}
				if (!string.IsNullOrEmpty(settings.EncryptionKey))
				{
					result2 = this.studioSystem.setAdvancedSettings(default(FMOD.Studio.ADVANCEDSETTINGS), Settings.Instance.EncryptionKey);
					this.CheckInitResult(result2, "FMOD.Studio.System.setAdvancedSettings");
				}
				if (settings.EnableMemoryTracking)
				{
					initflags |= FMOD.Studio.INITFLAGS.MEMORY_TRACKING;
				}
				this.currentPlatform.PreInitialize(this.studioSystem);
				PlatformCallbackHandler callbackHandler = this.currentPlatform.CallbackHandler;
				if (callbackHandler != null)
				{
					callbackHandler.PreInitialize(this.studioSystem, new Action<RESULT, string>(this.CheckInitResult));
				}
				result2 = this.studioSystem.initialize(virtualChannelCount, initflags, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
				if (result2 != RESULT.OK && result == RESULT.OK)
				{
					result = result2;
					output = OUTPUTTYPE.NOSOUND;
					RuntimeUtils.DebugLogErrorFormat("[FMOD] Studio::System::initialize returned {0}, defaulting to no-sound mode.", new object[]
					{
						result2.ToString()
					});
				}
				else
				{
					this.CheckInitResult(result2, "Studio::System::initialize");
					if ((initflags & FMOD.Studio.INITFLAGS.LIVEUPDATE) == FMOD.Studio.INITFLAGS.NORMAL)
					{
						break;
					}
					this.studioSystem.flushCommands();
					result2 = this.studioSystem.update();
					if (result2 != RESULT.ERR_NET_SOCKET_ERROR)
					{
						break;
					}
					initflags &= ~FMOD.Studio.INITFLAGS.LIVEUPDATE;
					RuntimeUtils.DebugLogWarning("[FMOD] Cannot open network port for Live Update (in-use), restarting with Live Update disabled.");
					result2 = this.studioSystem.release();
					this.CheckInitResult(result2, "FMOD.Studio.System.Release");
				}
			}
			this.currentPlatform.LoadPlugins(this.coreSystem, new Action<RESULT, string>(this.CheckInitResult));
			this.LoadBanks(settings);
			if (this.currentPlatform.IsOverlayEnabled)
			{
				this.SetOverlayPosition();
			}
			return result;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00008D00 File Offset: 0x00006F00
		private int GetChannelCountForFormat(CodecType format)
		{
			CodecChannelCount codecChannelCount = this.currentPlatform.CodecChannels.Find((CodecChannelCount x) => x.format == format);
			if (codecChannelCount != null)
			{
				return Math.Min(codecChannelCount.channels, 256);
			}
			return 0;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00008D4C File Offset: 0x00006F4C
		private static void SetThreadAffinities(Platform platform)
		{
			foreach (ThreadAffinityGroup threadAffinityGroup in platform.ThreadAffinities)
			{
				foreach (ThreadType threadType in threadAffinityGroup.threads)
				{
					THREAD_TYPE type = RuntimeUtils.ToFMODThreadType(threadType);
					THREAD_AFFINITY affinity = RuntimeUtils.ToFMODThreadAffinity(threadAffinityGroup.affinity);
					Thread.SetAttributes(type, affinity, THREAD_PRIORITY.DEFAULT, THREAD_STACK_SIZE.DEFAULT);
				}
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00008DEC File Offset: 0x00006FEC
		private void Update()
		{
			if (this.studioSystem.isValid())
			{
				if (StudioListener.ListenerCount <= 0 && !this.listenerWarningIssued)
				{
					this.listenerWarningIssued = true;
					RuntimeUtils.DebugLogWarning("[FMOD] Please add an 'FMOD Studio Listener' component to your camera in the scene for correct 3D positioning of sounds.");
				}
				StudioEventEmitter.UpdateActiveEmitters();
				for (int i = 0; i < this.attachedInstances.Count; i++)
				{
					PLAYBACK_STATE playback_STATE = PLAYBACK_STATE.STOPPED;
					if (this.attachedInstances[i].instance.isValid())
					{
						this.attachedInstances[i].instance.getPlaybackState(out playback_STATE);
					}
					if (playback_STATE == PLAYBACK_STATE.STOPPED || this.attachedInstances[i].transform == null)
					{
						this.attachedInstances[i] = this.attachedInstances[this.attachedInstances.Count - 1];
						this.attachedInstances.RemoveAt(this.attachedInstances.Count - 1);
						i--;
					}
					else if (this.attachedInstances[i].rigidBody)
					{
						this.attachedInstances[i].instance.set3DAttributes(RuntimeUtils.To3DAttributes(this.attachedInstances[i].transform, this.attachedInstances[i].rigidBody));
					}
					else if (this.attachedInstances[i].rigidBody2D)
					{
						this.attachedInstances[i].instance.set3DAttributes(RuntimeUtils.To3DAttributes(this.attachedInstances[i].transform, this.attachedInstances[i].rigidBody2D));
					}
					else if (!this.attachedInstances[i].nonRigidbodyVelocity)
					{
						this.attachedInstances[i].instance.set3DAttributes(this.attachedInstances[i].transform.To3DAttributes());
					}
					else
					{
						Vector3 position = this.attachedInstances[i].transform.position;
						Vector3 vector = Vector3.zero;
						if (Time.deltaTime != 0f)
						{
							vector = (position - this.attachedInstances[i].lastFramePosition) / Time.deltaTime;
							vector = Vector3.ClampMagnitude(vector, 20f);
						}
						this.attachedInstances[i].lastFramePosition = position;
						this.attachedInstances[i].instance.set3DAttributes(this.attachedInstances[i].transform.To3DAttributes(vector));
					}
				}
				if (this.isOverlayEnabled)
				{
					if (!this.overlayDrawer)
					{
						this.overlayDrawer = RuntimeManager.Instance.gameObject.AddComponent<FMODRuntimeManagerOnGUIHelper>();
						this.overlayDrawer.TargetRuntimeManager = this;
					}
					else
					{
						this.overlayDrawer.gameObject.SetActive(true);
					}
				}
				else if (this.overlayDrawer != null && this.overlayDrawer.gameObject.activeSelf)
				{
					this.overlayDrawer.gameObject.SetActive(false);
				}
				this.studioSystem.update();
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000090FC File Offset: 0x000072FC
		private static RuntimeManager.AttachedInstance FindOrAddAttachedInstance(EventInstance instance, Transform transform, ATTRIBUTES_3D attributes)
		{
			RuntimeManager.AttachedInstance attachedInstance = RuntimeManager.Instance.attachedInstances.Find((RuntimeManager.AttachedInstance x) => x.instance.handle == instance.handle);
			if (attachedInstance == null)
			{
				attachedInstance = new RuntimeManager.AttachedInstance();
				RuntimeManager.Instance.attachedInstances.Add(attachedInstance);
			}
			attachedInstance.instance = instance;
			attachedInstance.transform = transform;
			attachedInstance.instance.set3DAttributes(attributes);
			return attachedInstance;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0000916C File Offset: 0x0000736C
		public static void AttachInstanceToGameObject(EventInstance instance, GameObject gameObject, bool nonRigidbodyVelocity = false)
		{
			RuntimeManager.AttachedInstance attachedInstance = RuntimeManager.FindOrAddAttachedInstance(instance, gameObject.transform, gameObject.transform.To3DAttributes());
			if (nonRigidbodyVelocity)
			{
				attachedInstance.nonRigidbodyVelocity = nonRigidbodyVelocity;
				attachedInstance.lastFramePosition = gameObject.transform.position;
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x000091AC File Offset: 0x000073AC
		[Obsolete("This overload has been deprecated in favor of passing a GameObject instead of a Transform.", false)]
		public static void AttachInstanceToGameObject(EventInstance instance, Transform transform, bool nonRigidbodyVelocity = false)
		{
			RuntimeManager.AttachedInstance attachedInstance = RuntimeManager.FindOrAddAttachedInstance(instance, transform, transform.To3DAttributes());
			if (nonRigidbodyVelocity)
			{
				attachedInstance.nonRigidbodyVelocity = nonRigidbodyVelocity;
				attachedInstance.lastFramePosition = transform.position;
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x000091DD File Offset: 0x000073DD
		public static void AttachInstanceToGameObject(EventInstance instance, GameObject gameObject, Rigidbody rigidBody)
		{
			RuntimeManager.FindOrAddAttachedInstance(instance, gameObject.transform, RuntimeUtils.To3DAttributes(gameObject.transform, rigidBody)).rigidBody = rigidBody;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x000091FD File Offset: 0x000073FD
		[Obsolete("This overload has been deprecated in favor of passing a GameObject instead of a Transform.", false)]
		public static void AttachInstanceToGameObject(EventInstance instance, Transform transform, Rigidbody rigidBody)
		{
			RuntimeManager.FindOrAddAttachedInstance(instance, transform, RuntimeUtils.To3DAttributes(transform, rigidBody)).rigidBody = rigidBody;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00009213 File Offset: 0x00007413
		public static void AttachInstanceToGameObject(EventInstance instance, GameObject gameObject, Rigidbody2D rigidBody2D)
		{
			RuntimeManager.FindOrAddAttachedInstance(instance, gameObject.transform, RuntimeUtils.To3DAttributes(gameObject.transform, rigidBody2D)).rigidBody2D = rigidBody2D;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00009233 File Offset: 0x00007433
		[Obsolete("This overload has been deprecated in favor of passing a GameObject instead of a Transform.", false)]
		public static void AttachInstanceToGameObject(EventInstance instance, Transform transform, Rigidbody2D rigidBody2D)
		{
			RuntimeManager.FindOrAddAttachedInstance(instance, transform, RuntimeUtils.To3DAttributes(transform, rigidBody2D)).rigidBody2D = rigidBody2D;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000924C File Offset: 0x0000744C
		public static void DetachInstanceFromGameObject(EventInstance instance)
		{
			RuntimeManager runtimeManager = RuntimeManager.Instance;
			for (int i = 0; i < runtimeManager.attachedInstances.Count; i++)
			{
				if (runtimeManager.attachedInstances[i].instance.handle == instance.handle)
				{
					runtimeManager.attachedInstances[i] = runtimeManager.attachedInstances[runtimeManager.attachedInstances.Count - 1];
					runtimeManager.attachedInstances.RemoveAt(runtimeManager.attachedInstances.Count - 1);
					return;
				}
			}
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000092D8 File Offset: 0x000074D8
		internal void ExecuteOnGUI()
		{
			if (this.currentPlatform.OverlayRect != ScreenPosition.VR)
			{
				GUIStyle style = GUI.skin.GetStyle("window");
				style.fontSize = this.currentPlatform.OverlayFontSize;
				if (this.studioSystem.isValid() && this.isOverlayEnabled)
				{
					this.windowRect = GUI.Window(base.GetInstanceID(), this.windowRect, new GUI.WindowFunction(this.DrawDebugOverlay), "FMOD Studio Debug", style);
				}
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00009352 File Offset: 0x00007552
		private void Start()
		{
			this.isOverlayEnabled = this.currentPlatform.IsOverlayEnabled;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00009368 File Offset: 0x00007568
		private void UpdateDebugText()
		{
			if (this.lastDebugUpdate + 0.25f < Time.unscaledTime)
			{
				if (RuntimeManager.initException != null)
				{
					this.lastDebugText = RuntimeManager.initException.Message;
					return;
				}
				if (!this.mixerHead.hasHandle())
				{
					ChannelGroup channelGroup;
					this.coreSystem.getMasterChannelGroup(out channelGroup);
					channelGroup.getDSP(0, out this.mixerHead);
					this.mixerHead.setMeteringEnabled(false, true);
				}
				StringBuilder stringBuilder = new StringBuilder();
				FMOD.Studio.CPU_USAGE cpu_USAGE;
				FMOD.CPU_USAGE cpu_USAGE2;
				this.studioSystem.getCPUUsage(out cpu_USAGE, out cpu_USAGE2);
				stringBuilder.AppendFormat("CPU: dsp = {0:F1}%, studio = {1:F1}%\n", cpu_USAGE2.dsp, cpu_USAGE.update);
				int num;
				int num2;
				Memory.GetStats(out num, out num2, true);
				stringBuilder.AppendFormat("MEMORY: cur = {0}MB, max = {1}MB\n", num >> 20, num2 >> 20);
				int num3;
				int num4;
				this.coreSystem.getChannelsPlaying(out num3, out num4);
				stringBuilder.AppendFormat("CHANNELS: real = {0}, total = {1}\n", num4, num3);
				DSP_METERING_INFO dsp_METERING_INFO;
				this.mixerHead.getMeteringInfo(IntPtr.Zero, out dsp_METERING_INFO);
				float num5 = 0f;
				for (int i = 0; i < (int)dsp_METERING_INFO.numchannels; i++)
				{
					num5 += dsp_METERING_INFO.rmslevel[i] * dsp_METERING_INFO.rmslevel[i];
				}
				num5 = Mathf.Sqrt(num5 / (float)dsp_METERING_INFO.numchannels);
				float num6 = (num5 > 0f) ? (20f * Mathf.Log10(num5 * Mathf.Sqrt(2f))) : -80f;
				if (num6 > 10f)
				{
					num6 = 10f;
				}
				stringBuilder.AppendFormat("VOLUME: RMS = {0:f2}db\n", num6);
				this.lastDebugText = stringBuilder.ToString();
				this.lastDebugUpdate = Time.unscaledTime;
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00009528 File Offset: 0x00007728
		private void DrawDebugOverlay(int windowID)
		{
			this.UpdateDebugText();
			GUIStyle style = GUI.skin.GetStyle("label");
			style.fontSize = this.currentPlatform.OverlayFontSize;
			float width = (float)(this.currentPlatform.OverlayFontSize * 20);
			float height = (float)(this.currentPlatform.OverlayFontSize * 7);
			GUI.Label(new Rect(30f, 20f, width, height), this.lastDebugText, style);
			GUI.DragWindow();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000959D File Offset: 0x0000779D
		private void OnDestroy()
		{
			this.coreSystem.setCallback(null, (FMOD.SYSTEM_CALLBACK_TYPE)0U);
			this.ReleaseStudioSystem();
			RuntimeManager.initException = null;
			RuntimeManager.instance = null;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000095BF File Offset: 0x000077BF
		private void OnApplicationPause(bool pauseStatus)
		{
			if (this.studioSystem.isValid())
			{
				RuntimeManager.PauseAllEvents(pauseStatus);
				if (pauseStatus)
				{
					this.coreSystem.mixerSuspend();
					return;
				}
				this.coreSystem.mixerResume();
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000095F0 File Offset: 0x000077F0
		private static void ReferenceLoadedBank(string bankName, bool loadSamples)
		{
			RuntimeManager.LoadedBank value = RuntimeManager.Instance.loadedBanks[bankName];
			value.RefCount++;
			if (loadSamples)
			{
				value.Bank.loadSampleData();
			}
			RuntimeManager.Instance.loadedBanks[bankName] = value;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0000963C File Offset: 0x0000783C
		private void RegisterLoadedBank(RuntimeManager.LoadedBank loadedBank, string bankPath, string bankName, bool loadSamples, RESULT loadResult)
		{
			if (loadResult == RESULT.OK)
			{
				loadedBank.RefCount = 1;
				if (loadSamples)
				{
					loadedBank.Bank.loadSampleData();
				}
				RuntimeManager.Instance.loadedBanks.Add(bankName, loadedBank);
			}
			else
			{
				if (loadResult != RESULT.ERR_EVENT_ALREADY_LOADED)
				{
					throw new BankLoadException(bankPath, loadResult);
				}
				RuntimeUtils.DebugLogWarningFormat("[FMOD] Unable to load {0} - bank already loaded. This may occur when attempting to load another localized bank before the first is unloaded, or if a bank has been loaded via the API.", new object[]
				{
					bankName
				});
			}
			this.ExecuteSampleLoadRequestsIfReady();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x000096A4 File Offset: 0x000078A4
		private void ExecuteSampleLoadRequestsIfReady()
		{
			if (this.sampleLoadRequests.Count > 0)
			{
				foreach (string key in this.sampleLoadRequests)
				{
					if (!this.loadedBanks.ContainsKey(key))
					{
						return;
					}
				}
				foreach (string text in this.sampleLoadRequests)
				{
					this.CheckInitResult(this.loadedBanks[text].Bank.loadSampleData(), string.Format("Loading sample data for bank: {0}", text));
				}
				this.sampleLoadRequests.Clear();
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00009784 File Offset: 0x00007984
		public static void LoadBank(string bankName, bool loadSamples = false)
		{
			RuntimeManager.LoadBank(bankName, loadSamples, bankName);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00009790 File Offset: 0x00007990
		private static void LoadBank(string bankName, bool loadSamples, string bankId)
		{
			if (RuntimeManager.Instance.loadedBanks.ContainsKey(bankId))
			{
				RuntimeManager.ReferenceLoadedBank(bankId, loadSamples);
				return;
			}
			string text = RuntimeManager.Instance.currentPlatform.GetBankFolder();
			if (!string.IsNullOrEmpty(Settings.Instance.TargetSubFolder))
			{
				text = RuntimeUtils.GetCommonPlatformPath(Path.Combine(text, Settings.Instance.TargetSubFolder));
			}
			string text2;
			if (Path.GetExtension(bankName) != ".bank")
			{
				text2 = string.Format("{0}/{1}{2}", text, bankName, ".bank");
			}
			else
			{
				text2 = string.Format("{0}/{1}", text, bankName);
			}
			RuntimeManager.Instance.loadingBanksRef++;
			RuntimeManager.LoadedBank loadedBank = default(RuntimeManager.LoadedBank);
			RESULT loadResult = RuntimeManager.Instance.studioSystem.loadBankFile(text2, LOAD_BANK_FLAGS.NORMAL, out loadedBank.Bank);
			RuntimeManager.Instance.RegisterLoadedBank(loadedBank, text2, bankId, loadSamples, loadResult);
			RuntimeManager.Instance.loadingBanksRef--;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00009875 File Offset: 0x00007A75
		public static void LoadBank(TextAsset asset, bool loadSamples = false)
		{
			RuntimeManager.LoadBank(asset, loadSamples, asset.name);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00009884 File Offset: 0x00007A84
		private static void LoadBank(TextAsset asset, bool loadSamples, string bankId)
		{
			if (RuntimeManager.Instance.loadedBanks.ContainsKey(bankId))
			{
				RuntimeManager.ReferenceLoadedBank(bankId, loadSamples);
				return;
			}
			RuntimeManager.LoadedBank loadedBank = default(RuntimeManager.LoadedBank);
			RESULT loadResult = RuntimeManager.Instance.studioSystem.loadBankMemory(asset.bytes, LOAD_BANK_FLAGS.NORMAL, out loadedBank.Bank);
			RuntimeManager.Instance.RegisterLoadedBank(loadedBank, bankId, bankId, loadSamples, loadResult);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000098E0 File Offset: 0x00007AE0
		public static void LoadBank(AssetReference assetReference, bool loadSamples = false, Action completionCallback = null)
		{
			if (RuntimeManager.Instance.loadedBanks.ContainsKey(assetReference.AssetGUID))
			{
				RuntimeManager.ReferenceLoadedBank(assetReference.AssetGUID, loadSamples);
				return;
			}
			RuntimeManager.Instance.loadingBanksRef++;
			assetReference.LoadAssetAsync<TextAsset>().Completed += delegate(AsyncOperationHandle<TextAsset> obj)
			{
				if (!obj.IsValid())
				{
					string str = "[FMOD] Unable to load AssetReference: ";
					Exception operationException = obj.OperationException;
					RuntimeUtils.DebugLogError(str + ((operationException != null) ? operationException.ToString() : null));
					return;
				}
				RuntimeManager.LoadBank(obj.Result, loadSamples, assetReference.AssetGUID);
				RuntimeManager.Instance.loadingBanksRef--;
				if (completionCallback != null)
				{
					completionCallback();
				}
				assetReference.ReleaseAsset();
			};
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000996C File Offset: 0x00007B6C
		private void LoadBanks(Settings fmodSettings)
		{
			if (fmodSettings.ImportType == ImportType.StreamingAssets)
			{
				if (fmodSettings.AutomaticSampleLoading)
				{
					this.sampleLoadRequests.AddRange(this.BanksToLoad(fmodSettings));
				}
				try
				{
					foreach (string bankName in this.BanksToLoad(fmodSettings))
					{
						RuntimeManager.LoadBank(bankName, false);
					}
					RuntimeManager.WaitForAllSampleLoading();
				}
				catch (BankLoadException e)
				{
					RuntimeUtils.DebugLogException(e);
				}
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000099F8 File Offset: 0x00007BF8
		private IEnumerable<string> BanksToLoad(Settings fmodSettings)
		{
			switch (fmodSettings.BankLoadType)
			{
			case BankLoadType.All:
			{
				foreach (string masterBankFileName in fmodSettings.MasterBanks)
				{
					yield return masterBankFileName + ".strings";
					yield return masterBankFileName;
					masterBankFileName = null;
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
				foreach (string text in fmodSettings.Banks)
				{
					yield return text;
				}
				enumerator = default(List<string>.Enumerator);
				break;
			}
			case BankLoadType.Specified:
			{
				foreach (string text2 in fmodSettings.BanksToLoad)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						yield return text2;
					}
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
				break;
			}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00009A08 File Offset: 0x00007C08
		public static void UnloadBank(string bankName)
		{
			RuntimeManager.LoadedBank loadedBank;
			if (RuntimeManager.Instance.loadedBanks.TryGetValue(bankName, out loadedBank))
			{
				loadedBank.RefCount--;
				if (loadedBank.RefCount == 0)
				{
					loadedBank.Bank.unload();
					RuntimeManager.Instance.loadedBanks.Remove(bankName);
					RuntimeManager.Instance.sampleLoadRequests.Remove(bankName);
					return;
				}
				RuntimeManager.Instance.loadedBanks[bankName] = loadedBank;
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00009A7E File Offset: 0x00007C7E
		public static void UnloadBank(TextAsset asset)
		{
			RuntimeManager.UnloadBank(asset.name);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00009A8B File Offset: 0x00007C8B
		public static void UnloadBank(AssetReference assetReference)
		{
			RuntimeManager.UnloadBank(assetReference.AssetGUID);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00009A98 File Offset: 0x00007C98
		[Obsolete("[FMOD] Deprecated. Use AnySampleDataLoading instead.")]
		public static bool AnyBankLoading()
		{
			return RuntimeManager.AnySampleDataLoading();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00009AA0 File Offset: 0x00007CA0
		public static bool AnySampleDataLoading()
		{
			bool flag = false;
			foreach (RuntimeManager.LoadedBank loadedBank in RuntimeManager.Instance.loadedBanks.Values)
			{
				Bank bank = loadedBank.Bank;
				LOADING_STATE loading_STATE;
				bank.getSampleLoadingState(out loading_STATE);
				flag |= (loading_STATE == LOADING_STATE.LOADING);
			}
			return flag;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00009B14 File Offset: 0x00007D14
		[Obsolete("[FMOD] Deprecated. Use WaitForAllSampleLoading instead.")]
		public static void WaitForAllLoads()
		{
			RuntimeManager.WaitForAllSampleLoading();
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00009B1B File Offset: 0x00007D1B
		public static void WaitForAllSampleLoading()
		{
			RuntimeManager.Instance.studioSystem.flushSampleLoading();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00009B30 File Offset: 0x00007D30
		public static GUID PathToGUID(string path)
		{
			GUID result;
			if (path.StartsWith("{"))
			{
				Util.parseID(path, out result);
			}
			else if (RuntimeManager.Instance.studioSystem.lookupID(path, out result) == RESULT.ERR_EVENT_NOTFOUND)
			{
				throw new EventNotFoundException(path);
			}
			return result;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00009B74 File Offset: 0x00007D74
		public static EventReference PathToEventReference(string path)
		{
			GUID guid;
			try
			{
				guid = RuntimeManager.PathToGUID(path);
			}
			catch (EventNotFoundException)
			{
				guid = default(GUID);
			}
			return new EventReference
			{
				Guid = guid
			};
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00009BB8 File Offset: 0x00007DB8
		public static EventInstance CreateInstance(EventReference eventReference)
		{
			EventInstance result;
			try
			{
				result = RuntimeManager.CreateInstance(eventReference.Guid);
			}
			catch (EventNotFoundException)
			{
				throw new EventNotFoundException(eventReference);
			}
			return result;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00009BEC File Offset: 0x00007DEC
		public static EventInstance CreateInstance(string path)
		{
			EventInstance result;
			try
			{
				result = RuntimeManager.CreateInstance(RuntimeManager.PathToGUID(path));
			}
			catch (EventNotFoundException)
			{
				throw new EventNotFoundException(path);
			}
			return result;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00009C20 File Offset: 0x00007E20
		public static EventInstance CreateInstance(GUID guid)
		{
			EventInstance result;
			RuntimeManager.GetEventDescription(guid).createInstance(out result);
			return result;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00009C40 File Offset: 0x00007E40
		public static void PlayOneShot(EventReference eventReference, Vector3 position = default(Vector3))
		{
			try
			{
				RuntimeManager.PlayOneShot(eventReference.Guid, position);
			}
			catch (EventNotFoundException)
			{
				string str = "[FMOD] Event not found: ";
				EventReference eventReference2 = eventReference;
				RuntimeUtils.DebugLogWarning(str + eventReference2.ToString());
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00009C8C File Offset: 0x00007E8C
		public static void PlayOneShot(string path, Vector3 position = default(Vector3))
		{
			try
			{
				RuntimeManager.PlayOneShot(RuntimeManager.PathToGUID(path), position);
			}
			catch (EventNotFoundException)
			{
				RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + path);
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00009CCC File Offset: 0x00007ECC
		public static void PlayOneShot(GUID guid, Vector3 position = default(Vector3))
		{
			EventInstance eventInstance;
			if (RuntimeManager.CreateInstanceWithinMaxDistance(guid, position, out eventInstance))
			{
				eventInstance.set3DAttributes(position.To3DAttributes());
				eventInstance.start();
				eventInstance.release();
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00009D04 File Offset: 0x00007F04
		public static void PlayOneShotAttached(EventReference eventReference, GameObject gameObject)
		{
			try
			{
				RuntimeManager.PlayOneShotAttached(eventReference.Guid, gameObject);
			}
			catch (EventNotFoundException)
			{
				string str = "[FMOD] Event not found: ";
				EventReference eventReference2 = eventReference;
				RuntimeUtils.DebugLogWarning(str + eventReference2.ToString());
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00009D50 File Offset: 0x00007F50
		public static void PlayOneShotAttached(string path, GameObject gameObject)
		{
			try
			{
				RuntimeManager.PlayOneShotAttached(RuntimeManager.PathToGUID(path), gameObject);
			}
			catch (EventNotFoundException)
			{
				RuntimeUtils.DebugLogWarning("[FMOD] Event not found: " + path);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00009D90 File Offset: 0x00007F90
		public static void PlayOneShotAttached(GUID guid, GameObject gameObject)
		{
			EventInstance eventInstance;
			if (RuntimeManager.CreateInstanceWithinMaxDistance(guid, gameObject.transform.position, out eventInstance))
			{
				RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject, gameObject.GetComponent<Rigidbody>());
				eventInstance.start();
				eventInstance.release();
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00009DD0 File Offset: 0x00007FD0
		private static bool CreateInstanceWithinMaxDistance(GUID guid, Vector3 position, out EventInstance instance)
		{
			EventDescription eventDescription = RuntimeManager.GetEventDescription(guid);
			if (Settings.Instance.StopEventsOutsideMaxDistance)
			{
				bool flag;
				eventDescription.is3D(out flag);
				if (flag)
				{
					float num;
					float num2;
					eventDescription.getMinMaxDistance(out num, out num2);
					if (StudioListener.DistanceSquaredToNearestListener(position) > num2 * num2)
					{
						instance = default(EventInstance);
						return false;
					}
				}
			}
			eventDescription.createInstance(out instance);
			return true;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00009E28 File Offset: 0x00008028
		public static EventDescription GetEventDescription(EventReference eventReference)
		{
			EventDescription eventDescription;
			try
			{
				eventDescription = RuntimeManager.GetEventDescription(eventReference.Guid);
			}
			catch (EventNotFoundException)
			{
				throw new EventNotFoundException(eventReference);
			}
			return eventDescription;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00009E5C File Offset: 0x0000805C
		public static EventDescription GetEventDescription(string path)
		{
			EventDescription eventDescription;
			try
			{
				eventDescription = RuntimeManager.GetEventDescription(RuntimeManager.PathToGUID(path));
			}
			catch (EventNotFoundException)
			{
				throw new EventNotFoundException(path);
			}
			return eventDescription;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00009E90 File Offset: 0x00008090
		public static EventDescription GetEventDescription(GUID guid)
		{
			EventDescription eventDescription;
			if (RuntimeManager.Instance.cachedDescriptions.ContainsKey(guid) && RuntimeManager.Instance.cachedDescriptions[guid].isValid())
			{
				eventDescription = RuntimeManager.Instance.cachedDescriptions[guid];
			}
			else
			{
				if (RuntimeManager.Instance.studioSystem.getEventByID(guid, out eventDescription) != RESULT.OK)
				{
					throw new EventNotFoundException(guid);
				}
				if (eventDescription.isValid())
				{
					RuntimeManager.Instance.cachedDescriptions[guid] = eventDescription;
				}
			}
			return eventDescription;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00009F12 File Offset: 0x00008112
		public static void SetListenerLocation(GameObject gameObject, Rigidbody rigidBody, GameObject attenuationObject = null)
		{
			RuntimeManager.SetListenerLocation(0, gameObject, rigidBody, attenuationObject);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00009F20 File Offset: 0x00008120
		public static void SetListenerLocation(int listenerIndex, GameObject gameObject, Rigidbody rigidBody, GameObject attenuationObject = null)
		{
			if (attenuationObject)
			{
				RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, RuntimeUtils.To3DAttributes(gameObject.transform, rigidBody), attenuationObject.transform.position.ToFMODVector());
				return;
			}
			RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, RuntimeUtils.To3DAttributes(gameObject.transform, rigidBody));
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00009F80 File Offset: 0x00008180
		public static void SetListenerLocation(GameObject gameObject, Rigidbody2D rigidBody2D, GameObject attenuationObject = null)
		{
			RuntimeManager.SetListenerLocation(0, gameObject, rigidBody2D, attenuationObject);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00009F8C File Offset: 0x0000818C
		public static void SetListenerLocation(int listenerIndex, GameObject gameObject, Rigidbody2D rigidBody2D, GameObject attenuationObject = null)
		{
			if (attenuationObject)
			{
				RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, RuntimeUtils.To3DAttributes(gameObject.transform, rigidBody2D), attenuationObject.transform.position.ToFMODVector());
				return;
			}
			RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, RuntimeUtils.To3DAttributes(gameObject.transform, rigidBody2D));
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00009FEC File Offset: 0x000081EC
		public static void SetListenerLocation(int listenerIndex, GameObject gameObject, GameObject attenuationObject = null, Vector3 velocity = default(Vector3))
		{
			if (attenuationObject)
			{
				RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, gameObject.transform.To3DAttributes(velocity), attenuationObject.transform.position.ToFMODVector());
				return;
			}
			RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, gameObject.transform.To3DAttributes(velocity));
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000A04C File Offset: 0x0000824C
		public static void SetListenerLocation(GameObject gameObject, GameObject attenuationObject = null)
		{
			RuntimeManager.SetListenerLocation(0, gameObject, attenuationObject);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0000A058 File Offset: 0x00008258
		public static void SetListenerLocation(int listenerIndex, GameObject gameObject, GameObject attenuationObject = null)
		{
			if (attenuationObject)
			{
				RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, gameObject.transform.To3DAttributes(), attenuationObject.transform.position.ToFMODVector());
				return;
			}
			RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, gameObject.transform.To3DAttributes());
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0000A0B8 File Offset: 0x000082B8
		public static Bus GetBus(string path)
		{
			Bus result;
			if (RuntimeManager.StudioSystem.getBus(path, out result) != RESULT.OK)
			{
				throw new BusNotFoundException(path);
			}
			return result;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000A0E0 File Offset: 0x000082E0
		public static VCA GetVCA(string path)
		{
			VCA result;
			if (RuntimeManager.StudioSystem.getVCA(path, out result) != RESULT.OK)
			{
				throw new VCANotFoundException(path);
			}
			return result;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0000A108 File Offset: 0x00008308
		public static void PauseAllEvents(bool paused)
		{
			Bus bus;
			if (RuntimeManager.StudioSystem.getBus("bus:/", out bus) == RESULT.OK)
			{
				bus.setPaused(paused);
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0000A134 File Offset: 0x00008334
		public static void MuteAllEvents(bool muted)
		{
			RuntimeManager.Instance.isMuted = muted;
			RuntimeManager.ApplyMuteState();
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000A148 File Offset: 0x00008348
		private static void ApplyMuteState()
		{
			Bus bus;
			if (RuntimeManager.StudioSystem.getBus("bus:/", out bus) == RESULT.OK)
			{
				bus.setMute(RuntimeManager.Instance.isMuted);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0000A17D File Offset: 0x0000837D
		public static bool IsInitialized
		{
			get
			{
				return RuntimeManager.instance != null && RuntimeManager.instance.studioSystem.isValid();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x0000A19D File Offset: 0x0000839D
		public static bool HaveAllBanksLoaded
		{
			get
			{
				return RuntimeManager.Instance.loadingBanksRef == 0;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0000A1AC File Offset: 0x000083AC
		public static bool HaveMasterBanksLoaded
		{
			get
			{
				using (List<string>.Enumerator enumerator = Settings.Instance.MasterBanks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!RuntimeManager.HasBankLoaded(enumerator.Current))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000A20C File Offset: 0x0000840C
		public static bool HasBankLoaded(string loadedBank)
		{
			return RuntimeManager.Instance.loadedBanks.ContainsKey(loadedBank);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000A220 File Offset: 0x00008420
		private void SetOverlayPosition()
		{
			float num = (float)(this.currentPlatform.OverlayFontSize * 20);
			float num2 = (float)(this.currentPlatform.OverlayFontSize * 7);
			float num3 = 30f;
			switch (this.currentPlatform.OverlayRect)
			{
			case ScreenPosition.TopLeft:
				this.windowRect = new Rect(num3, num3, num, num2);
				return;
			case ScreenPosition.TopCenter:
				this.windowRect = new Rect((float)(Screen.width / 2) - num / 2f, num3, num, num2);
				return;
			case ScreenPosition.TopRight:
				this.windowRect = new Rect((float)Screen.width - (num + num3), num3, num, num2);
				return;
			case ScreenPosition.BottomLeft:
				this.windowRect = new Rect(num3, (float)Screen.height - (num2 + num3), num, num2);
				return;
			case ScreenPosition.BottomCenter:
				this.windowRect = new Rect((float)(Screen.width / 2) - num / 2f, (float)Screen.height - (num2 + num3), num, num2);
				return;
			case ScreenPosition.BottomRight:
				this.windowRect = new Rect((float)Screen.width - (num + num3), (float)Screen.height - (num2 + num3), num, num2);
				return;
			case ScreenPosition.Center:
				this.windowRect = new Rect((float)(Screen.width / 2) - num / 2f, (float)(Screen.height / 2) - num2 / 2f, num, num2);
				return;
			case ScreenPosition.VR:
				RuntimeUtils.DebugLogWarning("[FMOD] UNITY_URP_EXIST is not defined. The VR debug overlay requires the Universal Render Pipeline.");
				return;
			default:
				this.windowRect = new Rect(num3, num3, num, num2);
				return;
			}
		}

		// Token: 0x0400059D RID: 1437
		public const string BankStubPrefix = "bank stub:";

		// Token: 0x0400059E RID: 1438
		private static SystemNotInitializedException initException;

		// Token: 0x0400059F RID: 1439
		private static RuntimeManager instance;

		// Token: 0x040005A0 RID: 1440
		private Platform currentPlatform;

		// Token: 0x040005A1 RID: 1441
		private DEBUG_CALLBACK debugCallback;

		// Token: 0x040005A2 RID: 1442
		private FMOD.SYSTEM_CALLBACK errorCallback;

		// Token: 0x040005A3 RID: 1443
		private FMOD.Studio.System studioSystem;

		// Token: 0x040005A4 RID: 1444
		private FMOD.System coreSystem;

		// Token: 0x040005A5 RID: 1445
		private DSP mixerHead;

		// Token: 0x040005A6 RID: 1446
		private bool isMuted;

		// Token: 0x040005A7 RID: 1447
		private Dictionary<GUID, EventDescription> cachedDescriptions = new Dictionary<GUID, EventDescription>(new RuntimeManager.GuidComparer());

		// Token: 0x040005A8 RID: 1448
		private Dictionary<string, RuntimeManager.LoadedBank> loadedBanks = new Dictionary<string, RuntimeManager.LoadedBank>();

		// Token: 0x040005A9 RID: 1449
		private List<string> sampleLoadRequests = new List<string>();

		// Token: 0x040005AA RID: 1450
		private List<RuntimeManager.AttachedInstance> attachedInstances = new List<RuntimeManager.AttachedInstance>(128);

		// Token: 0x040005AB RID: 1451
		private bool listenerWarningIssued;

		// Token: 0x040005AC RID: 1452
		protected bool isOverlayEnabled;

		// Token: 0x040005AD RID: 1453
		private FMODRuntimeManagerOnGUIHelper overlayDrawer;

		// Token: 0x040005AE RID: 1454
		private Rect windowRect = new Rect(10f, 10f, 300f, 100f);

		// Token: 0x040005AF RID: 1455
		private string lastDebugText;

		// Token: 0x040005B0 RID: 1456
		private float lastDebugUpdate;

		// Token: 0x040005B1 RID: 1457
		private int loadingBanksRef;

		// Token: 0x040005B2 RID: 1458
		private static byte[] masterBusPrefix;

		// Token: 0x040005B3 RID: 1459
		private static byte[] eventSet3DAttributes;

		// Token: 0x040005B4 RID: 1460
		private static byte[] systemGetBus;

		// Token: 0x02000142 RID: 322
		private struct LoadedBank
		{
			// Token: 0x040006B0 RID: 1712
			public Bank Bank;

			// Token: 0x040006B1 RID: 1713
			public int RefCount;
		}

		// Token: 0x02000143 RID: 323
		private class GuidComparer : IEqualityComparer<GUID>
		{
			// Token: 0x060007FE RID: 2046 RVA: 0x0000C9FF File Offset: 0x0000ABFF
			bool IEqualityComparer<GUID>.Equals(GUID x, GUID y)
			{
				return x.Equals(y);
			}

			// Token: 0x060007FF RID: 2047 RVA: 0x0000CA09 File Offset: 0x0000AC09
			int IEqualityComparer<GUID>.GetHashCode(GUID obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02000144 RID: 324
		private class AttachedInstance
		{
			// Token: 0x040006B2 RID: 1714
			public EventInstance instance;

			// Token: 0x040006B3 RID: 1715
			public Transform transform;

			// Token: 0x040006B4 RID: 1716
			public Rigidbody rigidBody;

			// Token: 0x040006B5 RID: 1717
			public Vector3 lastFramePosition;

			// Token: 0x040006B6 RID: 1718
			public bool nonRigidbodyVelocity;

			// Token: 0x040006B7 RID: 1719
			public Rigidbody2D rigidBody2D;
		}
	}
}
