﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace CapaModelo
{
    public class Sentencias
    {
        Conexion con = new Conexion();


        //carlos enrique
        public DataTable Buscar(string tabla, string columna, string dato)
        {
            string consulta = $"SELECT * FROM {tabla} WHERE {columna} = '{dato}'";
            OdbcDataAdapter datos = new OdbcDataAdapter(consulta, con.conexion());

            DataTable dt = new DataTable();
            datos.Fill(dt);

            return dt;
        }


        //carlos enrique
        public bool Eliminar(string tabla, string columna, string valor)
        {
            using (OdbcConnection conn = con.conexion())
            {
                string consulta = $"DELETE FROM {tabla} WHERE {columna} = ?";
                using (OdbcCommand cmd = new OdbcCommand(consulta, conn))
                {
                    cmd.Parameters.AddWithValue("valor", valor);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        public bool Guardar(string tabla, Dictionary<string, object> valores)
        {
            using (OdbcConnection conn = con.conexion())
            {
                // Construir la consulta SQL para insertar datos
                string columnas = string.Join(", ", valores.Keys);
                string parametros = string.Join(", ", valores.Keys.Select(key => "?"));
                string consulta = $"INSERT INTO {tabla} ({columnas}) VALUES ({parametros})";

                using (OdbcCommand cmd = new OdbcCommand(consulta, conn))
                {
                    // Agregar parámetros con sus valores correspondientes
                    foreach (var kvp in valores)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }




        //FUNCIÓN PARA MODIFICAR
        //Justin Pennant
        // basado en Guardar
        public bool Modificar(string tabla, Dictionary<string, object> valores, string condicion)
        {
            using (OdbcConnection conn = con.conexion())
            {
                // Construir la consulta SQL para actualizar datos
                string setClause = string.Join(", ", valores.Keys.Select(key => $"{key} = ?"));
                string consulta = $"UPDATE {tabla} SET {setClause} WHERE {condicion}";

                using (OdbcCommand cmd = new OdbcCommand(consulta, conn))
                {
                    // Agregar parámetros con sus valores correspondientes
                    foreach (var kvp in valores)
                    {
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        /*//carlos enrique
        public OdbcDataAdapter llenarTblBitacora(string tabla)// metodo  que obtinene el contenio de una tabla
        {
            //consultamos
            string sql = "SELECT * FROM " + tabla + "  ;";
            OdbcDataAdapter dataTable = new OdbcDataAdapter(sql, con.conexion());
            return dataTable;
        }*/

        public string GetIpPublica()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.ipify.org/");
            request.UserAgent = "curl"; string publicIPAddress; request.Method = "GET";
            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    publicIPAddress = reader.ReadToEnd();
                }
            }
            return publicIPAddress.Replace("\n", "");
        }

        public string ObtenerHoraSistema()
        {
            DateTime horaSistema = DateTime.Now;
            return horaSistema.ToString("HH:mm:ss");
        }

        public string ObtenerFechaSistema()
        {
            DateTime fechaSistema = DateTime.Now;
            return fechaSistema.ToString("dd-MM-yyyy");
        }

        public string ObtenerNombreHost()
        {
            string nombreHost = Dns.GetHostName();
            return nombreHost;
        }

        //reutilizar el metodo OdbcDataAdapter llenarTblBitacora para obtener la informacion de usuario



        //--------------------------------


        //MODULO DE ENCRIPTACION SHA256| Jonathan Arriaga
        public string Encriptacion(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        //MODULO PARA LLENADO DE COMBOBOX Jonathan Arriaga
        public List<string> ObtenerDatos(string columna, string tabla)
        {
            List<string> datos = new List<string>();
            try
            {

                string consulta = $"SELECT {columna} FROM {tabla}";

                OdbcCommand command = new OdbcCommand(consulta, con.conexion());
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string nombre = reader[columna].ToString();
                    datos.Add(nombre);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return datos;
        }
    }
}
