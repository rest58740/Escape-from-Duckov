using System;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Triggers
{
	// Token: 0x0200006B RID: 107
	public static class AsyncTriggerExtensions
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0000A7C2 File Offset: 0x000089C2
		public static AsyncAwakeTrigger GetAsyncAwakeTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncAwakeTrigger>(gameObject);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000A7CA File Offset: 0x000089CA
		public static AsyncAwakeTrigger GetAsyncAwakeTrigger(this Component component)
		{
			return component.gameObject.GetAsyncAwakeTrigger();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000A7D7 File Offset: 0x000089D7
		public static AsyncDestroyTrigger GetAsyncDestroyTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDestroyTrigger>(gameObject);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000A7DF File Offset: 0x000089DF
		public static AsyncDestroyTrigger GetAsyncDestroyTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDestroyTrigger();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000A7EC File Offset: 0x000089EC
		public static AsyncStartTrigger GetAsyncStartTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncStartTrigger>(gameObject);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000A7F4 File Offset: 0x000089F4
		public static AsyncStartTrigger GetAsyncStartTrigger(this Component component)
		{
			return component.gameObject.GetAsyncStartTrigger();
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000A804 File Offset: 0x00008A04
		private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
		{
			T result;
			if (!gameObject.TryGetComponent<T>(ref result))
			{
				result = gameObject.AddComponent<T>();
			}
			return result;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000A823 File Offset: 0x00008A23
		public static UniTask OnDestroyAsync(this GameObject gameObject)
		{
			return gameObject.GetAsyncDestroyTrigger().OnDestroyAsync();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000A830 File Offset: 0x00008A30
		public static UniTask OnDestroyAsync(this Component component)
		{
			return component.GetAsyncDestroyTrigger().OnDestroyAsync();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000A83D File Offset: 0x00008A3D
		public static UniTask StartAsync(this GameObject gameObject)
		{
			return gameObject.GetAsyncStartTrigger().StartAsync();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000A84A File Offset: 0x00008A4A
		public static UniTask StartAsync(this Component component)
		{
			return component.GetAsyncStartTrigger().StartAsync();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000A857 File Offset: 0x00008A57
		public static UniTask AwakeAsync(this GameObject gameObject)
		{
			return gameObject.GetAsyncAwakeTrigger().AwakeAsync();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000A864 File Offset: 0x00008A64
		public static UniTask AwakeAsync(this Component component)
		{
			return component.GetAsyncAwakeTrigger().AwakeAsync();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000A871 File Offset: 0x00008A71
		public static AsyncFixedUpdateTrigger GetAsyncFixedUpdateTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncFixedUpdateTrigger>(gameObject);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000A879 File Offset: 0x00008A79
		public static AsyncFixedUpdateTrigger GetAsyncFixedUpdateTrigger(this Component component)
		{
			return component.gameObject.GetAsyncFixedUpdateTrigger();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000A886 File Offset: 0x00008A86
		public static AsyncLateUpdateTrigger GetAsyncLateUpdateTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncLateUpdateTrigger>(gameObject);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000A88E File Offset: 0x00008A8E
		public static AsyncLateUpdateTrigger GetAsyncLateUpdateTrigger(this Component component)
		{
			return component.gameObject.GetAsyncLateUpdateTrigger();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000A89B File Offset: 0x00008A9B
		public static AsyncAnimatorIKTrigger GetAsyncAnimatorIKTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncAnimatorIKTrigger>(gameObject);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000A8A3 File Offset: 0x00008AA3
		public static AsyncAnimatorIKTrigger GetAsyncAnimatorIKTrigger(this Component component)
		{
			return component.gameObject.GetAsyncAnimatorIKTrigger();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000A8B0 File Offset: 0x00008AB0
		public static AsyncAnimatorMoveTrigger GetAsyncAnimatorMoveTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncAnimatorMoveTrigger>(gameObject);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		public static AsyncAnimatorMoveTrigger GetAsyncAnimatorMoveTrigger(this Component component)
		{
			return component.gameObject.GetAsyncAnimatorMoveTrigger();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000A8C5 File Offset: 0x00008AC5
		public static AsyncApplicationFocusTrigger GetAsyncApplicationFocusTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncApplicationFocusTrigger>(gameObject);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000A8CD File Offset: 0x00008ACD
		public static AsyncApplicationFocusTrigger GetAsyncApplicationFocusTrigger(this Component component)
		{
			return component.gameObject.GetAsyncApplicationFocusTrigger();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000A8DA File Offset: 0x00008ADA
		public static AsyncApplicationPauseTrigger GetAsyncApplicationPauseTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncApplicationPauseTrigger>(gameObject);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000A8E2 File Offset: 0x00008AE2
		public static AsyncApplicationPauseTrigger GetAsyncApplicationPauseTrigger(this Component component)
		{
			return component.gameObject.GetAsyncApplicationPauseTrigger();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A8EF File Offset: 0x00008AEF
		public static AsyncApplicationQuitTrigger GetAsyncApplicationQuitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncApplicationQuitTrigger>(gameObject);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A8F7 File Offset: 0x00008AF7
		public static AsyncApplicationQuitTrigger GetAsyncApplicationQuitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncApplicationQuitTrigger();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000A904 File Offset: 0x00008B04
		public static AsyncAudioFilterReadTrigger GetAsyncAudioFilterReadTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncAudioFilterReadTrigger>(gameObject);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000A90C File Offset: 0x00008B0C
		public static AsyncAudioFilterReadTrigger GetAsyncAudioFilterReadTrigger(this Component component)
		{
			return component.gameObject.GetAsyncAudioFilterReadTrigger();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000A919 File Offset: 0x00008B19
		public static AsyncBecameInvisibleTrigger GetAsyncBecameInvisibleTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncBecameInvisibleTrigger>(gameObject);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000A921 File Offset: 0x00008B21
		public static AsyncBecameInvisibleTrigger GetAsyncBecameInvisibleTrigger(this Component component)
		{
			return component.gameObject.GetAsyncBecameInvisibleTrigger();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000A92E File Offset: 0x00008B2E
		public static AsyncBecameVisibleTrigger GetAsyncBecameVisibleTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncBecameVisibleTrigger>(gameObject);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000A936 File Offset: 0x00008B36
		public static AsyncBecameVisibleTrigger GetAsyncBecameVisibleTrigger(this Component component)
		{
			return component.gameObject.GetAsyncBecameVisibleTrigger();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000A943 File Offset: 0x00008B43
		public static AsyncBeforeTransformParentChangedTrigger GetAsyncBeforeTransformParentChangedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncBeforeTransformParentChangedTrigger>(gameObject);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000A94B File Offset: 0x00008B4B
		public static AsyncBeforeTransformParentChangedTrigger GetAsyncBeforeTransformParentChangedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncBeforeTransformParentChangedTrigger();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000A958 File Offset: 0x00008B58
		public static AsyncOnCanvasGroupChangedTrigger GetAsyncOnCanvasGroupChangedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncOnCanvasGroupChangedTrigger>(gameObject);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000A960 File Offset: 0x00008B60
		public static AsyncOnCanvasGroupChangedTrigger GetAsyncOnCanvasGroupChangedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncOnCanvasGroupChangedTrigger();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000A96D File Offset: 0x00008B6D
		public static AsyncCollisionEnterTrigger GetAsyncCollisionEnterTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCollisionEnterTrigger>(gameObject);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000A975 File Offset: 0x00008B75
		public static AsyncCollisionEnterTrigger GetAsyncCollisionEnterTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCollisionEnterTrigger();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000A982 File Offset: 0x00008B82
		public static AsyncCollisionEnter2DTrigger GetAsyncCollisionEnter2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCollisionEnter2DTrigger>(gameObject);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000A98A File Offset: 0x00008B8A
		public static AsyncCollisionEnter2DTrigger GetAsyncCollisionEnter2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCollisionEnter2DTrigger();
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A997 File Offset: 0x00008B97
		public static AsyncCollisionExitTrigger GetAsyncCollisionExitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCollisionExitTrigger>(gameObject);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A99F File Offset: 0x00008B9F
		public static AsyncCollisionExitTrigger GetAsyncCollisionExitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCollisionExitTrigger();
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000A9AC File Offset: 0x00008BAC
		public static AsyncCollisionExit2DTrigger GetAsyncCollisionExit2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCollisionExit2DTrigger>(gameObject);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		public static AsyncCollisionExit2DTrigger GetAsyncCollisionExit2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCollisionExit2DTrigger();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000A9C1 File Offset: 0x00008BC1
		public static AsyncCollisionStayTrigger GetAsyncCollisionStayTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCollisionStayTrigger>(gameObject);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000A9C9 File Offset: 0x00008BC9
		public static AsyncCollisionStayTrigger GetAsyncCollisionStayTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCollisionStayTrigger();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000A9D6 File Offset: 0x00008BD6
		public static AsyncCollisionStay2DTrigger GetAsyncCollisionStay2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCollisionStay2DTrigger>(gameObject);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000A9DE File Offset: 0x00008BDE
		public static AsyncCollisionStay2DTrigger GetAsyncCollisionStay2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCollisionStay2DTrigger();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000A9EB File Offset: 0x00008BEB
		public static AsyncControllerColliderHitTrigger GetAsyncControllerColliderHitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncControllerColliderHitTrigger>(gameObject);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000A9F3 File Offset: 0x00008BF3
		public static AsyncControllerColliderHitTrigger GetAsyncControllerColliderHitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncControllerColliderHitTrigger();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000AA00 File Offset: 0x00008C00
		public static AsyncDisableTrigger GetAsyncDisableTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDisableTrigger>(gameObject);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000AA08 File Offset: 0x00008C08
		public static AsyncDisableTrigger GetAsyncDisableTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDisableTrigger();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000AA15 File Offset: 0x00008C15
		public static AsyncDrawGizmosTrigger GetAsyncDrawGizmosTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDrawGizmosTrigger>(gameObject);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000AA1D File Offset: 0x00008C1D
		public static AsyncDrawGizmosTrigger GetAsyncDrawGizmosTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDrawGizmosTrigger();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000AA2A File Offset: 0x00008C2A
		public static AsyncDrawGizmosSelectedTrigger GetAsyncDrawGizmosSelectedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDrawGizmosSelectedTrigger>(gameObject);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000AA32 File Offset: 0x00008C32
		public static AsyncDrawGizmosSelectedTrigger GetAsyncDrawGizmosSelectedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDrawGizmosSelectedTrigger();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000AA3F File Offset: 0x00008C3F
		public static AsyncEnableTrigger GetAsyncEnableTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncEnableTrigger>(gameObject);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000AA47 File Offset: 0x00008C47
		public static AsyncEnableTrigger GetAsyncEnableTrigger(this Component component)
		{
			return component.gameObject.GetAsyncEnableTrigger();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000AA54 File Offset: 0x00008C54
		public static AsyncGUITrigger GetAsyncGUITrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncGUITrigger>(gameObject);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public static AsyncGUITrigger GetAsyncGUITrigger(this Component component)
		{
			return component.gameObject.GetAsyncGUITrigger();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000AA69 File Offset: 0x00008C69
		public static AsyncJointBreakTrigger GetAsyncJointBreakTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncJointBreakTrigger>(gameObject);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000AA71 File Offset: 0x00008C71
		public static AsyncJointBreakTrigger GetAsyncJointBreakTrigger(this Component component)
		{
			return component.gameObject.GetAsyncJointBreakTrigger();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000AA7E File Offset: 0x00008C7E
		public static AsyncJointBreak2DTrigger GetAsyncJointBreak2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncJointBreak2DTrigger>(gameObject);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000AA86 File Offset: 0x00008C86
		public static AsyncJointBreak2DTrigger GetAsyncJointBreak2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncJointBreak2DTrigger();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000AA93 File Offset: 0x00008C93
		public static AsyncMouseDownTrigger GetAsyncMouseDownTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseDownTrigger>(gameObject);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000AA9B File Offset: 0x00008C9B
		public static AsyncMouseDownTrigger GetAsyncMouseDownTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseDownTrigger();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000AAA8 File Offset: 0x00008CA8
		public static AsyncMouseDragTrigger GetAsyncMouseDragTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseDragTrigger>(gameObject);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public static AsyncMouseDragTrigger GetAsyncMouseDragTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseDragTrigger();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000AABD File Offset: 0x00008CBD
		public static AsyncMouseEnterTrigger GetAsyncMouseEnterTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseEnterTrigger>(gameObject);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000AAC5 File Offset: 0x00008CC5
		public static AsyncMouseEnterTrigger GetAsyncMouseEnterTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseEnterTrigger();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000AAD2 File Offset: 0x00008CD2
		public static AsyncMouseExitTrigger GetAsyncMouseExitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseExitTrigger>(gameObject);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000AADA File Offset: 0x00008CDA
		public static AsyncMouseExitTrigger GetAsyncMouseExitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseExitTrigger();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000AAE7 File Offset: 0x00008CE7
		public static AsyncMouseOverTrigger GetAsyncMouseOverTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseOverTrigger>(gameObject);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000AAEF File Offset: 0x00008CEF
		public static AsyncMouseOverTrigger GetAsyncMouseOverTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseOverTrigger();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000AAFC File Offset: 0x00008CFC
		public static AsyncMouseUpTrigger GetAsyncMouseUpTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseUpTrigger>(gameObject);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000AB04 File Offset: 0x00008D04
		public static AsyncMouseUpTrigger GetAsyncMouseUpTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseUpTrigger();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000AB11 File Offset: 0x00008D11
		public static AsyncMouseUpAsButtonTrigger GetAsyncMouseUpAsButtonTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMouseUpAsButtonTrigger>(gameObject);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000AB19 File Offset: 0x00008D19
		public static AsyncMouseUpAsButtonTrigger GetAsyncMouseUpAsButtonTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMouseUpAsButtonTrigger();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000AB26 File Offset: 0x00008D26
		public static AsyncParticleCollisionTrigger GetAsyncParticleCollisionTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncParticleCollisionTrigger>(gameObject);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000AB2E File Offset: 0x00008D2E
		public static AsyncParticleCollisionTrigger GetAsyncParticleCollisionTrigger(this Component component)
		{
			return component.gameObject.GetAsyncParticleCollisionTrigger();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000AB3B File Offset: 0x00008D3B
		public static AsyncParticleSystemStoppedTrigger GetAsyncParticleSystemStoppedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncParticleSystemStoppedTrigger>(gameObject);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000AB43 File Offset: 0x00008D43
		public static AsyncParticleSystemStoppedTrigger GetAsyncParticleSystemStoppedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncParticleSystemStoppedTrigger();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000AB50 File Offset: 0x00008D50
		public static AsyncParticleTriggerTrigger GetAsyncParticleTriggerTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncParticleTriggerTrigger>(gameObject);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000AB58 File Offset: 0x00008D58
		public static AsyncParticleTriggerTrigger GetAsyncParticleTriggerTrigger(this Component component)
		{
			return component.gameObject.GetAsyncParticleTriggerTrigger();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000AB65 File Offset: 0x00008D65
		public static AsyncParticleUpdateJobScheduledTrigger GetAsyncParticleUpdateJobScheduledTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncParticleUpdateJobScheduledTrigger>(gameObject);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000AB6D File Offset: 0x00008D6D
		public static AsyncParticleUpdateJobScheduledTrigger GetAsyncParticleUpdateJobScheduledTrigger(this Component component)
		{
			return component.gameObject.GetAsyncParticleUpdateJobScheduledTrigger();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000AB7A File Offset: 0x00008D7A
		public static AsyncPostRenderTrigger GetAsyncPostRenderTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPostRenderTrigger>(gameObject);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000AB82 File Offset: 0x00008D82
		public static AsyncPostRenderTrigger GetAsyncPostRenderTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPostRenderTrigger();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000AB8F File Offset: 0x00008D8F
		public static AsyncPreCullTrigger GetAsyncPreCullTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPreCullTrigger>(gameObject);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000AB97 File Offset: 0x00008D97
		public static AsyncPreCullTrigger GetAsyncPreCullTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPreCullTrigger();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000ABA4 File Offset: 0x00008DA4
		public static AsyncPreRenderTrigger GetAsyncPreRenderTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPreRenderTrigger>(gameObject);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000ABAC File Offset: 0x00008DAC
		public static AsyncPreRenderTrigger GetAsyncPreRenderTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPreRenderTrigger();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000ABB9 File Offset: 0x00008DB9
		public static AsyncRectTransformDimensionsChangeTrigger GetAsyncRectTransformDimensionsChangeTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncRectTransformDimensionsChangeTrigger>(gameObject);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000ABC1 File Offset: 0x00008DC1
		public static AsyncRectTransformDimensionsChangeTrigger GetAsyncRectTransformDimensionsChangeTrigger(this Component component)
		{
			return component.gameObject.GetAsyncRectTransformDimensionsChangeTrigger();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000ABCE File Offset: 0x00008DCE
		public static AsyncRectTransformRemovedTrigger GetAsyncRectTransformRemovedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncRectTransformRemovedTrigger>(gameObject);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000ABD6 File Offset: 0x00008DD6
		public static AsyncRectTransformRemovedTrigger GetAsyncRectTransformRemovedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncRectTransformRemovedTrigger();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000ABE3 File Offset: 0x00008DE3
		public static AsyncRenderImageTrigger GetAsyncRenderImageTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncRenderImageTrigger>(gameObject);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000ABEB File Offset: 0x00008DEB
		public static AsyncRenderImageTrigger GetAsyncRenderImageTrigger(this Component component)
		{
			return component.gameObject.GetAsyncRenderImageTrigger();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000ABF8 File Offset: 0x00008DF8
		public static AsyncRenderObjectTrigger GetAsyncRenderObjectTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncRenderObjectTrigger>(gameObject);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000AC00 File Offset: 0x00008E00
		public static AsyncRenderObjectTrigger GetAsyncRenderObjectTrigger(this Component component)
		{
			return component.gameObject.GetAsyncRenderObjectTrigger();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000AC0D File Offset: 0x00008E0D
		public static AsyncServerInitializedTrigger GetAsyncServerInitializedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncServerInitializedTrigger>(gameObject);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000AC15 File Offset: 0x00008E15
		public static AsyncServerInitializedTrigger GetAsyncServerInitializedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncServerInitializedTrigger();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000AC22 File Offset: 0x00008E22
		public static AsyncTransformChildrenChangedTrigger GetAsyncTransformChildrenChangedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTransformChildrenChangedTrigger>(gameObject);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000AC2A File Offset: 0x00008E2A
		public static AsyncTransformChildrenChangedTrigger GetAsyncTransformChildrenChangedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTransformChildrenChangedTrigger();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000AC37 File Offset: 0x00008E37
		public static AsyncTransformParentChangedTrigger GetAsyncTransformParentChangedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTransformParentChangedTrigger>(gameObject);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000AC3F File Offset: 0x00008E3F
		public static AsyncTransformParentChangedTrigger GetAsyncTransformParentChangedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTransformParentChangedTrigger();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000AC4C File Offset: 0x00008E4C
		public static AsyncTriggerEnterTrigger GetAsyncTriggerEnterTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTriggerEnterTrigger>(gameObject);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000AC54 File Offset: 0x00008E54
		public static AsyncTriggerEnterTrigger GetAsyncTriggerEnterTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTriggerEnterTrigger();
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000AC61 File Offset: 0x00008E61
		public static AsyncTriggerEnter2DTrigger GetAsyncTriggerEnter2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTriggerEnter2DTrigger>(gameObject);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000AC69 File Offset: 0x00008E69
		public static AsyncTriggerEnter2DTrigger GetAsyncTriggerEnter2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTriggerEnter2DTrigger();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000AC76 File Offset: 0x00008E76
		public static AsyncTriggerExitTrigger GetAsyncTriggerExitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTriggerExitTrigger>(gameObject);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000AC7E File Offset: 0x00008E7E
		public static AsyncTriggerExitTrigger GetAsyncTriggerExitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTriggerExitTrigger();
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000AC8B File Offset: 0x00008E8B
		public static AsyncTriggerExit2DTrigger GetAsyncTriggerExit2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTriggerExit2DTrigger>(gameObject);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000AC93 File Offset: 0x00008E93
		public static AsyncTriggerExit2DTrigger GetAsyncTriggerExit2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTriggerExit2DTrigger();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000ACA0 File Offset: 0x00008EA0
		public static AsyncTriggerStayTrigger GetAsyncTriggerStayTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTriggerStayTrigger>(gameObject);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000ACA8 File Offset: 0x00008EA8
		public static AsyncTriggerStayTrigger GetAsyncTriggerStayTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTriggerStayTrigger();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000ACB5 File Offset: 0x00008EB5
		public static AsyncTriggerStay2DTrigger GetAsyncTriggerStay2DTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncTriggerStay2DTrigger>(gameObject);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000ACBD File Offset: 0x00008EBD
		public static AsyncTriggerStay2DTrigger GetAsyncTriggerStay2DTrigger(this Component component)
		{
			return component.gameObject.GetAsyncTriggerStay2DTrigger();
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000ACCA File Offset: 0x00008ECA
		public static AsyncValidateTrigger GetAsyncValidateTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncValidateTrigger>(gameObject);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000ACD2 File Offset: 0x00008ED2
		public static AsyncValidateTrigger GetAsyncValidateTrigger(this Component component)
		{
			return component.gameObject.GetAsyncValidateTrigger();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000ACDF File Offset: 0x00008EDF
		public static AsyncWillRenderObjectTrigger GetAsyncWillRenderObjectTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncWillRenderObjectTrigger>(gameObject);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000ACE7 File Offset: 0x00008EE7
		public static AsyncWillRenderObjectTrigger GetAsyncWillRenderObjectTrigger(this Component component)
		{
			return component.gameObject.GetAsyncWillRenderObjectTrigger();
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public static AsyncResetTrigger GetAsyncResetTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncResetTrigger>(gameObject);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000ACFC File Offset: 0x00008EFC
		public static AsyncResetTrigger GetAsyncResetTrigger(this Component component)
		{
			return component.gameObject.GetAsyncResetTrigger();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000AD09 File Offset: 0x00008F09
		public static AsyncUpdateTrigger GetAsyncUpdateTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncUpdateTrigger>(gameObject);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000AD11 File Offset: 0x00008F11
		public static AsyncUpdateTrigger GetAsyncUpdateTrigger(this Component component)
		{
			return component.gameObject.GetAsyncUpdateTrigger();
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000AD1E File Offset: 0x00008F1E
		public static AsyncBeginDragTrigger GetAsyncBeginDragTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncBeginDragTrigger>(gameObject);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000AD26 File Offset: 0x00008F26
		public static AsyncBeginDragTrigger GetAsyncBeginDragTrigger(this Component component)
		{
			return component.gameObject.GetAsyncBeginDragTrigger();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000AD33 File Offset: 0x00008F33
		public static AsyncCancelTrigger GetAsyncCancelTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncCancelTrigger>(gameObject);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000AD3B File Offset: 0x00008F3B
		public static AsyncCancelTrigger GetAsyncCancelTrigger(this Component component)
		{
			return component.gameObject.GetAsyncCancelTrigger();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000AD48 File Offset: 0x00008F48
		public static AsyncDeselectTrigger GetAsyncDeselectTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDeselectTrigger>(gameObject);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000AD50 File Offset: 0x00008F50
		public static AsyncDeselectTrigger GetAsyncDeselectTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDeselectTrigger();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000AD5D File Offset: 0x00008F5D
		public static AsyncDragTrigger GetAsyncDragTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDragTrigger>(gameObject);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000AD65 File Offset: 0x00008F65
		public static AsyncDragTrigger GetAsyncDragTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDragTrigger();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000AD72 File Offset: 0x00008F72
		public static AsyncDropTrigger GetAsyncDropTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncDropTrigger>(gameObject);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000AD7A File Offset: 0x00008F7A
		public static AsyncDropTrigger GetAsyncDropTrigger(this Component component)
		{
			return component.gameObject.GetAsyncDropTrigger();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000AD87 File Offset: 0x00008F87
		public static AsyncEndDragTrigger GetAsyncEndDragTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncEndDragTrigger>(gameObject);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000AD8F File Offset: 0x00008F8F
		public static AsyncEndDragTrigger GetAsyncEndDragTrigger(this Component component)
		{
			return component.gameObject.GetAsyncEndDragTrigger();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000AD9C File Offset: 0x00008F9C
		public static AsyncInitializePotentialDragTrigger GetAsyncInitializePotentialDragTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncInitializePotentialDragTrigger>(gameObject);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		public static AsyncInitializePotentialDragTrigger GetAsyncInitializePotentialDragTrigger(this Component component)
		{
			return component.gameObject.GetAsyncInitializePotentialDragTrigger();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000ADB1 File Offset: 0x00008FB1
		public static AsyncMoveTrigger GetAsyncMoveTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncMoveTrigger>(gameObject);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000ADB9 File Offset: 0x00008FB9
		public static AsyncMoveTrigger GetAsyncMoveTrigger(this Component component)
		{
			return component.gameObject.GetAsyncMoveTrigger();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000ADC6 File Offset: 0x00008FC6
		public static AsyncPointerClickTrigger GetAsyncPointerClickTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPointerClickTrigger>(gameObject);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000ADCE File Offset: 0x00008FCE
		public static AsyncPointerClickTrigger GetAsyncPointerClickTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPointerClickTrigger();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000ADDB File Offset: 0x00008FDB
		public static AsyncPointerDownTrigger GetAsyncPointerDownTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPointerDownTrigger>(gameObject);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000ADE3 File Offset: 0x00008FE3
		public static AsyncPointerDownTrigger GetAsyncPointerDownTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPointerDownTrigger();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public static AsyncPointerEnterTrigger GetAsyncPointerEnterTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPointerEnterTrigger>(gameObject);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000ADF8 File Offset: 0x00008FF8
		public static AsyncPointerEnterTrigger GetAsyncPointerEnterTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPointerEnterTrigger();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000AE05 File Offset: 0x00009005
		public static AsyncPointerExitTrigger GetAsyncPointerExitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPointerExitTrigger>(gameObject);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000AE0D File Offset: 0x0000900D
		public static AsyncPointerExitTrigger GetAsyncPointerExitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPointerExitTrigger();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000AE1A File Offset: 0x0000901A
		public static AsyncPointerUpTrigger GetAsyncPointerUpTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncPointerUpTrigger>(gameObject);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000AE22 File Offset: 0x00009022
		public static AsyncPointerUpTrigger GetAsyncPointerUpTrigger(this Component component)
		{
			return component.gameObject.GetAsyncPointerUpTrigger();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000AE2F File Offset: 0x0000902F
		public static AsyncScrollTrigger GetAsyncScrollTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncScrollTrigger>(gameObject);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000AE37 File Offset: 0x00009037
		public static AsyncScrollTrigger GetAsyncScrollTrigger(this Component component)
		{
			return component.gameObject.GetAsyncScrollTrigger();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000AE44 File Offset: 0x00009044
		public static AsyncSelectTrigger GetAsyncSelectTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncSelectTrigger>(gameObject);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000AE4C File Offset: 0x0000904C
		public static AsyncSelectTrigger GetAsyncSelectTrigger(this Component component)
		{
			return component.gameObject.GetAsyncSelectTrigger();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000AE59 File Offset: 0x00009059
		public static AsyncSubmitTrigger GetAsyncSubmitTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncSubmitTrigger>(gameObject);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000AE61 File Offset: 0x00009061
		public static AsyncSubmitTrigger GetAsyncSubmitTrigger(this Component component)
		{
			return component.gameObject.GetAsyncSubmitTrigger();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000AE6E File Offset: 0x0000906E
		public static AsyncUpdateSelectedTrigger GetAsyncUpdateSelectedTrigger(this GameObject gameObject)
		{
			return AsyncTriggerExtensions.GetOrAddComponent<AsyncUpdateSelectedTrigger>(gameObject);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000AE76 File Offset: 0x00009076
		public static AsyncUpdateSelectedTrigger GetAsyncUpdateSelectedTrigger(this Component component)
		{
			return component.gameObject.GetAsyncUpdateSelectedTrigger();
		}
	}
}
