import React, { Component, useState, useEffect } from 'react';
import { Nav } from 'react-bootstrap';
import { NavLink } from 'react-router-dom';

import './Servers.css';
import { ServersEditModal, ServersAddModal } from './ServersEditModal';
import { DataTable } from './../../Manage/DataTable';

import styled from 'styled-components';

export const Servers = (props) => {

    const [servers, setServers] = useState(null);
    const [error, setError] = useState(null);
    const [isFetching, setFetching] = useState(true);

    useEffect(() => {
        props.GetServers()
        .then(data => setServers(data))
        .catch(error => setError(error))
        .finally(() => setFetching(false))
    }, []);

    const deleteServer = async (id) => {
        let message = await props.DeleteServer(id);
        // OpenModal()
        alert(message);
    }

    const changeServer = (selectedServer) => {
        props.OpenModal({
             modalType: ServersEditModal,
             modalProps: {
                 hasClose: true,
                 server: selectedServer,
                 ChangeServer: props.ChangeServer
                 // передать close + get servers, сделать получение и проверку прямо в компоненте для дальнейшего
                 // рендеринга окна завершения(модальное окно success or error)
             }
         });
    }

    const addServer = () => {
        props.OpenModal({
            modalType: ServersAddModal,
            modalProps: {
                hasClose: true,
                AddServer: props.AddServer
            }
        });
    }

    if(error) {
        return <p>Ошибка: {error.message}</p>
    }

    if(isFetching){
        return <p> Загрузка ... </p>
    }

    if(servers) {
        const serversTableHeaders = (
            <>
                <th>IP:PORT</th>
                <th></th>
                <th></th>        
            </>)

        const getContent = (it, Td) => {
            return (
            <>
                <Td>{`${it.id}: ${it.ip}:${it.port}`}</Td>
                <Td>
                    <button onClick={() => changeServer(it)}>
                        <i class="fa fa-pencil" aria-hidden="true"></i>
                    </button>
                </Td>
                <Td>
                    <button onClick={() => deleteServer(it.id)}>
                        <i class="fa fa-trash" aria-hidden="true"></i>
                    </button>
                </Td>            
            </>)
        };

        const DataTableComponent = styled.table`
            grid-template-columns: 
                minmax(150px, 1fr)
                minmax(150px, 1fr)
                minmax(150px, 1fr)
        `;

        return (   
            <div className="admin-panel">
                <div className="admin-panel__server-content">
                    <span>Список серверов:</span>

                    <DataTable
                        headers = {serversTableHeaders}
                        content = {servers}
                        getContent = {getContent}
                        tableStyleComponent = {DataTableComponent}
                    />
                    
                    <button onClick={() => addServer()}>Добавить сервер</button>
                </div>
            </div>                          
        );
    }

    return <></>
    
}