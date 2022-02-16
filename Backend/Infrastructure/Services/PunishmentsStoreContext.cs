using System;
using System.Threading.Tasks;
using System.Data.Common;

using MySql.Data.MySqlClient;
using SteamIDs_Engine;
using System.Collections.Generic;
using Domain.Context;
using Domain.Entities.Punishments;
using Infrastructure.SourceQuery.Models.ServerInfo;
using Infrastructure.SteamID;
using Infrastructure.Unix;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class PunishmentsStoreContext : StoreContext
    {
        public PunishmentsStoreContext(string connection) : base(connection) { }

        public async Task<BansInfo> GetBanList(ServersInfo servers, uint page)
        {
            //BansInfo bansInfo = new BansInfo();
            MySqlConnection mySqlConnection = GetConnection();
            MySqlCommand mySqlCommand = new MySqlCommand($"SELECT SUM(1) FROM `sb_bans`;" +
                                                                     $"SELECT `bid`, bans.ip, `authid`, bans.name, `created`, `ends`, `reason`, `RemoveType`, `aid`, vips.name, servers.ip, servers.port FROM `sb_bans` AS `bans` " +
                                                                     "LEFT JOIN `sb_servers` AS `servers` ON bans.sid = servers.sid " +
                                                                     "LEFT JOIN `vip_users` AS `vips` ON vips.account_id = aid " +
                                                                     "ORDER BY `created` DESC " +
                                                                     $"LIMIT 20 OFFSET {page * 20}");
            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                mySqlCommand.Connection = mySqlConnection;

                using (var reader = await mySqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        var bans = await ResponseBansReader(reader, servers);

                        return bans;
                    }

                    return null;
                }


            }
        }

        public async Task<BansInfo> GetSearchEntOfBans(ServersInfo servers, string entity, uint page)
        {
            MySqlConnection mySqlConnection = GetConnection();
            MySqlCommand mySqlCommand = new MySqlCommand($"SELECT COUNT(*) FROM (SELECT `bid`, `authid`, `name`, `created`, `ends`, `reason`, `RemoveType` FROM `sb_bans` " +
                                                                                $"WHERE `authid` LIKE '%{entity}%' OR `name` LIKE '%{entity}%') AS `searchRows`;" +

                                                            $"SELECT `bid`, bans.ip, `authid`, bans.name, `created`, `ends`, `reason`, `RemoveType`, `aid`, vips.name, servers.ip, servers.port FROM `sb_bans` AS `bans` " +
                                                            "LEFT JOIN `sb_servers` AS `servers` ON bans.sid = servers.sid " +
                                                            $"LEFT JOIN `vip_users` AS `vips` ON vips.account_id = aid " +
                                                            $"WHERE `authid` LIKE '%{entity}%' OR bans.name LIKE '%{entity}%' " +
                                                            $"ORDER BY `created` DESC " +
                                                            $"LIMIT 20 OFFSET {page * 20}");

            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                mySqlCommand.Connection = mySqlConnection;

                using (var reader = await mySqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        var bans = await ResponseBansReader(reader, servers);

                        return bans;
                    }

                    return null;
                }


            }
        }

        public async Task<BansInfo> ResponseBansReader(DbDataReader reader, ServersInfo servers)
        {
            BansInfo json = new BansInfo(); // jsonModel as BansInfo;

            if (await reader.ReadAsync())
            {
                json.length = Convert.ToInt64(reader[0]);
            }
            else
            {
                return null;
            }

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    json.bansInfo.Add(new BanInfo()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Ip = reader[1]?.ToString(),
                        AuthId = reader[2]?.ToString(),
                        Name = reader[3]?.ToString(),
                        Created = DateTimeUnix.UnixTimeStampToDateTime(Convert.ToInt32(reader[4])).ToString("g", System.Globalization.CultureInfo.GetCultureInfo("ru-ru")),
                        Ends = DateTimeUnix.UnixTimeStampToDateTime(Convert.ToInt32(reader[5])).ToString("g", System.Globalization.CultureInfo.GetCultureInfo("ru-ru")),
                        Reason = reader[6]?.ToString(),
                        RemoveType = reader[7]?.ToString(),
                        AuthId64 = SteamIDConvert.Steam2ToSteam64(reader[2]?.ToString()).ToString(),
                        AdminAuthId = SteamIDConvert.Steam32ToSteam64(Convert.ToInt32(reader[8])).ToString(),
                        AdminName = reader[9].ToString(),
                        ServerName = (reader[10] == DBNull.Value || reader[11] == DBNull.Value) ? "Веб-бан" : servers.Servers.Find(it => it.Ip.ToString() == reader[10].ToString() && it.Port.ToString() == reader[11].ToString())?.Name

                    // Avatar = await SteamAPI.GetUserAvatar(SteamIDConvert.Steam2ToSteam64(reader[2].ToString()))
                });


                }

                await SteamAPI.GetUserAvatarsAsync<BanInfo>(json.bansInfo);

            }

            return json;
        }

        public async Task<bool> UnbanUser(int? id)
        {
            var command = new MySqlCommand("UPDATE `sb_bans` SET `RemoveType` = 'U' WHERE `bid` = @id");
            command.Parameters.AddWithValue("id", id);

            return await ExecuteCommandNonQueryAsync(GetConnection(), command);
        }

        public async Task<bool> BanUser(int? id)
        {
            var command = new MySqlCommand("UPDATE `sb_bans` SET `RemoveType` = '' WHERE `bid` = @id");
            command.Parameters.AddWithValue("id", id);

            return await ExecuteCommandNonQueryAsync(GetConnection(), command);
        }

        public async Task<bool> EditBanInfo(uint id, string name, int typeBan, string steamid, string ip, int time, string reason)
        {
            var command = new MySqlCommand("UPDATE `sb_bans` SET `name` = @name, `type` = @typeBan, `authid` = @steamid, `ip` = @ip, `ends` = `created` + @time, reason` = @reason WHERE `bid` = @id");
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("type", typeBan);
            command.Parameters.AddWithValue("steamid", steamid);
            command.Parameters.AddWithValue("ip", ip);
            command.Parameters.AddWithValue("time", time);
            command.Parameters.AddWithValue("reason", reason);


            return await ExecuteCommandNonQueryAsync(GetConnection(), command);
        }
        
        public async Task<bool> EditCommsInfo(uint id, string name, int type, string steamid, int time, string reason)
        {
            var command = new MySqlCommand("UPDATE `sb_comms` SET `name` = @name, `type` = @type, `authid` = @steamid, `ends` = `created` + @time, `reason` = @reason WHERE `bid` = @id");
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("type", type);
            command.Parameters.AddWithValue("steamid", steamid);
            command.Parameters.AddWithValue("time", time);
            command.Parameters.AddWithValue("reason", reason);


            return await ExecuteCommandNonQueryAsync(GetConnection(), command);
        }

        public async Task<CommsInfo> GetCommsList(ServersInfo servers, uint page)
        {
            MySqlConnection mySqlConnection = GetConnection();
            MySqlCommand mySqlCommand = new MySqlCommand($"SELECT SUM(1) FROM `sb_comms`;" +
                                                                      $"SELECT `bid`, `authid`, comms.name, `created`, `ends`, `reason`, `type`, `RemoveType`, `aid`, vips.name, servers.ip, servers.port FROM `sb_comms` AS comms " +
                                                                      $"LEFT JOIN `sb_servers` AS `servers` ON comms.sid = servers.sid " +
                                                                      $"LEFT JOIN `vip_users` AS `vips` ON vips.account_id = aid " +
                                                                      $"ORDER BY `created` DESC LIMIT 20 OFFSET {page * 20}");

            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                mySqlCommand.Connection = mySqlConnection;

                using (var reader = await mySqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        var comms = await ResponseCommsReader(reader, servers);

                        return comms;
                    }

                    return null;
                }


            }
        }

        public async Task<CommsInfo> GetSearchEntOfComms(ServersInfo servers, string entity, uint page)
        {
            MySqlConnection mySqlConnection = GetConnection();
            MySqlCommand mySqlCommand = new MySqlCommand($"SELECT COUNT(*) FROM (SELECT `bid`, `authid`, `name`, `created`, `ends`, `reason`, `type`, `RemoveType` FROM `sb_comms` " +
                                                                                            $"WHERE `authid` LIKE '%{entity}%' OR `name` LIKE '%{entity}%') AS `searchRows`;" +

                                                                      $"SELECT `bid`, `authid`, comms.name, `created`, `ends`, `reason`, `type`, `RemoveType`, `aid`, vips.name, servers.ip, servers.port FROM `sb_comms` AS comms " +
                                                                      "LEFT JOIN `sb_servers` AS `servers` ON comms.sid = servers.sid " +
                                                                      $"LEFT JOIN `vip_users` AS `vips` ON vips.account_id = aid " +
                                                                      $"WHERE `authid` LIKE '%{entity}%' OR comms.name LIKE '%{entity}%' " +
                                                                      $"ORDER BY `created` DESC " +
                                                                      $"LIMIT 20 OFFSET {page * 20}");

            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                mySqlCommand.Connection = mySqlConnection;

                using (var reader = await mySqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        var comms = await ResponseCommsReader(reader, servers);

                        return comms;
                    }

                    return null;
                }


            }
        }

        public async Task<CommsInfo> ResponseCommsReader(DbDataReader reader, ServersInfo servers)
        {
            CommsInfo json = new CommsInfo();

            if (await reader.ReadAsync())
            {
                json.length = Convert.ToInt64(reader[0]);
            }
            else
            {
                return null;
            }

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    json.commsInfo.Add(new CommInfo()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        AuthId = reader[1].ToString(),
                        Name = reader[2].ToString(),
                        Created = DateTimeUnix.UnixTimeStampToDateTime(Convert.ToInt32(reader[3])).ToString("g", System.Globalization.CultureInfo.GetCultureInfo("ru-ru")),
                        Ends = DateTimeUnix.UnixTimeStampToDateTime(Convert.ToInt32(reader[4])).ToString("g", System.Globalization.CultureInfo.GetCultureInfo("ru-ru")),
                        Reason = reader[5].ToString(),
                        Type = Convert.ToByte(reader[6]),
                        RemoveType = reader[7] != DBNull.Value? reader[7].ToString() : null,
                        AuthId64 = SteamIDConvert.Steam2ToSteam64(reader[1].ToString()).ToString(),
                        AdminAuthId = SteamIDConvert.Steam32ToSteam64(Convert.ToInt32(reader[8])).ToString(),
                        AdminName = reader[9].ToString(),
                        ServerName = (reader[10] == DBNull.Value || reader[11] == DBNull.Value) ? "Веб-мут" : servers.Servers.Find(it => it.Ip.ToString() == reader[10].ToString() && it.Port.ToString() == reader[11].ToString())?.Name
                    });
                }

                await SteamAPI.GetUserAvatarsAsync<CommInfo>(json.commsInfo);
            }

            return json;
        }

    }
}

