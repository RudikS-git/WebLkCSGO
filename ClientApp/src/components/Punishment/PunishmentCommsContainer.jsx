import React from 'react'
import { connect } from 'react-redux'
import { PunishmentComms } from './PunishmentComms'

class PunishmentCommsContainer extends React.Component {

  render() {
    return (
        <PunishmentComms
          Role={this.props.role}
        />
        )
  }
}

const mapStateToProps = store => {
  return {
    role: store.accountInfo.user?.role?.id
  }
}

const mapDispatchToProps = dispatch => {
  return {
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(PunishmentCommsContainer)