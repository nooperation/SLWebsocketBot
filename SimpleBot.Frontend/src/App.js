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
    }
  }
  
  componentDidMount() {
    this.refs.chat.AddChatMessage('First.Person', 'Hello world!', '2017-02-06T21:42:54.7443608-05:00', '00000000-0000-0000-0000-000000000000', 'Agent', null, '#');
    this.refs.chat.AddChatMessage('Second.Person', 'Another message from a different person', '2017-02-06T21:42:55.0000000-05:00', '00000000-0000-0000-0000-000000000001', 'Agent', null, '#'); 
  }

  render() {
    return (
      <div className="App">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
        <Chat title="Chat" ref="chat"/>
      </div>
    );
  }
}

export default App;
