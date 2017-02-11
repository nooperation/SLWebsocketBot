import React, { Component } from 'react';
import SampleSession from './Samples.js';
import Chat from './Chat.js';
import './App.css';
import 'bootstrap/dist/css/bootstrap.css';


class App extends Component {
  constructor() {
    super();

    this.state = {
      socket: null,
      messages: [],
      profiles: [],
    }
  }

  addChatMessage(name, message, time, uuid, message_type, profile_image_url, profile_url) {
    this.setState((state) => {
      state.messages.push({
        name: name,
        message: message,
        date: time,
        uuid: uuid,
        message_type: message_type,
        profile_image_url: profile_image_url
      });
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
    setTimeout(function() {
      instance.setState((state) => {
        state.messages[1].profile_image_url = 'http://texture-service.agni.lindenlab.com/89556747-24cb-43ed-920b-47caed15465f/320x240.jpg/'
      });
    }, 1000);
  }

  render() {
    return (
      <div className="App">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
        <Chat title="Chat" messages={this.state.messages} onSendMessage={this.handleOnSendMessage}/>
      </div>
    );
  }
}

export default App;
