import React from 'react'
import { connect } from 'react-redux'
import { AccountInfo } from './AccountInfo'
import { GetAccountInfo } from '../../reducers/accountInfo'

class AccountInfoContainer extends React.Component {

  componentDidMount() {
    this.props.GetAccountInfo();
  }

  render() {
    const { accountInfo, GetAccountInfo } = this.props

    if(accountInfo)
    {
      return (
        <AccountInfo
          user={accountInfo.user}
          error={accountInfo.error}
          isFetching={accountInfo.isFetching}
          GetAccountInfo={() => GetAccountInfo()}
        />
      )
    }
    else
    {
      return <AccountInfo
      user={null}
      error={null}
      isFetching={true}
      GetAccountInfo={() => GetAccountInfo()}
    />
    }
    
  }
}

const mapStateToProps = store => {
  return {
    accountInfo: store.accountInfo,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    GetAccountInfo: () => dispatch(GetAccountInfo()),
  }
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(AccountInfoContainer)