import React from 'react'
import { connect } from 'react-redux'
import { GetServers, DeleteServer, AddServer, ChangeServer } from '../../../api/servers'
import { Servers } from './Servers';
import { modalShow, modalHide } from '../../../reducers/modal'

class ServersContainer extends React.Component {

    render() {
        const { OpenModal, CloseModal } = this.props;

        
        return(
        <Servers
            GetServers = {GetServers}
            DeleteServer = {DeleteServer}
            AddServer = {AddServer}
            ChangeServer = {ChangeServer}
            OpenModal= {OpenModal}
            CloseModal = {CloseModal}
        />)         
        
        
    }
}   

const mapStateToProps = store => {
  return {
    
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
)(ServersContainer)