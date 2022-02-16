import React, { useState, useEffect, useRef } from 'react';
import useOutsideClick from './../../../utils/useOutsideClick';
import './OnlineUsers.css';

export const OnlineUsers = (props) => {
    
    const [isActive, setActive] = useState(false);

    const ref = useRef();
    
    useOutsideClick(ref, () => {
        if (isActive) setActive(false);
    });

    return  <div className="online-users">
                <div className="online-users__btn" onClick={(e) => setActive(!isActive)}>
                    <i className="fa fa-user online-users__icon" aria-hidden="true"></i>
                </div>

                {
                    isActive && 
                        <div ref={ref} className= {isActive? "online-users__list online-users__list_active":"online-users__list"}>
                            <span className="online-users__text" >Проверяющие онлайн:</span>
                            <div className="online-users__list-content">
                                {props.users && props.users.map(it =>
                                <div className="online-users__user">
                                    <span>
                                        <img className="online-users__avatar" src={it.avatarSource} alt={it.avatarSource}/>
                                        <a className="online-users__name" href={`https://steamcommunity.com/profiles/${it.authId}`} target="_blank">{it.name}</a> 
                                    </span>
                                </div>
                                
                                )}
                            </div>
                        </div>
                }
            </div>
};