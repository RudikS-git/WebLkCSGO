import React, { Component } from 'react';
import { LoadingSpinner } from "../LoadingSpinner";
import { Footer } from './Footer';
import NavMenuContainer from './NavMenuContainer';
import {NotificationContainer, NotificationManager} from 'react-notifications';
import AsideContainer from './AsideContainer';

import './Layout.css';

export class Layout extends React.Component {
    displayName = Layout.name
    constructor(props) {
        super(props);
        this.state = { matches: window.matchMedia("(min-width: 1025px)".matches) };    
    }
    
    componentDidMount() {
        const handler = e => this.setState({matches: e.matches});
        window.matchMedia("(min-width: 1025x)").addListener(handler); //1750
    }

    render() {
        return (              
            <div className="layout_page">
                <header className="layout_container layout__header">
                    <div className="header__row">
                        <div className="navmenu">
                            <NavMenuContainer/>
                        </div> 
                    </div>
                </header>

                <main className="layout_container layout__main">
                    <div className="row">
                        <article className="layout__col layout__article">
                            <div className="layout_container layout__article-content">
                                {this.props.children}
                            </div>
                        </article>

                        <aside className="layout__additional-info">
                            <AsideContainer/>
                            <iframe className="layout__aside-discord" src="https://discordapp.com/widget?id=539417121810022402&theme=dark" width="246" height="100%" allowtransparency="true" frameBorder="0" sandbox="allow-popups allow-popups-to-escape-sandbox allow-same-origin allow-scripts"></iframe>
                        </aside>
                    </div>
                </main>

                <footer className="layout_container layout__footer">
                    <Footer />
                </footer>

                <NotificationContainer/>
            </div>
        );
    }
}
