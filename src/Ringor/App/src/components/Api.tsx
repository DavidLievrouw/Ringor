import * as React from "react";
import { IApiUrlGetter } from '../services/ApiUrlGetter';
import ReactJson from 'react-json-view';
import { IUrlService } from '../services/UrlService';
import { IApiUrlPasteHandler } from '../services/ApiUrlPasteHandler';
import { BigHeader } from './BigHeader';

export interface IApiProps {
  apiUrlGetter: IApiUrlGetter;
  urlService: IUrlService;
  apiUrlPasteHandler: IApiUrlPasteHandler;
}

export interface IApiState {
  applicationUrl: string;
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
      applicationUrl: props.urlService.getApplicationUrl(),
      requestCount: 0,
      clickedAwayPostmanMessage: false,
      url: '/api',
      error: null,
      isLoading: false,
      response: null
    };

    this.handleUrlChange = this.handleUrlChange.bind(this);
    this.handleKeyPress = this.handleKeyPress.bind(this);
    this.handlePaste = this.handlePaste.bind(this);
    this.handleFocus = this.handleFocus.bind(this);
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

  handlePaste(event: React.ClipboardEvent<HTMLInputElement>) {
    let pastedContent;
    if (event.clipboardData && event.clipboardData.types) {
      pastedContent = event.clipboardData.getData('text/plain');
    } else {
      // IE 11
      const theWindow = window as any;
      pastedContent = theWindow.clipboardData && theWindow.clipboardData.getData && theWindow.clipboardData.getData('Text');
    }
    
    const sanitizedPastedContent = this.props.apiUrlPasteHandler.sanitizePastedUrl(pastedContent);
    if (sanitizedPastedContent) {
      this.setState({
        url: sanitizedPastedContent,
        error: null
      });
      event.preventDefault();
    }
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

  handleFocus(event: React.FocusEvent<HTMLInputElement>) {
    event.target.select();
  }

  render() {
    let postmanMessage = null;
    if (!this.state.clickedAwayPostmanMessage && this.state.requestCount < 3 && !this.state.error) {
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
        <div className="row content">
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
      <div className="padded-content fillbox">
        <div className="row header">
          <BigHeader icon="file code" header="Navigate the API" />
          <table className="ui basic table">
            <tbody>
              <tr>
                <td className="stretched-horizontally">
                  <div className={`ui left icon ${this.state.isLoading ? 'disabled' : ''} input stretched-horizontally`}>
                    <input
                      type="text"
                      placeholder="Api URL..."
                      value={this.state.url}
                      onChange={this.handleUrlChange}
                      onKeyPress={this.handleKeyPress}
                      onPaste={this.handlePaste}
                      onFocus={this.handleFocus} />
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
