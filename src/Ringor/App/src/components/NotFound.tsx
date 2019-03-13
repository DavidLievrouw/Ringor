import * as React from "react";
import { IUrlService } from '../services/UrlService';

export interface INotFoundProps {
  urlService: IUrlService;
  path: string;
  query: string;
}

export interface INotFoundState {}

export class NotFound extends React.Component<INotFoundProps, INotFoundState> {
  constructor(props: INotFoundProps) {
    super(props);
  }

  componentWillMount() {
    const encodedPath = encodeURIComponent(this.props.path || '');
    const encodedQuery = encodeURIComponent(this.props.query || '');
    const url = this.props.urlService.getAbsoluteUrl('/error/404?path=' + encodedPath + '&query=' + encodedQuery);
    window.location.href = url;
  }

  render() {    
    return <div />
  }
}
