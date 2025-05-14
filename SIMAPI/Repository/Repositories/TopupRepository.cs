using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Models.Topup;
using SIMAPI.Repository.Interfaces;
using System.Data;

namespace SIMAPI.Repository.Repositories
{
    public class TopupRepository : Repository, ITopupRepository
    {
        public TopupRepository(SIMDBContext context) : base(context)
        {
        }
        public async Task<TopupResponse> ValidateIMEI(string phoneNo, string shopId, string topupAmount)
        {
            TopupResponse result = new TopupResponse();
            //string consString = "Data Source=184.168.47.10;Initial Catalog=AngleSims;Integrated Security=False;Persist Security Info=True;User ID=anil;Password=anil@123;";
            string consString = "Data Source=WIN-4AO2GAUSMUQ;Initial Catalog=Leaptel;User ID=sa;Password=$June$2024*06£05$";
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("Get_TopupCommission_From_Topup_System"))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@imei", phoneNo);
                    cmd.Parameters.AddWithValue("@shopId", shopId);
                    cmd.Parameters.AddWithValue("@topupAmount", topupAmount);
                    cmd.CommandTimeout = 6000;
                    SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    Adap.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result.StatusCode = Convert.ToInt32(dt.Rows[0]["StatusCode"].ToString());
                        result.Description = Convert.ToString(dt.Rows[0]["Description"].ToString());
                        result.SimId = Convert.ToInt32(dt.Rows[0]["SimId"].ToString());
                        result.ICICD = Convert.ToString(dt.Rows[0]["IMEI"].ToString());
                        result.Network = Convert.ToString(dt.Rows[0]["Network"].ToString());
                        result.Commission = Convert.ToString(dt.Rows[0]["Commission"].ToString());
                    }
                }
            }
            return result;
        }

        public async Task<bool> SaveTopup(string phoneNo, string shopId, string topupAmount)
        {
            TopupResponse result = new TopupResponse();
            //string consString = "Data Source=184.168.47.10;Initial Catalog=AngleSims;Integrated Security=False;Persist Security Info=True;User ID=anil;Password=anil@123;";
            string consString = "Data Source=WIN-4AO2GAUSMUQ;Initial Catalog=Leaptel;User ID=sa;Password=$June$2024*06£05$";
            var isSaved = false;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("SaveTopup"))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@imei", phoneNo);
                    cmd.Parameters.AddWithValue("@shopId", shopId);
                    cmd.Parameters.AddWithValue("@topupAmount", topupAmount);
                    cmd.CommandTimeout = 6000;
                    SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    Adap.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        isSaved = Convert.ToBoolean(dt.Rows[0]["Status"].ToString());
                    }
                }
            }
            return isSaved;
        }



    }
}
