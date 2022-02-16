import React from 'react'
import { connect } from 'react-redux'
import { PunishmentBans } from './PunishmentBans'
import { modalShow, modalHide } from '../../reducers/modal'

class PunishmentBansContainer extends React.Component {

  render() {
    const { role, OpenModal, CloseModal } = this.props
    
      return (
          <PunishmentBans
            Role={role}
            OpenModal= {OpenModal}
            CloseModal = {CloseModal}
          />
      )
  }
}

const mapStateToProps = store => {
  return {
    role: store.accountInfo.user && store.accountInfo.user.role? store.accountInfo.user.role.id : null
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
)(PunishmentBansContainer)