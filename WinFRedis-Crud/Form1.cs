using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFRedis_Crud
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        void Editar (bool valor)
        {
            textId.ReadOnly = valor;
            textNombre.ReadOnly = valor;
            textApellidos.ReadOnly = valor;
            textDni.ReadOnly = valor;
            textCargo.ReadOnly = valor;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            using (RedisClient cliente = new RedisClient("192.168.99.100", 6379))
            {
                IRedisTypedClient<Empleado> empleado = cliente.As<Empleado>();
                empleadoBindingSource.DataSource = empleado.GetAll();
                Editar(true);

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAnadir_Click(object sender, EventArgs e)
        {
            empleadoBindingSource.Add(new Empleado());
            empleadoBindingSource.MoveLast();
            Editar(false);
            textId.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar(false);
            textId.Focus();

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Editar(true);
            empleadoBindingSource.ResetBindings(false);
            textId.Text = string.Empty;
            textNombre.Text = string.Empty;
            textApellidos.Text = string.Empty;
            textDni.Text = string.Empty;
            textCargo.Text = string.Empty;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "Estas seguro de que vas a borrar el " +
                "registro!", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == 
                DialogResult.Yes)
            {
                Empleado em = empleadoBindingSource.Current as Empleado;
                if (em != null)
                {
                    using (RedisClient cliente = new RedisClient("192.168.99.100", 6379))
                    {
                        IRedisTypedClient<Empleado> empleado = cliente.As<Empleado>();
                        empleado.DeleteById(em.ID);
                        empleadoBindingSource.RemoveCurrent();
                        Limpiar();

                    }
                }
            }
        }
        void Limpiar()
        {
            textId.Text = string.Empty;
            textNombre.Text = string.Empty;
            textApellidos.Text = string.Empty;
            textDni.Text = string.Empty;
            textCargo.Text = string.Empty;
        }
            private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (RedisClient cliente = new RedisClient("192.168.99.100", 6379))
            {
                empleadoBindingSource.EndEdit();
                
                IRedisTypedClient<Empleado> empleado = cliente.As<Empleado>();
                empleado.StoreAll(empleadoBindingSource.DataSource as List<Empleado>);
                    MetroFramework.MetroMessageBox.Show(this, "Datos guardados.", "Mensaje", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }
    }
}
