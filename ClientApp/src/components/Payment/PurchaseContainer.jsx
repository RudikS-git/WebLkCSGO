import React from 'react'
import { connect } from 'react-redux'
import { Payment } from './Payment'
import { modalShow, modalHide } from '../../reducers/modal'

class PurchaseContainer extends React.Component {

  render() {
    const { steamId, currentPrivilege, OpenModal, CloseModal } = this.props

    return (
      <Payment
        steamId={steamId}
        currentPrivilege={currentPrivilege}
        openModal={OpenModal}
        closeModal={CloseModal}
      />
    )   
  }
}

const mapStateToProps = store => {
  return {
    steamId: store.accountInfo.user? store.accountInfo.user.steamId : null,
    currentPrivilege: store.accountInfo.user? store.accountInfo.user.privilege : null
  }
}

const mapDispatchToProps = dispatch => {
  return {
    OpenModal: (modalParams) => dispatch(modalShow(modalParams)),
    CloseModal: () => dispatch(modalHide(dispatch))
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(PurchaseContainer)