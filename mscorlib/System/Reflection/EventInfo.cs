using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Mono;
using Unity;

namespace System.Reflection
{
	// Token: 0x02000899 RID: 2201
	[Serializable]
	public abstract class EventInfo : MemberInfo, _EventInfo
	{
		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600489B RID: 18587 RVA: 0x00015831 File Offset: 0x00013A31
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Event;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600489C RID: 18588
		public abstract EventAttributes Attributes { get; }

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600489D RID: 18589 RVA: 0x000EE25E File Offset: 0x000EC45E
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & EventAttributes.SpecialName) > EventAttributes.None;
			}
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x000EE26F File Offset: 0x000EC46F
		public MethodInfo[] GetOtherMethods()
		{
			return this.GetOtherMethods(false);
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual MethodInfo[] GetOtherMethods(bool nonPublic)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x060048A0 RID: 18592 RVA: 0x000EE278 File Offset: 0x000EC478
		public virtual MethodInfo AddMethod
		{
			get
			{
				return this.GetAddMethod(true);
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x060048A1 RID: 18593 RVA: 0x000EE281 File Offset: 0x000EC481
		public virtual MethodInfo RemoveMethod
		{
			get
			{
				return this.GetRemoveMethod(true);
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060048A2 RID: 18594 RVA: 0x000EE28A File Offset: 0x000EC48A
		public virtual MethodInfo RaiseMethod
		{
			get
			{
				return this.GetRaiseMethod(true);
			}
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x000EE293 File Offset: 0x000EC493
		public MethodInfo GetAddMethod()
		{
			return this.GetAddMethod(false);
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x000EE29C File Offset: 0x000EC49C
		public MethodInfo GetRemoveMethod()
		{
			return this.GetRemoveMethod(false);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x000EE2A5 File Offset: 0x000EC4A5
		public MethodInfo GetRaiseMethod()
		{
			return this.GetRaiseMethod(false);
		}

		// Token: 0x060048A6 RID: 18598
		public abstract MethodInfo GetAddMethod(bool nonPublic);

		// Token: 0x060048A7 RID: 18599
		public abstract MethodInfo GetRemoveMethod(bool nonPublic);

		// Token: 0x060048A8 RID: 18600
		public abstract MethodInfo GetRaiseMethod(bool nonPublic);

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060048A9 RID: 18601 RVA: 0x000EE2B0 File Offset: 0x000EC4B0
		public virtual bool IsMulticast
		{
			get
			{
				Type eventHandlerType = this.EventHandlerType;
				return typeof(MulticastDelegate).IsAssignableFrom(eventHandlerType);
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060048AA RID: 18602 RVA: 0x000EE2D4 File Offset: 0x000EC4D4
		public virtual Type EventHandlerType
		{
			get
			{
				ParameterInfo[] parametersInternal = this.GetAddMethod(true).GetParametersInternal();
				Type typeFromHandle = typeof(Delegate);
				for (int i = 0; i < parametersInternal.Length; i++)
				{
					Type parameterType = parametersInternal[i].ParameterType;
					if (parameterType.IsSubclassOf(typeFromHandle))
					{
						return parameterType;
					}
				}
				return null;
			}
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x000EE31C File Offset: 0x000EC51C
		[DebuggerStepThrough]
		[DebuggerHidden]
		public virtual void RemoveEventHandler(object target, Delegate handler)
		{
			MethodInfo removeMethod = this.GetRemoveMethod(false);
			if (removeMethod == null)
			{
				throw new InvalidOperationException("Cannot remove the event handler since no public remove method exists for the event.");
			}
			if (removeMethod.GetParametersNoCopy()[0].ParameterType == typeof(EventRegistrationToken))
			{
				throw new InvalidOperationException("Adding or removing event handlers dynamically is not supported on WinRT events.");
			}
			removeMethod.Invoke(target, new object[]
			{
				handler
			});
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x000EE37E File Offset: 0x000EC57E
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x000EE387 File Offset: 0x000EC587
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(EventInfo left, EventInfo right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x000EE38F File Offset: 0x000EC58F
		public static bool operator !=(EventInfo left, EventInfo right)
		{
			return !(left == right);
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x000EE39C File Offset: 0x000EC59C
		[DebuggerStepThrough]
		[DebuggerHidden]
		public virtual void AddEventHandler(object target, Delegate handler)
		{
			if (this.cached_add_event == null)
			{
				MethodInfo addMethod = this.GetAddMethod();
				if (addMethod == null)
				{
					throw new InvalidOperationException("Cannot add the event handler since no public add method exists for the event.");
				}
				if (addMethod.DeclaringType.IsValueType)
				{
					if (target == null && !addMethod.IsStatic)
					{
						throw new TargetException("Cannot add a handler to a non static event with a null target");
					}
					addMethod.Invoke(target, new object[]
					{
						handler
					});
					return;
				}
				else
				{
					this.cached_add_event = EventInfo.CreateAddEventDelegate(addMethod);
				}
			}
			this.cached_add_event(target, handler);
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x000EE41C File Offset: 0x000EC61C
		private static void AddEventFrame<T, D>(EventInfo.AddEvent<T, D> addEvent, object obj, object dele)
		{
			if (obj == null)
			{
				throw new TargetException("Cannot add a handler to a non static event with a null target");
			}
			if (!(obj is T))
			{
				throw new TargetException("Object doesn't match target");
			}
			if (!(dele is D))
			{
				throw new ArgumentException(string.Format("Object of type {0} cannot be converted to type {1}.", dele.GetType(), typeof(D)));
			}
			addEvent((T)((object)obj), (D)((object)dele));
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x000EE484 File Offset: 0x000EC684
		private static void StaticAddEventAdapterFrame<D>(EventInfo.StaticAddEvent<D> addEvent, object obj, object dele)
		{
			addEvent((D)((object)dele));
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x000EE494 File Offset: 0x000EC694
		private static EventInfo.AddEventAdapter CreateAddEventDelegate(MethodInfo method)
		{
			Type[] typeArguments;
			Type typeFromHandle;
			string name;
			if (method.IsStatic)
			{
				typeArguments = new Type[]
				{
					method.GetParametersInternal()[0].ParameterType
				};
				typeFromHandle = typeof(EventInfo.StaticAddEvent<>);
				name = "StaticAddEventAdapterFrame";
			}
			else
			{
				typeArguments = new Type[]
				{
					method.DeclaringType,
					method.GetParametersInternal()[0].ParameterType
				};
				typeFromHandle = typeof(EventInfo.AddEvent<, >);
				name = "AddEventFrame";
			}
			object firstArgument = Delegate.CreateDelegate(typeFromHandle.MakeGenericType(typeArguments), method);
			MethodInfo methodInfo = typeof(EventInfo).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
			methodInfo = methodInfo.MakeGenericMethod(typeArguments);
			return (EventInfo.AddEventAdapter)Delegate.CreateDelegate(typeof(EventInfo.AddEventAdapter), firstArgument, methodInfo, true);
		}

		// Token: 0x060048B4 RID: 18612
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EventInfo internal_from_handle_type(IntPtr event_handle, IntPtr type_handle);

		// Token: 0x060048B5 RID: 18613 RVA: 0x000EE548 File Offset: 0x000EC748
		internal static EventInfo GetEventFromHandle(RuntimeEventHandle handle, RuntimeTypeHandle reflectedType)
		{
			if (handle.Value == IntPtr.Zero)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			EventInfo eventInfo = EventInfo.internal_from_handle_type(handle.Value, reflectedType.Value);
			if (eventInfo == null)
			{
				throw new ArgumentException("The event handle and the type handle are incompatible.");
			}
			return eventInfo;
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x000173AD File Offset: 0x000155AD
		void _EventInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00052959 File Offset: 0x00050B59
		Type _EventInfo.GetType()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x000173AD File Offset: 0x000155AD
		void _EventInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x000173AD File Offset: 0x000155AD
		void _EventInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x000173AD File Offset: 0x000155AD
		void _EventInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002E8B RID: 11915
		private EventInfo.AddEventAdapter cached_add_event;

		// Token: 0x0200089A RID: 2202
		// (Invoke) Token: 0x060048BC RID: 18620
		private delegate void AddEventAdapter(object _this, Delegate dele);

		// Token: 0x0200089B RID: 2203
		// (Invoke) Token: 0x060048C0 RID: 18624
		private delegate void AddEvent<T, D>(T _this, D dele);

		// Token: 0x0200089C RID: 2204
		// (Invoke) Token: 0x060048C4 RID: 18628
		private delegate void StaticAddEvent<D>(D dele);
	}
}
