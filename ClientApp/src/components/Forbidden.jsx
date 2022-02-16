import React, { Component } from 'react';
import { } from 'react-bootstrap';

import './Forbidden.css';

export const Forbidden = () => {
    return (
        <div className="forbidden">
            <div className="forbidden-content">
                <i class="far fa-frown forbidden__icon"></i>
                <span className="forbidden__text">У вас нет доступа к этой странице</span>
            </div>              
        </div>
    )      
};