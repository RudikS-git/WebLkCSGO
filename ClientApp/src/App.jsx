import React, { Component } from 'react';
import { connect } from 'react-redux'
import { BrowserRouter, StaticRouter, Redirect, Route, Routes, Switch } from 'react-router-dom';

import { Home } from './components/Home';
import { AuthSuccess } from './components/AuthSuccess';
import { Rules } from './components/Rules';
import { Offer } from './components/Offer';
import { NotFound } from './components/NotFound';

import { Panel } from './components/AdminPanel/Panel';
import ServersContainer from './components/AdminPanel/Servers/ServersContainer';
import TicketsContainer from './components/AdminPanel/Tickets/TicketsContainer';
import TicketManageContainer from './components/AdminPanel/Tickets/TicketManageContainer';
import { TopPlayers } from './components/TopPlayers';
import Profile from './components/Profile/Profile';

import LayoutContainer from './components/Shared/LayoutContainer';
import PunishmentBansContainer from './components/Punishment/PunishmentBansContainer';
import PunishmentCommsContainer from './components/Punishment/PunishmentCommsContainer';
import PurchaseContainer from './components/Payment/PurchaseContainer';

import PrivateRoute from './privateRoute';
import {store} from './configureStore';
import { Tickets } from './components/AdminPanel/Tickets/Tickets';

import { domain } from './configureFetch';
import { Privileges } from './components/AdminPanel/Privileges/Privileges';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

export default class App extends Component {
  displayName = App.name

  render() {
      return (
        <>
        <BrowserRouter basename={baseUrl}>
          <LayoutContainer>
                <Switch>
                  <Route exact path='/' component={Home} />
                  <Route exact path='/auth/success' component={AuthSuccess} />

                  <Route path='/payment' component={PurchaseContainer} />
                  <Route path='/banlist' component={PunishmentBansContainer} />
                  <Route path='/commslist' component={PunishmentCommsContainer} />
                  <Route path='/rules' component={Rules} />
                  <Route path='/offer' component={Offer} />
                  <Route path='/topplayers' component={TopPlayers} />

                  <Route exact path='/profile/:id' component={Profile} />

                  {/* Обязательно exact, иначе вложенные будет перебивать основной компонент */}
                  {/* <Route exact path="/admin/" component={Panel}/>
                  <Route path="/admin/servers" component={ServersContainer}></Route> 
                  При роутинге даже "/" влияет на конечный результат(он считает что это вообще другой роутинг) */}
                  
                  <PrivateRoute exact component={() => <Panel/>} store={store} path="/admin"/>
                  <PrivateRoute component={() => <ServersContainer/>} store={store} path="/admin/servers"/>
                  <PrivateRoute component={() => <Privileges/>} store={store} path="/admin/privileges"/>
                  <PrivateRoute exact component={() => <TicketsContainer/>} store={store} path="/admin/tickets"/>
                  <PrivateRoute component={(props) => <TicketManageContainer {...props}/>} store={store} path="/admin/tickets/:id"/>

                  <Route component={NotFound} />
                </Switch>
            </LayoutContainer> 
        </BrowserRouter>
        </>
    );
  }
}
