import 'jspolyfill-array.prototype.findIndex'; // For IE11 compatibility
import { IUrlService } from './UrlService';

export interface IApiUrlPasteHandler {
  sanitizePastedUrl(pastedContent: string): string;
}

class ApiUrlPasteHandler implements IApiUrlPasteHandler {
  private urlService: IUrlService;

  constructor(urlService: IUrlService) {
    this.urlService = urlService;
  }

  sanitizePastedUrl(pastedContent: string): string {
    const applicationUrl = this.urlService.getApplicationUrl();

    if (pastedContent) {
      const sanitized = pastedContent.replace(/['"]+/g, '');
      const pattern = ApiUrlPasteHandler.trimByChar(applicationUrl, '/');
      const regex = new RegExp(pattern, "gi");
      return sanitized.replace(regex, '');
    }

    return pastedContent;
  }

  private static trimByChar(str: string, character: string) {
    const first = str.split('').findIndex(char => char !== character);
    const last = str.split('').reverse().findIndex(char => char !== character);
    return str.substring(first, str.length - last);
  }
}

export default ApiUrlPasteHandler;