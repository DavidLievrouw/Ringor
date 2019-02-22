import "isomorphic-fetch"; // For IE11 compatibility

export interface IRequestSender {
  send(input: RequestInfo, init?: RequestInit): Promise<Response>;
}

class RequestSender implements IRequestSender {
  send(input: RequestInfo, init?: RequestInit): Promise<Response> {
    return fetch(input, init);
  }
}

export default RequestSender;