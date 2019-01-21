import React from 'react';
import composition from '../composition';
import "./styles/styles.less";

export class App extends React.Component {
  constructor(props) {
    super(props);
    this.apiClient = composition.services.apiClient;
    this.state = {
      callResult: null,
      isLoading: false
    };
  }

  callApiEndpoint(url) {
    this.setState({
      callResult: null,
      isLoading: true
    });

    this.apiClient.get(url, null)
      .then(result => {
        if (!result.ok) throw new Error(`Call to ${url} failed: ${result.status} - ${result.statusText}.`);
        return result.json();
      })
      .then(result => {
        this.setState({
          callResult: JSON.stringify(result, null, 2),
          isLoading: false
        });
      })
      .catch(failure => {
        this.setState({
          callResult: failure
            ? JSON.stringify(failure, Object.getOwnPropertyNames(failure), 2)
            : "[fail]",
          isLoading: false
        });
      });
  }

  render() {
    let callResultElement;
    if (this.state.isLoading) {
      callResultElement = <div>Calling Api...</div>;
    }
    if (this.state.callResult) {
      callResultElement = <div><textarea readOnly="readOnly" className="callResult" value={this.state.callResult} /></div>;
    }

    return (
      <div>
        <h1>Hi!</h1>
        <div className="container">
          <div className="item"><button onClick={() => this.callApiEndpoint('/api')}>Call the api home endpoint (unsecure)</button></div>
        </div>
        <div className="container">
          <div className="item">{callResultElement}</div>
        </div>
      </div>
    );
  }
}
