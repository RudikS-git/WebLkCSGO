import React, { Component } from 'react';

import './PunishmentSearch.css';

export class PunishmentSearch extends Component {
    displayName = PunishmentSearch.name

    constructor(props) {
        super(props);

        this.handleKeyPress = this.handleKeyPress.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.onChange = this.onChange.bind(this);
      }

    handleSubmit = () => {
        //if(this.props.searchEntity !== this.state.searchEntity) {
            this.props.getRowsOfPage(this.props.searchEntity, 0);
        //}
    }

    handleKeyPress = (event) => {

        if(event.key === 'Enter') {
            //if(this.props.searchEntity !== this.state.searchEntity) {
                this.props.getRowsOfPage(event.target.value, 0);
            //}
        }
    }

    onChange = (event) => {
        this.props.setSearchEntity(event.target.value);
    }

    render() {

        return (
            <span className="form-inline punishments-search__container">
                <input className="form-control punishments__search-block" type="search" placeholder="Поиск(Ник, STEAM ID)" aria-label="Поиск(Ник, STEAM ID)" value={this.props.searchEntity} onKeyPress={this.handleKeyPress} onChange={this.onChange}/>
                <button className="punishmnets__button-search" type="submit" onClick={this.handleSubmit}>
                    <i className="fa fa-search" aria-hidden="true"></i>
                </button>
            </span>
        );
    }
}