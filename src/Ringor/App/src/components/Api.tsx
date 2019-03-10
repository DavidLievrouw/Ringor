const styles = require('./styles/site.less');

import * as React from "react";
import { IApiUrlGetter } from '../services/ApiUrlGetter';
import ReactJson from 'react-json-view';

export interface IApiProps {
  apiUrlGetter: IApiUrlGetter;
}

export interface IApiState {
  requestCount: number;
  clickedAwayPostmanMessage: boolean;
  error: string;
  isLoading: boolean;
  url: string;
  response: Object;
}

export class Api extends React.Component<IApiProps, IApiState> {
  constructor(props: IApiProps) {
    super(props);

    this.state = {
      requestCount: 0,
      clickedAwayPostmanMessage: false,
      url: '/api',
      error: null,
      isLoading: false,
      response: null
    };

    this.handleUrlChange = this.handleUrlChange.bind(this);
    this.handleKeyPress = this.handleKeyPress.bind(this);
    this.clickAwayPostmanMessage = this.clickAwayPostmanMessage.bind(this);
    this.getUrl = this.getUrl.bind(this);
  }

  componentDidMount() {
    this.getUrl();
  }

  clickAwayPostmanMessage() {
    this.setState({
      clickedAwayPostmanMessage: true
    });
  }

  getUrl() {
    this.setState({
      response: null,
      isLoading: true,
      error: null,
      requestCount: this.state.requestCount + 1
    });

    this.props.apiUrlGetter
      .get(this.state.url)
      .then(response => {
        this.setState({
          response: response,
          isLoading: false,
          error: null
        });
      })
      .catch(failure => {
        this.setState({
          response: null,
          isLoading: false,
          error: failure && failure.message
            ? failure.message
            : "Unknown error"
        });
      });
  }

  handleUrlChange(event: React.ChangeEvent<HTMLInputElement>) {
    const newValue = event.target.value;
    this.setState({
      url: newValue,
      error: null
    });
  }

  handleKeyPress(event: React.KeyboardEvent<HTMLInputElement>) {
    if (event.key === 'Enter') {
      this.getUrl();
    }
  }

  render() {
    let postmanMessage = null;
    if (!this.state.clickedAwayPostmanMessage && this.state.requestCount < 3  && !this.state.error) {
      postmanMessage =
        <tr>
          <td colSpan={2}>
            <div className="ui info message">
              <i className="close icon" onClick={this.clickAwayPostmanMessage}></i>
              <div className="header">
                Want a better experience?
              </div>
              <a href="https://www.getpostman.com" target="_blank" onClick={this.clickAwayPostmanMessage}>Get Postman</a>
            </div>
          </td>
        </tr>;
    }

    let output = null;
    if (!this.state.isLoading && !this.state.error && this.state.response) {
      output =
        <div className={`${styles['row']} ${styles['content']}`}>
          <div className="ui basic segment">
            <ReactJson
              src={this.state.response}
              theme="monokai"
              collapsed={false}
              displayDataTypes={false}
              onAdd={false}
              onEdit={false}
              onDelete={false}
              collapseStringsAfterLength={100} />
          </div>
        </div>;
    }

    let errorMessage = null;
    if (this.state.error) {
      errorMessage = <div className="ui error message">
        <div className="header">Failed</div>
        <p>{this.state.error}</p>
      </div>
    }

    return (
      <div className={`${styles['padded-content']} ${styles['fillbox']}`}>
        <div className={`${styles['row']} ${styles['header']}`}>
          <table className="ui basic table">
            <tbody>
              <tr>
                <td className={`${styles['stretched-horizontally']}`}>
                  <div className={`ui left icon ${this.state.isLoading ? 'disabled' : ''} input ${styles['stretched-horizontally']}`}>
                    <input type="text" placeholder="Api URL..." value={this.state.url} onChange={this.handleUrlChange} onKeyPress={this.handleKeyPress} />
                    <i className="globe icon"></i>
                  </div>
                </td>
                <td>
                  <div className={`ui right labeled icon ${this.state.isLoading ? 'loading' : ''} primary button`} onClick={this.getUrl}>
                    <i className="down arrow icon"></i>
                    Show
                  </div>
                </td>
              </tr>
              {postmanMessage}
            </tbody>
          </table>
        </div>
        {errorMessage}
        {output}
      </div>
    );
  }
}
