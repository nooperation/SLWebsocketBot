import React from 'react';
import { Tabs } from './Tabs.js';
import 'bootstrap/dist/css/bootstrap.css';
import './Tabs.css';

export class ChatTabs extends Tabs {
  render() {
    var selected_child = null;
    const selected_tab_key = this.props.selectedTab;

    return (
      <div>
        <ul className="nav nav-tabs">
          {
            this.props.children.map((item, index) => {
              const num_unread_messages = item.props.totalMessages - item.props.lastReadMessage;
              var unread_messages_notification = null;
              var class_name = "tab-header ";

              if (num_unread_messages > 0) {
                unread_messages_notification = <span>{" (" + num_unread_messages + ") "}</span>
                class_name += " unread-messages";
              }

              if (item.key === selected_tab_key) {
                class_name += " active";
                selected_child = item;
              }
              else {
                class_name += " inactive";
              }

              return (
                <li className={class_name} key={index}>
                  <a className="tab-header-title" href="#" onClick={() => this.props.onTabSelected(item.key)}>
                    {item.props.header}
                    {unread_messages_notification}
                    <i href="#" onClick={(event) => { this.props.onTabClosed(item.key); event.stopPropagation(); }}><i className="tab-header-close fa fa-times"></i></i>
                  </a>
                </li>
              )
            })
          }
        </ul>
        {selected_child}
      </div>
    );
  }
}
