using System;
using System.Collections.Generic;
using UnityEngine;

namespace FX
{
	// Token: 0x02000217 RID: 535
	public class PopText : MonoBehaviour
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0003FDA1 File Offset: 0x0003DFA1
		private void Awake()
		{
			PopText.instance = this;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003FDAC File Offset: 0x0003DFAC
		private PopTextEntity GetOrCreateEntry()
		{
			PopTextEntity popTextEntity;
			if (this.inactiveEntries.Count > 0)
			{
				popTextEntity = this.inactiveEntries[0];
				this.inactiveEntries.RemoveAt(0);
			}
			popTextEntity = Object.Instantiate<PopTextEntity>(this.popTextPrefab, base.transform);
			this.activeEntries.Add(popTextEntity);
			popTextEntity.gameObject.SetActive(true);
			return popTextEntity;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003FE0C File Offset: 0x0003E00C
		public void InstancePop(string text, Vector3 worldPosition, Color color, float size, Sprite sprite = null)
		{
			PopTextEntity orCreateEntry = this.GetOrCreateEntry();
			orCreateEntry.Color = color;
			orCreateEntry.size = size;
			orCreateEntry.transform.localScale = Vector3.one * size;
			Transform transform = orCreateEntry.transform;
			transform.position = worldPosition;
			transform.rotation = PopText.LookAtMainCamera(worldPosition);
			float num = Random.Range(-this.randomAngle, this.randomAngle);
			float num2 = Random.Range(-this.randomAngle, this.randomAngle);
			Vector3 vector = Quaternion.Euler(num, 0f, num2) * Vector3.up;
			orCreateEntry.SetupContent(text, sprite);
			orCreateEntry.velocity = vector * this.spawnVelocity;
			orCreateEntry.spawnTime = Time.time;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0003FEC0 File Offset: 0x0003E0C0
		private static Quaternion LookAtMainCamera(Vector3 position)
		{
			if (Camera.main)
			{
				Transform transform = Camera.main.transform;
				return Quaternion.LookRotation(-(transform.position - position), transform.up);
			}
			return Quaternion.identity;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0003FF06 File Offset: 0x0003E106
		public void Recycle(PopTextEntity entry)
		{
			entry.gameObject.SetActive(false);
			this.activeEntries.Remove(entry);
			this.inactiveEntries.Add(entry);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003FF30 File Offset: 0x0003E130
		private void Update()
		{
			float deltaTime = Time.deltaTime;
			Vector3 vector = Vector3.up * this.gravityValue;
			bool flag = false;
			foreach (PopTextEntity popTextEntity in this.activeEntries)
			{
				if (popTextEntity == null)
				{
					flag = true;
				}
				else
				{
					Transform transform = popTextEntity.transform;
					transform.position += popTextEntity.velocity * deltaTime;
					transform.rotation = PopText.LookAtMainCamera(transform.position);
					popTextEntity.velocity += vector * deltaTime;
					popTextEntity.transform.localScale = this.sizeOverLife.Evaluate(popTextEntity.timeSinceSpawn / this.lifeTime) * popTextEntity.size * Vector3.one;
					float num = Mathf.Clamp01(popTextEntity.timeSinceSpawn / this.lifeTime * 2f - 1f);
					Color color = Color.Lerp(popTextEntity.Color, popTextEntity.EndColor, num);
					popTextEntity.SetColor(color);
					if (popTextEntity.timeSinceSpawn > this.lifeTime)
					{
						this.recycleList.Add(popTextEntity);
					}
				}
			}
			if (this.recycleList.Count > 0)
			{
				foreach (PopTextEntity entry in this.recycleList)
				{
					this.Recycle(entry);
				}
				this.recycleList.Clear();
			}
			if (flag)
			{
				this.activeEntries.RemoveAll((PopTextEntity e) => e == null);
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00040134 File Offset: 0x0003E334
		private void PopTest()
		{
			Vector3 worldPosition = base.transform.position;
			CharacterMainControl main = CharacterMainControl.Main;
			if (main != null)
			{
				worldPosition = main.transform.position + Vector3.up * 2f;
			}
			this.InstancePop("Test", worldPosition, Color.white, 1f, this.debugSprite);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00040198 File Offset: 0x0003E398
		public static void Pop(string text, Vector3 worldPosition, Color color, float size, Sprite sprite = null)
		{
			if (DevCam.devCamOn)
			{
				return;
			}
			if (PopText.instance)
			{
				PopText.instance.InstancePop(text, worldPosition, color, size, sprite);
			}
		}

		// Token: 0x04000D0D RID: 3341
		public static PopText instance;

		// Token: 0x04000D0E RID: 3342
		public PopTextEntity popTextPrefab;

		// Token: 0x04000D0F RID: 3343
		public List<PopTextEntity> inactiveEntries;

		// Token: 0x04000D10 RID: 3344
		public List<PopTextEntity> activeEntries;

		// Token: 0x04000D11 RID: 3345
		public float spawnVelocity = 5f;

		// Token: 0x04000D12 RID: 3346
		public float gravityValue = -9.8f;

		// Token: 0x04000D13 RID: 3347
		public float lifeTime = 1f;

		// Token: 0x04000D14 RID: 3348
		public AnimationCurve sizeOverLife;

		// Token: 0x04000D15 RID: 3349
		public float randomAngle = 10f;

		// Token: 0x04000D16 RID: 3350
		public Sprite debugSprite;

		// Token: 0x04000D17 RID: 3351
		private List<PopTextEntity> recycleList = new List<PopTextEntity>();
	}
}
