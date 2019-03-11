import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';

export interface ICompanyLogoProps {
  applicationInfo: IApplicationInfo;
}

export interface ICompanyLogoState { }

export class CompanyLogo extends React.Component<ICompanyLogoProps, ICompanyLogoState> {
  constructor(props: ICompanyLogoProps) {
    super(props);
  }

  render() {
    return (
      <div className="ui huge header">
        <div className="ui massive company">{this.props.applicationInfo.company}</div>
      </div>
    );
  }
}
