import React, { useMemo, Component } from 'react';
import { Nav } from 'react-bootstrap';
import { NavLink, Link } from 'react-router-dom';
import {Login} from './Login'
import './AccountInfo.css';
import { domain } from './../../configureFetch';
import { Dropdown } from './../Manage/Dropdown';
import { LoadingSpinner } from '../LoadingSpinner';
import { LocalError } from '../Manage/LocalError';

export class AccountInfo extends Component {

    handleClickAccount() {
        document.querySelector('.layout__additional-info').classList.toggle('active');
        document.querySelector('.header__burger').classList.remove('active');
        document.querySelector('.navbar__nav').classList.remove('active');
        document.querySelector('body').classList.remove('lockMenu')
        document.querySelector('body').classList.toggle('lockAcc')
    }

    render() {

        const { error, user, isFetching } = this.props;

        if(error) {
            return <LocalError error={error}/>
        }

        if(isFetching){
            return <LoadingSpinner/>
        }
        
        if(user) {
            console.log(user)

            if(!user.isAuthenticated) {
                return  <div className="account-info__login">
                            <Login/>
                        </div>  
            }

            let items = [		
                {			
                    name:"Мой профиль",			
                    content:(				
                        <NavLink onClick={this.handleClickAccount} className="account-info__adminInfo account-info__link" to={`/profile/${user.userStat.id}`}>Мой профиль</NavLink>			
                    )		
                },	
            ];		
                
            if(user.isAuthenticated && (user.role.id == 2 || user.role.id == 3 || user.role.id == 4)) {
                items.push({
                    name:"Панель администратора",
                    content:(
                        <NavLink onClick={this.handleClickAccount} className="account-info__adminInfo account-info__link" to="/admin">Панель администратора</NavLink>
                    )
                })
            }

            items[items.length] = {
                name:"Выход",
                content:(
                    <a className="account-info__btn-exit" href={`${domain}/api/account/logout`} target="_self">
                        Выход
                        <i class="fa fa-sign-out account__quit" aria-hidden="true"></i>
                    </a>
                )
            }
            
            return (   
                    <>
                        <div className="form-inline account-info__info account-info">
                            <div className="account-info__content">
                                <div className="account-info__content-first">
                                    <div className="account-info__avatar" >
                                        <img src={user.avatarSource} alt=""></img>
                                    </div>

                                    <div className="account-info__text-info">
                                        <p className="account-info__nick">
                                            {user.name}
                                            <Dropdown
                                                isHeader={false}
                                                headerText=""
                                                items={items}
                                            />
                                        </p>
                                        <p className="account-info__steamId"> ({user.steamId})</p>
                                    </div>
                                </div>

                                <div className="account__dop-info">
                                    <p className="account-info__rank_text">Текущий ранг:</p>
                                    <p className="account-info__rank">
                                        <img className="account-info__rank-img" src={"images/ranks/" + user.userStat.lvl + ".svg"}></img>
                                    </p>
                                </div>

                            </div>                        
                        </div>
                    </>                       
            );
        }

        return <></>
    }
}