using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperatingManagement.Framework.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using System.Data;
using Oracle.DataAccess.Client;

namespace OperatingManagement.DataAccessLayer.PlanManage
{
    /// <summary>
    /// 序列号
    /// </summary>
    public class Sequence
    {

        public Sequence()
        {
            _database = OracleDatabase.FromConfiguringNode("ApplicationServices");
        }

        /// <summary>
        /// 获取YY研究工作计划编号
        /// </summary>
        /// <returns></returns>
        public int GetYJJHSequnce()
        {
            return GetSequence("seq_tb_yjjh");
        }
        /// <summary>
        /// 获取KJ信息需求编号
        /// </summary>
        /// <returns></returns>
        public int GetXXXQSequnce()
        {
            return GetSequence("seq_tb_xxxq");
        }
        /// <summary>
        /// 获取ZZ DMZ工作计划编号
        /// </summary>
        /// <returns></returns>
        public int GeZZGZJHSequnce()
        {
            return GetSequence("seq_tb_zzgzjh");
        }
        /// <summary>
        /// 获取CK资源需求编号
        /// </summary>
        /// <returns></returns>
        public int GetDJZYSQSequnce()
        {
            return GetSequence("seq_tb_djzysq");
        }
        /// <summary>
        /// 获取ZC DMZ工作计划编号
        /// </summary>
        /// <returns></returns>
        public int GetGZJHSequnce()
        {
            return GetSequence("seq_tb_gzjh");
        }
        /// <summary>
        /// 获取中心运行计划编号
        /// </summary>
        /// <returns></returns>
        public int GetZXJHSequnce()
        {
            return GetSequence("seq_tb_zxjh");
        }
        /// <summary>
        /// 获取FZTY试验数据编号
        /// </summary>
        /// <returns></returns>
        public int GetTYSJSequnce()
        {
            return GetSequence("seq_tb_tysj");
        }


        /// <summary>
        /// 生成计划编号
        /// </summary>
        /// <param name="seqname"></param>
        /// <returns></returns>
        public int GetSequence(string seqname)
        {
            int result = 1;
            OracleParameter seqnum = new OracleParameter
            {
                ParameterName = "o_seqnum",
                Direction = ParameterDirection.Output,
                OracleDbType = OracleDbType.Int32
            };

            DataSet ds = _database.SpExecuteDataSet("up_gen_sequence", new OracleParameter[]{
                new OracleParameter("p_seqname",seqname),
                seqnum
            });

            if (seqnum.Value != null && seqnum.Value != DBNull.Value)
                result = Convert.ToInt32(seqnum.Value);
            return result;
        }

        #region -Properties-
        private OracleDatabase _database = null;
        #endregion
    }
}