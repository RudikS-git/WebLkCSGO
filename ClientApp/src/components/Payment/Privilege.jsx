import React, { Component } from 'react';
import { } from 'react-bootstrap';

import { PurchaseModal } from './PurchaseModal';

import './Privilege.css';

export class Privilege extends Component
{
    displayName = Privilege.name

    render()
    {
        return (
            <div className={this.props.isActive ? "privilege privilege_active_true" :"privilege privilege_active_false"}>
                <div className="privilege-header">

                    {
                    this.props.isOwner &&  
                    <div className="privilege__owner-block">
                        <span className="privilege__owner-text">Ваша привилегия</span>
                        <i class="fa fa-check privilege__owner-icon" aria-hidden="true"></i>
                    </div>
                    }
                        
                </div>
                <div class="privilege__face privilege__face_face1">
                    <div className="privilege__content_main">
                        <img className="privilege-img" src={this.props.Image} alt={this.props.Name}/>
                        <h3 className="privilege__name">{this.props.Name}</h3>

                        {this.props.discountPrice != null && this.props.Price != this.props.discountPrice?
                            <span className="privilege__text-price">
                                <span className="privilege__text_old-price">{this.props.Price}</span>
                                <span className="privilege__text_new-price">{this.props.discountPrice + " руб"}</span>
                            </span>
                            :
                            <h3 className="privilege__name">{this.props.Price}</h3>
                        }
                     </div>
                  </div>
                  <div class="privilege__face privilege__face_face2">
                     <div className="privilege__content_additional">
                        <ul className="privilege__features">
                            {
                                this.props.features && this.props.features.map(it =>
                                    <li>
                                        <i class="fas fa-plus privilege_icon"></i>
                                        {it.name}
                                    </li>
                                    )
                            }
                        </ul>

                        {
                            this.props.isActive &&
                            <button className="privilege__btn-buy"
                                onClick={() => {
                                    this.props.Buy({
                                        modalType: PurchaseModal,
                                        modalProps: {
                                            privilege: this.props
                                        }
                                    });
                                }}>
                                    Купить
                                </button> 
                        }

                    </div>
                </div>            
            </div>
        );
    }
}
