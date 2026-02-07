using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200077F RID: 1919
	[ComVisible(true)]
	[Guid("BCA8B44D-AAD6-3A86-8AB7-03349F4F2DA2")]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(Type))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _Type
	{
		// Token: 0x06004406 RID: 17414
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004407 RID: 17415
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004408 RID: 17416
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004409 RID: 17417
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x0600440A RID: 17418
		string ToString();

		// Token: 0x0600440B RID: 17419
		bool Equals(object other);

		// Token: 0x0600440C RID: 17420
		int GetHashCode();

		// Token: 0x0600440D RID: 17421
		Type GetType();

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x0600440E RID: 17422
		MemberTypes MemberType { get; }

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600440F RID: 17423
		string Name { get; }

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004410 RID: 17424
		Type DeclaringType { get; }

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06004411 RID: 17425
		Type ReflectedType { get; }

		// Token: 0x06004412 RID: 17426
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06004413 RID: 17427
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06004414 RID: 17428
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06004415 RID: 17429
		Guid GUID { get; }

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06004416 RID: 17430
		Module Module { get; }

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06004417 RID: 17431
		Assembly Assembly { get; }

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06004418 RID: 17432
		RuntimeTypeHandle TypeHandle { get; }

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06004419 RID: 17433
		string FullName { get; }

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x0600441A RID: 17434
		string Namespace { get; }

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x0600441B RID: 17435
		string AssemblyQualifiedName { get; }

		// Token: 0x0600441C RID: 17436
		int GetArrayRank();

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x0600441D RID: 17437
		Type BaseType { get; }

		// Token: 0x0600441E RID: 17438
		ConstructorInfo[] GetConstructors(BindingFlags bindingAttr);

		// Token: 0x0600441F RID: 17439
		Type GetInterface(string name, bool ignoreCase);

		// Token: 0x06004420 RID: 17440
		Type[] GetInterfaces();

		// Token: 0x06004421 RID: 17441
		Type[] FindInterfaces(TypeFilter filter, object filterCriteria);

		// Token: 0x06004422 RID: 17442
		EventInfo GetEvent(string name, BindingFlags bindingAttr);

		// Token: 0x06004423 RID: 17443
		EventInfo[] GetEvents();

		// Token: 0x06004424 RID: 17444
		EventInfo[] GetEvents(BindingFlags bindingAttr);

		// Token: 0x06004425 RID: 17445
		Type[] GetNestedTypes(BindingFlags bindingAttr);

		// Token: 0x06004426 RID: 17446
		Type GetNestedType(string name, BindingFlags bindingAttr);

		// Token: 0x06004427 RID: 17447
		MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr);

		// Token: 0x06004428 RID: 17448
		MemberInfo[] GetDefaultMembers();

		// Token: 0x06004429 RID: 17449
		MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria);

		// Token: 0x0600442A RID: 17450
		Type GetElementType();

		// Token: 0x0600442B RID: 17451
		bool IsSubclassOf(Type c);

		// Token: 0x0600442C RID: 17452
		bool IsInstanceOfType(object o);

		// Token: 0x0600442D RID: 17453
		bool IsAssignableFrom(Type c);

		// Token: 0x0600442E RID: 17454
		InterfaceMapping GetInterfaceMap(Type interfaceType);

		// Token: 0x0600442F RID: 17455
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06004430 RID: 17456
		MethodInfo GetMethod(string name, BindingFlags bindingAttr);

		// Token: 0x06004431 RID: 17457
		MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x06004432 RID: 17458
		FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06004433 RID: 17459
		FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06004434 RID: 17460
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

		// Token: 0x06004435 RID: 17461
		PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06004436 RID: 17462
		PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06004437 RID: 17463
		MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

		// Token: 0x06004438 RID: 17464
		MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x06004439 RID: 17465
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x0600443A RID: 17466
		Type UnderlyingSystemType { get; }

		// Token: 0x0600443B RID: 17467
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, CultureInfo culture);

		// Token: 0x0600443C RID: 17468
		object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args);

		// Token: 0x0600443D RID: 17469
		ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600443E RID: 17470
		ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600443F RID: 17471
		ConstructorInfo GetConstructor(Type[] types);

		// Token: 0x06004440 RID: 17472
		ConstructorInfo[] GetConstructors();

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06004441 RID: 17473
		ConstructorInfo TypeInitializer { get; }

		// Token: 0x06004442 RID: 17474
		MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06004443 RID: 17475
		MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06004444 RID: 17476
		MethodInfo GetMethod(string name, Type[] types);

		// Token: 0x06004445 RID: 17477
		MethodInfo GetMethod(string name);

		// Token: 0x06004446 RID: 17478
		MethodInfo[] GetMethods();

		// Token: 0x06004447 RID: 17479
		FieldInfo GetField(string name);

		// Token: 0x06004448 RID: 17480
		FieldInfo[] GetFields();

		// Token: 0x06004449 RID: 17481
		Type GetInterface(string name);

		// Token: 0x0600444A RID: 17482
		EventInfo GetEvent(string name);

		// Token: 0x0600444B RID: 17483
		PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600444C RID: 17484
		PropertyInfo GetProperty(string name, Type returnType, Type[] types);

		// Token: 0x0600444D RID: 17485
		PropertyInfo GetProperty(string name, Type[] types);

		// Token: 0x0600444E RID: 17486
		PropertyInfo GetProperty(string name, Type returnType);

		// Token: 0x0600444F RID: 17487
		PropertyInfo GetProperty(string name);

		// Token: 0x06004450 RID: 17488
		PropertyInfo[] GetProperties();

		// Token: 0x06004451 RID: 17489
		Type[] GetNestedTypes();

		// Token: 0x06004452 RID: 17490
		Type GetNestedType(string name);

		// Token: 0x06004453 RID: 17491
		MemberInfo[] GetMember(string name);

		// Token: 0x06004454 RID: 17492
		MemberInfo[] GetMembers();

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06004455 RID: 17493
		TypeAttributes Attributes { get; }

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06004456 RID: 17494
		bool IsNotPublic { get; }

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06004457 RID: 17495
		bool IsPublic { get; }

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004458 RID: 17496
		bool IsNestedPublic { get; }

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004459 RID: 17497
		bool IsNestedPrivate { get; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x0600445A RID: 17498
		bool IsNestedFamily { get; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x0600445B RID: 17499
		bool IsNestedAssembly { get; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x0600445C RID: 17500
		bool IsNestedFamANDAssem { get; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600445D RID: 17501
		bool IsNestedFamORAssem { get; }

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x0600445E RID: 17502
		bool IsAutoLayout { get; }

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x0600445F RID: 17503
		bool IsLayoutSequential { get; }

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06004460 RID: 17504
		bool IsExplicitLayout { get; }

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06004461 RID: 17505
		bool IsClass { get; }

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06004462 RID: 17506
		bool IsInterface { get; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06004463 RID: 17507
		bool IsValueType { get; }

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06004464 RID: 17508
		bool IsAbstract { get; }

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06004465 RID: 17509
		bool IsSealed { get; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004466 RID: 17510
		bool IsEnum { get; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06004467 RID: 17511
		bool IsSpecialName { get; }

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004468 RID: 17512
		bool IsImport { get; }

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004469 RID: 17513
		bool IsSerializable { get; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600446A RID: 17514
		bool IsAnsiClass { get; }

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600446B RID: 17515
		bool IsUnicodeClass { get; }

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600446C RID: 17516
		bool IsAutoClass { get; }

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x0600446D RID: 17517
		bool IsArray { get; }

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x0600446E RID: 17518
		bool IsByRef { get; }

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x0600446F RID: 17519
		bool IsPointer { get; }

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06004470 RID: 17520
		bool IsPrimitive { get; }

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06004471 RID: 17521
		bool IsCOMObject { get; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06004472 RID: 17522
		bool HasElementType { get; }

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06004473 RID: 17523
		bool IsContextful { get; }

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06004474 RID: 17524
		bool IsMarshalByRef { get; }

		// Token: 0x06004475 RID: 17525
		bool Equals(Type o);
	}
}
