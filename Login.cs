using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPProgramacionA.ClasesModuloUsuarios;
using TPProgramacionA.GestionDeUsuarios;
using MySql.Data.MySqlClient;
using TPProgramacionA.clases;
using System.Runtime.InteropServices;


namespace TPProgramacionA
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hand, int wmsg, int wparam, int lparam);
        private void groupBox1_Enter(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            validarCampos();
            autentificarUsuario();

            

        }

        private void LinkOlvido_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            this.Hide();    
            recuperarPassword recuperarPassword = new recuperarPassword();           
            recuperarPassword.FormClosed += (s,args) => this.Close();
            recuperarPassword.Show();
            
        }
        private bool validarCampos()
        {
            //Creamos variable booleana. La iniciamos en Verdadero.
            bool error = true;


            //Si el texto está VACIO, ingresa al if.
            if (TXT1.Text == "")
            {
                //Error pasa a Falso y se SETEA el Texto del errorProvider.
                error = false;
                errorProvider1.SetError(TXT1, "Ingrese su usuario");
            }
            else
            {
                //Si el campo no está vacío, se vacia el texto que pueda contener el Error.
                //En caso de que haya quedado cargado anteriormente.
                errorProvider1.Clear();
            }

            if (TXT2.Text == "")
            {
                error = false;
                errorProvider1.SetError(TXT2, "Ingrese su contraseña");
            }
            else
            {
                errorProvider1.Clear();
            }

            return error;

        }       
        public void autentificarUsuario()
        {
            string cadenaConexion = "server=localhost;user=root;pwd='';database=programacion";
            string MysqlValidarUser = @"SELECT COUNT(*)
                                        FROM usuario WHERE username = @username
                                        AND  password = @password AND activo = 1";
            try{
                
                MySqlConnection conectar = new MySqlConnection(cadenaConexion);
                conectar.Open();
                MySqlCommand comandoValidar = new MySqlCommand(MysqlValidarUser, conectar);
                comandoValidar.Parameters.AddWithValue("@username", TXT1.Text);
                string hash = ClasesModuloUsuarios.HashCode.CodificacionPassword(TXT2.Text);
                comandoValidar.Parameters.AddWithValue("@password", hash);

                int count = Convert.ToInt32(comandoValidar.ExecuteScalar());
            if (count == 0){
                    MessageBox.Show("Usuario o contraseña erronea");

            }else{
                this.Hide(); // hago invisible el form actual
                menuUsuario OBJMenuUsuario = new menuUsuario();
                OBJMenuUsuario.FormClosed += (s, args) => this.Close(); // cuando cierro este form, cierro el programa.
                OBJMenuUsuario.LBUsuario.Text = TXT1.Text;//Le envío el valor del TXTUusuario al Label del otro form
                //Para que pueda recibirlo, tengo que poner público el Label del otro form.
                OBJMenuUsuario.Show(); //Con este método abro otro form                 
                }              
            }catch (Exception ex){
                MessageBox.Show(ex.Message + "error");}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //DialogResult btnApretado = MessageBox.Show("¿Está seguro que quiere cerrar el programa?", "Cerrar Programa", MessageBoxButtons.OKCancel, (MessageBoxIcon)MsgBox.Icon.Info, (MessageBoxDefaultButton)MsgBox.AnimateStyle.FadeIn);
            DialogResult btnApretado = MessageBox.Show("¿Está seguro que quiere cerrar el programa?", "Cerrar Programa", MessageBoxButtons.OKCancel);
            if (btnApretado == DialogResult.Cancel)
            {
                //
            }
            else
            {

                this.Close();
            }

           
        }

        private void TXT1_Enter(object sender, EventArgs e)
        {

        }

        private void TXT1_Leave(object sender, EventArgs e)
        {

        }

        private void TXT2_Enter(object sender, EventArgs e)
        {

        }

        private void TXT2_Leave(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
