import React, { Component } from 'react';

import 'bootstrap/dist/css/bootstrap.css';

import { PunishmentTime } from './PunishmentTime';
import './PunishmentBansModal.css';

export class PunishmentBansModal extends Component {

    constructor(props) {
        super(props);

        const { modal: { modalProps } } = this.props;

        console.log(this.props);
        this.state = {  name: modalProps.banItem.name,
                        steamId: modalProps.banItem.authId,
                        ip: modalProps.banItem.ip,
                        start: modalProps.banItem.created.slice(0, modalProps.banItem.created.indexOf(' ')),
                        starttime: modalProps.banItem.created.slice(modalProps.banItem.created.indexOf(' '), modalProps.banItem.created.length),
                        end: modalProps.banItem.ends.slice(0, modalProps.banItem.ends.indexOf(' ')),
                        endtime: modalProps.banItem.ends.slice(modalProps.banItem.ends.indexOf(' '), modalProps.banItem.ends.length),
                        typeBan: null,
                        timeBan: null,
                        reason: modalProps.banItem.reason
                     }

        this.onChangeTypeBanHandler = this.onChangeTypeBanHandler.bind(this);
        this.onChangeTimeBanHandler = this.onChangeTimeBanHandler.bind(this);
    }

    onChangeTypeBanHandler = (e) => {
        this.setState({ typeBan: e.target.value });
    }

    onChangeTimeBanHandler = (e) => {
        this.setState({ timeBan: e.target.value });
    }

    render() {
        console.log(this.props);

        return (<form className="punishmentBansModal">

                    <div className="adminModal_block">
                        <label className="adminModal_textLine">Ник игрока</label>
                        
                        <div className="adminModal_infoBlock">
                            <input type="text" onChange={(event) => this.setState({name: event.target.value})} className="adminModal_textInput" ref="name" value={this.state.name} required></input>
                            <div className="adminModal_strokeBacklight"></div>
                        </div>
                    </div>

                    <div className="adminModal_block">
                        <label className="adminModal_textLine">Тип бана</label>
                        <div className="adminModal_infoBlock">
                            {/* <input type="date" onChange={(event) => this.setState({start: event.target.value})} className="adminModal_textInput" ref="start" value={this.state.start} required></input> */}
                            
                            <select className="form-control form-control-sm formControlPages" id="selectvalue" onChange={this.onChangeTypeBanHandler} value={Number(this.state.typeBan)}>
                                <option value="0">STEAM ID</option>
                                <option value="1">IP</option>
                            </select>

                            <div className="adminModal_strokeBacklight"></div>
                        </div>
                    </div>

                    <div className="adminModal_block">
                        <label className="adminModal_textLine">STEAM ID</label>

                        <div className="adminModal_infoBlock">
                            <input type="text" onChange={(event) => this.setState({steamId: event.target.value})} className="adminModal_textInput" ref="steamId" value={this.state.steamId} required></input>
                            <div className="adminModal_strokeBacklight"></div>
                        </div>
                    </div>

                    <div className="adminModal_block">
                        <label className="adminModal_textLine">IP</label>

                        <div className="adminModal_infoBlock">
                            <input type="text" onChange={(event) => this.setState({ip: event.target.value})} className="adminModal_textInput" ref="ip" value={this.state.ip} minlength="7" maxlength="15" size="15" required></input>
                            <div className="adminModal_strokeBacklight"></div>
                        </div>
                        
                    </div>

                    <div className="adminModal_block">
                        <label className="adminModal_textLine">Причина</label>

                        <div className="adminModal_infoBlock">
                            <input type="text" onChange={(event) => this.setState({reason: event.target.value})} className="adminModal_textInput" ref="reason" value={this.state.reason} required></input>
                            <div className="adminModal_strokeBacklight"></div>
                        </div>
                        
                    </div>

                    <div className="adminModal_block">
                        <label className="adminModal_textLine">Время бана:</label>
                        
                        <PunishmentTime />
                        
                    </div>
                
              </form>
            
        );


    }
}