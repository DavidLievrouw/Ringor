const styles = require('./styles/site.less');

import * as React from "react";
import IFrame from 'react-iframe';
import { IApiUrlGetter } from '../services/ApiUrlGetter';

export interface IApiProps {
  apiUrlGetter: IApiUrlGetter;
}

export interface IApiState {
  error: string;
  isLoading: boolean;
  url: string;
  response: string;
}

export class Api extends React.Component<IApiProps, IApiState> {
  constructor(props: IApiProps) {
    super(props);

    this.state = {
      url: '/api',
      error: null,
      isLoading: false,
      response: null
    };

    this.handleUrlChange = this.handleUrlChange.bind(this);
    this.loadTrace = this.loadTrace.bind(this);
  }

  componentDidMount() {
    this.loadTrace();
  }

  loadTrace() {
    this.setState({
      response: null,
      isLoading: true,
      error: null
    });

    this.props.apiUrlGetter
      .get(this.state.url)
      .then(response => {
        this.setState({
          response: JSON.stringify(response),
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

  render() {
    let output = null;
    if (!this.state.isLoading && !this.state.error && this.state.response) {
      output =
        <IFrame
          className={`${styles['row']} ${styles['content']}`}
          position="relative"
          url="api" />;
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
                  <div className={`ui left icon ${this.state.isLoading ? 'disabled' : ''} input ${styles['stretched-horizontally']}`} style={{width:'100%'}}>
                    <input type="text" placeholder="Api URL..." value={this.state.url} onChange={this.handleUrlChange} />
                    <i className="globe icon"></i>
                  </div>
                </td>
                <td>
                  <div className={`ui right labeled icon ${this.state.isLoading ? 'loading' : ''} primary button`} onClick={this.loadTrace}>
                    <i className="down arrow icon"></i>
                    Show
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        {errorMessage}
        {output}
      </div>
    );
  }
}
