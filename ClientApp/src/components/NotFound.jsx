import React, { Component } from 'react';
import { } from 'react-bootstrap';

import './NotFound.css';

export class NotFound extends Component {
  displayName = NotFound.name

  render() {

      return (
            <div className="notfound">
               <div className="notfound-content">
                  <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                  <span className="notfound__text">Данной страницы не существует!</span>
                  <p className="notfound__text-additional">Верно ли вы ввели адрес?</p>
               </div>              
            </div>              
   );
  }
}
