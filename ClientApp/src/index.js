import React from 'react';
import ReactDOM from 'react-dom';

import { Provider } from 'react-redux'

import { store } from './configureStore';

import App from './App';

import registerServiceWorker from './registerServiceWorker';
import ModalContainer from './components/Modal/ModalContainer';

ReactDOM.render(
  <Provider store={store}> 
      <App/>
      <ModalContainer/>
  </Provider>,
  document.getElementById('root'));

//registerServiceWorker();
