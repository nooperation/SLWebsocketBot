
import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import './Tabs.css';

export class TabItem extends Component {
  render() {
    return (
      <div className="tab-item-contents">{this.props.children}</div>
    );
  }
}

export class Tabs extends Component {
  constructor() {
    super();

    this.state = {
      active_child_index: 0
    };

    this.handleOnClick = this.handleOnClick.bind(this);
    this.handleCloseTab = this.handleCloseTab.bind(this);
  }

  handleOnClick(index) {
    this.setState(state => {
      state.active_child_index = index;
    });
  }

  handleCloseTab(index) {
    const item = this.props.children[index];
    this.props.onTabClosed(item.props.header);
  }

  render() {
    return (
      <div>
        <ul className="nav nav-tabs">
          {
            this.props.children.map((item, index) => {
              return (
                <li className="tab-header" className={(index === this.state.active_child_index) ? 'active' : 'inactive'} key={index}>
                  <a className="tab-header-title" href="#" onClick={() => this.handleOnClick(index)}>
                    {item.props.header}
                    <a href="#" onClick={() => this.handleCloseTab(index)}><i className="tab-header-close fa fa-times"></i></a>
                  </a>
                </li>
              )
            })
          }
        </ul>
        {this.props.children[this.state.active_child_index]}
      </div>
    );
  }
}
