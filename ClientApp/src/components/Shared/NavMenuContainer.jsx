import React from 'react'
import { connect } from 'react-redux'
import { NavMenu } from './NavMenu'


class NavMenuContainer extends React.Component {

  render() {
    const { user, isFetching, manageNavMenu, children } = this.props

    if(isFetching) {
      return (
        <NavMenu
            isAuth={null}
        />
      )
    }

    if(user) {
      return (
        <NavMenu
            isAuth={user.isAuthenticated}
        />
      )
    }

    return (
      <NavMenu
          isAuth={false}
      />
    )
  }
}

const mapStateToProps = store => {
  console.log(store)
    return {
        user: store.accountInfo.user,
        isFetching: store.accountInfo.isFetching
    }
}

const mapDispatchToProps = dispatch => {
  return { 
    //GetAccountInfo: () => dispatch(GetAccountInfo()),
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(NavMenuContainer)