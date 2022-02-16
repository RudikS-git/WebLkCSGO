import React, { Component } from 'react';
import Boostrap from 'react-bootstrap';
import { LoadingSpinner } from './../LoadingSpinner';

import './News.css';

export class News extends Component 
{
  displayName = News.name

    constructor(props) 
    {
        super(props);
    }

    render()
    { 
        return (    
        
                <div id="carouselExampleIndicators" className="carousel slide" data-ride="carousel">
                    <ol className="carousel-indicators">
                        <li data-target="#carouselExampleIndicators" data-slide-to="0" className="active"></li>
                        <li data-target="#carouselExampleIndicators" data-slide-to="1"></li>
                        <li data-target="#carouselExampleIndicators" data-slide-to="2"></li>
                    </ol>
                    <div className="carousel-inner">
                        <div className="carousel-item active">
                            <img className="d-block w-100" src="..." alt="Первый слайд"/>
                        </div>
                        <div className="carousel-item">
                            <img className="d-block w-100" src="..." alt="Второй слайд"/>
                        </div>
                        <div className="carousel-item">
                            <img className="d-block w-100" src="..." alt="Третий слайд"/>
                        </div>
                    </div>
                    <a className="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev">
                        <span className="carousel-control-prev-icon" aria-hidden="true"></span>
                        <span className="sr-only">Previous</span>
                    </a>
                    <a className="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next">
                        <span className="carousel-control-next-icon" aria-hidden="true"></span>
                        <span className="sr-only">Next</span>
                    </a>
                </div>        
        );
    }
}
