import React, { useState, useEffect } from "react";

import { GetPage, Search, UnbanUser, BanUser } from '../../api/punishmentBans'
import { LoadingSpinner } from "./../LoadingSpinner";
import { Pages } from "../Manage/Pages";
import { PunishmentSearch } from "./PunishmentSearch";
import { PunishmentBansModal } from "./Modal/PunishmentBansModal";
import { NotificationManager} from 'react-notifications';
import { Modal } from "./Modal/Modal";
import { Dropdown } from "./../Manage/Dropdown";

import "./Punishment.css";
import "./PunishmentBans.css";
import { LocalError } from "../Manage/LocalError";

export const PunishmentBans = (props) => {
  const [bans, setBans] = useState([]);
  const [lengthRows, setLengthRows] = useState(0);
  const [page, setPage] = useState(0);
  const [searchEntity, setSearchEntity] = useState(null);
  const [isFetching, setFetching] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    getRowsOfPage(0);
  }, []);

  const changeCollapse = (bansInfo) => {
    bans[bansInfo.num].collapse = !bans[bansInfo.num].collapse;
    setBans([...bans])
  };

  const getRowsOfPage = (page) => {
    setFetching(true);

    GetPage(page)
    .then(data => {

      data.sourcebans = data.sourcebans.map((it, index) => 
      { 
          it.collapse = false; 
          it.num = +index; 
          return it 
          
      });
        
      setPage(page);
      setBans(data.sourcebans);
      setLengthRows(data.lengthRows);
    })
    .catch(error => setError(error))
    .finally(() => setFetching(false));
  }

  const search = (searchEntity, page) => {

    setFetching(true);

    Search(searchEntity, page)
    .then(data => {

      console.log(data);
      if(data != null)
      {
        data.sourcebans = data.sourcebans.map((it, index) => 
        { 
            it.collapse = false; 
            it.num = +index; 
            return it 
            
        });
      }
        
      setPage(page);
      setBans(data.sourcebans);
      setLengthRows(data.lengthRows);
    })
    .catch(error => setError(error))
    .finally(() => setFetching(false));
  }

  const setEntity = (searchEntity) => {
    setSearchEntity(searchEntity);
  }

  const renderForecastsTable = () => {

    return (
      <>
        <table className="table punishments__table" id="banslist__table">
          <thead className="punishments__thead">
            <tr className="punishments__tr">
              <th className="punishments__th banlist__created_col">????????</th>
              <th className="punishments__th banlist__name">??????????</th>
              <th className="punishments__th banlist__steamid_col">
                ??????????????????????????
              </th>
              <th className="punishments__th banlist__reason">??????????????</th>
              <th className="punishments__th"></th>
            </tr>
          </thead>

          <tbody className="punishments__tbody">
            {bans &&
              bans.map((bansInfo) => (
                <>
                  <tr
                    key={bansInfo.id}
                    className="punishments__tr"
                    onClick={(e) => changeCollapse(bansInfo)}
                  >
                    {
                      <td className="punishments__td punishments__date">
                        <span>????????</span>
                        {bansInfo.created}
                      </td>
                    }
                    <td className="punishments__td banlist__name">
                      <span>??????????</span>
                      <a
                        className="punishments__name"
                        href={
                          "https://steamcommunity.com/profiles/" +
                          bansInfo.authId64
                        }
                        target="_blank"
                      >
                        {bansInfo.avatar != null && (
                          <img
                            className="punishments__avatars"
                            src={bansInfo.avatar}
                          />
                        )}
                        {bansInfo.name}
                      </a>
                    </td>

                    <td className="punishments__td banlist__steamid_col">
                      <span>??????????????????????????</span>
                      {bansInfo.adminAuthId && bansInfo.adminAuthId != "0" ? (
                        <a
                          className="punishments__name"
                          href={
                            "https://steamcommunity.com/profiles/" +
                            bansInfo.adminAuthId
                          }
                          target="_blank"
                        >
                          {bansInfo.adminAvatar != null && (
                            <img
                              className="punishments__avatars"
                              src={bansInfo.adminAvatar}
                            />
                          )}
                          {bansInfo.adminName}
                        </a>
                      ) : (
                        "CONSOLE"
                      )}
                    </td>

                    <td className="punishments__td banlist__reason">
                      <span>??????????????</span>
                      {bansInfo.reason}
                    </td>

                    {bansInfo.removeType === "" ||
                    bansInfo.removeType == undefined ? (
                      bansInfo.created == bansInfo.ends ? (
                        <td className="punishments__td punishments__date punishments__per">
                          <span>????????????????????????</span>
                          ????????????????
                        </td>
                      ) : (
                        <td className="punishments__td punishments__date punishments__per">
                          <span>????????????????????????</span>
                          {bansInfo.ends}
                        </td>
                      )
                    ) : bansInfo.removeType == "E" ? (
                      <td className="punishments__td punishments__date punishments__success">
                        <span>????????????????????????</span>
                        {bansInfo.ends}
                      </td>
                    ) : (
                      <td className="punishments__td punishments__date punishments__success">
                        <span>????????????????????????</span>
                        ????????????????
                      </td>
                    )}
                  </tr>

                  <tr className="punishments__tr punishments__additional-raddinfo">
                    {bansInfo.collapse == true && (
                      <>
                        <td
                          className="punishments__td punishments__td-collapse punishments__additional-addinfo"
                          style={{ padding: 0 }}
                          colspan="5"
                        >
                          <div className="punishments__additional-addinfo">
                            <div className="banslist__header">
                              <p className="punishments__additional-text">
                                ???????????????????????????? ????????????????????:
                              </p>
                              {
                                (props.Role == 4 || props.Role == 3) && 
                                (<Dropdown
                                    isHeader={false}
                                    items={[
                                      {
                                        name: "???????????????? ???????????????????? ?? ????????",
                                        content: (
                                          <p
                                            onClick={() => {
                                              props.OpenModal({
                                                modalType: PunishmentBansModal,
                                                modalProps: {
                                                  title: "??????????????????",
                                                  text: "?????????????? ????????????",
                                                  subTitle: "????????????????????????",
                                                  hasClose: true,
                                                  buttons: [
                                                    {
                                                      text: "??????????????",
                                                      intent: "success",
                                                      onClick: () => {
                                                        props.OpenModal();
                                                      },
                                                    },
                                                    {
                                                      text: "????????????????",
                                                      onClick: () => {
                                                        props.HideModal();
                                                      },
                                                    },
                                                  ],
                                                  banItem: bansInfo,
                                                },
                                              });
                                            }}
                                          >
                                            ???????????????? ???????????????????? ?? ????????
                                          </p>
                                        ),
                                      },

                                      {
                                        name: "???????????? ????????????????????????????",
                                        content:
                                          bansInfo.removeType == "" ? (
                                            <p
                                              className="punishments__additional-dd-item"
                                              onClick={(e) =>
                                                UnbanUser(bansInfo.id)
                                                .then(data => {getRowsOfPage(page); NotificationManager.success(data)})
                                                .catch(error => NotificationManager.error(error))
                                              }
                                            >
                                              ??????????????????
                                            </p>
                                          ) : (
                                            <p
                                              className="punishments__additional-dd-item"
                                              onClick={(e) =>
                                                BanUser(bansInfo.id)
                                              }
                                            >
                                              ????????????????????
                                            </p>
                                          ),
                                      },
                                    ]}
                                  />
                                )}
                            </div>

                            <div>
                              <div className="punishments__additional-content">
                                <p>??????????????????????????: {bansInfo.adminName}</p>
                                <p>{bansInfo.authId}</p>
                                {(props.Role == 2 || props.Role == 3)&& (
                                  <p>
                                    IP ??????????:{" "}
                                    {bansInfo.ip == null || bansInfo.ip == ""
                                      ? "????????????????????"
                                      : bansInfo.ip}
                                  </p>
                                )}
                                <p>?????? ??????????: {bansInfo.created}</p>
                                <p>
                                  ?????????? ????????:{" "}
                                  {bansInfo.ends == bansInfo.created
                                    ? "??????????????"
                                    : bansInfo.ends}
                                </p>
                                <p>
                                  ????????????:{" "}
                                  {bansInfo.serverName == null
                                    ? "??????-??????"
                                    : bansInfo.serverName}
                                </p>
                                <button>?????????? ?????????????? ????????</button>
                              </div>
                            </div>
                          </div>
                        </td>
                      </>
                    )}
                  </tr>
                </>
              ))}
          </tbody>
        </table>
      </>
    );
  };

  if(isFetching) {
    return <LoadingSpinner />;
  }

  if(error) {
    return <LocalError error={error}/>
  }

  return (
    <div>
      <div className="punishment__block">
        <div className="punishment__content">
          <span className="punishment__header-name">{`???????????? ?????????? (?????????? ????????????????????: ${lengthRows})`}</span>

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
                  ? getRowsOfPage(page)
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
