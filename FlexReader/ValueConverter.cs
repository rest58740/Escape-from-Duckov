using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlexFramework.Excel
{
	// Token: 0x02000011 RID: 17
	public static class ValueConverter
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000031C2 File Offset: 0x000013C2
		static ValueConverter()
		{
			ValueConverter.Reset();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000031D4 File Offset: 0x000013D4
		public static void Reset()
		{
			ValueConverter.converters.Clear();
			ValueConverter.Register<ColorConverter>();
			ValueConverter.Register<Color32Converter>();
			ValueConverter.Register<RectConverter>();
			ValueConverter.Register<Vector4Converter>();
			ValueConverter.Register<Vector3Converter>();
			ValueConverter.Register<Vector2Converter>();
			ValueConverter.Register<ObjectConverter>();
			ValueConverter.Register<ArrayConverter<string>>();
			ValueConverter.Register<ArrayConverter<int>>();
			ValueConverter.Register<ArrayConverter<float>>();
			ValueConverter.Register<ListConverter<string>>();
			ValueConverter.Register<ListConverter<int>>();
			ValueConverter.Register<ListConverter<float>>();
			ValueConverter.Register<DictionaryConverter<string, string>>();
			ValueConverter.Register<DictionaryConverter<string, int>>();
			ValueConverter.Register<DictionaryConverter<int, int>>();
			ValueConverter.Register<DictionaryConverter<int, string>>();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003251 File Offset: 0x00001451
		public static void Empty()
		{
			ValueConverter.converters.Clear();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003260 File Offset: 0x00001460
		public static object Convert(Type type, string value)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			IConverter converter = ValueConverter.converters.Find((IConverter c) => c.Type == type);
			if (converter == null)
			{
				TypeConverter @default = TypeDescriptor.GetConverter(type);
				if (@default == null)
				{
					throw new NotSupportedException("Converter not found for target type: " + type.Name);
				}
				converter = new ValueConverter.Converter((string i) => @default.ConvertFromString(i), type);
				ValueConverter.Register(converter);
			}
			object result;
			try
			{
				result = converter.Convert(value);
			}
			catch (FormatException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new FormatException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003340 File Offset: 0x00001540
		public static T Convert<T>(string value)
		{
			return (T)((object)ValueConverter.Convert(typeof(T), value));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003358 File Offset: 0x00001558
		public static bool Register(IConverter converter)
		{
			if (converter == null)
			{
				return false;
			}
			ValueConverter.converters.RemoveAll((IConverter c) => c.Type == converter.Type);
			ValueConverter.converters.Add(converter);
			return true;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000033A4 File Offset: 0x000015A4
		public static bool Register<T>() where T : IConverter, new()
		{
			return ValueConverter.Register(Activator.CreateInstance<T>());
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000033B5 File Offset: 0x000015B5
		public static bool Register<T>(Converter<string, T> converter)
		{
			return ValueConverter.Register(new ValueConverter.Converter<T>(converter));
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033C2 File Offset: 0x000015C2
		public static bool Register(Converter<string, object> converter, Type type)
		{
			return ValueConverter.Register(new ValueConverter.Converter(converter, type));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000033D0 File Offset: 0x000015D0
		public static bool Unregister(Type type)
		{
			return ValueConverter.converters.RemoveAll((IConverter c) => c.Type == type) > 0;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003403 File Offset: 0x00001603
		public static bool Unregister<T>()
		{
			return ValueConverter.Unregister(typeof(T));
		}

		// Token: 0x04000001 RID: 1
		private static readonly List<IConverter> converters = new List<IConverter>();

		// Token: 0x02000036 RID: 54
		private class Converter<T> : IConverter
		{
			// Token: 0x06000138 RID: 312 RVA: 0x0000569B File Offset: 0x0000389B
			public Converter(Converter<string, T> converter)
			{
				if (converter == null)
				{
					throw new ArgumentNullException();
				}
				this._converter = converter;
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000139 RID: 313 RVA: 0x000056B3 File Offset: 0x000038B3
			Type IConverter.Type
			{
				get
				{
					return typeof(T);
				}
			}

			// Token: 0x0600013A RID: 314 RVA: 0x000056BF File Offset: 0x000038BF
			object IConverter.Convert(string input)
			{
				return this._converter(input);
			}

			// Token: 0x04000043 RID: 67
			private Converter<string, T> _converter;
		}

		// Token: 0x02000037 RID: 55
		private class Converter : IConverter
		{
			// Token: 0x0600013B RID: 315 RVA: 0x000056D2 File Offset: 0x000038D2
			public Converter(Converter<string, object> converter, Type type)
			{
				if (converter == null || type == null)
				{
					throw new ArgumentNullException();
				}
				this._converter = converter;
				this._type = type;
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x0600013C RID: 316 RVA: 0x000056FA File Offset: 0x000038FA
			Type IConverter.Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x0600013D RID: 317 RVA: 0x00005702 File Offset: 0x00003902
			object IConverter.Convert(string input)
			{
				return this._converter(input);
			}

			// Token: 0x04000044 RID: 68
			private Converter<string, object> _converter;

			// Token: 0x04000045 RID: 69
			private Type _type;
		}
	}
}
