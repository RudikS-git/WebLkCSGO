import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import './Panel.css';

export const Panel = (props) => {


    return (
        <div className="admin-panel">
            <h1 className="admin-panel__text">Управление</h1>
            <div className="admin-panel__content">
                
                <NavLink className="admin-panel__component" to="/admin/servers">
                    Сервера
                </NavLink>
        
                <NavLink className="admin-panel__component" to="/admin/privileges">
                    Привилегии
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/news">
                    Новости
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/punishments">
                    Наказания
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/tickets">
                    Тикеты
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/rules">
                    Правила
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/offert">
                    Офферта
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/privs">
                    Управление привилегиями
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/roles">
                    Роли(сайт)
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/ranks">
                    Ранги
                </NavLink>

                <NavLink className="admin-panel__component" to="/admin/rconsystem">
                    RCON система
                </NavLink>
            </div>
        </div>
    )
}