using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200012D RID: 301
public class LoadUnitySceneOnStart : MonoBehaviour
{
	// Token: 0x060009FE RID: 2558 RVA: 0x0002BB37 File Offset: 0x00029D37
	private void Start()
	{
		SceneManager.LoadScene(this.sceneIndex);
	}

	// Token: 0x040008D8 RID: 2264
	public int sceneIndex;
}
