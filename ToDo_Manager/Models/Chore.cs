using System;
using System.Collections.Generic;

namespace ToDo_Manager.Models
{
    public partial class Chore
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? CompletionStatus { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
