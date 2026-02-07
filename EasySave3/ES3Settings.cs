using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using ES3Internal;
using Unity.VisualScripting;
using UnityEngine;

// Token: 0x02000010 RID: 16
[IncludeInSettings(true)]
public class ES3Settings : ICloneable
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600013B RID: 315 RVA: 0x0000564A File Offset: 0x0000384A
	public static ES3Defaults defaultSettingsScriptableObject
	{
		get
		{
			if (ES3Settings._defaultSettingsScriptableObject == null)
			{
				ES3Settings._defaultSettingsScriptableObject = Resources.Load<ES3Defaults>("ES3/ES3Defaults");
			}
			return ES3Settings._defaultSettingsScriptableObject;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600013C RID: 316 RVA: 0x0000566D File Offset: 0x0000386D
	public static ES3Settings defaultSettings
	{
		get
		{
			if (ES3Settings._defaults == null && ES3Settings.defaultSettingsScriptableObject != null)
			{
				ES3Settings._defaults = ES3Settings.defaultSettingsScriptableObject.settings;
			}
			return ES3Settings._defaults;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600013D RID: 317 RVA: 0x00005697 File Offset: 0x00003897
	internal static ES3Settings unencryptedUncompressedSettings
	{
		get
		{
			if (ES3Settings._unencryptedUncompressedSettings == null)
			{
				ES3Settings._unencryptedUncompressedSettings = new ES3Settings(new Enum[]
				{
					ES3.EncryptionType.None,
					ES3.CompressionType.None
				});
			}
			return ES3Settings._unencryptedUncompressedSettings;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600013E RID: 318 RVA: 0x000056C7 File Offset: 0x000038C7
	// (set) Token: 0x0600013F RID: 319 RVA: 0x000056EB File Offset: 0x000038EB
	public ES3.Location location
	{
		get
		{
			if (this._location == ES3.Location.File && (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.tvOS))
			{
				return ES3.Location.PlayerPrefs;
			}
			return this._location;
		}
		set
		{
			this._location = value;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000140 RID: 320 RVA: 0x000056F4 File Offset: 0x000038F4
	public string FullPath
	{
		get
		{
			if (this.path == null)
			{
				throw new NullReferenceException("The 'path' field of this ES3Settings is null, indicating that it was not possible to load the default settings from Resources. Please check that the ES3 Default Settings.prefab exists in Assets/Plugins/Resources/ES3/");
			}
			if (ES3Settings.IsAbsolute(this.path))
			{
				return this.path;
			}
			if (this.location == ES3.Location.File)
			{
				if (this.directory == ES3.Directory.PersistentDataPath)
				{
					return ES3IO.persistentDataPath + "/" + this.path;
				}
				if (this.directory == ES3.Directory.DataPath)
				{
					return Application.dataPath + "/" + this.path;
				}
				throw new NotImplementedException("File directory \"" + this.directory.ToString() + "\" has not been implemented.");
			}
			else
			{
				if (this.location != ES3.Location.Resources)
				{
					return this.path;
				}
				string extension = Path.GetExtension(this.path);
				bool flag = false;
				foreach (string b in ES3Settings.resourcesExtensions)
				{
					if (extension == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					throw new ArgumentException("Extension of file in Resources must be .json, .bytes, .txt, .csv, .htm, .html, .xml, .yaml or .fnt, but path given was \"" + this.path + "\"");
				}
				return this.path.Replace(extension, "");
			}
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00005807 File Offset: 0x00003A07
	public ES3Settings(string path = null, ES3Settings settings = null) : this(true)
	{
		if (settings != null)
		{
			settings.CopyInto(this);
		}
		if (path != null)
		{
			this.path = path;
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00005824 File Offset: 0x00003A24
	public ES3Settings(string path, params Enum[] enums) : this(enums)
	{
		if (path != null)
		{
			this.path = path;
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00005838 File Offset: 0x00003A38
	public ES3Settings(params Enum[] enums) : this(true)
	{
		foreach (Enum @enum in enums)
		{
			if (@enum is ES3.EncryptionType)
			{
				this.encryptionType = (ES3.EncryptionType)@enum;
			}
			else if (@enum is ES3.Location)
			{
				this.location = (ES3.Location)@enum;
			}
			else if (@enum is ES3.CompressionType)
			{
				this.compressionType = (ES3.CompressionType)@enum;
			}
			else if (@enum is ES3.ReferenceMode)
			{
				this.referenceMode = (ES3.ReferenceMode)@enum;
			}
			else if (@enum is ES3.Format)
			{
				this.format = (ES3.Format)@enum;
			}
			else if (@enum is ES3.Directory)
			{
				this.directory = (ES3.Directory)@enum;
			}
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x000058E8 File Offset: 0x00003AE8
	public ES3Settings(ES3.EncryptionType encryptionType, string encryptionPassword) : this(true)
	{
		this.encryptionType = encryptionType;
		this.encryptionPassword = encryptionPassword;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x000058FF File Offset: 0x00003AFF
	public ES3Settings(string path, ES3.EncryptionType encryptionType, string encryptionPassword, ES3Settings settings = null) : this(path, settings)
	{
		this.encryptionType = encryptionType;
		this.encryptionPassword = encryptionPassword;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00005918 File Offset: 0x00003B18
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES3Settings(bool applyDefaults)
	{
		if (applyDefaults && ES3Settings.defaultSettings != null)
		{
			ES3Settings._defaults.CopyInto(this);
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x000059B3 File Offset: 0x00003BB3
	private static bool IsAbsolute(string path)
	{
		return (path.Length > 0 && (path[0] == '/' || path[0] == '\\')) || (path.Length > 1 && path[1] == ':');
	}

	// Token: 0x06000148 RID: 328 RVA: 0x000059F0 File Offset: 0x00003BF0
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object Clone()
	{
		ES3Settings es3Settings = new ES3Settings(null, null);
		this.CopyInto(es3Settings);
		return es3Settings;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00005A10 File Offset: 0x00003C10
	private void CopyInto(ES3Settings newSettings)
	{
		newSettings._location = this._location;
		newSettings.directory = this.directory;
		newSettings.format = this.format;
		newSettings.prettyPrint = this.prettyPrint;
		newSettings.path = this.path;
		newSettings.encryptionType = this.encryptionType;
		newSettings.encryptionPassword = this.encryptionPassword;
		newSettings.compressionType = this.compressionType;
		newSettings.bufferSize = this.bufferSize;
		newSettings.encoding = this.encoding;
		newSettings.typeChecking = this.typeChecking;
		newSettings.safeReflection = this.safeReflection;
		newSettings.referenceMode = this.referenceMode;
		newSettings.memberReferenceMode = this.memberReferenceMode;
		newSettings.assemblyNames = this.assemblyNames;
		newSettings.saveChildren = this.saveChildren;
		newSettings.serializationDepthLimit = this.serializationDepthLimit;
		newSettings.postprocessRawCachedData = this.postprocessRawCachedData;
	}

	// Token: 0x04000031 RID: 49
	private static ES3Settings _defaults = null;

	// Token: 0x04000032 RID: 50
	private static ES3Defaults _defaultSettingsScriptableObject;

	// Token: 0x04000033 RID: 51
	private const string defaultSettingsPath = "ES3/ES3Defaults";

	// Token: 0x04000034 RID: 52
	private static ES3Settings _unencryptedUncompressedSettings = null;

	// Token: 0x04000035 RID: 53
	private static readonly string[] resourcesExtensions = new string[]
	{
		".txt",
		".htm",
		".html",
		".xml",
		".bytes",
		".json",
		".csv",
		".yaml",
		".fnt"
	};

	// Token: 0x04000036 RID: 54
	[SerializeField]
	private ES3.Location _location;

	// Token: 0x04000037 RID: 55
	public string path = "SaveFile.es3";

	// Token: 0x04000038 RID: 56
	public ES3.EncryptionType encryptionType;

	// Token: 0x04000039 RID: 57
	public ES3.CompressionType compressionType;

	// Token: 0x0400003A RID: 58
	public string encryptionPassword = "password";

	// Token: 0x0400003B RID: 59
	public ES3.Directory directory;

	// Token: 0x0400003C RID: 60
	public ES3.Format format;

	// Token: 0x0400003D RID: 61
	public bool prettyPrint = true;

	// Token: 0x0400003E RID: 62
	public int bufferSize = 2048;

	// Token: 0x0400003F RID: 63
	public Encoding encoding = Encoding.UTF8;

	// Token: 0x04000040 RID: 64
	public bool saveChildren = true;

	// Token: 0x04000041 RID: 65
	public bool postprocessRawCachedData;

	// Token: 0x04000042 RID: 66
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool typeChecking = true;

	// Token: 0x04000043 RID: 67
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool safeReflection = true;

	// Token: 0x04000044 RID: 68
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES3.ReferenceMode memberReferenceMode;

	// Token: 0x04000045 RID: 69
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ES3.ReferenceMode referenceMode = ES3.ReferenceMode.ByRefAndValue;

	// Token: 0x04000046 RID: 70
	[EditorBrowsable(EditorBrowsableState.Never)]
	public int serializationDepthLimit = 64;

	// Token: 0x04000047 RID: 71
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string[] assemblyNames = new string[]
	{
		"Assembly-CSharp-firstpass",
		"Assembly-CSharp"
	};
}
