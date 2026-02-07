using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Mono.Interop;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000748 RID: 1864
	public static class Marshal
	{
		// Token: 0x0600414E RID: 16718
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddRefInternal(IntPtr pUnk);

		// Token: 0x0600414F RID: 16719 RVA: 0x000E2006 File Offset: 0x000E0206
		public static int AddRef(IntPtr pUnk)
		{
			if (pUnk == IntPtr.Zero)
			{
				throw new ArgumentNullException("pUnk");
			}
			return Marshal.AddRefInternal(pUnk);
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static bool AreComObjectsAvailableForCleanup()
		{
			return false;
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x000E2026 File Offset: 0x000E0226
		public static void CleanupUnusedObjectsInCurrentContext()
		{
			if (Environment.IsRunningOnWindows)
			{
				throw new PlatformNotSupportedException();
			}
		}

		// Token: 0x06004152 RID: 16722
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr AllocCoTaskMem(int cb);

		// Token: 0x06004153 RID: 16723
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr AllocCoTaskMemSize(UIntPtr sizet);

		// Token: 0x06004154 RID: 16724
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr AllocHGlobal(IntPtr cb);

		// Token: 0x06004155 RID: 16725 RVA: 0x000E2035 File Offset: 0x000E0235
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static IntPtr AllocHGlobal(int cb)
		{
			return Marshal.AllocHGlobal((IntPtr)cb);
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x000479FC File Offset: 0x00045BFC
		public static object BindToMoniker(string monikerName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void ChangeWrapperHandleStrength(object otp, bool fIsWeak)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x000E2042 File Offset: 0x000E0242
		internal static void copy_to_unmanaged(Array source, int startIndex, IntPtr destination, int length)
		{
			Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, null);
		}

		// Token: 0x06004159 RID: 16729
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void copy_to_unmanaged_fixed(Array source, int startIndex, IntPtr destination, int length, void* fixed_source_element);

		// Token: 0x0600415A RID: 16730 RVA: 0x000E204F File Offset: 0x000E024F
		private static bool skip_fixed(Array array, int startIndex)
		{
			return startIndex < 0 || startIndex >= array.Length;
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x000E2064 File Offset: 0x000E0264
		internal unsafe static void copy_to_unmanaged(byte[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, null);
				return;
			}
			fixed (byte* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x000E20A0 File Offset: 0x000E02A0
		internal unsafe static void copy_to_unmanaged(char[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, null);
				return;
			}
			fixed (char* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x000E20DC File Offset: 0x000E02DC
		public unsafe static void Copy(byte[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (byte* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x000E2114 File Offset: 0x000E0314
		public unsafe static void Copy(char[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (char* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x000E214C File Offset: 0x000E034C
		public unsafe static void Copy(short[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (short* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x000E2184 File Offset: 0x000E0384
		public unsafe static void Copy(int[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (int* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x000E21BC File Offset: 0x000E03BC
		public unsafe static void Copy(long[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (long* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x000E21F4 File Offset: 0x000E03F4
		public unsafe static void Copy(float[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (float* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x000E222C File Offset: 0x000E042C
		public unsafe static void Copy(double[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (double* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x000E2264 File Offset: 0x000E0464
		public unsafe static void Copy(IntPtr[] source, int startIndex, IntPtr destination, int length)
		{
			if (Marshal.skip_fixed(source, startIndex))
			{
				Marshal.copy_to_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (IntPtr* ptr = &source[startIndex])
			{
				void* fixed_source_element = (void*)ptr;
				Marshal.copy_to_unmanaged_fixed(source, startIndex, destination, length, fixed_source_element);
			}
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x000E229C File Offset: 0x000E049C
		internal static void copy_from_unmanaged(IntPtr source, int startIndex, Array destination, int length)
		{
			Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, null);
		}

		// Token: 0x06004166 RID: 16742
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void copy_from_unmanaged_fixed(IntPtr source, int startIndex, Array destination, int length, void* fixed_destination_element);

		// Token: 0x06004167 RID: 16743 RVA: 0x000E22AC File Offset: 0x000E04AC
		public unsafe static void Copy(IntPtr source, byte[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (byte* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x000E22E4 File Offset: 0x000E04E4
		public unsafe static void Copy(IntPtr source, char[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (char* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x000E231C File Offset: 0x000E051C
		public unsafe static void Copy(IntPtr source, short[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (short* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x000E2354 File Offset: 0x000E0554
		public unsafe static void Copy(IntPtr source, int[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (int* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x000E238C File Offset: 0x000E058C
		public unsafe static void Copy(IntPtr source, long[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (long* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x000E23C4 File Offset: 0x000E05C4
		public unsafe static void Copy(IntPtr source, float[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (float* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x000E23FC File Offset: 0x000E05FC
		public unsafe static void Copy(IntPtr source, double[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (double* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x000E2434 File Offset: 0x000E0634
		public unsafe static void Copy(IntPtr source, IntPtr[] destination, int startIndex, int length)
		{
			if (Marshal.skip_fixed(destination, startIndex))
			{
				Marshal.copy_from_unmanaged(source, startIndex, destination, length);
				return;
			}
			fixed (IntPtr* ptr = &destination[startIndex])
			{
				void* fixed_destination_element = (void*)ptr;
				Marshal.copy_from_unmanaged_fixed(source, startIndex, destination, length, fixed_destination_element);
			}
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IntPtr CreateAggregatedObject(IntPtr pOuter, object o)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x000E246C File Offset: 0x000E066C
		public static IntPtr CreateAggregatedObject<T>(IntPtr pOuter, T o)
		{
			return Marshal.CreateAggregatedObject(pOuter, o);
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x000E247C File Offset: 0x000E067C
		public static object CreateWrapperOfType(object o, Type t)
		{
			__ComObject _ComObject = o as __ComObject;
			if (_ComObject == null)
			{
				throw new ArgumentException("o must derive from __ComObject", "o");
			}
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			foreach (Type type in o.GetType().GetInterfaces())
			{
				if (type.IsImport && _ComObject.GetInterface(type) == IntPtr.Zero)
				{
					throw new InvalidCastException();
				}
			}
			return ComInteropProxy.GetProxy(_ComObject.IUnknown, t).GetTransparentProxy();
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x000E2507 File Offset: 0x000E0707
		public static TWrapper CreateWrapperOfType<T, TWrapper>(T o)
		{
			return (TWrapper)((object)Marshal.CreateWrapperOfType(o, typeof(TWrapper)));
		}

		// Token: 0x06004173 RID: 16755
		[ComVisible(true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DestroyStructure(IntPtr ptr, Type structuretype);

		// Token: 0x06004174 RID: 16756 RVA: 0x000E2523 File Offset: 0x000E0723
		public static void DestroyStructure<T>(IntPtr ptr)
		{
			Marshal.DestroyStructure(ptr, typeof(T));
		}

		// Token: 0x06004175 RID: 16757
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FreeBSTR(IntPtr ptr);

		// Token: 0x06004176 RID: 16758
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FreeCoTaskMem(IntPtr ptr);

		// Token: 0x06004177 RID: 16759
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void FreeHGlobal(IntPtr hglobal);

		// Token: 0x06004178 RID: 16760 RVA: 0x000E2538 File Offset: 0x000E0738
		private static void ClearBSTR(IntPtr ptr)
		{
			int num = Marshal.ReadInt32(ptr, -4);
			for (int i = 0; i < num; i++)
			{
				Marshal.WriteByte(ptr, i, 0);
			}
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x000E2562 File Offset: 0x000E0762
		public static void ZeroFreeBSTR(IntPtr s)
		{
			Marshal.ClearBSTR(s);
			Marshal.FreeBSTR(s);
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x000E2570 File Offset: 0x000E0770
		private static void ClearAnsi(IntPtr ptr)
		{
			int num = 0;
			while (Marshal.ReadByte(ptr, num) != 0)
			{
				Marshal.WriteByte(ptr, num, 0);
				num++;
			}
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x000E2598 File Offset: 0x000E0798
		private static void ClearUnicode(IntPtr ptr)
		{
			int num = 0;
			while (Marshal.ReadInt16(ptr, num) != 0)
			{
				Marshal.WriteInt16(ptr, num, 0);
				num += 2;
			}
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x000E25BE File Offset: 0x000E07BE
		public static void ZeroFreeCoTaskMemAnsi(IntPtr s)
		{
			Marshal.ClearAnsi(s);
			Marshal.FreeCoTaskMem(s);
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x000E25CC File Offset: 0x000E07CC
		public static void ZeroFreeCoTaskMemUnicode(IntPtr s)
		{
			Marshal.ClearUnicode(s);
			Marshal.FreeCoTaskMem(s);
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x000E25BE File Offset: 0x000E07BE
		public static void ZeroFreeCoTaskMemUTF8(IntPtr s)
		{
			Marshal.ClearAnsi(s);
			Marshal.FreeCoTaskMem(s);
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x000E25DA File Offset: 0x000E07DA
		public static void ZeroFreeGlobalAllocAnsi(IntPtr s)
		{
			Marshal.ClearAnsi(s);
			Marshal.FreeHGlobal(s);
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x000E25E8 File Offset: 0x000E07E8
		public static void ZeroFreeGlobalAllocUnicode(IntPtr s)
		{
			Marshal.ClearUnicode(s);
			Marshal.FreeHGlobal(s);
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x000E25F6 File Offset: 0x000E07F6
		public static Guid GenerateGuidForType(Type type)
		{
			return type.GUID;
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x000E2600 File Offset: 0x000E0800
		public static string GenerateProgIdForType(Type type)
		{
			foreach (CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(type))
			{
				if (customAttributeData.Constructor.DeclaringType.Name == "ProgIdAttribute")
				{
					IList<CustomAttributeTypedArgument> constructorArguments = customAttributeData.ConstructorArguments;
					string text = customAttributeData.ConstructorArguments[0].Value as string;
					if (text == null)
					{
						text = string.Empty;
					}
					return text;
				}
			}
			return type.FullName;
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x000479FC File Offset: 0x00045BFC
		public static object GetActiveObject(string progID)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004184 RID: 16772
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetCCW(object o, Type T);

		// Token: 0x06004185 RID: 16773 RVA: 0x000E269C File Offset: 0x000E089C
		private static IntPtr GetComInterfaceForObjectInternal(object o, Type T)
		{
			if (Marshal.IsComObject(o))
			{
				return ((__ComObject)o).GetInterface(T);
			}
			return Marshal.GetCCW(o, T);
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x000E26BA File Offset: 0x000E08BA
		public static IntPtr GetComInterfaceForObject(object o, Type T)
		{
			IntPtr comInterfaceForObjectInternal = Marshal.GetComInterfaceForObjectInternal(o, T);
			Marshal.AddRef(comInterfaceForObjectInternal);
			return comInterfaceForObjectInternal;
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IntPtr GetComInterfaceForObject(object o, Type T, CustomQueryInterfaceMode mode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x000E26CA File Offset: 0x000E08CA
		public static IntPtr GetComInterfaceForObject<T, TInterface>(T o)
		{
			return Marshal.GetComInterfaceForObject(o, typeof(T));
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IntPtr GetComInterfaceForObjectInContext(object o, Type t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x000E26E1 File Offset: 0x000E08E1
		public static object GetComObjectData(object obj, object key)
		{
			throw new NotSupportedException("MSDN states user code should never need to call this method.");
		}

		// Token: 0x0600418B RID: 16779
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetComSlotForMethodInfoInternal(MemberInfo m);

		// Token: 0x0600418C RID: 16780 RVA: 0x000E26F0 File Offset: 0x000E08F0
		public static int GetComSlotForMethodInfo(MemberInfo m)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			if (!(m is MethodInfo))
			{
				throw new ArgumentException("The MemberInfo must be an interface method.", "m");
			}
			if (!m.DeclaringType.IsInterface)
			{
				throw new ArgumentException("The MemberInfo must be an interface method.", "m");
			}
			return Marshal.GetComSlotForMethodInfoInternal(m);
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x000479FC File Offset: 0x00045BFC
		public static int GetEndComSlot(Type t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x000479FC File Offset: 0x00045BFC
		[ComVisible(true)]
		public static IntPtr GetExceptionPointers()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x000E274C File Offset: 0x000E094C
		public static IntPtr GetHINSTANCE(Module m)
		{
			if (m == null)
			{
				throw new ArgumentNullException("m");
			}
			RuntimeModule runtimeModule = m as RuntimeModule;
			if (runtimeModule != null)
			{
				return RuntimeModule.GetHINSTANCE(runtimeModule.MonoModule);
			}
			return (IntPtr)(-1);
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x0001B98F File Offset: 0x00019B8F
		public static int GetExceptionCode()
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x000E278C File Offset: 0x000E098C
		public static int GetHRForException(Exception e)
		{
			if (e == null)
			{
				return 0;
			}
			ManagedErrorInfo errorInfo = new ManagedErrorInfo(e);
			Marshal.SetErrorInfo(0, errorInfo);
			return e._HResult;
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x000479FC File Offset: 0x00045BFC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int GetHRForLastWin32Error()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004193 RID: 16787
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetIDispatchForObjectInternal(object o);

		// Token: 0x06004194 RID: 16788 RVA: 0x000E27B3 File Offset: 0x000E09B3
		public static IntPtr GetIDispatchForObject(object o)
		{
			IntPtr idispatchForObjectInternal = Marshal.GetIDispatchForObjectInternal(o);
			Marshal.AddRef(idispatchForObjectInternal);
			return idispatchForObjectInternal;
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IntPtr GetIDispatchForObjectInContext(object o)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IntPtr GetITypeInfoForType(Type t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x000479FC File Offset: 0x00045BFC
		public static IntPtr GetIUnknownForObjectInContext(object o)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete("This method has been deprecated")]
		public static IntPtr GetManagedThunkForUnmanagedMethodPtr(IntPtr pfnMethodToWrap, IntPtr pbSignature, int cbSignature)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x000479FC File Offset: 0x00045BFC
		public static MemberInfo GetMethodInfoForComSlot(Type t, int slot, ref ComMemberType memberType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600419A RID: 16794
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetIUnknownForObjectInternal(object o);

		// Token: 0x0600419B RID: 16795 RVA: 0x000E27C2 File Offset: 0x000E09C2
		public static IntPtr GetIUnknownForObject(object o)
		{
			IntPtr iunknownForObjectInternal = Marshal.GetIUnknownForObjectInternal(o);
			Marshal.AddRef(iunknownForObjectInternal);
			return iunknownForObjectInternal;
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x000E27D4 File Offset: 0x000E09D4
		public static void GetNativeVariantForObject(object obj, IntPtr pDstNativeVariant)
		{
			Variant structure = default(Variant);
			structure.SetValue(obj);
			Marshal.StructureToPtr<Variant>(structure, pDstNativeVariant, false);
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x000E27F9 File Offset: 0x000E09F9
		public static void GetNativeVariantForObject<T>(T obj, IntPtr pDstNativeVariant)
		{
			Marshal.GetNativeVariantForObject(obj, pDstNativeVariant);
		}

		// Token: 0x0600419E RID: 16798
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object GetObjectForCCW(IntPtr pUnk);

		// Token: 0x0600419F RID: 16799 RVA: 0x000E2808 File Offset: 0x000E0A08
		public static object GetObjectForIUnknown(IntPtr pUnk)
		{
			object obj = Marshal.GetObjectForCCW(pUnk);
			if (obj == null)
			{
				obj = ComInteropProxy.GetProxy(pUnk, typeof(__ComObject)).GetTransparentProxy();
			}
			return obj;
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x000E2838 File Offset: 0x000E0A38
		public static object GetObjectForNativeVariant(IntPtr pSrcNativeVariant)
		{
			return ((Variant)Marshal.PtrToStructure(pSrcNativeVariant, typeof(Variant))).GetValue();
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x000E2864 File Offset: 0x000E0A64
		public static T GetObjectForNativeVariant<T>(IntPtr pSrcNativeVariant)
		{
			return (T)((object)((Variant)Marshal.PtrToStructure(pSrcNativeVariant, typeof(Variant))).GetValue());
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x000E2894 File Offset: 0x000E0A94
		public static object[] GetObjectsForNativeVariants(IntPtr aSrcNativeVariant, int cVars)
		{
			if (cVars < 0)
			{
				throw new ArgumentOutOfRangeException("cVars", "cVars cannot be a negative number.");
			}
			object[] array = new object[cVars];
			for (int i = 0; i < cVars; i++)
			{
				array[i] = Marshal.GetObjectForNativeVariant((IntPtr)(aSrcNativeVariant.ToInt64() + (long)(i * Marshal.SizeOf(typeof(Variant)))));
			}
			return array;
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x000E28F0 File Offset: 0x000E0AF0
		public static T[] GetObjectsForNativeVariants<T>(IntPtr aSrcNativeVariant, int cVars)
		{
			if (cVars < 0)
			{
				throw new ArgumentOutOfRangeException("cVars", "cVars cannot be a negative number.");
			}
			T[] array = new T[cVars];
			for (int i = 0; i < cVars; i++)
			{
				array[i] = Marshal.GetObjectForNativeVariant<T>((IntPtr)(aSrcNativeVariant.ToInt64() + (long)(i * Marshal.SizeOf(typeof(Variant)))));
			}
			return array;
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x000479FC File Offset: 0x00045BFC
		public static int GetStartComSlot(Type t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete("This method has been deprecated")]
		public static Thread GetThreadFromFiberCookie(int cookie)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x000E2950 File Offset: 0x000E0B50
		public static object GetTypedObjectForIUnknown(IntPtr pUnk, Type t)
		{
			__ComObject _ComObject = (__ComObject)new ComInteropProxy(pUnk, t).GetTransparentProxy();
			foreach (Type type in t.GetInterfaces())
			{
				if ((type.Attributes & TypeAttributes.Import) == TypeAttributes.Import && _ComObject.GetInterface(type) == IntPtr.Zero)
				{
					return null;
				}
			}
			return _ComObject;
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x000479FC File Offset: 0x00045BFC
		public static Type GetTypeForITypeInfo(IntPtr piTypeInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete]
		public static string GetTypeInfoName(UCOMITypeInfo pTI)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete]
		public static Guid GetTypeLibGuid(UCOMITypeLib pTLB)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x000479FC File Offset: 0x00045BFC
		public static Guid GetTypeLibGuid(ITypeLib typelib)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x000479FC File Offset: 0x00045BFC
		public static Guid GetTypeLibGuidForAssembly(Assembly asm)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete]
		public static int GetTypeLibLcid(UCOMITypeLib pTLB)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x000479FC File Offset: 0x00045BFC
		public static int GetTypeLibLcid(ITypeLib typelib)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete]
		public static string GetTypeLibName(UCOMITypeLib pTLB)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x000479FC File Offset: 0x00045BFC
		public static string GetTypeLibName(ITypeLib typelib)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void GetTypeLibVersionForAssembly(Assembly inputAssembly, out int majorVersion, out int minorVersion)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete("This method has been deprecated")]
		public static IntPtr GetUnmanagedThunkForManagedMethodPtr(IntPtr pfnMethodToWrap, IntPtr pbSignature, int cbSignature)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x000479FC File Offset: 0x00045BFC
		public static bool IsTypeVisibleFromCom(Type t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x000479FC File Offset: 0x00045BFC
		public static int NumParamBytes(MethodInfo m)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0001B98F File Offset: 0x00019B8F
		public static Type GetTypeFromCLSID(Guid clsid)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x0001B98F File Offset: 0x00019B8F
		public static string GetTypeInfoName(ITypeInfo typeInfo)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x0001B98F File Offset: 0x00019B8F
		public static object GetUniqueObjectForIUnknown(IntPtr unknown)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x060041B7 RID: 16823
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsComObject(object o);

		// Token: 0x060041B8 RID: 16824
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLastWin32Error();

		// Token: 0x060041B9 RID: 16825
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr OffsetOf(Type t, string fieldName);

		// Token: 0x060041BA RID: 16826 RVA: 0x000E29B1 File Offset: 0x000E0BB1
		public static IntPtr OffsetOf<T>(string fieldName)
		{
			return Marshal.OffsetOf(typeof(T), fieldName);
		}

		// Token: 0x060041BB RID: 16827
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Prelink(MethodInfo m);

		// Token: 0x060041BC RID: 16828
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PrelinkAll(Type c);

		// Token: 0x060041BD RID: 16829
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringAnsi(IntPtr ptr);

		// Token: 0x060041BE RID: 16830
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringAnsi(IntPtr ptr, int len);

		// Token: 0x060041BF RID: 16831 RVA: 0x000E29C3 File Offset: 0x000E0BC3
		public static string PtrToStringUTF8(IntPtr ptr)
		{
			return Marshal.PtrToStringAnsi(ptr);
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x000E29CB File Offset: 0x000E0BCB
		public static string PtrToStringUTF8(IntPtr ptr, int byteLen)
		{
			return Marshal.PtrToStringAnsi(ptr, byteLen);
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x000E29D4 File Offset: 0x000E0BD4
		public static string PtrToStringAuto(IntPtr ptr)
		{
			if (Marshal.SystemDefaultCharSize != 2)
			{
				return Marshal.PtrToStringAnsi(ptr);
			}
			return Marshal.PtrToStringUni(ptr);
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x000E29EB File Offset: 0x000E0BEB
		public static string PtrToStringAuto(IntPtr ptr, int len)
		{
			if (Marshal.SystemDefaultCharSize != 2)
			{
				return Marshal.PtrToStringAnsi(ptr, len);
			}
			return Marshal.PtrToStringUni(ptr, len);
		}

		// Token: 0x060041C3 RID: 16835
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringUni(IntPtr ptr);

		// Token: 0x060041C4 RID: 16836
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringUni(IntPtr ptr, int len);

		// Token: 0x060041C5 RID: 16837
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string PtrToStringBSTR(IntPtr ptr);

		// Token: 0x060041C6 RID: 16838
		[ComVisible(true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PtrToStructure(IntPtr ptr, object structure);

		// Token: 0x060041C7 RID: 16839
		[ComVisible(true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object PtrToStructure(IntPtr ptr, Type structureType);

		// Token: 0x060041C8 RID: 16840 RVA: 0x000E2A04 File Offset: 0x000E0C04
		public static void PtrToStructure<T>(IntPtr ptr, T structure)
		{
			Marshal.PtrToStructure(ptr, structure);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x000E2A12 File Offset: 0x000E0C12
		public static T PtrToStructure<T>(IntPtr ptr)
		{
			return (T)((object)Marshal.PtrToStructure(ptr, typeof(T)));
		}

		// Token: 0x060041CA RID: 16842
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int QueryInterfaceInternal(IntPtr pUnk, ref Guid iid, out IntPtr ppv);

		// Token: 0x060041CB RID: 16843 RVA: 0x000E2A29 File Offset: 0x000E0C29
		public static int QueryInterface(IntPtr pUnk, ref Guid iid, out IntPtr ppv)
		{
			if (pUnk == IntPtr.Zero)
			{
				throw new ArgumentNullException("pUnk");
			}
			return Marshal.QueryInterfaceInternal(pUnk, ref iid, out ppv);
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x000E2A4B File Offset: 0x000E0C4B
		public unsafe static byte ReadByte(IntPtr ptr)
		{
			return *(byte*)((void*)ptr);
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x000E2A54 File Offset: 0x000E0C54
		public unsafe static byte ReadByte(IntPtr ptr, int ofs)
		{
			return ((byte*)((void*)ptr))[ofs];
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x000479FC File Offset: 0x00045BFC
		[SuppressUnmanagedCodeSecurity]
		public static byte ReadByte([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x000E2A60 File Offset: 0x000E0C60
		public unsafe static short ReadInt16(IntPtr ptr)
		{
			byte* ptr2 = (byte*)((void*)ptr);
			if ((ptr2 & 1U) == 0U)
			{
				return *(short*)ptr2;
			}
			short result;
			Buffer.Memcpy((byte*)(&result), (byte*)((void*)ptr), 2);
			return result;
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x000E2A90 File Offset: 0x000E0C90
		public unsafe static short ReadInt16(IntPtr ptr, int ofs)
		{
			byte* ptr2 = (byte*)((void*)ptr) + ofs;
			if ((ptr2 & 1U) == 0U)
			{
				return *(short*)ptr2;
			}
			short result;
			Buffer.Memcpy((byte*)(&result), ptr2, 2);
			return result;
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x000479FC File Offset: 0x00045BFC
		[SuppressUnmanagedCodeSecurity]
		public static short ReadInt16([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x000E2ABC File Offset: 0x000E0CBC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public unsafe static int ReadInt32(IntPtr ptr)
		{
			byte* ptr2 = (byte*)((void*)ptr);
			if ((ptr2 & 3U) == 0U)
			{
				return *(int*)ptr2;
			}
			int result;
			Buffer.Memcpy((byte*)(&result), ptr2, 4);
			return result;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x000E2AE4 File Offset: 0x000E0CE4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public unsafe static int ReadInt32(IntPtr ptr, int ofs)
		{
			byte* ptr2 = (byte*)((void*)ptr) + ofs;
			if ((ptr2 & 3) == 0)
			{
				return *(int*)ptr2;
			}
			int result;
			Buffer.Memcpy((byte*)(&result), ptr2, 4);
			return result;
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x000479FC File Offset: 0x00045BFC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		public static int ReadInt32([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x000E2B10 File Offset: 0x000E0D10
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public unsafe static long ReadInt64(IntPtr ptr)
		{
			byte* ptr2 = (byte*)((void*)ptr);
			if ((ptr2 & 7U) == 0U)
			{
				return *(long*)((void*)ptr);
			}
			long result;
			Buffer.Memcpy((byte*)(&result), ptr2, 8);
			return result;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x000E2B40 File Offset: 0x000E0D40
		public unsafe static long ReadInt64(IntPtr ptr, int ofs)
		{
			byte* ptr2 = (byte*)((void*)ptr) + ofs;
			if ((ptr2 & 7U) == 0U)
			{
				return *(long*)ptr2;
			}
			long result;
			Buffer.Memcpy((byte*)(&result), ptr2, 8);
			return result;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x000479FC File Offset: 0x00045BFC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		public static long ReadInt64([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x000E2B6A File Offset: 0x000E0D6A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static IntPtr ReadIntPtr(IntPtr ptr)
		{
			if (IntPtr.Size == 4)
			{
				return (IntPtr)Marshal.ReadInt32(ptr);
			}
			return (IntPtr)Marshal.ReadInt64(ptr);
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x000E2B8B File Offset: 0x000E0D8B
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static IntPtr ReadIntPtr(IntPtr ptr, int ofs)
		{
			if (IntPtr.Size == 4)
			{
				return (IntPtr)Marshal.ReadInt32(ptr, ofs);
			}
			return (IntPtr)Marshal.ReadInt64(ptr, ofs);
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x000479FC File Offset: 0x00045BFC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static IntPtr ReadIntPtr([MarshalAs(UnmanagedType.AsAny)] [In] object ptr, int ofs)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041DB RID: 16859
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ReAllocCoTaskMem(IntPtr pv, int cb);

		// Token: 0x060041DC RID: 16860
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr ReAllocHGlobal(IntPtr pv, IntPtr cb);

		// Token: 0x060041DD RID: 16861
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ReleaseInternal(IntPtr pUnk);

		// Token: 0x060041DE RID: 16862 RVA: 0x000E2BAE File Offset: 0x000E0DAE
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Release(IntPtr pUnk)
		{
			if (pUnk == IntPtr.Zero)
			{
				throw new ArgumentNullException("pUnk");
			}
			return Marshal.ReleaseInternal(pUnk);
		}

		// Token: 0x060041DF RID: 16863
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ReleaseComObjectInternal(object co);

		// Token: 0x060041E0 RID: 16864 RVA: 0x000E2BCE File Offset: 0x000E0DCE
		public static int ReleaseComObject(object o)
		{
			if (o == null)
			{
				throw new ArgumentException("Value cannot be null.", "o");
			}
			if (!Marshal.IsComObject(o))
			{
				throw new ArgumentException("Value must be a Com object.", "o");
			}
			return Marshal.ReleaseComObjectInternal(o);
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x000479FC File Offset: 0x00045BFC
		[Obsolete]
		public static void ReleaseThreadCache()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x000E26E1 File Offset: 0x000E08E1
		public static bool SetComObjectData(object obj, object key, object data)
		{
			throw new NotSupportedException("MSDN states user code should never need to call this method.");
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x000E2C01 File Offset: 0x000E0E01
		[ComVisible(true)]
		public static int SizeOf(object structure)
		{
			return Marshal.SizeOf(structure.GetType());
		}

		// Token: 0x060041E4 RID: 16868
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int SizeOf(Type t);

		// Token: 0x060041E5 RID: 16869 RVA: 0x000E2C0E File Offset: 0x000E0E0E
		public static int SizeOf<T>()
		{
			return Marshal.SizeOf(typeof(T));
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x000E2C1F File Offset: 0x000E0E1F
		public static int SizeOf<T>(T structure)
		{
			return Marshal.SizeOf(structure.GetType());
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x000E2C33 File Offset: 0x000E0E33
		internal static uint SizeOfType(Type type)
		{
			return (uint)Marshal.SizeOf(type);
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x000E2C3C File Offset: 0x000E0E3C
		internal static uint AlignedSizeOf<T>() where T : struct
		{
			uint num = Marshal.SizeOfType(typeof(T));
			if (num == 1U || num == 2U)
			{
				return num;
			}
			if (IntPtr.Size == 8 && num == 4U)
			{
				return num;
			}
			return num + 3U & 4294967292U;
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x000E2C78 File Offset: 0x000E0E78
		public unsafe static IntPtr StringToBSTR(string s)
		{
			if (s == null)
			{
				return IntPtr.Zero;
			}
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return Marshal.BufferToBSTR(ptr, s.Length);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x000E2CAA File Offset: 0x000E0EAA
		public static IntPtr StringToCoTaskMemAnsi(string s)
		{
			return Marshal.StringToAllocatedMemoryUTF8(s);
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x000E2CB2 File Offset: 0x000E0EB2
		public static IntPtr StringToCoTaskMemAuto(string s)
		{
			if (Marshal.SystemDefaultCharSize != 2)
			{
				return Marshal.StringToCoTaskMemAnsi(s);
			}
			return Marshal.StringToCoTaskMemUni(s);
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x000E2CCC File Offset: 0x000E0ECC
		public static IntPtr StringToCoTaskMemUni(string s)
		{
			int num = s.Length + 1;
			IntPtr intPtr = Marshal.AllocCoTaskMem(num * 2);
			char[] array = new char[num];
			s.CopyTo(0, array, 0, s.Length);
			array[s.Length] = '\0';
			Marshal.copy_to_unmanaged(array, 0, intPtr, num);
			return intPtr;
		}

		// Token: 0x060041ED RID: 16877
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr StringToHGlobalAnsi(char* s, int length);

		// Token: 0x060041EE RID: 16878 RVA: 0x000E2D14 File Offset: 0x000E0F14
		public unsafe static IntPtr StringToHGlobalAnsi(string s)
		{
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return Marshal.StringToHGlobalAnsi(ptr, (s != null) ? s.Length : 0);
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x000E2D44 File Offset: 0x000E0F44
		public unsafe static IntPtr StringToAllocatedMemoryUTF8(string s)
		{
			if (s == null)
			{
				return IntPtr.Zero;
			}
			int num = (s.Length + 1) * 3;
			if (num < s.Length)
			{
				throw new ArgumentOutOfRangeException("s");
			}
			IntPtr intPtr = Marshal.AllocCoTaskMemSize(new UIntPtr((uint)(num + 1)));
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException();
			}
			byte* ptr = (byte*)((void*)intPtr);
			fixed (string text = s)
			{
				char* ptr2 = text;
				if (ptr2 != null)
				{
					ptr2 += RuntimeHelpers.OffsetToStringData / 2;
				}
				int bytes = Encoding.UTF8.GetBytes(ptr2, s.Length, ptr, num);
				ptr[bytes] = 0;
			}
			return intPtr;
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x000E2DCD File Offset: 0x000E0FCD
		public static IntPtr StringToHGlobalAuto(string s)
		{
			if (Marshal.SystemDefaultCharSize != 2)
			{
				return Marshal.StringToHGlobalAnsi(s);
			}
			return Marshal.StringToHGlobalUni(s);
		}

		// Token: 0x060041F1 RID: 16881
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr StringToHGlobalUni(char* s, int length);

		// Token: 0x060041F2 RID: 16882 RVA: 0x000E2DE4 File Offset: 0x000E0FE4
		public unsafe static IntPtr StringToHGlobalUni(string s)
		{
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return Marshal.StringToHGlobalUni(ptr, (s != null) ? s.Length : 0);
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x000E2E14 File Offset: 0x000E1014
		public unsafe static IntPtr SecureStringToBSTR(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			byte[] buffer = s.GetBuffer();
			int length = s.Length;
			if (BitConverter.IsLittleEndian)
			{
				for (int i = 0; i < buffer.Length; i += 2)
				{
					byte b = buffer[i];
					buffer[i] = buffer[i + 1];
					buffer[i + 1] = b;
				}
			}
			byte[] array;
			byte* ptr;
			if ((array = buffer) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			return Marshal.BufferToBSTR((char*)ptr, length);
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x000E2E89 File Offset: 0x000E1089
		internal static IntPtr SecureStringCoTaskMemAllocator(int len)
		{
			return Marshal.AllocCoTaskMem(len);
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x000E2E91 File Offset: 0x000E1091
		internal static IntPtr SecureStringGlobalAllocator(int len)
		{
			return Marshal.AllocHGlobal(len);
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x000E2E9C File Offset: 0x000E109C
		internal static IntPtr SecureStringToAnsi(SecureString s, Marshal.SecureStringAllocator allocator)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			int length = s.Length;
			IntPtr intPtr = allocator(length + 1);
			byte[] array = new byte[length + 1];
			try
			{
				byte[] buffer = s.GetBuffer();
				int i = 0;
				int num = 0;
				while (i < length)
				{
					array[i] = buffer[num + 1];
					buffer[num] = 0;
					buffer[num + 1] = 0;
					i++;
					num += 2;
				}
				array[i] = 0;
				Marshal.copy_to_unmanaged(array, 0, intPtr, length + 1);
			}
			finally
			{
				int j = length;
				while (j > 0)
				{
					j--;
					array[j] = 0;
				}
			}
			return intPtr;
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x000E2F40 File Offset: 0x000E1140
		internal static IntPtr SecureStringToUnicode(SecureString s, Marshal.SecureStringAllocator allocator)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			int length = s.Length;
			IntPtr intPtr = allocator(length * 2 + 2);
			byte[] array = null;
			try
			{
				array = s.GetBuffer();
				for (int i = 0; i < length; i++)
				{
					Marshal.WriteInt16(intPtr, i * 2, (short)((int)array[i * 2] << 8 | (int)array[i * 2 + 1]));
				}
				Marshal.WriteInt16(intPtr, array.Length, 0);
			}
			finally
			{
				if (array != null)
				{
					int j = array.Length;
					while (j > 0)
					{
						j--;
						array[j] = 0;
					}
				}
			}
			return intPtr;
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x000E2FD4 File Offset: 0x000E11D4
		public static IntPtr SecureStringToCoTaskMemAnsi(SecureString s)
		{
			return Marshal.SecureStringToAnsi(s, new Marshal.SecureStringAllocator(Marshal.SecureStringCoTaskMemAllocator));
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x000E2FE8 File Offset: 0x000E11E8
		public static IntPtr SecureStringToCoTaskMemUnicode(SecureString s)
		{
			return Marshal.SecureStringToUnicode(s, new Marshal.SecureStringAllocator(Marshal.SecureStringCoTaskMemAllocator));
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x000E2FFC File Offset: 0x000E11FC
		public static IntPtr SecureStringToGlobalAllocAnsi(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return Marshal.SecureStringToAnsi(s, new Marshal.SecureStringAllocator(Marshal.SecureStringGlobalAllocator));
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x000E301E File Offset: 0x000E121E
		public static IntPtr SecureStringToGlobalAllocUnicode(SecureString s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return Marshal.SecureStringToUnicode(s, new Marshal.SecureStringAllocator(Marshal.SecureStringGlobalAllocator));
		}

		// Token: 0x060041FC RID: 16892
		[ComVisible(true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld);

		// Token: 0x060041FD RID: 16893 RVA: 0x000E3040 File Offset: 0x000E1240
		public static void StructureToPtr<T>(T structure, IntPtr ptr, bool fDeleteOld)
		{
			Marshal.StructureToPtr(structure, ptr, fDeleteOld);
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x000E3050 File Offset: 0x000E1250
		public static void ThrowExceptionForHR(int errorCode)
		{
			Exception exceptionForHR = Marshal.GetExceptionForHR(errorCode);
			if (exceptionForHR != null)
			{
				throw exceptionForHR;
			}
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x000E306C File Offset: 0x000E126C
		public static void ThrowExceptionForHR(int errorCode, IntPtr errorInfo)
		{
			Exception exceptionForHR = Marshal.GetExceptionForHR(errorCode, errorInfo);
			if (exceptionForHR != null)
			{
				throw exceptionForHR;
			}
		}

		// Token: 0x06004200 RID: 16896
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr BufferToBSTR(char* ptr, int slen);

		// Token: 0x06004201 RID: 16897
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr UnsafeAddrOfPinnedArrayElement(Array arr, int index);

		// Token: 0x06004202 RID: 16898 RVA: 0x000E3086 File Offset: 0x000E1286
		public static IntPtr UnsafeAddrOfPinnedArrayElement<T>(T[] arr, int index)
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(arr, index);
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x000E308F File Offset: 0x000E128F
		public unsafe static void WriteByte(IntPtr ptr, byte val)
		{
			*(byte*)((void*)ptr) = val;
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x000E3099 File Offset: 0x000E1299
		public unsafe static void WriteByte(IntPtr ptr, int ofs, byte val)
		{
			*(byte*)((void*)IntPtr.Add(ptr, ofs)) = val;
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x000479FC File Offset: 0x00045BFC
		[SuppressUnmanagedCodeSecurity]
		public static void WriteByte([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, byte val)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x000E30AC File Offset: 0x000E12AC
		public unsafe static void WriteInt16(IntPtr ptr, short val)
		{
			byte* ptr2 = (byte*)((void*)ptr);
			if ((ptr2 & 1U) == 0U)
			{
				*(short*)ptr2 = val;
				return;
			}
			Buffer.Memcpy(ptr2, (byte*)(&val), 2);
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x000E30D4 File Offset: 0x000E12D4
		public unsafe static void WriteInt16(IntPtr ptr, int ofs, short val)
		{
			byte* ptr2 = (byte*)((void*)ptr) + ofs;
			if ((ptr2 & 1U) == 0U)
			{
				*(short*)ptr2 = val;
				return;
			}
			Buffer.Memcpy(ptr2, (byte*)(&val), 2);
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x000479FC File Offset: 0x00045BFC
		[SuppressUnmanagedCodeSecurity]
		public static void WriteInt16([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, short val)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x000E30FE File Offset: 0x000E12FE
		public static void WriteInt16(IntPtr ptr, char val)
		{
			Marshal.WriteInt16(ptr, 0, (short)val);
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x000E3109 File Offset: 0x000E1309
		public static void WriteInt16(IntPtr ptr, int ofs, char val)
		{
			Marshal.WriteInt16(ptr, ofs, (short)val);
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void WriteInt16([In] [Out] object ptr, int ofs, char val)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x000E3114 File Offset: 0x000E1314
		public unsafe static void WriteInt32(IntPtr ptr, int val)
		{
			byte* ptr2 = (byte*)((void*)ptr);
			if ((ptr2 & 3U) == 0U)
			{
				*(int*)ptr2 = val;
				return;
			}
			Buffer.Memcpy(ptr2, (byte*)(&val), 4);
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x000E313C File Offset: 0x000E133C
		public unsafe static void WriteInt32(IntPtr ptr, int ofs, int val)
		{
			byte* ptr2 = (byte*)((void*)ptr) + ofs;
			if ((ptr2 & 3U) == 0U)
			{
				*(int*)ptr2 = val;
				return;
			}
			Buffer.Memcpy(ptr2, (byte*)(&val), 4);
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x000479FC File Offset: 0x00045BFC
		[SuppressUnmanagedCodeSecurity]
		public static void WriteInt32([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, int val)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x000E3168 File Offset: 0x000E1368
		public unsafe static void WriteInt64(IntPtr ptr, long val)
		{
			byte* ptr2 = (byte*)((void*)ptr);
			if ((ptr2 & 7U) == 0U)
			{
				*(long*)ptr2 = val;
				return;
			}
			Buffer.Memcpy(ptr2, (byte*)(&val), 8);
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x000E3190 File Offset: 0x000E1390
		public unsafe static void WriteInt64(IntPtr ptr, int ofs, long val)
		{
			byte* ptr2 = (byte*)((void*)ptr) + ofs;
			if ((ptr2 & 7U) == 0U)
			{
				*(long*)ptr2 = val;
				return;
			}
			Buffer.Memcpy(ptr2, (byte*)(&val), 8);
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x000479FC File Offset: 0x00045BFC
		[SuppressUnmanagedCodeSecurity]
		public static void WriteInt64([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, long val)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x000E31BA File Offset: 0x000E13BA
		public static void WriteIntPtr(IntPtr ptr, IntPtr val)
		{
			if (IntPtr.Size == 4)
			{
				Marshal.WriteInt32(ptr, (int)val);
				return;
			}
			Marshal.WriteInt64(ptr, (long)val);
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x000E31DD File Offset: 0x000E13DD
		public static void WriteIntPtr(IntPtr ptr, int ofs, IntPtr val)
		{
			if (IntPtr.Size == 4)
			{
				Marshal.WriteInt32(ptr, ofs, (int)val);
				return;
			}
			Marshal.WriteInt64(ptr, ofs, (long)val);
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x000479FC File Offset: 0x00045BFC
		public static void WriteIntPtr([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object ptr, int ofs, IntPtr val)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x000E3204 File Offset: 0x000E1404
		private static Exception ConvertHrToException(int errorCode)
		{
			if (errorCode > -2147024362)
			{
				if (errorCode <= -2146232828)
				{
					if (errorCode <= -2146893792)
					{
						if (errorCode != -2147023895)
						{
							if (errorCode != -2146893792)
							{
								goto IL_3A9;
							}
							return new CryptographicException();
						}
					}
					else
					{
						if (errorCode == -2146234348)
						{
							return new AppDomainUnloadedException();
						}
						switch (errorCode)
						{
						case -2146233088:
							return new Exception();
						case -2146233087:
							return new SystemException();
						case -2146233086:
							return new ArgumentOutOfRangeException();
						case -2146233085:
							return new ArrayTypeMismatchException();
						case -2146233084:
							return new ContextMarshalException();
						case -2146233083:
						case -2146233074:
						case -2146233073:
						case -2146233061:
						case -2146233060:
						case -2146233059:
						case -2146233058:
						case -2146233057:
						case -2146233055:
						case -2146233053:
						case -2146233052:
						case -2146233051:
						case -2146233050:
						case -2146233046:
						case -2146233045:
						case -2146233044:
						case -2146233043:
						case -2146233042:
						case -2146233041:
						case -2146233040:
						case -2146233035:
						case -2146233034:
							goto IL_3A9;
						case -2146233082:
							return new ExecutionEngineException();
						case -2146233081:
							return new FieldAccessException();
						case -2146233080:
							return new IndexOutOfRangeException();
						case -2146233079:
							return new InvalidOperationException();
						case -2146233078:
							return new SecurityException();
						case -2146233077:
							return new RemotingException();
						case -2146233076:
							return new SerializationException();
						case -2146233075:
							return new VerificationException();
						case -2146233072:
							return new MethodAccessException();
						case -2146233071:
							return new MissingFieldException();
						case -2146233070:
							return new MissingMemberException();
						case -2146233069:
							return new MissingMethodException();
						case -2146233068:
							return new MulticastNotSupportedException();
						case -2146233067:
							return new NotSupportedException();
						case -2146233066:
							return new OverflowException();
						case -2146233065:
							return new RankException();
						case -2146233064:
							return new SynchronizationLockException();
						case -2146233063:
							return new ThreadInterruptedException();
						case -2146233062:
							return new MemberAccessException();
						case -2146233056:
							return new ThreadStateException();
						case -2146233054:
							return new TypeLoadException();
						case -2146233049:
							return new InvalidComObjectException();
						case -2146233048:
							return new NotFiniteNumberException();
						case -2146233047:
							return new DuplicateWaitObjectException();
						case -2146233039:
							return new InvalidOleVariantTypeException();
						case -2146233038:
							return new MissingManifestResourceException();
						case -2146233037:
							return new SafeArrayTypeMismatchException();
						case -2146233036:
							return new TypeInitializationException("", null);
						case -2146233033:
							return new FormatException();
						default:
							switch (errorCode)
							{
							case -2146232832:
								return new ApplicationException();
							case -2146232831:
								return new InvalidFilterCriteriaException();
							case -2146232830:
								return new ReflectionTypeLoadException(new Type[0], new Exception[0]);
							case -2146232829:
								return new TargetException();
							case -2146232828:
								return new TargetInvocationException(null);
							default:
								goto IL_3A9;
							}
							break;
						}
					}
				}
				else if (errorCode <= 3)
				{
					if (errorCode == -2146232800)
					{
						return new IOException();
					}
					if (errorCode == 2)
					{
						goto IL_2A6;
					}
					if (errorCode != 3)
					{
						goto IL_3A9;
					}
					goto IL_27C;
				}
				else
				{
					if (errorCode == 11)
					{
						goto IL_26A;
					}
					if (errorCode == 206)
					{
						goto IL_32A;
					}
					if (errorCode != 1001)
					{
						goto IL_3A9;
					}
				}
				return new StackOverflowException();
			}
			if (errorCode <= -2147024893)
			{
				if (errorCode <= -2147352562)
				{
					switch (errorCode)
					{
					case -2147467263:
						return new NotImplementedException();
					case -2147467262:
						return new InvalidCastException();
					case -2147467261:
						return new NullReferenceException();
					default:
						if (errorCode != -2147352562)
						{
							goto IL_3A9;
						}
						return new TargetParameterCountException();
					}
				}
				else
				{
					if (errorCode == -2147352558)
					{
						return new DivideByZeroException();
					}
					if (errorCode == -2147024894)
					{
						goto IL_2A6;
					}
					if (errorCode != -2147024893)
					{
						goto IL_3A9;
					}
					goto IL_27C;
				}
			}
			else if (errorCode <= -2147024858)
			{
				if (errorCode != -2147024885)
				{
					if (errorCode == -2147024882)
					{
						return new OutOfMemoryException();
					}
					if (errorCode != -2147024858)
					{
						goto IL_3A9;
					}
					return new EndOfStreamException();
				}
			}
			else
			{
				if (errorCode == -2147024809)
				{
					return new ArgumentException();
				}
				if (errorCode == -2147024690)
				{
					goto IL_32A;
				}
				if (errorCode != -2147024362)
				{
					goto IL_3A9;
				}
				return new ArithmeticException();
			}
			IL_26A:
			return new BadImageFormatException();
			IL_27C:
			return new DirectoryNotFoundException();
			IL_2A6:
			return new FileNotFoundException();
			IL_32A:
			return new PathTooLongException();
			IL_3A9:
			if (errorCode < 0)
			{
				return new COMException("", errorCode);
			}
			return null;
		}

		// Token: 0x06004216 RID: 16918
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetErrorInfo")]
		private static extern int _SetErrorInfo(int dwReserved, [MarshalAs(UnmanagedType.Interface)] IErrorInfo pIErrorInfo);

		// Token: 0x06004217 RID: 16919
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetErrorInfo")]
		private static extern int _GetErrorInfo(int dwReserved, [MarshalAs(UnmanagedType.Interface)] out IErrorInfo ppIErrorInfo);

		// Token: 0x06004218 RID: 16920 RVA: 0x000E35CC File Offset: 0x000E17CC
		internal static int SetErrorInfo(int dwReserved, IErrorInfo errorInfo)
		{
			int result = 0;
			errorInfo = null;
			if (Marshal.SetErrorInfoNotAvailable)
			{
				return -1;
			}
			try
			{
				result = Marshal._SetErrorInfo(dwReserved, errorInfo);
			}
			catch (Exception)
			{
				Marshal.SetErrorInfoNotAvailable = true;
			}
			return result;
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x000E360C File Offset: 0x000E180C
		internal static int GetErrorInfo(int dwReserved, out IErrorInfo errorInfo)
		{
			int result = 0;
			errorInfo = null;
			if (Marshal.GetErrorInfoNotAvailable)
			{
				return -1;
			}
			try
			{
				result = Marshal._GetErrorInfo(dwReserved, out errorInfo);
			}
			catch (Exception)
			{
				Marshal.GetErrorInfoNotAvailable = true;
			}
			return result;
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x000E364C File Offset: 0x000E184C
		public static Exception GetExceptionForHR(int errorCode)
		{
			return Marshal.GetExceptionForHR(errorCode, IntPtr.Zero);
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x000E365C File Offset: 0x000E185C
		public static Exception GetExceptionForHR(int errorCode, IntPtr errorInfo)
		{
			IErrorInfo errorInfo2 = null;
			if (errorInfo != (IntPtr)(-1))
			{
				if (errorInfo == IntPtr.Zero)
				{
					if (Marshal.GetErrorInfo(0, out errorInfo2) != 0)
					{
						errorInfo2 = null;
					}
				}
				else
				{
					errorInfo2 = (Marshal.GetObjectForIUnknown(errorInfo) as IErrorInfo);
				}
			}
			if (errorInfo2 is ManagedErrorInfo && ((ManagedErrorInfo)errorInfo2).Exception._HResult == errorCode)
			{
				return ((ManagedErrorInfo)errorInfo2).Exception;
			}
			Exception ex = Marshal.ConvertHrToException(errorCode);
			if (errorInfo2 != null && ex != null)
			{
				uint num;
				errorInfo2.GetHelpContext(out num);
				string text;
				errorInfo2.GetSource(out text);
				ex.Source = text;
				errorInfo2.GetDescription(out text);
				ex.SetMessage(text);
				errorInfo2.GetHelpFile(out text);
				if (num == 0U)
				{
					ex.HelpLink = text;
				}
				else
				{
					ex.HelpLink = string.Format("{0}#{1}", text, num);
				}
			}
			return ex;
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x000E372A File Offset: 0x000E192A
		public static int FinalReleaseComObject(object o)
		{
			while (Marshal.ReleaseComObject(o) != 0)
			{
			}
			return 0;
		}

		// Token: 0x0600421D RID: 16925
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Delegate GetDelegateForFunctionPointerInternal(IntPtr ptr, Type t);

		// Token: 0x0600421E RID: 16926 RVA: 0x000E3738 File Offset: 0x000E1938
		public static Delegate GetDelegateForFunctionPointer(IntPtr ptr, Type t)
		{
			if (t == null)
			{
				throw new ArgumentNullException("t");
			}
			if (!t.IsSubclassOf(typeof(MulticastDelegate)) || t == typeof(MulticastDelegate))
			{
				throw new ArgumentException("Type is not a delegate", "t");
			}
			if (t.IsGenericType)
			{
				throw new ArgumentException("The specified Type must not be a generic type definition.");
			}
			if (ptr == IntPtr.Zero)
			{
				throw new ArgumentNullException("ptr");
			}
			return Marshal.GetDelegateForFunctionPointerInternal(ptr, t);
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x000E37BF File Offset: 0x000E19BF
		public static TDelegate GetDelegateForFunctionPointer<TDelegate>(IntPtr ptr)
		{
			return (TDelegate)((object)Marshal.GetDelegateForFunctionPointer(ptr, typeof(TDelegate)));
		}

		// Token: 0x06004220 RID: 16928
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetFunctionPointerForDelegateInternal(Delegate d);

		// Token: 0x06004221 RID: 16929 RVA: 0x000E37D6 File Offset: 0x000E19D6
		public static IntPtr GetFunctionPointerForDelegate(Delegate d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return Marshal.GetFunctionPointerForDelegateInternal(d);
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x000E37EC File Offset: 0x000E19EC
		public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			return Marshal.GetFunctionPointerForDelegateInternal((Delegate)((object)d));
		}

		// Token: 0x06004223 RID: 16931
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLastWin32Error(int error);

		// Token: 0x06004224 RID: 16932
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetRawIUnknownForComObjectNoAddRef(object o);

		// Token: 0x06004225 RID: 16933
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetHRForException_WinRT(Exception e);

		// Token: 0x06004226 RID: 16934
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object GetNativeActivationFactory(Type type);

		// Token: 0x06004227 RID: 16935 RVA: 0x000E3814 File Offset: 0x000E1A14
		internal static ICustomMarshaler GetCustomMarshalerInstance(Type type, string cookie)
		{
			ValueTuple<Type, string> key = new ValueTuple<Type, string>(type, cookie);
			LazyInitializer.EnsureInitialized<Dictionary<ValueTuple<Type, string>, ICustomMarshaler>>(ref Marshal.MarshalerInstanceCache, () => new Dictionary<ValueTuple<Type, string>, ICustomMarshaler>(new Marshal.MarshalerInstanceKeyComparer()));
			object marshalerInstanceCacheLock = Marshal.MarshalerInstanceCacheLock;
			ICustomMarshaler customMarshaler;
			bool flag2;
			lock (marshalerInstanceCacheLock)
			{
				flag2 = Marshal.MarshalerInstanceCache.TryGetValue(key, out customMarshaler);
			}
			if (!flag2)
			{
				RuntimeMethodInfo runtimeMethodInfo;
				try
				{
					runtimeMethodInfo = (RuntimeMethodInfo)type.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, new Type[]
					{
						typeof(string)
					}, null);
				}
				catch (AmbiguousMatchException)
				{
					throw new ApplicationException("Custom marshaler '" + type.FullName + "' implements multiple static GetInstance methods that take a single string parameter.");
				}
				if (runtimeMethodInfo == null || runtimeMethodInfo.ReturnType != typeof(ICustomMarshaler))
				{
					throw new ApplicationException("Custom marshaler '" + type.FullName + "' does not implement a static GetInstance method that takes a single string parameter and returns an ICustomMarshaler.");
				}
				Exception ex;
				try
				{
					customMarshaler = (ICustomMarshaler)runtimeMethodInfo.InternalInvoke(null, new object[]
					{
						cookie
					}, out ex);
				}
				catch (Exception ex)
				{
					customMarshaler = null;
				}
				if (ex != null)
				{
					ExceptionDispatchInfo.Capture(ex).Throw();
				}
				if (customMarshaler == null)
				{
					throw new ApplicationException("A call to GetInstance() for custom marshaler '" + type.FullName + "' returned null, which is not allowed.");
				}
				marshalerInstanceCacheLock = Marshal.MarshalerInstanceCacheLock;
				lock (marshalerInstanceCacheLock)
				{
					Marshal.MarshalerInstanceCache[key] = customMarshaler;
				}
			}
			return customMarshaler;
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x000E39C0 File Offset: 0x000E1BC0
		public unsafe static IntPtr StringToCoTaskMemUTF8(string s)
		{
			if (s == null)
			{
				return IntPtr.Zero;
			}
			int maxByteCount = Encoding.UTF8.GetMaxByteCount(s.Length);
			IntPtr intPtr = Marshal.AllocCoTaskMem(maxByteCount + 1);
			byte* ptr = (byte*)((void*)intPtr);
			int bytes;
			fixed (string text = s)
			{
				char* ptr2 = text;
				if (ptr2 != null)
				{
					ptr2 += RuntimeHelpers.OffsetToStringData / 2;
				}
				bytes = Encoding.UTF8.GetBytes(ptr2, s.Length, ptr, maxByteCount);
			}
			ptr[bytes] = 0;
			return intPtr;
		}

		// Token: 0x04002BCC RID: 11212
		public static readonly int SystemMaxDBCSCharSize = 2;

		// Token: 0x04002BCD RID: 11213
		public static readonly int SystemDefaultCharSize = Environment.IsRunningOnWindows ? 2 : 1;

		// Token: 0x04002BCE RID: 11214
		private static bool SetErrorInfoNotAvailable;

		// Token: 0x04002BCF RID: 11215
		private static bool GetErrorInfoNotAvailable;

		// Token: 0x04002BD0 RID: 11216
		internal static Dictionary<ValueTuple<Type, string>, ICustomMarshaler> MarshalerInstanceCache;

		// Token: 0x04002BD1 RID: 11217
		internal static readonly object MarshalerInstanceCacheLock = new object();

		// Token: 0x02000749 RID: 1865
		// (Invoke) Token: 0x0600422B RID: 16939
		internal delegate IntPtr SecureStringAllocator(int len);

		// Token: 0x0200074A RID: 1866
		internal class MarshalerInstanceKeyComparer : IEqualityComparer<ValueTuple<Type, string>>
		{
			// Token: 0x0600422E RID: 16942 RVA: 0x000E3A46 File Offset: 0x000E1C46
			public bool Equals(ValueTuple<Type, string> lhs, ValueTuple<Type, string> rhs)
			{
				return lhs.CompareTo(rhs) == 0;
			}

			// Token: 0x0600422F RID: 16943 RVA: 0x000E3A53 File Offset: 0x000E1C53
			public int GetHashCode(ValueTuple<Type, string> key)
			{
				return key.GetHashCode();
			}
		}
	}
}
