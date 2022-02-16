import React, { Component } from 'react';
import { } from 'react-bootstrap';

import './AccountProfile.css';

export const AccountProfile = (props) => {
 //   const [ip, setIp] = useState(props.modal.modalProps.server.ip);
  
    return (
          <div className="admin-panel">
                  <input name="id" type="hidden" value={id}></input>
                  <input name="ip" type="input" value={ip} onChange={handleChangeIp}></input>
                  <label>IP:</label>
  
                  
                  <input name="port" type="input" value={port} onChange={handleChangePort}></input>
                  <label>Port:</label>
                  
                  <button type="submit" onClick={() => ChangeServer({id, ip, port})}>Добавить</button>
          </div>
    );
  };