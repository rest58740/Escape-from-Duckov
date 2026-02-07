using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CharacterSubVisuals : MonoBehaviour
{
	// Token: 0x0600039E RID: 926 RVA: 0x0000FF3C File Offset: 0x0000E13C
	private void InitLayers()
	{
		if (this.layerInited)
		{
			return;
		}
		this.layerInited = true;
		this.hiddenLayer = LayerMask.NameToLayer("SpecialCamera");
		this.showLayer = LayerMask.NameToLayer("Character");
		this.sodaLightShowLayer = LayerMask.NameToLayer("SodaLight");
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0000FF8C File Offset: 0x0000E18C
	public void SetRenderers()
	{
		this.renderers.Clear();
		this.particles.Clear();
		this.lights.Clear();
		this.sodaPointLights.Clear();
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>(true))
		{
			ParticleSystem component = renderer.GetComponent<ParticleSystem>();
			if (component)
			{
				this.particles.Add(component);
			}
			else
			{
				SodaPointLight component2 = renderer.GetComponent<SodaPointLight>();
				if (component2)
				{
					this.sodaPointLights.Add(component2);
				}
				else
				{
					this.renderers.Add(renderer);
				}
			}
		}
		foreach (Light item in base.GetComponentsInChildren<Light>(true))
		{
			this.lights.Add(item);
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00010054 File Offset: 0x0000E254
	public void AddRenderer(Renderer renderer)
	{
		if (renderer == null || this.renderers.Contains(renderer))
		{
			return;
		}
		this.InitLayers();
		int layer = this.hidden ? this.hiddenLayer : this.showLayer;
		renderer.gameObject.layer = layer;
		this.renderers.Add(renderer);
		if (this.character)
		{
			this.character.RemoveVisual(this);
			this.character.AddSubVisuals(this);
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000100D4 File Offset: 0x0000E2D4
	public void SetRenderersHidden(bool _hidden)
	{
		this.hidden = _hidden;
		this.InitLayers();
		int layer = _hidden ? this.hiddenLayer : this.showLayer;
		int num = this.renderers.Count;
		for (int i = 0; i < num; i++)
		{
			if (this.renderers[i] == null)
			{
				this.renderers.RemoveAt(i);
				i--;
				num--;
			}
			else
			{
				this.renderers[i].gameObject.layer = layer;
			}
		}
		int num2 = this.particles.Count;
		for (int j = 0; j < num2; j++)
		{
			if (this.particles[j] == null)
			{
				this.particles.RemoveAt(j);
				j--;
				num2--;
			}
			else
			{
				this.particles[j].gameObject.layer = layer;
			}
		}
		int num3 = this.lights.Count;
		for (int k = 0; k < num3; k++)
		{
			Light light = this.lights[k];
			if (light == null)
			{
				this.lights.RemoveAt(k);
				k--;
				num3--;
			}
			else
			{
				light.gameObject.layer = layer;
				if (this.hidden)
				{
					light.cullingMask = 0;
				}
				else
				{
					light.cullingMask = -1;
				}
			}
		}
		int layer2 = _hidden ? this.hiddenLayer : this.sodaLightShowLayer;
		int num4 = this.sodaPointLights.Count;
		for (int l = 0; l < this.sodaPointLights.Count; l++)
		{
			if (this.sodaPointLights[l] == null)
			{
				this.sodaPointLights.RemoveAt(l);
				l--;
				num4--;
			}
			else
			{
				this.sodaPointLights[l].gameObject.layer = layer2;
			}
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000102BC File Offset: 0x0000E4BC
	private void OnTransformParentChanged()
	{
		CharacterMainControl componentInParent = base.GetComponentInParent<CharacterMainControl>(true);
		this.SetCharacter(componentInParent);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x000102D8 File Offset: 0x0000E4D8
	public void SetCharacter(CharacterMainControl newCharacter)
	{
		if (newCharacter != null)
		{
			this.character = newCharacter;
			newCharacter.AddSubVisuals(this);
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x000102F1 File Offset: 0x0000E4F1
	private void OnDestroy()
	{
		if (this.character != null)
		{
			this.character.RemoveVisual(this);
		}
	}

	// Token: 0x040002B6 RID: 694
	private CharacterMainControl character;

	// Token: 0x040002B7 RID: 695
	public List<Renderer> renderers = new List<Renderer>();

	// Token: 0x040002B8 RID: 696
	public List<ParticleSystem> particles = new List<ParticleSystem>();

	// Token: 0x040002B9 RID: 697
	public List<Light> lights = new List<Light>();

	// Token: 0x040002BA RID: 698
	public List<SodaPointLight> sodaPointLights = new List<SodaPointLight>();

	// Token: 0x040002BB RID: 699
	private int hiddenLayer;

	// Token: 0x040002BC RID: 700
	private int showLayer;

	// Token: 0x040002BD RID: 701
	private int sodaLightShowLayer;

	// Token: 0x040002BE RID: 702
	private bool hidden;

	// Token: 0x040002BF RID: 703
	private bool layerInited;

	// Token: 0x040002C0 RID: 704
	public bool logWhenSetVisual;

	// Token: 0x040002C1 RID: 705
	public CharacterModel mainModel;

	// Token: 0x040002C2 RID: 706
	public bool debug;
}
