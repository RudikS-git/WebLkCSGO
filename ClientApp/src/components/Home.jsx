import React, { Component } from 'react';
import { } from 'react-bootstrap';

import { NewsBlock } from './Main/NewsBlock';

import { ServersMonitoring } from './Monitoring/ServersMonitoring';

import './Home.css';

export class Home extends Component {
  displayName = Home.name

  render() {
      return (
            <div className="home">
                <NewsBlock></NewsBlock>
                <ServersMonitoring></ServersMonitoring>
            </div>
    );
  }
}
