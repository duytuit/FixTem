using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixLabel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if(0<txtPartNo.Text.Length&&txtPartNo.Text.Length==9)
            {
                using (var ParadigmDB=new Entities())
                {
                   int CheckPartNo= ParadigmDB.DATA0050.Where(x=>x.CUSTOMER_PART_NUMBER.Contains(txtPartNo.Text)).Count();

                    if(CheckPartNo>0)
                    {
                        int countPartNo = ParadigmDB.T_BARCODE_FIXPROD_ID.Where(x => x.CUST_PART_FIX==txtPartNo.Text).Count();

                        if (countPartNo==0)
                        {
                            OracleConnection oraConn = new OracleConnection();
                            string connString = "User Id=ADMIN; Password=demo; Data Source=fnsystem:1521/live";
                            oraConn.ConnectionString = connString;
                            oraConn.Open();
                            OracleCommand cmd = new OracleCommand();
                           
                            cmd.Connection = oraConn;
                            OracleTransaction myTrans = oraConn.BeginTransaction();
                            cmd.Transaction = myTrans;
                            
                            try
                            {
                                string sqlInsert = "insert into T_BARCODE_FIXPROD_ID (cust_part_fix) values('" + txtPartNo.Text + "')";
                                cmd.CommandText = sqlInsert;
                                cmd.ExecuteNonQuery();
                                myTrans.Commit();
                                cmd.Dispose();
                                button1.Enabled = true;
                                txtPartNo.Text = null;
                                MessageBox.Show("Thành Công!");
                            }
                            catch 
                            {
                                myTrans.Rollback();
                                MessageBox.Show("Thất Bại!");
                            }
                            finally
                            {
                                oraConn.Close();
                            }
                        }
                        else  if(countPartNo == 1)
                        {
                            OracleConnection oraConn = new OracleConnection();
                            string connString = "User Id=ADMIN; Password=demo; Data Source=fnsystem:1521/live";
                            oraConn.ConnectionString = connString;
                            oraConn.Open();
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = oraConn;
                            OracleTransaction myTrans = oraConn.BeginTransaction();
                            cmd.Transaction = myTrans;

                            try
                            {
                                string sqlDelete = "delete from T_BARCODE_FIXPROD_ID where CUST_PART_FIX ='" + txtPartNo.Text + "'";
                                cmd.CommandText = sqlDelete;
                                cmd.ExecuteNonQuery();
                                string sqlInsert = "insert into T_BARCODE_FIXPROD_ID (cust_part_fix) values('" + txtPartNo.Text + "')";
                                cmd.CommandText = sqlInsert;
                                cmd.ExecuteNonQuery();
                                myTrans.Commit();
                                cmd.Dispose();
                                button1.Enabled = true;
                                txtPartNo.Text = null;
                                MessageBox.Show("Thành Công!");
                            }
                            catch
                            {
                                myTrans.Rollback();
                                MessageBox.Show("Thất Bại!");
                            }
                            finally
                            {
                                oraConn.Close();
                            }
                        }
                        else
                        {
                            OracleConnection oraConn = new OracleConnection();
                            string connString = "User Id=ADMIN; Password=demo; Data Source=fnsystem:1521/live";
                            oraConn.ConnectionString = connString;
                            oraConn.Open();
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = oraConn;
                            OracleTransaction myTrans = oraConn.BeginTransaction();
                            cmd.Transaction = myTrans;
                            try
                            {
                                for (int i = 0; i < countPartNo; i++)
                                {
                                    string sqlDelete = "delete from T_BARCODE_FIXPROD_ID where CUST_PART_FIX ='" + txtPartNo.Text + "'";
                                    cmd.CommandText = sqlDelete;
                                    cmd.ExecuteNonQuery();
                                }
                                string sqlInsert = "insert into T_BARCODE_FIXPROD_ID (cust_part_fix) values('" + txtPartNo.Text + "')";
                                cmd.CommandText = sqlInsert;
                                cmd.ExecuteNonQuery();
                                myTrans.Commit();
                                cmd.Dispose();
                                button1.Enabled = true;
                                txtPartNo.Text = null;
                                MessageBox.Show("Thành Công!");
                            }
                            catch
                            {
                                myTrans.Rollback();
                                MessageBox.Show("Thất Bại!");
                            }
                            finally
                            {
                                oraConn.Close();
                            }


                        }
                    }else
                    {
                        button1.Enabled = true;
                        txtPartNo.Text = null;
                        MessageBox.Show("Mã hàng không tồn tại!");
                    }
                }
            }
            else
            {
                txtPartNo.Text = null;
                button1.Enabled = true;
                MessageBox.Show("Nhập tối thiểu 9 ký tự!");
            }
        }
    }
}
