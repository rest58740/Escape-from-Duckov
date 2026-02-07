using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.CreditsUtility
{
	// Token: 0x0200030B RID: 779
	public class CreditsDisplay : MonoBehaviour
	{
		// Token: 0x0600199E RID: 6558 RVA: 0x0005DB94 File Offset: 0x0005BD94
		private void ParseAndDisplay()
		{
			this.Reset();
			CreditsLexer creditsLexer = new CreditsLexer(this.content.text);
			this.BeginVerticalLayout(Array.Empty<string>());
			foreach (Token token in creditsLexer)
			{
				if (this.status.records.Count > 0)
				{
					Token token2 = this.status.records[this.status.records.Count - 1];
				}
				this.status.records.Add(token);
				switch (token.type)
				{
				case TokenType.Invalid:
					Debug.LogError("Invalid Token: " + token.text);
					break;
				case TokenType.End:
					goto IL_F4;
				case TokenType.String:
					this.DoText(token.text);
					break;
				case TokenType.Instructor:
					this.DoInstructor(token.text);
					break;
				case TokenType.EmptyLine:
					this.EndItem();
					break;
				}
			}
			IL_F4:
			this.EndLayout(Array.Empty<string>());
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0005DCB0 File Offset: 0x0005BEB0
		private void EndItem()
		{
			if (this.status.activeItem)
			{
				this.status.activeItem = null;
				this.EndLayout(Array.Empty<string>());
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0005DCDC File Offset: 0x0005BEDC
		private void BeginItem()
		{
			this.status.activeItem = this.BeginVerticalLayout(Array.Empty<string>());
			this.status.activeItem.SetLayoutSpacing(this.internalItemSpacing);
			this.status.activeItem.SetPreferredWidth(this.itemWidth);
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0005DD2B File Offset: 0x0005BF2B
		private void DoEmpty(params string[] elements)
		{
			Object.Instantiate<EmptyEntry>(this.emptyPrefab, this.CurrentTransform).Setup(elements);
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0005DD44 File Offset: 0x0005BF44
		private void DoInstructor(string text)
		{
			string[] array = text.Split(' ', StringSplitOptions.None);
			if (array.Length < 1)
			{
				return;
			}
			string text2 = array[0];
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 3008443898U)
			{
				if (num <= 1811125385U)
				{
					if (num != 1031692888U)
					{
						if (num != 1811125385U)
						{
							return;
						}
						if (!(text2 == "Horizontal"))
						{
							return;
						}
						this.BeginHorizontalLayout(array);
						return;
					}
					else
					{
						if (!(text2 == "color"))
						{
							return;
						}
						this.DoColor(array);
						return;
					}
				}
				else if (num != 2163944795U)
				{
					if (num != 3008443898U)
					{
						return;
					}
					if (!(text2 == "image"))
					{
						return;
					}
					this.DoImage(array);
					return;
				}
				else
				{
					if (!(text2 == "Vertical"))
					{
						return;
					}
					this.BeginVerticalLayout(array);
					return;
				}
			}
			else if (num <= 3482547786U)
			{
				if (num != 3250860581U)
				{
					if (num != 3482547786U)
					{
						return;
					}
					if (!(text2 == "End"))
					{
						return;
					}
					this.EndLayout(Array.Empty<string>());
					return;
				}
				else
				{
					if (!(text2 == "Space"))
					{
						return;
					}
					this.DoEmpty(array);
					return;
				}
			}
			else if (num != 3876335077U)
			{
				if (num != 3909890315U)
				{
					if (num != 4127999362U)
					{
						return;
					}
					if (!(text2 == "s"))
					{
						return;
					}
					this.status.s = true;
					return;
				}
				else
				{
					if (!(text2 == "l"))
					{
						return;
					}
					this.status.l = true;
					return;
				}
			}
			else
			{
				if (!(text2 == "b"))
				{
					return;
				}
				this.status.b = true;
				return;
			}
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0005DEB4 File Offset: 0x0005C0B4
		private void DoImage(string[] elements)
		{
			if (this.status.activeItem == null)
			{
				this.BeginItem();
			}
			Object.Instantiate<ImageEntry>(this.imagePrefab, this.CurrentTransform).Setup(elements);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0005DEE8 File Offset: 0x0005C0E8
		private void DoColor(string[] elements)
		{
			if (elements.Length < 2)
			{
				return;
			}
			Color color;
			ColorUtility.TryParseHtmlString(elements[1], ref color);
			this.status.color = color;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0005DF14 File Offset: 0x0005C114
		private void DoText(string text)
		{
			if (this.status.activeItem == null)
			{
				this.BeginItem();
			}
			TextEntry textEntry = Object.Instantiate<TextEntry>(this.textPrefab, this.CurrentTransform);
			int size = 30;
			if (this.status.s)
			{
				size = 20;
			}
			if (this.status.l)
			{
				size = 40;
			}
			bool b = this.status.b;
			textEntry.Setup(text, this.status.color, size, b);
			this.status.Flush();
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0005DF98 File Offset: 0x0005C198
		private Transform GetCurrentTransform()
		{
			if (this.status == null)
			{
				return this.rootContentTransform;
			}
			if (this.status.transforms.Count == 0)
			{
				return this.rootContentTransform;
			}
			return this.status.transforms.Peek();
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x0005DFD2 File Offset: 0x0005C1D2
		private Transform CurrentTransform
		{
			get
			{
				return this.GetCurrentTransform();
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0005DFDA File Offset: 0x0005C1DA
		public void PushTransform(Transform trans)
		{
			if (this.status == null)
			{
				Debug.LogError("Status not found. Credits Display functions should be called after initialization.", this);
				return;
			}
			this.status.transforms.Push(trans);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0005E004 File Offset: 0x0005C204
		public Transform PopTransform()
		{
			if (this.status == null)
			{
				Debug.LogError("Status not found. Credits Display functions should be called after initialization.", this);
				return null;
			}
			if (this.status.transforms.Count == 0)
			{
				Debug.LogError("Nothing to pop. Makesure to match push and pop.", this);
				return null;
			}
			return this.status.transforms.Pop();
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0005E055 File Offset: 0x0005C255
		private void Awake()
		{
			if (this.setupOnAwake)
			{
				this.ParseAndDisplay();
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0005E068 File Offset: 0x0005C268
		private void Reset()
		{
			while (base.transform.childCount > 0)
			{
				Transform child = base.transform.GetChild(0);
				child.SetParent(null);
				if (Application.isPlaying)
				{
					Object.Destroy(child.gameObject);
				}
				else
				{
					Object.DestroyImmediate(child.gameObject);
				}
			}
			this.status = new CreditsDisplay.GenerationStatus();
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0005E0C4 File Offset: 0x0005C2C4
		private VerticalEntry BeginVerticalLayout(params string[] args)
		{
			VerticalEntry verticalEntry = Object.Instantiate<VerticalEntry>(this.verticalPrefab, this.CurrentTransform);
			verticalEntry.Setup(args);
			verticalEntry.SetLayoutSpacing(this.mainSpacing);
			this.PushTransform(verticalEntry.transform);
			return verticalEntry;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0005E103 File Offset: 0x0005C303
		private void EndLayout(params string[] args)
		{
			if (this.status.activeItem != null)
			{
				this.EndItem();
			}
			this.PopTransform();
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0005E128 File Offset: 0x0005C328
		private HorizontalEntry BeginHorizontalLayout(params string[] args)
		{
			HorizontalEntry horizontalEntry = Object.Instantiate<HorizontalEntry>(this.horizontalPrefab, this.CurrentTransform);
			horizontalEntry.Setup(args);
			this.PushTransform(horizontalEntry.transform);
			return horizontalEntry;
		}

		// Token: 0x040012A3 RID: 4771
		[SerializeField]
		private bool setupOnAwake;

		// Token: 0x040012A4 RID: 4772
		[SerializeField]
		private TextAsset content;

		// Token: 0x040012A5 RID: 4773
		[SerializeField]
		private Transform rootContentTransform;

		// Token: 0x040012A6 RID: 4774
		[SerializeField]
		private float internalItemSpacing = 8f;

		// Token: 0x040012A7 RID: 4775
		[SerializeField]
		private float mainSpacing = 16f;

		// Token: 0x040012A8 RID: 4776
		[SerializeField]
		private float itemWidth = 350f;

		// Token: 0x040012A9 RID: 4777
		[Header("Prefabs")]
		[SerializeField]
		private HorizontalEntry horizontalPrefab;

		// Token: 0x040012AA RID: 4778
		[SerializeField]
		private VerticalEntry verticalPrefab;

		// Token: 0x040012AB RID: 4779
		[SerializeField]
		private EmptyEntry emptyPrefab;

		// Token: 0x040012AC RID: 4780
		[SerializeField]
		private TextEntry textPrefab;

		// Token: 0x040012AD RID: 4781
		[SerializeField]
		private ImageEntry imagePrefab;

		// Token: 0x040012AE RID: 4782
		private CreditsDisplay.GenerationStatus status;

		// Token: 0x020005B1 RID: 1457
		private class GenerationStatus
		{
			// Token: 0x060029B6 RID: 10678 RVA: 0x0009ABB6 File Offset: 0x00098DB6
			public void Flush()
			{
				this.s = false;
				this.l = false;
				this.b = false;
				this.color = Color.white;
			}

			// Token: 0x040020C6 RID: 8390
			public List<Token> records = new List<Token>();

			// Token: 0x040020C7 RID: 8391
			public Stack<Transform> transforms = new Stack<Transform>();

			// Token: 0x040020C8 RID: 8392
			public bool s;

			// Token: 0x040020C9 RID: 8393
			public bool l;

			// Token: 0x040020CA RID: 8394
			public bool b;

			// Token: 0x040020CB RID: 8395
			public Color color = Color.white;

			// Token: 0x040020CC RID: 8396
			public VerticalEntry activeItem;
		}
	}
}
