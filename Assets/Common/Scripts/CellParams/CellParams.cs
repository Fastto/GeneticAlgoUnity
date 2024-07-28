using System.Collections.Generic;

namespace Common.Scripts.CellParams
{
    public static class CellParams
    {
        private static List<CellBoolParams> m_AllBoolParams = new List<CellBoolParams>
        {
            CellBoolParams.IsParasite
        };
        
        private static List<CellFloatParams> m_AllFloatParams = new List<CellFloatParams>
        {
            CellFloatParams.Energy,
            CellFloatParams.BirthTime
        };

        public static Dictionary<CellBoolParams, bool> GetBoolParams()
        {
            var dict = new Dictionary<CellBoolParams, bool>();
            foreach (var param in m_AllBoolParams)
            {
                dict.Add(param, false);
            }

            return dict;
        }
        
        public static Dictionary<CellFloatParams, float> GetFloatParams()
        {
            var dict = new Dictionary<CellFloatParams, float>();
            foreach (var param in m_AllFloatParams)
            {
                dict.Add(param, 0f);
            }

            return dict;
        }
    }
}