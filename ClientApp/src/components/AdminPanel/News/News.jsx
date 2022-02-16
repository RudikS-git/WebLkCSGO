import React, { Component } from 'react';
import { Nav } from 'react-bootstrap';
import { NavLink } from 'react-router-dom';
import styled from 'styled-components';

import { DataTable } from '../DataTable';
import './News.css';

export class News extends Component {
    render() {

        const { error, privileges, isFetching } = this.props;
        console.log(this.props);

        if(error) {
            return <p>Ошибка: {error.message}</p>
        }

        if(isFetching){
            return <p> Загрузка ... </p>
        }

        if(news) {

            const privilegesTableHeaders = (
                <>
                    <th>Новость</th>
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
                        <span>Список новостей:</span>

                        <DataTable
                            headers = {privilegesTableHeaders}
                            content = {privileges}
                            getContent = {getContent}
                            tableStyleComponent = {DataTableComponent}
                        />
                        <button>Добавить новость</button>
                    </div>
                </div>                          
            );
        }

        return <></>
    }
}