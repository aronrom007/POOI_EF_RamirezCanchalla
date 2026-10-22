using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using POOI_EF_RamírezCanchalla.Models;

namespace POOI_EF_RamírezCanchalla.Controllers
{
    public class ConsumidorController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["Cadena_Conexion"].ConnectionString;
        private List<Consumidor> ListarConsumidores()
        {
            List<Consumidor> lista = new List<Consumidor>();
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_listar_consumidor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Consumidor(
                    dr.GetString(0),
                    dr.GetString(1),
                    dr.GetString(2),
                    dr.IsDBNull(3) ? "" : dr.GetString(3),
                    dr.IsDBNull(4) ? "" : dr.GetString(4)));
            }
            dr.Close();
            cn.Close();
            return lista;
        }
        private Consumidor ObtenerConsumidor(string dnicont)
        {
            Consumidor c = null;
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_obtener_consumidor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dnicont", dnicont);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                c = new Consumidor(
                    dr.GetString(0),
                    dr.GetString(1),
                    dr.GetString(2),
                    dr.IsDBNull(3) ? "" : dr.GetString(3),
                    dr.IsDBNull(4) ? "" : dr.GetString(4));
            }
            dr.Close();
            cn.Close();
            return c;

        }
        private string AgregarConsumidor(Consumidor c)
        {
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_agrega_consumidor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dnicont", c.dnicont);
            cmd.Parameters.AddWithValue("@nomcont", c.nomcont);
            cmd.Parameters.AddWithValue("@apecont", c.apecont);
            cmd.Parameters.AddWithValue("@emailcont",
                string.IsNullOrWhiteSpace(c.emailcont) ? (object)DBNull.Value : c.emailcont);
            cmd.Parameters.AddWithValue("@idpais",
                string.IsNullOrWhiteSpace(c.idpais) ? (object)DBNull.Value : c.idpais);

            SqlParameter pSalida = new SqlParameter("@resultado", SqlDbType.VarChar, 200);
            pSalida.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pSalida);

            cmd.ExecuteNonQuery();
            string mensaje = pSalida.Value.ToString();
            cn.Close();
            return mensaje;
        }
        private int ActualizarConsumidor(Consumidor c)
        {
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_actualizar_consumidor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dnicont", c.dnicont);
            cmd.Parameters.AddWithValue("@nomcont", c.nomcont);
            cmd.Parameters.AddWithValue("@apecont", c.apecont);
            cmd.Parameters.AddWithValue("@emailcont",
                string.IsNullOrWhiteSpace(c.emailcont) ? (object)DBNull.Value : c.emailcont);
            cmd.Parameters.AddWithValue("@idpais",
                string.IsNullOrWhiteSpace(c.idpais) ? (object)DBNull.Value : c.idpais);
            int filas = cmd.ExecuteNonQuery();
            cn.Close();
            return filas;
        }
        private int EliminarConsumidor(string dnicont)
        {
            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlCommand cmd = new SqlCommand("usp_eliminar_consumidor", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dnicont", dnicont);
            int filas = cmd.ExecuteNonQuery();
            cn.Close();
            return filas;
        }


        // GET: Consumidor
        public ActionResult Index()
        {
            List<Consumidor> lista = ListarConsumidores();
            return View(lista);
        }
        public ActionResult Create()
        {
            return View(new Consumidor());
        }
        [HttpPost]
        public ActionResult Create(Consumidor c)
        {
            string mensaje = AgregarConsumidor(c);
            if (mensaje == "Consumidor agregado correctamente.")
            {
                TempData["msgOK"] = mensaje;
                return RedirectToAction("Index");
            }
            ViewBag.Mensaje = mensaje;
            return View(c);
        }
        public ActionResult Details(string id)
        {
            return View(ObtenerConsumidor(id));
        }
        public ActionResult Edit(string id)
        {
            return View(ObtenerConsumidor(id));
        }

        [HttpPost]
        public ActionResult Edit(Consumidor c)
        {
            ActualizarConsumidor(c);
            TempData["msgOk"] = "Consumidor actualizado correctamente.";
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string id)
        {
            return View(ObtenerConsumidor(id));
        }

        [HttpPost]
        public ActionResult Delete(Consumidor c)
        {
            int filas = EliminarConsumidor(c.dnicont);
            TempData["msgOk"] = filas > 0
                ? "Consumidor eliminado correctamente."
                : "No se pudo eliminar: el DNI no existe.";
            return RedirectToAction("Index");
        }

    }
}