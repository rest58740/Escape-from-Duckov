using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MeshCombineStudio
{
	// Token: 0x0200000E RID: 14
	public class StreamedSceneCombiner : MonoBehaviour
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00003168 File Offset: 0x00001368
		private void Start()
		{
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				RootSceneCombiner rootSceneCombiner;
				MeshCombiner meshCombiner;
				if (rootGameObjects[i].TryGetComponent<RootSceneCombiner>(out rootSceneCombiner) && rootSceneCombiner.MCSGameObject != null && rootSceneCombiner.MCSGameObject.TryGetComponent<MeshCombiner>(out meshCombiner))
				{
					MeshCombiner component = UnityEngine.Object.Instantiate<MeshCombiner>(rootSceneCombiner.MCSGameObject, base.transform.parent).GetComponent<MeshCombiner>();
					component.searchOptions.parentGOs = new GameObject[]
					{
						base.gameObject
					};
					component.CombineAll(true);
					return;
				}
			}
		}
	}
}
