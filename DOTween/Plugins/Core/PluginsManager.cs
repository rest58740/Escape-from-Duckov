using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
	// Token: 0x02000043 RID: 67
	internal static class PluginsManager
	{
		// Token: 0x0600026B RID: 619 RVA: 0x0000DB58 File Offset: 0x0000BD58
		internal static ABSTweenPlugin<T1, T2, TPlugOptions> GetDefaultPlugin<T1, T2, TPlugOptions>() where TPlugOptions : struct, IPlugOptions
		{
			Type typeFromHandle = typeof(T1);
			Type typeFromHandle2 = typeof(T2);
			ITweenPlugin tweenPlugin = null;
			if (typeFromHandle == typeof(Vector3) && typeFromHandle == typeFromHandle2)
			{
				if (PluginsManager._vector3Plugin == null)
				{
					PluginsManager._vector3Plugin = new Vector3Plugin();
				}
				tweenPlugin = PluginsManager._vector3Plugin;
			}
			else if (typeFromHandle == typeof(Vector3) && typeFromHandle2 == typeof(Vector3[]))
			{
				if (PluginsManager._vector3ArrayPlugin == null)
				{
					PluginsManager._vector3ArrayPlugin = new Vector3ArrayPlugin();
				}
				tweenPlugin = PluginsManager._vector3ArrayPlugin;
			}
			else if (typeFromHandle == typeof(Quaternion))
			{
				if (typeFromHandle2 == typeof(Quaternion))
				{
					Debugger.LogError("Quaternion tweens require a Vector3 endValue", null);
				}
				else
				{
					if (PluginsManager._quaternionPlugin == null)
					{
						PluginsManager._quaternionPlugin = new QuaternionPlugin();
					}
					tweenPlugin = PluginsManager._quaternionPlugin;
				}
			}
			else if (typeFromHandle == typeof(Vector2))
			{
				if (PluginsManager._vector2Plugin == null)
				{
					PluginsManager._vector2Plugin = new Vector2Plugin();
				}
				tweenPlugin = PluginsManager._vector2Plugin;
			}
			else if (typeFromHandle == typeof(float))
			{
				if (PluginsManager._floatPlugin == null)
				{
					PluginsManager._floatPlugin = new FloatPlugin();
				}
				tweenPlugin = PluginsManager._floatPlugin;
			}
			else if (typeFromHandle == typeof(Color))
			{
				if (PluginsManager._colorPlugin == null)
				{
					PluginsManager._colorPlugin = new ColorPlugin();
				}
				tweenPlugin = PluginsManager._colorPlugin;
			}
			else if (typeFromHandle == typeof(int))
			{
				if (PluginsManager._intPlugin == null)
				{
					PluginsManager._intPlugin = new IntPlugin();
				}
				tweenPlugin = PluginsManager._intPlugin;
			}
			else if (typeFromHandle == typeof(Vector4))
			{
				if (PluginsManager._vector4Plugin == null)
				{
					PluginsManager._vector4Plugin = new Vector4Plugin();
				}
				tweenPlugin = PluginsManager._vector4Plugin;
			}
			else if (typeFromHandle == typeof(Rect))
			{
				if (PluginsManager._rectPlugin == null)
				{
					PluginsManager._rectPlugin = new RectPlugin();
				}
				tweenPlugin = PluginsManager._rectPlugin;
			}
			else if (typeFromHandle == typeof(RectOffset))
			{
				if (PluginsManager._rectOffsetPlugin == null)
				{
					PluginsManager._rectOffsetPlugin = new RectOffsetPlugin();
				}
				tweenPlugin = PluginsManager._rectOffsetPlugin;
			}
			else if (typeFromHandle == typeof(uint))
			{
				if (PluginsManager._uintPlugin == null)
				{
					PluginsManager._uintPlugin = new UintPlugin();
				}
				tweenPlugin = PluginsManager._uintPlugin;
			}
			else if (typeFromHandle == typeof(string))
			{
				if (PluginsManager._stringPlugin == null)
				{
					PluginsManager._stringPlugin = new StringPlugin();
				}
				tweenPlugin = PluginsManager._stringPlugin;
			}
			else if (typeFromHandle == typeof(Color2))
			{
				if (PluginsManager._color2Plugin == null)
				{
					PluginsManager._color2Plugin = new Color2Plugin();
				}
				tweenPlugin = PluginsManager._color2Plugin;
			}
			else if (typeFromHandle == typeof(long))
			{
				if (PluginsManager._longPlugin == null)
				{
					PluginsManager._longPlugin = new LongPlugin();
				}
				tweenPlugin = PluginsManager._longPlugin;
			}
			else if (typeFromHandle == typeof(ulong))
			{
				if (PluginsManager._ulongPlugin == null)
				{
					PluginsManager._ulongPlugin = new UlongPlugin();
				}
				tweenPlugin = PluginsManager._ulongPlugin;
			}
			else if (typeFromHandle == typeof(double))
			{
				if (PluginsManager._doublePlugin == null)
				{
					PluginsManager._doublePlugin = new DoublePlugin();
				}
				tweenPlugin = PluginsManager._doublePlugin;
			}
			if (tweenPlugin != null)
			{
				return tweenPlugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
			}
			return null;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000DE38 File Offset: 0x0000C038
		public static ABSTweenPlugin<T1, T2, TPlugOptions> GetCustomPlugin<TPlugin, T1, T2, TPlugOptions>() where TPlugin : ITweenPlugin, new() where TPlugOptions : struct, IPlugOptions
		{
			Type typeFromHandle = typeof(TPlugin);
			ITweenPlugin tweenPlugin;
			if (PluginsManager._customPlugins == null)
			{
				PluginsManager._customPlugins = new Dictionary<Type, ITweenPlugin>(20);
			}
			else if (PluginsManager._customPlugins.TryGetValue(typeFromHandle, out tweenPlugin))
			{
				return tweenPlugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
			}
			tweenPlugin = Activator.CreateInstance<TPlugin>();
			PluginsManager._customPlugins.Add(typeFromHandle, tweenPlugin);
			return tweenPlugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000DE98 File Offset: 0x0000C098
		internal static void PurgeAll()
		{
			PluginsManager._floatPlugin = null;
			PluginsManager._intPlugin = null;
			PluginsManager._uintPlugin = null;
			PluginsManager._longPlugin = null;
			PluginsManager._ulongPlugin = null;
			PluginsManager._vector2Plugin = null;
			PluginsManager._vector3Plugin = null;
			PluginsManager._vector4Plugin = null;
			PluginsManager._quaternionPlugin = null;
			PluginsManager._colorPlugin = null;
			PluginsManager._rectPlugin = null;
			PluginsManager._rectOffsetPlugin = null;
			PluginsManager._stringPlugin = null;
			PluginsManager._vector3ArrayPlugin = null;
			PluginsManager._color2Plugin = null;
			if (PluginsManager._customPlugins != null)
			{
				PluginsManager._customPlugins.Clear();
			}
		}

		// Token: 0x04000111 RID: 273
		private static ITweenPlugin _floatPlugin;

		// Token: 0x04000112 RID: 274
		private static ITweenPlugin _doublePlugin;

		// Token: 0x04000113 RID: 275
		private static ITweenPlugin _intPlugin;

		// Token: 0x04000114 RID: 276
		private static ITweenPlugin _uintPlugin;

		// Token: 0x04000115 RID: 277
		private static ITweenPlugin _longPlugin;

		// Token: 0x04000116 RID: 278
		private static ITweenPlugin _ulongPlugin;

		// Token: 0x04000117 RID: 279
		private static ITweenPlugin _vector2Plugin;

		// Token: 0x04000118 RID: 280
		private static ITweenPlugin _vector3Plugin;

		// Token: 0x04000119 RID: 281
		private static ITweenPlugin _vector4Plugin;

		// Token: 0x0400011A RID: 282
		private static ITweenPlugin _quaternionPlugin;

		// Token: 0x0400011B RID: 283
		private static ITweenPlugin _colorPlugin;

		// Token: 0x0400011C RID: 284
		private static ITweenPlugin _rectPlugin;

		// Token: 0x0400011D RID: 285
		private static ITweenPlugin _rectOffsetPlugin;

		// Token: 0x0400011E RID: 286
		private static ITweenPlugin _stringPlugin;

		// Token: 0x0400011F RID: 287
		private static ITweenPlugin _vector3ArrayPlugin;

		// Token: 0x04000120 RID: 288
		private static ITweenPlugin _color2Plugin;

		// Token: 0x04000121 RID: 289
		private const int _MaxCustomPlugins = 20;

		// Token: 0x04000122 RID: 290
		private static Dictionary<Type, ITweenPlugin> _customPlugins;
	}
}
