﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2021_g01.Model {
    public class Answer {
        [Key]
        public int Id { get; set; }
        public virtual Question Question { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}
