using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x0200032D RID: 813
	public class BuildingManager : MonoBehaviour
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x00061FE5 File Offset: 0x000601E5
		// (set) Token: 0x06001B07 RID: 6919 RVA: 0x00061FEC File Offset: 0x000601EC
		public static BuildingManager Instance { get; private set; }

		// Token: 0x06001B08 RID: 6920 RVA: 0x00061FF4 File Offset: 0x000601F4
		private static int GenerateBuildingGUID(string buildingID)
		{
			BuildingManager.<>c__DisplayClass4_0 CS$<>8__locals1 = new BuildingManager.<>c__DisplayClass4_0();
			CS$<>8__locals1.<GenerateBuildingGUID>g__Regenerate|0();
			while (BuildingManager.Any((BuildingManager.BuildingData e) => e != null && e.GUID == CS$<>8__locals1.result))
			{
				CS$<>8__locals1.<GenerateBuildingGUID>g__Regenerate|0();
			}
			return CS$<>8__locals1.result;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x00062030 File Offset: 0x00060230
		public int GetTokenAmount(string id)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry != null)
			{
				return buildingTokenAmountEntry.amount;
			}
			return 0;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x00062070 File Offset: 0x00060270
		private void SetTokenAmount(string id, int amount)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry != null)
			{
				buildingTokenAmountEntry.amount = amount;
				return;
			}
			buildingTokenAmountEntry = new BuildingManager.BuildingTokenAmountEntry
			{
				id = id,
				amount = amount
			};
			this.tokens.Add(buildingTokenAmountEntry);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000620D4 File Offset: 0x000602D4
		private void AddToken(string id, int amount = 1)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry == null)
			{
				buildingTokenAmountEntry = new BuildingManager.BuildingTokenAmountEntry
				{
					id = id,
					amount = 0
				};
				this.tokens.Add(buildingTokenAmountEntry);
			}
			buildingTokenAmountEntry.amount += amount;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0006213C File Offset: 0x0006033C
		private bool PayToken(string id)
		{
			BuildingManager.BuildingTokenAmountEntry buildingTokenAmountEntry = this.tokens.Find((BuildingManager.BuildingTokenAmountEntry e) => e.id == id);
			if (buildingTokenAmountEntry == null)
			{
				return false;
			}
			if (buildingTokenAmountEntry.amount <= 0)
			{
				return false;
			}
			buildingTokenAmountEntry.amount--;
			return true;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x00062190 File Offset: 0x00060390
		public static Vector2Int[] GetOccupyingCoords(Vector2Int dimensions, BuildingRotation rotations, Vector2Int coord)
		{
			if (rotations % BuildingRotation.Half != BuildingRotation.Zero)
			{
				dimensions..ctor(dimensions.y, dimensions.x);
			}
			Vector2Int[] array = new Vector2Int[dimensions.x * dimensions.y];
			for (int i = 0; i < dimensions.y; i++)
			{
				for (int j = 0; j < dimensions.x; j++)
				{
					int num = j + dimensions.x * i;
					array[num] = coord + new Vector2Int(j, i);
				}
			}
			return array;
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x00062211 File Offset: 0x00060411
		public List<BuildingManager.BuildingAreaData> Areas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0006221C File Offset: 0x0006041C
		public BuildingManager.BuildingAreaData GetOrCreateArea(string id)
		{
			BuildingManager.BuildingAreaData buildingAreaData = this.areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == id);
			if (buildingAreaData != null)
			{
				return buildingAreaData;
			}
			BuildingManager.BuildingAreaData buildingAreaData2 = new BuildingManager.BuildingAreaData(id);
			this.areas.Add(buildingAreaData2);
			return buildingAreaData2;
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0006226C File Offset: 0x0006046C
		public BuildingManager.BuildingAreaData GetArea(string id)
		{
			return this.areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == id);
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x0006229D File Offset: 0x0006049D
		private void CleanupAndSort()
		{
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x0006229F File Offset: 0x0006049F
		public static BuildingInfo GetBuildingInfo(string id)
		{
			return BuildingDataCollection.GetInfo(id);
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000622A8 File Offset: 0x000604A8
		public static bool Any(string id, bool includeTokens = false)
		{
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			if (includeTokens && BuildingManager.Instance.GetTokenAmount(id) > 0)
			{
				return true;
			}
			using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Any(id))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x00062328 File Offset: 0x00060528
		public static bool Any(Func<BuildingManager.BuildingData, bool> predicate)
		{
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Any(predicate))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x00062398 File Offset: 0x00060598
		public static int GetBuildingAmount(string id)
		{
			if (BuildingManager.Instance == null)
			{
				return 0;
			}
			int num = 0;
			foreach (BuildingManager.BuildingAreaData buildingAreaData in BuildingManager.Instance.Areas)
			{
				using (List<BuildingManager.BuildingData>.Enumerator enumerator2 = buildingAreaData.Buildings.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.ID == id)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06001B16 RID: 6934 RVA: 0x00062444 File Offset: 0x00060644
		// (remove) Token: 0x06001B17 RID: 6935 RVA: 0x00062478 File Offset: 0x00060678
		public static event Action OnBuildingListChanged;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06001B18 RID: 6936 RVA: 0x000624AC File Offset: 0x000606AC
		// (remove) Token: 0x06001B19 RID: 6937 RVA: 0x000624E0 File Offset: 0x000606E0
		public static event Action<int> OnBuildingBuilt;

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06001B1A RID: 6938 RVA: 0x00062514 File Offset: 0x00060714
		// (remove) Token: 0x06001B1B RID: 6939 RVA: 0x00062548 File Offset: 0x00060748
		public static event Action<int> OnBuildingDestroyed;

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06001B1C RID: 6940 RVA: 0x0006257C File Offset: 0x0006077C
		// (remove) Token: 0x06001B1D RID: 6941 RVA: 0x000625B0 File Offset: 0x000607B0
		public static event Action<int, BuildingInfo> OnBuildingBuiltComplex;

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06001B1E RID: 6942 RVA: 0x000625E4 File Offset: 0x000607E4
		// (remove) Token: 0x06001B1F RID: 6943 RVA: 0x00062618 File Offset: 0x00060818
		public static event Action<int, BuildingInfo> OnBuildingDestroyedComplex;

		// Token: 0x06001B20 RID: 6944 RVA: 0x0006264B File Offset: 0x0006084B
		private void Awake()
		{
			BuildingManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			this.Load();
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0006266A File Offset: 0x0006086A
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0006267D File Offset: 0x0006087D
		private void OnCollectSaveData()
		{
			this.Save();
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00062688 File Offset: 0x00060888
		private void Load()
		{
			BuildingManager.SaveData saveData = SavesSystem.Load<BuildingManager.SaveData>("BuildingData");
			this.areas.Clear();
			if (saveData.data != null)
			{
				this.areas.AddRange(saveData.data);
			}
			this.tokens.Clear();
			if (saveData.tokenAmounts != null)
			{
				this.tokens.AddRange(saveData.tokenAmounts);
			}
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x000626E8 File Offset: 0x000608E8
		private void Save()
		{
			BuildingManager.SaveData value = new BuildingManager.SaveData
			{
				data = new List<BuildingManager.BuildingAreaData>(this.areas),
				tokenAmounts = new List<BuildingManager.BuildingTokenAmountEntry>(this.tokens)
			};
			SavesSystem.Save<BuildingManager.SaveData>("BuildingData", value);
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00062730 File Offset: 0x00060930
		internal static BuildingManager.BuildingAreaData GetAreaData(string areaID)
		{
			if (BuildingManager.Instance == null)
			{
				return null;
			}
			return BuildingManager.Instance.Areas.Find((BuildingManager.BuildingAreaData e) => e != null && e.AreaID == areaID);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00062774 File Offset: 0x00060974
		internal static BuildingManager.BuildingAreaData GetOrCreateAreaData(string areaID)
		{
			if (BuildingManager.Instance == null)
			{
				return null;
			}
			return BuildingManager.Instance.GetOrCreateArea(areaID);
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00062790 File Offset: 0x00060990
		internal static BuildingManager.BuildingData GetBuildingData(int guid, string areaID = null)
		{
			if (areaID == null)
			{
				using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.Areas.GetEnumerator())
				{
					Predicate<BuildingManager.BuildingData> <>9__0;
					while (enumerator.MoveNext())
					{
						BuildingManager.BuildingAreaData buildingAreaData = enumerator.Current;
						List<BuildingManager.BuildingData> buildings = buildingAreaData.Buildings;
						Predicate<BuildingManager.BuildingData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((BuildingManager.BuildingData e) => e != null && e.GUID == guid));
						}
						BuildingManager.BuildingData buildingData = buildings.Find(match);
						if (buildingData != null)
						{
							return buildingData;
						}
					}
					goto IL_9B;
				}
				goto IL_74;
				IL_9B:
				return null;
			}
			IL_74:
			BuildingManager.BuildingAreaData areaData = BuildingManager.GetAreaData(areaID);
			if (areaData == null)
			{
				return null;
			}
			return areaData.Buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == guid);
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0006284C File Offset: 0x00060A4C
		internal static BuildingBuyAndPlaceResults BuyAndPlace(string areaID, string id, Vector2Int coord, BuildingRotation rotation)
		{
			if (BuildingManager.Instance == null)
			{
				return BuildingBuyAndPlaceResults.NoReferences;
			}
			BuildingInfo buildingInfo = BuildingManager.GetBuildingInfo(id);
			if (!buildingInfo.Valid)
			{
				return BuildingBuyAndPlaceResults.InvalidBuildingInfo;
			}
			BuildingManager.GetBuildingAmount(id);
			if (buildingInfo.ReachedAmountLimit)
			{
				return BuildingBuyAndPlaceResults.ReachedAmountLimit;
			}
			BuildingManager.Instance.GetTokenAmount(id);
			if (!BuildingManager.Instance.PayToken(id) && !buildingInfo.cost.Pay(true, true))
			{
				return BuildingBuyAndPlaceResults.PaymentFailure;
			}
			BuildingManager.BuildingAreaData orCreateArea = BuildingManager.Instance.GetOrCreateArea(areaID);
			int num = BuildingManager.GenerateBuildingGUID(id);
			orCreateArea.Add(id, rotation, coord, num);
			Action onBuildingListChanged = BuildingManager.OnBuildingListChanged;
			if (onBuildingListChanged != null)
			{
				onBuildingListChanged();
			}
			Action<int> onBuildingBuilt = BuildingManager.OnBuildingBuilt;
			if (onBuildingBuilt != null)
			{
				onBuildingBuilt(num);
			}
			Action<int, BuildingInfo> onBuildingBuiltComplex = BuildingManager.OnBuildingBuiltComplex;
			if (onBuildingBuiltComplex != null)
			{
				onBuildingBuiltComplex(num, buildingInfo);
			}
			AudioManager.Post("UI/building_up");
			return BuildingBuyAndPlaceResults.Succeed;
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x00062914 File Offset: 0x00060B14
		internal static bool DestroyBuilding(int guid, string areaID = null)
		{
			BuildingManager.BuildingData buildingData;
			BuildingManager.BuildingAreaData buildingAreaData;
			if (!BuildingManager.TryGetBuildingDataAndAreaData(guid, out buildingData, out buildingAreaData, areaID))
			{
				return false;
			}
			buildingAreaData.Remove(buildingData);
			Action onBuildingListChanged = BuildingManager.OnBuildingListChanged;
			if (onBuildingListChanged != null)
			{
				onBuildingListChanged();
			}
			Action<int> onBuildingDestroyed = BuildingManager.OnBuildingDestroyed;
			if (onBuildingDestroyed != null)
			{
				onBuildingDestroyed(guid);
			}
			Action<int, BuildingInfo> onBuildingDestroyedComplex = BuildingManager.OnBuildingDestroyedComplex;
			if (onBuildingDestroyedComplex != null)
			{
				onBuildingDestroyedComplex(guid, buildingData.Info);
			}
			return true;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00062974 File Offset: 0x00060B74
		internal static bool TryGetBuildingDataAndAreaData(int guid, out BuildingManager.BuildingData buildingData, out BuildingManager.BuildingAreaData areaData, string areaID = null)
		{
			buildingData = null;
			areaData = null;
			if (BuildingManager.Instance == null)
			{
				return false;
			}
			if (areaID == null)
			{
				using (List<BuildingManager.BuildingAreaData>.Enumerator enumerator = BuildingManager.Instance.areas.GetEnumerator())
				{
					Predicate<BuildingManager.BuildingData> <>9__0;
					while (enumerator.MoveNext())
					{
						BuildingManager.BuildingAreaData buildingAreaData = enumerator.Current;
						List<BuildingManager.BuildingData> buildings = buildingAreaData.Buildings;
						Predicate<BuildingManager.BuildingData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((BuildingManager.BuildingData e) => e != null && e.GUID == guid));
						}
						BuildingManager.BuildingData buildingData2 = buildings.Find(match);
						if (buildingData2 != null)
						{
							areaData = buildingAreaData;
							buildingData = buildingData2;
							return true;
						}
					}
					return false;
				}
			}
			BuildingManager.BuildingAreaData area = BuildingManager.Instance.GetArea(areaID);
			if (area == null)
			{
				return false;
			}
			BuildingManager.BuildingData buildingData3 = area.Buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == guid);
			if (buildingData3 != null)
			{
				areaData = area;
				buildingData = buildingData3;
			}
			return false;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00062A64 File Offset: 0x00060C64
		internal static UniTask<bool> ReturnBuilding(int guid, string areaID = null)
		{
			BuildingManager.<ReturnBuilding>d__53 <ReturnBuilding>d__;
			<ReturnBuilding>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<ReturnBuilding>d__.guid = guid;
			<ReturnBuilding>d__.areaID = areaID;
			<ReturnBuilding>d__.<>1__state = -1;
			<ReturnBuilding>d__.<>t__builder.Start<BuildingManager.<ReturnBuilding>d__53>(ref <ReturnBuilding>d__);
			return <ReturnBuilding>d__.<>t__builder.Task;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x00062AB0 File Offset: 0x00060CB0
		internal static UniTask<int> ReturnBuildings(string areaID = null, params int[] buildings)
		{
			BuildingManager.<ReturnBuildings>d__54 <ReturnBuildings>d__;
			<ReturnBuildings>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<ReturnBuildings>d__.areaID = areaID;
			<ReturnBuildings>d__.buildings = buildings;
			<ReturnBuildings>d__.<>1__state = -1;
			<ReturnBuildings>d__.<>t__builder.Start<BuildingManager.<ReturnBuildings>d__54>(ref <ReturnBuildings>d__);
			return <ReturnBuildings>d__.<>t__builder.Task;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00062AFC File Offset: 0x00060CFC
		internal static UniTask<int> ReturnBuildingsOfType(string buildingID, string areaID = null)
		{
			BuildingManager.<ReturnBuildingsOfType>d__55 <ReturnBuildingsOfType>d__;
			<ReturnBuildingsOfType>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<ReturnBuildingsOfType>d__.buildingID = buildingID;
			<ReturnBuildingsOfType>d__.areaID = areaID;
			<ReturnBuildingsOfType>d__.<>1__state = -1;
			<ReturnBuildingsOfType>d__.<>t__builder.Start<BuildingManager.<ReturnBuildingsOfType>d__55>(ref <ReturnBuildingsOfType>d__);
			return <ReturnBuildingsOfType>d__.<>t__builder.Task;
		}

		// Token: 0x04001369 RID: 4969
		private List<BuildingManager.BuildingTokenAmountEntry> tokens = new List<BuildingManager.BuildingTokenAmountEntry>();

		// Token: 0x0400136A RID: 4970
		[SerializeField]
		private List<BuildingManager.BuildingAreaData> areas = new List<BuildingManager.BuildingAreaData>();

		// Token: 0x04001370 RID: 4976
		private const string SaveKey = "BuildingData";

		// Token: 0x04001371 RID: 4977
		private static bool returningBuilding;

		// Token: 0x020005CD RID: 1485
		[Serializable]
		public class BuildingTokenAmountEntry
		{
			// Token: 0x04002133 RID: 8499
			public string id;

			// Token: 0x04002134 RID: 8500
			public int amount;
		}

		// Token: 0x020005CE RID: 1486
		[Serializable]
		public class BuildingAreaData
		{
			// Token: 0x170007AB RID: 1963
			// (get) Token: 0x060029F2 RID: 10738 RVA: 0x0009C1FF File Offset: 0x0009A3FF
			public string AreaID
			{
				get
				{
					return this.areaID;
				}
			}

			// Token: 0x170007AC RID: 1964
			// (get) Token: 0x060029F3 RID: 10739 RVA: 0x0009C207 File Offset: 0x0009A407
			public List<BuildingManager.BuildingData> Buildings
			{
				get
				{
					return this.buildings;
				}
			}

			// Token: 0x060029F4 RID: 10740 RVA: 0x0009C210 File Offset: 0x0009A410
			public bool Any(string buildingID)
			{
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					if (buildingData != null && buildingData.Info.Valid)
					{
						if (buildingData.ID == buildingID)
						{
							return true;
						}
						if (buildingData.Info.alternativeFor.Contains(buildingID))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x060029F5 RID: 10741 RVA: 0x0009C29C File Offset: 0x0009A49C
			public bool Add(string buildingID, BuildingRotation rotation, Vector2Int coord, int guid = -1)
			{
				BuildingManager.GetBuildingInfo(buildingID);
				if (guid < 0)
				{
					guid = BuildingManager.GenerateBuildingGUID(buildingID);
				}
				this.buildings.Add(new BuildingManager.BuildingData(guid, buildingID, rotation, coord));
				return true;
			}

			// Token: 0x060029F6 RID: 10742 RVA: 0x0009C2C8 File Offset: 0x0009A4C8
			public bool Remove(int buildingGUID)
			{
				BuildingManager.BuildingData buildingData = this.buildings.Find((BuildingManager.BuildingData e) => e != null && e.GUID == buildingGUID);
				return buildingData != null && this.buildings.Remove(buildingData);
			}

			// Token: 0x060029F7 RID: 10743 RVA: 0x0009C30B File Offset: 0x0009A50B
			public bool Remove(BuildingManager.BuildingData building)
			{
				return this.buildings.Remove(building);
			}

			// Token: 0x060029F8 RID: 10744 RVA: 0x0009C31C File Offset: 0x0009A51C
			public BuildingManager.BuildingData GetBuildingAt(Vector2Int coord)
			{
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					if (BuildingManager.GetOccupyingCoords(buildingData.Dimensions, buildingData.Rotation, buildingData.Coord).Contains(coord))
					{
						return buildingData;
					}
				}
				return null;
			}

			// Token: 0x060029F9 RID: 10745 RVA: 0x0009C390 File Offset: 0x0009A590
			public HashSet<Vector2Int> GetAllOccupiedCoords()
			{
				HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
				foreach (BuildingManager.BuildingData buildingData in this.buildings)
				{
					Vector2Int[] occupyingCoords = BuildingManager.GetOccupyingCoords(buildingData.Dimensions, buildingData.Rotation, buildingData.Coord);
					LinqExtensions.AddRange<Vector2Int>(hashSet, occupyingCoords);
				}
				return hashSet;
			}

			// Token: 0x060029FA RID: 10746 RVA: 0x0009C404 File Offset: 0x0009A604
			public bool Collide(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
			{
				HashSet<Vector2Int> allOccupiedCoords = this.GetAllOccupiedCoords();
				foreach (Vector2Int item in BuildingManager.GetOccupyingCoords(dimensions, rotation, coord))
				{
					if (allOccupiedCoords.Contains(item))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060029FB RID: 10747 RVA: 0x0009C443 File Offset: 0x0009A643
			internal bool Any(Func<BuildingManager.BuildingData, bool> predicate)
			{
				return this.buildings.Any(predicate);
			}

			// Token: 0x060029FC RID: 10748 RVA: 0x0009C451 File Offset: 0x0009A651
			public BuildingAreaData()
			{
			}

			// Token: 0x060029FD RID: 10749 RVA: 0x0009C464 File Offset: 0x0009A664
			public BuildingAreaData(string areaID)
			{
				this.areaID = areaID;
			}

			// Token: 0x04002135 RID: 8501
			[SerializeField]
			private string areaID;

			// Token: 0x04002136 RID: 8502
			[SerializeField]
			private List<BuildingManager.BuildingData> buildings = new List<BuildingManager.BuildingData>();
		}

		// Token: 0x020005CF RID: 1487
		[Serializable]
		public class BuildingData
		{
			// Token: 0x170007AD RID: 1965
			// (get) Token: 0x060029FE RID: 10750 RVA: 0x0009C47E File Offset: 0x0009A67E
			public int GUID
			{
				get
				{
					return this.guid;
				}
			}

			// Token: 0x170007AE RID: 1966
			// (get) Token: 0x060029FF RID: 10751 RVA: 0x0009C486 File Offset: 0x0009A686
			public string ID
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x170007AF RID: 1967
			// (get) Token: 0x06002A00 RID: 10752 RVA: 0x0009C490 File Offset: 0x0009A690
			public Vector2Int Dimensions
			{
				get
				{
					return this.Info.Dimensions;
				}
			}

			// Token: 0x170007B0 RID: 1968
			// (get) Token: 0x06002A01 RID: 10753 RVA: 0x0009C4AB File Offset: 0x0009A6AB
			public Vector2Int Coord
			{
				get
				{
					return this.coord;
				}
			}

			// Token: 0x170007B1 RID: 1969
			// (get) Token: 0x06002A02 RID: 10754 RVA: 0x0009C4B3 File Offset: 0x0009A6B3
			public BuildingRotation Rotation
			{
				get
				{
					return this.rotation;
				}
			}

			// Token: 0x170007B2 RID: 1970
			// (get) Token: 0x06002A03 RID: 10755 RVA: 0x0009C4BB File Offset: 0x0009A6BB
			public BuildingInfo Info
			{
				get
				{
					return BuildingDataCollection.GetInfo(this.id);
				}
			}

			// Token: 0x06002A04 RID: 10756 RVA: 0x0009C4C8 File Offset: 0x0009A6C8
			public BuildingData(int guid, string id, BuildingRotation rotation, Vector2Int coord)
			{
				this.guid = guid;
				this.id = id;
				this.coord = coord;
				this.rotation = rotation;
			}

			// Token: 0x06002A05 RID: 10757 RVA: 0x0009C4F0 File Offset: 0x0009A6F0
			internal Vector3 GetTransformPosition()
			{
				Vector2Int dimensions = this.Dimensions;
				if (this.rotation % BuildingRotation.Half > BuildingRotation.Zero)
				{
					dimensions..ctor(dimensions.y, dimensions.x);
				}
				return new Vector3((float)this.coord.x - 0.5f + (float)dimensions.x / 2f, 0f, (float)this.coord.y - 0.5f + (float)dimensions.y / 2f);
			}

			// Token: 0x04002137 RID: 8503
			[SerializeField]
			private int guid;

			// Token: 0x04002138 RID: 8504
			[SerializeField]
			private string id;

			// Token: 0x04002139 RID: 8505
			[SerializeField]
			private Vector2Int coord;

			// Token: 0x0400213A RID: 8506
			[SerializeField]
			private BuildingRotation rotation;
		}

		// Token: 0x020005D0 RID: 1488
		[Serializable]
		private struct SaveData
		{
			// Token: 0x0400213B RID: 8507
			[SerializeField]
			public List<BuildingManager.BuildingAreaData> data;

			// Token: 0x0400213C RID: 8508
			[SerializeField]
			public List<BuildingManager.BuildingTokenAmountEntry> tokenAmounts;
		}
	}
}
