using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000050 RID: 80
	[AddComponentMenu("Animancer/Sprite Renderer Texture Swap")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/SpriteRendererTextureSwap")]
	[DefaultExecutionOrder(30000)]
	public class SpriteRendererTextureSwap : MonoBehaviour
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		public ref SpriteRenderer Renderer
		{
			get
			{
				return ref this._Renderer;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x0000D4C0 File Offset: 0x0000B6C0
		public Texture2D Texture
		{
			get
			{
				return this._Texture;
			}
			set
			{
				this._Texture = value;
				this.RefreshSpriteMap();
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000D4CF File Offset: 0x0000B6CF
		private void RefreshSpriteMap()
		{
			this._SpriteMap = SpriteRendererTextureSwap.GetSpriteMap(this._Texture);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000D4E2 File Offset: 0x0000B6E2
		protected virtual void Awake()
		{
			this.RefreshSpriteMap();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000D4EA File Offset: 0x0000B6EA
		protected virtual void OnValidate()
		{
			this.RefreshSpriteMap();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		protected virtual void LateUpdate()
		{
			if (this._Renderer == null)
			{
				return;
			}
			Sprite sprite = this._Renderer.sprite;
			if (SpriteRendererTextureSwap.TrySwapTexture(this._SpriteMap, this._Texture, ref sprite))
			{
				this._Renderer.sprite = sprite;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000D53D File Offset: 0x0000B73D
		public void ClearCache()
		{
			SpriteRendererTextureSwap.DestroySprites(this._SpriteMap);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000D54C File Offset: 0x0000B74C
		public static Dictionary<Sprite, Sprite> GetSpriteMap(Texture2D texture)
		{
			if (texture == null)
			{
				return null;
			}
			Dictionary<Sprite, Sprite> result;
			if (!SpriteRendererTextureSwap.TextureToSpriteMap.TryGetValue(texture, out result))
			{
				SpriteRendererTextureSwap.TextureToSpriteMap.Add(texture, result = new Dictionary<Sprite, Sprite>());
			}
			return result;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000D588 File Offset: 0x0000B788
		public static bool TrySwapTexture(Dictionary<Sprite, Sprite> spriteMap, Texture2D texture, ref Sprite sprite)
		{
			if (spriteMap == null || sprite == null || texture == null || sprite.texture == texture)
			{
				return false;
			}
			Sprite sprite2;
			if (!spriteMap.TryGetValue(sprite, out sprite2))
			{
				Vector2 pivot = sprite.pivot;
				pivot.x /= sprite.rect.width;
				pivot.y /= sprite.rect.height;
				sprite2 = Sprite.Create(texture, sprite.rect, pivot, sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect, sprite.border, false);
				spriteMap.Add(sprite, sprite2);
			}
			sprite = sprite2;
			return true;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000D634 File Offset: 0x0000B834
		public static void DestroySprites(Dictionary<Sprite, Sprite> spriteMap)
		{
			if (spriteMap == null)
			{
				return;
			}
			foreach (Sprite obj in spriteMap.Values)
			{
				UnityEngine.Object.Destroy(obj);
			}
			spriteMap.Clear();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000D690 File Offset: 0x0000B890
		public static void DestroySprites(Texture2D texture)
		{
			Dictionary<Sprite, Sprite> spriteMap;
			if (SpriteRendererTextureSwap.TextureToSpriteMap.TryGetValue(texture, out spriteMap))
			{
				SpriteRendererTextureSwap.TextureToSpriteMap.Remove(texture);
				SpriteRendererTextureSwap.DestroySprites(spriteMap);
			}
		}

		// Token: 0x040000CC RID: 204
		public const int DefaultExecutionOrder = 30000;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		[Tooltip("The SpriteRenderer that will have its Sprite modified")]
		private SpriteRenderer _Renderer;

		// Token: 0x040000CE RID: 206
		[SerializeField]
		[Tooltip("The replacement for the original Sprite texture")]
		private Texture2D _Texture;

		// Token: 0x040000CF RID: 207
		private Dictionary<Sprite, Sprite> _SpriteMap;

		// Token: 0x040000D0 RID: 208
		private static readonly Dictionary<Texture2D, Dictionary<Sprite, Sprite>> TextureToSpriteMap = new Dictionary<Texture2D, Dictionary<Sprite, Sprite>>();
	}
}
