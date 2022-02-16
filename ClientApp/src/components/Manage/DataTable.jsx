import React from 'react';

import './DataTable.css';
import styled from 'styled-components';

export const DataTable = (props) => {
    
    const Table = styled(props.tableStyleComponent)`
        display: grid;
        border-collapse: collapse;
        min-width: 100%;
    `;
    
    

    const Td = props.Td? 
                    props.Td :
                    styled.td`
                        padding-top: 10px;
                        padding-bottom: 10px;
                        color: white;
                        border-bottom: solid 1px #424242;
                        &:last-child{
                           width: calc(100% - 10px )
                        }
                        `
    return (
    <Table>
        <thead className="data-table__thead">
            <tr className="data-table__tr">
                {props.headers && props.headers}
            </tr>     
        </thead>

        <tbody className="data-table__tbody">
            {
                props.content && props.content.map(it => 
                    <tr id={it.id} className="data-table__tr">
                        {props.getContent(it, Td)}
                    </tr>
                )
            }
        </tbody>
    </Table>)
        
}