import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { ApiInterceptor } from "./api.interceptor";
import { BaseUrlInterceptor } from "./base-url.interceptor";
import { ErrorInterceptor } from "./error.interceptor";
import { LoggingInterceptor } from "./loggin.interceptor";
import { NoopInterceptor } from "./noop.interceptor";

/** Http interceptor providers in outside-in order */
export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: NoopInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: BaseUrlInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: LoggingInterceptor, multi: true },
];

export * from "./api.interceptor";
export * from "./base-url.interceptor";
export * from "./error.interceptor";
export * from "./loggin.interceptor";
export * from "./noop.interceptor";

