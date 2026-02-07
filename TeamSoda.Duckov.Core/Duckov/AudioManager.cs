using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Duckov.Options;
using Duckov.Scenes;
using Duckov.UI;
using FMOD.Studio;
using FMODUnity;
using ItemStatsSystem;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Duckov
{
	// Token: 0x02000236 RID: 566
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x00044C96 File Offset: 0x00042E96
		public static AudioManager Instance
		{
			get
			{
				return GameManager.AudioManager;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x00044CA0 File Offset: 0x00042EA0
		public static bool IsStingerPlaying
		{
			get
			{
				if (AudioManager.Instance == null)
				{
					return false;
				}
				if (AudioManager.Instance.stingerSource == null)
				{
					return false;
				}
				return AudioManager.Instance.stingerSource.events.Any((EventInstance e) => e.isValid());
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00044D03 File Offset: 0x00042F03
		private IEnumerable<AudioManager.Bus> AllBueses()
		{
			yield return this.masterBus;
			yield return this.sfxBus;
			yield return this.musicBus;
			yield break;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x00044D13 File Offset: 0x00042F13
		private Transform listener
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00044D1B File Offset: 0x00042F1B
		private static Transform SoundSourceParent
		{
			get
			{
				if (AudioManager._soundSourceParent == null)
				{
					GameObject gameObject = new GameObject("Sound Sources");
					AudioManager._soundSourceParent = gameObject.transform;
					Object.DontDestroyOnLoad(gameObject);
				}
				return AudioManager._soundSourceParent;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060011A6 RID: 4518 RVA: 0x00044D4C File Offset: 0x00042F4C
		private static ObjectPool<GameObject> SoundSourcePool
		{
			get
			{
				if (AudioManager._soundSourcePool == null)
				{
					AudioManager._soundSourcePool = new ObjectPool<GameObject>(delegate()
					{
						GameObject gameObject = new GameObject("SoundSource");
						gameObject.transform.SetParent(AudioManager.SoundSourceParent);
						return gameObject;
					}, delegate(GameObject e)
					{
						e.SetActive(true);
					}, delegate(GameObject e)
					{
						e.SetActive(false);
					}, null, true, 10, 10000);
				}
				return AudioManager._soundSourcePool;
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00044DD8 File Offset: 0x00042FD8
		public static EventInstance? Post(string eventName, GameObject gameObject)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			if (gameObject == null)
			{
				Debug.LogError(string.Format("Posting event but gameObject is null: {0}", gameObject));
			}
			if (!gameObject.activeSelf)
			{
				Debug.LogError(string.Format("Posting event but gameObject is not active: {0}", gameObject));
			}
			return AudioManager.Instance.MPost(eventName, gameObject);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00044E34 File Offset: 0x00043034
		public static EventInstance? PostCustomSFX(string filePath, GameObject gameObject = null, bool loop = false)
		{
			if (AudioManager.Instance == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(filePath))
			{
				return null;
			}
			if (gameObject != null && !gameObject.activeSelf)
			{
				Debug.LogError(string.Format("Posting event but gameObject is not active: {0}", gameObject));
			}
			return AudioManager.Instance.MPostCustomSFX(filePath, gameObject, loop);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00044E98 File Offset: 0x00043098
		public static EventInstance? Post(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			return AudioManager.Instance.MPost(eventName, null);
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00044EC4 File Offset: 0x000430C4
		public static EventInstance? Post(string eventName, Vector3 position)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			return AudioManager.Instance.MPost(eventName, position);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x00044EEF File Offset: 0x000430EF
		internal static EventInstance? PostQuak(string soundKey, AudioManager.VoiceType voiceType, GameObject gameObject)
		{
			AudioObject orCreate = AudioObject.GetOrCreate(gameObject);
			orCreate.VoiceType = voiceType;
			return orCreate.PostQuak(soundKey);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00044F04 File Offset: 0x00043104
		public static void PostHitMarker(bool crit)
		{
			AudioManager.Post(crit ? "SFX/Combat/Marker/hitmarker_head" : "SFX/Combat/Marker/hitmarker");
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00044F1B File Offset: 0x0004311B
		public static void PostKillMarker(bool crit = false)
		{
			AudioManager.Post(crit ? "SFX/Combat/Marker/killmarker_head" : "SFX/Combat/Marker/killmarker");
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00044F34 File Offset: 0x00043134
		private void Awake()
		{
			CharacterSoundMaker.OnFootStepSound = (Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>)Delegate.Combine(CharacterSoundMaker.OnFootStepSound, new Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>(this.OnFootStepSound));
			Projectile.OnBulletFlyByCharacter = (Action<Vector3>)Delegate.Combine(Projectile.OnBulletFlyByCharacter, new Action<Vector3>(this.OnBulletFlyby));
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
			ItemUIUtilities.OnPutItem += this.OnPutItem;
			Health.OnDead += this.OnHealthDead;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
			OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.LoadOptions();
			}
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00045024 File Offset: 0x00043224
		private void OnDestroy()
		{
			CharacterSoundMaker.OnFootStepSound = (Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>)Delegate.Remove(CharacterSoundMaker.OnFootStepSound, new Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>(this.OnFootStepSound));
			Projectile.OnBulletFlyByCharacter = (Action<Vector3>)Delegate.Remove(Projectile.OnBulletFlyByCharacter, new Action<Vector3>(this.OnBulletFlyby));
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
			ItemUIUtilities.OnPutItem -= this.OnPutItem;
			Health.OnDead -= this.OnHealthDead;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
			OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000450D8 File Offset: 0x000432D8
		private void OnOptionsChanged(string key)
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.NotifyOptionsChanged(key);
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00045124 File Offset: 0x00043324
		public static AudioManager.Bus GetBus(string name)
		{
			if (AudioManager.Instance == null)
			{
				return null;
			}
			foreach (AudioManager.Bus bus in AudioManager.Instance.AllBueses())
			{
				if (bus.Name == name)
				{
					return bus;
				}
			}
			return null;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00045194 File Offset: 0x00043394
		private void OnStartedLoadingScene(SceneLoadingContext context)
		{
			if (this.ambientSource)
			{
				this.ambientSource.StopAll(1);
			}
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000451AF File Offset: 0x000433AF
		private void OnLevelInitialized()
		{
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000451B1 File Offset: 0x000433B1
		private void Start()
		{
			this.UpdateBuses();
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x000451B9 File Offset: 0x000433B9
		private void OnHealthDead(Health health, DamageInfo info)
		{
			if (health.TryGetCharacter() == CharacterMainControl.Main)
			{
				AudioManager.StopBGM();
				AudioManager.Post("Music/Stinger/stg_death");
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000451DD File Offset: 0x000433DD
		private void OnPutItem(Item item, bool pickup = false)
		{
			AudioManager.PlayPutItemSFX(item, pickup);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x000451E6 File Offset: 0x000433E6
		public static void PlayPutItemSFX(Item item, bool pickup = false)
		{
			if (item == null)
			{
				return;
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			AudioManager.Post(StringExtensions.Format(pickup ? "SFX/Item/pickup_{soundkey}" : "SFX/Item/put_{soundkey}", new
			{
				soundkey = item.SoundKey.ToLower()
			}));
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00045224 File Offset: 0x00043424
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Opening ears";
			SubSceneEntry subSceneInfo = core.GetSubSceneInfo(scene);
			if (subSceneInfo == null)
			{
				return;
			}
			if (this.ambientSource)
			{
				LevelManager.LevelInitializingComment = "Hearing Ambient";
				this.ambientSource.StopAll(1);
				this.ambientSource.Post(StringExtensions.Format("Amb/amb_{soundkey}", new
				{
					soundkey = subSceneInfo.AmbientSound.ToLower()
				}), true);
			}
			LevelManager.LevelInitializingComment = "Hearing Buses";
			this.ApplyBuses();
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x000452A1 File Offset: 0x000434A1
		public static bool PlayingBGM
		{
			get
			{
				return AudioManager.playingBGM;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x000452A8 File Offset: 0x000434A8
		private static bool LogEvent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x000452AC File Offset: 0x000434AC
		public static bool TryCreateEventInstance(string eventPath, out EventInstance eventInstance)
		{
			eventInstance = default(EventInstance);
			string text = "event:/" + eventPath;
			try
			{
				eventInstance = RuntimeManager.CreateInstance(text);
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				if (AudioManager.LogEvent)
				{
					Debug.LogError("[AudioEvent][Failed] " + text);
				}
			}
			return false;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00045310 File Offset: 0x00043510
		public static EventInstance? PlayCustomBGM(string filePath, bool loop = true)
		{
			AudioManager.StopBGM();
			if (AudioManager.Instance == null)
			{
				return null;
			}
			AudioManager.playingBGM = true;
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return null;
			}
			if (!File.Exists(filePath))
			{
				Debug.Log("[Audio] [Custom BGM] File don't exist: " + filePath);
			}
			string eventPath = loop ? "Music/custom_loop" : "Music/custom";
			return AudioManager.Instance.bgmSource.PostFile(eventPath, filePath, true);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0004538C File Offset: 0x0004358C
		public static EventInstance? PlayBGM(string name)
		{
			AudioManager.StopBGM();
			if (AudioManager.Instance == null)
			{
				return null;
			}
			AudioManager.playingBGM = true;
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			string eventName = StringExtensions.Format("Music/Loop/{soundkey}", new
			{
				soundkey = name
			});
			return AudioManager.Instance.bgmSource.Post(eventName, true);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x000453EF File Offset: 0x000435EF
		public static void StopBGM()
		{
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.Instance.bgmSource.StopAll(0);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00045410 File Offset: 0x00043610
		public static void PlayStringer(string key)
		{
			string eventName = StringExtensions.Format("Music/Stinger/{key}", new
			{
				key
			});
			AudioManager.Instance.stingerSource.Post(eventName, true);
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00045440 File Offset: 0x00043640
		private void OnBulletFlyby(Vector3 vector)
		{
			AudioManager.Post("SFX/Combat/Bullet/flyby", vector);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0004544E File Offset: 0x0004364E
		public static void SetState(string stateGroup, string state)
		{
			AudioManager.globalStates[stateGroup] = state;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004545C File Offset: 0x0004365C
		public static string GetState(string stateGroup)
		{
			string result;
			if (AudioManager.globalStates.TryGetValue(stateGroup, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0004547B File Offset: 0x0004367B
		private void Update()
		{
			this.UpdateListener();
			this.UpdateBuses();
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0004548C File Offset: 0x0004368C
		private void UpdateListener()
		{
			if (LevelManager.Instance == null)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					this.listener.transform.position = main.transform.position;
					this.listener.transform.rotation = main.transform.rotation;
				}
				return;
			}
			GameCamera gameCamera = LevelManager.Instance.GameCamera;
			if (gameCamera != null)
			{
				if (CharacterMainControl.Main != null)
				{
					this.listener.transform.position = CharacterMainControl.Main.transform.position + Vector3.up * 2f;
				}
				else
				{
					this.listener.transform.position = gameCamera.renderCamera.transform.position;
				}
				this.listener.transform.rotation = gameCamera.renderCamera.transform.rotation;
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00045588 File Offset: 0x00043788
		private void UpdateBuses()
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				if (bus.Dirty)
				{
					bus.Apply();
				}
			}
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000455DC File Offset: 0x000437DC
		private void ApplyBuses()
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.Apply();
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00045628 File Offset: 0x00043828
		private void OnFootStepSound(Vector3 position, CharacterSoundMaker.FootStepTypes type, CharacterMainControl character)
		{
			if (character == null)
			{
				return;
			}
			GameObject gameObject = character.gameObject;
			string value = "floor";
			this.MSetParameter(gameObject, "terrain", value);
			if (character.FootStepMaterialType != AudioManager.FootStepMaterialType.noSound)
			{
				string charaType = character.FootStepMaterialType.ToString();
				string strengthType = "light";
				switch (type)
				{
				case CharacterSoundMaker.FootStepTypes.walkLight:
				case CharacterSoundMaker.FootStepTypes.runLight:
					strengthType = "light";
					break;
				case CharacterSoundMaker.FootStepTypes.walkHeavy:
				case CharacterSoundMaker.FootStepTypes.runHeavy:
					strengthType = "heavy";
					break;
				}
				AudioManager.Post(StringExtensions.Format("Char/Footstep/footstep_{charaType}_{strengthType}", new
				{
					charaType,
					strengthType
				}), character.gameObject);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x000456C1 File Offset: 0x000438C1
		public static bool Initialized
		{
			get
			{
				return RuntimeManager.IsInitialized;
			}
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x000456C8 File Offset: 0x000438C8
		private void MSetParameter(GameObject gameObject, string parameterName, string value)
		{
			if (gameObject == null)
			{
				Debug.LogError("Game Object must exist");
				return;
			}
			AudioObject.GetOrCreate(gameObject).SetParameterByNameWithLabel(parameterName, value);
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000456EC File Offset: 0x000438EC
		private EventInstance? MPost(string eventName, GameObject gameObject = null)
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(eventName))
			{
				return null;
			}
			if (gameObject == null)
			{
				gameObject = AudioManager.Instance.gameObject;
			}
			else if (!gameObject.activeInHierarchy)
			{
				Debug.LogWarning("Posting event on inactive object, canceled");
				return null;
			}
			return AudioObject.GetOrCreate(gameObject).Post(eventName ?? "", true);
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00045768 File Offset: 0x00043968
		private EventInstance? MPostCustomSFX(string filePath, GameObject gameObject = null, bool loop = false)
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return null;
			}
			if (gameObject == null)
			{
				gameObject = AudioManager.Instance.gameObject;
			}
			else if (!gameObject.activeInHierarchy)
			{
				Debug.LogWarning("Posting event on inactive object, canceled");
				return null;
			}
			return AudioObject.GetOrCreate(gameObject).PostCustomSFX(filePath, true, loop);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x000457DC File Offset: 0x000439DC
		private EventInstance? MPost(string eventName, Vector3 position)
		{
			AudioManager.SoundSourcePool.Get().transform.position = position;
			EventInstance value;
			if (!AudioManager.TryCreateEventInstance(eventName ?? "", out value))
			{
				return null;
			}
			value.set3DAttributes(RuntimeUtils.To3DAttributes(position));
			value.start();
			value.release();
			return new EventInstance?(value);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004583F File Offset: 0x00043A3F
		public static void StopAll(GameObject gameObject, STOP_MODE mode = 1)
		{
			AudioObject.GetOrCreate(gameObject).StopAll(mode);
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00045850 File Offset: 0x00043A50
		internal void MSetRTPC(string key, float value, GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				RuntimeManager.StudioSystem.setParameterByName("parameter:/" + key, value, false);
				if (AudioManager.LogEvent)
				{
					Debug.Log(string.Format("[AudioEvent][Parameter][Global] {0} = {1}", key, value));
					return;
				}
			}
			else
			{
				AudioObject.GetOrCreate(gameObject).SetParameterByName("parameter:/" + key, value);
				if (AudioManager.LogEvent)
				{
					Debug.Log(string.Format("[AudioEvent][Parameter][GameObject] {0} = {1}", key, value), gameObject);
				}
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000458D4 File Offset: 0x00043AD4
		internal static void SetRTPC(string key, float value, GameObject gameObject = null)
		{
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.Instance.MSetRTPC(key, value, gameObject);
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x000458F1 File Offset: 0x00043AF1
		public static void SetVoiceType(GameObject gameObject, AudioManager.VoiceType voiceType)
		{
			if (gameObject == null)
			{
				return;
			}
			AudioObject.GetOrCreate(gameObject).VoiceType = voiceType;
		}

		// Token: 0x04000DCE RID: 3534
		[SerializeField]
		private AudioObject ambientSource;

		// Token: 0x04000DCF RID: 3535
		[SerializeField]
		private AudioObject bgmSource;

		// Token: 0x04000DD0 RID: 3536
		[SerializeField]
		private AudioObject stingerSource;

		// Token: 0x04000DD1 RID: 3537
		[SerializeField]
		private AudioManager.Bus masterBus = new AudioManager.Bus("Master");

		// Token: 0x04000DD2 RID: 3538
		[SerializeField]
		private AudioManager.Bus sfxBus = new AudioManager.Bus("Master/SFX");

		// Token: 0x04000DD3 RID: 3539
		[SerializeField]
		private AudioManager.Bus musicBus = new AudioManager.Bus("Master/Music");

		// Token: 0x04000DD4 RID: 3540
		private static Transform _soundSourceParent;

		// Token: 0x04000DD5 RID: 3541
		private static ObjectPool<GameObject> _soundSourcePool;

		// Token: 0x04000DD6 RID: 3542
		private const string path_hitmarker_norm = "SFX/Combat/Marker/hitmarker";

		// Token: 0x04000DD7 RID: 3543
		private const string path_hitmarker_crit = "SFX/Combat/Marker/hitmarker_head";

		// Token: 0x04000DD8 RID: 3544
		private const string path_killmarker_norm = "SFX/Combat/Marker/killmarker";

		// Token: 0x04000DD9 RID: 3545
		private const string path_killmarker_crit = "SFX/Combat/Marker/killmarker_head";

		// Token: 0x04000DDA RID: 3546
		private const string path_music_death = "Music/Stinger/stg_death";

		// Token: 0x04000DDB RID: 3547
		private const string path_bullet_flyby = "SFX/Combat/Bullet/flyby";

		// Token: 0x04000DDC RID: 3548
		private const string path_pickup_item_fmt_soundkey = "SFX/Item/pickup_{soundkey}";

		// Token: 0x04000DDD RID: 3549
		private const string path_put_item_fmt_soundkey = "SFX/Item/put_{soundkey}";

		// Token: 0x04000DDE RID: 3550
		private const string path_ambient_fmt_soundkey = "Amb/amb_{soundkey}";

		// Token: 0x04000DDF RID: 3551
		private const string path_music_loop_fmt_soundkey = "Music/Loop/{soundkey}";

		// Token: 0x04000DE0 RID: 3552
		private const string path_footstep_fmt_soundkey = "Char/Footstep/footstep_{charaType}_{strengthType}";

		// Token: 0x04000DE1 RID: 3553
		public const string path_reload_fmt_soundkey = "SFX/Combat/Gun/Reload/{soundkey}";

		// Token: 0x04000DE2 RID: 3554
		public const string path_shoot_fmt_gunkey = "SFX/Combat/Gun/Shoot/{soundkey}";

		// Token: 0x04000DE3 RID: 3555
		public const string path_task_finished = "UI/mission_small";

		// Token: 0x04000DE4 RID: 3556
		public const string path_building_built = "UI/building_up";

		// Token: 0x04000DE5 RID: 3557
		public const string path_gun_unload = "SFX/Combat/Gun/unload";

		// Token: 0x04000DE6 RID: 3558
		public const string path_stinger_fmt_key = "Music/Stinger/{key}";

		// Token: 0x04000DE7 RID: 3559
		private static bool playingBGM;

		// Token: 0x04000DE8 RID: 3560
		private static EventInstance bgmEvent;

		// Token: 0x04000DE9 RID: 3561
		private static Dictionary<string, string> globalStates = new Dictionary<string, string>();

		// Token: 0x04000DEA RID: 3562
		private static Dictionary<int, AudioManager.VoiceType> gameObjectVoiceTypes = new Dictionary<int, AudioManager.VoiceType>();

		// Token: 0x02000543 RID: 1347
		[Serializable]
		public class Bus
		{
			// Token: 0x17000782 RID: 1922
			// (get) Token: 0x060028BC RID: 10428 RVA: 0x00095BFC File Offset: 0x00093DFC
			public string Name
			{
				get
				{
					return this.volumeRTPC;
				}
			}

			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x060028BD RID: 10429 RVA: 0x00095C04 File Offset: 0x00093E04
			// (set) Token: 0x060028BE RID: 10430 RVA: 0x00095C0C File Offset: 0x00093E0C
			public float Volume
			{
				get
				{
					return this.volume;
				}
				set
				{
					this.volume = value;
					this.Apply();
				}
			}

			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x060028BF RID: 10431 RVA: 0x00095C1B File Offset: 0x00093E1B
			// (set) Token: 0x060028C0 RID: 10432 RVA: 0x00095C23 File Offset: 0x00093E23
			public bool Mute
			{
				get
				{
					return this.mute;
				}
				set
				{
					this.mute = value;
					this.Apply();
				}
			}

			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x060028C1 RID: 10433 RVA: 0x00095C32 File Offset: 0x00093E32
			public bool Dirty
			{
				get
				{
					return this.appliedVolume != this.Volume;
				}
			}

			// Token: 0x060028C2 RID: 10434 RVA: 0x00095C48 File Offset: 0x00093E48
			public void Apply()
			{
				try
				{
					FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + this.volumeRTPC);
					bus.setVolume(this.Volume);
					bus.setMute(this.Mute);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				this.appliedVolume = this.Volume;
				OptionsManager.Save<float>(this.SaveKey, this.volume);
			}

			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x060028C3 RID: 10435 RVA: 0x00095CC0 File Offset: 0x00093EC0
			private string SaveKey
			{
				get
				{
					return "Audio/" + this.volumeRTPC;
				}
			}

			// Token: 0x060028C4 RID: 10436 RVA: 0x00095CD2 File Offset: 0x00093ED2
			public Bus(string rtpc)
			{
				this.volumeRTPC = rtpc;
			}

			// Token: 0x060028C5 RID: 10437 RVA: 0x00095D02 File Offset: 0x00093F02
			internal void LoadOptions()
			{
				this.volume = OptionsManager.Load<float>(this.SaveKey, 1f);
			}

			// Token: 0x060028C6 RID: 10438 RVA: 0x00095D1A File Offset: 0x00093F1A
			internal void NotifyOptionsChanged(string key)
			{
				if (key == this.SaveKey)
				{
					this.LoadOptions();
				}
			}

			// Token: 0x04001F26 RID: 7974
			[SerializeField]
			private string volumeRTPC = "Master";

			// Token: 0x04001F27 RID: 7975
			[HideInInspector]
			[SerializeField]
			private float volume = 1f;

			// Token: 0x04001F28 RID: 7976
			[HideInInspector]
			[SerializeField]
			private bool mute;

			// Token: 0x04001F29 RID: 7977
			private float appliedVolume = float.MinValue;
		}

		// Token: 0x02000544 RID: 1348
		public enum FootStepMaterialType
		{
			// Token: 0x04001F2B RID: 7979
			organic,
			// Token: 0x04001F2C RID: 7980
			mech,
			// Token: 0x04001F2D RID: 7981
			danger,
			// Token: 0x04001F2E RID: 7982
			noSound
		}

		// Token: 0x02000545 RID: 1349
		public enum VoiceType
		{
			// Token: 0x04001F30 RID: 7984
			Duck,
			// Token: 0x04001F31 RID: 7985
			Robot,
			// Token: 0x04001F32 RID: 7986
			Wolf,
			// Token: 0x04001F33 RID: 7987
			Chicken,
			// Token: 0x04001F34 RID: 7988
			Crow,
			// Token: 0x04001F35 RID: 7989
			Eagle
		}
	}
}
