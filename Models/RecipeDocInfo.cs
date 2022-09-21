using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerator.Models
{
    public class RecipeDocInfo
    {
        public int Id { get; set; }

        public string HospitalName { get; set; }
        public string DepartmentName { get; set; }
        public string ClientName { get; set; }
        public string ClientAge { get; set; }
        public string ClientSex { get; set; }

        public string Rps { get; set; }
        public List<string> RpList { get; set; }


        public string DraftEmployeeName { get; set; }


        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public string AuditEmployeeName { get; set; }

        public string DraftEmployeeSignature { get; set; }

        public string AuditEmployeeSignature { get; set; }

        public string StartTimeString { get; set; }

    }
}
