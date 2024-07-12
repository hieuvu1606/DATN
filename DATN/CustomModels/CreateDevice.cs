using DATN.Models;

namespace DATN.CustomModels
{
    public class CreateDevice
    {
        public int CategoryId { get; set; }

        public string Descr { get; set; } = null!;

        public string ShortDescr { get; set; } = null!;

        public string? Image { get; set; }

        public string? DescrFunction { get; set; }

        public string? Pdf { get; set; }
    }
}
