using System;
using System.Configuration.Assemblies;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Security;
using System.Security.Policy;
using System.Threading;
using Unity;

namespace System
{
	// Token: 0x020001EA RID: 490
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Activator))]
	public sealed class Activator : _Activator
	{
		// Token: 0x060014E5 RID: 5349 RVA: 0x0000259F File Offset: 0x0000079F
		private Activator()
		{
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000523EF File Offset: 0x000505EF
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture)
		{
			return Activator.CreateInstance(type, bindingAttr, binder, args, culture, null);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00052400 File Offset: 0x00050600
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static object CreateInstance(Type type, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (RuntimeFeature.IsDynamicCodeSupported && type is TypeBuilder)
			{
				throw new NotSupportedException(Environment.GetResourceString("CreateInstance cannot be used with an object of type TypeBuilder."));
			}
			if ((bindingAttr & (BindingFlags)255) == BindingFlags.Default)
			{
				bindingAttr |= (BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
			}
			if (activationAttributes != null && activationAttributes.Length != 0)
			{
				if (!type.IsMarshalByRef)
				{
					throw new NotSupportedException(Environment.GetResourceString("Activation Attributes are not supported for types not deriving from MarshalByRefObject."));
				}
				if (!type.IsContextful && (activationAttributes.Length > 1 || !(activationAttributes[0] is UrlAttribute)))
				{
					throw new NotSupportedException(Environment.GetResourceString("UrlAttribute is the only attribute supported for MarshalByRefObject."));
				}
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Type must be a type provided by the runtime."), "type");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return runtimeType.CreateInstanceImpl(bindingAttr, binder, args, culture, activationAttributes, ref stackCrawlMark);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x000524D0 File Offset: 0x000506D0
		public static object CreateInstance(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, args, null, null);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x000524E1 File Offset: 0x000506E1
		public static object CreateInstance(Type type, object[] args, object[] activationAttributes)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, args, null, activationAttributes);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x000524F2 File Offset: 0x000506F2
		public static object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type, false);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x000524FC File Offset: 0x000506FC
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, null, null, ref stackCrawlMark);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00052534 File Offset: 0x00050734
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes, null, ref stackCrawlMark);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0005256B File Offset: 0x0005076B
		public static object CreateInstance(Type type, bool nonPublic)
		{
			return Activator.CreateInstance(type, nonPublic, true);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00052578 File Offset: 0x00050778
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static object CreateInstance(Type type, bool nonPublic, bool wrapExceptions)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			RuntimeType runtimeType = type.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Type must be a type provided by the runtime."), "type");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return runtimeType.CreateInstanceDefaultCtor(!nonPublic, false, true, wrapExceptions, ref stackCrawlMark);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x000525CC File Offset: 0x000507CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static T CreateInstance<T>()
		{
			RuntimeType runtimeType = typeof(T) as RuntimeType;
			if (runtimeType.HasElementType)
			{
				throw new MissingMethodException(Environment.GetResourceString("No parameterless constructor defined for this object."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return (T)((object)runtimeType.CreateInstanceDefaultCtor(true, true, true, true, ref stackCrawlMark));
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x00052612 File Offset: 0x00050812
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, null);
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0005261C File Offset: 0x0005081C
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
		{
			return Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, activationAttributes);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00052630 File Offset: 0x00050830
		[Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstance which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo, ref stackCrawlMark);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00052668 File Offset: 0x00050868
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (assemblyName == null)
			{
				assemblyName = Assembly.GetCallingAssembly().GetName().Name;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null, ref stackCrawlMark);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000526A0 File Offset: 0x000508A0
		[SecurityCritical]
		internal static ObjectHandle CreateInstance(string assemblyString, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo, ref StackCrawlMark stackMark)
		{
			Type type = null;
			Assembly assembly = null;
			if (assemblyString == null)
			{
				assembly = RuntimeAssembly.GetExecutingAssembly(ref stackMark);
			}
			else
			{
				RuntimeAssembly runtimeAssembly;
				AssemblyName assemblyName = RuntimeAssembly.CreateAssemblyName(assemblyString, false, out runtimeAssembly);
				if (runtimeAssembly != null)
				{
					assembly = runtimeAssembly;
				}
				else if (assemblyName.ContentType == AssemblyContentType.WindowsRuntime)
				{
					type = Type.GetType(typeName + ", " + assemblyString, true, ignoreCase);
				}
				else
				{
					assembly = RuntimeAssembly.InternalLoadAssemblyName(assemblyName, securityInfo, null, ref stackMark, true, false, false);
				}
			}
			if (type == null)
			{
				if (assembly == null)
				{
					return null;
				}
				type = assembly.GetType(typeName, true, ignoreCase);
			}
			object obj = Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00052740 File Offset: 0x00050940
		[Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstanceFrom which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			return Activator.CreateInstanceFromInternal(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityInfo);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00052760 File Offset: 0x00050960
		public static ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			return Activator.CreateInstanceFromInternal(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00052780 File Offset: 0x00050980
		private static ObjectHandle CreateInstanceFromInternal(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityInfo)
		{
			object obj = Activator.CreateInstance(Assembly.LoadFrom(assemblyFile, securityInfo).GetType(typeName, true, ignoreCase), bindingAttr, binder, args, culture, activationAttributes);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x000527B7 File Offset: 0x000509B7
		[SecurityCritical]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x000527D0 File Offset: 0x000509D0
		[SecurityCritical]
		[Obsolete("Methods which use evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstance which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00052800 File Offset: 0x00050A00
		[SecurityCritical]
		public static ObjectHandle CreateInstance(AppDomain domain, string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceWithNoSecurity(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0005282F File Offset: 0x00050A2F
		[SecurityCritical]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00052848 File Offset: 0x00050A48
		[Obsolete("Methods which use Evidence to sandbox are obsolete and will be removed in a future release of the .NET Framework. Please use an overload of CreateInstanceFrom which does not take an Evidence parameter. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		[SecurityCritical]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00052878 File Offset: 0x00050A78
		[SecurityCritical]
		public static ObjectHandle CreateInstanceFrom(AppDomain domain, string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
		{
			if (domain == null)
			{
				throw new ArgumentNullException("domain");
			}
			return domain.InternalCreateInstanceFromWithNoSecurity(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x000528A7 File Offset: 0x00050AA7
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName)
		{
			return Activator.CreateComInstanceFrom(assemblyName, typeName, null, AssemblyHashAlgorithm.None);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x000528B4 File Offset: 0x00050AB4
		public static ObjectHandle CreateComInstanceFrom(string assemblyName, string typeName, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyName, hashValue, hashAlgorithm);
			Type type = assembly.GetType(typeName, true, false);
			object[] customAttributes = type.GetCustomAttributes(typeof(ComVisibleAttribute), false);
			if (customAttributes.Length != 0 && !((ComVisibleAttribute)customAttributes[0]).Value)
			{
				throw new TypeLoadException(Environment.GetResourceString("The specified type must be visible from COM."));
			}
			if (assembly == null)
			{
				return null;
			}
			object obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null, null);
			if (obj == null)
			{
				return null;
			}
			return new ObjectHandle(obj);
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0005292D File Offset: 0x00050B2D
		[SecurityCritical]
		public static object GetObject(Type type, string url)
		{
			return Activator.GetObject(type, url, null);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00052937 File Offset: 0x00050B37
		[SecurityCritical]
		public static object GetObject(Type type, string url, object state)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RemotingServices.Connect(type, url, state);
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00052955 File Offset: 0x00050B55
		[Conditional("_DEBUG")]
		private static void Log(bool test, string title, string success, string failure)
		{
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Activator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00052959 File Offset: 0x00050B59
		[SecuritySafeCritical]
		public static ObjectHandle CreateInstance(ActivationContext activationContext)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00052959 File Offset: 0x00050B59
		[SecuritySafeCritical]
		public static ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x040014F4 RID: 5364
		internal const int LookupMask = 255;

		// Token: 0x040014F5 RID: 5365
		internal const BindingFlags ConLookup = BindingFlags.Instance | BindingFlags.Public;

		// Token: 0x040014F6 RID: 5366
		internal const BindingFlags ConstructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
	}
}
