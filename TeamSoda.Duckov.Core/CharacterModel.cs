using System;
using System.Collections.Generic;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class CharacterModel : MonoBehaviour
{
	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000375 RID: 885 RVA: 0x0000F43C File Offset: 0x0000D63C
	// (remove) Token: 0x06000376 RID: 886 RVA: 0x0000F474 File Offset: 0x0000D674
	public event Action<CharacterModel> OnDestroyEvent;

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000377 RID: 887 RVA: 0x0000F4A9 File Offset: 0x0000D6A9
	public Transform LefthandSocket
	{
		get
		{
			return this.lefthandSocket;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000378 RID: 888 RVA: 0x0000F4B1 File Offset: 0x0000D6B1
	public Transform RightHandSocket
	{
		get
		{
			return this.rightHandSocket;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000379 RID: 889 RVA: 0x0000F4B9 File Offset: 0x0000D6B9
	public Transform ArmorSocket
	{
		get
		{
			return this.armorSocket;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x0600037A RID: 890 RVA: 0x0000F4C1 File Offset: 0x0000D6C1
	public Transform HelmatSocket
	{
		get
		{
			return this.helmatSocket;
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x0600037B RID: 891 RVA: 0x0000F4C9 File Offset: 0x0000D6C9
	public Transform FaceMaskSocket
	{
		get
		{
			if (this.faceSocket)
			{
				return this.faceSocket;
			}
			return this.helmatSocket;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x0600037C RID: 892 RVA: 0x0000F4E5 File Offset: 0x0000D6E5
	public Transform BackpackSocket
	{
		get
		{
			return this.backpackSocket;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x0600037D RID: 893 RVA: 0x0000F4ED File Offset: 0x0000D6ED
	public Transform MeleeWeaponSocket
	{
		get
		{
			return this.meleeWeaponSocket;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600037E RID: 894 RVA: 0x0000F4F5 File Offset: 0x0000D6F5
	public Transform PopTextSocket
	{
		get
		{
			return this.popTextSocket;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600037F RID: 895 RVA: 0x0000F4FD File Offset: 0x0000D6FD
	public CustomFaceInstance CustomFace
	{
		get
		{
			return this.customFace;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000380 RID: 896 RVA: 0x0000F505 File Offset: 0x0000D705
	public bool Hidden
	{
		get
		{
			return this.characterMainControl.Hidden;
		}
	}

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000381 RID: 897 RVA: 0x0000F514 File Offset: 0x0000D714
	// (remove) Token: 0x06000382 RID: 898 RVA: 0x0000F54C File Offset: 0x0000D74C
	public event Action OnCharacterSetEvent;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000383 RID: 899 RVA: 0x0000F584 File Offset: 0x0000D784
	// (remove) Token: 0x06000384 RID: 900 RVA: 0x0000F5BC File Offset: 0x0000D7BC
	public event Action OnAttackOrShootEvent;

	// Token: 0x06000385 RID: 901 RVA: 0x0000F5F1 File Offset: 0x0000D7F1
	private void Awake()
	{
		this.defaultRightHandLocalRotation = this.rightHandSocket.localRotation;
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0000F604 File Offset: 0x0000D804
	private void Start()
	{
		CharacterSubVisuals component = base.GetComponent<CharacterSubVisuals>();
		if (component != null)
		{
			if (this.subVisuals.Contains(component))
			{
				this.RemoveVisual(component);
			}
			this.AddSubVisuals(component);
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0000F63D File Offset: 0x0000D83D
	private void LateUpdate()
	{
		if (this.autoSyncRightHandRotation)
		{
			this.SyncRightHandRotation();
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0000F650 File Offset: 0x0000D850
	public void OnMainCharacterSetted(CharacterMainControl _characterMainControl)
	{
		this.characterMainControl = _characterMainControl;
		if (!this.characterMainControl)
		{
			return;
		}
		if (this.characterMainControl.attackAction)
		{
			this.characterMainControl.attackAction.OnAttack += this.OnAttack;
		}
		this.characterMainControl.OnShootEvent += this.OnShoot;
		this.characterMainControl.EquipmentController.OnHelmatSlotContentChanged += this.OnHelmatSlotContentChange;
		this.characterMainControl.EquipmentController.OnFaceMaskSlotContentChanged += this.OnFaceMaskSlotContentChange;
		if (_characterMainControl.mainDamageReceiver != null)
		{
			CapsuleCollider component = _characterMainControl.mainDamageReceiver.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				component.radius = this.damageReceiverRadius;
				if (this.damageReceiverRadius <= 0f)
				{
					component.enabled = false;
				}
			}
		}
		Action onCharacterSetEvent = this.OnCharacterSetEvent;
		if (onCharacterSetEvent != null)
		{
			onCharacterSetEvent();
		}
		this.hurtVisual.SetHealth(_characterMainControl.Health);
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0000F754 File Offset: 0x0000D954
	private void CharacterMainControl_OnShootEvent(DuckovItemAgent obj)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0000F75C File Offset: 0x0000D95C
	private void OnHelmatSlotContentChange(Slot slot)
	{
		if (slot == null)
		{
			return;
		}
		this.helmatShowHair = (slot.Content == null || slot.Content.Constants.GetBool(this.showHairHash, false));
		this.helmatShowMouth = (slot.Content == null || slot.Content.Constants.GetBool(this.showMouthHash, true));
		if (this.customFace && this.customFace.hairSocket)
		{
			this.customFace.hairSocket.gameObject.SetActive(this.helmatShowHair && this.faceMaskShowHair);
		}
		if (this.customFace && this.customFace.mouthPart.socket)
		{
			this.customFace.mouthPart.socket.gameObject.SetActive(this.helmatShowMouth && this.faceMaskShowMouth);
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0000F860 File Offset: 0x0000DA60
	private void OnFaceMaskSlotContentChange(Slot slot)
	{
		if (slot == null)
		{
			return;
		}
		this.faceMaskShowHair = (slot.Content == null || slot.Content.Constants.GetBool(this.showHairHash, true));
		this.faceMaskShowMouth = (slot.Content == null || slot.Content.Constants.GetBool(this.showMouthHash, true));
		if (this.customFace && this.customFace.hairSocket)
		{
			this.customFace.hairSocket.gameObject.SetActive(this.helmatShowHair && this.faceMaskShowHair);
		}
		if (this.customFace && this.customFace.mouthPart.socket)
		{
			this.customFace.mouthPart.socket.gameObject.SetActive(this.helmatShowMouth && this.faceMaskShowMouth);
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0000F964 File Offset: 0x0000DB64
	private void OnDestroy()
	{
		if (this.destroied)
		{
			return;
		}
		this.destroied = true;
		Action<CharacterModel> onDestroyEvent = this.OnDestroyEvent;
		if (onDestroyEvent != null)
		{
			onDestroyEvent(this);
		}
		if (this.characterMainControl)
		{
			if (this.characterMainControl.attackAction)
			{
				this.characterMainControl.attackAction.OnAttack -= this.OnAttack;
			}
			this.characterMainControl.OnShootEvent -= this.OnShoot;
			this.characterMainControl.EquipmentController.OnHelmatSlotContentChanged -= this.OnHelmatSlotContentChange;
			this.characterMainControl.EquipmentController.OnFaceMaskSlotContentChanged -= this.OnFaceMaskSlotContentChange;
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0000FA20 File Offset: 0x0000DC20
	private void SyncRightHandRotation()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		if (this.characterMainControl.Running)
		{
			flag = false;
		}
		Quaternion quaternion;
		if (flag)
		{
			quaternion = Quaternion.LookRotation(this.characterMainControl.CurrentAimDirection, Vector3.up);
		}
		else
		{
			quaternion = this.rightHandSocket.parent.transform.rotation * this.defaultRightHandLocalRotation;
		}
		float num = 999f;
		if (!flag2)
		{
			num = 360f * Time.deltaTime;
		}
		this.rightHandSocket.rotation = Quaternion.RotateTowards(this.rightHandSocket.rotation, quaternion, num);
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0000FABC File Offset: 0x0000DCBC
	public void AddSubVisuals(CharacterSubVisuals visuals)
	{
		visuals.mainModel = this;
		if (this.subVisuals.Contains(visuals))
		{
			return;
		}
		this.subVisuals.Add(visuals);
		this.renderers.AddRange(visuals.renderers);
		this.hurtVisual.SetRenderers(this.renderers);
		visuals.SetRenderersHidden(this.Hidden);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0000FB1C File Offset: 0x0000DD1C
	public void RemoveVisual(CharacterSubVisuals _subVisuals)
	{
		this.subVisuals.Remove(_subVisuals);
		foreach (Renderer item in _subVisuals.renderers)
		{
			this.renderers.Remove(item);
		}
		this.hurtVisual.SetRenderers(this.renderers);
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0000FB94 File Offset: 0x0000DD94
	public void SyncHiddenToMainCharacter()
	{
		bool renderersHidden = this.Hidden;
		if (!Team.IsEnemy(Teams.player, this.characterMainControl.Team))
		{
			renderersHidden = false;
		}
		if (this.subVisuals.Count > 0)
		{
			foreach (CharacterSubVisuals characterSubVisuals in this.subVisuals)
			{
				if (!(characterSubVisuals == null))
				{
					characterSubVisuals.SetRenderersHidden(renderersHidden);
				}
			}
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0000FC1C File Offset: 0x0000DE1C
	public void SetFaceFromPreset(CustomFacePreset preset)
	{
		if (preset == null)
		{
			return;
		}
		if (!this.customFace)
		{
			return;
		}
		this.customFace.LoadFromData(preset.settings);
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0000FC47 File Offset: 0x0000DE47
	public void SetFaceFromData(CustomFaceSettingData data)
	{
		if (!this.customFace)
		{
			return;
		}
		this.customFace.LoadFromData(data);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0000FC63 File Offset: 0x0000DE63
	private void OnAttack()
	{
		Action onAttackOrShootEvent = this.OnAttackOrShootEvent;
		if (onAttackOrShootEvent == null)
		{
			return;
		}
		onAttackOrShootEvent();
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0000FC75 File Offset: 0x0000DE75
	public void ForcePlayAttackAnimation()
	{
		this.OnAttack();
	}

	// Token: 0x06000395 RID: 917 RVA: 0x0000FC7D File Offset: 0x0000DE7D
	private void OnShoot(DuckovItemAgent agent)
	{
		Action onAttackOrShootEvent = this.OnAttackOrShootEvent;
		if (onAttackOrShootEvent == null)
		{
			return;
		}
		onAttackOrShootEvent();
	}

	// Token: 0x04000295 RID: 661
	public CharacterMainControl characterMainControl;

	// Token: 0x04000296 RID: 662
	public bool invisable;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	private Transform lefthandSocket;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	private Transform rightHandSocket;

	// Token: 0x0400029A RID: 666
	private Quaternion defaultRightHandLocalRotation;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	private HurtVisual hurtVisual;

	// Token: 0x0400029C RID: 668
	[SerializeField]
	private Transform armorSocket;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private Transform helmatSocket;

	// Token: 0x0400029E RID: 670
	[SerializeField]
	private Transform faceSocket;

	// Token: 0x0400029F RID: 671
	[SerializeField]
	private Transform backpackSocket;

	// Token: 0x040002A0 RID: 672
	[SerializeField]
	private Transform meleeWeaponSocket;

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	private Transform popTextSocket;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private List<CharacterSubVisuals> subVisuals;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private List<Renderer> renderers;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private CustomFaceInstance customFace;

	// Token: 0x040002A5 RID: 677
	public bool autoSyncRightHandRotation = true;

	// Token: 0x040002A6 RID: 678
	public float damageReceiverRadius = 0.45f;

	// Token: 0x040002A7 RID: 679
	public float attackFxZOffset;

	// Token: 0x040002A8 RID: 680
	private int showHairHash = "ShowHair".GetHashCode();

	// Token: 0x040002A9 RID: 681
	private int showMouthHash = "ShowMouth".GetHashCode();

	// Token: 0x040002AC RID: 684
	private bool helmatShowMouth = true;

	// Token: 0x040002AD RID: 685
	private bool helmatShowHair = true;

	// Token: 0x040002AE RID: 686
	private bool faceMaskShowHair = true;

	// Token: 0x040002AF RID: 687
	private bool faceMaskShowMouth = true;

	// Token: 0x040002B0 RID: 688
	private bool destroied;
}
