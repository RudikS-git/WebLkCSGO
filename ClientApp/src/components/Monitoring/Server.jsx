import React, { Component } from 'react';
import { NotificationManager} from 'react-notifications';
import 'react-notifications/lib/notifications.css';
import './Server.css';
import PlayersModal from './PlayersModal';

export const Server = (props) =>
{
  const [open, setOpen] = React.useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const connectServer = () => {
    const url = "steam://connect/" + props.ip + ":" + props.port;
    window.location.replace(url); 
  }

  const copyServer = () => {
   
    const copy = "connect " + props.ip + ":" + props.port;
    NotificationManager.success('Скопирован адрес сервера ')
    navigator.clipboard.writeText(copy)
  }
  
  const val = Math.round(Number(props.players * 100 / props.maxPlayers));

    return (
      props.stateServer != 0?
      (
        <div id={props.id} className="server__main_window server__flex-container">
              <div className="server__col">
                <div className="server__text_block server_text">

                  <p className="server__info_nameserver"> {props.name} </p>
                <button className="server__button_connect" onClick={connectServer} >
                <i className="fas fa-play-circle server__connect_icon"/>
                </button>

                <button className="server__button_copy" onClick={() => copyServer()} >
                <i className="fas fa-copy"/>
                </button>

                  {/* <span >
                    <span className="server__info_ip">{props.ip}:{props.port}
                      <button className="server__connect_icon" onClick={connectServer} >
                          <svg width="20px" height="15px" viewBox="0 0 16 16" className="bi bi-caret-right-fill" fill="white" xmlns="http://www.w3.org/2000/svg">
                            <path d="M12.14 8.753l-5.482 4.796c-.646.566-1.658.106-1.658-.753V3.204a1 1 0 0 1 1.659-.753l5.48 4.796a1 1 0 0 1 0 1.506z"/>
                          </svg>
                      </button>
                    </span>
                  </span> */}
                  {/* сделать кнопку копривать и сделать медия запрос для серверов что бы были повыше  */}
                  
                  <div className="server__info_players">
                    <p className="server__progress-bar-text">{props.players} / {props.maxPlayers}</p>
                    <div className="server__bar" role="progressbar" style={{width:`${val}%`}} aria-valuenow={val} aria-valuemin="0" aria-valuemax="100"></div>
                    <button className="server__button_online" onClick={() => handleOpen()} >
                     <i class="fas fa-user-friends"></i>
                    </button>
                  
                  </div>
                </div>
  
                <img className="server__img" src="images/awplego11.jpg" alt="" />

              </div>

              <PlayersModal players={props.playersList?.players} open={open} setOpen={setOpen}/>
        </div>      
      )

      :
      (  <div id={props.id} className="server__main_window">
            <div className="server__flex-container">
              <div className="server__col server__window">
                    <div className="server__text_block">
                        <div className="server__text">
                          {props.name} - {props.ip}:{props.port} <br/>
                          Сервер не работает на данный момент :(
                        </div>
                    </div>
                  
                    <img className="server__img" src="images/awplego11.jpg" alt="" />
              </div>
            </div>
        </div>
      )
  );
  
}
