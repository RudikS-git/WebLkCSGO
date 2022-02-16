import React, { Component, useRef } from 'react';
import { Link } from 'react-router-dom';
import { domain } from './../../configureFetch';
import { Login } from './../Auth/Login'
import { Purchase } from './../../api/purchase';

import './PurchaseModal.css';

export class SteamIdField extends Component 
{ 
    constructor(props) {
        super(props);
        var isValid = this.validate(props.value);
        this.state = { value: props.value, valid: isValid };
        this.onChange = this.onChange.bind(this);
    }

    validate(val) {
        return String(val).length >= 6;
    }

    onChange(e) {
        var val = e.target.value;
        var isValid = this.validate(val);
        this.setState({value: val, valid: isValid});
    }

    render() {
        var color = this.state.valid===true? "green":"red";

        console.log(this.props.steamId);

        if(this.props.steamId)
        {
            return (
            <div className="form__group field payment_p">
                <input type="input" className="payment__field"  maxlength="32" placeholder="Введите Steam ID" name="steamId" id='steamId' value={this.props.steamId} style={{borderColor:color}} readonly/>
                <label for="steamId" className="payment__label">Ваш STEAMID</label>
            </div> )
        }
        else
        return( 
                <div className="payment__group field payment_p">
                    <input type="input" onChange={this.onChange} className="payment__field"  maxlength="32" placeholder="Введите Steam ID" name="steamId" id='steamId' style={{borderColor:color}} required/>
                    <label for="steamId" className="payment__label">Ваш STEAMID</label>
                </div> 
        ) 
    }   
}

export const PurchaseModal = (props) => {

    const steamIdField = useRef();

    const purchase = async (id, steamId) => {
        const response = await Purchase(id, steamId);
        window.location.href = response;
    }

    const { modal: {modalProps}, CloseModal } = props;

      return (
        <div className="purchase__modal-overlay__window">
            <div className="purchase__modal-overlay__header">
                
            </div>

            <div className="modal-overlay__body">

                <div className="purchase__icon-block">
                    <button className="purchase__modal-button-exit" onClick={CloseModal}>
                        <i className="fa fa-times purchase__icon-exit" aria-hidden="true"></i>
                    </button>
                </div>

                <div className="payment_form">
                    <div>Мы принимаем</div>
                    <img src="\images\qiwi.svg" className="purchase__qiwi" width="60" height="60" />
                    <img src="\images\visa-mastercard-mir.svg" class="purchase__qiwi" width="60" height="60"></img>
                    
                    {modalProps.privilege.SteamId == null?
                    
                        <>
                            <span>Чтобы оплатить нужно зайти через STEAM</span>
                            <Login></Login>
                        </>
                        :
                        <SteamIdField steamId={modalProps.privilege.SteamId} ref={steamIdField} />
                    }

                    <div>
                        <span>Вы собираетесь приобрести <b>{modalProps.privilege.Name}</b></span>
                    </div>

                    <div>К оплате <b>{modalProps.privilege.discountPrice? modalProps.privilege.discountPrice:modalProps.privilege.price}</b> рублей</div>

                    {modalProps.privilege.SteamId != null &&
                        <button className="purchase__btn-buy" onClick={() => purchase(modalProps.privilege.Id, modalProps.privilege.SteamId)}>Оплатить</button>
                    }

                    <div className="payment_text">
                        <p>нажимая кнопку "Оплатить" вы соглашаетесь с
                            <a href="/offer"> договором оферты</a>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}