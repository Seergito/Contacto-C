
using EjemploConexiónBDAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace AgendaContactos
{
   
    public partial class MainWindow : Window
    {

        string estado;
        int posicion;
        BDMySql bd; //DELCARAR BD
        DataTable tabla;
        string SetenciaSelect = "SELECT * FROM `contacto` order by id";





        public MainWindow()
        {
            InitializeComponent();
            
           
            estado = "";
            posicion = 0;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            txtbox_observaciones.AcceptsReturn = true;
            txtbox_observaciones.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ajustaEstado("Sin Datos"); // La lista no se guarda, por lo que siempre se iniciará la aplicación sin datos
            
            bd = BDMySql.getInstance(); //CREAR OBJETO BD
            bd.AbrirConexion();

            tabla=bd.LanzaSelect(SetenciaSelect,true); //TRUE; REALIZAR CAMBIOS 



        }


        public void ElminarContacto(int pos)
        {
            tabla.Rows[pos].Delete();
            bd.ActualizaDatosTabla(SetenciaSelect, tabla);
        }

        public void ActualizarContacto(int pos, Contacto c)
        {
            tabla.Rows[pos]["nombre"] = c.Nombre;
            tabla.Rows[pos]["apellidos"] = c.Apellidos;
            tabla.Rows[pos]["telefono"] = c.Telefono;
            tabla.Rows[pos]["observaciones"] = c.Observaciones;
            bd.ActualizaDatosTabla(SetenciaSelect, tabla);
            MessageBox.Show("Contacto actualizado");
        }

        public void Añadir_contacto(Contacto c)
        {
            DataRow fila = tabla.NewRow();
            fila["nombre"] = c.Nombre;
            fila["apellidos"] = c.Apellidos;
            fila["telefono"] = c.Telefono;
            fila["observaciones"] = c.Observaciones;
            tabla.Rows.Add(fila);
            bd.ActualizaDatosTabla(SetenciaSelect, tabla);
            MessageBox.Show("Contacto añadido");

        }

        public Contacto ObtenerContacto(int pos)
        {
            int id = Int32.Parse(tabla.Rows[pos]["id"].ToString());
            string nombre = tabla.Rows[pos]["nombre"].ToString();
            string apellidos = tabla.Rows[pos]["apellidos"].ToString();
            string telefono = tabla.Rows[pos]["telefono"].ToString();
            string observaciones = tabla.Rows[pos]["observaciones"].ToString();
            return new Contacto(id, nombre, apellidos, telefono, observaciones);
        }




        private void limpiar()
        {
            txtbox_nombre.Text = "";
            txtbox_apellidos.Text = "";
            txtbox_telefono.Text = "";
            txtbox_observaciones.Text = "";
            lbb_posicion.Text = "";
        }

        public void visualizaDatos(int pos)
        {

            posicion = pos; // Actualizamos la posición actual
            limpiar();
            lbb_posicion.Text = (posicion + 1) + " de " + tabla.Rows.Count ;

            Contacto c = ObtenerContacto(pos);
            txtbox_nombre.Text = c.Nombre;
            txtbox_apellidos.Text = c.Apellidos;
            txtbox_telefono.Text = c.Telefono;
            txtbox_observaciones.Text = c.Observaciones;
        }

        public void ajustaEstado(string est)
        {
            estado = est; // Actualizamos el estado actual del formulario
            lbb_estado.Text = estado;
            switch (estado)
            {
                case "Sin Datos":
                    limpiar();
                    txtbox_nombre.IsEnabled = false;
                    txtbox_apellidos.IsEnabled = false;
                    txtbox_telefono.IsEnabled = false;
                    txtbox_observaciones.IsEnabled = false;
                    btn_grabar.IsEnabled = false;
                    btn_cancelar.IsEnabled = false;
                    btn_avanzar.IsEnabled = false;
                    btn_editar.IsEnabled = false;
                    btn_eliminar.IsEnabled = false;
                    btn_primero.IsEnabled = false;
                    btn_retroceder.IsEnabled = false;
                    btn_ultimo.IsEnabled = false;
                    btn_agregar.IsEnabled = true;
                    break;
                case "Nuevo":
                    limpiar();
                    txtbox_nombre.IsEnabled = true;
                    txtbox_apellidos.IsEnabled = true;
                    txtbox_telefono.IsEnabled = true;
                    txtbox_observaciones.IsEnabled = true;
                    btn_grabar.IsEnabled = true;
                    btn_cancelar.IsEnabled = true;
                    btn_avanzar.IsEnabled = false;
                    btn_editar.IsEnabled = false;
                    btn_eliminar.IsEnabled = false;
                    btn_primero.IsEnabled = false;
                    btn_retroceder.IsEnabled = false;
                    btn_ultimo.IsEnabled = false;
                    btn_agregar.IsEnabled = false;
                    break;

                case "Listando":
                    txtbox_nombre.IsEnabled = false;
                    txtbox_apellidos.IsEnabled = false;
                    txtbox_telefono.IsEnabled = false;
                    txtbox_observaciones.IsEnabled = false;
                    btn_grabar.IsEnabled = false;
                    btn_cancelar.IsEnabled = false;
                    btn_avanzar.IsEnabled = true;
                    btn_editar.IsEnabled = true;
                    btn_eliminar.IsEnabled = true;
                    btn_primero.IsEnabled = true;
                    btn_retroceder.IsEnabled = true;
                    btn_ultimo.IsEnabled = true;
                    btn_agregar.IsEnabled = true;
                    break;

                case "Editando":
                    txtbox_nombre.IsEnabled = true;
                    txtbox_apellidos.IsEnabled = true;
                    txtbox_telefono.IsEnabled = true;
                    txtbox_observaciones.IsEnabled = true;
                    btn_grabar.IsEnabled = true;
                    btn_cancelar.IsEnabled = true;
                    btn_avanzar.IsEnabled = false;
                    btn_editar.IsEnabled = false;
                    btn_eliminar.IsEnabled = false;
                    btn_primero.IsEnabled = false;
                    btn_retroceder.IsEnabled = false;
                    btn_ultimo.IsEnabled = false;
                    btn_agregar.IsEnabled = false;
                    break;
      
            }
        }



        private void btn_primero_Click(object sender, RoutedEventArgs e)
        {
            if (posicion!=0)
                visualizaDatos(0);
            else
                MessageBox.Show("Está en la primera posición","Aviso");
        }

        private void btn_avanzar_Click(object sender, RoutedEventArgs e)
        {
            if (posicion < tabla.Rows.Count-1)
                visualizaDatos(posicion+1);
            else
                MessageBox.Show("Está en la última posición", "Aviso");
        }

        private void btn_retroceder_Click(object sender, RoutedEventArgs e)
        {
           
            if (posicion > 0)
                visualizaDatos(posicion - 1);
            else
                MessageBox.Show("Está en la primera posición","Aviso");
            
        }

        private void btn_ultimo_Click(object sender, RoutedEventArgs e)
        {
             if (posicion!=tabla.Rows.Count-1)
                visualizaDatos(tabla.Rows.Count - 1);
             else
                MessageBox.Show("Está en la última posición","Aviso");

        }

        private void btn_eliminar_Click(object sender, RoutedEventArgs e)
        {

            ElminarContacto(posicion);
            

            if (posicion > tabla.Rows.Count - 1) posicion= tabla.Rows.Count - 1;
            
            if(tabla.Rows.Count > 0)
            { 
                 visualizaDatos(posicion);
            }
            else
            {
                posicion = -1;
                ajustaEstado("Sin Datos");
            }
        }

        private void btn_agregar_Click(object sender, RoutedEventArgs e)
        {
           
            ajustaEstado("Nuevo");

        }

      

        private void btn_grabar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtbox_nombre.Text;
            string apellidos = txtbox_apellidos.Text;
            string telefono = txtbox_telefono.Text; 
            string observacion = txtbox_observaciones.Text;

            if (estado.Equals("Nuevo")) // valdría estado == "Nuevo"
            {
                if (nombre.Length > 0)
                {
                    Contacto c = new Contacto(-1,nombre, apellidos, telefono, observacion);
              
                    ajustaEstado("Listando");
                    Añadir_contacto(c);
                    visualizaDatos(tabla.Rows.Count -1);
                }
                else
                    MessageBox.Show("Teclee al menos el nombre del contacto","Aviso");

            }
            else if (estado.Equals("Editando"))
            {
                
                Contacto c = new Contacto(-1,nombre, apellidos, telefono, observacion);
                ActualizarContacto(posicion, c);
                visualizaDatos(posicion);
                ajustaEstado("Listando");

                
            }
        }

        private void btn_editar_click(object sender, RoutedEventArgs e)
        {
            ajustaEstado("Editando");
 
        }

        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (posicion < 0)
            {
                ajustaEstado("Sin Datos");
            }
            else
            {
                ajustaEstado("Listando");
                visualizaDatos(posicion); // Se vuelven a visualizar los datos originales
            }
        }
    }
}
