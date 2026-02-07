using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel
{
	// Token: 0x0200006B RID: 107
	public struct LazyMemberInfo
	{
		// Token: 0x060002AC RID: 684 RVA: 0x000086B8 File Offset: 0x000068B8
		public LazyMemberInfo(MemberInfo member)
		{
			Requires.NotNull<MemberInfo>(member, "member");
			LazyMemberInfo.EnsureSupportedMemberType(member.MemberType, "member");
			this._accessorsCreator = null;
			this._memberType = member.MemberType;
			MemberTypes memberType = this._memberType;
			if (memberType == 2)
			{
				EventInfo eventInfo = (EventInfo)member;
				this._accessors = new MemberInfo[]
				{
					eventInfo.GetRaiseMethod(true),
					eventInfo.GetAddMethod(true),
					eventInfo.GetRemoveMethod(true)
				};
				return;
			}
			if (memberType == 16)
			{
				PropertyInfo propertyInfo = (PropertyInfo)member;
				Assumes.NotNull<PropertyInfo>(propertyInfo);
				this._accessors = new MemberInfo[]
				{
					propertyInfo.GetGetMethod(true),
					propertyInfo.GetSetMethod(true)
				};
				return;
			}
			this._accessors = new MemberInfo[]
			{
				member
			};
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00008774 File Offset: 0x00006974
		public LazyMemberInfo(MemberTypes memberType, params MemberInfo[] accessors)
		{
			LazyMemberInfo.EnsureSupportedMemberType(memberType, "memberType");
			Requires.NotNull<MemberInfo[]>(accessors, "accessors");
			string text;
			if (!LazyMemberInfo.AreAccessorsValid(memberType, accessors, out text))
			{
				throw new ArgumentException(text, "accessors");
			}
			this._memberType = memberType;
			this._accessors = accessors;
			this._accessorsCreator = null;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000087C3 File Offset: 0x000069C3
		public LazyMemberInfo(MemberTypes memberType, Func<MemberInfo[]> accessorsCreator)
		{
			LazyMemberInfo.EnsureSupportedMemberType(memberType, "memberType");
			Requires.NotNull<Func<MemberInfo[]>>(accessorsCreator, "accessorsCreator");
			this._memberType = memberType;
			this._accessors = null;
			this._accessorsCreator = accessorsCreator;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000087F0 File Offset: 0x000069F0
		public MemberTypes MemberType
		{
			get
			{
				return this._memberType;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000087F8 File Offset: 0x000069F8
		public MemberInfo[] GetAccessors()
		{
			if (this._accessors == null && this._accessorsCreator != null)
			{
				MemberInfo[] accessors = this._accessorsCreator.Invoke();
				string text;
				if (!LazyMemberInfo.AreAccessorsValid(this.MemberType, accessors, out text))
				{
					throw new InvalidOperationException(text);
				}
				this._accessors = accessors;
			}
			return this._accessors;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00008848 File Offset: 0x00006A48
		public override int GetHashCode()
		{
			if (this._accessorsCreator != null)
			{
				return this.MemberType.GetHashCode() ^ this._accessorsCreator.GetHashCode();
			}
			Assumes.NotNull<MemberInfo[]>(this._accessors);
			Assumes.NotNull<MemberInfo>(this._accessors[0]);
			return this.MemberType.GetHashCode() ^ this._accessors[0].GetHashCode();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000088B8 File Offset: 0x00006AB8
		public override bool Equals(object obj)
		{
			LazyMemberInfo lazyMemberInfo = (LazyMemberInfo)obj;
			if (this._memberType != lazyMemberInfo._memberType)
			{
				return false;
			}
			if (this._accessorsCreator != null || lazyMemberInfo._accessorsCreator != null)
			{
				return object.Equals(this._accessorsCreator, lazyMemberInfo._accessorsCreator);
			}
			Assumes.NotNull<MemberInfo[]>(this._accessors);
			Assumes.NotNull<MemberInfo[]>(lazyMemberInfo._accessors);
			return this._accessors.SequenceEqual(lazyMemberInfo._accessors);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00008925 File Offset: 0x00006B25
		public static bool operator ==(LazyMemberInfo left, LazyMemberInfo right)
		{
			return left.Equals(right);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000893A File Offset: 0x00006B3A
		public static bool operator !=(LazyMemberInfo left, LazyMemberInfo right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00008954 File Offset: 0x00006B54
		private static void EnsureSupportedMemberType(MemberTypes memberType, string argument)
		{
			MemberTypes enumFlagSet = 191;
			Requires.IsInMembertypeSet(memberType, argument, enumFlagSet);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00008970 File Offset: 0x00006B70
		private static bool AreAccessorsValid(MemberTypes memberType, MemberInfo[] accessors, out string errorMessage)
		{
			errorMessage = string.Empty;
			if (accessors == null)
			{
				errorMessage = Strings.LazyMemberInfo_AccessorsNull;
				return false;
			}
			if (accessors.All((MemberInfo accessor) => accessor == null))
			{
				errorMessage = Strings.LazyMemberInfo_NoAccessors;
				return false;
			}
			if (memberType != 2)
			{
				if (memberType == 16)
				{
					if (accessors.Length != 2)
					{
						errorMessage = Strings.LazyMemberInfo_InvalidPropertyAccessors_Cardinality;
						return false;
					}
					if ((from accessor in accessors
					where accessor != null && accessor.MemberType != 8
					select accessor).Any<MemberInfo>())
					{
						errorMessage = Strings.LazyMemberinfo_InvalidPropertyAccessors_AccessorType;
						return false;
					}
				}
				else if (accessors.Length != 1 || (accessors.Length == 1 && accessors[0].MemberType != memberType))
				{
					errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.LazyMemberInfo_InvalidAccessorOnSimpleMember, memberType);
					return false;
				}
			}
			else
			{
				if (accessors.Length != 3)
				{
					errorMessage = Strings.LazyMemberInfo_InvalidEventAccessors_Cardinality;
					return false;
				}
				if ((from accessor in accessors
				where accessor != null && accessor.MemberType != 8
				select accessor).Any<MemberInfo>())
				{
					errorMessage = Strings.LazyMemberinfo_InvalidEventAccessors_AccessorType;
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000124 RID: 292
		private readonly MemberTypes _memberType;

		// Token: 0x04000125 RID: 293
		private MemberInfo[] _accessors;

		// Token: 0x04000126 RID: 294
		private readonly Func<MemberInfo[]> _accessorsCreator;
	}
}
