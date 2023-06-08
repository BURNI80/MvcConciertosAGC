using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcConciertosAGC.Models
{
    [Table("categoriaevento")]
    public class Categoria
    {
        [Key]
        [Column("idcategoria")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

    }
}
