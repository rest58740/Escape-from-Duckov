using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000052 RID: 82
	public struct Geometry
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00004A7D File Offset: 0x00002C7D
		public RESULT release()
		{
			return Geometry.FMOD5_Geometry_Release(this.handle);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00004A8A File Offset: 0x00002C8A
		public RESULT addPolygon(float directocclusion, float reverbocclusion, bool doublesided, int numvertices, VECTOR[] vertices, out int polygonindex)
		{
			return Geometry.FMOD5_Geometry_AddPolygon(this.handle, directocclusion, reverbocclusion, doublesided, numvertices, vertices, out polygonindex);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00004AA0 File Offset: 0x00002CA0
		public RESULT getNumPolygons(out int numpolygons)
		{
			return Geometry.FMOD5_Geometry_GetNumPolygons(this.handle, out numpolygons);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00004AAE File Offset: 0x00002CAE
		public RESULT getMaxPolygons(out int maxpolygons, out int maxvertices)
		{
			return Geometry.FMOD5_Geometry_GetMaxPolygons(this.handle, out maxpolygons, out maxvertices);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00004ABD File Offset: 0x00002CBD
		public RESULT getPolygonNumVertices(int index, out int numvertices)
		{
			return Geometry.FMOD5_Geometry_GetPolygonNumVertices(this.handle, index, out numvertices);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00004ACC File Offset: 0x00002CCC
		public RESULT setPolygonVertex(int index, int vertexindex, ref VECTOR vertex)
		{
			return Geometry.FMOD5_Geometry_SetPolygonVertex(this.handle, index, vertexindex, ref vertex);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00004ADC File Offset: 0x00002CDC
		public RESULT getPolygonVertex(int index, int vertexindex, out VECTOR vertex)
		{
			return Geometry.FMOD5_Geometry_GetPolygonVertex(this.handle, index, vertexindex, out vertex);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00004AEC File Offset: 0x00002CEC
		public RESULT setPolygonAttributes(int index, float directocclusion, float reverbocclusion, bool doublesided)
		{
			return Geometry.FMOD5_Geometry_SetPolygonAttributes(this.handle, index, directocclusion, reverbocclusion, doublesided);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00004AFE File Offset: 0x00002CFE
		public RESULT getPolygonAttributes(int index, out float directocclusion, out float reverbocclusion, out bool doublesided)
		{
			return Geometry.FMOD5_Geometry_GetPolygonAttributes(this.handle, index, out directocclusion, out reverbocclusion, out doublesided);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00004B10 File Offset: 0x00002D10
		public RESULT setActive(bool active)
		{
			return Geometry.FMOD5_Geometry_SetActive(this.handle, active);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00004B1E File Offset: 0x00002D1E
		public RESULT getActive(out bool active)
		{
			return Geometry.FMOD5_Geometry_GetActive(this.handle, out active);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00004B2C File Offset: 0x00002D2C
		public RESULT setRotation(ref VECTOR forward, ref VECTOR up)
		{
			return Geometry.FMOD5_Geometry_SetRotation(this.handle, ref forward, ref up);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00004B3B File Offset: 0x00002D3B
		public RESULT getRotation(out VECTOR forward, out VECTOR up)
		{
			return Geometry.FMOD5_Geometry_GetRotation(this.handle, out forward, out up);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00004B4A File Offset: 0x00002D4A
		public RESULT setPosition(ref VECTOR position)
		{
			return Geometry.FMOD5_Geometry_SetPosition(this.handle, ref position);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00004B58 File Offset: 0x00002D58
		public RESULT getPosition(out VECTOR position)
		{
			return Geometry.FMOD5_Geometry_GetPosition(this.handle, out position);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00004B66 File Offset: 0x00002D66
		public RESULT setScale(ref VECTOR scale)
		{
			return Geometry.FMOD5_Geometry_SetScale(this.handle, ref scale);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00004B74 File Offset: 0x00002D74
		public RESULT getScale(out VECTOR scale)
		{
			return Geometry.FMOD5_Geometry_GetScale(this.handle, out scale);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00004B82 File Offset: 0x00002D82
		public RESULT save(IntPtr data, out int datasize)
		{
			return Geometry.FMOD5_Geometry_Save(this.handle, data, out datasize);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00004B91 File Offset: 0x00002D91
		public RESULT setUserData(IntPtr userdata)
		{
			return Geometry.FMOD5_Geometry_SetUserData(this.handle, userdata);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00004B9F File Offset: 0x00002D9F
		public RESULT getUserData(out IntPtr userdata)
		{
			return Geometry.FMOD5_Geometry_GetUserData(this.handle, out userdata);
		}

		// Token: 0x060003C4 RID: 964
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_Release(IntPtr geometry);

		// Token: 0x060003C5 RID: 965
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_AddPolygon(IntPtr geometry, float directocclusion, float reverbocclusion, bool doublesided, int numvertices, VECTOR[] vertices, out int polygonindex);

		// Token: 0x060003C6 RID: 966
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetNumPolygons(IntPtr geometry, out int numpolygons);

		// Token: 0x060003C7 RID: 967
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetMaxPolygons(IntPtr geometry, out int maxpolygons, out int maxvertices);

		// Token: 0x060003C8 RID: 968
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetPolygonNumVertices(IntPtr geometry, int index, out int numvertices);

		// Token: 0x060003C9 RID: 969
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetPolygonVertex(IntPtr geometry, int index, int vertexindex, ref VECTOR vertex);

		// Token: 0x060003CA RID: 970
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetPolygonVertex(IntPtr geometry, int index, int vertexindex, out VECTOR vertex);

		// Token: 0x060003CB RID: 971
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetPolygonAttributes(IntPtr geometry, int index, float directocclusion, float reverbocclusion, bool doublesided);

		// Token: 0x060003CC RID: 972
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetPolygonAttributes(IntPtr geometry, int index, out float directocclusion, out float reverbocclusion, out bool doublesided);

		// Token: 0x060003CD RID: 973
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetActive(IntPtr geometry, bool active);

		// Token: 0x060003CE RID: 974
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetActive(IntPtr geometry, out bool active);

		// Token: 0x060003CF RID: 975
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetRotation(IntPtr geometry, ref VECTOR forward, ref VECTOR up);

		// Token: 0x060003D0 RID: 976
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetRotation(IntPtr geometry, out VECTOR forward, out VECTOR up);

		// Token: 0x060003D1 RID: 977
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetPosition(IntPtr geometry, ref VECTOR position);

		// Token: 0x060003D2 RID: 978
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetPosition(IntPtr geometry, out VECTOR position);

		// Token: 0x060003D3 RID: 979
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetScale(IntPtr geometry, ref VECTOR scale);

		// Token: 0x060003D4 RID: 980
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetScale(IntPtr geometry, out VECTOR scale);

		// Token: 0x060003D5 RID: 981
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_Save(IntPtr geometry, IntPtr data, out int datasize);

		// Token: 0x060003D6 RID: 982
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_SetUserData(IntPtr geometry, IntPtr userdata);

		// Token: 0x060003D7 RID: 983
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Geometry_GetUserData(IntPtr geometry, out IntPtr userdata);

		// Token: 0x060003D8 RID: 984 RVA: 0x00004BAD File Offset: 0x00002DAD
		public Geometry(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00004BB6 File Offset: 0x00002DB6
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00004BC8 File Offset: 0x00002DC8
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000265 RID: 613
		public IntPtr handle;
	}
}
