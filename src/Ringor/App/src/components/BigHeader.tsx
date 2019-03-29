import * as React from "react";

export interface IBigHeaderProps {
  icon: string;
  header: string;
  subHeader?: string;
}

export interface IBigHeaderState { }

export class BigHeader extends React.Component<IBigHeaderProps, IBigHeaderState> {
  constructor(props: IBigHeaderProps) {
    super(props);
  }

  render() {
    let subHeader = null;
    if (this.props.subHeader) {
      subHeader = <div className="sub header">{this.props.subHeader}</div>;
    }

    let iconClass = "";
    let icon = null;
    if (this.props.icon) {
      iconClass = "icon";
      icon = <i className={`${this.props.icon} icon center`} />;
    }

    return (
      <h1 className={`ui ${iconClass} center aligned header`}>
        {icon}
        <div className="content">{this.props.header}</div>
        {subHeader}
      </h1>
    );
  }
}
