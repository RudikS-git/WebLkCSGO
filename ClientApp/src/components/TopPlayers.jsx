import React, { useState, useEffect } from 'react';
import { DataTable } from './Manage/DataTable';
import styled from 'styled-components';
import { GetTopPlayers } from './../api/topPlayers';
import { LoadingSpinner } from './LoadingSpinner';

export const TopPlayers = (props) => {
    const [topPlayers, setTopPlayers] = useState(null);
    const [isFetching, setFetching] = useState(true);
    const [error, setError] = useState(null);
    
    useEffect(() => {
        getTopPlayer();
    }, []) //  внутри компонента вызывается хук, что приводит к зацикливанию. с [] вызывается 1 раз

    const getTopPlayer = () => {
        GetTopPlayers()
            .then(response => {
                setTopPlayers(response);
            })
            .catch(error => setError(error))
            .finally(() => setFetching(false));
    }

    const TableHeaders = (
    <>
        <th className="tickets__data-table">Игрок</th>
        <th className="tickets__data-table">Очков</th>
        <th className="tickets__data-table">KD</th>
        <th className="tickets__data-table">Убийств</th>
        <th className="tickets__data-table">Смертей</th>
    </>)

    const getContent = (it, Td) => {
        return (
        <>
            <Td>
                <a>{it.name}</a>
            </Td>
            <Td>{it.value}</Td>
            <Td>{(it.kills / it.deaths).toFixed(2)}</Td>
            <Td>{it.kills}</Td>
            <Td>{it.deaths}</Td> 
        </>)
    };

    const DataTableCSSComponent= styled.table`
        grid-template-columns: 
            minmax(60px, 1fr)
            minmax(60px, 1fr)
            minmax(60px, 1fr)
            minmax(60px, 1fr)
            minmax(60px, 1fr)
    `;

    if(error)
    {
        return <p>{error}</p>
    }

    if(isFetching)
    {
        return <LoadingSpinner/>;
    }

    if(topPlayers) {
        return (
            <div>
                <DataTable
                    headers = {TableHeaders}
                    content = {topPlayers}
                    getContent = {getContent}
                    tableStyleComponent = {DataTableCSSComponent}
                        />
            </div>
        );
    }
};