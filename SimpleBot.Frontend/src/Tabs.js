
import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.css';

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
  }

  handleOnClick(index) {
    this.setState(state => {
      state.active_child_index = index;
    });
  }

  render() {
    return (
      <div>
        <ul className="nav nav-tabs">
          {
            this.props.children.map((item, index) => {
              return (
                <li className={(index == this.state.active_child_index) ? 'active' : ''} key={index}>
                  <a href="#" onClick={() => this.handleOnClick(index)}>{item.props.header}</a>
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
