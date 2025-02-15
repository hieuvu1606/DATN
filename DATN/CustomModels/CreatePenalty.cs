﻿using DATN.Models;

namespace DATN.CustomModels
{
    public class CreatePenalty
    {
        public int LineRef { get; set; }

        public string Descr { get; set; } = null!;

        public int Fine { get; set; }
    }

    public class PostPenalty
    {
        public int PenaltyID { get; set; }
        public int RegistID { get; set; }
        public int ManagerID { get; set; }
        public List<CreatePenalty> ListPenalty { get; set; }
    }
}
