using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class AimTargetFinder : MonoBehaviour
{
	// Token: 0x06000473 RID: 1139 RVA: 0x00014AE9 File Offset: 0x00012CE9
	private void Start()
	{
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00014AEC File Offset: 0x00012CEC
	public Transform Find(bool search, Vector3 findPoint, ref CharacterMainControl foundCharacter)
	{
		Transform result = null;
		if (search)
		{
			result = this.Search(findPoint, ref foundCharacter);
		}
		return result;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00014B08 File Offset: 0x00012D08
	private Transform Search(Vector3 findPoint, ref CharacterMainControl character)
	{
		character = null;
		if (this.overlapcColliders == null)
		{
			this.overlapcColliders = new Collider[6];
			this.damageReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
		}
		int num = Physics.OverlapSphereNonAlloc(findPoint, this.searchRadius, this.overlapcColliders, this.damageReceiverLayers);
		Collider collider = null;
		if (num > 0)
		{
			int i = 0;
			while (i < num)
			{
				DamageReceiver component = this.overlapcColliders[i].GetComponent<DamageReceiver>();
				if (!(component == null) && component.Team != Teams.player)
				{
					collider = this.overlapcColliders[i];
					if (component.health != null)
					{
						character = component.health.GetComponent<CharacterMainControl>();
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		if (collider)
		{
			return collider.transform;
		}
		return null;
	}

	// Token: 0x040003E0 RID: 992
	private Vector3 searchPoint;

	// Token: 0x040003E1 RID: 993
	public float searchRadius;

	// Token: 0x040003E2 RID: 994
	private LayerMask damageReceiverLayers;

	// Token: 0x040003E3 RID: 995
	private Collider[] overlapcColliders;
}
