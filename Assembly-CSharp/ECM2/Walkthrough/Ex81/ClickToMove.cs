using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex81
{
	// Token: 0x02000066 RID: 102
	public class ClickToMove : MonoBehaviour
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000EFD5 File Offset: 0x0000D1D5
		private void Awake()
		{
			this._navMeshCharacter = this.character.GetComponent<NavMeshCharacter>();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
		private void Update()
		{
			RaycastHit raycastHit;
			if (Input.GetMouseButton(0) && Physics.Raycast(this.mainCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity, this.groundMask))
			{
				this._navMeshCharacter.MoveToDestination(raycastHit.point);
			}
		}

		// Token: 0x04000256 RID: 598
		public Camera mainCamera;

		// Token: 0x04000257 RID: 599
		public Character character;

		// Token: 0x04000258 RID: 600
		public LayerMask groundMask;

		// Token: 0x04000259 RID: 601
		private NavMeshCharacter _navMeshCharacter;
	}
}
