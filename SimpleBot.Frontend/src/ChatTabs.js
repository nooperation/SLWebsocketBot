import React from 'react';
import { Tabs } from './Tabs.js';
import 'bootstrap/dist/css/bootstrap.css';
import './Tabs.css';

export class ChatTabs extends Tabs {
  constructor() {
    super();

    this.state = {
      active_child_index: 0,
      message_stats: {
        last_read_message: 0,
        total_messages: 0
      }
    }
  }

  handleOnClick(index) {
    const item = this.props.children[index];
    var messages = item.props.children.props.messages;

    this.setState(state => {
      state.active_child_index = index;
      state.message_stats[item.props.header] = {
        last_read_message: messages.length,
        total_messages: messages.length
      }
    });
  }

  handleCloseTab(index) {
    const item = this.props.children[index];
    this.props.onTabClosed(item.props.header);
    this.setState(state => {
      delete state.message_stats[item.props.header];
    });
  }

  componentWillReceiveProps(nextProps) {
    const currently_selected_tab_key = nextProps.children[this.state.active_child_index].props.header;
    this.setState(state => {
      var new_message_stats = state.message_stats;

      nextProps.children.map(child => {
        var child_key = child.props.header;
        var child_messages = child.props.children.props.messages;
        if (!(child_key in new_message_stats)) {
          new_message_stats[child_key] = {
            last_read_message: 0,
            total_messages: 0
          };
        }
        if (child_key === currently_selected_tab_key) {
          new_message_stats[child_key].last_read_message = child_messages.length;
        }
        new_message_stats[child_key].total_messages = child_messages.length;
        return child;
      });

      state.message_stats = new_message_stats;
    });
  }

  render() {
    return (
      <div>
        <ul className="nav nav-tabs">
          {
            this.props.children.map((item, index) => {
              const item_message_stats = this.state.message_stats[item.props.header];
              const num_unread_messages = item_message_stats.total_messages - item_message_stats.last_read_message;
              var unread_messages_notification = null;
              var class_name = "tab-header ";

              if (num_unread_messages > 0) {
                unread_messages_notification = <span>{" (" + num_unread_messages + ") "}</span>
                class_name += " unread-messages";
              }

              if (index === this.state.active_child_index) {
                class_name += " active";
              }
              else {
                class_name += " inactive";
              }

              return (
                <li className={class_name} key={index}>
                  <a className="tab-header-title" href="#" onClick={() => this.handleOnClick(index)}>
                    {item.props.header}
                    {unread_messages_notification}
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
