using System.ComponentModel.DataAnnotations;

namespace AGPU_WEB.Models.DTO
{
    public class AGPUCreateDTO
    {

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(15)]
        public string? Brand { get; set; }

        [MaxLength(20)]
        public string? Model { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? Price { get; set; }
        public double? ValuePercentage { get; set; }
        public double? AverageBenchPercentage { get; set; }

        public List<string>? Images { get; set; }
    }
}
