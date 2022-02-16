import React, { Component } from 'react';

import 'bootstrap/dist/css/bootstrap.css';

import { PunishmentTime } from './PunishmentTime';
import './PunishmentCommsModal.css';

export class PunishmentCommsModal extends Component {

    constructor(props) {
        super(props);
        this.state = {  name: this.props.content.name,
                        steamId: this.props.content.authId,
                        ip: this.props.content.ip,
                        start: this.props.content.created.slice(0, this.props.content.created.indexOf(' ')),
                        starttime: this.props.content.created.slice(this.props.content.created.indexOf(' '), this.props.content.created.length),
                        end: this.props.content.ends.slice(0, this.props.content.ends.indexOf(' ')),
                        endtime: this.props.content.ends.slice(this.props.content.ends.indexOf(' '), this.props.content.ends.length),
                        typeBan: null,
                        timeBan: null
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
                            
                            <select className="form-control form-control-sm formControlPages" id="selectvalue" onChange={this.onChangeTypeBanHandler} value={Number(this.state.typeBan)}>
                                <option value="0">Микрофон</option>
                                <option value="1">Чат</option>
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
                        <label className="adminModal_textLine">Время мута:</label>
                        <PunishmentTime /> 
                    </div>

              </form>
            
        );


    }
}