import React from 'react'
import { connect } from 'react-redux'
import { GetTickets, GetTicket, SetTicketState, GetTicketsByUser } from '../../../api/tickets'
import { Tickets } from './Tickets';
import { modalShow, modalHide } from '../../../reducers/modal'
import { TicketManage } from './TicketManage';

class TicketManageContainer extends React.Component {

    render() {
        const { OpenModal, CloseModal, checkingUserId } = this.props;

        return(<TicketManage GetTicket={GetTicket} GetTicketsByUser={GetTicketsByUser} GetTickets={GetTickets} setTicketState={SetTicketState} {... this.props}/>)         
    }
}   

const mapStateToProps = store => {
  return {
    checkingUserId: store.accountInfo.user.steamId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    OpenModal: (modalParams) => dispatch(modalShow(modalParams)),
    CloseModal: () => dispatch(modalHide(dispatch)),
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TicketManageContainer)