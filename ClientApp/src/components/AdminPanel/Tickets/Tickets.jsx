import React, { Component, useState, useEffect } from 'react';
import { Nav } from 'react-bootstrap';
import { NavLink, useLocation } from 'react-router-dom';
import styled from 'styled-components';

import { DataTable } from '../../Manage/DataTable';
import { Pages } from '../../Manage/Pages'
import { OnlineUsers } from './OnlineUsers'

import { domain, credentials } from '../../../configureFetch';
import './Tickets.css';

import { LoadingSpinner } from '../../LoadingSpinner';

import { NotificationManager} from 'react-notifications';
import { CheckValidateToken } from './../../../utils/ajaxHelper';
import { getCookie } from './../../../libs/cookie';
import 'react-notifications/lib/notifications.css';

const signalR = require("@microsoft/signalr");

export const Tickets = (props) => {

    const [tickets, setTickets] = useState(null);
    const [page, setPage] = useState(0);
    const [users, setUsers] = useState(null);
    const [error, setError] = useState(null);
    const [isFetching, setFetching] = useState(true);
    const location = useLocation();

    useEffect(() => {

        const connectStart = async () => {

            const connection = new signalR.HubConnectionBuilder()
                            .withUrl(`${domain}/signalServer`, {
                                accessTokenFactory: () => getCookie("token")
                            })
                            .build();

            connection.serverTimeoutInMilliseconds = 1000 * 60;
            
            connection.on("refreshTickets", data => {
                NotificationManager.success('Пришел новый тикет!')

                let audio = new Audio();
                audio.src = "notify_ticket.mp3";
                audio.play();

                if(location.pathname == "/admin/tickets") {
                    getTicketsByPage(0);
                }
            });

            connection.on("ticketStateUpdate", data => {
                NotificationManager.success('Состояние тикета изменено администратором!')

                if(location.pathname == "/admin/tickets") {
                    props.GetTickets();
                }
            });

            connection.on("UpdateActiveConnections", data => {
                connection.invoke("GetAllActiveConnections")
                .then(data => {
                    if(location.pathname == "/admin/tickets") {
                        setUsers(data);
                    }
                })
            });

            connection.onclose(error => {
                
                // console.log("[signalR] сброс подключения (" + error + ")")
                // CheckValidateToken()
                //     .then(response => {
                //         if(response) {
                //              // переподключение при потери соединения


                //              connection.start()
                //                 .then(() => {

                //                     if(!props.connection || props.connection.connectionState == 0) {
                //                         props.connection.stop();
                //                     }
                                    
                //                     NotificationManager.info('Вы успешно переподключились к получению тикетов!')
                //                     props.setConnection(connection);
                                    
                //                     connection.invoke("GetAllActiveConnections")
                //                         .then(data => {
                //                             setUsers(data);
                //                         })
                //                         .catch(error => setError(error))
                //                 })
                //                 .catch(error => setError(error));

                //             console.log("[signalR] Переподключение")
                //             console.log(connection)
                //         }
                //     })
            })

            connection.start()
                .then(() => {
                    
                    NotificationManager.info('Вы успешно присоединились к получению тикетов!')
                    props.setConnection(connection)

                    
                    connection.invoke("GetAllActiveConnections")
                    .then(data => {
                        if(location.pathname == "/admin/tickets") {
                            setUsers(data);
                        }
                    })
                })
                .catch(error => setError(error));
        }

      CheckValidateToken()
      .then(data => {
          if(data) {
            props.GetTickets()
                .then(data => { 
                    setTickets(data);
                    
                    
                    if(props.connection) {
                        props.connection.invoke("GetAllActiveConnections")
                        .then(data => {
                            if(location.pathname == "/admin/tickets") {
                                setUsers(data);
                            }
                        })
                    }

                    if(!props.connection || props.connection.connectionState == 0 || props.connection.connectionState == 3) {
                        
                        connectStart();
                    }  
                    
                })
                .catch(error => setError(error))
                .finally(() => setFetching(false))
          }
      }); 

      

    }, []);

    const getTicketsByPage = (page) => {
        setFetching(true);
        setPage(page);
        props.GetTickets(page)
        .then(data => setTickets(data))
        .catch(error => setError(error))
        .finally(() => setFetching(false))
    }

    const getStateBtn = (id, state) => {
        if(state === 0) {
            return (
                <NavLink className="tickets__btn-take" to={`/admin/tickets/${id}`}>
                    Открыть
                </NavLink>
            )
        }
        else if(state === 1) {
            return (
                <NavLink className="tickets__btn-busy" to={`/admin/tickets/${id}`}>
                    Рассматривается
                </NavLink>
            )
        }
        else {
            return (
                <NavLink className="tickets__btn-closed" to={`/admin/tickets/${id}`}>
                    Закрыт
                </NavLink>
            )
        }
    }

  
    if(error) {
        return <p>Ошибка: {error.message}</p>
    }

    if(isFetching){
        return <LoadingSpinner/>;
    }

    if(tickets) {

        const TableHeaders = (
            <>
                <th className="tickets__data-table">Время</th>
                <th className="tickets__data-table tickets__data-table_type_mobile">Сервер</th>
                <th className="tickets__data-table tickets__data-table_type_mobile">Отправитель</th>
                <th className="tickets__data-table">Нарушитель</th>
                <th className="tickets__data-table tickets__data-table_type_mobile">Причина</th>
                <th className="tickets__data-table">Состояние</th> 
            </>)

        const getContent = (it, Td) => {
            return (
            <>
                <Td>{it.dateCreation}</Td>
                <Td>{it.server.ip + ':' + it.server.port}</Td>
                <Td>
                    <a href={`https://steamcommunity.com/profiles/${it.senderUserStatId}`} target="_blank">
                        {it.senderUserName}
                    </a>
                </Td>
                <Td>
                    <a href={`https://steamcommunity.com/profiles/${it.accusedUserStatId}`} target="_blank">
                        {it.accusedUserName}
                    </a>
                </Td>
                <Td>{it.reportMessage}</Td> 
                <Td>{getStateBtn(it.id, it.state)}</Td> 
            </>)
        };

        const DataTableCSSComponent= styled.table`
            grid-template-columns: 
                minmax(60px, 1fr)
                minmax(80px, 1fr)
                minmax(60px, 1fr)
                minmax(60px, 1fr)
                minmax(60px, 1fr)
                minmax(60px, 1fr);
            @media(max-width:1025px){
                grid-template-columns: 
                    minmax(20px, 1fr)
                    minmax(20px, 1fr)
                    minmax(20px, 1fr)
            }
        `;

        return (   
            <div className="tickets">
                <div className="tickets__content">
                    <div className="tickets__header">
                        <div className="tickets__header-main">
                            <span className="tickets__header-text">Управление тикетами:</span>
                            <span>{users && users.length}</span>
                            <OnlineUsers users={users}/>
                        </div>
                        

                        <div className="tickets__manage">
                            <Pages page={page} 
                                count = {Math.ceil(tickets.count / 50)} 
                                getRowsOfPage={getTicketsByPage}
                                />
                        </div>
                        </div>


                    <div className="tickets__info">
                        <DataTable
                            headers = {TableHeaders}
                            content = {tickets.tickets}
                            getContent = {getContent}
                            tableStyleComponent = {DataTableCSSComponent}
                            Td={styled.td`
                                    padding-top: 10px;
                                    padding-bottom: 10px;
                                    color: white;
                                    border-bottom: solid 1px #424242;
                                    &:{
                                    width: calc(100% - 10px )
                                    }
                                    @media (max-width: 1025px){
                                    &:nth-child(2){
                                    display:none;
                                    }
                                    &:nth-child(3){
                                    display:none;
                                    }
                                    &:nth-child(5){
                                    display:none;
                                    }
                                    }
                                    `}
                        />
                    </div>
                </div>
            </div>                          
        );
    }
}