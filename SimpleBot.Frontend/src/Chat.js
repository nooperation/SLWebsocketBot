import React, { Component } from 'react';

class ChatItem extends Component {
  render() {
    return (
      <div className="chat-item chat-sourcetype-{this.props.sourceType}">
        <img src="http://texture-service.agni.lindenlab.com/4235acd5-6726-caa7-fe26-60c965992a63/320x240.jpg/" alt="user image" className="chat-item-image known-profile-image" data-uuid="{this.props.uuid}" />
        <div className="chat-item-message-container">
          <a className="chat-item-name" target="self" href={"https://my.secondlife.com/" + this.props.name}>{this.props.name}</a>
          <p className="chat-item-message">{this.props.message}</p>
        </div>  
        <small className="chat-item-extras">{this.props.date}</small>
      </div>
    )
  }
}

class ChatHeader extends Component {
  render() {
    return (
      <header className="chat-header">
        <i className="chat-header-icon fa fa-comments-o"></i>
        <h3 className="chat-header-title">Chat</h3>
      </header>
    );
  }
}

class ChatContent extends Component {
  render() {
    return (
      <article className="box-body chat chat-container-body">
        {
          this.props.messages.map(function (item) {
            return (
              <ChatItem name={item.name} message={item.message} uuid={item.uuid} sourceType={item.sourceType} date={item.date} />
            );
          })
        }
      </article>
    );
  }
}

class ChatInput extends Component {
  render() {
    return (
      <footer className="chat-container-footer">
        <form action="#" id="chat-form">
          <div className="input-group">
            <input id="chat-input" className="form-control" type="text" autocomplete="off" placeholder="Write your message here..." />
            <span className="input-group-btn">
              <button className="btn btn-default" type="submit">Send</button>
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

    this.state = {
      messages: [
        { name: 'First.Person', date: '2017-02-06T21:42:54.7443608-05:00', uuid: '00000000-0000-0000-0000-000000000000', sourceType: 'Agent', message: 'Hello world!'   },
        { name: 'Second.Person', date: '2017-02-06T21:42:54.7443608-05:00', uuid: '00000000-0000-0000-0000-000000000000', sourceType: 'Agent', message: 'This is another message'   },
      ]
    }
  }

  render() {
    return (
      <div className="chat-container wrapper">
        <ChatHeader />
        <ChatContent messages={this.state.messages} />
        <ChatInput />
      </div>
    );
  }  
}

export default Chat;
