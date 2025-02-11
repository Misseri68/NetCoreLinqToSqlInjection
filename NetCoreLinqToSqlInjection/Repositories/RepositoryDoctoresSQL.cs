using System.Data;
using Microsoft.Data.SqlClient;
using NetCoreLinqToSqlInjection.Models;

namespace NetCoreLinqToSqlInjection.Repositories
{

    #region sp
    /*
     create procedure SP_DELETE_DOCTOR
	(@iddoctor int)
	as
		delete from DOCTOR where DOCTOR_NO=@iddoctor
	go*/

    #endregion

    public class RepositoryDoctoresSQL :IRepositoryDoctores
    {
        private DataTable tableDoctores;
        private SqlConnection cn;
        SqlCommand com;

        public RepositoryDoctoresSQL() 
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Trust Server Certificate=True";
            this.tableDoctores = new DataTable();
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            string query = "Select * from DOCTOR";
            this.tableDoctores = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            adapter.Fill(tableDoctores);
        }

        public List<Doctor> GetDoctores()
        {
            List<Doctor> doctores = new List<Doctor>();
            var consulta = from datos in tableDoctores.AsEnumerable()
                           select datos;
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor();
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
                doctores.Add(doc);
            }
            return doctores;
        }

        public void InsertDoctor
            (int idDoctor, string apellido, string especialidad
            , int salario, int idHospital)
        {
            string sql = "insert into DOCTOR values (@idhospital, @iddoctor "
            + ", @apellido, @especialidad, @salario)";
            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.Parameters.AddWithValue("@idhospital", idHospital);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        
        }

        public void DeleteDoctor(int idDoctor)
        {
            string sql = "sp_delete_doctor";
            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Doctor FindDoctor(int idDoctor)
        {
            Doctor doc = new Doctor();
            var consulta = from datos in tableDoctores.AsEnumerable()
                           where datos.Field<int>("DOCTOR_NO") == idDoctor
                           select datos;
            if (consulta.Count() > 0) {
                var row = consulta.First();
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
            }
            return doc;
        }

        public void UpdateDoctor(int idDoctor, string apellido, string especialidad,
          int salario, int idHospital)
        {
            string sql = "update DOCTOR set APELLIDO=@apellido," +
                "ESPECIALIDAD=@especialidad, SALARIO=@salario, " +
                "HOSPITAL_COD=@idHospital where DOCTOR_NO = @idDoctor";

            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

    }
}
