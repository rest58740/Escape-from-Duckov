using System;
using System.Collections.Generic;

namespace ParadoxNotion
{
	// Token: 0x02000070 RID: 112
	public class HierarchyTree
	{
		// Token: 0x02000118 RID: 280
		public class Element
		{
			// Token: 0x17000156 RID: 342
			// (get) Token: 0x060007F5 RID: 2037 RVA: 0x00017EA9 File Offset: 0x000160A9
			public object reference
			{
				get
				{
					return this._reference;
				}
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00017EB1 File Offset: 0x000160B1
			public HierarchyTree.Element parent
			{
				get
				{
					return this._parent;
				}
			}

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00017EB9 File Offset: 0x000160B9
			public IEnumerable<HierarchyTree.Element> children
			{
				get
				{
					return this._children;
				}
			}

			// Token: 0x060007F8 RID: 2040 RVA: 0x00017EC1 File Offset: 0x000160C1
			public Element(object reference)
			{
				this._reference = reference;
			}

			// Token: 0x060007F9 RID: 2041 RVA: 0x00017ED0 File Offset: 0x000160D0
			public HierarchyTree.Element AddChild(HierarchyTree.Element child)
			{
				if (this._children == null)
				{
					this._children = new List<HierarchyTree.Element>();
				}
				child._parent = this;
				this._children.Add(child);
				return child;
			}

			// Token: 0x060007FA RID: 2042 RVA: 0x00017EF9 File Offset: 0x000160F9
			public void RemoveChild(HierarchyTree.Element child)
			{
				if (this._children != null)
				{
					this._children.Remove(child);
				}
			}

			// Token: 0x060007FB RID: 2043 RVA: 0x00017F10 File Offset: 0x00016110
			public HierarchyTree.Element GetRoot()
			{
				HierarchyTree.Element parent;
				for (parent = this._parent; parent != null; parent = parent._parent)
				{
				}
				return parent;
			}

			// Token: 0x060007FC RID: 2044 RVA: 0x00017F34 File Offset: 0x00016134
			public HierarchyTree.Element FindReferenceElement(object target)
			{
				if (this._reference == target)
				{
					return this;
				}
				if (this._children == null)
				{
					return null;
				}
				for (int i = 0; i < this._children.Count; i++)
				{
					HierarchyTree.Element element = this._children[i].FindReferenceElement(target);
					if (element != null)
					{
						return element;
					}
				}
				return null;
			}

			// Token: 0x060007FD RID: 2045 RVA: 0x00017F88 File Offset: 0x00016188
			public T GetFirstParentReferenceOfType<T>()
			{
				if (this._reference is T)
				{
					return (T)((object)this._reference);
				}
				if (this._parent == null)
				{
					return default(T);
				}
				return this._parent.GetFirstParentReferenceOfType<T>();
			}

			// Token: 0x060007FE RID: 2046 RVA: 0x00017FCB File Offset: 0x000161CB
			public IEnumerable<T> GetAllChildrenReferencesOfType<T>()
			{
				if (this._children == null)
				{
					yield break;
				}
				int num;
				for (int i = 0; i < this._children.Count; i = num + 1)
				{
					HierarchyTree.Element element = this._children[i];
					if (element._reference is T)
					{
						yield return (T)((object)element._reference);
					}
					foreach (T t in element.GetAllChildrenReferencesOfType<T>())
					{
						yield return t;
					}
					IEnumerator<T> enumerator = null;
					element = null;
					num = i;
				}
				yield break;
				yield break;
			}

			// Token: 0x040002C9 RID: 713
			private object _reference;

			// Token: 0x040002CA RID: 714
			private HierarchyTree.Element _parent;

			// Token: 0x040002CB RID: 715
			private List<HierarchyTree.Element> _children;
		}
	}
}
