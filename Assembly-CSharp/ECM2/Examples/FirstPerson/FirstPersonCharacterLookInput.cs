using System;
using UnityEngine;

namespace ECM2.Examples.FirstPerson
{
	// Token: 0x02000090 RID: 144
	public class FirstPersonCharacterLookInput : MonoBehaviour
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x00012A3E File Offset: 0x00010C3E
		private void Awake()
		{
			this._character = base.GetComponent<FirstPersonCharacter>();
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00012A4C File Offset: 0x00010C4C
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00012A54 File Offset: 0x00010C54
		private void Update()
		{
			Vector2 vector = new Vector2
			{
				x = Input.GetAxisRaw("Mouse X"),
				y = Input.GetAxisRaw("Mouse Y")
			};
			vector *= this.mouseSensitivity;
			this._character.AddControlYawInput(vector.x);
			this._character.AddControlPitchInput(this.invertLook ? (-vector.y) : vector.y, -80f, 80f);
		}

		// Token: 0x040002D6 RID: 726
		[Space(15f)]
		public bool invertLook = true;

		// Token: 0x040002D7 RID: 727
		[Tooltip("Mouse look sensitivity")]
		public Vector2 mouseSensitivity = new Vector2(1f, 1f);

		// Token: 0x040002D8 RID: 728
		[Space(15f)]
		[Tooltip("How far in degrees can you move the camera down.")]
		public float minPitch = -80f;

		// Token: 0x040002D9 RID: 729
		[Tooltip("How far in degrees can you move the camera up.")]
		public float maxPitch = 80f;

		// Token: 0x040002DA RID: 730
		private FirstPersonCharacter _character;
	}
}
