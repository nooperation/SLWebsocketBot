import React, { Component } from 'react';
import SampleSession from './Samples.js';

const DEFUALT_PROFILE_IMAGE = "http://texture-service.agni.lindenlab.com/4235acd5-6726-caa7-fe26-60c965992a63/320x240.jpg/";

class ChatItem extends Component {
  render() {
    return (
      <div className={"chat-item chat-message-type-" + this.props.message.message_type }>
        <img src={this.props.message.profile_image_url || DEFUALT_PROFILE_IMAGE} alt="user image" className="chat-item-image" />
        <div className="chat-item-message-container">
          <a className="chat-item-name" target="self" href={"https://my.secondlife.com/" + this.props.message.name}>{this.props.message.name}</a>
          <p className="chat-item-message">{this.props.message.message}</p>
        </div>  
        <small className="chat-item-extras">{this.props.message.date}</small>
      </div>
    )
  }
}

class ChatHeader extends Component {
  render() {
    return (
      <header className="chat-header">
        <i className="chat-header-icon fa fa-comments-o"></i>
        <h3 className="chat-header-title">{this.props.title}</h3>
      </header>
    );
  }
}

class ChatContent extends Component {
  render() {
    return (
      <article className="box-body chat-container-body">
        {
          this.props.messages.map(function (item) {
            return (
              <ChatItem message={item} />
            );
          })
        }
      </article>
    );
  }
}

class ChatInput extends Component {
  constructor() {
    super();
    this.state = {
      message: ''
    }

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(event) {
    this.setState({
      message: event.target.value
    });
  }

  handleSubmit(event) {
    event.preventDefault();
    if (this.props.onSendMessage) {
      this.props.onSendMessage(this.state.message);
    }  
    this.setState({
      message: ''
    });
  }

  render() {
    return (
      <footer className="chat-container-footer">
        <form onSubmit={this.handleSubmit} action="#">
          <div className="input-group">
            <input className="form-control" type="text" autocomplete="off" placeholder="Write your message here..." value={this.state.message} onChange={this.handleChange}  />
            <span className="input-group-btn">
              <button className="btn btn-default" type="button" onClick={this.handleSubmit}>Send</button>
            </span>
          </div>
        </form>
      </footer>
    );
  } 
}

class Chat extends Component {
  constructor() {
    super();

    this.is_updating_state = false;
    this.message_queue = [];
    this.state = {
      messages: [],
    }
  }

  AddChatMessage(name, message, time, uuid, message_type, profile_image_url, profile_url) {
    this.message_queue.push({
      name: name,
      message: message,
      date: time,
      uuid: uuid,
      message_type: message_type,
      profile_image_url: profile_image_url
    });
    this.PollMessageQueue();
  }

  PollMessageQueue() {
    if (this.is_updating_state == false && this.message_queue.length > 0) {
      this.is_updating_state = true;

      const old_messages = this.state.messages.slice();
      const new_messages = this.message_queue.splice(0);
      const new_state = {
        messages: old_messages.concat(new_messages)
      }
      this.setState(new_state, this.OnMessageStateUpdated);
    }
  }

  OnMessageStateUpdated() {
    this.is_updating_state = false;
    this.PollMessageQueue();
  }

  handleOnSendMessage(message) {
    alert('TODO: send "' + message + '"');
  }

  render() {
    return (
      <div className="chat-container wrapper">
        <ChatHeader title={this.props.title} />
        <ChatContent messages={this.state.messages} />
        <ChatInput onSendMessage={this.handleOnSendMessage}/>
      </div>
    );
  }
}

export default Chat;
