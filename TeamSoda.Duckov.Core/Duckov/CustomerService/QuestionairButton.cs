using System;
using Duckov.Rules;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.CustomerService
{
	// Token: 0x02000412 RID: 1042
	public class QuestionairButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060025C1 RID: 9665 RVA: 0x000836F8 File Offset: 0x000818F8
		public string GenerateQuestionair()
		{
			SystemLanguage currentLanguage = LocalizationManager.CurrentLanguage;
			string address;
			if (currentLanguage != 22)
			{
				if (currentLanguage == 40)
				{
					address = this.addressCN;
				}
				else
				{
					address = this.addressEN;
				}
			}
			else
			{
				address = this.addressJP;
			}
			int currentSlot = SavesSystem.CurrentSlot;
			string id = string.Format("{0}_{1}", PlatformInfo.Platform, PlatformInfo.GetID());
			string time = string.Format("{0:0}", GameClock.GetRealTimePlayedOfSaveSlot(currentSlot).TotalMinutes);
			string level = string.Format("{0}", EXPManager.Level);
			RuleIndex ruleIndexOfSaveSlot = GameRulesManager.GetRuleIndexOfSaveSlot(currentSlot);
			int num = 0;
			if (ruleIndexOfSaveSlot <= RuleIndex.Easy)
			{
				if (ruleIndexOfSaveSlot != RuleIndex.Standard)
				{
					if (ruleIndexOfSaveSlot != RuleIndex.Custom)
					{
						if (ruleIndexOfSaveSlot == RuleIndex.Easy)
						{
							num = 2;
						}
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 3;
				}
			}
			else if (ruleIndexOfSaveSlot != RuleIndex.ExtraEasy)
			{
				if (ruleIndexOfSaveSlot != RuleIndex.Hard)
				{
					if (ruleIndexOfSaveSlot == RuleIndex.ExtraHard)
					{
						num = 5;
					}
				}
				else
				{
					num = 4;
				}
			}
			else
			{
				num = 1;
			}
			string difficulty = string.Format("{0}", num);
			return StringExtensions.Format(this.format, new
			{
				address,
				id,
				time,
				level,
				difficulty
			});
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x00083802 File Offset: 0x00081A02
		public void OnPointerClick(PointerEventData eventData)
		{
			Application.OpenURL(this.GenerateQuestionair());
		}

		// Token: 0x040019B9 RID: 6585
		private string addressCN = "rsmTLx1";

		// Token: 0x040019BA RID: 6586
		private string addressJP = "mHE3yAa";

		// Token: 0x040019BB RID: 6587
		private string addressEN = "YdoJpod";

		// Token: 0x040019BC RID: 6588
		private string format = "https://usersurvey.biligame.com/vm/{address}.aspx?sojumpparm={id}|{difficulty}|{time}|{level}";
	}
}
