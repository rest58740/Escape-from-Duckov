using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000092 RID: 146
	[Category("GameObject")]
	[Description("Note that this is slow.\nAction will end in Failure if no objects are found")]
	public class FindAllWithName : ActionTask
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000280 RID: 640 RVA: 0x00009E96 File Offset: 0x00008096
		protected override string info
		{
			get
			{
				string text = "GetObjects '";
				BBParameter<string> bbparameter = this.searchName;
				string text2 = (bbparameter != null) ? bbparameter.ToString() : null;
				string text3 = "' as ";
				BBParameter<List<GameObject>> bbparameter2 = this.saveAs;
				return text + text2 + text3 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00009ECC File Offset: 0x000080CC
		protected override void OnExecute()
		{
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
			{
				if (gameObject.name == this.searchName.value)
				{
					list.Add(gameObject);
				}
			}
			this.saveAs.value = list;
			base.EndAction(list.Count != 0);
		}

		// Token: 0x040001A5 RID: 421
		[RequiredField]
		public BBParameter<string> searchName = "GameObject";

		// Token: 0x040001A6 RID: 422
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveAs;
	}
}
