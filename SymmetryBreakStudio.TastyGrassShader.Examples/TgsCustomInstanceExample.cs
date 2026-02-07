using System;
using SymmetryBreakStudio.TastyGrassShader;
using UnityEngine;

// Token: 0x02000002 RID: 2
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TgsCustomInstanceExample : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void OnEnable()
	{
		this._instance.Hide = false;
		this.BakeInstance();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
	private void OnValidate()
	{
		this.BakeInstance();
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
	private void BakeInstance()
	{
		if (this.settings.preset == null)
		{
			return;
		}
		if (this.windSettings == null)
		{
			return;
		}
		Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		Bounds bounds = base.GetComponent<MeshRenderer>().bounds;
		TgsInstance.TgsInstanceRecipe bakeParameters = TgsInstance.TgsInstanceRecipe.BakeFromMesh(localToWorldMatrix, this.settings, sharedMesh, bounds);
		this._instance.SetBakeParameters(bakeParameters);
		this._instance.MarkGeometryDirty();
		this._instance.UsedWindSettings = this.windSettings;
		this._instance.MarkMaterialDirty();
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
	private void OnDisable()
	{
		this._instance.Hide = true;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000210E File Offset: 0x0000030E
	private void OnDestroy()
	{
		this._instance.Release();
	}

	// Token: 0x04000001 RID: 1
	private TgsInstance _instance = new TgsInstance();

	// Token: 0x04000002 RID: 2
	public TgsPreset.Settings settings = TgsPreset.Settings.GetDefault();

	// Token: 0x04000003 RID: 3
	public TgsWindSettings windSettings;
}
