using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000271 RID: 625
	public class NoteIndex : MonoBehaviour
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x0004A2D4 File Offset: 0x000484D4
		public static NoteIndex Instance
		{
			get
			{
				return GameManager.NoteIndex;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0004A2DB File Offset: 0x000484DB
		public List<Note> Notes
		{
			get
			{
				return this.notes;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x0004A2E3 File Offset: 0x000484E3
		private Dictionary<string, Note> MDic
		{
			get
			{
				if (this._dic == null)
				{
					this.RebuildDic();
				}
				return this._dic;
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0004A2FC File Offset: 0x000484FC
		private void RebuildDic()
		{
			if (this._dic == null)
			{
				this._dic = new Dictionary<string, Note>();
			}
			this._dic.Clear();
			foreach (Note note in this.notes)
			{
				this._dic[note.key] = note;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x0004A378 File Offset: 0x00048578
		public HashSet<string> UnlockedNotes
		{
			get
			{
				return this.unlockedNotes;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x0004A380 File Offset: 0x00048580
		public HashSet<string> ReadNotes
		{
			get
			{
				return this.unlockedNotes;
			}
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0004A388 File Offset: 0x00048588
		public static IEnumerable<string> GetAllNotes(bool unlockedOnly = true)
		{
			if (NoteIndex.Instance == null)
			{
				yield break;
			}
			foreach (Note note in NoteIndex.Instance.notes)
			{
				string key = note.key;
				if (!note.hide && (!unlockedOnly || NoteIndex.GetNoteUnlocked(key)))
				{
					yield return note.key;
				}
			}
			List<Note>.Enumerator enumerator = default(List<Note>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0004A398 File Offset: 0x00048598
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
			this.Load();
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0004A3C2 File Offset: 0x000485C2
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0004A3E8 File Offset: 0x000485E8
		private void Save()
		{
			NoteIndex.SaveData value = new NoteIndex.SaveData(this);
			SavesSystem.Save<NoteIndex.SaveData>("NoteIndexData", value);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0004A408 File Offset: 0x00048608
		private void Load()
		{
			SavesSystem.Load<NoteIndex.SaveData>("NoteIndexData").Setup(this);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0004A428 File Offset: 0x00048628
		public void MSetEntryDynamic(Note note)
		{
			this.MDic[note.key] = note;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0004A43C File Offset: 0x0004863C
		public Note MGetNote(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogError("Trying to get note with an empty key.");
				return null;
			}
			Note result;
			if (!this.MDic.TryGetValue(key, out result))
			{
				Debug.LogError("Cannot find note: " + key);
				return null;
			}
			return result;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0004A480 File Offset: 0x00048680
		public static Note GetNote(string key)
		{
			if (NoteIndex.Instance == null)
			{
				return null;
			}
			return NoteIndex.Instance.MGetNote(key);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0004A49C File Offset: 0x0004869C
		public static bool SetNoteDynamic(Note note)
		{
			if (NoteIndex.Instance == null)
			{
				return false;
			}
			NoteIndex.Instance.MSetEntryDynamic(note);
			return true;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0004A4B9 File Offset: 0x000486B9
		public static bool GetNoteUnlocked(string noteKey)
		{
			return !(NoteIndex.Instance == null) && NoteIndex.Instance.unlockedNotes.Contains(noteKey);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0004A4DA File Offset: 0x000486DA
		public static bool GetNoteRead(string noteKey)
		{
			return !(NoteIndex.Instance == null) && NoteIndex.Instance.readNotes.Contains(noteKey);
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0004A4FB File Offset: 0x000486FB
		public static void SetNoteUnlocked(string noteKey)
		{
			if (NoteIndex.Instance == null)
			{
				return;
			}
			NoteIndex.Instance.unlockedNotes.Add(noteKey);
			Action<string> action = NoteIndex.onNoteStatusChanged;
			if (action == null)
			{
				return;
			}
			action(noteKey);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0004A52C File Offset: 0x0004872C
		public static void SetNoteRead(string noteKey)
		{
			if (NoteIndex.Instance == null)
			{
				return;
			}
			NoteIndex.Instance.readNotes.Add(noteKey);
			Action<string> action = NoteIndex.onNoteStatusChanged;
			if (action == null)
			{
				return;
			}
			action(noteKey);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0004A560 File Offset: 0x00048760
		internal static int GetTotalNoteCount()
		{
			if (NoteIndex.Instance == null)
			{
				return 0;
			}
			return (from e in NoteIndex.Instance.Notes
			where !e.hide
			select e).Count<Note>();
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0004A5AF File Offset: 0x000487AF
		internal static int GetUnlockedNoteCount()
		{
			if (NoteIndex.Instance == null)
			{
				return 0;
			}
			return NoteIndex.Instance.UnlockedNotes.Count;
		}

		// Token: 0x04000EC2 RID: 3778
		[SerializeField]
		private List<Note> notes = new List<Note>();

		// Token: 0x04000EC3 RID: 3779
		private Dictionary<string, Note> _dic;

		// Token: 0x04000EC4 RID: 3780
		private HashSet<string> unlockedNotes = new HashSet<string>();

		// Token: 0x04000EC5 RID: 3781
		private HashSet<string> readNotes = new HashSet<string>();

		// Token: 0x04000EC6 RID: 3782
		public static Action<string> onNoteStatusChanged;

		// Token: 0x04000EC7 RID: 3783
		private const string SaveKey = "NoteIndexData";

		// Token: 0x0200055C RID: 1372
		[Serializable]
		private struct SaveData
		{
			// Token: 0x06002906 RID: 10502 RVA: 0x00096DAB File Offset: 0x00094FAB
			public SaveData(NoteIndex from)
			{
				this.unlockedNotes = from.unlockedNotes.ToList<string>();
				this.readNotes = from.unlockedNotes.ToList<string>();
			}

			// Token: 0x06002907 RID: 10503 RVA: 0x00096DD0 File Offset: 0x00094FD0
			public void Setup(NoteIndex to)
			{
				to.unlockedNotes.Clear();
				if (this.unlockedNotes != null)
				{
					LinqExtensions.AddRange<string>(to.unlockedNotes, this.unlockedNotes);
				}
				to.readNotes.Clear();
				if (this.readNotes != null)
				{
					LinqExtensions.AddRange<string>(to.readNotes, this.readNotes);
				}
			}

			// Token: 0x04001F79 RID: 8057
			public List<string> unlockedNotes;

			// Token: 0x04001F7A RID: 8058
			public List<string> readNotes;
		}
	}
}
