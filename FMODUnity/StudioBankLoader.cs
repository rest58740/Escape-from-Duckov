using System;
using System.Collections.Generic;
using UnityEngine;

namespace FMODUnity
{
	// Token: 0x0200012A RID: 298
	[AddComponentMenu("FMOD Studio/FMOD Studio Bank Loader")]
	public class StudioBankLoader : MonoBehaviour
	{
		// Token: 0x060007A2 RID: 1954 RVA: 0x0000B3F7 File Offset: 0x000095F7
		private void HandleGameEvent(LoaderGameEvent gameEvent)
		{
			if (this.LoadEvent == gameEvent)
			{
				this.Load();
			}
			if (this.UnloadEvent == gameEvent)
			{
				this.Unload();
			}
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000B417 File Offset: 0x00009617
		private void Start()
		{
			RuntimeUtils.EnforceLibraryOrder();
			this.HandleGameEvent(LoaderGameEvent.ObjectStart);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0000B425 File Offset: 0x00009625
		private void OnApplicationQuit()
		{
			this.isQuitting = true;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0000B42E File Offset: 0x0000962E
		private void OnDestroy()
		{
			if (!this.isQuitting)
			{
				this.HandleGameEvent(LoaderGameEvent.ObjectDestroy);
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0000B43F File Offset: 0x0000963F
		private void OnTriggerEnter(Collider other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag))
			{
				this.HandleGameEvent(LoaderGameEvent.TriggerEnter);
			}
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0000B463 File Offset: 0x00009663
		private void OnTriggerExit(Collider other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag))
			{
				this.HandleGameEvent(LoaderGameEvent.TriggerExit);
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0000B487 File Offset: 0x00009687
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag))
			{
				this.HandleGameEvent(LoaderGameEvent.TriggerEnter2D);
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0000B4AB File Offset: 0x000096AB
		private void OnTriggerExit2D(Collider2D other)
		{
			if (string.IsNullOrEmpty(this.CollisionTag) || other.CompareTag(this.CollisionTag))
			{
				this.HandleGameEvent(LoaderGameEvent.TriggerExit2D);
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0000B4CF File Offset: 0x000096CF
		private void OnEnable()
		{
			this.HandleGameEvent(LoaderGameEvent.ObjectEnable);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0000B4D8 File Offset: 0x000096D8
		private void OnDisable()
		{
			this.HandleGameEvent(LoaderGameEvent.ObjectDisable);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public void Load()
		{
			foreach (string bankName in this.Banks)
			{
				try
				{
					RuntimeManager.LoadBank(bankName, this.PreloadSamples);
				}
				catch (BankLoadException e)
				{
					RuntimeUtils.DebugLogException(e);
				}
			}
			RuntimeManager.WaitForAllSampleLoading();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0000B558 File Offset: 0x00009758
		public void Unload()
		{
			foreach (string bankName in this.Banks)
			{
				RuntimeManager.UnloadBank(bankName);
			}
		}

		// Token: 0x04000656 RID: 1622
		public LoaderGameEvent LoadEvent;

		// Token: 0x04000657 RID: 1623
		public LoaderGameEvent UnloadEvent;

		// Token: 0x04000658 RID: 1624
		[BankRef]
		public List<string> Banks;

		// Token: 0x04000659 RID: 1625
		public string CollisionTag;

		// Token: 0x0400065A RID: 1626
		public bool PreloadSamples;

		// Token: 0x0400065B RID: 1627
		private bool isQuitting;
	}
}
