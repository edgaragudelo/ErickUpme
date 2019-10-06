using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpmeSubasta2019.Reportes
{
    public class ValidarRegistrosCargados
    {

        public static bool ValidarCargas(int NroRegsPost, int NroRegsSql,string ProcesoCarga)
        {
            //  Proceso para validar los registros cargados desde pstgress a sqlserver en todos los procesos de carga en ambos sobres 1 y 2
            bool Resultado = false;
            if (NroRegsPost !=NroRegsSql)
            {
                Resultado = false;
                MessageBox.Show(ProcesoCarga, "Hay diferencias entre los registros leidos y los cargados");
            }
            else
            {
                Resultado = true;
            }
            return Resultado;
        }
}
}
