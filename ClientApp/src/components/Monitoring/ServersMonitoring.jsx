import React, { Component, useState, useEffect } from 'react';
import { Server } from './Server';
import { GetServers } from '../../api/monitoring'

import { LoadingSpinner } from './../LoadingSpinner';

import './ServersMonitoring.css';

export const ServersMonitoring = (props) => {

    const [servers, setServers] = useState(null);
    const [error, setError] = useState(null);
    const [isFetching, setFetching] = useState(true);

    useEffect(() => {
        GetServers()
        .then(data => setServers(data))
        .catch(error => setError(error))
        .finally(() => setFetching(false))

    }, []);


    if(error)
    {
        return <p>Во время запроса произошла ошибка: {error}</p>
    }

    if(isFetching)
    {
        return <LoadingSpinner/>
    }

    if(servers) {

        return (<div className="servers__container">
        <div className="servers-header">
            <span className="servers__header_name">Наши сервера</span>
        </div>

        <div className="servers__flex-container servers__row">
            {servers.servers.map(servers =>
                <Server id={servers.id} stateServer={servers.status} name={servers.name} ip={servers.ip} port={servers.port} players={servers.players} maxPlayers={servers.maxPlayers} playersList={servers.playersList}></Server>
            )}
        </div>

        <div className="servers-info">
            <div className="servers-info__content">
                <span className="servers__servers-stat_first servers__servers-stat_hover">Игроков сейчас: {servers.players}/{servers.slots}</span>
            </div>
            <div className="servers-info__content">
                <span className="servers__servers-stat servers__servers-stat_hover">Игроков за сегодня: {servers.countPlayersDay}</span>
            </div>
            <div className="servers-info__content">
                <span className="servers__servers-stat servers__servers-stat_hover">Новых игроков: {servers.countNewPlayersDay}</span>
            </div >
            <div className="servers-info__content">
                <span className="servers__servers-stat_last servers__servers-stat_hover">Всего игроков: {servers.countPlayers}</span>
            </div>
        </div>    
    </div>)
    }
}