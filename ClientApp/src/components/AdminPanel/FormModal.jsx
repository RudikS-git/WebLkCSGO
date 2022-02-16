import React, { useState } from 'react';
import './Servers.css'; 

export const FormModal = (props) => {
  const [ip, setIp] = useState(props.modal.modalProps.server.ip);
  const [port, setPort] = useState(props.modal.modalProps.server.port);
  const id = props.modal.modalProps.server.id;
  const ChangeServer = props.modal.modalProps.ChangeServer;

  const handleChangeIp = (e) => setIp(e.target.value);
  const handleChangePort = (e) => setPort(e.target.value);

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

export const ServersAddModal = (props) => {
    const [ip, setIp] = useState();
    const [port, setPort] = useState();
    const AddServer = (ip, port) => props.modal.modalProps.AddServer(ip, port)
  
    const handleChangeIp = (e) => setIp(e.target.value);
    const handleChangePort = (e) => setPort(e.target.value);
  
    return (
          <div className="admin-panel">
                <input name="ip" type="input" value={ip} onChange={handleChangeIp}></input>
                <label>IP:</label>

                
                <input name="port" type="input" value={port} onChange={handleChangePort}></input>
                <label>Port:</label>
                
                <button type="submit" onClick={() => AddServer(ip, port)}>Добавить</button>
          </div>
    );
  };