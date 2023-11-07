using System;
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
    public partial class frm_mantenimiento_deducciones_percepciones : Form
    {
        Seguridad_Controlador.Controlador cn = new Seguridad_Controlador.Controlador();
        Controlador_PrototipoMenu.Controlador ctrl = new Controlador_PrototipoMenu.Controlador();
        public frm_mantenimiento_deducciones_percepciones()
        {
            InitializeComponent();
        }

        private void navegador1_Load(object sender, EventArgs e)
        {
            NavegadorVista.Navegador.idApp = "6003";  //código aplicación asignado al formulario
            navegador1.actual = this;
            navegador1.tabla = dataGridView1;
            TextBox[] Grupotextbox = { txt_id_dedu_perc, txt_dedperc_nombre, txt_dedperc_monto, txt_dedperc_movimiento};
            TextBox[] Idtextbox = { txt_id_dedu_perc, txt_dedperc_nombre };
            navegador1.textbox = Grupotextbox;
            navegador1.textboxi = Idtextbox;
            navegador1.cargar(dataGridView1, Grupotextbox, cn.getNombreBd());
        }
    }
}
