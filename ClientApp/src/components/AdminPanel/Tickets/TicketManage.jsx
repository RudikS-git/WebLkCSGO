import React, { Component, useState, useEffect } from 'react';
import { Nav } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { DataTable } from '../../Manage/DataTable';
import { Dropdown } from '../../Manage/Dropdown';
import { CloseTicketModal } from './TicketModals';
import { GetHistory } from './../../../api/gamechat';
import './TicketManage.css';
import { LocalError } from '../../Manage/LocalError';

export const TicketManage = (props) => {

    const [ticket, setTicket] = useState();
    const [historyTickets, setHistoryTickets] = useState();
    
    const [answer, setAnswer] = useState();
    const [isCustomReason, setIsCustomReason] = useState(true);

    const [chat, setChat] = useState();

    const [error, setError] = useState(null);
     
    const ticketAnswerChange = (e) => setAnswer(e.target.value);

    const getHistoryChat = (serverId, count, steamId) => {
        GetHistory(serverId, count, steamId)
        .then(json => {
            setChat(json);
        })
        .catch(error => setError(error));
    }

    useEffect(() => {
        props.GetTicket(props.match.params.id)
    }, [])

    
    const getTicket = () => {
        props.GetTicket(props.match.params.id)
                  .then(data => { 
                      setTicket(data); 

                      getHistoryChat(data.serverId, 15, data.accusedUserStatId)

                      props.GetTicketsByUser(data.accusedUserStatId, data.id).then(data => {
                        setHistoryTickets(data)
                      });
                    });
    }

    if(ticket == null) {
        getTicket();
    }

    const getStateMessage = (state) => {
        switch(state) {
            case 0: {
                return "Открыт";
            }

            case 1: {
                return "На проверке";
            }

            case 2: {
                return "Закрыт";

            }
                
        }
    }

    const setStateTicket = (ticket, checkingUserId, state) => {
        
        props.OpenModal({
            modalType: CloseTicketModal,
            modalProps: {
                hasClose: true,
                ticket: ticket,
                closeModal: () => props.CloseModal(),
                setTicketState: (reason) => props.setTicketState(checkingUserId, ticket.id, state, reason)
            }
        });
    }

    const selectHandler = (e) => {
        if(e.target.value == 0) {
            setIsCustomReason(true);
        }
        else {
            console.log(e);
            setIsCustomReason(false);
            setAnswer(e.target.value)

        }
    }

    const getButton = (state) => {
        switch(state) {
            case 0: { // открыт
                return (
                    <>
                        <button className="ticket__btn" onClick={() =>  props.setTicketState(checkingUserId, ticket.id, 1, "").then(data => setTicket(data))}>Взять</button>  
                    </>
                )
            }

            case 1: { // на проверке
                return <>
                            <div className="ticket__report_reason" >
                                <label className="ticket_reason-label">Причина:</label>
                                <div className="ticket__reason-textbox">
                                    <select className="ticket__report_select" onClick={selectHandler}>
                                        <option value={0}>Своя причина</option>
                                        <option value={"Был наказан"}>Был наказан</option>
                                        <option value={"Не обнаружено"}>Не обнаружено</option>
                                        <option value={"Ложная заявка"}>Ложная заявка</option>
                                        <option value={"Игрок вышел"}>Игрок вышел</option>
                                        <option value={"Срок заявки истек"}>Срок заявки истек</option>
                                    </select>

                                    {
                                        isCustomReason && <input className="ticket__report_input" name="reason" type="input" placeholder={answer} onChange={ticketAnswerChange}></input>
                                    }

                                </div>
                                 
                            </div>
                            <button className="ticket__btn" type="submit" onClick={() => props.setTicketState(checkingUserId, ticket.id, 2, answer).then(data => setTicket(data))}>Закрыть</button>
                        </>
            }

            case 2: { // закрыт
                return <button className="ticket__btn" onClick={() => props.setTicketState(checkingUserId, ticket.id, 0, "").then(data => setTicket(data))}>Открыть</button>

            }
                
        }
    }

    const connectServer = () => {
        let url = `steam://connect/${ticket.server.ip}:${ticket.server.port}`;
        window.location.replace(url); 
      }

    const TableHeaders = (
        <>
            <th>Дата</th>
            <th>Отправитель</th>
            <th>Причина</th>        
        </>)

    const getContent = (it, Td) => {
        return (
        <>
            <Td>{it.dateCreation}</Td>
            <Td>
                {getSteamLink(it.senderUserStatId, it.senderUserName)}
            </Td>
            <Td>
                {it.reportMessage}
            </Td>            
        </>)
    };

    const getSteamLink = (steamId, content) => {
        return (<a className="ticket__link" href={`https://steamcommunity.com/profiles/${steamId}`} target="_blank">{content}</a>)
    }

    const unixTimeToDate = (timestamp) => {
        let date = new Date();
        date.setTime(timestamp * 1000);

        return `${date.getDate()}.${date.getMonth() + 1} ${date.getHours()}:${date.getMinutes()}`;
    }

    const DataTableCSSComponent = styled.table`
        grid-template-columns: 
            minmax(50px, 1fr)
            minmax(50px, 1fr)
            minmax(50px, 1fr);
        margin: 5px;
        min-width: auto;
    `;

    const { isFetching, checkingUserId } = props;

    if(error) {
        return <LocalError error={error}/>
    }

    if(ticket == null)
        return <></>;

    return (
        //if(privileges) {
      <div className="ticket">
         <div className="ticket__content">
            <Link className="tickets__button_close" to="/admin/tickets">
                <i class="fas  fa-times ticket__close" aria-hidden="true"></i>
            </Link>
                        <h4 className="ticket__header">Управление тикетом номер {ticket.id}:</h4>

                        <div className="ticket__info-container">
                            <div className="ticket__info">
                                <div className="ticket__report">
                                    <p>
                                        <span>Отправитель - </span>
                                        {getSteamLink(ticket.senderUserStatId, ticket.senderUserName)}
                                    </p>
                                    <p>
                                        <span>Обвиняемый - </span>
                                        {getSteamLink(ticket.accusedUserStatId, ticket.accusedUserName)}                                    
                                    </p>
                                    <p>
                                        <span>Причина - </span> 
                                        {ticket.reportMessage}
                                    </p>
                                    <p>
                                        <span>Сервер - </span>
                                        <button onClick={() => connectServer()} className="ticket__report-server">
                                            <span>{`${ticket.server.ip}:${ticket.server.port}`}</span>
                                        </button>
                                    </p>
                                    </div>

                                    <div className="ticket__report">
                                    <p>
                                        <span>Состояние - </span>
                                        <span>{getStateMessage(ticket.state)}</span>
                                    </p>
                                    <p>
                                        <span>Причина закрытия - </span>
                                        <span>{ticket.answer? ticket.answer:"Отсутствует"}</span>
                                    </p>
                                    <p>
                                        <span>Взял тикет - </span>
                                        <a className="ticket__link" href={`https://steamcommunity.com/profiles/${ticket.accusedUserStatId}`} target="_blank"/>
                                        <span>{ticket.checkingUserName? getSteamLink(ticket.checkingUserStatId, ticket.checkingUserName) 
                                                                        :ticket.checkingUser? ticket.checkingUser.auth64Id:"Отсутствует"}
                                        </span>
                                    </p>
                                </div>

                                
                            </div>

                            <div className="ticket_chat-history">
                              <div className="ticket__chat_info">
                              <p>История чата:</p>
                              <div className="ticket__chat_button">
                              <i class="fas fa-ban button__chat_mute "></i>
                              <i className="fas fa-comment-slash button__chat_mute"></i>
                              </div>
                             
                              </div>
                              
                              <div className="ticket__chat">
                                {chat? chat.map(row =>
                                    <p className="chat__text">{"[" + row.time + "] " + row.name + ": " + row.message}</p>)
                                :
                                <p className="chat__text">Данный игрок ничего не писал в чат</p>
                                }
                              </div>
                           </div>
                        </div>
                  <div className="ticket__report-actions">
                     {getButton(ticket.state)}
                  </div>
                  <h4 className="ticket__history-header">История тикетов</h4>
                  <div className="ticket__history">
                    
                      <DataTable
                                headers = {TableHeaders}
                                content = {historyTickets}
                                getContent = {getContent}
                                tableStyleComponent = {DataTableCSSComponent}
                            />
                      <div></div>
                  </div>
         </div>
      </div>                          
       // }

    )
}