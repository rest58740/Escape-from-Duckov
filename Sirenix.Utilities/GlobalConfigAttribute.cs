using System;
using System.ComponentModel;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x02000020 RID: 32
	[AttributeUsage(4)]
	public class GlobalConfigAttribute : Attribute
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000154 RID: 340 RVA: 0x0000B561 File Offset: 0x00009761
		[Obsolete("It's a bit more complicated than that as it's not always possible to know the full path, so try and make due without it if you can, only using the AssetDatabase.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string FullPath
		{
			get
			{
				return Application.dataPath + "/" + this.AssetPath;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000B578 File Offset: 0x00009778
		public string AssetPath
		{
			get
			{
				return this.assetPath;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000156 RID: 342 RVA: 0x0000B580 File Offset: 0x00009780
		internal string AssetPathWithAssetsPrefix
		{
			get
			{
				string text = this.AssetPath;
				if (text.StartsWith("Assets/"))
				{
					return text;
				}
				return "Assets/" + text;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000B5B0 File Offset: 0x000097B0
		internal string AssetPathWithoutAssetsPrefix
		{
			get
			{
				string text = this.AssetPath;
				if (text.StartsWith("Assets/"))
				{
					return text.Substring("Assets/".Length);
				}
				return text;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000158 RID: 344 RVA: 0x0000B5E4 File Offset: 0x000097E4
		public string ResourcesPath
		{
			get
			{
				if (this.IsInResourcesFolder)
				{
					string text = this.AssetPath;
					int num = text.LastIndexOf("/resources/", 3);
					if (num >= 0)
					{
						return text.Substring(num + "/resources/".Length);
					}
				}
				return "";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000B629 File Offset: 0x00009829
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000B631 File Offset: 0x00009831
		[Obsolete("This option is obsolete and will have no effect - a GlobalConfig will always have an asset generated now; use a POCO singleton or a ScriptableSingleton<T> instead. Asset-less config objects that are recreated every reload cause UnityEngine.Object leaks.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool UseAsset { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000B63A File Offset: 0x0000983A
		public bool IsInResourcesFolder
		{
			get
			{
				return this.AssetPath.Contains("/resources/", 5);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000B64D File Offset: 0x0000984D
		public GlobalConfigAttribute() : this("Assets/Resources/Global Settings")
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000B65C File Offset: 0x0000985C
		public GlobalConfigAttribute(string assetPath)
		{
			this.assetPath = assetPath.Trim().TrimEnd(new char[]
			{
				'/',
				'\\'
			}).TrimStart(new char[]
			{
				'/',
				'\\'
			}).Replace('\\', '/') + "/";
		}

		// Token: 0x0400004F RID: 79
		private string assetPath;
	}
}
