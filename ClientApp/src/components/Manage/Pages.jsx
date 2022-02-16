import React, { Component } from 'react';

import './Pages.css';

export class Pages extends Component {
    displayName = Pages.name

    onChangeHandler = (e) => {

        this.props.getRowsOfPage(+e.target.value);
    }

    nextPage = (e) => {

        if(+this.props.count - 1 <= +this.props.page) {
            this.props.getRowsOfPage(0);
        }
        else {
            this.props.getRowsOfPage(+this.props.page + 1);
        }
    }

    pastPage = (e) => {

        if(this.props.page < 1) {
            this.props.getRowsOfPage(+this.props.count - 1);
        }
        else {
            this.props.getRowsOfPage(+this.props.page - 1);
        }
    }

    render() {
        let count = this.props.count;
        let Numbers = [];

        for(let i = 0; i < count; i++)
        {
            Numbers.push(<option key={i} value={i}> {i+1} </option>)
        }

        return (
            <span className="punishment-pages__container">
                <button className="btn form-control__switch-page" onClick={this.pastPage}>
                    <i className="fa fa-chevron-left" aria-hidden="true"></i>
                </button>

                <select className="form-control form-control-sm form-control__pages" id="selectvalue" onChange={this.onChangeHandler} value={this.props.page}>
            
                    {
                        Numbers.map(Numbers => Numbers)
                    }

                </select>

                <button className="btn form-control__switch-page" onClick={this.nextPage}>
                    <i className="fa fa-chevron-right" aria-hidden="true"></i>
                </button>
            </span>
        );
    }
}