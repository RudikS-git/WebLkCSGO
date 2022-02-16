import React, { Component, useState, useEffect } from 'react';
import { Nav } from 'react-bootstrap';
import { NavLink } from 'react-router-dom';
import { GetPrivileges } from '../../../api/purchase';

import styled from 'styled-components';

import { DataTable } from './../../Manage/DataTable';
import './Privileges.css';
import { LocalError } from '../../Manage/LocalError';
import { LoadingSpinner } from '../../LoadingSpinner';

export const Privileges = (props) => {
    const [name, setName] = useState();
    const [typePrivId, setTypePrivId] = useState();
    const [privileges, setPrivileges] = useState([]);
    const [isFetching, setFetching] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        GetPrivileges()
        .then(data => {

            setPrivileges(data)
        })
        .catch(error => setError(error))
        .finally(() => setFetching(false));
    }, []);

        if(error) {
            return <LocalError error={error}/>
        }

        if(isFetching){
            return <LoadingSpinner/>
        }

        if(privileges) {

            const privilegesTableHeaders = (
                <>
                    <th>Привилегия</th>
                    <th></th>
                    <th></th>        
                </>)

            const getContent = (it, Td) => {
                return (
                <>
                    <Td>{`${it.name} - ${it.price}`}</Td>
                    <Td>
                        <button>
                            <i class="fa fa-pencil" aria-hidden="true"></i>
                        </button>
                    </Td>
                    <Td>
                        <button>
                            <i class="fa fa-trash" aria-hidden="true" color="red"></i>
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
                        <span>Список привилегий:</span>

                        <DataTable
                            headers = {privilegesTableHeaders}
                            content = {privileges}
                            getContent = {getContent}
                            tableStyleComponent = {DataTableComponent}
                        />
                        <input onChange={(e) => setName(e.target.value)}></input>
                        <button onClick={() => props.AddFeature(typePrivId, name)}>Добавить привилегию</button>
                    </div>

                    <div>
                        <select value={typePrivId} onChange={e => setTypePrivId(e.target.value)}>
                            {privileges && privileges.map(it => {
                              return <option value={it.id}>{it.name}</option>  
                            })}
                        </select>
                        <label>Добавить возможность</label>
                        <input onChange={(e) => setName(e.target.value)}></input>
                        <button onClick={() => props.AddFeature(typePrivId, name)}>Добавить</button>
                    </div>
                </div>
            );
        }

        return <></>
}