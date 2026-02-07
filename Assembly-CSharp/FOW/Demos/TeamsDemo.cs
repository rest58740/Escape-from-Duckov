using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FOW.Demos
{
	// Token: 0x02000062 RID: 98
	public class TeamsDemo : MonoBehaviour
	{
		// Token: 0x06000343 RID: 835 RVA: 0x0000E559 File Offset: 0x0000C759
		private void Awake()
		{
			this.team = 2;
			this.changeTeams();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000E568 File Offset: 0x0000C768
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.changeTeams();
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E57C File Offset: 0x0000C77C
		private void changeTeams()
		{
			this.team++;
			this.team %= 3;
			this.teamText.text = string.Format("VIEWING AS TEAM {0}", this.team + 1);
			foreach (FogOfWarRevealer fogOfWarRevealer in this.team1Members)
			{
				fogOfWarRevealer.enabled = false;
				fogOfWarRevealer.GetComponent<FogOfWarHider>().enabled = true;
			}
			foreach (FogOfWarRevealer fogOfWarRevealer2 in this.team2Members)
			{
				fogOfWarRevealer2.enabled = false;
				fogOfWarRevealer2.GetComponent<FogOfWarHider>().enabled = true;
			}
			foreach (FogOfWarRevealer fogOfWarRevealer3 in this.team3Members)
			{
				fogOfWarRevealer3.enabled = false;
				fogOfWarRevealer3.GetComponent<FogOfWarHider>().enabled = true;
			}
			switch (this.team)
			{
			case 0:
				this.teamText.color = this.team1Color;
				using (List<FogOfWarRevealer>.Enumerator enumerator = this.team1Members.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FogOfWarRevealer fogOfWarRevealer4 = enumerator.Current;
						fogOfWarRevealer4.enabled = true;
						fogOfWarRevealer4.GetComponent<FogOfWarHider>().enabled = false;
					}
					return;
				}
				break;
			case 1:
				break;
			case 2:
				goto IL_1BD;
			default:
				return;
			}
			this.teamText.color = this.team2Color;
			using (List<FogOfWarRevealer>.Enumerator enumerator = this.team2Members.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FogOfWarRevealer fogOfWarRevealer5 = enumerator.Current;
					fogOfWarRevealer5.enabled = true;
					fogOfWarRevealer5.GetComponent<FogOfWarHider>().enabled = false;
				}
				return;
			}
			IL_1BD:
			this.teamText.color = this.team3Color;
			foreach (FogOfWarRevealer fogOfWarRevealer6 in this.team3Members)
			{
				fogOfWarRevealer6.enabled = true;
				fogOfWarRevealer6.GetComponent<FogOfWarHider>().enabled = false;
			}
		}

		// Token: 0x04000231 RID: 561
		public Text teamText;

		// Token: 0x04000232 RID: 562
		public Color team1Color = Color.blue;

		// Token: 0x04000233 RID: 563
		public List<FogOfWarRevealer> team1Members = new List<FogOfWarRevealer>();

		// Token: 0x04000234 RID: 564
		public Color team2Color = Color.green;

		// Token: 0x04000235 RID: 565
		public List<FogOfWarRevealer> team2Members = new List<FogOfWarRevealer>();

		// Token: 0x04000236 RID: 566
		public Color team3Color = Color.red;

		// Token: 0x04000237 RID: 567
		public List<FogOfWarRevealer> team3Members = new List<FogOfWarRevealer>();

		// Token: 0x04000238 RID: 568
		private int team;
	}
}
