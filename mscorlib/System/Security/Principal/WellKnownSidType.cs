using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004E9 RID: 1257
	[ComVisible(false)]
	public enum WellKnownSidType
	{
		// Token: 0x040022CD RID: 8909
		NullSid,
		// Token: 0x040022CE RID: 8910
		WorldSid,
		// Token: 0x040022CF RID: 8911
		LocalSid,
		// Token: 0x040022D0 RID: 8912
		CreatorOwnerSid,
		// Token: 0x040022D1 RID: 8913
		CreatorGroupSid,
		// Token: 0x040022D2 RID: 8914
		CreatorOwnerServerSid,
		// Token: 0x040022D3 RID: 8915
		CreatorGroupServerSid,
		// Token: 0x040022D4 RID: 8916
		NTAuthoritySid,
		// Token: 0x040022D5 RID: 8917
		DialupSid,
		// Token: 0x040022D6 RID: 8918
		NetworkSid,
		// Token: 0x040022D7 RID: 8919
		BatchSid,
		// Token: 0x040022D8 RID: 8920
		InteractiveSid,
		// Token: 0x040022D9 RID: 8921
		ServiceSid,
		// Token: 0x040022DA RID: 8922
		AnonymousSid,
		// Token: 0x040022DB RID: 8923
		ProxySid,
		// Token: 0x040022DC RID: 8924
		EnterpriseControllersSid,
		// Token: 0x040022DD RID: 8925
		SelfSid,
		// Token: 0x040022DE RID: 8926
		AuthenticatedUserSid,
		// Token: 0x040022DF RID: 8927
		RestrictedCodeSid,
		// Token: 0x040022E0 RID: 8928
		TerminalServerSid,
		// Token: 0x040022E1 RID: 8929
		RemoteLogonIdSid,
		// Token: 0x040022E2 RID: 8930
		LogonIdsSid,
		// Token: 0x040022E3 RID: 8931
		LocalSystemSid,
		// Token: 0x040022E4 RID: 8932
		LocalServiceSid,
		// Token: 0x040022E5 RID: 8933
		NetworkServiceSid,
		// Token: 0x040022E6 RID: 8934
		BuiltinDomainSid,
		// Token: 0x040022E7 RID: 8935
		BuiltinAdministratorsSid,
		// Token: 0x040022E8 RID: 8936
		BuiltinUsersSid,
		// Token: 0x040022E9 RID: 8937
		BuiltinGuestsSid,
		// Token: 0x040022EA RID: 8938
		BuiltinPowerUsersSid,
		// Token: 0x040022EB RID: 8939
		BuiltinAccountOperatorsSid,
		// Token: 0x040022EC RID: 8940
		BuiltinSystemOperatorsSid,
		// Token: 0x040022ED RID: 8941
		BuiltinPrintOperatorsSid,
		// Token: 0x040022EE RID: 8942
		BuiltinBackupOperatorsSid,
		// Token: 0x040022EF RID: 8943
		BuiltinReplicatorSid,
		// Token: 0x040022F0 RID: 8944
		BuiltinPreWindows2000CompatibleAccessSid,
		// Token: 0x040022F1 RID: 8945
		BuiltinRemoteDesktopUsersSid,
		// Token: 0x040022F2 RID: 8946
		BuiltinNetworkConfigurationOperatorsSid,
		// Token: 0x040022F3 RID: 8947
		AccountAdministratorSid,
		// Token: 0x040022F4 RID: 8948
		AccountGuestSid,
		// Token: 0x040022F5 RID: 8949
		AccountKrbtgtSid,
		// Token: 0x040022F6 RID: 8950
		AccountDomainAdminsSid,
		// Token: 0x040022F7 RID: 8951
		AccountDomainUsersSid,
		// Token: 0x040022F8 RID: 8952
		AccountDomainGuestsSid,
		// Token: 0x040022F9 RID: 8953
		AccountComputersSid,
		// Token: 0x040022FA RID: 8954
		AccountControllersSid,
		// Token: 0x040022FB RID: 8955
		AccountCertAdminsSid,
		// Token: 0x040022FC RID: 8956
		AccountSchemaAdminsSid,
		// Token: 0x040022FD RID: 8957
		AccountEnterpriseAdminsSid,
		// Token: 0x040022FE RID: 8958
		AccountPolicyAdminsSid,
		// Token: 0x040022FF RID: 8959
		AccountRasAndIasServersSid,
		// Token: 0x04002300 RID: 8960
		NtlmAuthenticationSid,
		// Token: 0x04002301 RID: 8961
		DigestAuthenticationSid,
		// Token: 0x04002302 RID: 8962
		SChannelAuthenticationSid,
		// Token: 0x04002303 RID: 8963
		ThisOrganizationSid,
		// Token: 0x04002304 RID: 8964
		OtherOrganizationSid,
		// Token: 0x04002305 RID: 8965
		BuiltinIncomingForestTrustBuildersSid,
		// Token: 0x04002306 RID: 8966
		BuiltinPerformanceMonitoringUsersSid,
		// Token: 0x04002307 RID: 8967
		BuiltinPerformanceLoggingUsersSid,
		// Token: 0x04002308 RID: 8968
		BuiltinAuthorizationAccessSid,
		// Token: 0x04002309 RID: 8969
		WinBuiltinTerminalServerLicenseServersSid,
		// Token: 0x0400230A RID: 8970
		MaxDefined = 60,
		// Token: 0x0400230B RID: 8971
		WinBuiltinDCOMUsersSid,
		// Token: 0x0400230C RID: 8972
		WinBuiltinIUsersSid,
		// Token: 0x0400230D RID: 8973
		WinIUserSid,
		// Token: 0x0400230E RID: 8974
		WinBuiltinCryptoOperatorsSid,
		// Token: 0x0400230F RID: 8975
		WinUntrustedLabelSid,
		// Token: 0x04002310 RID: 8976
		WinLowLabelSid,
		// Token: 0x04002311 RID: 8977
		WinMediumLabelSid,
		// Token: 0x04002312 RID: 8978
		WinHighLabelSid,
		// Token: 0x04002313 RID: 8979
		WinSystemLabelSid,
		// Token: 0x04002314 RID: 8980
		WinWriteRestrictedCodeSid,
		// Token: 0x04002315 RID: 8981
		WinCreatorOwnerRightsSid,
		// Token: 0x04002316 RID: 8982
		WinCacheablePrincipalsGroupSid,
		// Token: 0x04002317 RID: 8983
		WinNonCacheablePrincipalsGroupSid,
		// Token: 0x04002318 RID: 8984
		WinEnterpriseReadonlyControllersSid,
		// Token: 0x04002319 RID: 8985
		WinAccountReadonlyControllersSid,
		// Token: 0x0400231A RID: 8986
		WinBuiltinEventLogReadersGroup,
		// Token: 0x0400231B RID: 8987
		WinNewEnterpriseReadonlyControllersSid,
		// Token: 0x0400231C RID: 8988
		WinBuiltinCertSvcDComAccessGroup,
		// Token: 0x0400231D RID: 8989
		WinMediumPlusLabelSid,
		// Token: 0x0400231E RID: 8990
		WinLocalLogonSid,
		// Token: 0x0400231F RID: 8991
		WinConsoleLogonSid,
		// Token: 0x04002320 RID: 8992
		WinThisOrganizationCertificateSid,
		// Token: 0x04002321 RID: 8993
		WinApplicationPackageAuthoritySid,
		// Token: 0x04002322 RID: 8994
		WinBuiltinAnyPackageSid,
		// Token: 0x04002323 RID: 8995
		WinCapabilityInternetClientSid,
		// Token: 0x04002324 RID: 8996
		WinCapabilityInternetClientServerSid,
		// Token: 0x04002325 RID: 8997
		WinCapabilityPrivateNetworkClientServerSid,
		// Token: 0x04002326 RID: 8998
		WinCapabilityPicturesLibrarySid,
		// Token: 0x04002327 RID: 8999
		WinCapabilityVideosLibrarySid,
		// Token: 0x04002328 RID: 9000
		WinCapabilityMusicLibrarySid,
		// Token: 0x04002329 RID: 9001
		WinCapabilityDocumentsLibrarySid,
		// Token: 0x0400232A RID: 9002
		WinCapabilitySharedUserCertificatesSid,
		// Token: 0x0400232B RID: 9003
		WinCapabilityEnterpriseAuthenticationSid,
		// Token: 0x0400232C RID: 9004
		WinCapabilityRemovableStorageSid
	}
}
