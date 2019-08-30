using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Domain.Entity
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [StringLength(64)]
        public string Application { get; set; }

        [StringLength(128)]
        public string Logged { get; set; }

        [StringLength(64)]
        public string Level { get; set; }

        [StringLength(512)]
        public string Message { get; set; }

        [StringLength(256)]
        public string Logger { get; set; }

        [StringLength(512)]
        public string CallSite { get; set; }

        [StringLength(512)]
        public string Exception { get; set; }
    }
}
