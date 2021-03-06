import React, { Component } from 'react';

export const DEFUALT_PROFILE_IMAGE = "http://texture-service.agni.lindenlab.com/4235acd5-6726-caa7-fe26-60c965992a63/320x240.jpg/";

class ChatItem extends Component {
  constructor() {
    super();

    this.revision = 0;
  }

  shouldComponentUpdate(nextProps, nextState) {
    if (nextProps.message.revision !== this.revision) {
      this.revision = nextProps.message.revision;
      return true;
    }

    return false;
  }

  render() {
    return (
      <div className={"chat-item chat-message-type-" + this.props.message.message_type }>
        <img src={this.props.message.profile_image_url || DEFUALT_PROFILE_IMAGE} alt="headshot" className="chat-item-image" />
        <div className="chat-item-message-container">
          <a className="chat-item-name" target="self" href={"https://my.secondlife.com/" + this.props.message.name.replace(' Resident', '').replace(' ', '.')}>{this.props.message.name}</a>
          <p className="chat-item-message">{this.props.message.message}</p>
        </div>
        <small className="chat-item-extras">{this.props.message.date}</small>
      </div>
    )
  }
}

class ChatHeader extends React.PureComponent {
  render() {
    return (
      <header className="chat-header">
        <i className="chat-header-icon fa fa-comments-o"></i>
        <h3 className="chat-header-title">{this.props.title}</h3>
      </header>
    );
  }
}

class UserList extends Component {
  render() {
    return (
      <div className="chat-container-userlist btn-group-vertical">
        {
          this.props.users.map(function (item, index) {
            return (
              <button type="button" className="btn btn-default chat-container-userlist-item" key={item.id}>{item.name}</button>
            );
          })
        }
      </div>
    );
  }
}

class ChatMessages extends Component {
  constructor() {
    super();

    this.handleOnScroll = this.handleOnScroll.bind(this);
    this.chat_list = null;
    this.is_autoscroll_enabled = true;
  }

  scrollToBottom() {
    this.chat_list.scrollTop = this.chat_list.scrollHeight;
  }

  componentDidMount() {
    this.scrollToBottom();
  }

  componentDidUpdate() {
    if (this.chat_list && this.is_autoscroll_enabled) {
      this.scrollToBottom();
    }
  }

  handleOnScroll(event) {
    const AUTOSCROLL_THRESHOLD = 5;

    const target = event.target;
    const diff = Math.abs(target.scrollTop - (target.scrollHeight - target.clientHeight));

    this.is_autoscroll_enabled =  diff < AUTOSCROLL_THRESHOLD
  }

  render() {
    return (
      <article id="foobar" className="box-body chat-container-messages" ref={chat_list => this.chat_list = chat_list} onScroll={this.handleOnScroll}>
        {
          this.props.messages.map(function (item, index) {
            return (
              <ChatItem key={index} message={item} />
            );
          })
        }
      </article>
    );
  }
}

class ChatInput extends React.PureComponent {
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
            <input className="form-control" type="text" autoComplete="off" placeholder="Write your message here..." value={this.state.message} onChange={this.handleChange}  />
            <span className="input-group-btn">
              <button className="btn btn-default" type="button" onClick={this.handleSubmit}>Send</button>
            </span>
          </div>
        </form>
      </footer>
    );
  }
}

export class Chat extends Component {

  render() {
    var users_list = null;
    if(this.props.users && this.props.users.length > 0)
    {
      users_list = <UserList users={this.props.users} />;
    }

    return (
      <div className="chat-container wrapper">
        <ChatHeader title={this.props.title} />
        <div className="chat-container-content">
          <ChatMessages messages={this.props.messages} />
          {users_list}
        </div>
        <ChatInput onSendMessage={this.props.onSendMessage}/>
      </div>
    );
  }
}
