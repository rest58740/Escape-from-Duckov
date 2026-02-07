using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x02000092 RID: 146
	public class ContractBasedImportDefinition : ImportDefinition
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x0000AF23 File Offset: 0x00009123
		protected ContractBasedImportDefinition()
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000AF38 File Offset: 0x00009138
		public ContractBasedImportDefinition(string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy) : this(contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, isPrerequisite, requiredCreationPolicy, MetadataServices.EmptyMetadata)
		{
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000AF5B File Offset: 0x0000915B
		public ContractBasedImportDefinition(string contractName, string requiredTypeIdentity, IEnumerable<KeyValuePair<string, Type>> requiredMetadata, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite, CreationPolicy requiredCreationPolicy, IDictionary<string, object> metadata) : base(contractName, cardinality, isRecomposable, isPrerequisite, metadata)
		{
			Requires.NotNullOrEmpty(contractName, "contractName");
			this._requiredTypeIdentity = requiredTypeIdentity;
			if (requiredMetadata != null)
			{
				this._requiredMetadata = requiredMetadata;
			}
			this._requiredCreationPolicy = requiredCreationPolicy;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000AF9B File Offset: 0x0000919B
		public virtual string RequiredTypeIdentity
		{
			get
			{
				return this._requiredTypeIdentity;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000AFA3 File Offset: 0x000091A3
		public virtual IEnumerable<KeyValuePair<string, Type>> RequiredMetadata
		{
			get
			{
				this.ValidateRequiredMetadata();
				return this._requiredMetadata;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000AFB4 File Offset: 0x000091B4
		private void ValidateRequiredMetadata()
		{
			if (!this._isRequiredMetadataValidated)
			{
				foreach (KeyValuePair<string, Type> keyValuePair in this._requiredMetadata)
				{
					if (keyValuePair.Key == null || keyValuePair.Value == null)
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.Argument_NullElement, "requiredMetadata"));
					}
				}
				this._isRequiredMetadataValidated = true;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000B03C File Offset: 0x0000923C
		public virtual CreationPolicy RequiredCreationPolicy
		{
			get
			{
				return this._requiredCreationPolicy;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000B044 File Offset: 0x00009244
		public override Expression<Func<ExportDefinition, bool>> Constraint
		{
			get
			{
				if (this._constraint == null)
				{
					this._constraint = ConstraintServices.CreateConstraint(this.ContractName, this.RequiredTypeIdentity, this.RequiredMetadata, this.RequiredCreationPolicy);
				}
				return this._constraint;
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000B077 File Offset: 0x00009277
		public override bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
		{
			Requires.NotNull<ExportDefinition>(exportDefinition, "exportDefinition");
			return StringComparers.ContractName.Equals(this.ContractName, exportDefinition.ContractName) && this.MatchRequiredMatadata(exportDefinition);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000B0A8 File Offset: 0x000092A8
		private bool MatchRequiredMatadata(ExportDefinition definition)
		{
			if (!string.IsNullOrEmpty(this.RequiredTypeIdentity))
			{
				string value = definition.Metadata.GetValue("ExportTypeIdentity");
				if (!StringComparers.ContractName.Equals(this.RequiredTypeIdentity, value))
				{
					return false;
				}
			}
			foreach (KeyValuePair<string, Type> keyValuePair in this.RequiredMetadata)
			{
				string key = keyValuePair.Key;
				Type value2 = keyValuePair.Value;
				object obj = null;
				if (!definition.Metadata.TryGetValue(key, ref obj))
				{
					return false;
				}
				if (obj != null)
				{
					if (!value2.IsInstanceOfType(obj))
					{
						return false;
					}
				}
				else if (value2.IsValueType)
				{
					return false;
				}
			}
			if (this.RequiredCreationPolicy == CreationPolicy.Any)
			{
				return true;
			}
			CreationPolicy value3 = definition.Metadata.GetValue("System.ComponentModel.Composition.CreationPolicy");
			return value3 == CreationPolicy.Any || value3 == this.RequiredCreationPolicy;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000B19C File Offset: 0x0000939C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("\n\tContractName\t{0}", this.ContractName));
			stringBuilder.Append(string.Format("\n\tRequiredTypeIdentity\t{0}", this.RequiredTypeIdentity));
			if (this._requiredCreationPolicy != CreationPolicy.Any)
			{
				stringBuilder.Append(string.Format("\n\tRequiredCreationPolicy\t{0}", this.RequiredCreationPolicy));
			}
			if (this._requiredMetadata.Count<KeyValuePair<string, Type>>() > 0)
			{
				stringBuilder.Append(string.Format("\n\tRequiredMetadata", Array.Empty<object>()));
				foreach (KeyValuePair<string, Type> keyValuePair in this._requiredMetadata)
				{
					stringBuilder.Append(string.Format("\n\t\t{0}\t({1})", keyValuePair.Key, keyValuePair.Value));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400017C RID: 380
		private readonly IEnumerable<KeyValuePair<string, Type>> _requiredMetadata = Enumerable.Empty<KeyValuePair<string, Type>>();

		// Token: 0x0400017D RID: 381
		private Expression<Func<ExportDefinition, bool>> _constraint;

		// Token: 0x0400017E RID: 382
		private readonly CreationPolicy _requiredCreationPolicy;

		// Token: 0x0400017F RID: 383
		private readonly string _requiredTypeIdentity;

		// Token: 0x04000180 RID: 384
		private bool _isRequiredMetadataValidated;
	}
}
