import React, { Component } from 'react';
import SampleSession from './Samples.js';
import Chat from './Chat.js';
import './App.css';
import 'bootstrap/dist/css/bootstrap.css';
import { Tabs, TabItem } from './Tabs.js';

class App extends Component {
  constructor() {
    super();

    this.state = {
      socket: null,
      messages: [],
      profiles: [],
    }

    this.handleOnSendMessage = this.handleOnSendMessage.bind(this);
  }

  addChatMessage(name, message, time, uuid, message_type, profile_image_url, profile_url) {
    if (uuid in this.state.profiles) {
      profile_image_url = this.state.profiles[uuid].profile_image_url;
    }
    this.setState((state) => {
      state.messages.push({
        name: name,
        message: message,
        date: time,
        uuid: uuid,
        message_type: message_type,
        profile_image_url: profile_image_url,
        revision: 0
      });
    });
  }

  addProfile(uuid, profile) {
    this.setState(state => {
      state.profiles[uuid] = profile;
      this.updateMessagesInState(uuid, state);
    });
  }

  updateMessagesInState(uuid, state) {
    var messages = state.messages.slice(0);
    for (var i = 0; i < messages.length; ++i) {
      if (messages[i].uuid === uuid) {
        messages[i].profile_image_url = this.state.profiles[uuid].profile_image_url;
        messages[i].revision++;
      }
    }
    state.messages = messages;
  }

  updateMessages(uuid) {
    this.setState(state => {
      this.updateMessagesInState(uuid, state);
    });
  }

  handleOnSendMessage(message) {
    alert('TODO: send "' + message + '"');
  }

  componentDidMount() {
    for (var i = 0; i < 100; i += 2) {
      this.addChatMessage('First.Person', (i + 0) + '  Hello world!', '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000000', 'Agent', null, '#');
      this.addChatMessage('Second.Person', (i + 1) + '  Another message from a different person', '2017-02-06T21:42:55.0000000-05:00', '00000000-0000-0000-0000-000000000001', 'Agent', null, '#');
    }

    var instance = this;
    var spam_counter = 0;
    setInterval(function () {
      instance.addChatMessage('Another.Person', spam_counter + ' | ' + Math.random(), '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000003', 'Agent', null, '#');
      ++spam_counter;
    }, 1000);

    setTimeout(function () {
      instance.addProfile('00000000-0000-0000-0000-000000000001', {
        profile_image_url: 'http://texture-service.agni.lindenlab.com/89556747-24cb-43ed-920b-47caed15465f/320x240.jpg/',
      });
    }, 1000);

  }

  render() {
    return (
      <div className="App">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
        <Tabs className="nav nav-tabs">
          <TabItem header="Local Chat">
            <Chat title="Local Chat" messages={this.state.messages} onSendMessage={this.handleOnSendMessage} />
          </TabItem>
          <TabItem header="Test 1">
            <Chat title="Null chat" messages={[]} />
          </TabItem>
          <TabItem header="Test 2">
            <h2>Test 2</h2>
          </TabItem>
        </Tabs>
      </div>
    );
  }
}

export default App;
