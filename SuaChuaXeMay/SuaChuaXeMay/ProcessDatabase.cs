using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuaChuaXeMay
{
    internal class ProcessDatabase
    {
        SqlConnection con = new SqlConnection(@"Data Source=DUNGLE\DUNGLEE;Initial Catalog=SuaChuaXeMay;Integrated Security=True;Encrypt=False");
        public void KetNoi()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void DongKetNoi()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }

        public DataTable DocBang(string sql)
        {
            DataTable tb = new DataTable();
            KetNoi();
            SqlDataAdapter ad = new SqlDataAdapter(sql, con);
            ad.Fill(tb);
            DongKetNoi();
            return tb;
        }

        public void CapNhat(string sql)
        {
            KetNoi();
            SqlCommand cm = new SqlCommand();
            cm.Connection = con;
            cm.CommandText = sql;
            cm.ExecuteNonQuery();
            DongKetNoi();
            cm.Dispose();
        }
    }
}

