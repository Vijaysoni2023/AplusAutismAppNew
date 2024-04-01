using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Data.Models
{
    public class ExceptionMessageTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public string statuscode { get; set; }

        public string ExceptionMessage { get; set; }

        public string Message { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
