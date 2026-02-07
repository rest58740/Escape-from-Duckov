using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000065 RID: 101
	[HelpURL("https://arongranberg.com/astar/documentation/stable/animationlink.html")]
	public class AnimationLink : NodeLink2
	{
		// Token: 0x0600038B RID: 907 RVA: 0x000115A8 File Offset: 0x0000F7A8
		private static Transform SearchRec(Transform tr, string name)
		{
			int childCount = tr.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = tr.GetChild(i);
				if (child.name == name)
				{
					return child;
				}
				Transform transform = AnimationLink.SearchRec(child, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000115F4 File Offset: 0x0000F7F4
		public void CalculateOffsets(List<Vector3> trace, out Vector3 endPosition)
		{
			endPosition = base.transform.position;
			if (this.referenceMesh == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.referenceMesh, base.transform.position, base.transform.rotation);
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			Transform transform = AnimationLink.SearchRec(gameObject.transform, this.boneRoot);
			if (transform == null)
			{
				throw new Exception("Could not find root transform");
			}
			Animation animation = gameObject.GetComponent<Animation>();
			if (animation == null)
			{
				animation = gameObject.AddComponent<Animation>();
			}
			for (int i = 0; i < this.sequence.Length; i++)
			{
				animation.AddClip(this.sequence[i].clip, this.sequence[i].clip.name);
			}
			Vector3 a = Vector3.zero;
			Vector3 vector = base.transform.position;
			Vector3 b = Vector3.zero;
			for (int j = 0; j < this.sequence.Length; j++)
			{
				AnimationLink.LinkClip linkClip = this.sequence[j];
				if (linkClip == null)
				{
					endPosition = vector;
					return;
				}
				animation[linkClip.clip.name].enabled = true;
				animation[linkClip.clip.name].weight = 1f;
				for (int k = 0; k < linkClip.loopCount; k++)
				{
					animation[linkClip.clip.name].normalizedTime = 0f;
					animation.Sample();
					Vector3 vector2 = transform.position - base.transform.position;
					if (j > 0)
					{
						vector += a - vector2;
					}
					else
					{
						b = vector2;
					}
					for (int l = 0; l <= 20; l++)
					{
						float num = (float)l / 20f;
						animation[linkClip.clip.name].normalizedTime = num;
						animation.Sample();
						Vector3 item = vector + (transform.position - base.transform.position) + linkClip.velocity * num * linkClip.clip.length;
						trace.Add(item);
					}
					vector += linkClip.velocity * 1f * linkClip.clip.length;
					animation[linkClip.clip.name].normalizedTime = 1f;
					animation.Sample();
					a = transform.position - base.transform.position;
				}
				animation[linkClip.clip.name].enabled = false;
				animation[linkClip.clip.name].weight = 0f;
			}
			vector += a - b;
			UnityEngine.Object.DestroyImmediate(gameObject);
			endPosition = vector;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000118FC File Offset: 0x0000FAFC
		public override void DrawGizmos()
		{
			base.DrawGizmos();
			List<Vector3> list = ListPool<Vector3>.Claim();
			Vector3 zero = Vector3.zero;
			this.CalculateOffsets(list, out zero);
			for (int i = 0; i < list.Count - 1; i++)
			{
				Draw.Line(list[i], list[i + 1], Color.blue);
			}
		}

		// Token: 0x04000229 RID: 553
		public string clip;

		// Token: 0x0400022A RID: 554
		public float animSpeed = 1f;

		// Token: 0x0400022B RID: 555
		public bool reverseAnim = true;

		// Token: 0x0400022C RID: 556
		public GameObject referenceMesh;

		// Token: 0x0400022D RID: 557
		public AnimationLink.LinkClip[] sequence;

		// Token: 0x0400022E RID: 558
		public string boneRoot = "bn_COG_Root";

		// Token: 0x02000066 RID: 102
		[Serializable]
		public class LinkClip
		{
			// Token: 0x170000AA RID: 170
			// (get) Token: 0x0600038F RID: 911 RVA: 0x00011976 File Offset: 0x0000FB76
			public string name
			{
				get
				{
					if (!(this.clip != null))
					{
						return "";
					}
					return this.clip.name;
				}
			}

			// Token: 0x0400022F RID: 559
			public AnimationClip clip;

			// Token: 0x04000230 RID: 560
			public Vector3 velocity;

			// Token: 0x04000231 RID: 561
			public int loopCount = 1;
		}
	}
}
