import React, { Component } from 'react';
import {Route, NavLink, HashRouter} from 'react-router-dom'

import 'bootstrap/dist/css/bootstrap.css';

import './NavMenu.css';
import { AccountInfo } from '../Auth/AccountInfo';
import { Login } from '../Auth/Login';
import { STEAM_PROFILE_LINK } from '../../CONST';

export class NavMenu extends Component
{
    displayName = NavMenu.name;

    constructor(props) {
        super(props);
        this.state = { accountinfo: "", loading: true, isLoggedIn: false };
    }

    handleClickMenu(){
      document.querySelector('.header__burger').classList.toggle('active');
      document.querySelector('.navbar__nav').classList.toggle('active');
      document.querySelector('.layout__additional-info').classList.remove('active');
      document.querySelector('body').classList.toggle('lockMenu')
      document.querySelector('body').classList.remove('lockAcc')
    }  

    handleClickAccount(){
      document.querySelector('.layout__additional-info').classList.toggle('active');
      document.querySelector('.header__burger').classList.remove('active');
      document.querySelector('.navbar__nav').classList.remove('active');
      document.querySelector('body').classList.remove('lockMenu')
      document.querySelector('body').classList.toggle('lockAcc')
    }

  render() {

    return (
        <nav className="navbar navbar-expand-sm menu Navul" >

          <div className="navmenu__item_logo">
              <NavLink className="img-fluid" to="/">
                  <img className="navmenu__logo" src="images/logo.png"></img>
              </NavLink>
          </div>

          <div onClick={this.handleClickMenu} className="header__burger">
            <span></span>
          </div>

          <ul className="navbar__nav nav-pills Navul"> 
                <NavLink onClick={this.handleClickMenu} className="nav__link NavMenuRoute textNav icon-nav-link" to="/">
                  <li className="nav-item active">
                    <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-house-door nav-icon" fill="white" xmlns="http://www.w3.org/2000/svg">
                      <path fillRule="evenodd" d="M7.646 1.146a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1 .146.354v7a.5.5 0 0 1-.5.5H9.5a.5.5 0 0 1-.5-.5v-4H7v4a.5.5 0 0 1-.5.5H2a.5.5 0 0 1-.5-.5v-7a.5.5 0 0 1 .146-.354l6-6zM2.5 7.707V14H6v-4a.5.5 0 0 1 .5-.5h3a.5.5 0 0 1 .5.5v4h3.5V7.707L8 2.207l-5.5 5.5z"/>
                      <path fillRule="evenodd" d="M13 2.5V6l-2-2V2.5a.5.5 0 0 1 .5-.5h1a.5.5 0 0 1 .5.5z"/>
                    </svg>
                    Главная
                </li>
              </NavLink>

              <NavLink onClick={this.handleClickMenu} className="nav__link NavMenuRoute textNav icon-nav-link" to="/payment">
                <li className="nav-item active">
                  <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-wallet-fill nav-icon" fill="white" xmlns="http://www.w3.org/2000/svg">
                      <path d="M1.5 2A1.5 1.5 0 0 0 0 3.5v2h6a.5.5 0 0 1 .5.5c0 .253.08.644.306.958.207.288.557.542 1.194.542.637 0 .987-.254 1.194-.542.226-.314.306-.705.306-.958a.5.5 0 0 1 .5-.5h6v-2A1.5 1.5 0 0 0 14.5 2h-13z"/>
                      <path d="M16 6.5h-5.551a2.678 2.678 0 0 1-.443 1.042C9.613 8.088 8.963 8.5 8 8.5c-.963 0-1.613-.412-2.006-.958A2.679 2.679 0 0 1 5.551 6.5H0v6A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-6z"/>
                    </svg>
                  Донат
                </li>
              </NavLink>

              <NavLink onClick={this.handleClickMenu} className="nav__link NavMenuRoute textNav icon-nav-link" to="/banlist"> 
                <li className="nav-item active">
                <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-table nav-icon" fill="white" xmlns="http://www.w3.org/2000/svg">
                  <path fillRule="evenodd" d="M0 2a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V2zm15 2h-4v3h4V4zm0 4h-4v3h4V8zm0 4h-4v3h3a1 1 0 0 0 1-1v-2zm-5 3v-3H6v3h4zm-5 0v-3H1v2a1 1 0 0 0 1 1h3zm-4-4h4V8H1v3zm0-4h4V4H1v3zm5-3v3h4V4H6zm4 4H6v3h4V8z"/>
                </svg>
                  Бан лист
                </li>
              </NavLink>

                <NavLink onClick={this.handleClickMenu} className="nav__link NavMenuRoute textNav icon-nav-link" to="/commslist">
                <li className="nav-item active">
                    <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-mic-mute-fill nav-icon" fill="white" xmlns="http://www.w3.org/2000/svg">
                    <path fillRule="evenodd" d="M12.734 9.613A4.995 4.995 0 0 0 13 8V7a.5.5 0 0 0-1 0v1c0 .274-.027.54-.08.799l.814.814zm-2.522 1.72A4 4 0 0 1 4 8V7a.5.5 0 0 0-1 0v1a5 5 0 0 0 4.5 4.975V15h-3a.5.5 0 0 0 0 1h7a.5.5 0 0 0 0-1h-3v-2.025a4.973 4.973 0 0 0 2.43-.923l-.718-.719zM11 7.88V3a3 3 0 0 0-5.842-.963L11 7.879zM5 6.12l4.486 4.486A3 3 0 0 1 5 8V6.121zm8.646 7.234l-12-12 .708-.708 12 12-.708.707z"/>
                  </svg>
                  Мут лист
                </li>
                </NavLink>

                <NavLink onClick={this.handleClickMenu} className="nav__link NavMenuRoute textNav icon-nav-link" to="/topplayers">
                  <li className="nav-item active">
                      <i class="fa fa-users bi nav-icon" aria-hidden="true"></i>
                      Топ игроков
                  </li>
                </NavLink>

                <li className="nav-item dropdown">
                  <span onClick={this.handleClickMenu} className="nav__link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                  Дополнительно
                  </span>

                  <div className="navbar__dropdown" aria-labelledby="navbarDropdown">
                    <NavLink onClick={this.handleClickMenu} className="nav__link dropdownitem" to="/rules"><i className="fas fa-gavel nav__icon"></i> Правила</NavLink>
                    <NavLink onClick={this.handleClickMenu} className="nav__link dropdownitem" to="/offer"><i className="fas fa-sync nav__icon"></i>Оферта </NavLink>                    
                    <a onClick={this.handleClickMenu} className="nav__link dropdownitem" href={STEAM_PROFILE_LINK} target="_blank"><i className="fas fa-phone-square-alt nav__icon"></i>Связь с создателем</a>
                  </div>
                </li>


                {this.props.isAuth == false &&
                  <Login className="header__login_1025"/>
                }
              </ul>

              {
                this.props.isAuth == true && 
                    <div onClick={this.handleClickAccount} className="header__account">
                      <i className="fas fa-user"></i>
                    </div>
              }
        </nav>
    );
  }
}
