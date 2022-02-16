import React from 'react'
import { connect } from 'react-redux'
import { GetTickets } from '../../../api/tickets'
import { Tickets } from './Tickets';
import { modalShow, modalHide } from '../../../reducers/modal'
import { setConnection } from '../../../reducers/signalR'

class TicketsContainer extends React.Component {

    render() {

      return <Tickets token={this.props.token} GetTickets={GetTickets} setConnection={this.props.setConnection} connection={this.props.connection}/>       
 
    }
}   

const mapStateToProps = store => {

  return {
    connection: store.signalR.connection,
    token: store.accountInfo.token
  }
}

const mapDispatchToProps = dispatch => {
  return {
    setConnection: (action) => dispatch(setConnection(action))
  //  DeleteServer: (id) => dispatch(DeleteServer(id)),
  //  OpenModal: (modalParams) => dispatch(modalShow(modalParams)),
   // AddServer: (ip, port) => dispatch(AddServer(ip, port)),
  //  ChangeServer: (server) => dispatch(ChangeServer(server)),
   // CloseModal: () => dispatch(modalHide(dispatch)),
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(TicketsContainer)