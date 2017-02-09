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
  
  render() {
    return (
      <div className="App">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css" />
        <Chat />
      </div>
    );
  }
}

export default App;
