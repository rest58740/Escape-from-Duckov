using System;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x02000277 RID: 631
	public abstract class ModBehaviour : MonoBehaviour
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x0004A72C File Offset: 0x0004892C
		// (set) Token: 0x060013D8 RID: 5080 RVA: 0x0004A734 File Offset: 0x00048934
		public ModManager master { get; private set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x0004A73D File Offset: 0x0004893D
		// (set) Token: 0x060013DA RID: 5082 RVA: 0x0004A745 File Offset: 0x00048945
		public ModInfo info { get; private set; }

		// Token: 0x060013DB RID: 5083 RVA: 0x0004A74E File Offset: 0x0004894E
		public void Setup(ModManager master, ModInfo info)
		{
			this.master = master;
			this.info = info;
			this.OnAfterSetup();
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0004A764 File Offset: 0x00048964
		public void NotifyBeforeDeactivate()
		{
			this.OnBeforeDeactivate();
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0004A76C File Offset: 0x0004896C
		protected virtual void OnAfterSetup()
		{
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0004A76E File Offset: 0x0004896E
		protected virtual void OnBeforeDeactivate()
		{
		}
	}
}
