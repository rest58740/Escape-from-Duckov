using System;
using System.Collections.Generic;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E7 RID: 231
public class DuckovItemAgent : ItemAgent
{
	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000760 RID: 1888 RVA: 0x00021433 File Offset: 0x0001F633
	public CharacterMainControl Holder
	{
		get
		{
			return this.holder;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x0002143C File Offset: 0x0001F63C
	private Dictionary<string, Transform> SocketsDic
	{
		get
		{
			if (this._socketsDic == null)
			{
				this._socketsDic = new Dictionary<string, Transform>();
				foreach (Transform transform in this.socketsList)
				{
					if (!this._socketsDic.ContainsKey(transform.name))
					{
						this._socketsDic.Add(transform.name, transform);
					}
				}
			}
			return this._socketsDic;
		}
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x000214C8 File Offset: 0x0001F6C8
	public void AddSocket(Transform socket)
	{
		if (socket == null)
		{
			return;
		}
		if (!this.socketsList.Contains(socket))
		{
			this.socketsList.Add(socket);
		}
		if (this.SocketsDic.ContainsKey(socket.name))
		{
			this.SocketsDic.Remove(socket.name);
		}
		this.SocketsDic.Add(socket.name, socket);
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00021530 File Offset: 0x0001F730
	public Transform GetSocket(string socketName, bool createNew)
	{
		Transform transform;
		bool flag = this.SocketsDic.TryGetValue(socketName, out transform);
		if (flag && transform == null)
		{
			this.SocketsDic.Remove(socketName);
			flag = false;
		}
		if (!flag && createNew)
		{
			transform = new GameObject(socketName).transform;
			transform.SetParent(base.transform);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			this.AddSocket(transform);
		}
		return transform;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x000215B0 File Offset: 0x0001F7B0
	public void SetHolder(CharacterMainControl _holder)
	{
		this.holder = _holder;
		if (this.setActiveIfMainCharacter)
		{
			this.setActiveIfMainCharacter.SetActive(_holder.IsMainCharacter);
		}
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x000215D7 File Offset: 0x0001F7D7
	public CharacterMainControl GetHolder()
	{
		return this.holder;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x000215DF File Offset: 0x0001F7DF
	protected override void OnInitialize()
	{
		base.OnInitialize();
		this.InitInterfaces();
		UnityEvent onInitializdEvent = this.OnInitializdEvent;
		if (onInitializdEvent == null)
		{
			return;
		}
		onInitializdEvent.Invoke();
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x000215FD File Offset: 0x0001F7FD
	private void InitInterfaces()
	{
		this.usableInterface = (this as IAgentUsable);
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000768 RID: 1896 RVA: 0x0002160B File Offset: 0x0001F80B
	public IAgentUsable UsableInterface
	{
		get
		{
			return this.usableInterface;
		}
	}

	// Token: 0x04000717 RID: 1815
	public HandheldSocketTypes handheldSocket = HandheldSocketTypes.normalHandheld;

	// Token: 0x04000718 RID: 1816
	public HandheldAnimationType handAnimationType = HandheldAnimationType.normal;

	// Token: 0x04000719 RID: 1817
	private CharacterMainControl holder;

	// Token: 0x0400071A RID: 1818
	public UnityEvent OnInitializdEvent;

	// Token: 0x0400071B RID: 1819
	[SerializeField]
	private List<Transform> socketsList = new List<Transform>();

	// Token: 0x0400071C RID: 1820
	public GameObject setActiveIfMainCharacter;

	// Token: 0x0400071D RID: 1821
	private Dictionary<string, Transform> _socketsDic;

	// Token: 0x0400071E RID: 1822
	private IAgentUsable usableInterface;
}
