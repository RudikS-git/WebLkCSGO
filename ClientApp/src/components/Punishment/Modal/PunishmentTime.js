import React, { Component } from 'react';
import './PunishmentTime.css';

export class PunishmentTime extends Component {

    constructor(props) {
        super(props);

        this.onChangeTypeBanHandler = this.onChangeTypeBanHandler.bind(this);
        this.onChangeTimeBanHandler = this.onChangeTimeBanHandler.bind(this);

        this.state = { timeBan: null }
    }

    onChangeTypeBanHandler = (e) => {
        this.setState({ typeBan: e.target.value });
    }

    onChangeTimeBanHandler = (e) => {
        this.setState({ timeBan: e.target.value });
    }

    render() {
        return (<div className="adminModal_infoBlock">          
                    <select className="form-control form-control-sm formControlPages" id="selectvalue" onChange={this.onChangeTimeBanHandler} value={Number(this.state.timeBan)}>
                        <option value="0">Навсегда</option>
                        <option className="bansmodal_optionOff" disabled>Месяцы</option>
                        <option className="bansmodal_option">6 месяцев </option>
                        <option className="bansmodal_option">3 месяца </option>
                        <option className="bansmodal_option">2 месяца </option>
                        <option className="bansmodal_option">1 месяц </option>
                        <option className="bansmodal_optionOff" disabled> Недели </option>
                        <option className="bansmodal_option">4 недели </option>
                        <option className="bansmodal_option">3 недели </option>
                        <option className="bansmodal_option">2 неделя </option>
                        <option className="bansmodal_option">1 неделя </option>

                        <option className="bansmodal_optionOff" disabled> Дни </option>
                        <option className="bansmodal_option">30 дней</option>
                        <option className="bansmodal_option">14 дней</option>
                        <option className="bansmodal_option">7 дней</option>
                        <option className="bansmodal_option">3 дня</option>
                        <option className="bansmodal_option">2 дня</option>
                        <option className="bansmodal_option">1 день</option>
                        

                        <option className="bansmodal_optionOff" disabled> Часы </option>
                        <option className="bansmodal_option">23 часа</option>
                        <option className="bansmodal_option">22 часа</option>
                        <option className="bansmodal_option">21 час</option>
                        <option className="bansmodal_option">20 часов</option>
                        <option className="bansmodal_option">19 часов</option>
                        <option className="bansmodal_option">18 часов</option>
                        <option className="bansmodal_option">17 часов</option>
                        <option className="bansmodal_option">16 часов</option>
                        <option className="bansmodal_option">15 часов</option>
                        <option className="bansmodal_option">14 часов</option>
                        <option className="bansmodal_option">13 часов</option>
                        <option className="bansmodal_option">12 часов</option>
                        <option className="bansmodal_option">11 часов</option>
                        <option className="bansmodal_option">10 часов</option>
                        <option className="bansmodal_option">9 часов</option>
                        <option className="bansmodal_option">8 часов</option>
                        <option className="bansmodal_option">7 часов</option>
                        <option className="bansmodal_option">6 часов</option>
                        <option className="bansmodal_option">5 часов</option>
                        <option className="bansmodal_option">4 часа</option>
                        <option className="bansmodal_option">3 часа</option>
                        <option className="bansmodal_option">2 часа</option>
                        <option className="bansmodal_option">1 час</option>

                    </select>

                    <div className="adminModal_strokeBacklight"></div>
                </div>
        );


    }
}