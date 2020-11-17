using System;
using System.Collections.Generic;
using System.Data;

namespace SqlLib
{
    public class MsSqlDbTypeMap
    {
        private static Dictionary<Type, SqlDbType> _typeMap;
        public MsSqlDbTypeMap()
        {
            SetTypeMap();
        }

        private static void SetTypeMap()
        {
            if (_typeMap == null)
            {
                _typeMap = new Dictionary<Type, SqlDbType>();
                _typeMap[typeof(Guid)] = SqlDbType.UniqueIdentifier;
                _typeMap[typeof(string)] = SqlDbType.NVarChar;
                _typeMap[typeof(String)] = SqlDbType.NVarChar;
                _typeMap[typeof(char[])] = SqlDbType.NVarChar;
                _typeMap[typeof(byte)] = SqlDbType.TinyInt;
                _typeMap[typeof(short)] = SqlDbType.SmallInt;
                _typeMap[typeof(int)] = SqlDbType.Int;
                _typeMap[typeof(Int16)] = SqlDbType.Int;
                _typeMap[typeof(Int32)] = SqlDbType.Int;
                _typeMap[typeof(Int64)] = SqlDbType.Int;
                _typeMap[typeof(long)] = SqlDbType.BigInt;
                _typeMap[typeof(byte[])] = SqlDbType.Image;
                _typeMap[typeof(bool)] = SqlDbType.Bit;
                _typeMap[typeof(DateTime)] = SqlDbType.DateTime2;
                _typeMap[typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset;
                _typeMap[typeof(decimal)] = SqlDbType.Money;
                _typeMap[typeof(Decimal)] = SqlDbType.Money;
                _typeMap[typeof(float)] = SqlDbType.Real;
                _typeMap[typeof(double)] = SqlDbType.Float;
                _typeMap[typeof(TimeSpan)] = SqlDbType.Time;
            }
        }
        public static SqlDbType GetDbType(Type giveType)
        {
            SetTypeMap();
            giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;

            if (_typeMap.ContainsKey(giveType))
            {
                return _typeMap[giveType];
            }
            throw new ArgumentException("{giveType.FullName} is not a supported .NET class");
        }
    }
}
