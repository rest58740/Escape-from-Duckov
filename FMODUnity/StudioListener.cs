using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x0200012D RID: 301
	[AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
	public class StudioListener : MonoBehaviour
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0000BEAD File Offset: 0x0000A0AD
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0000BEB5 File Offset: 0x0000A0B5
		public GameObject AttenuationObject
		{
			get
			{
				return this.attenuationObject;
			}
			set
			{
				this.attenuationObject = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		public static int ListenerCount
		{
			get
			{
				return StudioListener.listeners.Count;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0000BECA File Offset: 0x0000A0CA
		public int ListenerNumber
		{
			get
			{
				return StudioListener.listeners.IndexOf(this);
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		public static float DistanceToNearestListener(Vector3 position)
		{
			float num = float.MaxValue;
			for (int i = 0; i < StudioListener.listeners.Count; i++)
			{
				if (StudioListener.listeners[i].attenuationObject == null)
				{
					num = Mathf.Min(num, Vector3.Distance(position, StudioListener.listeners[i].transform.position));
				}
				else
				{
					num = Mathf.Min(num, Vector3.Distance(position, StudioListener.listeners[i].attenuationObject.transform.position));
				}
			}
			return num;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0000BF64 File Offset: 0x0000A164
		public static float DistanceSquaredToNearestListener(Vector3 position)
		{
			float num = float.MaxValue;
			for (int i = 0; i < StudioListener.listeners.Count; i++)
			{
				if (StudioListener.listeners[i].attenuationObject == null)
				{
					num = Mathf.Min(num, (position - StudioListener.listeners[i].transform.position).sqrMagnitude);
				}
				else
				{
					num = Mathf.Min(num, (position - StudioListener.listeners[i].attenuationObject.transform.position).sqrMagnitude);
				}
			}
			return num;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0000C004 File Offset: 0x0000A204
		private static void AddListener(StudioListener listener)
		{
			if (StudioListener.listeners.Contains(listener))
			{
				Debug.LogWarning(string.Format("[FMOD] Listener has already been added at index {0}.", listener.ListenerNumber));
				return;
			}
			if (StudioListener.listeners.Count >= 8)
			{
				Debug.LogWarning(string.Format("[FMOD] Max number of listeners reached : {0}.", 8));
			}
			StudioListener.listeners.Add(listener);
			RuntimeManager.StudioSystem.setNumListeners(Mathf.Clamp(StudioListener.listeners.Count, 1, 8));
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0000C088 File Offset: 0x0000A288
		private static void RemoveListener(StudioListener listener)
		{
			StudioListener.listeners.Remove(listener);
			RuntimeManager.StudioSystem.setNumListeners(Mathf.Clamp(StudioListener.listeners.Count, 1, 8));
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		private void OnEnable()
		{
			RuntimeUtils.EnforceLibraryOrder();
			this.rigidBody = base.gameObject.GetComponent<Rigidbody>();
			if (this.nonRigidbodyVelocity && this.rigidBody)
			{
				Debug.LogWarning(string.Format("[FMOD] Non-Rigidbody Velocity is enabled on Listener attached to GameObject \"{0}\", which also has a Rigidbody component attached - this will be disabled in favor of velocity from Rigidbody component.", base.name));
				this.nonRigidbodyVelocity = false;
			}
			this.rigidBody2D = base.gameObject.GetComponent<Rigidbody2D>();
			if (this.nonRigidbodyVelocity && this.rigidBody2D)
			{
				Debug.LogWarning(string.Format("[FMOD] Non-Rigidbody Velocity is enabled on Listener attached to GameObject \"{0}\", which also has a Rigidbody2D component attached - this will be disabled in favor of velocity from Rigidbody2D component.", base.name));
				this.nonRigidbodyVelocity = false;
			}
			StudioListener.AddListener(this);
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0000C16D File Offset: 0x0000A36D
		private void OnDisable()
		{
			StudioListener.RemoveListener(this);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0000C178 File Offset: 0x0000A378
		private void Update()
		{
			if (this.ListenerNumber < 0 || this.ListenerNumber >= 8)
			{
				return;
			}
			if (this.nonRigidbodyVelocity)
			{
				Vector3 vector = Vector3.zero;
				Vector3 position = base.transform.position;
				if (Time.deltaTime != 0f)
				{
					vector = (position - this.lastFramePosition) / Time.deltaTime;
					vector = Vector3.ClampMagnitude(vector, 20f);
				}
				this.lastFramePosition = position;
				RuntimeManager.SetListenerLocation(this.ListenerNumber, base.gameObject, this.attenuationObject, vector);
				return;
			}
			if (this.rigidBody)
			{
				RuntimeManager.SetListenerLocation(this.ListenerNumber, base.gameObject, this.rigidBody, this.attenuationObject);
				return;
			}
			if (this.rigidBody2D)
			{
				RuntimeManager.SetListenerLocation(this.ListenerNumber, base.gameObject, this.rigidBody2D, this.attenuationObject);
				return;
			}
			RuntimeManager.SetListenerLocation(this.ListenerNumber, base.gameObject, this.attenuationObject);
		}

		// Token: 0x04000675 RID: 1653
		[SerializeField]
		private bool nonRigidbodyVelocity;

		// Token: 0x04000676 RID: 1654
		[SerializeField]
		private GameObject attenuationObject;

		// Token: 0x04000677 RID: 1655
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x04000678 RID: 1656
		private Rigidbody rigidBody;

		// Token: 0x04000679 RID: 1657
		private Rigidbody2D rigidBody2D;

		// Token: 0x0400067A RID: 1658
		private static List<StudioListener> listeners = new List<StudioListener>();
	}
}
