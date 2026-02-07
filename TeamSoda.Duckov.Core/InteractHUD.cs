using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class InteractHUD : MonoBehaviour
{
	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001C9CC File Offset: 0x0001ABCC
	private PrefabPool<InteractSelectionHUD> Selections
	{
		get
		{
			if (this._selectionsCache == null)
			{
				this._selectionsCache = new PrefabPool<InteractSelectionHUD>(this.selectionPrefab, null, null, null, null, true, 10, 10000, null);
			}
			return this._selectionsCache;
		}
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0001CA05 File Offset: 0x0001AC05
	private void Awake()
	{
		this.interactableGroup = new List<InteractableBase>();
		this.selectionsHUD = new List<InteractSelectionHUD>();
		this.selectionPrefab.gameObject.SetActive(false);
		this.master.gameObject.SetActive(false);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0001CA40 File Offset: 0x0001AC40
	private void Update()
	{
		if (this.characterMainControl == null)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl == null)
			{
				return;
			}
		}
		if (this.camera == null)
		{
			this.camera = Camera.main;
			if (this.camera == null)
			{
				return;
			}
		}
		bool flag = false;
		bool flag2 = false;
		this.interactableMaster = this.characterMainControl.interactAction.MasterInteractableAround;
		bool flag3 = InputManager.InputActived && (!this.characterMainControl.CurrentAction || !this.characterMainControl.CurrentAction.Running);
		Shader.SetGlobalFloat(this.interactableHash, flag3 ? 1f : 0f);
		this.interactable = (this.interactableMaster != null && flag3);
		if (this.interactable)
		{
			if (this.interactableMaster != this.interactableMasterTemp)
			{
				this.interactableMasterTemp = this.interactableMaster;
				flag = true;
				flag2 = true;
			}
			if (this.interactableIndexTemp != this.characterMainControl.interactAction.InteractIndexInGroup)
			{
				this.interactableIndexTemp = this.characterMainControl.interactAction.InteractIndexInGroup;
				flag2 = true;
			}
		}
		else
		{
			this.interactableMasterTemp = null;
		}
		if (this.interactable != this.master.gameObject.activeInHierarchy)
		{
			this.master.gameObject.SetActive(this.interactable);
		}
		if (flag)
		{
			this.RefreshContent();
			this.SyncPos();
		}
		if (flag2)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0001CBC7 File Offset: 0x0001ADC7
	private void LateUpdate()
	{
		if (this.characterMainControl == null)
		{
			return;
		}
		if (this.camera == null)
		{
			return;
		}
		this.SyncPos();
		this.UpdateInteractLine();
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0001CBF4 File Offset: 0x0001ADF4
	private void SyncPos()
	{
		if (!this.syncPosToTarget)
		{
			return;
		}
		if (!this.interactableMaster)
		{
			return;
		}
		Vector3 vector = this.interactableMaster.transform.TransformPoint(this.interactableMaster.interactMarkerOffset);
		Vector3 vector2 = LevelManager.Instance.GameCamera.renderCamera.WorldToScreenPoint(vector);
		Vector2 vector3;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, vector2, null, ref vector3);
		base.transform.localPosition = vector3;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0001CC7C File Offset: 0x0001AE7C
	private void RefreshContent()
	{
		if (this.interactableMaster == null)
		{
			return;
		}
		this.selectionsHUD.Clear();
		this.interactableGroup.Clear();
		foreach (InteractableBase interactableBase in this.interactableMaster.GetInteractableList())
		{
			if (interactableBase != null)
			{
				this.interactableGroup.Add(interactableBase);
			}
		}
		this.Selections.ReleaseAll();
		foreach (InteractableBase interactableBase2 in this.interactableGroup)
		{
			InteractSelectionHUD interactSelectionHUD = this.Selections.Get(null);
			interactSelectionHUD.transform.SetAsLastSibling();
			interactSelectionHUD.SetInteractable(interactableBase2, this.interactableGroup.Count > 1);
			this.selectionsHUD.Add(interactSelectionHUD);
		}
		this.master.ForceUpdateRectTransforms();
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x0001CD94 File Offset: 0x0001AF94
	private void RefreshSelection()
	{
		InteractableBase interactTarget = this.characterMainControl.interactAction.InteractTarget;
		foreach (InteractSelectionHUD interactSelectionHUD in this.selectionsHUD)
		{
			if (interactSelectionHUD.InteractTarget == interactTarget)
			{
				interactSelectionHUD.SetSelection(true);
			}
			else
			{
				interactSelectionHUD.SetSelection(false);
			}
		}
		this.master.ForceUpdateRectTransforms();
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0001CE1C File Offset: 0x0001B01C
	private void UpdateInteractLine()
	{
	}

	// Token: 0x0400060F RID: 1551
	private CharacterMainControl characterMainControl;

	// Token: 0x04000610 RID: 1552
	public RectTransform master;

	// Token: 0x04000611 RID: 1553
	private InteractableBase interactableMaster;

	// Token: 0x04000612 RID: 1554
	private InteractableBase interactableMasterTemp;

	// Token: 0x04000613 RID: 1555
	private List<InteractableBase> interactableGroup;

	// Token: 0x04000614 RID: 1556
	private List<InteractSelectionHUD> selectionsHUD;

	// Token: 0x04000615 RID: 1557
	private int interactableIndexTemp;

	// Token: 0x04000616 RID: 1558
	private bool interactable;

	// Token: 0x04000617 RID: 1559
	private Camera camera;

	// Token: 0x04000618 RID: 1560
	public bool syncPosToTarget;

	// Token: 0x04000619 RID: 1561
	public InteractSelectionHUD selectionPrefab;

	// Token: 0x0400061A RID: 1562
	private int interactableHash = Shader.PropertyToID("Interactable");

	// Token: 0x0400061B RID: 1563
	private PrefabPool<InteractSelectionHUD> _selectionsCache;
}
