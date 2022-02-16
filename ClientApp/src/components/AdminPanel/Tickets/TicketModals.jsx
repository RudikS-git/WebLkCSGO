import React, { useState } from 'react';

export const CloseTicketModal = (props) => {
    const [answer, setAnswer] = useState("Был наказан");
    const setTicketState = props.modal.modalProps.setTicketState;
    const closeModal = props.modal.modalProps.closeModal;
  
    const ticketAnswerChange = (e) => setAnswer(e.target.value);
  
    return (
          <div className="admin-panel">
                  <label>Причина:</label>
                  <input name="reason" type="input" value={answer} onChange={ticketAnswerChange}></input>
                  
                  <button type="submit" onClick={() => { setTicketState(answer); closeModal()} }>Подтвердить</button>
          </div>
    );
  };