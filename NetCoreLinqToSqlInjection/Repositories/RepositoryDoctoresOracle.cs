using System.Data;
using NetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;

namespace NetCoreLinqToSqlInjection.Repositories
{

    #region SP

    /*
     
     create or replace procedure sp_delete_doctor
(p_iddoctor DOCTOR.DOCTOR_NO%TYPE)
as
begin
  delete from DOCTOR where DOCTOR_NO=p_iddoctor;
  commit;
end;
    */


    #endregion
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tableDoctores;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryDoctoresOracle()
        {
            string connectionString = "Data Source=LOCALHOST:1521/XE;Persist Security Info=True;User Id=system;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;

            this.tableDoctores = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(
                "select * from DOCTOR", connectionString);
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

        public void InsertDoctor(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "INSERT INTO DOCTOR (HOSPITAL_COD, DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO) " +
                         "VALUES (:idhospital, :iddoctor, :apellido, :especialidad, :salario)";

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.com.Parameters.Add(new OracleParameter(":idhospital", idHospital));
            this.com.Parameters.Add(new OracleParameter(":iddoctor", idDoctor));
            this.com.Parameters.Add(new OracleParameter(":apellido", apellido));
            this.com.Parameters.Add(new OracleParameter(":especialidad", especialidad));
            this.com.Parameters.Add(new OracleParameter(":salario", salario));

            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
        }

        public void DeleteDoctor(int idDoctor)
        {
            string sql = "sp_delete_doctor";
            this.com.Parameters.Add(new OracleParameter(":p_iddoctor", idDoctor));
            this.com.CommandText = sql;
            this.com.CommandType = CommandType.StoredProcedure;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }




        public void UpdateDoctor(int idDoctor, string apellido, string especialidad,
                int salario, int idHospital)
        {
        
            string sql = "update DOCTOR set APELLIDO = :apellido," +
                "ESPECIALIDAD = :especialidad, SALARIO = :salario, " +
                "HOSPITAL_COD = :idHospital where DOCTOR_NO = :idDoctor";

            this.com.Parameters.Add(new OracleParameter(":apellido", apellido));
            this.com.Parameters.Add(new OracleParameter(":especialidad", especialidad));
            this.com.Parameters.Add(new OracleParameter(":salario", salario));
            this.com.Parameters.Add(new OracleParameter(":idHospital", idHospital));
            this.com.Parameters.Add(new OracleParameter(":idDoctor", idDoctor));

            this.com.CommandText = sql;
            this.com.CommandType = CommandType.Text;
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
            if (consulta.Count() > 0)
            {
                var row = consulta.First();
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
            }
            return doc;
        }

       

    }
}
