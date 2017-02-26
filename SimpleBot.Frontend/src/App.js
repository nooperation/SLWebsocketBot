import React, { Component } from 'react';
//import SampleSession from './Samples.js';
import { Chat, DEFUALT_PROFILE_IMAGE } from './Chat.js';
import './App.css';
import 'bootstrap/dist/css/bootstrap.css';
import { TabItem } from './Tabs.js';
import { ChatTabs } from './ChatTabs.js';

const UUID_ZERO = '00000000-0000-0000-0000-000000000000';

class App extends Component {
  constructor() {
    super();

    this.state = {
      profiles: [],
      chat_sessions: {}
    }

    this.handleOnSendMessage = this.handleOnSendMessage.bind(this);
    this.handleOnTabClosed = this.handleOnTabClosed.bind(this);
    this.handleOnTabSelected = this.handleOnTabSelected.bind(this);

    this.startWebsocket = this.startWebsocket.bind(this);

    this.socket = null;

    setTimeout(this.startWebsocket, 1);
  }

  startWebsocket() {
    var instance = this;

    instance.addDebugMessage('Connecting...');
    instance.socket = new WebSocket('ws://127.0.0.1:55000/Bot');

    instance.socket.onopen = function (event) {
      instance.addDebugMessage('Connected');
    };

    instance.socket.onerror = function (event) {
    };

    instance.socket.onclose = function (event) {
      instance.addDebugMessage('Connection closed');
      setTimeout(instance.startWebsocket, 1000);
    };

    instance.socket.onmessage = function (event) {
      try {
        var message = JSON.parse(event.data);
        instance.handleNetworkMessage(message);
      }
      catch (ex) {
        instance.addDebugMessage('Failed to parse message.\n Exception: ' + ex + '\n Message: ' + event.data);
      }
    };
  };

  addDebugMessage(message) {
    this.addChatMessage("Debug", "SimpleBot", message, Date.now(), UUID_ZERO, 'Debug', '', '');
  }

  requestAvatarList() {
    if (!this.socket) {
      return;
    }

    var json_data = JSON.stringify({
      MessageType: 'AvatarListRequest',
      Payload: null
    });
    this.socket.send(json_data);
  }

  requestProfile(uuid) {
    if (!this.socket) {
      return;
    }

    var json_data = JSON.stringify({
      MessageType: 'ProfileRequest',
      Payload: {
        AgentId: uuid
      }
    });
    this.socket.send(json_data);
  }

  getProfileImage(uuid) {
    if (uuid in this.state.profiles) {
      return this.state.profiles[uuid].profile_image_url;
    }
    else {
      this.requestProfile(uuid);
      return DEFUALT_PROFILE_IMAGE;
    }
  }

  getOrCreateChatSessionInState(chat_session_name, state) {
    if (!(chat_session_name in state.chat_sessions)) {
      state.chat_sessions[chat_session_name] = {
        header: chat_session_name,
        last_read_message: 0,
        messages: [],
        users: []
      }
    }

    return state.chat_sessions[chat_session_name];
  }

