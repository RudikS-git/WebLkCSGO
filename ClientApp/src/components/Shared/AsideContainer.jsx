import React from 'react'
import { connect } from 'react-redux'
import { Aside } from './Aside'

class AsideContainer extends React.Component {

  render() {
    const { accountInfo, children } = this.props
    
    if(accountInfo) {
        return (
            <Aside
                user={accountInfo.user}
            />
        )
    }

    return (
        <></>
    )
  }
}

const mapStateToProps = store => {
    return {
        accountInfo: store.accountInfo,
    }
}

const mapDispatchToProps = dispatch => {
  return {

  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(AsideContainer)