using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Resources
{
	// Token: 0x02000868 RID: 2152
	[ComVisible(true)]
	[Serializable]
	public class ResourceManager
	{
		// Token: 0x06004781 RID: 18305 RVA: 0x000E94BC File Offset: 0x000E76BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Init()
		{
			try
			{
				this.m_callingAssembly = (RuntimeAssembly)Assembly.GetCallingAssembly();
			}
			catch
			{
			}
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x000E94F0 File Offset: 0x000E76F0
		protected ResourceManager()
		{
			this.Init();
			this._lastUsedResourceCache = new ResourceManager.CultureNameResourceSetPair();
			ResourceManager.ResourceManagerMediator mediator = new ResourceManager.ResourceManagerMediator(this);
			this.resourceGroveler = new ManifestBasedResourceGroveler(mediator);
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x000E9528 File Offset: 0x000E7728
		private ResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (resourceDir == null)
			{
				throw new ArgumentNullException("resourceDir");
			}
			this.BaseNameField = baseName;
			this.moduleDir = resourceDir;
			this._userResourceSet = usingResourceSet;
			this.ResourceSets = new Hashtable();
			this._resourceSets = new Dictionary<string, ResourceSet>();
			this._lastUsedResourceCache = new ResourceManager.CultureNameResourceSetPair();
			this.UseManifest = false;
			ResourceManager.ResourceManagerMediator mediator = new ResourceManager.ResourceManagerMediator(this);
			this.resourceGroveler = new FileBasedResourceGroveler(mediator);
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x000E95A8 File Offset: 0x000E77A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(string baseName, Assembly assembly)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (null == assembly)
			{
				throw new ArgumentNullException("assembly");
			}
			if (!(assembly is RuntimeAssembly))
			{
				throw new ArgumentException(Environment.GetResourceString("Assembly must be a runtime Assembly object."));
			}
			this.MainAssembly = assembly;
			this.BaseNameField = baseName;
			this.SetAppXConfiguration();
			this.CommonAssemblyInit();
			try
			{
				this.m_callingAssembly = (RuntimeAssembly)Assembly.GetCallingAssembly();
				if (assembly == typeof(object).Assembly && this.m_callingAssembly != assembly)
				{
					this.m_callingAssembly = null;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x000E9660 File Offset: 0x000E7860
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(string baseName, Assembly assembly, Type usingResourceSet)
		{
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			if (null == assembly)
			{
				throw new ArgumentNullException("assembly");
			}
			if (!(assembly is RuntimeAssembly))
			{
				throw new ArgumentException(Environment.GetResourceString("Assembly must be a runtime Assembly object."));
			}
			this.MainAssembly = assembly;
			this.BaseNameField = baseName;
			if (usingResourceSet != null && usingResourceSet != ResourceManager._minResourceSet && !usingResourceSet.IsSubclassOf(ResourceManager._minResourceSet))
			{
				throw new ArgumentException(Environment.GetResourceString("Type parameter must refer to a subclass of ResourceSet."), "usingResourceSet");
			}
			this._userResourceSet = usingResourceSet;
			this.CommonAssemblyInit();
			try
			{
				this.m_callingAssembly = (RuntimeAssembly)Assembly.GetCallingAssembly();
				if (assembly == typeof(object).Assembly && this.m_callingAssembly != assembly)
				{
					this.m_callingAssembly = null;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x000E9754 File Offset: 0x000E7954
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceManager(Type resourceSource)
		{
			if (null == resourceSource)
			{
				throw new ArgumentNullException("resourceSource");
			}
			if (!(resourceSource is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Type must be a runtime Type object."));
			}
			this._locationInfo = resourceSource;
			this.MainAssembly = this._locationInfo.Assembly;
			this.BaseNameField = resourceSource.Name;
			this.SetAppXConfiguration();
			this.CommonAssemblyInit();
			try
			{
				this.m_callingAssembly = (RuntimeAssembly)Assembly.GetCallingAssembly();
				if (this.MainAssembly == typeof(object).Assembly && this.m_callingAssembly != this.MainAssembly)
				{
					this.m_callingAssembly = null;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x000E9820 File Offset: 0x000E7A20
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this._resourceSets = null;
			this.resourceGroveler = null;
			this._lastUsedResourceCache = null;
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x000E9838 File Offset: 0x000E7A38
		[SecuritySafeCritical]
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this._resourceSets = new Dictionary<string, ResourceSet>();
			this._lastUsedResourceCache = new ResourceManager.CultureNameResourceSetPair();
			ResourceManager.ResourceManagerMediator mediator = new ResourceManager.ResourceManagerMediator(this);
			if (this.UseManifest)
			{
				this.resourceGroveler = new ManifestBasedResourceGroveler(mediator);
			}
			else
			{
				this.resourceGroveler = new FileBasedResourceGroveler(mediator);
			}
			if (this.m_callingAssembly == null)
			{
				this.m_callingAssembly = (RuntimeAssembly)this._callingAssembly;
			}
			if (this.UseManifest && this._neutralResourcesCulture == null)
			{
				this._neutralResourcesCulture = ManifestBasedResourceGroveler.GetNeutralResourcesLanguage(this.MainAssembly, ref this._fallbackLoc);
			}
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x000E98CA File Offset: 0x000E7ACA
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this._callingAssembly = this.m_callingAssembly;
			this.UseSatelliteAssem = this.UseManifest;
			this.ResourceSets = new Hashtable();
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x000E98F0 File Offset: 0x000E7AF0
		[SecuritySafeCritical]
		private void CommonAssemblyInit()
		{
			this.UseManifest = true;
			this._resourceSets = new Dictionary<string, ResourceSet>();
			this._lastUsedResourceCache = new ResourceManager.CultureNameResourceSetPair();
			this._fallbackLoc = UltimateResourceFallbackLocation.MainAssembly;
			ResourceManager.ResourceManagerMediator mediator = new ResourceManager.ResourceManagerMediator(this);
			this.resourceGroveler = new ManifestBasedResourceGroveler(mediator);
			this._neutralResourcesCulture = ManifestBasedResourceGroveler.GetNeutralResourcesLanguage(this.MainAssembly, ref this._fallbackLoc);
			this.ResourceSets = new Hashtable();
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x0600478B RID: 18315 RVA: 0x000E9956 File Offset: 0x000E7B56
		public virtual string BaseName
		{
			get
			{
				return this.BaseNameField;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x0600478C RID: 18316 RVA: 0x000E995E File Offset: 0x000E7B5E
		// (set) Token: 0x0600478D RID: 18317 RVA: 0x000E9966 File Offset: 0x000E7B66
		public virtual bool IgnoreCase
		{
			get
			{
				return this._ignoreCase;
			}
			set
			{
				this._ignoreCase = value;
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x0600478E RID: 18318 RVA: 0x000E996F File Offset: 0x000E7B6F
		public virtual Type ResourceSetType
		{
			get
			{
				if (!(this._userResourceSet == null))
				{
					return this._userResourceSet;
				}
				return typeof(RuntimeResourceSet);
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600478F RID: 18319 RVA: 0x000E9990 File Offset: 0x000E7B90
		// (set) Token: 0x06004790 RID: 18320 RVA: 0x000E9998 File Offset: 0x000E7B98
		protected UltimateResourceFallbackLocation FallbackLocation
		{
			get
			{
				return this._fallbackLoc;
			}
			set
			{
				this._fallbackLoc = value;
			}
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x000E99A4 File Offset: 0x000E7BA4
		public virtual void ReleaseAllResources()
		{
			Dictionary<string, ResourceSet> resourceSets = this._resourceSets;
			this._resourceSets = new Dictionary<string, ResourceSet>();
			this._lastUsedResourceCache = new ResourceManager.CultureNameResourceSetPair();
			Dictionary<string, ResourceSet> obj = resourceSets;
			lock (obj)
			{
				IDictionaryEnumerator dictionaryEnumerator = resourceSets.GetEnumerator();
				IDictionaryEnumerator dictionaryEnumerator2 = null;
				if (this.ResourceSets != null)
				{
					dictionaryEnumerator2 = this.ResourceSets.GetEnumerator();
				}
				this.ResourceSets = new Hashtable();
				while (dictionaryEnumerator.MoveNext())
				{
					((ResourceSet)dictionaryEnumerator.Value).Close();
				}
				if (dictionaryEnumerator2 != null)
				{
					while (dictionaryEnumerator2.MoveNext())
					{
						((ResourceSet)dictionaryEnumerator2.Value).Close();
					}
				}
			}
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x000E9A60 File Offset: 0x000E7C60
		public static ResourceManager CreateFileBasedResourceManager(string baseName, string resourceDir, Type usingResourceSet)
		{
			return new ResourceManager(baseName, resourceDir, usingResourceSet);
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x000E9A6C File Offset: 0x000E7C6C
		protected virtual string GetResourceFileName(CultureInfo culture)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			stringBuilder.Append(this.BaseNameField);
			if (!culture.HasInvariantCultureName)
			{
				CultureInfo.VerifyCultureName(culture.Name, true);
				stringBuilder.Append('.');
				stringBuilder.Append(culture.Name);
			}
			stringBuilder.Append(".resources");
			return stringBuilder.ToString();
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x000E9AD0 File Offset: 0x000E7CD0
		internal ResourceSet GetFirstResourceSet(CultureInfo culture)
		{
			if (this._neutralResourcesCulture != null && culture.Name == this._neutralResourcesCulture.Name)
			{
				culture = CultureInfo.InvariantCulture;
			}
			if (this._lastUsedResourceCache != null)
			{
				ResourceManager.CultureNameResourceSetPair lastUsedResourceCache = this._lastUsedResourceCache;
				lock (lastUsedResourceCache)
				{
					if (culture.Name == this._lastUsedResourceCache.lastCultureName)
					{
						return this._lastUsedResourceCache.lastResourceSet;
					}
				}
			}
			Dictionary<string, ResourceSet> resourceSets = this._resourceSets;
			ResourceSet resourceSet = null;
			if (resourceSets != null)
			{
				Dictionary<string, ResourceSet> obj = resourceSets;
				lock (obj)
				{
					resourceSets.TryGetValue(culture.Name, out resourceSet);
				}
			}
			if (resourceSet != null)
			{
				if (this._lastUsedResourceCache != null)
				{
					ResourceManager.CultureNameResourceSetPair lastUsedResourceCache = this._lastUsedResourceCache;
					lock (lastUsedResourceCache)
					{
						this._lastUsedResourceCache.lastCultureName = culture.Name;
						this._lastUsedResourceCache.lastResourceSet = resourceSet;
					}
				}
				return resourceSet;
			}
			return null;
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x000E9BFC File Offset: 0x000E7DFC
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			Dictionary<string, ResourceSet> resourceSets = this._resourceSets;
			if (resourceSets != null)
			{
				Dictionary<string, ResourceSet> obj = resourceSets;
				lock (obj)
				{
					ResourceSet result;
					if (resourceSets.TryGetValue(culture.Name, out result))
					{
						return result;
					}
				}
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (this.UseManifest && culture.HasInvariantCultureName)
			{
				string resourceFileName = this.GetResourceFileName(culture);
				Stream manifestResourceStream = ((RuntimeAssembly)this.MainAssembly).GetManifestResourceStream(this._locationInfo, resourceFileName, this.m_callingAssembly == this.MainAssembly, ref stackCrawlMark);
				if (createIfNotExists && manifestResourceStream != null)
				{
					ResourceSet result = ((ManifestBasedResourceGroveler)this.resourceGroveler).CreateResourceSet(manifestResourceStream, this.MainAssembly);
					ResourceManager.AddResourceSet(resourceSets, culture.Name, ref result);
					return result;
				}
			}
			return this.InternalGetResourceSet(culture, createIfNotExists, tryParents);
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x000E9CE8 File Offset: 0x000E7EE8
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		protected virtual ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return this.InternalGetResourceSet(culture, createIfNotExists, tryParents, ref stackCrawlMark);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x000E9D04 File Offset: 0x000E7F04
		[SecurityCritical]
		private ResourceSet InternalGetResourceSet(CultureInfo requestedCulture, bool createIfNotExists, bool tryParents, ref StackCrawlMark stackMark)
		{
			Dictionary<string, ResourceSet> resourceSets = this._resourceSets;
			ResourceSet resourceSet = null;
			CultureInfo cultureInfo = null;
			Dictionary<string, ResourceSet> obj = resourceSets;
			lock (obj)
			{
				if (resourceSets.TryGetValue(requestedCulture.Name, out resourceSet))
				{
					return resourceSet;
				}
			}
			ResourceFallbackManager resourceFallbackManager = new ResourceFallbackManager(requestedCulture, this._neutralResourcesCulture, tryParents);
			foreach (CultureInfo cultureInfo2 in resourceFallbackManager)
			{
				obj = resourceSets;
				lock (obj)
				{
					if (resourceSets.TryGetValue(cultureInfo2.Name, out resourceSet))
					{
						if (requestedCulture != cultureInfo2)
						{
							cultureInfo = cultureInfo2;
						}
						break;
					}
				}
				resourceSet = this.resourceGroveler.GrovelForResourceSet(cultureInfo2, resourceSets, tryParents, createIfNotExists, ref stackMark);
				if (resourceSet != null)
				{
					cultureInfo = cultureInfo2;
					break;
				}
			}
			if (resourceSet != null && cultureInfo != null)
			{
				foreach (CultureInfo cultureInfo3 in resourceFallbackManager)
				{
					ResourceManager.AddResourceSet(resourceSets, cultureInfo3.Name, ref resourceSet);
					if (cultureInfo3 == cultureInfo)
					{
						break;
					}
				}
			}
			return resourceSet;
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x000E9E5C File Offset: 0x000E805C
		private static void AddResourceSet(Dictionary<string, ResourceSet> localResourceSets, string cultureName, ref ResourceSet rs)
		{
			lock (localResourceSets)
			{
				ResourceSet resourceSet;
				if (localResourceSets.TryGetValue(cultureName, out resourceSet))
				{
					if (resourceSet != rs)
					{
						if (!localResourceSets.ContainsValue(rs))
						{
							rs.Dispose();
						}
						rs = resourceSet;
					}
				}
				else
				{
					localResourceSets.Add(cultureName, rs);
				}
			}
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x000E9EC0 File Offset: 0x000E80C0
		protected static Version GetSatelliteContractVersion(Assembly a)
		{
			if (a == null)
			{
				throw new ArgumentNullException("a", Environment.GetResourceString("Assembly cannot be null."));
			}
			string text = null;
			if (a.ReflectionOnly)
			{
				foreach (CustomAttributeData customAttributeData in CustomAttributeData.GetCustomAttributes(a))
				{
					if (customAttributeData.Constructor.DeclaringType == typeof(SatelliteContractVersionAttribute))
					{
						text = (string)customAttributeData.ConstructorArguments[0].Value;
						break;
					}
				}
				if (text == null)
				{
					return null;
				}
			}
			else
			{
				object[] customAttributes = a.GetCustomAttributes(typeof(SatelliteContractVersionAttribute), false);
				if (customAttributes.Length == 0)
				{
					return null;
				}
				text = ((SatelliteContractVersionAttribute)customAttributes[0]).Version;
			}
			Version result;
			try
			{
				result = new Version(text);
			}
			catch (ArgumentOutOfRangeException innerException)
			{
				if (a == typeof(object).Assembly)
				{
					return null;
				}
				throw new ArgumentException(Environment.GetResourceString("Satellite contract version attribute on the assembly '{0}' specifies an invalid version: {1}.", new object[]
				{
					a.ToString(),
					text
				}), innerException);
			}
			return result;
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x000E9FF4 File Offset: 0x000E81F4
		[SecuritySafeCritical]
		protected static CultureInfo GetNeutralResourcesLanguage(Assembly a)
		{
			UltimateResourceFallbackLocation ultimateResourceFallbackLocation = UltimateResourceFallbackLocation.MainAssembly;
			return ManifestBasedResourceGroveler.GetNeutralResourcesLanguage(a, ref ultimateResourceFallbackLocation);
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x000EA00C File Offset: 0x000E820C
		internal static bool CompareNames(string asmTypeName1, string typeName2, AssemblyName asmName2)
		{
			int num = asmTypeName1.IndexOf(',');
			if (((num == -1) ? asmTypeName1.Length : num) != typeName2.Length)
			{
				return false;
			}
			if (string.Compare(asmTypeName1, 0, typeName2, 0, typeName2.Length, StringComparison.Ordinal) != 0)
			{
				return false;
			}
			if (num == -1)
			{
				return true;
			}
			while (char.IsWhiteSpace(asmTypeName1[++num]))
			{
			}
			AssemblyName assemblyName = new AssemblyName(asmTypeName1.Substring(num));
			if (string.Compare(assemblyName.Name, asmName2.Name, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return false;
			}
			if (string.Compare(assemblyName.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			if (assemblyName.CultureInfo != null && asmName2.CultureInfo != null && assemblyName.CultureInfo.LCID != asmName2.CultureInfo.LCID)
			{
				return false;
			}
			byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
			byte[] publicKeyToken2 = asmName2.GetPublicKeyToken();
			if (publicKeyToken != null && publicKeyToken2 != null)
			{
				if (publicKeyToken.Length != publicKeyToken2.Length)
				{
					return false;
				}
				for (int i = 0; i < publicKeyToken.Length; i++)
				{
					if (publicKeyToken[i] != publicKeyToken2[i])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x00004BF9 File Offset: 0x00002DF9
		private void SetAppXConfiguration()
		{
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x000EA104 File Offset: 0x000E8304
		public virtual string GetString(string name)
		{
			return this.GetString(name, null);
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x000EA110 File Offset: 0x000E8310
		public virtual string GetString(string name, CultureInfo culture)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = Thread.CurrentThread.GetCurrentUICultureNoAppX();
			}
			ResourceSet resourceSet = this.GetFirstResourceSet(culture);
			if (resourceSet != null)
			{
				string @string = resourceSet.GetString(name, this._ignoreCase);
				if (@string != null)
				{
					return @string;
				}
			}
			foreach (CultureInfo cultureInfo in new ResourceFallbackManager(culture, this._neutralResourcesCulture, true))
			{
				ResourceSet resourceSet2 = this.InternalGetResourceSet(cultureInfo, true, true);
				if (resourceSet2 == null)
				{
					break;
				}
				if (resourceSet2 != resourceSet)
				{
					string string2 = resourceSet2.GetString(name, this._ignoreCase);
					if (string2 != null)
					{
						if (this._lastUsedResourceCache != null)
						{
							ResourceManager.CultureNameResourceSetPair lastUsedResourceCache = this._lastUsedResourceCache;
							lock (lastUsedResourceCache)
							{
								this._lastUsedResourceCache.lastCultureName = cultureInfo.Name;
								this._lastUsedResourceCache.lastResourceSet = resourceSet2;
							}
						}
						return string2;
					}
					resourceSet = resourceSet2;
				}
			}
			return null;
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x000EA22C File Offset: 0x000E842C
		public virtual object GetObject(string name)
		{
			return this.GetObject(name, null, true);
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x000EA237 File Offset: 0x000E8437
		public virtual object GetObject(string name, CultureInfo culture)
		{
			return this.GetObject(name, culture, true);
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x000EA244 File Offset: 0x000E8444
		private object GetObject(string name, CultureInfo culture, bool wrapUnmanagedMemStream)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (culture == null)
			{
				culture = Thread.CurrentThread.GetCurrentUICultureNoAppX();
			}
			ResourceSet resourceSet = this.GetFirstResourceSet(culture);
			if (resourceSet != null)
			{
				object @object = resourceSet.GetObject(name, this._ignoreCase);
				if (@object != null)
				{
					UnmanagedMemoryStream unmanagedMemoryStream = @object as UnmanagedMemoryStream;
					if (unmanagedMemoryStream != null && wrapUnmanagedMemStream)
					{
						return new UnmanagedMemoryStreamWrapper(unmanagedMemoryStream);
					}
					return @object;
				}
			}
			foreach (CultureInfo cultureInfo in new ResourceFallbackManager(culture, this._neutralResourcesCulture, true))
			{
				ResourceSet resourceSet2 = this.InternalGetResourceSet(cultureInfo, true, true);
				if (resourceSet2 == null)
				{
					break;
				}
				if (resourceSet2 != resourceSet)
				{
					object object2 = resourceSet2.GetObject(name, this._ignoreCase);
					if (object2 != null)
					{
						if (this._lastUsedResourceCache != null)
						{
							ResourceManager.CultureNameResourceSetPair lastUsedResourceCache = this._lastUsedResourceCache;
							lock (lastUsedResourceCache)
							{
								this._lastUsedResourceCache.lastCultureName = cultureInfo.Name;
								this._lastUsedResourceCache.lastResourceSet = resourceSet2;
							}
						}
						UnmanagedMemoryStream unmanagedMemoryStream2 = object2 as UnmanagedMemoryStream;
						if (unmanagedMemoryStream2 != null && wrapUnmanagedMemStream)
						{
							return new UnmanagedMemoryStreamWrapper(unmanagedMemoryStream2);
						}
						return object2;
					}
					else
					{
						resourceSet = resourceSet2;
					}
				}
			}
			return null;
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x000EA39C File Offset: 0x000E859C
		[ComVisible(false)]
		public UnmanagedMemoryStream GetStream(string name)
		{
			return this.GetStream(name, null);
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x000EA3A8 File Offset: 0x000E85A8
		[ComVisible(false)]
		public UnmanagedMemoryStream GetStream(string name, CultureInfo culture)
		{
			object @object = this.GetObject(name, culture, false);
			UnmanagedMemoryStream unmanagedMemoryStream = @object as UnmanagedMemoryStream;
			if (unmanagedMemoryStream == null && @object != null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Resource '{0}' was not a Stream - call GetObject instead.", new object[]
				{
					name
				}));
			}
			return unmanagedMemoryStream;
		}

		// Token: 0x04002DCE RID: 11726
		protected string BaseNameField;

		// Token: 0x04002DCF RID: 11727
		[Obsolete("call InternalGetResourceSet instead")]
		protected Hashtable ResourceSets;

		// Token: 0x04002DD0 RID: 11728
		[NonSerialized]
		private Dictionary<string, ResourceSet> _resourceSets;

		// Token: 0x04002DD1 RID: 11729
		private string moduleDir;

		// Token: 0x04002DD2 RID: 11730
		protected Assembly MainAssembly;

		// Token: 0x04002DD3 RID: 11731
		private Type _locationInfo;

		// Token: 0x04002DD4 RID: 11732
		private Type _userResourceSet;

		// Token: 0x04002DD5 RID: 11733
		private CultureInfo _neutralResourcesCulture;

		// Token: 0x04002DD6 RID: 11734
		[NonSerialized]
		private ResourceManager.CultureNameResourceSetPair _lastUsedResourceCache;

		// Token: 0x04002DD7 RID: 11735
		private bool _ignoreCase;

		// Token: 0x04002DD8 RID: 11736
		private bool UseManifest;

		// Token: 0x04002DD9 RID: 11737
		[OptionalField(VersionAdded = 1)]
		private bool UseSatelliteAssem;

		// Token: 0x04002DDA RID: 11738
		[OptionalField]
		private UltimateResourceFallbackLocation _fallbackLoc;

		// Token: 0x04002DDB RID: 11739
		[OptionalField]
		private Version _satelliteContractVersion;

		// Token: 0x04002DDC RID: 11740
		[OptionalField]
		private bool _lookedForSatelliteContractVersion;

		// Token: 0x04002DDD RID: 11741
		[OptionalField(VersionAdded = 1)]
		private Assembly _callingAssembly;

		// Token: 0x04002DDE RID: 11742
		[OptionalField(VersionAdded = 4)]
		private RuntimeAssembly m_callingAssembly;

		// Token: 0x04002DDF RID: 11743
		[NonSerialized]
		private IResourceGroveler resourceGroveler;

		// Token: 0x04002DE0 RID: 11744
		public static readonly int MagicNumber = -1091581234;

		// Token: 0x04002DE1 RID: 11745
		public static readonly int HeaderVersionNumber = 1;

		// Token: 0x04002DE2 RID: 11746
		private static readonly Type _minResourceSet = typeof(ResourceSet);

		// Token: 0x04002DE3 RID: 11747
		internal static readonly string ResReaderTypeName = typeof(ResourceReader).FullName;

		// Token: 0x04002DE4 RID: 11748
		internal static readonly string ResSetTypeName = typeof(RuntimeResourceSet).FullName;

		// Token: 0x04002DE5 RID: 11749
		internal static readonly string MscorlibName = typeof(ResourceReader).Assembly.FullName;

		// Token: 0x04002DE6 RID: 11750
		internal const string ResFileExtension = ".resources";

		// Token: 0x04002DE7 RID: 11751
		internal const int ResFileExtensionLength = 10;

		// Token: 0x04002DE8 RID: 11752
		internal static readonly int DEBUG = 0;

		// Token: 0x02000869 RID: 2153
		internal class CultureNameResourceSetPair
		{
			// Token: 0x04002DE9 RID: 11753
			public string lastCultureName;

			// Token: 0x04002DEA RID: 11754
			public ResourceSet lastResourceSet;
		}

		// Token: 0x0200086A RID: 2154
		internal class ResourceManagerMediator
		{
			// Token: 0x060047A6 RID: 18342 RVA: 0x000EA45B File Offset: 0x000E865B
			internal ResourceManagerMediator(ResourceManager rm)
			{
				if (rm == null)
				{
					throw new ArgumentNullException("rm");
				}
				this._rm = rm;
			}

			// Token: 0x17000AF7 RID: 2807
			// (get) Token: 0x060047A7 RID: 18343 RVA: 0x000EA478 File Offset: 0x000E8678
			internal string ModuleDir
			{
				get
				{
					return this._rm.moduleDir;
				}
			}

			// Token: 0x17000AF8 RID: 2808
			// (get) Token: 0x060047A8 RID: 18344 RVA: 0x000EA485 File Offset: 0x000E8685
			internal Type LocationInfo
			{
				get
				{
					return this._rm._locationInfo;
				}
			}

			// Token: 0x17000AF9 RID: 2809
			// (get) Token: 0x060047A9 RID: 18345 RVA: 0x000EA492 File Offset: 0x000E8692
			internal Type UserResourceSet
			{
				get
				{
					return this._rm._userResourceSet;
				}
			}

			// Token: 0x17000AFA RID: 2810
			// (get) Token: 0x060047AA RID: 18346 RVA: 0x000EA49F File Offset: 0x000E869F
			internal string BaseNameField
			{
				get
				{
					return this._rm.BaseNameField;
				}
			}

			// Token: 0x17000AFB RID: 2811
			// (get) Token: 0x060047AB RID: 18347 RVA: 0x000EA4AC File Offset: 0x000E86AC
			// (set) Token: 0x060047AC RID: 18348 RVA: 0x000EA4B9 File Offset: 0x000E86B9
			internal CultureInfo NeutralResourcesCulture
			{
				get
				{
					return this._rm._neutralResourcesCulture;
				}
				set
				{
					this._rm._neutralResourcesCulture = value;
				}
			}

			// Token: 0x060047AD RID: 18349 RVA: 0x000EA4C7 File Offset: 0x000E86C7
			internal string GetResourceFileName(CultureInfo culture)
			{
				return this._rm.GetResourceFileName(culture);
			}

			// Token: 0x17000AFC RID: 2812
			// (get) Token: 0x060047AE RID: 18350 RVA: 0x000EA4D5 File Offset: 0x000E86D5
			// (set) Token: 0x060047AF RID: 18351 RVA: 0x000EA4E2 File Offset: 0x000E86E2
			internal bool LookedForSatelliteContractVersion
			{
				get
				{
					return this._rm._lookedForSatelliteContractVersion;
				}
				set
				{
					this._rm._lookedForSatelliteContractVersion = value;
				}
			}

			// Token: 0x17000AFD RID: 2813
			// (get) Token: 0x060047B0 RID: 18352 RVA: 0x000EA4F0 File Offset: 0x000E86F0
			// (set) Token: 0x060047B1 RID: 18353 RVA: 0x000EA4FD File Offset: 0x000E86FD
			internal Version SatelliteContractVersion
			{
				get
				{
					return this._rm._satelliteContractVersion;
				}
				set
				{
					this._rm._satelliteContractVersion = value;
				}
			}

			// Token: 0x060047B2 RID: 18354 RVA: 0x000EA50B File Offset: 0x000E870B
			internal Version ObtainSatelliteContractVersion(Assembly a)
			{
				return ResourceManager.GetSatelliteContractVersion(a);
			}

			// Token: 0x17000AFE RID: 2814
			// (get) Token: 0x060047B3 RID: 18355 RVA: 0x000EA513 File Offset: 0x000E8713
			// (set) Token: 0x060047B4 RID: 18356 RVA: 0x000EA520 File Offset: 0x000E8720
			internal UltimateResourceFallbackLocation FallbackLoc
			{
				get
				{
					return this._rm.FallbackLocation;
				}
				set
				{
					this._rm._fallbackLoc = value;
				}
			}

			// Token: 0x17000AFF RID: 2815
			// (get) Token: 0x060047B5 RID: 18357 RVA: 0x000EA52E File Offset: 0x000E872E
			internal RuntimeAssembly CallingAssembly
			{
				get
				{
					return this._rm.m_callingAssembly;
				}
			}

			// Token: 0x17000B00 RID: 2816
			// (get) Token: 0x060047B6 RID: 18358 RVA: 0x000EA53B File Offset: 0x000E873B
			internal RuntimeAssembly MainAssembly
			{
				get
				{
					return (RuntimeAssembly)this._rm.MainAssembly;
				}
			}

			// Token: 0x17000B01 RID: 2817
			// (get) Token: 0x060047B7 RID: 18359 RVA: 0x000EA54D File Offset: 0x000E874D
			internal string BaseName
			{
				get
				{
					return this._rm.BaseName;
				}
			}

			// Token: 0x04002DEB RID: 11755
			private ResourceManager _rm;
		}
	}
}
