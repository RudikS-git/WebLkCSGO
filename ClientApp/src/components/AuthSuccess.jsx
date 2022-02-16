import React, { Component } from 'react';
import { Redirect } from 'react-router';

export class AuthSuccess extends Component {
  displayName = AuthSuccess.name

  render() {
    
    const now = new Date();
    const utcTime = Date.UTC(now.getFullYear(),now.getMonth(), now.getDate() , 
    now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds()) / 1000;

    localStorage.setItem("getting-refresh", utcTime);

    return (
        <Redirect path="\"/>
    );
  }
}
