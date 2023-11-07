using System;
using Controlador_PrototipoMenu;
using Modelo_PrototipoMenu;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista_PrototipoMenu
{
    public partial class frm_nomina : Form
    {
        Controlador ctrl = new Controlador();
        string tabla = "tbl_nomina";
        public frm_nomina()
        {
            InitializeComponent();
            llenar();
        }

        public void llenar()
        {
            DataTable dt = ctrl.llenartabla(tabla);
            dataGridView1.DataSource = dt;

        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(txt_id_empleado.Text, out int idEmpleado))
                {
                    // Obtén los datos del empleado
                    Empleado empleadoEncontrado = ctrl.BuscarEmpleadoPorID(idEmpleado);
                    string departamentoEncontrado = ctrl.ObtenerDepartamento(idEmpleado);

                    if (empleadoEncontrado != null)
                    {
                        // Llena los campos con los datos del empleado
                        foreach (Control control in groupBox3.Controls)
                        {
                            if (control is TextBox)
                            {
                                TextBox textBox = (TextBox)control;
                                if (textBox.Tag != null && textBox.Tag.ToString() != "")
                                {
                                    string campo = textBox.Tag.ToString();
                                    if (campo == "emp_nombre")
                                    {
                                        textBox.Text = empleadoEncontrado.Nombre;
                                    }
                                    else if (campo == "emp_apellido")
                                    {
                                        textBox.Text = empleadoEncontrado.Apellido;
                                    }
                                    else if (campo == "emp_sueldo")
                                    {
                                        textBox.Text = empleadoEncontrado.Sueldo;
                                    }
                                    else if (campo == "emp_cargo")
                                    {
                                        textBox.Text = empleadoEncontrado.Cargo;
                                    }
                                    else if (campo == "emp_tipo_moneda")
                                    {
                                        textBox.Text = empleadoEncontrado.Moneda;
                                    }
                                    else if (campo == "tbl_departamento_Pk_id_departamento")
                                    {
                                        textBox.Text = departamentoEncontrado;
                                    }
                                }
                            }
                        }

                     
                        string deduccionTotalEncontrada = ctrl.ObtenerDeduccionTotal(idEmpleado);
                        txt_totalDeducciones.Text = deduccionTotalEncontrada;
                        //MessageBox.Show("Valor de deduccionTotalEncontrada: " + deduccionTotalEncontrada);
                        string percepcionTotalEncontrada = ctrl.ObtenerPercepcionTotal(idEmpleado);
                        txt_totalPercepciones.Text = percepcionTotalEncontrada;
                        //MessageBox.Show("Valor de percepcion: " + percepcionTotalEncontrada);
                        if (double.TryParse(empleadoEncontrado.Sueldo, out double sueldoBase) &&
                            double.TryParse(percepcionTotalEncontrada, out double totalPercepciones) &&
                            double.TryParse(deduccionTotalEncontrada, out double totalDeducciones)
                            )
                        {
                            double sueldoFinal = sueldoBase + totalPercepciones - totalDeducciones;
                            txt_sueldoFinal.Text = sueldoFinal.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Faltan datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            
                        }



                    }
                    else
                    {
                        MessageBox.Show("Empleado no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("ID de empleado no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_guardar_nomina_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txt_id_empleado.Text, out int idEmpleado) &&
                double.TryParse(txt_totalDeducciones.Text, out double totalDeducciones) &&
                double.TryParse(txt_sueldoB_empleado.Text, out double sueldoBase) &&
                double.TryParse(txt_totalPercepciones.Text, out double totalPercepciones) &&
                double.TryParse(txt_sueldoFinal.Text, out double sueldoFinal)
                )
            {

                string tipoMoneda = txt_emp_tipo_moneda.Text;
                ctrl.InsertarNomina(idEmpleado, tipoMoneda, sueldoBase.ToString(), totalPercepciones.ToString(), totalDeducciones.ToString(), sueldoFinal.ToString()); // Convierte sueldoFinal a cadena
                MessageBox.Show("Nómina guardada ");
            }
            else
            {
                MessageBox.Show("Datos Incorrectos");
            }
            llenar();
        }
    }
}
