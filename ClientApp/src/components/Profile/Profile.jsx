import React, { useState, useEffect } from 'react';
import { Redirect } from 'react-router-dom';
import { connect } from 'react-redux';
import { NotFound } from './../NotFound';
import { LoadingSpinner } from './../LoadingSpinner';

import { GetProfile } from './../../api/profile';

import './Profile.css';

const Profile = (props) => {

    const [profile={isLoaded:true}, setProfile] = useState();

    useEffect(() => {
        GetProfile(props.match.params.id)
            .then(data => {
                setProfile(data);
            });
    }, []);

    if(!profile) {
        
        return <NotFound/>
    }

    if(profile.isLoaded) {
        return <LoadingSpinner/>
    }

    return (<div className="profile">
                <div className="profile__content">
                    <div className="profile__main">

                        <div>
                            <div className="profile__avatar">
                                <img className="profile__avatar-img" src={profile.avatarSource}></img>
                            </div>

                            <div className="profile__priv">
                                <div className="profile__priv-content">
                                    <span className="profile__priv_text">{profile.groupName? profile.groupName:"Игрок"}</span>
                                </div>
                            </div>
                        </div>
                        

                        <div className="profile__name-block">
                            <div>
                                <a href={profile.auth64} className="profile__name">{profile.name}</a>
                            </div>
                            
                            <p>Был(а) в игре: {profile.lastConnection}</p>
                            <div className="profile__rank">
                                <div className="profile__process_content">
                                 
                                        
                                    <div className="profile__process_content-block-img">

                                        <div className="profile__process_block-rank">
                                            <span className="profile__process_text">Текущее звание:</span>
                                            <img className="profile__rank-img" src={"images/ranks/" + profile.lvl + ".svg"}></img>
                                        </div>      

                                        {
                                            profile.nextRank?

                                                <>
                                                    <div className="profile__progress-bar">
                                                        <div className="profile__rank_process-value" role="progressbar" style={{width:`${(profile.value * 100) / profile.nextRank.value}%`}} aria-valuenow={50} aria-valuemin="0" aria-valuemax="100"/>
                                                    </div>
                                                    
                                                    <div className="profile__points_text">Очков {profile.value + " из " + profile.nextRank.value}</div>

                                                    {/* <img className="profile__rank-img" src={"images/ranks/" + (+profile.lvl + 1) + ".svg"}></img> */}
                                                </>
                                                :
                                                <>
                                                    <div className="profile__progress-bar">
                                                        <div className="profile__rank_process-value" role="progressbar" style={{width:`${100}%`}} aria-valuenow={50} aria-valuemin="0" aria-valuemax="100"/>
                                                    </div>

                                                    <div className="profile__points_text">Очков {profile.value}</div>
                                                </>
                                        }
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="profile__stat">
                        <div className="profile__stat-content">
                            <p className="profile__stat-content_item">
                                <i class="fas fa-place-of-worship"></i>
                                Место по очкам: 
                                <span className="profile__text">{profile.placeTopOfPoints}</span>
                            </p>

                            <p className="profile__stat-content_item">
                                <i class="fas fa-place-of-worship"></i>
                                Место по времени: 
                                <span className="profile__text">{profile.placeTopOfTime}</span>
                            </p>
                        
                            <p className="profile__stat-content_item">
                                <i class="fas fa-head-side-virus"></i>
                                В голову: 
                                <span className="profile__text">{profile.headshots}
                                </span>
                            </p>
                        
                            <p className="profile__stat-content_item">
                                <i class="fas fa-crosshairs"></i>
                                Убийств:
                                <span className="profile__text">
                                    {profile.kills}
                                </span>
                                
                            </p>
                        
                            <p className="profile__stat-content_item">
                                <i className="fas fa-skull-crossbones"></i>
                                Смертей: 
                                <span className="profile__text">{profile.deaths}</span>
                            </p>

                            <p className="profile__stat-content_item">
                                <i class="fas fa-hands-helping"></i>
                                Ассистов:
                                <span className="profile__text">{profile.assists}</span>
                            </p>

                            <p className="profile__stat-content_item">
                                <i class="fas fa-trophy"></i>
                                Выйграно раундов: 
                                <span className="profile__text">{profile.roundWins}</span>
                            </p>

                            <p className="profile__stat-content_item">
                                <i class="fas fa-arrow-down"></i>
                                Проиграно раундов: 
                                <span className="profile__text">{profile.roundLosses}</span>
                            </p>

                            <p className="profile__stat-content_item">
                                <i className="far fa-clock"></i>
                                Игровое время: 
                                <span className="profile__text">{(profile.playTime / 3600).toFixed(2) + " часов"}</span>
                            </p>
                        </div>
                    </div>

                </div>
            </div>
    )
};

export default Profile;