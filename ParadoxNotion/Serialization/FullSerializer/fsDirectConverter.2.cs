using System;
using System.Collections.Generic;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A6 RID: 166
	public abstract class fsDirectConverter<TModel> : fsDirectConverter
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00011B9F File Offset: 0x0000FD9F
		public override Type ModelType
		{
			get
			{
				return typeof(TModel);
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00011BAC File Offset: 0x0000FDAC
		public sealed override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Dictionary<string, fsData> dictionary = new Dictionary<string, fsData>();
			fsResult result = this.DoSerialize((TModel)((object)instance), dictionary);
			serialized = new fsData(dictionary);
			return result;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		public sealed override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Object));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			TModel tmodel = (TModel)((object)instance);
			fsResult += this.DoDeserialize(data.AsDictionary, ref tmodel);
			instance = tmodel;
			return fsResult;
		}

		// Token: 0x06000632 RID: 1586
		protected abstract fsResult DoSerialize(TModel model, Dictionary<string, fsData> serialized);

		// Token: 0x06000633 RID: 1587
		protected abstract fsResult DoDeserialize(Dictionary<string, fsData> data, ref TModel model);
	}
}
