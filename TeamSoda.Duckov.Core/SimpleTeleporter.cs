using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x020000B2 RID: 178
public class SimpleTeleporter : InteractableBase
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0001ADC3 File Offset: 0x00018FC3
	public Transform TeleportPoint
	{
		get
		{
			if (!this.selfTeleportPoint)
			{
				return base.transform;
			}
			return this.selfTeleportPoint;
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001ADDF File Offset: 0x00018FDF
	protected override void Awake()
	{
		base.Awake();
		this.teleportVolume.gameObject.SetActive(false);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001ADF8 File Offset: 0x00018FF8
	protected override void OnInteractFinished()
	{
		if (!this.interactCharacter)
		{
			return;
		}
		this.Teleport(this.interactCharacter).Forget();
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001AE1C File Offset: 0x0001901C
	private UniTask Teleport(CharacterMainControl targetCharacter)
	{
		SimpleTeleporter.<Teleport>d__13 <Teleport>d__;
		<Teleport>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Teleport>d__.<>4__this = this;
		<Teleport>d__.targetCharacter = targetCharacter;
		<Teleport>d__.<>1__state = -1;
		<Teleport>d__.<>t__builder.Start<SimpleTeleporter.<Teleport>d__13>(ref <Teleport>d__);
		return <Teleport>d__.<>t__builder.Task;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0001AE68 File Offset: 0x00019068
	private UniTask VolumeFx(bool show, float time)
	{
		SimpleTeleporter.<VolumeFx>d__14 <VolumeFx>d__;
		<VolumeFx>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<VolumeFx>d__.<>4__this = this;
		<VolumeFx>d__.show = show;
		<VolumeFx>d__.time = time;
		<VolumeFx>d__.<>1__state = -1;
		<VolumeFx>d__.<>t__builder.Start<SimpleTeleporter.<VolumeFx>d__14>(ref <VolumeFx>d__);
		return <VolumeFx>d__.<>t__builder.Task;
	}

	// Token: 0x04000587 RID: 1415
	public Transform target;

	// Token: 0x04000588 RID: 1416
	[SerializeField]
	private Transform selfTeleportPoint;

	// Token: 0x04000589 RID: 1417
	[SerializeField]
	private SimpleTeleporter.TransitionTypes transitionType;

	// Token: 0x0400058A RID: 1418
	[FormerlySerializedAs("fxTime")]
	public float transitionTime = 0.28f;

	// Token: 0x0400058B RID: 1419
	private float delay = 0.3f;

	// Token: 0x0400058C RID: 1420
	public Volume teleportVolume;

	// Token: 0x0400058D RID: 1421
	private int fxShaderID = Shader.PropertyToID("TeleportFXStrength");

	// Token: 0x0400058E RID: 1422
	private bool blackScreen;

	// Token: 0x02000477 RID: 1143
	public enum TransitionTypes
	{
		// Token: 0x04001BD5 RID: 7125
		volumeFx,
		// Token: 0x04001BD6 RID: 7126
		blackScreen
	}
}
