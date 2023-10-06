﻿using System;
using System.Data;
using System.Data.Odbc;
using System.IO;

namespace CapaModelo
    /* AUTORES:
        LUIS ALBERTO FRANCO MORAN 0901-20-23979
        OTTO ADRIAN LOPEZ VENTURA 0901-20-1069  */
{
    public class Sentencias
    {
        private Conexion con;

        public Sentencias()
        {
            con = new Conexion();
        }

        // Insertar un nuevo registro con archivo de texto
        public void InsertarReporte(string correlativo,string nombreArchivo, string estado, string rutaArchivo)
        {
            using (OdbcConnection connection = con.AbrirConexion())
            {
                if (connection != null)
                {
                    using (OdbcTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string insertQuery = "INSERT INTO tbl_reportes (nbr_correlativo, nbr_nombre, fk_estado, nbr_fecha, nbr_archivo) VALUES (?, ?, ?, ?, ?)";
                            using (OdbcCommand cmd = new OdbcCommand(insertQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@nbr_correlativo", correlativo);
                                cmd.Parameters.AddWithValue("@nbr_nombre", nombreArchivo);
                                cmd.Parameters.AddWithValue("@fk_estado", estado);
                                cmd.Parameters.AddWithValue("@nbr_fecha", DateTime.Now);
                                cmd.Parameters.AddWithValue("@nbr_archivo", rutaArchivo);

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error al insertar el registro: {ex.Message}");
                        }
                    }
                }
            }
        }
        

        // Leer registros existentes
        public DataTable ObtenerReportes()
        {
            using (OdbcConnection connection = con.AbrirConexion())
            {
                if (connection != null)
                {
                    string selectQuery = "SELECT pk_id_reporte, nbr_correlativo, nbr_nombre, fk_estado, nbr_archivo ,nbr_fecha, nbr_fechaMod FROM reportes";
                    using (OdbcCommand cmd = new OdbcCommand(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        using (OdbcDataAdapter adapter = new OdbcDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        // Actualizar un registro existente
        public void ActualizarReporte(int idReporte,string nombre, string estado)
        {
    using (OdbcConnection connection = con.AbrirConexion())
    {
        if (connection != null)
        {
            using (OdbcTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    string updateQuery = "UPDATE tbl_reportes SET nbr_nombre=?, fk_estado=?, nbr_fechaMod=? WHERE pk_id_reporte=?";
                    using (OdbcCommand cmd = new OdbcCommand(updateQuery, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@nbr_nombre", nombre);
                        cmd.Parameters.AddWithValue("@fk_estado", estado);
                        cmd.Parameters.AddWithValue("@nbr_fechaMod", DateTime.Now);
                        cmd.Parameters.AddWithValue("@pk_id_reporte", idReporte);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error al actualizar el registro: {ex.Message}");
                }
            }
        }
    }
        }

        // Eliminar un registro existente
        public void EliminarReporte(int idReporte)
        {
            using (OdbcConnection connection = con.AbrirConexion())
            {
                if (connection != null)
                {
                    using (OdbcTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string deleteQuery = "DELETE FROM tbl_reportes WHERE pk_id_reporte=?";
                            using (OdbcCommand cmd = new OdbcCommand(deleteQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@pk_id_reporte", idReporte);

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error al eliminar el registro: {ex.Message}");
                        }
                    }
                }
            }
        }

        // El método llenarTbl que mencionaste previamente
        public DataTable llenarTbl(string tabla)
        {
            using (OdbcConnection connection = con.AbrirConexion())
            {
                if (connection != null)
                {
                    string sql = "SELECT pk_id_reporte, nbr_correlativo, nbr_nombre, fk_estado, nbr_archivo ,nbr_fecha, nbr_fechaMod FROM  " + tabla + ";";
                    OdbcDataAdapter dataTable = new OdbcDataAdapter(sql, connection);
                    DataTable table = new DataTable();
                    dataTable.Fill(table);
                    return table;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}


    