  addChatMessage(chat_session_name, name, message, time, uuid, message_type, profile_image_url, profile_url) {
    profile_image_url = this.getProfileImage(uuid);

    this.setState(state => {
      var chat_session = this.getOrCreateChatSessionInState(chat_session_name, state);
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
      var chat_session = this.getOrCreateChatSessionInState(key, state);
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
    var json_data = JSON.stringify({
      MessageType: 'ChatRequest',
      Payload: {
        Message: message,
        ChatType: 'Normal',
        Channel: 0
      }
    });
    this.socket.send(json_data);
  }

  onChatMessage(message) {
    this.addChatMessage("Local Chat", message.FromName, message.Message, message.Time, message.OwnerId.Guid, message.MessageType, '', '');
  }

  onProfileResponse(message) {
    var profile_url;

    if (message.ProfileImage.Guid == UUID_ZERO) {
      profile_url = DEFUALT_PROFILE_IMAGE;
    }
    else {
      profile_url = 'http://texture-service.agni.lindenlab.com/' + message.ProfileImage.Guid + '/320x240.jpg';
    }

    this.addProfile(message.AgentId.Guid, {
      profile_image_url: profile_url
    });
  }

  onInstantMessage(message) {
    if (message.IM.Dialog === "MessageFromObject") {
      this.addChatMessage("Local Chat", message.IM.FromAgentName, '[' + message.IM.Dialog + '] ' + message.IM.Message, message.IM.Timestamp, message.IM.FromAgentID.Guid, message.IM.Dialog, '', '');
    }
    else {
      this.addChatMessage(message.IM.FromAgentName, message.IM.FromAgentName, '[' + message.IM.Dialog + '] ' +  message.IM.Message, message.IM.Timestamp, message.IM.FromAgentID.Guid, message.IM.Dialog, '', '');
    }
  }

  onInit(message) {
    this.requestAvatarList();
  }

  onAvatarListResponse(message) {
    this.setState(state => {
      var chat_session =  this.getOrCreateChatSessionInState("Local Chat", state);;
      const avatar_locations = message.AvatarLocations;
      for (var i = 0; i < avatar_locations.length; ++i) {
        chat_session.users.push({
          name: avatar_locations[i].FirstName + ' ' + avatar_locations[i].LastName,
          uuid: avatar_locations[i].Id,
          location: avatar_locations[i].Location
        });
      }
    });
  }

  handleNetworkMessage(message) {
    this.addDebugMessage(JSON.stringify(message));

    switch (message.MessageType) {
      case "Init":
        this.onInit(message);
        break;
      case "Chat":
        this.onChatMessage(message);
        break;
      case "ProfileResponse":
        this.onProfileResponse(message);
        break;
      case "InstantMessage":
        this.onInstantMessage(message);
        break;
      case "AvatarListResponse":
        this.onAvatarListResponse(message);
        break;
      default:
        break;
    }
  }

  componentDidMount() {
    // this.addDebugMessage("ComponentDidMount");

    // this.addChatMessage("Local Chat", 'First.Person', 'Hello world!', '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000000', 'Agent', null, '#');
    // this.addChatMessage("Local Chat", 'Second.Person', 'Another message from a different person', '2017-02-06T21:42:55.0000000-05:00', '00000000-0000-0000-0000-000000000001', 'Agent', null, '#');

    // this.selectTab("Local Chat");
    // var instance = this;
    // var spam_counter = 0;
    // setInterval(function () {
    //   instance.addChatMessage("Local Chat 2", 'Another.Person', spam_counter + ' | ' + Math.random(), '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000003', 'Agent', null, '#');
    //   ++spam_counter;
    // }, 1000);

    // var num_samples = SampleSession.length;
    // var current_sample = 0;
    // setInterval(function () {
    //   instance.handleNetworkMessage(SampleSession[current_sample]);
    //   ++current_sample
    //   if (current_sample >= num_samples) {
    //     current_sample = 0;
    //   }
    // }, 1000);

    // setTimeout(function () {
    //   instance.addProfile('00000000-0000-0000-0000-000000000001', {
    //     profile_image_url: 'http://texture-service.agni.lindenlab.com/89556747-24cb-43ed-920b-47caed15465f/320x240.jpg/',
    //   });
    // }, 1000);

  }

  selectTabInState(chat_session_name, state) {
    if (chat_session_name in state.chat_sessions) {
      var chat_session = this.getOrCreateChatSessionInState(chat_session_name, state);
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
      if (chat_session_name in state.chat_sessions) {
        delete state.chat_sessions[chat_session_name];
      }
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
        <ChatTabs onTabClosed={this.handleOnTabClosed} onTabSelected={this.handleOnTabSelected} selectedTab={this.state.selected_chat_session_name}>
          {
            Object.keys(this.state.chat_sessions).map(key => {
              const item = this.getOrCreateChatSessionInState(key, this.state);
              return (
                <TabItem header={item.header} lastReadMessage={item.last_read_message} totalMessages={item.messages.length} key={key}>
                  <Chat title={item.header} messages={item.messages} onSendMessage={this.handleOnSendMessage} users={item.users} />
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
