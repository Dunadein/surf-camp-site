using SurfLevel.Contracts.Models.DatabaseObjects;
using System;

namespace SurfLevel.Domain.Fetching
{
    internal static class PrimaryKeyStrategy
    {
        public static Func<TDataBaseObject, bool> GetById<TDataBaseObject>(int id)
            where TDataBaseObject : IPrimaryKeyObject
            => p => p.Id == id;
    }
}
