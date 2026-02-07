using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Zip;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000232 RID: 562
	public class AstarSerializer
	{
		// Token: 0x06000D58 RID: 3416 RVA: 0x0005405C File Offset: 0x0005225C
		private static StringBuilder GetStringBuilder()
		{
			AstarSerializer._stringBuilder.Length = 0;
			return AstarSerializer._stringBuilder;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0005406E File Offset: 0x0005226E
		public AstarSerializer(AstarData data, GameObject contextRoot) : this(data, SerializeSettings.Settings, contextRoot)
		{
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0005407D File Offset: 0x0005227D
		public AstarSerializer(AstarData data, SerializeSettings settings, GameObject contextRoot)
		{
			this.data = data;
			this.contextRoot = contextRoot;
			this.settings = settings;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000540AC File Offset: 0x000522AC
		private void AddChecksum(byte[] bytes)
		{
			this.checksum = Checksum.GetChecksum(bytes, this.checksum);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x000540C0 File Offset: 0x000522C0
		private void AddEntry(string name, byte[] bytes)
		{
			this.zip.AddEntry(name, bytes);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x000540D0 File Offset: 0x000522D0
		public uint GetChecksum()
		{
			return this.checksum;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x000540D8 File Offset: 0x000522D8
		public void OpenSerialize()
		{
			this.zipStream = new MemoryStream();
			this.zip = new ZipFile();
			this.zip.AlternateEncoding = Encoding.UTF8;
			this.zip.AlternateEncodingUsage = ZipOption.Always;
			this.zip.ParallelDeflateThreshold = -1L;
			this.meta = new GraphMeta();
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00054130 File Offset: 0x00052330
		public byte[] CloseSerialize()
		{
			byte[] array = this.SerializeMeta();
			this.AddChecksum(array);
			this.AddEntry("meta.json", array);
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			foreach (ZipEntry zipEntry in this.zip.Entries)
			{
				zipEntry.AccessedTime = dateTime;
				zipEntry.CreationTime = dateTime;
				zipEntry.LastModified = dateTime;
				zipEntry.ModifiedTime = dateTime;
			}
			this.zip.Save(this.zipStream);
			this.zip.Dispose();
			array = this.zipStream.ToArray();
			this.zip = null;
			this.zipStream = null;
			return array;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000541F8 File Offset: 0x000523F8
		public void SerializeGraphs(NavGraph[] _graphs)
		{
			if (this.graphs != null)
			{
				throw new InvalidOperationException("Cannot serialize graphs multiple times.");
			}
			this.graphs = _graphs;
			if (this.zip == null)
			{
				throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
			}
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			this.persistentGraphs = new bool[this.graphs.Length];
			for (int i = 0; i < this.graphs.Length; i++)
			{
				this.persistentGraphs[i] = (this.graphs[i] != null && this.graphs[i].persistent);
				if (this.persistentGraphs[i])
				{
					byte[] bytes = this.Serialize(this.graphs[i]);
					this.AddChecksum(bytes);
					this.AddEntry("graph" + i.ToString() + ".json", bytes);
				}
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000542CC File Offset: 0x000524CC
		private byte[] SerializeMeta()
		{
			if (this.graphs == null)
			{
				throw new Exception("No call to SerializeGraphs has been done");
			}
			this.meta.version = AstarPath.Version;
			this.meta.graphs = this.graphs.Length;
			this.meta.guids = new List<string>();
			this.meta.typeNames = new List<string>();
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.persistentGraphs[i])
				{
					this.meta.guids.Add(this.graphs[i].guid.ToString());
					this.meta.typeNames.Add(this.graphs[i].GetType().FullName);
				}
				else
				{
					this.meta.guids.Add(null);
					this.meta.typeNames.Add(null);
				}
			}
			StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
			TinyJsonSerializer.Serialize(this.meta, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000543E0 File Offset: 0x000525E0
		public byte[] Serialize(NavGraph graph)
		{
			StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
			TinyJsonSerializer.Serialize(graph, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0005440C File Offset: 0x0005260C
		private static int GetMaxNodeIndexInAllGraphs(NavGraph[] graphs)
		{
			int maxIndex = 0;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null && graphs[i].persistent)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							maxIndex = Math.Max((int)node.NodeIndex, maxIndex);
							if (node.Destroyed)
							{
								Debug.LogError("Graph contains destroyed nodes. This is a bug.");
							}
						});
					}
					navGraph.GetNodes(action);
				}
			}
			return maxIndex;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00054470 File Offset: 0x00052670
		private static byte[] SerializeNodeIndices(NavGraph[] graphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			int maxNodeIndexInAllGraphs = AstarSerializer.GetMaxNodeIndexInAllGraphs(graphs);
			writer.Write(maxNodeIndexInAllGraphs);
			int maxNodeIndex2 = 0;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null && graphs[i].persistent)
				{
					NavGraph navGraph = graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							maxNodeIndex2 = Math.Max((int)node.NodeIndex, maxNodeIndex2);
							writer.Write(node.NodeIndex);
						});
					}
					navGraph.GetNodes(action);
				}
			}
			if (maxNodeIndex2 != maxNodeIndexInAllGraphs)
			{
				throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
			}
			byte[] result = memoryStream.ToArray();
			writer.Close();
			return result;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0005451C File Offset: 0x0005271C
		private static byte[] SerializeGraphExtraInfo(NavGraph graph, bool[] persistentGraphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter, persistentGraphs);
			((IGraphInternals)graph).SerializeExtraInfo(ctx);
			byte[] result = memoryStream.ToArray();
			binaryWriter.Close();
			return result;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00054550 File Offset: 0x00052750
		private static byte[] SerializeGraphNodeReferences(NavGraph graph, bool[] persistentGraphs)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryWriter, persistentGraphs);
			graph.GetNodes(delegate(GraphNode node)
			{
				node.SerializeReferences(ctx);
			});
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0005459C File Offset: 0x0005279C
		public void SerializeExtraInfo()
		{
			if (!this.settings.nodes)
			{
				return;
			}
			if (this.graphs == null)
			{
				throw new InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
			}
			byte[] bytes = AstarSerializer.SerializeNodeIndices(this.graphs);
			this.AddChecksum(bytes);
			this.AddEntry("graph_references.binary", bytes);
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null && this.graphs[i].persistent)
				{
					bytes = AstarSerializer.SerializeGraphExtraInfo(this.graphs[i], this.persistentGraphs);
					this.AddChecksum(bytes);
					this.AddEntry("graph" + i.ToString() + "_extra.binary", bytes);
					bytes = AstarSerializer.SerializeGraphNodeReferences(this.graphs[i], this.persistentGraphs);
					this.AddChecksum(bytes);
					this.AddEntry("graph" + i.ToString() + "_references.binary", bytes);
				}
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0005468B File Offset: 0x0005288B
		private ZipEntry GetEntry(string name)
		{
			return this.zip[name];
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00054699 File Offset: 0x00052899
		private bool ContainsEntry(string name)
		{
			return this.GetEntry(name) != null;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x000546A8 File Offset: 0x000528A8
		public bool OpenDeserialize(byte[] bytes)
		{
			this.zipStream = new MemoryStream();
			this.zipStream.Write(bytes, 0, bytes.Length);
			this.zipStream.Position = 0L;
			try
			{
				this.zip = ZipFile.Read(this.zipStream);
				this.zip.ParallelDeflateThreshold = -1L;
			}
			catch (Exception ex)
			{
				string str = "Caught exception when loading from zip\n";
				Exception ex2 = ex;
				Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
				this.zipStream.Dispose();
				return false;
			}
			if (this.ContainsEntry("meta.json"))
			{
				this.meta = this.DeserializeMeta(this.GetEntry("meta.json"));
			}
			else
			{
				if (!this.ContainsEntry("meta.binary"))
				{
					throw new Exception("No metadata found in serialized data.");
				}
				this.meta = this.DeserializeBinaryMeta(this.GetEntry("meta.binary"));
			}
			if (AstarSerializer.FullyDefinedVersion(this.meta.version) > AstarSerializer.FullyDefinedVersion(AstarPath.Version))
			{
				string[] array = new string[5];
				array[0] = "Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ";
				int num = 1;
				Version version = AstarPath.Version;
				array[num] = ((version != null) ? version.ToString() : null);
				array[2] = " Data version: ";
				int num2 = 3;
				Version version2 = this.meta.version;
				array[num2] = ((version2 != null) ? version2.ToString() : null);
				array[4] = "\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n";
				Debug.LogWarning(string.Concat(array));
			}
			return true;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00054810 File Offset: 0x00052A10
		private static Version FullyDefinedVersion(Version v)
		{
			return new Version(Mathf.Max(v.Major, 0), Mathf.Max(v.Minor, 0), Mathf.Max(v.Build, 0), Mathf.Max(v.Revision, 0));
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00054847 File Offset: 0x00052A47
		public void CloseDeserialize()
		{
			this.zipStream.Dispose();
			this.zip.Dispose();
			this.zip = null;
			this.zipStream = null;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00054870 File Offset: 0x00052A70
		private NavGraph DeserializeGraph(int zipIndex, int graphIndex, Type[] availableGraphTypes)
		{
			Type graphType = this.meta.GetGraphType(zipIndex, availableGraphTypes);
			if (object.Equals(graphType, null))
			{
				return null;
			}
			NavGraph navGraph = this.data.CreateGraph(graphType);
			navGraph.graphIndex = (uint)graphIndex;
			string name = "graph" + zipIndex.ToString() + ".json";
			if (!this.ContainsEntry(name))
			{
				throw new FileNotFoundException(string.Concat(new string[]
				{
					"Could not find data for graph ",
					zipIndex.ToString(),
					" in zip. Entry 'graph",
					zipIndex.ToString(),
					".json' does not exist"
				}));
			}
			TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(this.GetEntry(name)), graphType, navGraph, this.contextRoot);
			if (navGraph.guid.ToString() != this.meta.guids[zipIndex])
			{
				string str = "Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n";
				Pathfinding.Util.Guid guid = navGraph.guid;
				throw new Exception(str + guid.ToString() + " != " + this.meta.guids[zipIndex]);
			}
			return navGraph;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00054988 File Offset: 0x00052B88
		public NavGraph[] DeserializeGraphs(Type[] availableGraphTypes, bool allowLoadingNodes, Func<int> nextGraphIndex)
		{
			List<NavGraph> list = new List<NavGraph>();
			this.graphIndexInZip = new Dictionary<NavGraph, int>();
			for (int i = 0; i < this.meta.graphs; i++)
			{
				NavGraph navGraph = this.DeserializeGraph(i, nextGraphIndex(), availableGraphTypes);
				if (navGraph != null)
				{
					list.Add(navGraph);
					this.graphIndexInZip[navGraph] = i;
				}
			}
			this.graphs = list.ToArray();
			this.DeserializeEditorSettingsCompatibility();
			if (allowLoadingNodes)
			{
				this.DeserializeExtraInfo();
			}
			return this.graphs;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00054A04 File Offset: 0x00052C04
		private bool DeserializeExtraInfo(NavGraph graph)
		{
			ZipEntry entry = this.GetEntry("graph" + this.graphIndexInZip[graph].ToString() + "_extra.binary");
			if (entry == null)
			{
				return false;
			}
			GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), null, graph.graphIndex, this.meta);
			((IGraphInternals)graph).DeserializeExtraInfo(ctx);
			return true;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00054A64 File Offset: 0x00052C64
		private bool AnyDestroyedNodesInGraphs()
		{
			bool result = false;
			Action<GraphNode> <>9__0;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				NavGraph navGraph = this.graphs[i];
				Action<GraphNode> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(GraphNode node)
					{
						if (node.Destroyed)
						{
							result = true;
						}
					});
				}
				navGraph.GetNodes(action);
			}
			return result;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00054AC4 File Offset: 0x00052CC4
		private GraphNode[] DeserializeNodeReferenceMap()
		{
			ZipEntry entry = this.GetEntry("graph_references.binary");
			if (entry == null)
			{
				throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader reader = AstarSerializer.GetBinaryReader(entry);
			int num = reader.ReadInt32();
			GraphNode[] int2Node = new GraphNode[num + 1];
			try
			{
				Action<GraphNode> <>9__0;
				for (int i = 0; i < this.graphs.Length; i++)
				{
					NavGraph navGraph = this.graphs[i];
					Action<GraphNode> action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate(GraphNode node)
						{
							int num2 = reader.ReadInt32();
							int2Node[num2] = node;
						});
					}
					navGraph.GetNodes(action);
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", innerException);
			}
			if (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				throw new Exception((reader.BaseStream.Length / 4L).ToString() + " nodes were serialized, but only data for " + (reader.BaseStream.Position / 4L).ToString() + " nodes was found. The data looks corrupt.");
			}
			reader.Close();
			return int2Node;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00054BFC File Offset: 0x00052DFC
		private void DeserializeNodeReferences(NavGraph graph, GraphNode[] int2Node)
		{
			int num = this.graphIndexInZip[graph];
			ZipEntry entry = this.GetEntry("graph" + num.ToString() + "_references.binary");
			if (entry == null)
			{
				throw new Exception("Node references for graph " + num.ToString() + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			BinaryReader binaryReader = AstarSerializer.GetBinaryReader(entry);
			GraphSerializationContext ctx = new GraphSerializationContext(binaryReader, int2Node, graph.graphIndex, this.meta);
			graph.GetNodes(delegate(GraphNode node)
			{
				node.DeserializeReferences(ctx);
			});
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00054C88 File Offset: 0x00052E88
		private void DeserializeAndRemoveOldNodeLinks(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ctx.reader.ReadUInt64();
				GraphNode graphNode = ctx.DeserializeNodeReference();
				GraphNode graphNode2 = ctx.DeserializeNodeReference();
				GraphNode lhs = ctx.DeserializeNodeReference();
				GraphNode lhs2 = ctx.DeserializeNodeReference();
				ctx.DeserializeVector3();
				ctx.DeserializeVector3();
				ctx.reader.ReadBoolean();
				graphNode.ClearConnections(true);
				graphNode2.ClearConnections(true);
				graphNode.Walkable = false;
				graphNode2.Walkable = false;
				GraphNode.Disconnect(lhs, graphNode);
				GraphNode.Disconnect(lhs2, graphNode2);
			}
			bool flag = false;
			int num2 = 0;
			while (num2 < this.graphs.Length && !flag)
			{
				if (this.graphs[num2] != null)
				{
					PointGraph pointGraph = this.graphs[num2] as PointGraph;
					if (pointGraph != null)
					{
						bool anyWalkable = false;
						int count2 = 0;
						pointGraph.GetNodes(delegate(GraphNode node)
						{
							anyWalkable |= node.Walkable;
							int count = count2;
							count2 = count + 1;
						});
						if (!anyWalkable && pointGraph.root == null && 2 * num == count2 && (count2 > 0 || pointGraph.name.Contains("used for node links")))
						{
							((IGraphInternals)this.graphs[num2]).DestroyAllNodes();
							List<NavGraph> list = new List<NavGraph>(this.graphs);
							list.RemoveAt(num2);
							this.graphs = list.ToArray();
							flag = true;
						}
						if (pointGraph.name == "PointGraph (used for node links)")
						{
							pointGraph.name = "PointGraph";
						}
					}
				}
				num2++;
			}
			if (!flag && num > 0)
			{
				Debug.LogWarning("Old off-mesh links were present in the serialized graph data. Not everything could be cleaned up properly. It is recommended that you re-scan all graphs and save the cache or graph file again. An attempt to migrate the old links was made, but a stray point graph may have been left behind.");
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00054E34 File Offset: 0x00053034
		private void DeserializeExtraInfo()
		{
			bool flag = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				flag |= this.DeserializeExtraInfo(this.graphs[i]);
			}
			if (!flag)
			{
				return;
			}
			if (this.AnyDestroyedNodesInGraphs())
			{
				Debug.LogError("Graph contains destroyed nodes. This is a bug.");
			}
			GraphNode[] array = this.DeserializeNodeReferenceMap();
			for (int j = 0; j < this.graphs.Length; j++)
			{
				this.DeserializeNodeReferences(this.graphs[j], array);
			}
			if (this.meta.version < AstarSerializer.V4_3_85)
			{
				ZipEntry entry = this.GetEntry("node_link2.binary");
				if (entry != null)
				{
					GraphSerializationContext ctx = new GraphSerializationContext(AstarSerializer.GetBinaryReader(entry), array, 0U, this.meta);
					this.DeserializeAndRemoveOldNodeLinks(ctx);
				}
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00054EEC File Offset: 0x000530EC
		public void PostDeserialization()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				GraphSerializationContext ctx = new GraphSerializationContext(null, null, 0U, this.meta);
				((IGraphInternals)this.graphs[i]).PostDeserialization(ctx);
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00054F2C File Offset: 0x0005312C
		private void DeserializeEditorSettingsCompatibility()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				ZipEntry entry = this.GetEntry("graph" + this.graphIndexInZip[this.graphs[i]].ToString() + "_editor.json");
				if (entry != null)
				{
					((IGraphInternals)this.graphs[i]).SerializedEditorSettings = AstarSerializer.GetString(entry);
				}
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00054F94 File Offset: 0x00053194
		private static BinaryReader GetBinaryReader(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			return new BinaryReader(memoryStream);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00054FBC File Offset: 0x000531BC
		private static string GetString(ZipEntry entry)
		{
			MemoryStream memoryStream = new MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			streamReader.Dispose();
			return result;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00054FF1 File Offset: 0x000531F1
		private GraphMeta DeserializeMeta(ZipEntry entry)
		{
			return TinyJsonDeserializer.Deserialize(AstarSerializer.GetString(entry), typeof(GraphMeta), null, null) as GraphMeta;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00055010 File Offset: 0x00053210
		private GraphMeta DeserializeBinaryMeta(ZipEntry entry)
		{
			GraphMeta graphMeta = new GraphMeta();
			BinaryReader binaryReader = AstarSerializer.GetBinaryReader(entry);
			if (binaryReader.ReadString() != "A*")
			{
				throw new Exception("Invalid magic number in saved data");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			int num3 = binaryReader.ReadInt32();
			int num4 = binaryReader.ReadInt32();
			if (num < 0)
			{
				graphMeta.version = new Version(0, 0);
			}
			else if (num2 < 0)
			{
				graphMeta.version = new Version(num, 0);
			}
			else if (num3 < 0)
			{
				graphMeta.version = new Version(num, num2);
			}
			else if (num4 < 0)
			{
				graphMeta.version = new Version(num, num2, num3);
			}
			else
			{
				graphMeta.version = new Version(num, num2, num3, num4);
			}
			graphMeta.graphs = binaryReader.ReadInt32();
			graphMeta.guids = new List<string>();
			int num5 = binaryReader.ReadInt32();
			for (int i = 0; i < num5; i++)
			{
				graphMeta.guids.Add(binaryReader.ReadString());
			}
			graphMeta.typeNames = new List<string>();
			num5 = binaryReader.ReadInt32();
			for (int j = 0; j < num5; j++)
			{
				graphMeta.typeNames.Add(binaryReader.ReadString());
			}
			binaryReader.Close();
			return graphMeta;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00055144 File Offset: 0x00053344
		public static void SaveToFile(string path, byte[] data)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				fileStream.Write(data, 0, data.Length);
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00055180 File Offset: 0x00053380
		public static byte[] LoadFromFile(string path)
		{
			byte[] result;
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				byte[] array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				result = array;
			}
			return result;
		}

		// Token: 0x04000A4C RID: 2636
		private AstarData data;

		// Token: 0x04000A4D RID: 2637
		private ZipFile zip;

		// Token: 0x04000A4E RID: 2638
		private MemoryStream zipStream;

		// Token: 0x04000A4F RID: 2639
		private GraphMeta meta;

		// Token: 0x04000A50 RID: 2640
		private SerializeSettings settings;

		// Token: 0x04000A51 RID: 2641
		private GameObject contextRoot;

		// Token: 0x04000A52 RID: 2642
		private NavGraph[] graphs;

		// Token: 0x04000A53 RID: 2643
		private bool[] persistentGraphs;

		// Token: 0x04000A54 RID: 2644
		private Dictionary<NavGraph, int> graphIndexInZip;

		// Token: 0x04000A55 RID: 2645
		private const string binaryExt = ".binary";

		// Token: 0x04000A56 RID: 2646
		private const string jsonExt = ".json";

		// Token: 0x04000A57 RID: 2647
		private uint checksum = uint.MaxValue;

		// Token: 0x04000A58 RID: 2648
		private UTF8Encoding encoding = new UTF8Encoding();

		// Token: 0x04000A59 RID: 2649
		private static StringBuilder _stringBuilder = new StringBuilder();

		// Token: 0x04000A5A RID: 2650
		public static readonly Version V3_8_3 = new Version(3, 8, 3);

		// Token: 0x04000A5B RID: 2651
		public static readonly Version V3_9_0 = new Version(3, 9, 0);

		// Token: 0x04000A5C RID: 2652
		public static readonly Version V4_1_0 = new Version(4, 1, 0);

		// Token: 0x04000A5D RID: 2653
		public static readonly Version V4_3_2 = new Version(4, 3, 2);

		// Token: 0x04000A5E RID: 2654
		public static readonly Version V4_3_6 = new Version(4, 3, 6);

		// Token: 0x04000A5F RID: 2655
		public static readonly Version V4_3_37 = new Version(4, 3, 37);

		// Token: 0x04000A60 RID: 2656
		public static readonly Version V4_3_12 = new Version(4, 3, 12);

		// Token: 0x04000A61 RID: 2657
		public static readonly Version V4_3_68 = new Version(4, 3, 68);

		// Token: 0x04000A62 RID: 2658
		public static readonly Version V4_3_74 = new Version(4, 3, 74);

		// Token: 0x04000A63 RID: 2659
		public static readonly Version V4_3_80 = new Version(4, 3, 80);

		// Token: 0x04000A64 RID: 2660
		public static readonly Version V4_3_83 = new Version(4, 3, 83);

		// Token: 0x04000A65 RID: 2661
		public static readonly Version V4_3_85 = new Version(4, 3, 85);

		// Token: 0x04000A66 RID: 2662
		public static readonly Version V4_3_87 = new Version(4, 3, 87);

		// Token: 0x04000A67 RID: 2663
		public static readonly Version V5_1_0 = new Version(5, 1, 0);

		// Token: 0x04000A68 RID: 2664
		public static readonly Version V5_2_0 = new Version(5, 2, 0);
	}
}
