﻿namespace Infrastructure.SourceQuery.Models.ServerInfo
{
    public class EnumsServer
    {
        public enum Engine
        {
            Goldsource,
            Source
        }

        public enum ServerType
        {
            Dedicated,
            NonDedicated,
            SourceTvRelay
        }

        public enum Environment
        {
            Linux,
            Windows,
            Mac
        }

        public enum TheShipMode
        {
            Hunt,
            Elimination,
            Duel,
            Deathmatch,
            VipTeam,
            TeamElimination
        }

        public enum Visibility
        {
            Public,
            Private
        }
    }
}
