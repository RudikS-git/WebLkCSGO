import React from 'react'
import { connect } from 'react-redux'
import { Layout } from './Layout'

class LayoutContainer extends React.Component {
  
  render() {
    const { accountInfo, children } = this.props
    
    if(accountInfo) {
        return (
            <Layout
                user={accountInfo.user}
                children={children}
            />
            )
    }

    return (
        <Layout user={accountInfo.user}/>
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
)(LayoutContainer)