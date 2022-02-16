import React from 'react';
import { Redirect, Route } from 'react-router';
import { connect } from 'react-redux'

import { Forbidden } from './components/Forbidden';

const PrivateRoute = ({ component: Component, store, ...rest }) => {
    //store.getState()
    const user = rest.accountInfo.user;

    if(!user) {
        return <></>;
    }

    return <Route {...rest}
            render={props => (user.role && user.role.id != 1) ? 
                            <Component {...props}/> : <Forbidden/> }
    />
}

const mapStateToProps = store => {
    return {
        accountInfo: store.accountInfo,
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
)(PrivateRoute)