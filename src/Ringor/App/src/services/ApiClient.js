class ApiClient {
  constructor(urlService) {
    this.urlService = urlService;
  }

  get(url, data, headers, mode = 'same-origin') {
    var query = [];
    for (var key in data) {
      query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
    }
    return fetch(url, {
      method: 'GET',
      mode: mode,
      headers: headers && new Headers(headers)
    });
  }

  post(url, data, headers, mode = 'same-origin') {
    var query = [];
    for (var key in data) {
      query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
    }
    return fetch(url, {
      method: 'POST',
      mode: mode,
      headers: headers && new Headers(headers),
      body: data && JSON.stringify(data)
    });
  }
}

export default ApiClient;