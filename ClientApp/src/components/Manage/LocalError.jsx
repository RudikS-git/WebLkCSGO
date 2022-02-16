import React, { Component } from 'react';
import { } from 'react-bootstrap';

import './LocalError.css';

export const LocalError = (props) => {
    return (
        <div className="local-error">
            <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
            <p>Произошла ошибка: {props.error? props.error:"Неизвестная"}</p>
        </div>
    )      
};