import React, { Component, useState, useEffect } from 'react';
import {Route, NavLink, HashRouter} from 'react-router-dom'

import AccountInfoContainer from './../Auth/AccountInfoContainer';
import './Aside.css';

export const Aside = (props) => {

    return (
        <div className="aside__account-info">
            <AccountInfoContainer/>
        </div>
    )
};