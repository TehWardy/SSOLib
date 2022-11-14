using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Objects.Entities
{
    public class Token
    {
        public string Id { get; set; }

        public int Reason { get; set; }

        public DateTimeOffset Expires { get; set; }

        public string UserName { get; set; }

        public virtual SSOUser User { get; set; }
    }
}