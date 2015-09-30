using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledgeBatch.Models
{
    public class BatchXMLResultModel
    {
        public string NumeroConsulta { get; set; }
        public string IdentificacionConsulta { get; set; }
        public string NombreConsulta { get; set; }
        public string IdGrupoLista { get; set; }
        public string NombreGrupoLista { get; set; }
        public string Prioridad { get; set; }
        public string TipoDocumento { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string NombreCompleto { get; set; }
        public string IdTipoLista { get; set; }
        public string NombreTipoLista { get; set; }
        public string Alias { get; set; }
        public string CargoDelito { get; set; }
        public string Peps { get; set; }
        public string Zona { get; set; }
        public string Link { get; set; }
        public string OtraInformacion { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaActualizacion { get; set; }
        public string Estado { get; set; }         
    }
}
