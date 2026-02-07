using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class HeadCollider : MonoBehaviour
{
	// Token: 0x060003AF RID: 943 RVA: 0x00010481 File Offset: 0x0000E681
	public void Init(CharacterMainControl _character)
	{
		this.character = _character;
		this.character.OnTeamChanged += this.OnSetTeam;
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x000104A1 File Offset: 0x0000E6A1
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.OnTeamChanged -= this.OnSetTeam;
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x000104C8 File Offset: 0x0000E6C8
	private void OnSetTeam(Teams team)
	{
		bool enabled = Team.IsEnemy(Teams.player, team);
		this.sphereCollider.enabled = enabled;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x000104EC File Offset: 0x0000E6EC
	private void OnDrawGizmos()
	{
		Color yellow = Color.yellow;
		yellow.a = 0.3f;
		Gizmos.color = yellow;
		Gizmos.DrawSphere(base.transform.position, this.sphereCollider.radius * base.transform.lossyScale.x);
	}

	// Token: 0x040002CB RID: 715
	private CharacterMainControl character;

	// Token: 0x040002CC RID: 716
	[SerializeField]
	private SphereCollider sphereCollider;
}
