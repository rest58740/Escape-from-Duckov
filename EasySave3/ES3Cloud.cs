using System;
using System.Collections;
using System.Text;
using ES3Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000014 RID: 20
[IncludeInSettings(true)]
public class ES3Cloud : ES3WebClass
{
	// Token: 0x06000152 RID: 338 RVA: 0x00005C24 File Offset: 0x00003E24
	public ES3Cloud(string url, string apiKey) : base(url, apiKey)
	{
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00005C41 File Offset: 0x00003E41
	public ES3Cloud(string url, string apiKey, int timeout) : base(url, apiKey)
	{
		this.timeout = timeout;
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000154 RID: 340 RVA: 0x00005C65 File Offset: 0x00003E65
	public byte[] data
	{
		get
		{
			return this._data;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000155 RID: 341 RVA: 0x00005C6D File Offset: 0x00003E6D
	public string text
	{
		get
		{
			if (this.data == null)
			{
				return null;
			}
			return this.encoding.GetString(this.data);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000156 RID: 342 RVA: 0x00005C8A File Offset: 0x00003E8A
	public string[] filenames
	{
		get
		{
			if (this.data == null || this.data.Length == 0)
			{
				return new string[0];
			}
			return this.text.Split(new char[]
			{
				';'
			}, StringSplitOptions.RemoveEmptyEntries);
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000157 RID: 343 RVA: 0x00005CBC File Offset: 0x00003EBC
	public DateTime timestamp
	{
		get
		{
			if (this.data == null || this.data.Length == 0)
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			}
			double value;
			if (!double.TryParse(this.text, out value))
			{
				throw new FormatException("Could not convert downloaded data to a timestamp. Data downloaded was: " + this.text);
			}
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00005D2B File Offset: 0x00003F2B
	public IEnumerator Sync()
	{
		return this.Sync(new ES3Settings(null, null), "", "");
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00005D44 File Offset: 0x00003F44
	public IEnumerator Sync(string filePath)
	{
		return this.Sync(new ES3Settings(filePath, null), "", "");
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00005D5D File Offset: 0x00003F5D
	public IEnumerator Sync(string filePath, string user)
	{
		return this.Sync(new ES3Settings(filePath, null), user, "");
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00005D72 File Offset: 0x00003F72
	public IEnumerator Sync(string filePath, string user, string password)
	{
		return this.Sync(new ES3Settings(filePath, null), user, password);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00005D83 File Offset: 0x00003F83
	public IEnumerator Sync(string filePath, ES3Settings settings)
	{
		return this.Sync(new ES3Settings(filePath, settings), "", "");
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00005D9C File Offset: 0x00003F9C
	public IEnumerator Sync(string filePath, string user, ES3Settings settings)
	{
		return this.Sync(new ES3Settings(filePath, settings), user, "");
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00005DB1 File Offset: 0x00003FB1
	public IEnumerator Sync(string filePath, string user, string password, ES3Settings settings)
	{
		return this.Sync(new ES3Settings(filePath, settings), user, password);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00005DC3 File Offset: 0x00003FC3
	private IEnumerator Sync(ES3Settings settings, string user, string password)
	{
		this.Reset();
		yield return this.DownloadFile(settings, user, password, this.GetFileTimestamp(settings));
		if (this.errorCode == 3L)
		{
			this.Reset();
			if (ES3.FileExists(settings))
			{
				yield return this.UploadFile(settings, user, password);
			}
		}
		this.isDone = true;
		yield break;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00005DE7 File Offset: 0x00003FE7
	public IEnumerator UploadFile()
	{
		return this.UploadFile(new ES3Settings(null, null), "", "");
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00005E00 File Offset: 0x00004000
	public IEnumerator UploadFile(string filePath)
	{
		return this.UploadFile(new ES3Settings(filePath, null), "", "");
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00005E19 File Offset: 0x00004019
	public IEnumerator UploadFile(string filePath, string user)
	{
		return this.UploadFile(new ES3Settings(filePath, null), user, "");
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00005E2E File Offset: 0x0000402E
	public IEnumerator UploadFile(string filePath, string user, string password)
	{
		return this.UploadFile(new ES3Settings(filePath, null), user, password);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00005E3F File Offset: 0x0000403F
	public IEnumerator UploadFile(string filePath, ES3Settings settings)
	{
		return this.UploadFile(new ES3Settings(filePath, settings), "", "");
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00005E58 File Offset: 0x00004058
	public IEnumerator UploadFile(string filePath, string user, ES3Settings settings)
	{
		return this.UploadFile(new ES3Settings(filePath, settings), user, "");
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00005E6D File Offset: 0x0000406D
	public IEnumerator UploadFile(string filePath, string user, string password, ES3Settings settings)
	{
		return this.UploadFile(new ES3Settings(filePath, settings), user, password);
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00005E7F File Offset: 0x0000407F
	public IEnumerator UploadFile(ES3File es3File)
	{
		return this.UploadFile(es3File.GetBytes(null), es3File.settings, "", "", this.DateTimeToUnixTimestamp(DateTime.Now));
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00005EA9 File Offset: 0x000040A9
	public IEnumerator UploadFile(ES3File es3File, string user)
	{
		return this.UploadFile(es3File.GetBytes(null), es3File.settings, user, "", this.DateTimeToUnixTimestamp(DateTime.Now));
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00005ECF File Offset: 0x000040CF
	public IEnumerator UploadFile(ES3File es3File, string user, string password)
	{
		return this.UploadFile(es3File.GetBytes(null), es3File.settings, user, password, this.DateTimeToUnixTimestamp(DateTime.Now));
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00005EF1 File Offset: 0x000040F1
	public IEnumerator UploadFile(ES3Settings settings, string user, string password)
	{
		return this.UploadFile(ES3.LoadRawBytes(settings), settings, user, password);
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00005F02 File Offset: 0x00004102
	public IEnumerator UploadFile(byte[] bytes, ES3Settings settings, string user, string password)
	{
		return this.UploadFile(bytes, settings, user, password, this.DateTimeToUnixTimestamp(ES3.GetTimestamp(settings)));
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00005F1B File Offset: 0x0000411B
	private IEnumerator UploadFile(byte[] bytes, ES3Settings settings, string user, string password, long fileTimestamp)
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("putFile", settings.path);
		wwwform.AddField("timestamp", fileTimestamp.ToString());
		wwwform.AddField("user", base.GetUser(user, password));
		wwwform.AddBinaryData("data", bytes, "data.dat", "multipart/form-data");
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			base.HandleError(webRequest, true);
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00005F4F File Offset: 0x0000414F
	public IEnumerator DownloadFile()
	{
		return this.DownloadFile(new ES3Settings(null, null), "", "", 0L);
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00005F6A File Offset: 0x0000416A
	public IEnumerator DownloadFile(string filePath)
	{
		return this.DownloadFile(new ES3Settings(filePath, null), "", "", 0L);
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00005F85 File Offset: 0x00004185
	public IEnumerator DownloadFile(string filePath, string user)
	{
		return this.DownloadFile(new ES3Settings(filePath, null), user, "", 0L);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00005F9C File Offset: 0x0000419C
	public IEnumerator DownloadFile(string filePath, string user, string password)
	{
		return this.DownloadFile(new ES3Settings(filePath, null), user, password, 0L);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00005FAF File Offset: 0x000041AF
	public IEnumerator DownloadFile(string filePath, ES3Settings settings)
	{
		return this.DownloadFile(new ES3Settings(filePath, settings), "", "", 0L);
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00005FCA File Offset: 0x000041CA
	public IEnumerator DownloadFile(string filePath, string user, ES3Settings settings)
	{
		return this.DownloadFile(new ES3Settings(filePath, settings), user, "", 0L);
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00005FE1 File Offset: 0x000041E1
	public IEnumerator DownloadFile(string filePath, string user, string password, ES3Settings settings)
	{
		return this.DownloadFile(new ES3Settings(filePath, settings), user, password, 0L);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00005FF5 File Offset: 0x000041F5
	public IEnumerator DownloadFile(ES3File es3File)
	{
		return this.DownloadFile(es3File, "", "", 0L);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000600A File Offset: 0x0000420A
	public IEnumerator DownloadFile(ES3File es3File, string user)
	{
		return this.DownloadFile(es3File, user, "", 0L);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000601B File Offset: 0x0000421B
	public IEnumerator DownloadFile(ES3File es3File, string user, string password)
	{
		return this.DownloadFile(es3File, user, password, 0L);
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00006028 File Offset: 0x00004228
	private IEnumerator DownloadFile(ES3File es3File, string user, string password, long timestamp)
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("getFile", es3File.settings.path);
		wwwform.AddField("user", base.GetUser(user, password));
		if (timestamp > 0L)
		{
			wwwform.AddField("timestamp", timestamp.ToString());
		}
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			if (!base.HandleError(webRequest, false))
			{
				if (webRequest.downloadedBytes > 0UL)
				{
					es3File.Clear();
					es3File.SaveRaw(webRequest.downloadHandler.data, null);
				}
				else
				{
					this.error = string.Format("File {0} was not found on the server.", es3File.settings.path);
					this.errorCode = 3L;
				}
			}
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00006054 File Offset: 0x00004254
	private IEnumerator DownloadFile(ES3Settings settings, string user, string password, long timestamp)
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("getFile", settings.path);
		wwwform.AddField("user", base.GetUser(user, password));
		if (timestamp > 0L)
		{
			wwwform.AddField("timestamp", timestamp.ToString());
		}
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			if (!base.HandleError(webRequest, false))
			{
				if (webRequest.downloadedBytes > 0UL)
				{
					ES3.SaveRaw(webRequest.downloadHandler.data, settings);
				}
				else
				{
					this.error = string.Format("File {0} was not found on the server.", settings.path);
					this.errorCode = 3L;
				}
			}
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00006080 File Offset: 0x00004280
	public IEnumerator DeleteFile()
	{
		return this.DeleteFile(new ES3Settings(null, null), "", "");
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00006099 File Offset: 0x00004299
	public IEnumerator DeleteFile(string filePath)
	{
		return this.DeleteFile(new ES3Settings(filePath, null), "", "");
	}

	// Token: 0x0600017B RID: 379 RVA: 0x000060B2 File Offset: 0x000042B2
	public IEnumerator DeleteFile(string filePath, string user)
	{
		return this.DeleteFile(new ES3Settings(filePath, null), user, "");
	}

	// Token: 0x0600017C RID: 380 RVA: 0x000060C7 File Offset: 0x000042C7
	public IEnumerator DeleteFile(string filePath, string user, string password)
	{
		return this.DeleteFile(new ES3Settings(filePath, null), user, password);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x000060D8 File Offset: 0x000042D8
	public IEnumerator DeleteFile(string filePath, ES3Settings settings)
	{
		return this.DeleteFile(new ES3Settings(filePath, settings), "", "");
	}

	// Token: 0x0600017E RID: 382 RVA: 0x000060F1 File Offset: 0x000042F1
	public IEnumerator DeleteFile(string filePath, string user, ES3Settings settings)
	{
		return this.DeleteFile(new ES3Settings(filePath, settings), user, "");
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00006106 File Offset: 0x00004306
	public IEnumerator DeleteFile(string filePath, string user, string password, ES3Settings settings)
	{
		return this.DeleteFile(new ES3Settings(filePath, settings), user, password);
	}

	// Token: 0x06000180 RID: 384 RVA: 0x00006118 File Offset: 0x00004318
	private IEnumerator DeleteFile(ES3Settings settings, string user, string password)
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("deleteFile", settings.path);
		wwwform.AddField("user", base.GetUser(user, password));
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			base.HandleError(webRequest, true);
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000613C File Offset: 0x0000433C
	public IEnumerator RenameFile(string filePath, string newFilePath)
	{
		return this.RenameFile(new ES3Settings(filePath, null), new ES3Settings(newFilePath, null), "", "");
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000615C File Offset: 0x0000435C
	public IEnumerator RenameFile(string filePath, string newFilePath, string user)
	{
		return this.RenameFile(new ES3Settings(filePath, null), new ES3Settings(newFilePath, null), user, "");
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00006178 File Offset: 0x00004378
	public IEnumerator RenameFile(string filePath, string newFilePath, string user, string password)
	{
		return this.RenameFile(new ES3Settings(filePath, null), new ES3Settings(newFilePath, null), user, password);
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00006191 File Offset: 0x00004391
	public IEnumerator RenameFile(string filePath, string newFilePath, ES3Settings settings)
	{
		return this.RenameFile(new ES3Settings(filePath, settings), new ES3Settings(newFilePath, settings), "", "");
	}

	// Token: 0x06000185 RID: 389 RVA: 0x000061B1 File Offset: 0x000043B1
	public IEnumerator RenameFile(string filePath, string newFilePath, string user, ES3Settings settings)
	{
		return this.RenameFile(new ES3Settings(filePath, settings), new ES3Settings(newFilePath, settings), user, "");
	}

	// Token: 0x06000186 RID: 390 RVA: 0x000061CF File Offset: 0x000043CF
	public IEnumerator RenameFile(string filePath, string newFilePath, string user, string password, ES3Settings settings)
	{
		return this.RenameFile(new ES3Settings(filePath, settings), new ES3Settings(newFilePath, settings), user, password);
	}

	// Token: 0x06000187 RID: 391 RVA: 0x000061EA File Offset: 0x000043EA
	private IEnumerator RenameFile(ES3Settings settings, ES3Settings newSettings, string user, string password)
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("renameFile", settings.path);
		wwwform.AddField("newFilename", newSettings.path);
		wwwform.AddField("user", base.GetUser(user, password));
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			base.HandleError(webRequest, true);
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x00006216 File Offset: 0x00004416
	public IEnumerator DownloadFilenames(string user = "", string password = "")
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("getFilenames", "");
		wwwform.AddField("user", base.GetUser(user, password));
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			if (!base.HandleError(webRequest, false))
			{
				this._data = webRequest.downloadHandler.data;
			}
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00006233 File Offset: 0x00004433
	public IEnumerator SearchFilenames(string searchPattern, string user = "", string password = "")
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("getFilenames", "");
		wwwform.AddField("user", base.GetUser(user, password));
		if (!string.IsNullOrEmpty(searchPattern))
		{
			wwwform.AddField("pattern", searchPattern);
		}
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			if (!base.HandleError(webRequest, false))
			{
				this._data = webRequest.downloadHandler.data;
			}
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00006257 File Offset: 0x00004457
	public IEnumerator DownloadTimestamp()
	{
		return this.DownloadTimestamp(new ES3Settings(null, null), "", "");
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00006270 File Offset: 0x00004470
	public IEnumerator DownloadTimestamp(string filePath)
	{
		return this.DownloadTimestamp(new ES3Settings(filePath, null), "", "");
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00006289 File Offset: 0x00004489
	public IEnumerator DownloadTimestamp(string filePath, string user)
	{
		return this.DownloadTimestamp(new ES3Settings(filePath, null), user, "");
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000629E File Offset: 0x0000449E
	public IEnumerator DownloadTimestamp(string filePath, string user, string password)
	{
		return this.DownloadTimestamp(new ES3Settings(filePath, null), user, password);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x000062AF File Offset: 0x000044AF
	public IEnumerator DownloadTimestamp(string filePath, ES3Settings settings)
	{
		return this.DownloadTimestamp(new ES3Settings(filePath, settings), "", "");
	}

	// Token: 0x0600018F RID: 399 RVA: 0x000062C8 File Offset: 0x000044C8
	public IEnumerator DownloadTimestamp(string filePath, string user, ES3Settings settings)
	{
		return this.DownloadTimestamp(new ES3Settings(filePath, settings), user, "");
	}

	// Token: 0x06000190 RID: 400 RVA: 0x000062DD File Offset: 0x000044DD
	public IEnumerator DownloadTimestamp(string filePath, string user, string password, ES3Settings settings)
	{
		return this.DownloadTimestamp(new ES3Settings(filePath, settings), user, password);
	}

	// Token: 0x06000191 RID: 401 RVA: 0x000062EF File Offset: 0x000044EF
	private IEnumerator DownloadTimestamp(ES3Settings settings, string user, string password)
	{
		this.Reset();
		WWWForm wwwform = base.CreateWWWForm();
		wwwform.AddField("apiKey", this.apiKey);
		wwwform.AddField("getTimestamp", settings.path);
		wwwform.AddField("user", base.GetUser(user, password));
		using (UnityWebRequest webRequest = UnityWebRequest.Post(this.url, wwwform))
		{
			webRequest.timeout = this.timeout;
			yield return base.SendWebRequest(webRequest);
			if (!base.HandleError(webRequest, false))
			{
				this._data = webRequest.downloadHandler.data;
			}
		}
		UnityWebRequest webRequest = null;
		this.isDone = true;
		yield break;
		yield break;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00006314 File Offset: 0x00004514
	private long DateTimeToUnixTimestamp(DateTime dt)
	{
		return Convert.ToInt64((dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000634B File Offset: 0x0000454B
	private long GetFileTimestamp(ES3Settings settings)
	{
		return this.DateTimeToUnixTimestamp(ES3.GetTimestamp(settings));
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00006359 File Offset: 0x00004559
	protected override void Reset()
	{
		this._data = null;
		base.Reset();
	}

	// Token: 0x0400004A RID: 74
	private int timeout = 20;

	// Token: 0x0400004B RID: 75
	public Encoding encoding = Encoding.UTF8;

	// Token: 0x0400004C RID: 76
	private byte[] _data;
}
