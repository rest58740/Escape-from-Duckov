using System;
using System.ComponentModel;

namespace ES3Internal
{
	// Token: 0x020000E7 RID: 231
	internal class ES3CacheWriter : ES3Writer
	{
		// Token: 0x060004EC RID: 1260 RVA: 0x0001E7D1 File Offset: 0x0001C9D1
		internal ES3CacheWriter(ES3Settings settings, bool writeHeaderAndFooter, bool mergeKeys) : base(settings, writeHeaderAndFooter, mergeKeys)
		{
			this.es3File = new ES3File(settings);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
		public override void Write<T>(string key, object value)
		{
			this.es3File.Save<T>(key, (T)((object)value));
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		internal override void Write(string key, Type type, byte[] value)
		{
			ES3Debug.LogError("Not implemented", null, 0);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001E80A File Offset: 0x0001CA0A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Write(Type type, string key, object value)
		{
			this.es3File.Save<object>(key, value);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001E819 File Offset: 0x0001CA19
		internal override void WritePrimitive(int value)
		{
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001E81B File Offset: 0x0001CA1B
		internal override void WritePrimitive(float value)
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001E81D File Offset: 0x0001CA1D
		internal override void WritePrimitive(bool value)
		{
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001E81F File Offset: 0x0001CA1F
		internal override void WritePrimitive(decimal value)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001E821 File Offset: 0x0001CA21
		internal override void WritePrimitive(double value)
		{
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001E823 File Offset: 0x0001CA23
		internal override void WritePrimitive(long value)
		{
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001E825 File Offset: 0x0001CA25
		internal override void WritePrimitive(ulong value)
		{
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001E827 File Offset: 0x0001CA27
		internal override void WritePrimitive(uint value)
		{
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001E829 File Offset: 0x0001CA29
		internal override void WritePrimitive(byte value)
		{
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001E82B File Offset: 0x0001CA2B
		internal override void WritePrimitive(sbyte value)
		{
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001E82D File Offset: 0x0001CA2D
		internal override void WritePrimitive(short value)
		{
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001E82F File Offset: 0x0001CA2F
		internal override void WritePrimitive(ushort value)
		{
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001E831 File Offset: 0x0001CA31
		internal override void WritePrimitive(char value)
		{
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001E833 File Offset: 0x0001CA33
		internal override void WritePrimitive(byte[] value)
		{
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001E835 File Offset: 0x0001CA35
		internal override void WritePrimitive(string value)
		{
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001E837 File Offset: 0x0001CA37
		internal override void WriteNull()
		{
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001E839 File Offset: 0x0001CA39
		private static bool CharacterRequiresEscaping(char c)
		{
			return false;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001E83C File Offset: 0x0001CA3C
		private void WriteCommaIfRequired()
		{
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001E83E File Offset: 0x0001CA3E
		internal override void WriteRawProperty(string name, byte[] value)
		{
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001E840 File Offset: 0x0001CA40
		internal override void StartWriteFile()
		{
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001E842 File Offset: 0x0001CA42
		internal override void EndWriteFile()
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001E844 File Offset: 0x0001CA44
		internal override void StartWriteProperty(string name)
		{
			base.StartWriteProperty(name);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001E84D File Offset: 0x0001CA4D
		internal override void EndWriteProperty(string name)
		{
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001E84F File Offset: 0x0001CA4F
		internal override void StartWriteObject(string name)
		{
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001E851 File Offset: 0x0001CA51
		internal override void EndWriteObject(string name)
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001E853 File Offset: 0x0001CA53
		internal override void StartWriteCollection()
		{
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001E855 File Offset: 0x0001CA55
		internal override void EndWriteCollection()
		{
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001E857 File Offset: 0x0001CA57
		internal override void StartWriteCollectionItem(int index)
		{
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001E859 File Offset: 0x0001CA59
		internal override void EndWriteCollectionItem(int index)
		{
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001E85B File Offset: 0x0001CA5B
		internal override void StartWriteDictionary()
		{
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001E85D File Offset: 0x0001CA5D
		internal override void EndWriteDictionary()
		{
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001E85F File Offset: 0x0001CA5F
		internal override void StartWriteDictionaryKey(int index)
		{
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001E861 File Offset: 0x0001CA61
		internal override void EndWriteDictionaryKey(int index)
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001E863 File Offset: 0x0001CA63
		internal override void StartWriteDictionaryValue(int index)
		{
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001E865 File Offset: 0x0001CA65
		internal override void EndWriteDictionaryValue(int index)
		{
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001E867 File Offset: 0x0001CA67
		public override void Dispose()
		{
		}

		// Token: 0x04000172 RID: 370
		private ES3File es3File;
	}
}
