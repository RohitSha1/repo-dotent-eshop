using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Net;
public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnResetPass_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyShopDB"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from tblUsers where Email=@Email", con);
            cmd.Parameters.AddWithValue("@Email", txtEmailID.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                String myGUID = Guid.NewGuid().ToString();
                int Uid = Convert.ToInt32(dt.Rows[0][0]);

                SqlCommand cmd1 = new SqlCommand("Insert into ForgotPass(Id,Uid,RequestDateTime) values('" + myGUID + "','" + Uid + "',GETDATE())", con);
                cmd1.ExecuteNonQuery();


                //Send Reset link via Email

                // String ToEmailAddress = dt.Rows[0][4].ToString();
                String Username = dt.Rows[0][1].ToString();
                //String EmailBody = "Hi ," + Username + ",<br/><br/>Click the link below to reset your password<br/> <br/> http://localhost:1288/RecoverPassword.aspx?id=" + myGUID;
                string frommail = "onlinecherry3@gmail.com";
                string frompassword = "wwztbtffahecaxhl";

                MailMessage PassRecMail = new MailMessage();
                PassRecMail.From = new MailAddress(frommail);
                PassRecMail.To.Add(new MailAddress("onlinecherry3@gmail.com"));
                PassRecMail.Body = "<html><body> test body </body><?html>";
                PassRecMail.IsBodyHtml = true;
                PassRecMail.Subject = "Reset Password";

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587))
                {


                    //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    //smtpClient.Credentials = new System.Net.NetworkCredential("kaushalyasharma017@gmail.com", "devendra", "smtp.gmail.com");
                    //smtpClient.EnableSsl = true;
                    //smtpClient.UseDefaultCredentials = false;

                    //smtpClient.Port = 587;
                    //smtpClient.Host = "smtp.gmail.com";
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    var SmtpClient = new SmtpClient("smtp.gmail.com");
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential(frommail, frompassword);
                        smtpClient.EnableSsl = true;
                    };
                    smtpClient.Send(PassRecMail);

                }

                //--------------


                lblResetPassMsg.Text = "Reset Link send ! Check YOur email for reset password";
                lblResetPassMsg.ForeColor = System.Drawing.Color.Green;
                txtEmailID.Text = string.Empty;
            }
            else
            {
                lblResetPassMsg.Text = "OOps! This Email Does not Exist...Try agian ";
                lblResetPassMsg.ForeColor = System.Drawing.Color.Red;
                txtEmailID.Text = string.Empty;
                txtEmailID.Focus();

            }




        }
    }
}