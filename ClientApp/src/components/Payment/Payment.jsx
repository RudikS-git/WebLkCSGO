import React, { Component, useState, useEffect } from 'react';
import { } from 'react-bootstrap';
import { createBrowserHistory } from 'history';

import { Redirect } from 'react-router-dom';

import './Payment.css';
import { Privilege } from './Privilege';
import { PurchaseModal } from './PurchaseModal';
import { LoadingSpinner } from '../LoadingSpinner';
import { LocalError } from '../Manage/LocalError';
import { GetPrivileges } from './../../api/purchase';

export class AmountField extends Component 
{
    constructor(props) 
    {
        super(props);
        var isValid = this.validate(props.value);
        this.state = {value: props.value, valid: isValid};
        this.onChange = this.onChange.bind(this);
    }

    validate(val)
    {
        return val >= 1 && val.length < 6;
    }

    onChange(e) 
    {
        var val = e.target.value;
        var isValid = this.validate(val);
        this.setState({value: val, valid: isValid});
    }

    render() 
    {
        var color = this.state.valid===true? "green":"red";

        return (

            <div className="payment__group field payment_p">
                
                <input type="number" min="1" max="10000" onChange={this.onChange} className="payment__field" placeholder="Сумма пополнения" name="amount" id='rub' style={{borderColor:color}} required />
                <label for="amount" className="payment__label">Сумма пополнения</label>
            </div>
        )
    }   
}

export const Payment = (props) =>
{
    // handleSubmit(e) 
    // {
    //     e.preventDefault();

    //     var steamId = this.refs.steamIdField.state.value;
    //     var amount = this.refs.amountField.state.value;

    //     if(this.refs.steamIdField.state.valid && this.refs.amountField.state.valid)
    //     {
    //         const formData = new FormData();
    //         formData.append('steamId', steamId);
    //         formData.append('amount', amount);

    //         let url;
    //         fetch("payment/qiwi", {
    //             method: "POST",
    //             body: formData
    //         })
    //         .then(response => {

    //             console.log(response.url);
    //             // const history = createBrowserHistory({basename=null});
    //             // history.push(response.url)});

    //             // this.setState({referrer: response.url
    //             window.location.replace(response.url); 
    //             },

    //             (error) => {
    //                 console.log(error);
    //             }
    //         );

    const [privileges, setPrivileges] = useState([]);
    const [isFetching, setFetching] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        GetPrivileges()
        .then(data => setPrivileges(data))
        .catch(error => setError(error))
        .finally(() => setFetching(false));
    }, [])


    if(isFetching) {
        return <LoadingSpinner></LoadingSpinner>
    }

    if(error) {
        return <LocalError error={error}/>
    }
    
    const priceCurrentPriv =    privileges &&
                                props.currentPrivilege &&
                                privileges.find(it => it.groupName == props.currentPrivilege.groupName)
    
    const isActive = priceCurrentPriv == undefined;

    return (
        <div className="purchase">
            <div className="purchase-header">
                <span className="purchase-header__name">Донат</span>
            </div>

            {/* <div>Ваша привилегия {this.props.currentPrivilege.groupName}</div> */}
            <div className="purchase-privileges">
                    {
                        privileges && privileges.map(privilege =>
                        <>                                
                            <Privilege Id={privilege.id}
                                    Name={privilege.name} 
                                    Price = {privilege.price + " руб"} 
                                    Image={privilege.imageSource} 
                                    discountPrice={privilege.discountPrice} 
                                    SteamId={props.steamId}
                                    isOwner={props.currentPrivilege && privilege.groupName === props.currentPrivilege.groupName? true:false}
                                    isActive={isActive || privilege.price > priceCurrentPriv.price}
                                    features={privilege.features}
                                    Buy={props.openModal}
                                    Cancel={props.closeModal}>
                            </Privilege>
                        </>
                        
                    )}
            </div>
        </div>
    );
    
}