using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x020008DE RID: 2270
	public struct CustomAttributeTypedArgument
	{
		// Token: 0x06004B9B RID: 19355 RVA: 0x000F0D14 File Offset: 0x000EEF14
		public CustomAttributeTypedArgument(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = CustomAttributeTypedArgument.CanonicalizeValue(value);
			this.ArgumentType = value.GetType();
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x000F0D3C File Offset: 0x000EEF3C
		public CustomAttributeTypedArgument(Type argumentType, object value)
		{
			if (argumentType == null)
			{
				throw new ArgumentNullException("argumentType");
			}
			this.Value = ((value == null) ? null : CustomAttributeTypedArgument.CanonicalizeValue(value));
			this.ArgumentType = argumentType;
			Array array = value as Array;
			if (array != null)
			{
				Type elementType = array.GetType().GetElementType();
				CustomAttributeTypedArgument[] array2 = new CustomAttributeTypedArgument[array.GetLength(0)];
				for (int i = 0; i < array2.Length; i++)
				{
					object value2 = array.GetValue(i);
					Type argumentType2 = (elementType == typeof(object) && value2 != null) ? value2.GetType() : elementType;
					array2[i] = new CustomAttributeTypedArgument(argumentType2, value2);
				}
				this.Value = new ReadOnlyCollection<CustomAttributeTypedArgument>(array2);
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004B9D RID: 19357 RVA: 0x000F0DEE File Offset: 0x000EEFEE
		public readonly Type ArgumentType { get; }

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06004B9E RID: 19358 RVA: 0x000F0DF6 File Offset: 0x000EEFF6
		public readonly object Value { get; }

		// Token: 0x06004B9F RID: 19359 RVA: 0x000F0DFE File Offset: 0x000EEFFE
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x000F0E0E File Offset: 0x000EF00E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x000F0E20 File Offset: 0x000EF020
		public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
		{
			return left.Equals(right);
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x000F0E35 File Offset: 0x000EF035
		public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x000F0E4D File Offset: 0x000EF04D
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x000F0E58 File Offset: 0x000EF058
		internal string ToString(bool typed)
		{
			if (this.ArgumentType == null)
			{
				return base.ToString();
			}
			string result;
			try
			{
				if (this.ArgumentType.IsEnum)
				{
					result = string.Format(CultureInfo.CurrentCulture, typed ? "{0}" : "({1}){0}", this.Value, this.ArgumentType.FullNameOrDefault);
				}
				else if (this.Value == null)
				{
					result = string.Format(CultureInfo.CurrentCulture, typed ? "null" : "({0})null", this.ArgumentType.NameOrDefault);
				}
				else if (this.ArgumentType == typeof(string))
				{
					result = string.Format(CultureInfo.CurrentCulture, "\"{0}\"", this.Value);
				}
				else if (this.ArgumentType == typeof(char))
				{
					result = string.Format(CultureInfo.CurrentCulture, "'{0}'", this.Value);
				}
				else if (this.ArgumentType == typeof(Type))
				{
					result = string.Format(CultureInfo.CurrentCulture, "typeof({0})", ((Type)this.Value).FullNameOrDefault);
				}
				else if (this.ArgumentType.IsArray)
				{
					IList<CustomAttributeTypedArgument> list = this.Value as IList<CustomAttributeTypedArgument>;
					Type elementType = this.ArgumentType.GetElementType();
					string str = string.Format(CultureInfo.CurrentCulture, "new {0}[{1}] {{ ", elementType.IsEnum ? elementType.FullNameOrDefault : elementType.NameOrDefault, list.Count);
					for (int i = 0; i < list.Count; i++)
					{
						str += string.Format(CultureInfo.CurrentCulture, (i == 0) ? "{0}" : ", {0}", list[i].ToString(elementType != typeof(object)));
					}
					result = str + " }";
				}
				else
				{
					result = string.Format(CultureInfo.CurrentCulture, typed ? "{0}" : "({1}){0}", this.Value, this.ArgumentType.NameOrDefault);
				}
			}
			catch (MissingMetadataException)
			{
				result = base.ToString();
			}
			return result;
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x000F10B4 File Offset: 0x000EF2B4
		private static object CanonicalizeValue(object value)
		{
			if (value.GetType().IsEnum)
			{
				return ((Enum)value).GetValue();
			}
			return value;
		}
	}
}
