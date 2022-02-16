import React, { useState, useEffect } from "react";
import { LoadingSpinner } from "./../LoadingSpinner";

import { Modal } from "./Modal/Modal";
import { PunishmentCommsModal } from "./Modal/PunishmentCommsModal";

import { Pages } from "../Manage/Pages";
import { PunishmentSearch } from "./PunishmentSearch";
import { GetPage, Search, UnmuteUser, MuteUser, ChangeCollapse } from '../../api/punishmentComms'

import "./Punishment.css";
import "./PunishmentComms.css";
import { LocalError } from "../Manage/LocalError";

export const PunishmentComms = (props) => {
  const [sourcecomms, setComms] = useState([]);
  const [searchEntity, setSearchEntity] = useState(null);
  const [lengthRows, setLengthRows] = useState(0);
  const [page, setPage] = useState(0);
  const [isFetching, setFetching] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    getPage(0);
  }, [])

  const getPage = (page) => {
    setFetching(true);
    GetPage(page)
    .then(data => 
    {
      data.sourcecomms = data.sourcecomms.map((it, index) => 
      { 
        it.collapse = false; 
        it.num = +index; 
        return it 
        
      });

      setComms(data.sourcecomms);
      setLengthRows(data.lengthRows);
      setPage(page);
    })
    .catch(error => setError(error))
    .finally(() => setFetching(false));
  }

  const setEntity = (searchEntity) => {
    setSearchEntity(searchEntity);
  }

  const getTypeComms = (type) => {
    switch (type) {
      case 1: // Микрофон
        return (
          <svg
            width="2em"
            height="2em"
            viewBox="0 0 16 16"
            className="bi bi-mic-mute"
            fill="currentColor"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              fill-rule="evenodd"
              d="M12.734 9.613A4.995 4.995 0 0 0 13 8V7a.5.5 0 0 0-1 0v1c0 .274-.027.54-.08.799l.814.814zm-2.522 1.72A4 4 0 0 1 4 8V7a.5.5 0 0 0-1 0v1a5 5 0 0 0 4.5 4.975V15h-3a.5.5 0 0 0 0 1h7a.5.5 0 0 0 0-1h-3v-2.025a4.973 4.973 0 0 0 2.43-.923l-.718-.719zM11 7.88V3a3 3 0 0 0-5.842-.963l.845.845A2 2 0 0 1 10 3v3.879l1 1zM8.738 9.86l.748.748A3 3 0 0 1 5 8V6.121l1 1V8a2 2 0 0 0 2.738 1.86zm4.908 3.494l-12-12 .708-.708 12 12-.708.707z"
            />
          </svg>
        );

      case 2: // Чат
        return (
          <svg
            width="2em"
            height="2em"
            viewBox="0 0 16 16"
            className="bi bi-chat-left-dots"
            fill="currentColor"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              fill-rule="evenodd"
              d="M14 1H2a1 1 0 0 0-1 1v11.586l2-2A2 2 0 0 1 4.414 11H14a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 0a2 2 0 0 0-2 2v12.793a.5.5 0 0 0 .854.353l2.853-2.853A1 1 0 0 1 4.414 12H14a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z"
            />
            <path d="M5 6a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm4 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0zm4 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0z" />
          </svg>
        );

      default:
        return "Неизвестно";
    }
  };

  const search = (searchEntity, page) => {

    setFetching(true);

    Search(searchEntity, page)
    .then(data => {

      if(data != null)
      {
        data.sourcecomms = data.sourcecomms.map((it, index) => 
        { 
            it.collapse = false; 
            it.num = +index; 
            return it 
            
        });
      }
        
      setPage(page);
      setComms(data.sourcecomms);
      setLengthRows(data.lengthRows);
    })
    .catch(error => setError(error))
    .finally(() => setFetching(false));
  }

  const changeCollapse = (commsInfo) => {
    sourcecomms[commsInfo.num].collapse = !sourcecomms[commsInfo.num].collapse;
    setComms([...sourcecomms]);
  };

  const renderForecastsTable = () => {
    return (
      <>
        <table className="table punishments__table punishments-comms__table">
          <thead className="punishments__thead">
            <tr className="punishments__tr">
              <th className="punishments__th">Тип</th>
              <th className="punishments__th">Дата</th>
              <th className="punishments__th">Игрок</th>
              <th className="punishments__th">Администратор</th>
              <th className="punishments__th">Причина</th>
              <th className="punishments__th">Статус мута</th>
            </tr>
          </thead>

          <tbody className="punishments__tbody">
            {sourcecomms.map((comms) => (
              <>
                <tr
                  key={comms.id}
                  className="punishments__tr"
                  onClick={(e) => changeCollapse(comms)}
                >
                  <td className="punishments__td comms__type">
                    <span>Тип</span>
                    {getTypeComms(comms.type)}
                  </td>

                  <td className="punishments__td comms__date">
                  <span>Дата</span>
                    {comms.created}
                  </td>

                  <td className="punishments__td punishments__name">
                    <span>Игрок</span>
                    <div>
                      

                      <a
                        className="punishments__name"
                        href={
                          "https://steamcommunity.com/profiles/" +
                          comms.authId64
                        }
                        target="_blank"
                      >
                        {comms.avatar != null && (
                        <img
                          className="punishments__avatars"
                          src={comms.avatar}
                          width="25"
                          height="25"
                        />
                        )}
                        {comms.name}
                      </a>
                    </div>
                  </td>

                  <td className="punishments__td">
                    <span>Администратор</span>
                    <div>
                        

                        <a
                          className="punishments__name"
                          href={
                            "https://steamcommunity.com/profiles/" +
                            comms.adminAuthId
                          }
                          target="_blank"
                        >
                          {comms.avatar != null && (
                          <img
                            className="punishments__avatars"
                            src={comms.adminAvatar}
                            width="25"
                            height="25"
                          />
                          )}
                          {comms.adminName}
                        </a>
                      </div>
                  </td>

                  <td className="punishments__td comms__reason">
                    <span>Причина</span>
                    {comms.reason}
                  </td>

                  {comms.removeType === "" || comms.removeType == undefined ? (
                    comms.created == comms.ends ? (
                      <td className="punishments__td comms__date punishments__per">
                        <span>Длительность</span>
                        Навсегда
                      </td>
                    ) : (
                      <td className="punishments__td comms__date punishments__per">
                        <span>Длительность</span>
                        {comms.ends}
                      </td>
                    )
                  ) : comms.removeType == "E" ? (
                    <td className="punishments__td punishments__date punishments__success">
                      <span>Длительность</span>
                      Истёк
                    </td>
                  ) : (
                    <td className="punishments__td punishments__date punishments__success">
                      <span>Длительность</span>
                      Размучен
                    </td>
                  )}
                </tr>
                <tr className="punishments__additional-addinfo punishments__tr">
                  {comms.collapse == true && (
                    <td
                      className="punishments__additional-addinfo punishments-comms__td-collapse"
                      style={{ padding: 0 }}
                      colspan="7"
                    >
                      <div className="punishments__additional-addinfo">
                        <div className="punishments__additional-header">
                          <p className="punishments__additional-addinfo-text">
                            Дополнительная информация:
                          </p>
                        </div>

                        <div>
                          <p>Администратор:</p>
                          {props.Role === 2 && (
                            <p>
                              IP адрес:{" "}
                              {comms.ip == null || comms.ip == ""
                                ? "Неизвестно"
                                : comms.ip}
                            </p>
                          )}
                          <p>Был выдан: {comms.created}</p>
                          <p>
                            Будет снят:{" "}
                            {comms.ends == comms.created
                              ? "Никогда"
                              : comms.ends}
                          </p>
                          <p>
                            Сервер:{" "}
                            {comms.serverName == null
                              ? "Веб-мут"
                              : comms.serverName}
                          </p>
                        </div>
                      </div>
                    </td>
                  )}
                </tr>
              </>
            ))}
          </tbody>
        </table>
      </>
    );
  };

  if (isFetching) {
    return <LoadingSpinner />;
  }

  if (error) {
    return <LocalError error={error}/>
  }

  return (
    <div>
      <div className="punishment__block">
        <div className="punishment__content">
          <span className="punishment__header-name">{`Список мутов (Всего блокировок: ${lengthRows})`}</span>
          
          <div className="punishment__manage-tools">
            <PunishmentSearch
              searchEntity={searchEntity}
              setSearchEntity={setEntity}
              getRowsOfPage={search}
            />

            <Pages
              page={page}
              count={Math.ceil(lengthRows / 20)}
              getRowsOfPage={(page) =>
                searchEntity === "" || searchEntity == null
                  ? getPage(page)
                  : search(searchEntity, page)
              }
            />
          </div>
          
        </div>
      </div>
      {renderForecastsTable()}
    </div>
  );
};
