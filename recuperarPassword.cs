using MySql.Data.MySqlClient;
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
using System.Runtime.InteropServices;

namespace TPProgramacionA
{
    public partial class recuperarPassword : Form
    {
        public recuperarPassword()
        {
            InitializeComponent();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hand, int wmsg, int wparam, int lparam);


        string cadenaConexion = "server=localhost;user=root;pwd='';database=programacion";
        private void BTNAceptar_Click(object sender, EventArgs e)
        {
            DialogResult btnApretado = MessageBox.Show("¿Está seguro que quiere recuperar contraseña?", "Recuperar Contraseña", MessageBoxButtons.OKCancel);
            if (btnApretado == DialogResult.Cancel)
            {
                //
            }
            else
            {

                recuperarPass();
            }

          
          
        }

        public void recuperarPass() {

            //lberror.Text = "";
            string cambiarPassword = @"UPDATE Usuario 
                                            SET password = @password
                                            WHERE mail = @mail";
            //Conectamos 
            MySqlConnection conectar = new MySqlConnection(cadenaConexion);
            //Creamos objeto para realizar consulta a la BDD
            conectar.Open();

            MySqlCommand SqlConsulta = new MySqlCommand();
            SqlConsulta.Connection = conectar;
            SqlConsulta.CommandText = ("select * from usuario where mail= '" + TXT1.Text + "' and activo = '1'");

            MySqlDataReader leer = SqlConsulta.ExecuteReader();
            if (leer.Read()){
                conectar.Close();
                // Si existe el usuario, le cambio la pw a contraseñaAleatorio
                //creo objeto para realizar un núm aleatorio
                Random contraseñaAleatorio = new Random();
                //Genero número random de 6 digitos.
                int nRandom = contraseñaAleatorio.Next(100000, 999999);
                //encripto la contraseña nueva realizada por el método .Next
                string nuevaPassword = HashCode.CodificacionPassword(nRandom.ToString());
                MySqlCommand cmdActualizar = new MySqlCommand(cambiarPassword, conectar);
                //cambio pw por la  encriptada
                cmdActualizar.Parameters.AddWithValue("password", nuevaPassword);
                cmdActualizar.Parameters.AddWithValue("mail", TXT1.Text);

                conectar.Open();
                cmdActualizar.ExecuteNonQuery();
                conectar.Close();
                MessageBox.Show("Contraseña modificada, se le enviará a su casilla de correo la nueva contraseña");
                ClasesModuloUsuarios.EnvioMail.EnvioDeMail(TXT1.Text,"Contraseña restablecida", "Su contraseña en UmbrellaCorp fue reestablecida. " +
                "Su nueva contraseña es: " + nRandom + " .Ingrese a la página para modificarla.");
               
            }else{
                MessageBox.Show("Mail Erroneo");
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult btnApretado = MessageBox.Show("¿Está seguro que quiere volver a la página de login?", "Volver Página Inicial", MessageBoxButtons.OKCancel);
            if (btnApretado == DialogResult.Cancel)
            {
                //
            }
            else
            {

                this.Hide();
                Login OBJMenuGestionRecurso = new Login();
                //OBJMenuGestionRecurso.LBUsuario.Text = LBUsuario.Text;
                OBJMenuGestionRecurso.FormClosed += (s, args) => this.Close();
                OBJMenuGestionRecurso.Show();
            }

           
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
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

        private void recuperarPassword_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}