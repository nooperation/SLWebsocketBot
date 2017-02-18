import React, { Component } from 'react';
//import SampleSession from './Samples.js';
import Chat from './Chat.js';
import './App.css';
import 'bootstrap/dist/css/bootstrap.css';
import { TabItem } from './Tabs.js';
import { ChatTabs } from './ChatTabs.js';

const UUID_ZERO = '00000000-0000-0000-0000-000000000000';

class App extends Component {
  constructor() {
    super();

    this.state = {
      socket: null,
      profiles: [],
      chat_sessions: {}
    }

    this.handleOnSendMessage = this.handleOnSendMessage.bind(this);
    this.handleOnTabClosed = this.handleOnTabClosed.bind(this);
    this.handleOnTabSelected = this.handleOnTabSelected.bind(this);
  }

  addDebugMessage(message) {
    this.addChatMessage("Debug", "SimpleBot", message, Date.now(), UUID_ZERO, 'Debug', '', '');
  }

  addChatMessage(chat_session_name, name, message, time, uuid, message_type, profile_image_url, profile_url) {
    if (uuid in this.state.profiles) {
      profile_image_url = this.state.profiles[uuid].profile_image_url;
    }
    this.setState(state => {
      if (chat_session_name in state.chat_sessions === false) {
        state.chat_sessions[chat_session_name] = {
          header: chat_session_name,
          last_read_message: 0,
          messages: []
        }
      }

      var chat_session = state.chat_sessions[chat_session_name];
      chat_session.messages.push({
        name: name,
        message: message,
        date: time,
        uuid: uuid,
        message_type: message_type,
        profile_image_url: profile_image_url,
        revision: 0
      });

      if (!(state.selected_chat_session_name in state.chat_sessions)) {
        state.selected_chat_session_name = chat_session_name;
      }

      if (chat_session_name === state.selected_chat_session_name) {
        chat_session.last_read_message = chat_session.messages.length;
      }
    });
  }

  addProfile(uuid, profile) {
    this.setState(state => {
      state.profiles[uuid] = profile;
      this.updateMessagesInState(uuid, state);
    });
  }

  updateMessagesInState(uuid, state) {
    Object.keys(state.chat_sessions).map(key => {
      var chat_session = state.chat_sessions[key];
      var messages = chat_session.messages.slice(0);
      for (var i = 0; i < messages.length; ++i) {
        if (messages[i].uuid === uuid) {
          messages[i].profile_image_url = this.state.profiles[uuid].profile_image_url;
          messages[i].revision++;
        }
      }
      chat_session.messages = messages;
      return key;
    });
  }

  handleOnSendMessage(message) {
    alert('TODO: send "' + message + '"');
  }

  componentDidMount() {
    this.addDebugMessage("ComponentDidMount");

    for (var i = 0; i < 100; i += 2) {
      this.addChatMessage("Local Chat", 'First.Person', (i + 0) + '  Hello world!', '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000000', 'Agent', null, '#');
      this.addChatMessage("Local Chat", 'Second.Person', (i + 1) + '  Another message from a different person', '2017-02-06T21:42:55.0000000-05:00', '00000000-0000-0000-0000-000000000001', 'Agent', null, '#');
    }

    this.selectTab("Local Chat");
    var instance = this;
    var spam_counter = 0;
    setInterval(function () {
      instance.addChatMessage("Local Chat 2", 'Another.Person', spam_counter + ' | ' + Math.random(), '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000003', 'Agent', null, '#');
      ++spam_counter;
    }, 1000);

    setTimeout(function () {
      instance.addProfile('00000000-0000-0000-0000-000000000001', {
        profile_image_url: 'http://texture-service.agni.lindenlab.com/89556747-24cb-43ed-920b-47caed15465f/320x240.jpg/',
      });
    }, 1000);

  }

  selectTabInState(chat_session_name, state) {
    if (chat_session_name in state.chat_sessions) {
      var chat_session = state.chat_sessions[chat_session_name];
      chat_session.last_read_message = chat_session.messages.length;
    }

    state.selected_chat_session_name = chat_session_name;
  }

  selectTab(chat_session_name) {
    this.setState(state => {
      this.selectTabInState(chat_session_name, state);
    });
  }

  handleOnTabClosed(chat_session_name) {
    this.setState(state => {
      delete state.chat_sessions[chat_session_name];
      const chat_session_names = Object.keys(this.state.chat_sessions);
      const next_selected_chat_session_name = chat_session_names.length > 0 ? chat_session_names[0] : null;

      this.selectTabInState(next_selected_chat_session_name, state);
    });
  }

  handleOnTabSelected(chat_session_name) {
    this.setState(state => {
      this.selectTabInState(chat_session_name, state);
    });
  }

  render() {
    return (
      <div className="App">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
        <ChatTabs className="nav nav-tabs" onTabClosed={this.handleOnTabClosed} onTabSelected={this.handleOnTabSelected} selectedTab={this.state.selected_chat_session_name}>
          {
            Object.keys(this.state.chat_sessions).map(key => {
              const item = this.state.chat_sessions[key];
              return (
                <TabItem header={item.header} lastReadMessage={item.last_read_message} totalMessages={item.messages.length} key={key}>
                  <Chat title={item.header} messages={item.messages} onSendMessage={this.handleOnSendMessage} />
                </TabItem>
              );
            })
          }
        </ChatTabs>
      </div>
    );
  }
}

export default App;
