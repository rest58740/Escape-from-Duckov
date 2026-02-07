using System;
using UnityEngine;

namespace HardSlashes
{
	// Token: 0x0200005E RID: 94
	public class DemoManager : MonoBehaviour
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0000DE4C File Offset: 0x0000C04C
		private void Start()
		{
			this.text_fx_name.text = "[" + (this.index_fx + 1).ToString() + "] " + this.prefabs[this.index_fx].name;
			UnityEngine.Object.Destroy(GameObject.Find("Instructions"), 11.5f);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(this.ray.origin, this.ray.direction, out this.ray_cast_hit, 1000f))
				{
					this.Aim();
					this.current_animation = UnityEngine.Object.Instantiate<GameObject>(this.prefabs[this.index_fx], this.ray_cast_hit.point, base.transform.rotation);
				}
			}
			if (Input.GetKeyDown("z") || Input.GetKeyDown("left"))
			{
				this.index_fx--;
				if (this.index_fx <= -1)
				{
					this.index_fx = this.prefabs.Length - 1;
				}
				this.text_fx_name.text = "[" + (this.index_fx + 1).ToString() + "] " + this.prefabs[this.index_fx].name;
			}
			if (Input.GetKeyDown("x") || Input.GetKeyDown("right"))
			{
				this.index_fx++;
				if (this.index_fx >= this.prefabs.Length)
				{
					this.index_fx = 0;
				}
				this.text_fx_name.text = "[" + (this.index_fx + 1).ToString() + "] " + this.prefabs[this.index_fx].name;
			}
			if (Input.GetKeyDown("space"))
			{
				Debug.Break();
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E038 File Offset: 0x0000C238
		private void Aim()
		{
			base.transform.LookAt(new Vector3(this.ray_cast_hit.point.x, 1f, this.ray_cast_hit.point.z));
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y + 180f, base.transform.eulerAngles.z);
		}

		// Token: 0x0400021E RID: 542
		public TextMesh text_fx_name;

		// Token: 0x0400021F RID: 543
		public GameObject[] prefabs;

		// Token: 0x04000220 RID: 544
		public int index_fx;

		// Token: 0x04000221 RID: 545
		private GameObject current_animation;

		// Token: 0x04000222 RID: 546
		private Ray ray;

		// Token: 0x04000223 RID: 547
		private RaycastHit ray_cast_hit;
	}
}
