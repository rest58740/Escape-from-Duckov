using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Animancer
{
	// Token: 0x02000016 RID: 22
	public static class AnimancerUtilities
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x000077E5 File Offset: 0x000059E5
		public static float Wrap01(float value)
		{
			double num = (double)value;
			value = (float)(num - Math.Floor(num));
			if (value >= 1f)
			{
				return 0f;
			}
			return value;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00007804 File Offset: 0x00005A04
		public static float Wrap(float value, float length)
		{
			double num = (double)value;
			double num2 = (double)length;
			value = (float)(num - Math.Floor(num / num2) * num2);
			if (value >= length)
			{
				return 0f;
			}
			return value;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000782F File Offset: 0x00005A2F
		public static float Round(float value)
		{
			return (float)Math.Round((double)value, MidpointRounding.AwayFromZero);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000783A File Offset: 0x00005A3A
		public static float Round(float value, float multiple)
		{
			return AnimancerUtilities.Round(value / multiple) * multiple;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00007846 File Offset: 0x00005A46
		public static bool IsFinite(this float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000785B File Offset: 0x00005A5B
		public static bool IsFinite(this double value)
		{
			return !double.IsNaN(value) && !double.IsInfinity(value);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00007870 File Offset: 0x00005A70
		public static bool IsFinite(this Vector2 value)
		{
			return value.x.IsFinite() && value.y.IsFinite();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000788C File Offset: 0x00005A8C
		public static string ToStringOrNull(object obj)
		{
			if (obj == null)
			{
				return "Null";
			}
			UnityEngine.Object @object = obj as UnityEngine.Object;
			if (@object != null && @object == null)
			{
				return string.Format("Null ({0})", obj.GetType());
			}
			return obj.ToString();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000078CC File Offset: 0x00005ACC
		public static void CopyExactArray<T>(T[] copyFrom, ref T[] copyTo)
		{
			if (copyFrom == null)
			{
				copyTo = null;
				return;
			}
			int length = copyFrom.Length;
			AnimancerUtilities.SetLength<T>(ref copyTo, length);
			Array.Copy(copyFrom, copyTo, length);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000078F8 File Offset: 0x00005AF8
		public static void Swap<T>(this T[] array, int a, int b)
		{
			T t = array[a];
			array[a] = array[b];
			array[b] = t;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00007923 File Offset: 0x00005B23
		public static bool IsNullOrEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000792F File Offset: 0x00005B2F
		public static bool SetLength<T>(ref T[] array, int length)
		{
			if (array == null || array.Length != length)
			{
				array = new T[length];
				return true;
			}
			return false;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00007947 File Offset: 0x00005B47
		public static bool IsValid(this AnimancerNode node)
		{
			return node != null && node.IsValid;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00007954 File Offset: 0x00005B54
		public static bool IsValid(this ITransitionDetailed transition)
		{
			return transition != null && transition.IsValid;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00007964 File Offset: 0x00005B64
		public static AnimancerState CreateStateAndApply(this ITransition transition, AnimancerPlayable root = null)
		{
			AnimancerState animancerState = transition.CreateState();
			animancerState.SetRoot(root);
			transition.Apply(animancerState);
			return animancerState;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00007988 File Offset: 0x00005B88
		public static void RemovePlayable(Playable playable, bool destroy = true)
		{
			if (!playable.IsValid<Playable>())
			{
				return;
			}
			Playable input = playable.GetInput(0);
			if (!input.IsValid<Playable>())
			{
				if (destroy)
				{
					playable.Destroy<Playable>();
				}
				return;
			}
			PlayableGraph graph = playable.GetGraph<Playable>();
			Playable output = playable.GetOutput(0);
			if (output.IsValid<Playable>())
			{
				if (destroy)
				{
					playable.Destroy<Playable>();
				}
				else
				{
					graph.Disconnect<Playable>(output, 0);
					graph.Disconnect<Playable>(playable, 0);
				}
				graph.Connect<Playable, Playable>(input, 0, output, 0);
				return;
			}
			if (destroy)
			{
				playable.Destroy<Playable>();
			}
			else
			{
				graph.Disconnect<Playable>(playable, 0);
			}
			graph.GetOutput(0).SetSourcePlayable(input);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00007A1C File Offset: 0x00005C1C
		public static bool HasEvent(IAnimationClipCollection source, string functionName)
		{
			HashSet<AnimationClip> hashSet = ObjectPool.AcquireSet<AnimationClip>();
			source.GatherAnimationClips(hashSet);
			using (HashSet<AnimationClip>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (AnimancerUtilities.HasEvent(enumerator.Current, functionName))
					{
						ObjectPool.Release<AnimationClip>(hashSet);
						return true;
					}
				}
			}
			ObjectPool.Release<AnimationClip>(hashSet);
			return false;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00007A8C File Offset: 0x00005C8C
		public static bool HasEvent(AnimationClip clip, string functionName)
		{
			AnimationEvent[] events = clip.events;
			for (int i = events.Length - 1; i >= 0; i--)
			{
				if (events[i].functionName == functionName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00007AC4 File Offset: 0x00005CC4
		public static void CalculateThresholdsFromAverageVelocityXZ(this MixerState<Vector2> mixer)
		{
			mixer.ValidateThresholdCount();
			for (int i = mixer.ChildCount - 1; i >= 0; i--)
			{
				AnimancerState child = mixer.GetChild(i);
				if (child != null)
				{
					Vector3 averageVelocity = child.AverageVelocity;
					mixer.SetThreshold(i, new Vector2(averageVelocity.x, averageVelocity.z));
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00007B18 File Offset: 0x00005D18
		public static void CopyParameterValue(Animator copyFrom, Animator copyTo, AnimatorControllerParameter parameter)
		{
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
				copyTo.SetFloat(parameter.nameHash, copyFrom.GetFloat(parameter.nameHash));
				return;
			case (AnimatorControllerParameterType)2:
				goto IL_71;
			case AnimatorControllerParameterType.Int:
				copyTo.SetInteger(parameter.nameHash, copyFrom.GetInteger(parameter.nameHash));
				return;
			case AnimatorControllerParameterType.Bool:
				break;
			default:
				if (type != AnimatorControllerParameterType.Trigger)
				{
					goto IL_71;
				}
				break;
			}
			copyTo.SetBool(parameter.nameHash, copyFrom.GetBool(parameter.nameHash));
			return;
			IL_71:
			throw AnimancerUtilities.CreateUnsupportedArgumentException<AnimatorControllerParameterType>(parameter.type);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00007BA4 File Offset: 0x00005DA4
		public static void CopyParameterValue(AnimatorControllerPlayable copyFrom, AnimatorControllerPlayable copyTo, AnimatorControllerParameter parameter)
		{
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
				copyTo.SetFloat(parameter.nameHash, copyFrom.GetFloat(parameter.nameHash));
				return;
			case (AnimatorControllerParameterType)2:
				goto IL_77;
			case AnimatorControllerParameterType.Int:
				copyTo.SetInteger(parameter.nameHash, copyFrom.GetInteger(parameter.nameHash));
				return;
			case AnimatorControllerParameterType.Bool:
				break;
			default:
				if (type != AnimatorControllerParameterType.Trigger)
				{
					goto IL_77;
				}
				break;
			}
			copyTo.SetBool(parameter.nameHash, copyFrom.GetBool(parameter.nameHash));
			return;
			IL_77:
			throw AnimancerUtilities.CreateUnsupportedArgumentException<AnimatorControllerParameterType>(parameter.type);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00007C34 File Offset: 0x00005E34
		public static object GetParameterValue(Animator animator, AnimatorControllerParameter parameter)
		{
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
				return animator.GetFloat(parameter.nameHash);
			case (AnimatorControllerParameterType)2:
				goto IL_5C;
			case AnimatorControllerParameterType.Int:
				return animator.GetInteger(parameter.nameHash);
			case AnimatorControllerParameterType.Bool:
				break;
			default:
				if (type != AnimatorControllerParameterType.Trigger)
				{
					goto IL_5C;
				}
				break;
			}
			return animator.GetBool(parameter.nameHash);
			IL_5C:
			throw AnimancerUtilities.CreateUnsupportedArgumentException<AnimatorControllerParameterType>(parameter.type);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00007CA8 File Offset: 0x00005EA8
		public static object GetParameterValue(AnimatorControllerPlayable playable, AnimatorControllerParameter parameter)
		{
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
				return playable.GetFloat(parameter.nameHash);
			case (AnimatorControllerParameterType)2:
				goto IL_5F;
			case AnimatorControllerParameterType.Int:
				return playable.GetInteger(parameter.nameHash);
			case AnimatorControllerParameterType.Bool:
				break;
			default:
				if (type != AnimatorControllerParameterType.Trigger)
				{
					goto IL_5F;
				}
				break;
			}
			return playable.GetBool(parameter.nameHash);
			IL_5F:
			throw AnimancerUtilities.CreateUnsupportedArgumentException<AnimatorControllerParameterType>(parameter.type);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00007D20 File Offset: 0x00005F20
		public static void SetParameterValue(Animator animator, AnimatorControllerParameter parameter, object value)
		{
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
				animator.SetFloat(parameter.nameHash, (float)value);
				return;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				animator.SetInteger(parameter.nameHash, (int)value);
				return;
			case AnimatorControllerParameterType.Bool:
				animator.SetBool(parameter.nameHash, (bool)value);
				return;
			default:
				if (type == AnimatorControllerParameterType.Trigger)
				{
					if ((bool)value)
					{
						animator.SetTrigger(parameter.nameHash);
						return;
					}
					animator.ResetTrigger(parameter.nameHash);
					return;
				}
				break;
			}
			throw AnimancerUtilities.CreateUnsupportedArgumentException<AnimatorControllerParameterType>(parameter.type);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00007DBC File Offset: 0x00005FBC
		public static void SetParameterValue(AnimatorControllerPlayable playable, AnimatorControllerParameter parameter, object value)
		{
			AnimatorControllerParameterType type = parameter.type;
			switch (type)
			{
			case AnimatorControllerParameterType.Float:
				playable.SetFloat(parameter.nameHash, (float)value);
				return;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				playable.SetInteger(parameter.nameHash, (int)value);
				return;
			case AnimatorControllerParameterType.Bool:
				playable.SetBool(parameter.nameHash, (bool)value);
				return;
			default:
				if (type == AnimatorControllerParameterType.Trigger)
				{
					if ((bool)value)
					{
						playable.SetTrigger(parameter.nameHash);
						return;
					}
					playable.ResetTrigger(parameter.nameHash);
					return;
				}
				break;
			}
			throw AnimancerUtilities.CreateUnsupportedArgumentException<AnimatorControllerParameterType>(parameter.type);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00007E5A File Offset: 0x0000605A
		public static NativeArray<T> CreateNativeReference<T>() where T : struct
		{
			return new NativeArray<T>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00007E64 File Offset: 0x00006064
		public static NativeArray<TransformStreamHandle> ConvertToTransformStreamHandles(IList<Transform> transforms, Animator animator)
		{
			int count = transforms.Count;
			NativeArray<TransformStreamHandle> result = new NativeArray<TransformStreamHandle>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < count; i++)
			{
				result[i] = animator.BindStreamTransform(transforms[i]);
			}
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00007EA4 File Offset: 0x000060A4
		public static string GetUnsupportedMessage<T>(T value)
		{
			return string.Format("Unsupported {0}: {1}", typeof(T).FullName, value);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00007EC5 File Offset: 0x000060C5
		public static ArgumentException CreateUnsupportedArgumentException<T>(T value)
		{
			return new ArgumentException(AnimancerUtilities.GetUnsupportedMessage<T>(value));
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00007ED2 File Offset: 0x000060D2
		public static T AddAnimancerComponent<T>(this Animator animator) where T : Component, IAnimancerComponent
		{
			T t = animator.gameObject.AddComponent<T>();
			t.Animator = animator;
			return t;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00007EEC File Offset: 0x000060EC
		public static T GetOrAddAnimancerComponent<T>(this Animator animator) where T : Component, IAnimancerComponent
		{
			T result;
			if (animator.TryGetComponent<T>(out result))
			{
				return result;
			}
			return animator.AddAnimancerComponent<T>();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00007F0C File Offset: 0x0000610C
		public static T GetComponentInParentOrChildren<T>(this GameObject gameObject) where T : class
		{
			T componentInParent = gameObject.GetComponentInParent<T>();
			if (componentInParent != null)
			{
				return componentInParent;
			}
			return gameObject.GetComponentInChildren<T>();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00007F30 File Offset: 0x00006130
		public static bool GetComponentInParentOrChildren<T>(this GameObject gameObject, ref T component) where T : class
		{
			if (component != null)
			{
				UnityEngine.Object @object = component as UnityEngine.Object;
				if (@object == null || @object != null)
				{
					return false;
				}
			}
			component = gameObject.GetComponentInParentOrChildren<T>();
			return component != null;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00007F86 File Offset: 0x00006186
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message)
		{
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00007F88 File Offset: 0x00006188
		[Conditional("UNITY_EDITOR")]
		public static void SetDirty(UnityEngine.Object target)
		{
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00007F8A File Offset: 0x0000618A
		[Conditional("UNITY_EDITOR")]
		public static void EditModeSampleAnimation(this AnimationClip clip, Component component, float time = 0f)
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00007F8C File Offset: 0x0000618C
		[Conditional("UNITY_EDITOR")]
		public static void EditModePlay(this AnimationClip clip, Component component)
		{
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00007F8E File Offset: 0x0000618E
		public static void Gather(this ICollection<AnimationClip> clips, AnimationClip clip)
		{
			if (clip != null && !clips.Contains(clip))
			{
				clips.Add(clip);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00007FAC File Offset: 0x000061AC
		public static void Gather(this ICollection<AnimationClip> clips, IList<AnimationClip> gatherFrom)
		{
			if (gatherFrom == null)
			{
				return;
			}
			for (int i = gatherFrom.Count - 1; i >= 0; i--)
			{
				clips.Gather(gatherFrom[i]);
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00007FE0 File Offset: 0x000061E0
		public static void Gather(this ICollection<AnimationClip> clips, IEnumerable<AnimationClip> gatherFrom)
		{
			if (gatherFrom == null)
			{
				return;
			}
			foreach (AnimationClip clip in gatherFrom)
			{
				clips.Gather(clip);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000802C File Offset: 0x0000622C
		public static void GatherFromAsset(this ICollection<AnimationClip> clips, PlayableAsset asset)
		{
			if (asset == null)
			{
				return;
			}
			MethodInfo method = asset.GetType().GetMethod("GetRootTracks");
			if (method != null && typeof(IEnumerable).IsAssignableFrom(method.ReturnType) && method.GetParameters().Length == 0)
			{
				object obj = method.Invoke(asset, null);
				AnimancerUtilities.GatherFromTracks(clips, obj as IEnumerable);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00008094 File Offset: 0x00006294
		private static void GatherFromTracks(ICollection<AnimationClip> clips, IEnumerable tracks)
		{
			if (tracks == null)
			{
				return;
			}
			foreach (object obj in tracks)
			{
				if (obj != null)
				{
					Type type = obj.GetType();
					MethodInfo method = type.GetMethod("GetClips");
					if (method != null && typeof(IEnumerable).IsAssignableFrom(method.ReturnType) && method.GetParameters().Length == 0)
					{
						IEnumerable enumerable = method.Invoke(obj, null) as IEnumerable;
						if (enumerable != null)
						{
							foreach (object obj2 in enumerable)
							{
								PropertyInfo property = obj2.GetType().GetProperty("animationClip");
								if (property != null && property.PropertyType == typeof(AnimationClip))
								{
									MethodInfo getMethod = property.GetGetMethod();
									clips.Gather(getMethod.Invoke(obj2, null) as AnimationClip);
								}
							}
						}
					}
					MethodInfo method2 = type.GetMethod("GetChildTracks");
					if (method2 != null && typeof(IEnumerable).IsAssignableFrom(method2.ReturnType) && method2.GetParameters().Length == 0)
					{
						object obj3 = method2.Invoke(obj, null);
						AnimancerUtilities.GatherFromTracks(clips, obj3 as IEnumerable);
					}
				}
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00008248 File Offset: 0x00006448
		public static void GatherFromSource(this ICollection<AnimationClip> clips, IAnimationClipSource source)
		{
			if (source == null)
			{
				return;
			}
			List<AnimationClip> list = ObjectPool.AcquireList<AnimationClip>();
			source.GetAnimationClips(list);
			clips.Gather(list);
			ObjectPool.Release<AnimationClip>(list);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00008274 File Offset: 0x00006474
		public static void GatherFromSource(this ICollection<AnimationClip> clips, IEnumerable source)
		{
			if (source != null)
			{
				foreach (object source2 in source)
				{
					clips.GatherFromSource(source2);
				}
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000082C8 File Offset: 0x000064C8
		public static bool GatherFromSource(this ICollection<AnimationClip> clips, object source)
		{
			AnimationClip clip;
			if (AnimancerUtilities.TryGetWrappedObject<AnimationClip>(source, out clip))
			{
				clips.Gather(clip);
				return true;
			}
			IAnimationClipCollection animationClipCollection;
			if (AnimancerUtilities.TryGetWrappedObject<IAnimationClipCollection>(source, out animationClipCollection))
			{
				animationClipCollection.GatherAnimationClips(clips);
				return true;
			}
			IAnimationClipSource source2;
			if (AnimancerUtilities.TryGetWrappedObject<IAnimationClipSource>(source, out source2))
			{
				clips.GatherFromSource(source2);
				return true;
			}
			IEnumerable source3;
			if (AnimancerUtilities.TryGetWrappedObject<IEnumerable>(source, out source3))
			{
				clips.GatherFromSource(source3);
				return true;
			}
			return false;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00008324 File Offset: 0x00006524
		public static bool TryGetFrameRate(object clipSource, out float frameRate)
		{
			HashSet<AnimationClip> hashSet;
			bool result;
			using (ObjectPool.Disposable.AcquireSet<AnimationClip>(out hashSet))
			{
				hashSet.GatherFromSource(clipSource);
				if (hashSet.Count == 0)
				{
					frameRate = float.NaN;
					result = false;
				}
				else
				{
					frameRate = float.NaN;
					foreach (AnimationClip animationClip in hashSet)
					{
						if (float.IsNaN(frameRate))
						{
							frameRate = animationClip.frameRate;
						}
						else if (frameRate != animationClip.frameRate)
						{
							frameRate = float.NaN;
							return false;
						}
					}
					result = (frameRate > 0f);
				}
			}
			return result;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000083E8 File Offset: 0x000065E8
		public static T Clone<T>(this T original) where T : class, ICopyable<T>, new()
		{
			if (original == null)
			{
				return default(T);
			}
			T t = Activator.CreateInstance<T>();
			t.CopyFrom(original);
			return t;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00008418 File Offset: 0x00006618
		public static bool TryGetAverageAngularSpeed(object motion, out float averageAngularSpeed)
		{
			Motion motion2 = motion as Motion;
			if (motion2 != null)
			{
				averageAngularSpeed = motion2.averageAngularSpeed;
				return true;
			}
			IMotion motion3;
			if (AnimancerUtilities.TryGetWrappedObject<IMotion>(motion, out motion3))
			{
				averageAngularSpeed = motion3.AverageAngularSpeed;
				return true;
			}
			averageAngularSpeed = 0f;
			return false;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00008458 File Offset: 0x00006658
		public static bool TryGetAverageVelocity(object motion, out Vector3 averageVelocity)
		{
			Motion motion2 = motion as Motion;
			if (motion2 != null)
			{
				averageVelocity = motion2.averageSpeed;
				return true;
			}
			IMotion motion3;
			if (AnimancerUtilities.TryGetWrappedObject<IMotion>(motion, out motion3))
			{
				averageVelocity = motion3.AverageVelocity;
				return true;
			}
			averageVelocity = default(Vector3);
			return false;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000084A0 File Offset: 0x000066A0
		public static bool IsValid(this ITransition transition)
		{
			ITransitionDetailed transitionDetailed;
			return transition != null && (!AnimancerUtilities.TryGetWrappedObject<ITransitionDetailed>(transition, out transitionDetailed) || transitionDetailed.IsValid);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000084C4 File Offset: 0x000066C4
		public static bool TryGetIsLooping(object motionOrTransition, out bool isLooping)
		{
			Motion motion = motionOrTransition as Motion;
			if (motion != null)
			{
				isLooping = motion.isLooping;
				return true;
			}
			ITransitionDetailed transitionDetailed;
			if (AnimancerUtilities.TryGetWrappedObject<ITransitionDetailed>(motionOrTransition, out transitionDetailed))
			{
				isLooping = transitionDetailed.IsLooping;
				return true;
			}
			isLooping = false;
			return false;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00008500 File Offset: 0x00006700
		public static bool TryGetLength(object motionOrTransition, out float length)
		{
			AnimationClip animationClip = motionOrTransition as AnimationClip;
			if (animationClip != null)
			{
				length = animationClip.length;
				return true;
			}
			ITransitionDetailed transitionDetailed;
			if (AnimancerUtilities.TryGetWrappedObject<ITransitionDetailed>(motionOrTransition, out transitionDetailed))
			{
				length = transitionDetailed.MaximumDuration;
				return true;
			}
			length = 0f;
			return false;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00008540 File Offset: 0x00006740
		public static object GetWrappedObject(object wrapper)
		{
			for (;;)
			{
				IWrapper wrapper2 = wrapper as IWrapper;
				if (wrapper2 == null)
				{
					break;
				}
				wrapper = wrapper2.WrappedObject;
			}
			return wrapper;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00008564 File Offset: 0x00006764
		public static bool TryGetWrappedObject<T>(object wrapper, out T wrapped) where T : class
		{
			for (;;)
			{
				wrapped = (wrapper as T);
				if (wrapped != null)
				{
					break;
				}
				IWrapper wrapper2 = wrapper as IWrapper;
				if (wrapper2 == null)
				{
					return false;
				}
				wrapper = wrapper2.WrappedObject;
			}
			return true;
		}

		// Token: 0x04000056 RID: 86
		public const bool IsAnimancerPro = true;
	}
}
