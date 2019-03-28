export interface IWindow extends Window {
  trackingConsent: ITrackingConsent;
}
declare var window: IWindow;

export interface ITrackingConsent {
  showNag: boolean;
  cookieString: string;
}

const trackingConsent = (typeof window !== "undefined" && window.trackingConsent || {}) as ITrackingConsent;

export default trackingConsent;