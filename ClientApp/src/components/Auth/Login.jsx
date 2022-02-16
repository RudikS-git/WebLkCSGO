import React, { Component } from 'react';
import './Login.css';
import { domain } from './../../configureFetch';
import { Link } from 'react-router-dom';

export const Login = (props) => {

    return (
        <a onClick={() => document.location = `${domain}/api/account/externallogin`} href={`${domain}/api/account/externallogin`} className="login__button" target="_self">
            <i class="fa fa-steam login__icon-steam" aria-hidden="true"></i>
            <span className="login__text">Войти через STEAM</span>
        </a>
    );
    
}
  