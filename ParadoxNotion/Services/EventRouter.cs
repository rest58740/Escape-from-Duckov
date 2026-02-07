using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ParadoxNotion.Services
{
	// Token: 0x02000080 RID: 128
	public class EventRouter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, IDropHandler
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060004D4 RID: 1236 RVA: 0x0000DF7C File Offset: 0x0000C17C
		// (remove) Token: 0x060004D5 RID: 1237 RVA: 0x0000DFB4 File Offset: 0x0000C1B4
		public event EventRouter.EventDelegate<PointerEventData> onPointerEnter;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060004D6 RID: 1238 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		// (remove) Token: 0x060004D7 RID: 1239 RVA: 0x0000E024 File Offset: 0x0000C224
		public event EventRouter.EventDelegate<PointerEventData> onPointerExit;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060004D8 RID: 1240 RVA: 0x0000E05C File Offset: 0x0000C25C
		// (remove) Token: 0x060004D9 RID: 1241 RVA: 0x0000E094 File Offset: 0x0000C294
		public event EventRouter.EventDelegate<PointerEventData> onPointerDown;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060004DA RID: 1242 RVA: 0x0000E0CC File Offset: 0x0000C2CC
		// (remove) Token: 0x060004DB RID: 1243 RVA: 0x0000E104 File Offset: 0x0000C304
		public event EventRouter.EventDelegate<PointerEventData> onPointerUp;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060004DC RID: 1244 RVA: 0x0000E13C File Offset: 0x0000C33C
		// (remove) Token: 0x060004DD RID: 1245 RVA: 0x0000E174 File Offset: 0x0000C374
		public event EventRouter.EventDelegate<PointerEventData> onPointerClick;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060004DE RID: 1246 RVA: 0x0000E1AC File Offset: 0x0000C3AC
		// (remove) Token: 0x060004DF RID: 1247 RVA: 0x0000E1E4 File Offset: 0x0000C3E4
		public event EventRouter.EventDelegate<PointerEventData> onDrag;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060004E0 RID: 1248 RVA: 0x0000E21C File Offset: 0x0000C41C
		// (remove) Token: 0x060004E1 RID: 1249 RVA: 0x0000E254 File Offset: 0x0000C454
		public event EventRouter.EventDelegate<PointerEventData> onDrop;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060004E2 RID: 1250 RVA: 0x0000E28C File Offset: 0x0000C48C
		// (remove) Token: 0x060004E3 RID: 1251 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
		public event EventRouter.EventDelegate<PointerEventData> onScroll;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060004E4 RID: 1252 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		// (remove) Token: 0x060004E5 RID: 1253 RVA: 0x0000E334 File Offset: 0x0000C534
		public event EventRouter.EventDelegate<BaseEventData> onUpdateSelected;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060004E6 RID: 1254 RVA: 0x0000E36C File Offset: 0x0000C56C
		// (remove) Token: 0x060004E7 RID: 1255 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		public event EventRouter.EventDelegate<BaseEventData> onSelect;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060004E8 RID: 1256 RVA: 0x0000E3DC File Offset: 0x0000C5DC
		// (remove) Token: 0x060004E9 RID: 1257 RVA: 0x0000E414 File Offset: 0x0000C614
		public event EventRouter.EventDelegate<BaseEventData> onDeselect;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060004EA RID: 1258 RVA: 0x0000E44C File Offset: 0x0000C64C
		// (remove) Token: 0x060004EB RID: 1259 RVA: 0x0000E484 File Offset: 0x0000C684
		public event EventRouter.EventDelegate<AxisEventData> onMove;

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060004EC RID: 1260 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		// (remove) Token: 0x060004ED RID: 1261 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		public event EventRouter.EventDelegate<BaseEventData> onSubmit;

		// Token: 0x060004EE RID: 1262 RVA: 0x0000E529 File Offset: 0x0000C729
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (this.onPointerEnter != null)
			{
				this.onPointerEnter(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000E54B File Offset: 0x0000C74B
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (this.onPointerExit != null)
			{
				this.onPointerExit(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000E56D File Offset: 0x0000C76D
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.onPointerDown != null)
			{
				this.onPointerDown(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000E58F File Offset: 0x0000C78F
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (this.onPointerUp != null)
			{
				this.onPointerUp(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000E5B1 File Offset: 0x0000C7B1
		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (this.onPointerClick != null)
			{
				this.onPointerClick(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000E5D3 File Offset: 0x0000C7D3
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (this.onDrag != null)
			{
				this.onDrag(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000E5F5 File Offset: 0x0000C7F5
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (this.onDrop != null)
			{
				this.onDrop(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000E617 File Offset: 0x0000C817
		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			if (this.onScroll != null)
			{
				this.onScroll(new EventData<PointerEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000E639 File Offset: 0x0000C839
		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			if (this.onUpdateSelected != null)
			{
				this.onUpdateSelected(new EventData<BaseEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000E65B File Offset: 0x0000C85B
		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (this.onSelect != null)
			{
				this.onSelect(new EventData<BaseEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000E67D File Offset: 0x0000C87D
		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (this.onDeselect != null)
			{
				this.onDeselect(new EventData<BaseEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000E69F File Offset: 0x0000C89F
		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			if (this.onMove != null)
			{
				this.onMove(new EventData<AxisEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000E6C1 File Offset: 0x0000C8C1
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			if (this.onSubmit != null)
			{
				this.onSubmit(new EventData<BaseEventData>(eventData, base.gameObject, this));
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060004FB RID: 1275 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
		// (remove) Token: 0x060004FC RID: 1276 RVA: 0x0000E71C File Offset: 0x0000C91C
		public event EventRouter.EventDelegate onMouseDown;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060004FD RID: 1277 RVA: 0x0000E754 File Offset: 0x0000C954
		// (remove) Token: 0x060004FE RID: 1278 RVA: 0x0000E78C File Offset: 0x0000C98C
		public event EventRouter.EventDelegate onMouseDrag;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060004FF RID: 1279 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		// (remove) Token: 0x06000500 RID: 1280 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		public event EventRouter.EventDelegate onMouseEnter;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06000501 RID: 1281 RVA: 0x0000E834 File Offset: 0x0000CA34
		// (remove) Token: 0x06000502 RID: 1282 RVA: 0x0000E86C File Offset: 0x0000CA6C
		public event EventRouter.EventDelegate onMouseExit;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06000503 RID: 1283 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		// (remove) Token: 0x06000504 RID: 1284 RVA: 0x0000E8DC File Offset: 0x0000CADC
		public event EventRouter.EventDelegate onMouseOver;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000505 RID: 1285 RVA: 0x0000E914 File Offset: 0x0000CB14
		// (remove) Token: 0x06000506 RID: 1286 RVA: 0x0000E94C File Offset: 0x0000CB4C
		public event EventRouter.EventDelegate onMouseUp;

		// Token: 0x06000507 RID: 1287 RVA: 0x0000E981 File Offset: 0x0000CB81
		private void OnMouseDown()
		{
			if (this.onMouseDown != null)
			{
				this.onMouseDown(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000E9A2 File Offset: 0x0000CBA2
		private void OnMouseDrag()
		{
			if (this.onMouseDrag != null)
			{
				this.onMouseDrag(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0000E9C3 File Offset: 0x0000CBC3
		private void OnMouseEnter()
		{
			if (this.onMouseEnter != null)
			{
				this.onMouseEnter(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000E9E4 File Offset: 0x0000CBE4
		private void OnMouseExit()
		{
			if (this.onMouseExit != null)
			{
				this.onMouseExit(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0000EA05 File Offset: 0x0000CC05
		private void OnMouseOver()
		{
			if (this.onMouseOver != null)
			{
				this.onMouseOver(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000EA26 File Offset: 0x0000CC26
		private void OnMouseUp()
		{
			if (this.onMouseUp != null)
			{
				this.onMouseUp(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x0600050D RID: 1293 RVA: 0x0000EA48 File Offset: 0x0000CC48
		// (remove) Token: 0x0600050E RID: 1294 RVA: 0x0000EA80 File Offset: 0x0000CC80
		public event EventRouter.EventDelegate onEnable;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x0600050F RID: 1295 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
		// (remove) Token: 0x06000510 RID: 1296 RVA: 0x0000EAF0 File Offset: 0x0000CCF0
		public event EventRouter.EventDelegate onDisable;

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000511 RID: 1297 RVA: 0x0000EB28 File Offset: 0x0000CD28
		// (remove) Token: 0x06000512 RID: 1298 RVA: 0x0000EB60 File Offset: 0x0000CD60
		public event EventRouter.EventDelegate onDestroy;

		// Token: 0x06000513 RID: 1299 RVA: 0x0000EB95 File Offset: 0x0000CD95
		private void OnEnable()
		{
			if (this.onEnable != null)
			{
				this.onEnable(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000EBB6 File Offset: 0x0000CDB6
		private void OnDisable()
		{
			if (this.onDisable != null)
			{
				this.onDisable(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000EBD7 File Offset: 0x0000CDD7
		private void OnDestroy()
		{
			if (this.onDestroy != null)
			{
				this.onDestroy(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000516 RID: 1302 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		// (remove) Token: 0x06000517 RID: 1303 RVA: 0x0000EC30 File Offset: 0x0000CE30
		public event EventRouter.EventDelegate onTransformChildrenChanged;

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000518 RID: 1304 RVA: 0x0000EC68 File Offset: 0x0000CE68
		// (remove) Token: 0x06000519 RID: 1305 RVA: 0x0000ECA0 File Offset: 0x0000CEA0
		public event EventRouter.EventDelegate onTransformParentChanged;

		// Token: 0x0600051A RID: 1306 RVA: 0x0000ECD5 File Offset: 0x0000CED5
		private void OnTransformChildrenChanged()
		{
			if (this.onTransformChildrenChanged != null)
			{
				this.onTransformChildrenChanged(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000ECF6 File Offset: 0x0000CEF6
		private void OnTransformParentChanged()
		{
			if (this.onTransformParentChanged != null)
			{
				this.onTransformParentChanged(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600051C RID: 1308 RVA: 0x0000ED18 File Offset: 0x0000CF18
		// (remove) Token: 0x0600051D RID: 1309 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public event EventRouter.EventDelegate<int> onAnimatorIK;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x0600051E RID: 1310 RVA: 0x0000ED85 File Offset: 0x0000CF85
		// (remove) Token: 0x0600051F RID: 1311 RVA: 0x0000EDB2 File Offset: 0x0000CFB2
		public event EventRouter.EventDelegate onAnimatorMove
		{
			add
			{
				if (this._routerAnimatorMove == null)
				{
					this._routerAnimatorMove = base.gameObject.GetAddComponent<EventRouterAnimatorMove>();
				}
				this._routerAnimatorMove.onAnimatorMove += value;
			}
			remove
			{
				if (this._routerAnimatorMove != null)
				{
					this._routerAnimatorMove.onAnimatorMove -= value;
				}
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000EDCE File Offset: 0x0000CFCE
		private void OnAnimatorIK(int layerIndex)
		{
			if (this.onAnimatorIK != null)
			{
				this.onAnimatorIK(new EventData<int>(layerIndex, base.gameObject, this));
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000521 RID: 1313 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
		// (remove) Token: 0x06000522 RID: 1314 RVA: 0x0000EE28 File Offset: 0x0000D028
		public event EventRouter.EventDelegate onBecameInvisible;

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000523 RID: 1315 RVA: 0x0000EE60 File Offset: 0x0000D060
		// (remove) Token: 0x06000524 RID: 1316 RVA: 0x0000EE98 File Offset: 0x0000D098
		public event EventRouter.EventDelegate onBecameVisible;

		// Token: 0x06000525 RID: 1317 RVA: 0x0000EECD File Offset: 0x0000D0CD
		private void OnBecameInvisible()
		{
			if (this.onBecameInvisible != null)
			{
				this.onBecameInvisible(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000EEEE File Offset: 0x0000D0EE
		private void OnBecameVisible()
		{
			if (this.onBecameVisible != null)
			{
				this.onBecameVisible(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000527 RID: 1319 RVA: 0x0000EF10 File Offset: 0x0000D110
		// (remove) Token: 0x06000528 RID: 1320 RVA: 0x0000EF48 File Offset: 0x0000D148
		public event EventRouter.EventDelegate<ControllerColliderHit> onControllerColliderHit;

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000529 RID: 1321 RVA: 0x0000EF80 File Offset: 0x0000D180
		// (remove) Token: 0x0600052A RID: 1322 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public event EventRouter.EventDelegate<GameObject> onParticleCollision;

		// Token: 0x0600052B RID: 1323 RVA: 0x0000EFED File Offset: 0x0000D1ED
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (this.onControllerColliderHit != null)
			{
				this.onControllerColliderHit(new EventData<ControllerColliderHit>(hit, base.gameObject, this));
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000F00F File Offset: 0x0000D20F
		private void OnParticleCollision(GameObject other)
		{
			if (this.onParticleCollision != null)
			{
				this.onParticleCollision(new EventData<GameObject>(other, base.gameObject, this));
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600052D RID: 1325 RVA: 0x0000F034 File Offset: 0x0000D234
		// (remove) Token: 0x0600052E RID: 1326 RVA: 0x0000F06C File Offset: 0x0000D26C
		public event EventRouter.EventDelegate<Collision> onCollisionEnter;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x0600052F RID: 1327 RVA: 0x0000F0A4 File Offset: 0x0000D2A4
		// (remove) Token: 0x06000530 RID: 1328 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		public event EventRouter.EventDelegate<Collision> onCollisionExit;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000531 RID: 1329 RVA: 0x0000F114 File Offset: 0x0000D314
		// (remove) Token: 0x06000532 RID: 1330 RVA: 0x0000F14C File Offset: 0x0000D34C
		public event EventRouter.EventDelegate<Collision> onCollisionStay;

		// Token: 0x06000533 RID: 1331 RVA: 0x0000F181 File Offset: 0x0000D381
		private void OnCollisionEnter(Collision collisionInfo)
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter(new EventData<Collision>(collisionInfo, base.gameObject, this));
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000F1A3 File Offset: 0x0000D3A3
		private void OnCollisionExit(Collision collisionInfo)
		{
			if (this.onCollisionExit != null)
			{
				this.onCollisionExit(new EventData<Collision>(collisionInfo, base.gameObject, this));
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000F1C5 File Offset: 0x0000D3C5
		private void OnCollisionStay(Collision collisionInfo)
		{
			if (this.onCollisionStay != null)
			{
				this.onCollisionStay(new EventData<Collision>(collisionInfo, base.gameObject, this));
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000536 RID: 1334 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
		// (remove) Token: 0x06000537 RID: 1335 RVA: 0x0000F220 File Offset: 0x0000D420
		public event EventRouter.EventDelegate<Collision2D> onCollisionEnter2D;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000538 RID: 1336 RVA: 0x0000F258 File Offset: 0x0000D458
		// (remove) Token: 0x06000539 RID: 1337 RVA: 0x0000F290 File Offset: 0x0000D490
		public event EventRouter.EventDelegate<Collision2D> onCollisionExit2D;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x0600053A RID: 1338 RVA: 0x0000F2C8 File Offset: 0x0000D4C8
		// (remove) Token: 0x0600053B RID: 1339 RVA: 0x0000F300 File Offset: 0x0000D500
		public event EventRouter.EventDelegate<Collision2D> onCollisionStay2D;

		// Token: 0x0600053C RID: 1340 RVA: 0x0000F335 File Offset: 0x0000D535
		private void OnCollisionEnter2D(Collision2D collisionInfo)
		{
			if (this.onCollisionEnter2D != null)
			{
				this.onCollisionEnter2D(new EventData<Collision2D>(collisionInfo, base.gameObject, this));
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000F357 File Offset: 0x0000D557
		private void OnCollisionExit2D(Collision2D collisionInfo)
		{
			if (this.onCollisionExit2D != null)
			{
				this.onCollisionExit2D(new EventData<Collision2D>(collisionInfo, base.gameObject, this));
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000F379 File Offset: 0x0000D579
		private void OnCollisionStay2D(Collision2D collisionInfo)
		{
			if (this.onCollisionStay2D != null)
			{
				this.onCollisionStay2D(new EventData<Collision2D>(collisionInfo, base.gameObject, this));
			}
		}

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x0600053F RID: 1343 RVA: 0x0000F39C File Offset: 0x0000D59C
		// (remove) Token: 0x06000540 RID: 1344 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
		public event EventRouter.EventDelegate<Collider> onTriggerEnter;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000541 RID: 1345 RVA: 0x0000F40C File Offset: 0x0000D60C
		// (remove) Token: 0x06000542 RID: 1346 RVA: 0x0000F444 File Offset: 0x0000D644
		public event EventRouter.EventDelegate<Collider> onTriggerExit;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000543 RID: 1347 RVA: 0x0000F47C File Offset: 0x0000D67C
		// (remove) Token: 0x06000544 RID: 1348 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		public event EventRouter.EventDelegate<Collider> onTriggerStay;

		// Token: 0x06000545 RID: 1349 RVA: 0x0000F4E9 File Offset: 0x0000D6E9
		private void OnTriggerEnter(Collider other)
		{
			if (this.onTriggerEnter != null)
			{
				this.onTriggerEnter(new EventData<Collider>(other, base.gameObject, this));
			}
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000F50B File Offset: 0x0000D70B
		private void OnTriggerExit(Collider other)
		{
			if (this.onTriggerExit != null)
			{
				this.onTriggerExit(new EventData<Collider>(other, base.gameObject, this));
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000F52D File Offset: 0x0000D72D
		private void OnTriggerStay(Collider other)
		{
			if (this.onTriggerStay != null)
			{
				this.onTriggerStay(new EventData<Collider>(other, base.gameObject, this));
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000548 RID: 1352 RVA: 0x0000F550 File Offset: 0x0000D750
		// (remove) Token: 0x06000549 RID: 1353 RVA: 0x0000F588 File Offset: 0x0000D788
		public event EventRouter.EventDelegate<Collider2D> onTriggerEnter2D;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x0600054A RID: 1354 RVA: 0x0000F5C0 File Offset: 0x0000D7C0
		// (remove) Token: 0x0600054B RID: 1355 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		public event EventRouter.EventDelegate<Collider2D> onTriggerExit2D;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x0600054C RID: 1356 RVA: 0x0000F630 File Offset: 0x0000D830
		// (remove) Token: 0x0600054D RID: 1357 RVA: 0x0000F668 File Offset: 0x0000D868
		public event EventRouter.EventDelegate<Collider2D> onTriggerStay2D;

		// Token: 0x0600054E RID: 1358 RVA: 0x0000F69D File Offset: 0x0000D89D
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (this.onTriggerEnter2D != null)
			{
				this.onTriggerEnter2D(new EventData<Collider2D>(other, base.gameObject, this));
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000F6BF File Offset: 0x0000D8BF
		private void OnTriggerExit2D(Collider2D other)
		{
			if (this.onTriggerExit2D != null)
			{
				this.onTriggerExit2D(new EventData<Collider2D>(other, base.gameObject, this));
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000F6E1 File Offset: 0x0000D8E1
		private void OnTriggerStay2D(Collider2D other)
		{
			if (this.onTriggerStay2D != null)
			{
				this.onTriggerStay2D(new EventData<Collider2D>(other, base.gameObject, this));
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000551 RID: 1361 RVA: 0x0000F704 File Offset: 0x0000D904
		// (remove) Token: 0x06000552 RID: 1362 RVA: 0x0000F73C File Offset: 0x0000D93C
		public event Action<RenderTexture, RenderTexture> onRenderImage;

		// Token: 0x06000553 RID: 1363 RVA: 0x0000F771 File Offset: 0x0000D971
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.onRenderImage != null)
			{
				this.onRenderImage.Invoke(source, destination);
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000554 RID: 1364 RVA: 0x0000F788 File Offset: 0x0000D988
		// (remove) Token: 0x06000555 RID: 1365 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
		public event EventRouter.EventDelegate onDrawGizmos;

		// Token: 0x06000556 RID: 1366 RVA: 0x0000F7F5 File Offset: 0x0000D9F5
		private void OnDrawGizmos()
		{
			if (this.onDrawGizmos != null)
			{
				this.onDrawGizmos(new EventData(base.gameObject, this));
			}
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000557 RID: 1367 RVA: 0x0000F818 File Offset: 0x0000DA18
		// (remove) Token: 0x06000558 RID: 1368 RVA: 0x0000F850 File Offset: 0x0000DA50
		public event EventRouter.CustomEventDelegate onCustomEvent;

		// Token: 0x06000559 RID: 1369 RVA: 0x0000F885 File Offset: 0x0000DA85
		public void InvokeCustomEvent(string name, object value, object sender)
		{
			if (this.onCustomEvent != null)
			{
				this.onCustomEvent(name, new EventData(value, base.gameObject, sender));
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0000F8AD File Offset: 0x0000DAAD
		public void InvokeCustomEvent<T>(string name, T value, object sender)
		{
			if (this.onCustomEvent != null)
			{
				this.onCustomEvent(name, new EventData<T>(value, base.gameObject, sender));
			}
		}

		// Token: 0x04000181 RID: 385
		private EventRouterAnimatorMove _routerAnimatorMove;

		// Token: 0x02000121 RID: 289
		// (Invoke) Token: 0x0600082A RID: 2090
		public delegate void EventDelegate(EventData msg);

		// Token: 0x02000122 RID: 290
		// (Invoke) Token: 0x0600082E RID: 2094
		public delegate void EventDelegate<T>(EventData<T> msg);

		// Token: 0x02000123 RID: 291
		// (Invoke) Token: 0x06000832 RID: 2098
		public delegate void CustomEventDelegate(string name, IEventData data);
	}
}
