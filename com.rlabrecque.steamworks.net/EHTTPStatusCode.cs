using System;

namespace Steamworks
{
	// Token: 0x02000168 RID: 360
	public enum EHTTPStatusCode
	{
		// Token: 0x0400090A RID: 2314
		k_EHTTPStatusCodeInvalid,
		// Token: 0x0400090B RID: 2315
		k_EHTTPStatusCode100Continue = 100,
		// Token: 0x0400090C RID: 2316
		k_EHTTPStatusCode101SwitchingProtocols,
		// Token: 0x0400090D RID: 2317
		k_EHTTPStatusCode200OK = 200,
		// Token: 0x0400090E RID: 2318
		k_EHTTPStatusCode201Created,
		// Token: 0x0400090F RID: 2319
		k_EHTTPStatusCode202Accepted,
		// Token: 0x04000910 RID: 2320
		k_EHTTPStatusCode203NonAuthoritative,
		// Token: 0x04000911 RID: 2321
		k_EHTTPStatusCode204NoContent,
		// Token: 0x04000912 RID: 2322
		k_EHTTPStatusCode205ResetContent,
		// Token: 0x04000913 RID: 2323
		k_EHTTPStatusCode206PartialContent,
		// Token: 0x04000914 RID: 2324
		k_EHTTPStatusCode300MultipleChoices = 300,
		// Token: 0x04000915 RID: 2325
		k_EHTTPStatusCode301MovedPermanently,
		// Token: 0x04000916 RID: 2326
		k_EHTTPStatusCode302Found,
		// Token: 0x04000917 RID: 2327
		k_EHTTPStatusCode303SeeOther,
		// Token: 0x04000918 RID: 2328
		k_EHTTPStatusCode304NotModified,
		// Token: 0x04000919 RID: 2329
		k_EHTTPStatusCode305UseProxy,
		// Token: 0x0400091A RID: 2330
		k_EHTTPStatusCode307TemporaryRedirect = 307,
		// Token: 0x0400091B RID: 2331
		k_EHTTPStatusCode308PermanentRedirect,
		// Token: 0x0400091C RID: 2332
		k_EHTTPStatusCode400BadRequest = 400,
		// Token: 0x0400091D RID: 2333
		k_EHTTPStatusCode401Unauthorized,
		// Token: 0x0400091E RID: 2334
		k_EHTTPStatusCode402PaymentRequired,
		// Token: 0x0400091F RID: 2335
		k_EHTTPStatusCode403Forbidden,
		// Token: 0x04000920 RID: 2336
		k_EHTTPStatusCode404NotFound,
		// Token: 0x04000921 RID: 2337
		k_EHTTPStatusCode405MethodNotAllowed,
		// Token: 0x04000922 RID: 2338
		k_EHTTPStatusCode406NotAcceptable,
		// Token: 0x04000923 RID: 2339
		k_EHTTPStatusCode407ProxyAuthRequired,
		// Token: 0x04000924 RID: 2340
		k_EHTTPStatusCode408RequestTimeout,
		// Token: 0x04000925 RID: 2341
		k_EHTTPStatusCode409Conflict,
		// Token: 0x04000926 RID: 2342
		k_EHTTPStatusCode410Gone,
		// Token: 0x04000927 RID: 2343
		k_EHTTPStatusCode411LengthRequired,
		// Token: 0x04000928 RID: 2344
		k_EHTTPStatusCode412PreconditionFailed,
		// Token: 0x04000929 RID: 2345
		k_EHTTPStatusCode413RequestEntityTooLarge,
		// Token: 0x0400092A RID: 2346
		k_EHTTPStatusCode414RequestURITooLong,
		// Token: 0x0400092B RID: 2347
		k_EHTTPStatusCode415UnsupportedMediaType,
		// Token: 0x0400092C RID: 2348
		k_EHTTPStatusCode416RequestedRangeNotSatisfiable,
		// Token: 0x0400092D RID: 2349
		k_EHTTPStatusCode417ExpectationFailed,
		// Token: 0x0400092E RID: 2350
		k_EHTTPStatusCode4xxUnknown,
		// Token: 0x0400092F RID: 2351
		k_EHTTPStatusCode429TooManyRequests = 429,
		// Token: 0x04000930 RID: 2352
		k_EHTTPStatusCode444ConnectionClosed = 444,
		// Token: 0x04000931 RID: 2353
		k_EHTTPStatusCode500InternalServerError = 500,
		// Token: 0x04000932 RID: 2354
		k_EHTTPStatusCode501NotImplemented,
		// Token: 0x04000933 RID: 2355
		k_EHTTPStatusCode502BadGateway,
		// Token: 0x04000934 RID: 2356
		k_EHTTPStatusCode503ServiceUnavailable,
		// Token: 0x04000935 RID: 2357
		k_EHTTPStatusCode504GatewayTimeout,
		// Token: 0x04000936 RID: 2358
		k_EHTTPStatusCode505HTTPVersionNotSupported,
		// Token: 0x04000937 RID: 2359
		k_EHTTPStatusCode5xxUnknown = 599
	}
}
