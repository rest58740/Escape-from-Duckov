using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Profiling;

namespace Pathfinding.Util
{
	// Token: 0x0200028E RID: 654
	[HelpURL("https://arongranberg.com/astar/documentation/stable/batchedevents.html")]
	public class BatchedEvents : VersionedMonoBehaviour
	{
		// Token: 0x06000FA7 RID: 4007 RVA: 0x0005FF95 File Offset: 0x0005E195
		private void OnEnable()
		{
			if (BatchedEvents.instance == null)
			{
				BatchedEvents.instance = this;
			}
			BatchedEvents.instance != this;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0005FFB6 File Offset: 0x0005E1B6
		private void OnDisable()
		{
			if (BatchedEvents.instance == this)
			{
				BatchedEvents.instance = null;
			}
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0005FFCB File Offset: 0x0005E1CB
		private static void CreateInstance()
		{
			GameObject gameObject = new GameObject("Batch Helper");
			gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
			BatchedEvents.instance = gameObject.AddComponent<BatchedEvents>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0005FFF0 File Offset: 0x0005E1F0
		public static T Find<T, K>(K key, Func<T, K, bool> predicate) where T : class, IEntityIndex
		{
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < BatchedEvents.data.Length; i++)
			{
				if (BatchedEvents.data[i].type == typeFromHandle)
				{
					T[] array = BatchedEvents.data[i].objects as T[];
					for (int j = 0; j < BatchedEvents.data[i].objectCount; j++)
					{
						if (predicate(array[j], key))
						{
							return array[j];
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00060088 File Offset: 0x0005E288
		public static void Remove<T>(T obj) where T : IEntityIndex
		{
			int num = obj.EntityIndex;
			if (num == 0)
			{
				return;
			}
			int num2 = ((num & 1069547520) >> 22) - 1;
			num &= -1069547521;
			if (BatchedEvents.isIterating && BatchedEvents.isIteratingOverTypeIndex == num2)
			{
				throw new Exception("Cannot add or remove entities during an event (Update/LateUpdate/...) that this helper initiated");
			}
			BatchedEvents.data[num2].Remove(num);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000600E8 File Offset: 0x0005E2E8
		public static int GetComponents<T>(BatchedEvents.Event eventTypes, out TransformAccessArray transforms, out T[] components) where T : Component, IEntityIndex
		{
			if (BatchedEvents.instance == null)
			{
				BatchedEvents.CreateInstance();
			}
			int num = (int)(eventTypes * (BatchedEvents.Event)12582917);
			if (BatchedEvents.isIterating && BatchedEvents.isIteratingOverTypeIndex == num)
			{
				throw new Exception("Cannot add or remove entities during an event (Update/LateUpdate/...) that this helper initiated");
			}
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < BatchedEvents.data.Length; i++)
			{
				if (BatchedEvents.data[i].type == typeFromHandle && BatchedEvents.data[i].variant == num)
				{
					transforms = BatchedEvents.data[i].transforms;
					components = (BatchedEvents.data[i].objects as T[]);
					return BatchedEvents.data[i].objectCount;
				}
			}
			transforms = default(TransformAccessArray);
			components = null;
			return 0;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000601BA File Offset: 0x0005E3BA
		public static bool Has<T>(T obj) where T : IEntityIndex
		{
			return obj.EntityIndex != 0;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x000601CC File Offset: 0x0005E3CC
		public static void Add<T>(T obj, BatchedEvents.Event eventTypes, Action<T[], int> action, int archetypeVariant = 0) where T : Component, IEntityIndex
		{
			BatchedEvents.Add<T>(obj, eventTypes, null, action, archetypeVariant);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x000601D8 File Offset: 0x0005E3D8
		public static void Add<T>(T obj, BatchedEvents.Event eventTypes, Action<T[], int, TransformAccessArray, BatchedEvents.Event> action, int archetypeVariant = 0) where T : Component, IEntityIndex
		{
			BatchedEvents.Add<T>(obj, eventTypes, action, null, archetypeVariant);
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x000601E4 File Offset: 0x0005E3E4
		private static void Add<T>(T obj, BatchedEvents.Event eventTypes, Action<T[], int, TransformAccessArray, BatchedEvents.Event> action1, Action<T[], int> action2, int archetypeVariant = 0) where T : Component, IEntityIndex
		{
			if (obj.EntityIndex != 0)
			{
				throw new ArgumentException("This object is already registered. Call Remove before adding the object again.");
			}
			if (BatchedEvents.instance == null)
			{
				BatchedEvents.CreateInstance();
			}
			archetypeVariant = (int)(eventTypes * (BatchedEvents.Event)12582917);
			if (BatchedEvents.isIterating && BatchedEvents.isIteratingOverTypeIndex == archetypeVariant)
			{
				throw new Exception("Cannot add or remove entities during an event (Update/LateUpdate/...) that this helper initiated");
			}
			Type type = obj.GetType();
			for (int i = 0; i < BatchedEvents.data.Length; i++)
			{
				if (BatchedEvents.data[i].type == type && BatchedEvents.data[i].variant == archetypeVariant)
				{
					BatchedEvents.data[i].Add(obj);
					return;
				}
			}
			Memory.Realloc<BatchedEvents.Archetype>(ref BatchedEvents.data, BatchedEvents.data.Length + 1);
			Action<T[], int, TransformAccessArray, BatchedEvents.Event> ac1 = action1;
			Action<T[], int> ac2 = action2;
			Action<object[], int, TransformAccessArray, BatchedEvents.Event> action3 = delegate(object[] objs, int count, TransformAccessArray tr, BatchedEvents.Event ev)
			{
				ac1((T[])objs, count, tr, ev);
			};
			Action<object[], int, TransformAccessArray, BatchedEvents.Event> action4 = delegate(object[] objs, int count, TransformAccessArray tr, BatchedEvents.Event ev)
			{
				ac2((T[])objs, count);
			};
			BatchedEvents.data[BatchedEvents.data.Length - 1] = new BatchedEvents.Archetype
			{
				type = type,
				events = eventTypes,
				variant = archetypeVariant,
				archetypeIndex = BatchedEvents.data.Length - 1 + 1,
				action = ((ac1 != null) ? action3 : action4),
				sampler = CustomSampler.Create(type.Name, false)
			};
			BatchedEvents.data[BatchedEvents.data.Length - 1].Add(obj);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00060370 File Offset: 0x0005E570
		private void Process(BatchedEvents.Event eventType, Type typeFilter)
		{
			try
			{
				BatchedEvents.isIterating = true;
				for (int i = 0; i < BatchedEvents.data.Length; i++)
				{
					ref BatchedEvents.Archetype ptr = ref BatchedEvents.data[i];
					if (ptr.objectCount > 0 && (ptr.events & eventType) != BatchedEvents.Event.None && (typeFilter == null || typeFilter == ptr.type))
					{
						BatchedEvents.isIteratingOverTypeIndex = ptr.variant;
						try
						{
							ptr.action(ptr.objects, ptr.objectCount, ptr.transforms, eventType);
						}
						finally
						{
						}
					}
				}
			}
			finally
			{
				BatchedEvents.isIterating = false;
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0006041C File Offset: 0x0005E61C
		public static void ProcessEvent<T>(BatchedEvents.Event eventType)
		{
			BatchedEvents batchedEvents = BatchedEvents.instance;
			if (batchedEvents == null)
			{
				return;
			}
			batchedEvents.Process(eventType, typeof(T));
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00060438 File Offset: 0x0005E638
		private void Update()
		{
			this.Process(BatchedEvents.Event.Update, null);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00060442 File Offset: 0x0005E642
		private void LateUpdate()
		{
			this.Process(BatchedEvents.Event.LateUpdate, null);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0006044C File Offset: 0x0005E64C
		private void FixedUpdate()
		{
			this.Process(BatchedEvents.Event.FixedUpdate, null);
		}

		// Token: 0x04000B75 RID: 2933
		private const int ArchetypeOffset = 22;

		// Token: 0x04000B76 RID: 2934
		private const int ArchetypeMask = 1069547520;

		// Token: 0x04000B77 RID: 2935
		private static BatchedEvents.Archetype[] data = new BatchedEvents.Archetype[0];

		// Token: 0x04000B78 RID: 2936
		private static BatchedEvents instance;

		// Token: 0x04000B79 RID: 2937
		private static int isIteratingOverTypeIndex = -1;

		// Token: 0x04000B7A RID: 2938
		private static bool isIterating = false;

		// Token: 0x0200028F RID: 655
		[Flags]
		public enum Event
		{
			// Token: 0x04000B7C RID: 2940
			Update = 1,
			// Token: 0x04000B7D RID: 2941
			LateUpdate = 2,
			// Token: 0x04000B7E RID: 2942
			FixedUpdate = 4,
			// Token: 0x04000B7F RID: 2943
			Custom = 8,
			// Token: 0x04000B80 RID: 2944
			None = 0
		}

		// Token: 0x02000290 RID: 656
		private struct Archetype
		{
			// Token: 0x06000FB8 RID: 4024 RVA: 0x00060470 File Offset: 0x0005E670
			public void Add(Component obj)
			{
				this.objectCount++;
				if (this.objects == null)
				{
					this.objects = (object[])Array.CreateInstance(this.type, math.ceilpow2(this.objectCount));
				}
				if (this.objectCount > this.objects.Length)
				{
					Array array = Array.CreateInstance(this.type, math.ceilpow2(this.objectCount));
					this.objects.CopyTo(array, 0);
					this.objects = (object[])array;
				}
				this.objects[this.objectCount - 1] = obj;
				if (!this.transforms.isCreated)
				{
					this.transforms = new TransformAccessArray(16, -1);
				}
				this.transforms.Add(obj.transform);
				((IEntityIndex)obj).EntityIndex = (this.archetypeIndex << 22 | this.objectCount - 1);
			}

			// Token: 0x06000FB9 RID: 4025 RVA: 0x00060550 File Offset: 0x0005E750
			public void Remove(int index)
			{
				this.objectCount--;
				((IEntityIndex)this.objects[this.objectCount]).EntityIndex = (this.archetypeIndex << 22 | index);
				((IEntityIndex)this.objects[index]).EntityIndex = 0;
				this.objects[index] = this.objects[this.objectCount];
				this.objects[this.objectCount] = null;
				this.transforms.RemoveAtSwapBack(index);
				if (this.objectCount == 0)
				{
					this.transforms.Dispose();
				}
			}

			// Token: 0x04000B81 RID: 2945
			public object[] objects;

			// Token: 0x04000B82 RID: 2946
			public int objectCount;

			// Token: 0x04000B83 RID: 2947
			public Type type;

			// Token: 0x04000B84 RID: 2948
			public TransformAccessArray transforms;

			// Token: 0x04000B85 RID: 2949
			public int variant;

			// Token: 0x04000B86 RID: 2950
			public int archetypeIndex;

			// Token: 0x04000B87 RID: 2951
			public BatchedEvents.Event events;

			// Token: 0x04000B88 RID: 2952
			public Action<object[], int, TransformAccessArray, BatchedEvents.Event> action;

			// Token: 0x04000B89 RID: 2953
			public CustomSampler sampler;
		}
	}
}
