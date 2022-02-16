import React, { Component } from 'react';
import { } from 'react-bootstrap';

import { Portal } from './../../Portal';

import './Modal.css';

export class Modal extends Component {
  displayName = Modal.name

  render() {

      return (
        <Portal>
            <div className="modal-overlay">
                <div className="modal-overlay__window">
                    <div className="modal-overlay__header">
                        {this.props.header}
                    </div>

                    <div className="modal-overlay__body">
                        {this.props.children}
                    </div>

                    <div className="modal-overlay__footer">
                        <button className="btn btn-danger modal-overlay__btn-cancel" onClick={this.props.cancel}>Отменить</button>
                        <button className="btn btn-success modal-overlay__btn-edit" onClick={this.props.edit}>Изменить</button>
                    </div>
                </div>
            </div>
      </Portal>
              
    );
  }
}
