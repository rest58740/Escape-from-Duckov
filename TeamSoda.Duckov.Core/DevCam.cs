using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001BF RID: 447
public class DevCam : MonoBehaviour
{
	// Token: 0x06000D6A RID: 3434 RVA: 0x00038515 File Offset: 0x00036715
	private void Awake()
	{
		this.root.gameObject.SetActive(false);
		Shader.SetGlobalFloat("DevCamOn", 0f);
		DevCam.devCamOn = false;
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00038540 File Offset: 0x00036740
	private void Toggle()
	{
		this.active = true;
		DevCam.devCamOn = this.active;
		Shader.SetGlobalFloat("DevCamOn", this.active ? 1f : 0f);
		this.root.gameObject.SetActive(this.active);
		for (int i = 0; i < Display.displays.Length; i++)
		{
			if (i == 1 && this.active)
			{
				Display.displays[i].Activate();
			}
		}
		UniversalRenderPipelineAsset universalRenderPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.shadowDistance = 500f;
		}
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x000385DC File Offset: 0x000367DC
	private void OnDestroy()
	{
		DevCam.devCamOn = false;
	}

	// Token: 0x06000D6D RID: 3437 RVA: 0x000385E4 File Offset: 0x000367E4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (Gamepad.all.Count <= 0)
		{
			return;
		}
		this.timer -= Time.deltaTime;
		if (this.timer <= 0f)
		{
			this.timer = 0f;
			this.pressCounter = 0;
		}
		if (Gamepad.current.leftStickButton.isPressed && Gamepad.current.rightStickButton.wasPressedThisFrame)
		{
			this.pressCounter++;
			this.timer = 1.5f;
			Debug.Log("Toggle Dev Cam");
			if (this.pressCounter >= 2)
			{
				this.pressCounter = 0;
				this.Toggle();
			}
		}
		if (CharacterMainControl.Main != null)
		{
			this.postTarget.position = CharacterMainControl.Main.transform.position;
		}
	}

	// Token: 0x04000B9E RID: 2974
	public Camera devCamera;

	// Token: 0x04000B9F RID: 2975
	public Transform postTarget;

	// Token: 0x04000BA0 RID: 2976
	private bool active;

	// Token: 0x04000BA1 RID: 2977
	public Transform root;

	// Token: 0x04000BA2 RID: 2978
	public static bool devCamOn;

	// Token: 0x04000BA3 RID: 2979
	private float timer = 1.5f;

	// Token: 0x04000BA4 RID: 2980
	private int pressCounter;
}
