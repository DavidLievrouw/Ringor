export interface IRequestSender {
  send(input: RequestInfo, init?: RequestInit): Promise<Response>;
}

class RequestSender implements IRequestSender {
  send(input: RequestInfo, init?: RequestInit): Promise<Response> {
    return fetch(input, init);
  }
}

export default RequestSender;