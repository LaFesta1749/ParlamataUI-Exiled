using Exiled.API.Features;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParlamataUI.XPSystem
{
    public static class XPDatabase
    {
        private static readonly string DbPath = Path.Combine(Paths.Configs, "XPSystem.db");
        private static readonly string CollectionName = "players";

        private static LiteDatabase? _db;
        private static ILiteCollection<PlayerXP>? _collection;

        public static void Initialize()
        {
            if (_db != null && _collection != null)
                return;

            _db = new LiteDatabase(DbPath);
            _collection = _db.GetCollection<PlayerXP>(CollectionName);
            _collection.EnsureIndex(x => x.UserId);
        }

        public static PlayerXP Get(string userId)
        {
            Initialize();

            var player = _collection!.FindOne(x => x.UserId == userId);
            if (player == null)
            {
                player = new PlayerXP { UserId = userId };
                _collection.Insert(player);
            }

            return player;
        }

        public static void Save(PlayerXP data)
        {
            Initialize();
            _collection!.Update(data);
        }

        public static void ResetAll()
        {
            Initialize();
            _collection!.DeleteAll();
        }

        public static IEnumerable<PlayerXP> GetAll()
        {
            Initialize();
            return _collection!.FindAll();
        }
    }
}
