﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetApp.Codigo
{
    internal class ParametrosOUTPUT
    {
        public async Task CodigoDelCurso(string[] args)
        {

            using IHost host = Host.CreateDefaultBuilder(args).Build();

            var configuración = host.Services.GetService<IConfiguration>();

            var cadenaConexión = configuración.GetConnectionString("cadenaDeConexion");

            Console.Write("Escriba el nombre de la persona a ingresar a la BD: ");
            var nombre = Console.ReadLine();

            try
            {
                using (SqlConnection conexión = new SqlConnection(cadenaConexión))
                {
                    // Abrimos la conexión
                    conexión.Open();
                    Console.WriteLine("Conexión abierta");
                    var query = "Personas_Insertar";

                    using (SqlCommand comando = new SqlCommand(query, conexión))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", nombre));

                        var parametroId = new SqlParameter
                        {
                            ParameterName = "@id",
                            Direction = ParameterDirection.Output,
                            DbType = DbType.Int32
                        };

                        comando.Parameters.Add(parametroId);

                        await comando.ExecuteNonQueryAsync();

                        var id = (int)parametroId.Value;
                        Console.WriteLine("Id de la personas: " + id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No pudimos abrir la conexión");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Fin");
        }
    }
}
