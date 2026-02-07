using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sirenix.OdinInspector
{
	// Token: 0x0200008E RID: 142
	public static class SelfValidationResultItemExtensions
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x000041D1 File Offset: 0x000023D1
		public static ref SelfValidationResult.ResultItem WithFix(this SelfValidationResult.ResultItem item, string title, Action fix, bool offerInInspector = true)
		{
			item.Fix = new SelfFix?(SelfFix.Create(title, fix, offerInInspector));
			return ref item;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000041E7 File Offset: 0x000023E7
		public static ref SelfValidationResult.ResultItem WithFix<T>(this SelfValidationResult.ResultItem item, string title, Action<T> fix, bool offerInInspector = true) where T : new()
		{
			item.Fix = new SelfFix?(SelfFix.Create<T>(title, fix, offerInInspector));
			return ref item;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000041FD File Offset: 0x000023FD
		public static ref SelfValidationResult.ResultItem WithFix(this SelfValidationResult.ResultItem item, Action fix, bool offerInInspector = true)
		{
			item.Fix = new SelfFix?(SelfFix.Create(fix, offerInInspector));
			return ref item;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00004212 File Offset: 0x00002412
		public static ref SelfValidationResult.ResultItem WithFix<T>(this SelfValidationResult.ResultItem item, Action<T> fix, bool offerInInspector = true) where T : new()
		{
			item.Fix = new SelfFix?(SelfFix.Create<T>(fix, offerInInspector));
			return ref item;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00004227 File Offset: 0x00002427
		public static ref SelfValidationResult.ResultItem WithFix(this SelfValidationResult.ResultItem item, SelfFix fix)
		{
			item.Fix = new SelfFix?(fix);
			return ref item;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00004236 File Offset: 0x00002436
		public static ref SelfValidationResult.ResultItem WithContextClick(this SelfValidationResult.ResultItem item, Func<IEnumerable<SelfValidationResult.ContextMenuItem>> onContextClick)
		{
			item.OnContextClick = onContextClick;
			return ref item;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00004240 File Offset: 0x00002440
		public static ref SelfValidationResult.ResultItem WithContextClick(this SelfValidationResult.ResultItem item, string path, Action onClick)
		{
			item.OnContextClick = (() => new SelfValidationResult.ContextMenuItem[]
			{
				new SelfValidationResult.ContextMenuItem
				{
					Path = path,
					OnClick = onClick
				}
			});
			return ref item;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00004274 File Offset: 0x00002474
		public static ref SelfValidationResult.ResultItem WithContextClick(this SelfValidationResult.ResultItem item, string path, bool on, Action onClick)
		{
			item.OnContextClick = (() => new SelfValidationResult.ContextMenuItem[]
			{
				new SelfValidationResult.ContextMenuItem
				{
					Path = path,
					On = on,
					OnClick = onClick
				}
			});
			return ref item;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000042B0 File Offset: 0x000024B0
		public static ref SelfValidationResult.ResultItem WithContextClick(this SelfValidationResult.ResultItem item, SelfValidationResult.ContextMenuItem onContextClick)
		{
			item.OnContextClick = (() => new SelfValidationResult.ContextMenuItem[]
			{
				onContextClick
			});
			return ref item;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000042DD File Offset: 0x000024DD
		public static ref SelfValidationResult.ResultItem WithSceneGUI(this SelfValidationResult.ResultItem item, Action onSceneGUI)
		{
			item.OnSceneGUI = onSceneGUI;
			return ref item;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000042E7 File Offset: 0x000024E7
		public static ref SelfValidationResult.ResultItem SetSelectionObject(this SelfValidationResult.ResultItem item, Object uObj)
		{
			item.SelectionObject = uObj;
			return ref item;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000042F1 File Offset: 0x000024F1
		public static ref SelfValidationResult.ResultItem EnableRichText(this SelfValidationResult.ResultItem item)
		{
			item.RichText = true;
			return ref item;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000042FC File Offset: 0x000024FC
		public static ref SelfValidationResult.ResultItem WithMetaData(this SelfValidationResult.ResultItem resultItem, string name, object value, params Attribute[] attributes)
		{
			resultItem.MetaData = (resultItem.MetaData ?? new SelfValidationResult.ResultItemMetaData[0]);
			Array.Resize<SelfValidationResult.ResultItemMetaData>(ref resultItem.MetaData, resultItem.MetaData.Length + 1);
			resultItem.MetaData[resultItem.MetaData.Length - 1] = new SelfValidationResult.ResultItemMetaData(name, value, attributes);
			return ref resultItem;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00004354 File Offset: 0x00002554
		public static ref SelfValidationResult.ResultItem WithMetaData(this SelfValidationResult.ResultItem resultItem, object value, params Attribute[] attributes)
		{
			resultItem.MetaData = (resultItem.MetaData ?? new SelfValidationResult.ResultItemMetaData[0]);
			Array.Resize<SelfValidationResult.ResultItemMetaData>(ref resultItem.MetaData, resultItem.MetaData.Length + 1);
			resultItem.MetaData[resultItem.MetaData.Length - 1] = new SelfValidationResult.ResultItemMetaData(null, value, attributes);
			return ref resultItem;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000043AC File Offset: 0x000025AC
		public static ref SelfValidationResult.ResultItem WithButton(this SelfValidationResult.ResultItem resultItem, string name, Action onClick)
		{
			resultItem.MetaData = (resultItem.MetaData ?? new SelfValidationResult.ResultItemMetaData[0]);
			Array.Resize<SelfValidationResult.ResultItemMetaData>(ref resultItem.MetaData, resultItem.MetaData.Length + 1);
			resultItem.MetaData[resultItem.MetaData.Length - 1] = new SelfValidationResult.ResultItemMetaData(name, onClick, Array.Empty<Attribute>());
			return ref resultItem;
		}
	}
}
