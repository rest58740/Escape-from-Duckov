using System;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.CreditsUtility
{
	// Token: 0x02000311 RID: 785
	public class ImageEntry : MonoBehaviour
	{
		// Token: 0x060019BE RID: 6590 RVA: 0x0005E5E4 File Offset: 0x0005C7E4
		internal void Setup(string[] elements)
		{
			if (elements.Length < 2)
			{
				return;
			}
			for (int i = 0; i < elements.Length; i++)
			{
				float preferredWidth;
				if (i == 1)
				{
					string text = elements[1];
					Sprite sprite = GameplayDataSettings.GetSprite(text);
					if (sprite == null)
					{
						Debug.LogError("Cannot find sprite:" + text);
					}
					else
					{
						this.image.sprite = sprite;
					}
				}
				else if (i == 2)
				{
					float preferredHeight;
					if (float.TryParse(elements[2], out preferredHeight))
					{
						this.layoutElement.preferredHeight = preferredHeight;
					}
				}
				else if (i == 3 && float.TryParse(elements[2], out preferredWidth))
				{
					this.layoutElement.preferredWidth = preferredWidth;
				}
			}
		}

		// Token: 0x040012BE RID: 4798
		[SerializeField]
		private Image image;

		// Token: 0x040012BF RID: 4799
		[SerializeField]
		private LayoutElement layoutElement;
	}
}
