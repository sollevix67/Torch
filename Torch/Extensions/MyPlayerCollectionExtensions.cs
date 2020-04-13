﻿using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;

namespace Torch
{
    public static class MyPlayerCollectionExtensions
    {
        public static MyPlayer TryGetPlayerBySteamId(this MyPlayerCollection collection, ulong steamId, int serialId = 0)
        {
            var identity = collection.TryGetIdentityId(steamId, serialId);
            if (identity == 0)
                return null;
            if (!collection.TryGetPlayerId(identity, out var playerId))
                return null;

            return collection.TryGetPlayerById(playerId, out var player) ? player : null;
        }
    }
}